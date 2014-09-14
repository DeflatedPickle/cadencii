using System;
using System.IO;
using cadencii.core;
using System.Collections.Generic;
using System.Reflection;
using cadencii.vsq;

namespace cadencii
{
	public static class EditorManager
	{
		/// <summary>
		/// エディタの設定
		/// </summary>
		public static EditorConfig editorConfig = new EditorConfig ();

		static EditorManager ()
		{
			Selected = 1;
			SelectedTool = EditTool.PENCIL;
			editHistory = new EditHistoryModel ();
		}

		static int mSelected;

		/// <summary>
		/// 現在選択されているトラックを取得または設定します
		/// </summary>
		public static int Selected {
			get {
				int tracks = MusicManager.getVsqFile ().Track.Count;
				if (tracks <= mSelected) {
					mSelected = tracks - 1;
				}
				return mSelected;
			}
			set {
				mSelected = value;
			}
		}

		/// <summary>
		/// 現在選択されているツールを取得または設定します
		/// </summary>
		/// <returns></returns>
		public static EditTool SelectedTool { get; set; }

		/// <summary>
		/// 編集履歴を管理するmodel
		/// </summary>
		public static EditHistoryModel editHistory = null;

		#region Tool paths

		/// <summary>
		/// スクリプトが格納されているディレクトリのパスを取得します。
		/// </summary>
		/// <returns></returns>
		public static string getScriptPath ()
		{
			string dir = Path.Combine (PortUtil.getApplicationStartupPath (), "script");
			if (!Directory.Exists (dir)) {
				PortUtil.createDirectory (dir);
			}
			return dir;
		}

		/// <summary>
		/// キャッシュされたアセンブリが保存されているディレクトリのパスを取得します。
		/// </summary>
		/// <returns></returns>
		public static string getCachedAssemblyPath ()
		{
			string dir = Path.Combine (ApplicationGlobal.getApplicationDataPath (), "cachedAssembly");
			if (!Directory.Exists (dir)) {
				PortUtil.createDirectory (dir);
			}
			return dir;
		}

		/// <summary>
		/// パレットツールが格納されているディレクトリのパスを取得します。
		/// </summary>
		/// <returns></returns>
		public static string getToolPath ()
		{
			string dir = Path.Combine (PortUtil.getApplicationStartupPath (), "tool");
			if (!Directory.Exists (dir)) {
				PortUtil.createDirectory (dir);
			}
			return dir;
		}

		public static string getVersion ()
		{
			string suffix = "";
			SortedDictionary<string, Boolean> directives = Config.getDirectives ();
			suffix += "\n\n";
			foreach (var k in directives.Keys) {
				Boolean v = directives [k];
				suffix += k + ": " + (v ? "enabled" : "disabled") + "\n";
			}
			return BAssemblyInfo.fileVersion + " " + suffix;
		}

		public static string getAssemblyConfigurationAttribute ()
		{
			Assembly a = Assembly.GetAssembly (typeof(EditorManager));
			AssemblyConfigurationAttribute attr = (AssemblyConfigurationAttribute)Attribute.GetCustomAttribute (a, typeof(AssemblyConfigurationAttribute));
			return attr.Configuration;
		}

		public static string getAssemblyFileVersion (Type t)
		{
			Assembly a = Assembly.GetAssembly (t);
			AssemblyFileVersionAttribute afva = (AssemblyFileVersionAttribute)Attribute.GetCustomAttribute (a, typeof(AssemblyFileVersionAttribute));
			return afva.Version;
		}

		public static string getAssemblyNameAndFileVersion (Type t)
		{
			Assembly a = Assembly.GetAssembly (t);
			AssemblyFileVersionAttribute afva = (AssemblyFileVersionAttribute)Attribute.GetCustomAttribute (a, typeof(AssemblyFileVersionAttribute));
			return a.GetName ().Name + " v" + afva.Version;
		}

		#endregion

		#region Script eval

