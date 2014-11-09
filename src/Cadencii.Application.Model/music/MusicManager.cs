using System;
using System.IO;
using cadencii.core;
using System.Collections.Generic;
using cadencii.vsq;
using System.Linq;
using cadencii.utau;
using Cadencii.Utilities;

namespace cadencii
{
	public class MusicManager
	{
		private static string mFile {
			get { return EditorManager.CurrentFile; }
			set { EditorManager.CurrentFile = value; }
		}

		#if !TREECOM
		private static VsqFileEx mVsq;
		#endif
		
		/// <summary>
		/// _vsq_fileにセットされたvsqファイルの名前を取得します。
		/// </summary>
		public static string getFileName ()
		{
			return mFile;
		}

		public static void ProcessAutoBackup ()
		{
			if (!mFile.Equals ("") && System.IO.File.Exists (mFile)) {
				string path = PortUtil.getDirectoryName (mFile);
				string backup = Path.Combine (path, "~$" + PortUtil.getFileName (mFile));
				string file2 = Path.Combine (path, PortUtil.getFileNameWithoutExtension (backup) + ".vsq");
				if (System.IO.File.Exists (backup)) {
					try {
						PortUtil.deleteFile (backup);
					} catch (Exception ex) {
						Logger.StdErr ("EditorManager::handleAutoBackupTimerTick; ex=" + ex);
						Logger.write (typeof(MusicManager) + ".handleAutoBackupTimerTick; ex=" + ex + "\n");
					}
				}
				if (System.IO.File.Exists (file2)) {
					try {
						PortUtil.deleteFile (file2);
					} catch (Exception ex) {
						Logger.StdErr ("EditorManager::handleAutoBackupTimerTick; ex=" + ex);
						Logger.write (typeof(MusicManager) + ".handleAutoBackupTimerTick; ex=" + ex + "\n");
					}
				}
				saveToCor (backup);
			}
		}

		private static void saveToCor (string file)
		{
			bool hide = false;
			if (mVsq != null) {
				string path = PortUtil.getDirectoryName (file);
				//String file2 = fsys.combine( path, PortUtil.getFileNameWithoutExtension( file ) + ".vsq" );
				mVsq.writeAsXml (file);
				//mVsq.write( file2 );
				if (hide) {
					try {
						System.IO.File.SetAttributes (file, System.IO.FileAttributes.Hidden);
						//System.IO.File.SetAttributes( file2, System.IO.FileAttributes.Hidden );
					} catch (Exception ex) {
						Logger.StdErr ("EditorManager#saveToCor; ex=" + ex);
						Logger.write (typeof(MusicManager) + ".saveToCor; ex=" + ex + "\n");
					}
				}
			}
		}

