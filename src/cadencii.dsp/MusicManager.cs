using System;
using System.IO;
using cadencii.core;

namespace cadencii
{
	public class MusicManager
	{
		private static string mFile = "";
		#if !TREECOM
        private static VsqFileEx mVsq;
#endif
		
        /// <summary>
        /// _vsq_fileにセットされたvsqファイルの名前を取得します。
        /// </summary>
        public static string getFileName()
        {
            return mFile;
        }

        public static void ProcessAutoBackup ()
		{
			if (!mFile.Equals("") && System.IO.File.Exists(mFile)) {
                string path = PortUtil.getDirectoryName(mFile);
                string backup = Path.Combine(path, "~$" + PortUtil.getFileName(mFile));
                string file2 = Path.Combine(path, PortUtil.getFileNameWithoutExtension(backup) + ".vsq");
                if (System.IO.File.Exists(backup)) {
                    try {
                        PortUtil.deleteFile(backup);
                    } catch (Exception ex) {
                        serr.println("AppManager::handleAutoBackupTimerTick; ex=" + ex);
                        Logger.write(typeof(MusicManager) + ".handleAutoBackupTimerTick; ex=" + ex + "\n");
                    }
                }
                if (System.IO.File.Exists(file2)) {
                    try {
                        PortUtil.deleteFile(file2);
                    } catch (Exception ex) {
                        serr.println("AppManager::handleAutoBackupTimerTick; ex=" + ex);
                        Logger.write(typeof(MusicManager) + ".handleAutoBackupTimerTick; ex=" + ex + "\n");
                    }
                }
                saveToCor(backup);
            }
		}
		
        private static void saveToCor(string file)
        {
            bool hide = false;
            if (mVsq != null) {
                string path = PortUtil.getDirectoryName(file);
                //String file2 = fsys.combine( path, PortUtil.getFileNameWithoutExtension( file ) + ".vsq" );
                mVsq.writeAsXml(file);
                //mVsq.write( file2 );
                if (hide) {
                    try {
                        System.IO.File.SetAttributes(file, System.IO.FileAttributes.Hidden);
                        //System.IO.File.SetAttributes( file2, System.IO.FileAttributes.Hidden );
                    } catch (Exception ex) {
                        serr.println("AppManager#saveToCor; ex=" + ex);
                        Logger.write(typeof(MusicManager) + ".saveToCor; ex=" + ex + "\n");
                    }
                }
            }
        }