		/// <summary>
		/// 与えられた式をC#の数式とみなし、評価します。
		/// equationに"x"という文字列がある場合、それを変数xとみなし、引数xの値が代入される。
		/// </summary>
		/// <param name="x"></param>
		/// <param name="equation"></param>
		/// <returns></returns>
		public static double eval (double x, string equation)
		{
			string equ = "(" + equation + ")"; // ( )でくくる
			equ = equ.Replace ("Math.PI", Math.PI + ""); // πを数値に置換
			equ = equ.Replace ("Math.E", Math.E + ""); // eを数値に置換
			equ = equ.Replace ("exp", "ezp"); // exp を ezp に置換しておく
			equ = equ.Replace ("x", x + ""); // xを数字に置換
			equ = equ.Replace ("ezp", "exp"); // ezp を exp に戻す

			int m0 = 0; // -- の処理-------（注釈：x を数値に置換したので、x が負値のとき  --3.1 のようになっている）
			while (true) {
				int m1 = equ.IndexOf ("--", m0);
				if (m1 < 0) {
					break;
				}
				int eq_mi_1 = equ [m1 - 1];
				if (m1 == 0 || eq_mi_1 == 40 || eq_mi_1 == 42 || eq_mi_1 == 47 || eq_mi_1 == 43 || eq_mi_1 == 44) {
					equ = equ.Substring (0, m1 - 0) + equ.Substring (m1 + 2); // -- を 取る
				} else {
					equ = equ.Substring (0, m1 - 0) + "+" + equ.Substring (m1 + 2); // -- を + に置換
				}
				m0 = m1;
				if (m0 > PortUtil.getStringLength (equ) - 1) {
					break;
				}
			}

			m0 = 0; // - の処理-------
			while (true) {
				int m1 = equ.IndexOf ("-", m0);
				if (m1 < 0) {
					break;
				}
				int eq_mi_1 = equ [m1 - 1];
				if (m1 == 0 || eq_mi_1 == 40 || eq_mi_1 == 42 || eq_mi_1 == 47 || eq_mi_1 == 43 || eq_mi_1 == 44) {
					m0 = m1 + 1;
				} else {
					equ = equ.Substring (0, m1 - 0) + "+(-1)*" + equ.Substring (m1 + 1); // -a、-Math.sin(A) などを +(-1)*a、 +(-1)*Math.sin(A) などに置き換える
					m0 = m1 + 6;
				}
				if (m0 > PortUtil.getStringLength (equ) - 1) {
					break;
				}
			}
			double valResult = double.Parse (evalMy0 (equ));
			return valResult;
		}