		public static void saveTo (string file, Action<string,string,int,int> showMessageBox, Func<string,string> _, Action<string> postProcess)
		{
			if (mVsq != null) {
				if (ApplicationGlobal.appConfig.UseProjectCache) {
					// キャッシュディレクトリの処理
					string dir = PortUtil.getDirectoryName (file);
					string name = PortUtil.getFileNameWithoutExtension (file);
					string cacheDir = Path.Combine (dir, name + ".cadencii");

					if (!Directory.Exists (cacheDir)) {
						try {
							PortUtil.createDirectory (cacheDir);
						} catch (Exception ex) {
							Logger.StdErr ("EditorManager#saveTo; ex=" + ex);
							showMessageBox (PortUtil.formatMessage (_ ("failed creating cache directory, '{0}'."), cacheDir),
								_ ("Info."),
								Cadencii.Gui.AwtHost.OK_OPTION,
								Dialog.MSGBOX_INFORMATION_MESSAGE);
							Logger.write (typeof(MusicManager) + ".saveTo; ex=" + ex + "\n");
							return;
						}
					}

					string currentCacheDir = ApplicationGlobal.getTempWaveDir ();
					if (!currentCacheDir.Equals (cacheDir)) {
						for (int i = 1; i < mVsq.Track.Count; i++) {
							string wavFrom = Path.Combine (currentCacheDir, i + ".wav");
							string wavTo = Path.Combine (cacheDir, i + ".wav");
							if (System.IO.File.Exists (wavFrom)) {
								if (System.IO.File.Exists (wavTo)) {
									try {
										PortUtil.deleteFile (wavTo);
									} catch (Exception ex) {
										Logger.StdErr ("EditorManager#saveTo; ex=" + ex);
										Logger.write (typeof(MusicManager) + ".saveTo; ex=" + ex + "\n");
									}
								}
								try {
									PortUtil.moveFile (wavFrom, wavTo);
								} catch (Exception ex) {
									Logger.StdErr ("EditorManager#saveTo; ex=" + ex);
									showMessageBox (PortUtil.formatMessage (_ ("failed copying WAVE cache file, '{0}'."), wavFrom),
										_ ("Error"),
										Cadencii.Gui.AwtHost.OK_OPTION,
										Dialog.MSGBOX_WARNING_MESSAGE);
									Logger.write (typeof(MusicManager) + ".saveTo; ex=" + ex + "\n");
									break;
								}
							}

							string xmlFrom = Path.Combine (currentCacheDir, i + ".xml");
							string xmlTo = Path.Combine (cacheDir, i + ".xml");
							if (System.IO.File.Exists (xmlFrom)) {
								if (System.IO.File.Exists (xmlTo)) {
									try {
										PortUtil.deleteFile (xmlTo);
									} catch (Exception ex) {
										Logger.StdErr ("EditorManager#saveTo; ex=" + ex);
										Logger.write (typeof(MusicManager) + ".saveTo; ex=" + ex + "\n");
									}
								}
								try {
									PortUtil.moveFile (xmlFrom, xmlTo);
								} catch (Exception ex) {
									Logger.StdErr ("EditorManager#saveTo; ex=" + ex);
									showMessageBox (PortUtil.formatMessage (_ ("failed copying XML cache file, '{0}'."), xmlFrom),
										_ ("Error"),
										Cadencii.Gui.AwtHost.OK_OPTION,
										Dialog.MSGBOX_WARNING_MESSAGE);
									Logger.write (typeof(MusicManager) + ".saveTo; ex=" + ex + "\n");
									break;
								}
							}
						}

						ApplicationGlobal.setTempWaveDir (cacheDir);
					}
					mVsq.cacheDir = cacheDir;
				}
			}

			saveToCor (file);

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
		public static bool readVsq (string file, Action<bool> postProcess)
		{
			mFile = file;
			VsqFileEx newvsq = null;
			try {
				newvsq = VsqFileEx.readFromXml (file);
			} catch (Exception ex) {
				Logger.StdErr ("EditorManager#readVsq; ex=" + ex);
				Logger.write (typeof(MusicManager) + ".readVsq; ex=" + ex + "\n");
				return true;
			}
			if (newvsq == null) {
				return true;
			}
			mVsq = newvsq;
			for (int i = 0; i < mVsq.editorStatus.renderRequired.Length; i++) {
				if (i < mVsq.Track.Count - 1) {
					mVsq.editorStatus.renderRequired [i] = true;
				} else {
					mVsq.editorStatus.renderRequired [i] = false;
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
		public static VsqFileEx getVsqFile ()
		{
			return mVsq;
		}

		[Obsolete]
		public static VsqFileEx VsqFile {
			get {
				return getVsqFile ();
			}
		}
		#endif

		public static void setVsqFile (VsqFileEx vsq, Action<int> postProcess)
		{
			mVsq = vsq;
			for (int i = 0; i < mVsq.editorStatus.renderRequired.Length; i++) {
				if (i < mVsq.Track.Count - 1) {
					mVsq.editorStatus.renderRequired [i] = true;
				} else {
					mVsq.editorStatus.renderRequired [i] = false;
				}
			}
			mFile = "";
			//mStartMarker = mVsq.getPreMeasureClocks();
			//int bar = mVsq.getPreMeasure() + 1;
			//mEndMarker = mVsq.getClockFromBarCount( bar );
			postProcess (mVsq.getPreMeasureClocks ());
		}

		#region BGM 関連

		public static int getBgmCount ()
		{
			if (mVsq == null) {
				return 0;
			} else {
				return mVsq.BgmFiles.Count;
			}
		}

		public static BgmFile getBgm (int index)
		{
			if (mVsq == null) {
				return null;
			}
			return mVsq.BgmFiles [index];
		}

		public static void removeBgm (int index, Action<ICommand> bgmRemoved)
		{
			if (mVsq == null) {
				return;
			}
			List<BgmFile> list = new List<BgmFile> ();
			int count = mVsq.BgmFiles.Count;
			for (int i = 0; i < count; i++) {
				if (i != index) {
					list.Add ((BgmFile)mVsq.BgmFiles [i].clone ());
				}
			}
			CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate (list);
			bgmRemoved (mVsq.executeCommand (run));
		}

		public static void clearBgm (Action<ICommand> bgmCleared)
		{
			if (mVsq == null) {
				return;
			}
			List<BgmFile> list = new List<BgmFile> ();
			CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate (list);
			bgmCleared (mVsq.executeCommand (run));
		}

		public static void addBgm (string file, Action<ICommand> bgmAdded)
		{
			if (mVsq == null) {
				return;
			}
			List<BgmFile> list = new List<BgmFile> ();
			int count = mVsq.BgmFiles.Count;
			for (int i = 0; i < count; i++) {
				list.Add ((BgmFile)mVsq.BgmFiles [i].clone ());
			}
			BgmFile item = new BgmFile ();
			item.file = file;
			item.feder = 0;
			item.panpot = 0;
			list.Add (item);
			CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate (list);
			bgmAdded (mVsq.executeCommand (run));
		}

		#endregion

		#region Singer related
		/// <summary>
		/// 指定した音声合成システムが使用する歌手のリストを取得します
		/// </summary>
		/// <param name="kind">音声合成システムの種類</param>
		/// <returns>歌手のリスト</returns>
		public static List<VsqID> getSingerListFromRendererKind (RendererKind kind)
		{
			List<VsqID> singers = null;
			if (kind == RendererKind.AQUES_TONE) {
				singers = new List<VsqID> ();
#if ENABLE_AQUESTONE
				singers.AddRange (AquesToneDriver.Singers.Select ((config) => getSingerIDAquesTone (config.Program)));
#endif
			} else if (kind == RendererKind.AQUES_TONE2) {
				singers = new List<VsqID> ();
#if ENABLE_AQUESTONE
				singers.AddRange (AquesTone2Driver.Singers.Select ((config) => getSingerIDAquesTone2 (config.Program)));
#endif
			} else if (kind == RendererKind.VCNT || kind == RendererKind.UTAU) {
				List<SingerConfig> list = ApplicationGlobal.appConfig.UtauSingers;
				singers = new List<VsqID> ();
				foreach (var sc in list) {
					singers.Add (getSingerIDUtau (sc.Language, sc.Program));
				}
			} else if (kind == RendererKind.VOCALOID1) {
				SingerConfig[] configs = VocaloSysUtil.getSingerConfigs (SynthesizerType.VOCALOID1);
				singers = new List<VsqID> ();
				for (int i = 0; i < configs.Length; i++) {
					SingerConfig sc = configs [i];
					singers.Add (VocaloSysUtil.getSingerID (sc.Language, sc.Program, SynthesizerType.VOCALOID1));
				}
			} else if (kind == RendererKind.VOCALOID2) {
				singers = new List<VsqID> ();
				SingerConfig[] configs = VocaloSysUtil.getSingerConfigs (SynthesizerType.VOCALOID2);
				for (int i = 0; i < configs.Length; i++) {
					SingerConfig sc = configs [i];
					singers.Add (VocaloSysUtil.getSingerID (sc.Language, sc.Program, SynthesizerType.VOCALOID2));
				}
			}
			return singers;
		}

		public static VsqID getSingerIDUtau (int language, int program)
		{
			VsqID ret = new VsqID (0);
			ret.type = VsqIDType.Singer;
			int index = language << 7 | program;
			if (0 <= index && index < ApplicationGlobal.appConfig.UtauSingers.Count) {
				SingerConfig sc = ApplicationGlobal.appConfig.UtauSingers [index];
				ret.IconHandle = new IconHandle ();
				ret.IconHandle.IconID = "$0701" + PortUtil.toHexString (language, 2) + PortUtil.toHexString (program, 2);
				ret.IconHandle.IDS = sc.VOICENAME;
				ret.IconHandle.Index = 0;
				ret.IconHandle.Language = language;
				ret.IconHandle.setLength (1);
				ret.IconHandle.Original = language << 8 | program;
				ret.IconHandle.Program = program;
				ret.IconHandle.Caption = "";
				return ret;
			} else {
				ret.IconHandle = new IconHandle ();
				ret.IconHandle.Program = 0;
				ret.IconHandle.Language = 0;
				ret.IconHandle.IconID = "$0701" + PortUtil.toHexString (0, 4);
				ret.IconHandle.IDS = "Unknown";
				ret.type = VsqIDType.Singer;
				return ret;
			}
		}

		public static SingerConfig getSingerInfoUtau (int language, int program)
		{
			int index = language << 7 | program;
			if (0 <= index && index < ApplicationGlobal.appConfig.UtauSingers.Count) {
				return ApplicationGlobal.appConfig.UtauSingers [index];
			} else {
				return null;
			}
		}

		/// <summary>
		/// 指定したトラックの，指定した音符イベントについて，UTAUのパラメータを適用します
		/// </summary>
		/// <param name="vsq_track"></param>
		/// <param name="item"></param>
		public static void applyUtauParameter (VsqTrack vsq_track, VsqEvent item)
		{
			VsqEvent singer = vsq_track.getSingerEventAt (item.Clock);
			if (singer == null) {
				return;
			}
			SingerConfig sc = getSingerInfoUtau (singer.ID.IconHandle.Language, singer.ID.IconHandle.Program);
			if (sc != null && UtauWaveGenerator.mUtauVoiceDB.ContainsKey (sc.VOICEIDSTR)) {
				UtauVoiceDB db = UtauWaveGenerator.mUtauVoiceDB [sc.VOICEIDSTR];
				OtoArgs oa = db.attachFileNameFromLyric (item.ID.LyricHandle.L0.Phrase, item.ID.Note);
				if (item.UstEvent == null) {
					item.UstEvent = new UstEvent ();
				}
				item.UstEvent.setVoiceOverlap (oa.msOverlap);
				item.UstEvent.setPreUtterance (oa.msPreUtterance);
			}
		}

		private static VsqID createAquesToneSingerID (int program, Func<int, SingerConfig> get_singer_config)
		{
			VsqID ret = new VsqID (0);
			ret.type = VsqIDType.Singer;
			SingerConfig config = null;
			if (get_singer_config != null) {
				config = get_singer_config (program);
			}
			if (config != null) {
				int lang = 0;
				ret.IconHandle = new IconHandle ();
				ret.IconHandle.IconID = "$0701" + PortUtil.toHexString (lang, 2) + PortUtil.toHexString (program, 2);
				ret.IconHandle.IDS = config.VOICENAME;
				ret.IconHandle.Index = 0;
				ret.IconHandle.Language = lang;
				ret.IconHandle.setLength (1);
				ret.IconHandle.Original = lang << 8 | program;
				ret.IconHandle.Program = program;
				ret.IconHandle.Caption = "";
			} else {
				ret.IconHandle = new IconHandle ();
				ret.IconHandle.Program = 0;
				ret.IconHandle.Language = 0;
				ret.IconHandle.IconID = "$0701" + PortUtil.toHexString (0, 2) + PortUtil.toHexString (0, 2);
				ret.IconHandle.IDS = "Unknown";
				ret.type = VsqIDType.Singer;
			}
			return ret;
		}

		public static VsqID getSingerIDAquesTone (int program)
		{
#if ENABLE_AQUESTONE
			return createAquesToneSingerID (program, AquesToneDriver.getSingerConfig);
#else
			return createAquesToneSingerID( program, null );
#endif
		}

		public static VsqID getSingerIDAquesTone2 (int program)
		{
#if ENABLE_AQUESTONE
			return createAquesToneSingerID (program, AquesTone2Driver.getSingerConfig);
#else
			return createAquesToneSingerID( program, null );
#endif
		}
		#endregion
	}
}