        public static void saveTo(string file, Action<string,string,int,int> showMessageBox, Func<string,string> _, Action<string> postProcess)
        {
            if (mVsq != null) {
                if (ApplicationGlobal.appConfig.UseProjectCache) {
                    // キャッシュディレクトリの処理
                    string dir = PortUtil.getDirectoryName(file);
                    string name = PortUtil.getFileNameWithoutExtension(file);
                    string cacheDir = Path.Combine(dir, name + ".cadencii");

                    if (!Directory.Exists(cacheDir)) {
                        try {
                            PortUtil.createDirectory(cacheDir);
                        } catch (Exception ex) {
                            serr.println("AppManager#saveTo; ex=" + ex);
                            showMessageBox(PortUtil.formatMessage(_("failed creating cache directory, '{0}'."), cacheDir),
                                            _("Info."),
								cadencii.java.awt.AwtHost.OK_OPTION,
                                            Dialog.MSGBOX_INFORMATION_MESSAGE);
                            Logger.write(typeof(MusicManager) + ".saveTo; ex=" + ex + "\n");
                            return;
                        }
                    }

                    string currentCacheDir = ApplicationGlobal.getTempWaveDir();
                    if (!currentCacheDir.Equals(cacheDir)) {
                        for (int i = 1; i < mVsq.Track.Count; i++) {
                            string wavFrom = Path.Combine(currentCacheDir, i + ".wav");
                            string wavTo = Path.Combine(cacheDir, i + ".wav");
                            if (System.IO.File.Exists(wavFrom)) {
                                if (System.IO.File.Exists(wavTo)) {
                                    try {
                                        PortUtil.deleteFile(wavTo);
                                    } catch (Exception ex) {
                                        serr.println("AppManager#saveTo; ex=" + ex);
                                        Logger.write(typeof(MusicManager) + ".saveTo; ex=" + ex + "\n");
                                    }
                                }
                                try {
                                    PortUtil.moveFile(wavFrom, wavTo);
                                } catch (Exception ex) {
                                    serr.println("AppManager#saveTo; ex=" + ex);
                                    showMessageBox(PortUtil.formatMessage(_("failed copying WAVE cache file, '{0}'."), wavFrom),
                                                    _("Error"),
										cadencii.java.awt.AwtHost.OK_OPTION,
                                                    Dialog.MSGBOX_WARNING_MESSAGE);
                                    Logger.write(typeof(MusicManager) + ".saveTo; ex=" + ex + "\n");
                                    break;
                                }
                            }

                            string xmlFrom = Path.Combine(currentCacheDir, i + ".xml");
                            string xmlTo = Path.Combine(cacheDir, i + ".xml");
                            if (System.IO.File.Exists(xmlFrom)) {
                                if (System.IO.File.Exists(xmlTo)) {
                                    try {
                                        PortUtil.deleteFile(xmlTo);
                                    } catch (Exception ex) {
                                        serr.println("AppManager#saveTo; ex=" + ex);
                                        Logger.write(typeof(MusicManager) + ".saveTo; ex=" + ex + "\n");
                                    }
                                }
                                try {
                                    PortUtil.moveFile(xmlFrom, xmlTo);
                                } catch (Exception ex) {
                                    serr.println("AppManager#saveTo; ex=" + ex);
                                    showMessageBox(PortUtil.formatMessage(_("failed copying XML cache file, '{0}'."), xmlFrom),
                                                    _("Error"),
										cadencii.java.awt.AwtHost.OK_OPTION,
                                                    Dialog.MSGBOX_WARNING_MESSAGE);
                                    Logger.write(typeof(MusicManager) + ".saveTo; ex=" + ex + "\n");
                                    break;
                                }
                            }
                        }

						ApplicationGlobal.setTempWaveDir(cacheDir);
                    }
                    mVsq.cacheDir = cacheDir;
                }
            }

            saveToCor(file);

            if (mVsq != null) {
                mFile = file;
                postProcess (mFile);
            }
        }
		
        /// <summary>
        /// xvsqファイルを読込みます．キャッシュディレクトリの更新は行われません
        /// </summary>
        /// <param name="file"></param>
        /// <returns>ファイルの読み込みに成功した場合にtrueを，それ以外の場合はfalseを返します</returns>
        public static bool readVsq(string file, Action<bool> postProcess)
        {
            mFile = file;
            VsqFileEx newvsq = null;
            try {
                newvsq = VsqFileEx.readFromXml(file);
            } catch (Exception ex) {
                serr.println("AppManager#readVsq; ex=" + ex);
                Logger.write(typeof(MusicManager) + ".readVsq; ex=" + ex + "\n");
                return true;
            }
            if (newvsq == null) {
                return true;
            }
            mVsq = newvsq;
            for (int i = 0; i < mVsq.editorStatus.renderRequired.Length; i++) {
                if (i < mVsq.Track.Count - 1) {
                    mVsq.editorStatus.renderRequired[i] = true;
                } else {
                    mVsq.editorStatus.renderRequired[i] = false;
                }
            }
            //mStartMarker = mVsq.getPreMeasureClocks();
            //int bar = mVsq.getPreMeasure() + 1;
            //mEndMarker = mVsq.getClockFromBarCount( bar );
			postProcess (mVsq.Track.Count >= 1);
            return false;
        }

#if !TREECOM
        /// <summary>
        /// vsqファイル。
        /// </summary>
        public static VsqFileEx getVsqFile()
        {
            return mVsq;
        }

        [Obsolete]
        public static VsqFileEx VsqFile
        {
            get
            {
                return getVsqFile();
            }
        }
#endif

        public static void setVsqFile(VsqFileEx vsq, Action<int> postProcess)
        {
            mVsq = vsq;
            for (int i = 0; i < mVsq.editorStatus.renderRequired.Length; i++) {
                if (i < mVsq.Track.Count - 1) {
                    mVsq.editorStatus.renderRequired[i] = true;
                } else {
                    mVsq.editorStatus.renderRequired[i] = false;
                }
            }
            mFile = "";
            //mStartMarker = mVsq.getPreMeasureClocks();
            //int bar = mVsq.getPreMeasure() + 1;
            //mEndMarker = mVsq.getClockFromBarCount( bar );
			postProcess (mVsq.getPreMeasureClocks());
        }
	}
}