		//----------------------------------------------------------------------------------
		private static string evalMy0 (string equation)
		{
			string equ = equation;
			while (true) {
				// 最内側の（ ） から計算する（注釈：最内側（ ）内には、Math.…() のようなものはない）
				int n1 = equ.IndexOf (")");
				if (n1 < 0) {
					break;
				} // ) の検索
				int n2 = equ.LastIndexOf ("(", n1 - 1); // ( に対応する ) の検索
				if (n2 < 0) {
					break;
				}
				string str2 = equ.Substring (n2 + 1, n1 - n2 - 1); // ( )内の文字
				int ne0 = str2.IndexOf (","); // ( )内の , の検索
				double val = 0;

				if (ne0 >= 0) {
					// ( )内に , があるので、 Math.log(A,B) or Math.Pow(A,B) の処理
					if (equ.Substring (n2 - 3, n2 - n2 + 3) == "log") {
						// Math.log(A,B) のとき
						string strA = str2.Substring (0, ne0 - 0); // Math.log(A,B)の A の文字
						double valA = double.Parse (evalMy0 ("(" + strA + ")")); // （注：再帰である）
						string strB = str2.Substring (ne0 + 1); // Math.log(A,B)の B の文字
						double valB = double.Parse (evalMy0 ("(" + strB + ")")); //（注：再帰である）
						val = Math.Log (valB) / Math.Log (valA);
						equ = equ.Replace ("Math.Log(" + strA + "," + strB + ")", "" + val);
					} else if (equ.Substring (n2 - 3, n2 - n2 + 3) == "pow") { // Math.Pow(A,B) のとき
						string strA = str2.Substring (0, ne0 - 0); // Math.Pow(A,B)の A の文字
						double valA = double.Parse (evalMy0 ("(" + strA + ")")); // （注：再帰である）
						string strB = str2.Substring (ne0 + 1); // Math.Pow(A,B)の B の文字
						double valB = double.Parse (evalMy0 ("(" + strB + ")")); //（注：再帰である）
						val = Math.Pow (valA, valB);
						equ = equ.Replace ("Math.Pow(" + strA + "," + strB + ")", "" + val);
					}
				} else {
					int check0 = 0; // strが数値（数字）かどうかチェック（str="-3.7" なら 数値なので 0 とする）
					for (int i = 0; i < PortUtil.getStringLength (str2); i++) {
						if (i == 0) {
							int str0 = str2 [0];
							if ((str0 < 48 || str0 > 57) && str0 != 46 && str0 != 43 && str0 != 45) {
								check0 = 1;
								break;
							}
						} else {
							int stri = str2 [i];
							if ((stri < 48 || stri > 57) && stri != 46) {
								check0 = 1;
								break;
							}
						}
					}

					if (check0 == 1) {
						val = evalMy1 (str2); // ( ) の処理をし数値をもとめる
					} else {
						val = double.Parse (str2); // 文字を数値に変換
					}
					if (n2 - 8 >= 0) {
						string str1 = equ.Substring (n2 - 8, n2 - (n2 - 8));
						if (str1 == "Math.Sin") {
							val = Math.Sin (val);
							equ = equ.Replace ("Math.Sin(" + str2 + ")", "" + val);
							n2 -= 8;
						} else if (str1 == "Math.Cos") {
							val = Math.Cos (val);
							equ = equ.Replace ("Math.Cos(" + str2 + ")", "" + val);
							n2 -= 8;
						} else if (str1 == "Math.Tan") {
							val = Math.Tan (val);
							equ = equ.Replace ("Math.Tan(" + str2 + ")", "" + val);
							n2 -= 8;
						} else if (str1 == "ath.Asin") {
							val = Math.Asin (val);
							equ = equ.Replace ("Math.Asin(" + str2 + ")", "" + val);
							n2 -= 9;
						} else if (str1 == "ath.Acos") {
							val = Math.Acos (val);
							equ = equ.Replace ("Math.Acos(" + str2 + ")", "" + val);
							n2 -= 9;
						} else if (str1 == "ath.Atan") {
							val = Math.Atan (val);
							equ = equ.Replace ("Math.Atan(" + str2 + ")", "" + val);
							n2 -= 9;
						} else if (str1 == "Math.Log") {
							val = Math.Log (val);
							equ = equ.Replace ("Math.Log(" + str2 + ")", "" + val);
							n2 -= 8;
						} else if (str1 == "Math.Exp") {
							val = Math.Exp (val);
							equ = equ.Replace ("Math.Exp(" + str2 + ")", "" + val);
							n2 -= 8;
						} else if (str1 == "Math.Abs") {
							val = Math.Abs (val);
							equ = equ.Replace ("Math.Abs(" + str2 + ")", "" + val);
							n2 -= 8;
						} else if (str1 == "ath.Sqrt") {
							val = Math.Sqrt (val);
							equ = equ.Replace ("Math.Sqrt(" + str2 + ")", "" + val);
							n2 -= 9;
						} else {
							equ = equ.Replace ("(" + str2 + ")", "" + val);
						} // ( ) を取る
					} else {
						equ = equ.Replace ("(" + str2 + ")", "" + val); // ( ) を取る
					}
				}
			}
			return equ;
		}

		//　* と / のみからなる数式の いくつかの和、差からなる式の処理----------------
		private static double evalMy1 (string equation)
		{
			double val = 0;
			while (true) {
				string equ0 = "";
				int n0 = equation.IndexOf ("+");
				if (n0 < 0) {
					equ0 = equation;
				} else {
					equ0 = equation.Substring (0, n0 - 0);
				} // 最初の + より前の項
				val += evalMy2 (equ0);
				if (n0 < 0) {
					break;
				} else {
					equation = equation.Substring (n0 + 1);
				} // 最初の + より以降の項
			}
			return val;
		}

