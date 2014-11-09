using System;
using System.IO;
using System.Text;
using cadencii.vsq;
using Cadencii.Utilities;

namespace cadencii.utau
{
	public static class Utau
	{
	/// <summary>
        /// UTAU関連のテキストファイルで受け付けるエンコーディングの種類
        /// </summary>
        public static readonly string[] TEXT_ENCODINGS_IN_UTAU = new string[] { "Shift_JIS", "UTF-16", "us-ascii" };
		
        /// <summary>
        /// 指定したディレクトリをUTAU音源のディレクトリとみなし，音源名と音源の保存パスを保持したSingerConfigを返します
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static string readUtauSingerConfig(string directory, SingerConfig sc)
        {
            sc.VOICEIDSTR = directory;

            // character.txt読込み
            string character = Path.Combine(directory, "character.txt");
            string name = null;
            string image = "";
            int mode = 0;
            if (System.IO.File.Exists(character)) {
                // 読み込みを試みるエンコーディングのリスト
                foreach (string encoding in TEXT_ENCODINGS_IN_UTAU) {
                    StreamReader sr2 = null;
                    try {
                        sr2 = new StreamReader(character, Encoding.GetEncoding(encoding));
                        string line = "";
                        while ((line = sr2.ReadLine()) != null) {
                            string[] spl = PortUtil.splitString(line, '=');
                            if (spl.Length > 1) {
                                string s = spl[0].ToLower();
                                if (s == "name") {
                                    name = spl[1];
                                    mode |= 1;
                                } else if (s == "image") {
                                    image = Path.Combine(directory, spl[1]);
                                    mode |= 2;
                                }
                                if (mode == 3) {
                                    break;
                                }
                            }
                        }
                    } catch (Exception ex) {
                        Logger.StdErr("Utility#readUtausingerConfig; ex=" + ex);
                        Logger.write(typeof(Utau) + ".readUtausingerConfig; ex=" + ex + "\n");
                    } finally {
                        if (sr2 != null) {
                            try {
                                sr2.Close();
                            } catch (Exception ex2) {
                                Logger.StdErr("Utility#readUtausingerConfig; ex2=" + ex2);
                                Logger.write(typeof(Utau) + ".readUtausingerConfig; ex= " + ex2 + "\n");
                            }
                        }
                    }
                    if (name != null) {
#if DEBUG
                        Logger.StdOut("Utility#readUtausingerConfig; name=" + name + "; encoding=" + encoding);
#endif
                        break;
                    }
                }
            }
            if (name == null) {
                name = PortUtil.getFileNameWithoutExtension(directory);
            }
            sc.VOICENAME = name;
            return image;
        }
	}
}

