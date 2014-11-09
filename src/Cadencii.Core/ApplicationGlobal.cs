using System;
using System.IO;
using Cadencii.Utilities;

namespace cadencii.core
{
	public static class ApplicationGlobal
	{
		static ApplicationGlobal ()
		{
			// from EditorManager#init()
			#if !TREECOM
			mID = PortUtil.getMD5FromString ((long)PortUtil.getCurrentTime () + "").Replace ("_", "");
			mTempWaveDir = Path.Combine (getCadenciiTempDir (), mID);
			if (!Directory.Exists (mTempWaveDir)) {
				PortUtil.createDirectory (mTempWaveDir);
			}
			string log = Path.Combine (getTempWaveDir (), "run.log");
			#endif
		}
		public static EditorConfig appConfig = new EditorConfig ();
        /// <summary>
        /// プリメジャーの最小値
        /// </summary>
        public const int MIN_PRE_MEASURE = 1;
        /// <summary>
        /// プリメジャーの最大値
        /// </summary>
        public const int MAX_PRE_MEASURE = 8;
        public const string CONFIG_FILE_NAME = "config.xml";

        public const int MAX_NUM_TRACK = 16;
		
        /// <summary>
        /// Cadenciiが使用する一時ディレクトリのパスを取得します。
        /// </summary>
        /// <returns></returns>
        public static string getCadenciiTempDir()
        {
            string temp = Path.Combine(PortUtil.getTempPath(), TEMPDIR_NAME);
            if (!Directory.Exists(temp)) {
                PortUtil.createDirectory(temp);
            }
            return temp;
        }
		
        private const string TEMPDIR_NAME = "cadencii";

		/// <summary>
        /// このCadenciiのID。起動ごとにユニークな値が設定され、一時フォルダのフォルダ名等に使用する
        /// </summary>
        private static string mID = "";
		/// <summary>
        /// wavを出力するための一時ディレクトリのパス。
        /// </summary>
        private static string mTempWaveDir = "";
		
        /// <summary>
        /// 音声ファイルのキャッシュディレクトリのパスを設定します。
        /// このメソッドでは、キャッシュディレクトリの変更に伴う他の処理は実行されません。
        /// </summary>
        /// <param name="value"></param>
        public static void setTempWaveDir(string value)
        {
#if DEBUG
            Logger.StdOut("EditorManager#setTempWaveDir; before: \"" + mTempWaveDir + "\"");
            Logger.StdOut("                           after:  \"" + value + "\"");
#endif
            mTempWaveDir = value;
        }

        /// <summary>
        /// 音声ファイルのキャッシュディレクトリのパスを取得します。
        /// </summary>
        /// <returns></returns>
        public static string getTempWaveDir()
        {
            return mTempWaveDir;
        }
		
        /// <summary>
        /// FormMainを識別するID
        /// </summary>
        public static string getID()
        {
            return mID;
        }
		
        /// <summary>
        /// gettext
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string _(string id)
        {
            return cadencii.apputil.Messaging.getMessage(id);
        }
		
        /// <summary>
        /// 鍵盤用の音源が保存されているディレクトリへのパスを返します。
        /// </summary>
        /// <returns></returns>
        public static string getKeySoundPath()
        {
            string data_path = getApplicationDataPath();
            string ret = Path.Combine(data_path, "cache");
            if (!Directory.Exists(ret)) {
                PortUtil.createDirectory(ret);
            }
            return ret;
        }
		
		private const string CONFIG_DIR_NAME = "Cadencii";
        /// <summary>
        /// アプリケーションデータの保存位置を取得します
        /// Gets the path for application data
        /// </summary>
        public static string getApplicationDataPath()
        {
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Boare");
            if (!Directory.Exists(dir)) {
                PortUtil.createDirectory(dir);
            }

            string dir2 = Path.Combine(dir, CONFIG_DIR_NAME);
            if (!Directory.Exists(dir2)) {
                PortUtil.createDirectory(dir2);
            }
            Logger.StdOut("Cadencii accesses ApplicationData at" + dir2 + "\n");//Keep this line for debuging.

            return dir2;
        }
    }
}