		//　* と / のみからなる数式についての処理-----------------------------------
		private static double evalMy2 (string equation)
		{
			double val0 = 1;
			while (true) {
				string equ0 = "";
				int n0 = equation.IndexOf ("*");
				if (n0 < 0) {
					equ0 = equation;
				} else {
					equ0 = equation.Substring (0, n0);
				} // 最初の * より前の項

				int kai = 0;
				double val1 = 1;
				while (true) { // / を含んだ項の計算
					string equ1 = "";
					int n1 = equ0.IndexOf ("/");
					if (n1 < 0) {
						equ1 = equ0;
					} else {
						equ1 = equ0.Substring (0, n1 - 0);
					} // 最初の / より前の項
					if (kai == 0) {
						val1 = double.Parse (equ1);
					} else {
						val1 /= double.Parse (equ1);
					}
					if (n1 < 0) {
						break;
					} else {
						kai++;
						equ0 = equ0.Substring (n1 + 1);
					} // 最初の / より以降の項
				}
				val0 *= val1;
				if (n0 < 0) {
					break;
				} else {
					equation = equation.Substring (n0 + 1);
				} // 最初の * より以降の項
			}
			return val0;
		}

		#endregion

		/// <summary>
		/// VSQイベントの長さを変更すると同時に、ビブラートの長さを指定したルールに則って変更します。
		/// </summary>
		/// <param name="vsq_event"></param>
		/// <param name="new_length"></param>
		/// <param name="rule"></param>
		public static void editLengthOfVsqEvent (VsqEvent vsq_event, int new_length, VibratoLengthEditingRule rule)
		{
#if DEBUG
			sout.println ("Utility#editLengthOfVsqEvent; rule=" + rule);
#endif
			if (vsq_event.ID.VibratoHandle != null) {
				int oldlength = vsq_event.ID.getLength ();
				int new_delay = vsq_event.ID.VibratoDelay; // ここではディレイが独立変数

				if (rule == VibratoLengthEditingRule.DELAY) {
					// ディレイが保存される
					// 特に何もしない
				} else if (rule == VibratoLengthEditingRule.LENGTH) {
					// ビブラート長さが保存される
					new_delay = new_length - vsq_event.ID.VibratoHandle.getLength ();
					if (new_delay < 0) {
						new_delay = 0;
					}
				} else if (rule == VibratoLengthEditingRule.PERCENTAGE) {
					// ビブラート長の割合が保存される
					double old_percentage = vsq_event.ID.VibratoDelay / (double)oldlength * 100.0;
					new_delay = (int)(new_length * old_percentage / 100.0);
					if (new_delay < 0) {
						new_delay = 0;
					}
				}

				if (new_delay >= new_length) {
					// ディレイが音符より長い場合。ビブラートは削除される
					vsq_event.ID.VibratoDelay = new_length;
					vsq_event.ID.VibratoHandle = null;
				} else {
					vsq_event.ID.VibratoDelay = new_delay;
					vsq_event.ID.VibratoHandle.setLength (new_length - new_delay);
				}
			}

			if (vsq_event.ID.type == VsqIDType.Anote) {
				// 音符
				vsq_event.ID.setLength (new_length);
			} else if (vsq_event.ID.type == VsqIDType.Singer) {
				// 歌手変更
				vsq_event.ID.setLength (1);
			} else if (vsq_event.ID.type == VsqIDType.Aicon) {
				// 強弱記号、クレッシェンド、デクレッシェンド
				if (vsq_event.ID.IconDynamicsHandle != null) {
					if (vsq_event.ID.IconDynamicsHandle.isDynaffType ()) {
						// 強弱記号
						vsq_event.ID.IconDynamicsHandle.setLength (1);
						vsq_event.ID.setLength (1);
					} else {
						// クレッシェンド、デクレッシェンド
						vsq_event.ID.IconDynamicsHandle.setLength (new_length);
						vsq_event.ID.setLength (new_length);
					}
				} else {
					vsq_event.ID.setLength (new_length);
				}
			} else {
				// 不明
				vsq_event.ID.setLength (new_length);
			}
		}

		/// <summary>
		/// 音符の長さが変更されたとき、ビブラートの長さがどう影響を受けるかを決める因子
		/// </summary>
		public static VibratoLengthEditingRule vibratoLengthEditingRule = VibratoLengthEditingRule.PERCENTAGE;
	}
}

