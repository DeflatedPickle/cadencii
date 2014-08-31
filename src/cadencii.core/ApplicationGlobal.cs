using System;
using System.IO;

namespace cadencii.core
{
	public static class ApplicationGlobal
	{
		static ApplicationGlobal ()
		{
			// from AppManager#init()
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
		public const int MIN_KEY_WIDTH = 68;
        public const int MAX_KEY_WIDTH = MIN_KEY_WIDTH * 5;
        /// <summary>
        /// プリメジャーの最小値
        /// </summary>
        public const int MIN_PRE_MEASURE = 1;
        /// <summary>
        /// プリメジャーの最大値
        /// </summary>
        public const int MAX_PRE_MEASURE = 8;
        private const string CONFIG_FILE_NAME = "config.xml";

        public const int MAX_NUM_TRACK = 16;

        /// <summary>
        /// 鍵盤の表示幅(pixel)
        /// </summary>
        public static int keyWidth = MIN_KEY_WIDTH * 2;
        /// <summary>
        /// keyWidth+keyOffsetの位置からが、0になってる
        /// </summary>
        public const int keyOffset = 6;
		
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
            sout.println("AppManager#setTempWaveDir; before: \"" + mTempWaveDir + "\"");
            sout.println("                           after:  \"" + value + "\"");
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
	}
}

