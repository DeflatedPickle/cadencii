using System;
using System.IO;
using cadencii.core;
using System.Collections.Generic;
using System.Reflection;
using cadencii.vsq;
using cadencii.java.awt;
using cadencii.xml;
using cadencii.utau;

namespace cadencii
{
	public static class EditorManager
	{
		/// <summary>
		/// メインの編集画面のインスタンス
		/// </summary>
		public static FormMainUi MainWindow = null;
		#if ENABLE_PROPERTY
		/// <summary>
		/// プロパティパネルのインスタンス
		/// </summary>
		public static PropertyPanel propertyPanel;
		#endif

		/// <summary>
		/// 鍵盤の表示幅(pixel)
		/// </summary>
		public static int keyWidth = EditorConfig.MIN_KEY_WIDTH * 2;

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

		#region Colors (this should be customizible and saved somewhere else...)

		private static Color mHilightBrush = cadencii.java.awt.Colors.CornflowerBlue;

		public static Color getHilightColor ()
		{
			return mHilightBrush;
		}

		public static void setHilightColor (Color value)
		{
			mHilightBrush = value;
		}

		/// <summary>
		/// ピアノロール上の音符の警告色を取得します．
		/// 音抜けの可能性がある音符の背景色として利用されます
		/// </summary>
		/// <returns></returns>
		public static Color getAlertColor ()
		{
			return cadencii.java.awt.Colors.HotPink;
		}

		/// <summary>
		/// ピアノロール上の音符の警告色を取得します．
		/// 音抜けの可能性のある音符であって，かつ現在選択されている音符の背景色として利用されます．
		/// </summary>
		/// <returns></returns>
		public static Color getAlertHilightColor ()
		{
			return cadencii.java.awt.Colors.DeepPink;
		}

		#endregion

		#region 選択範囲の管理

		enum SelectedInterval
		{
			None,
			Curve,
			Whole
		}

		static SelectedInterval selected_interval;

		public static bool IsWholeSelectedIntervalEnabled {
			get { return selected_interval == SelectedInterval.Whole; }
			set {
				if (value)
					selected_interval = SelectedInterval.Whole;
				else
					selected_interval = selected_interval == SelectedInterval.Whole ? SelectedInterval.None : selected_interval;
			}
		}

		public static bool IsCurveSelectedIntervalEnabled {
			get { return selected_interval == SelectedInterval.Curve; }
			set {
				if (value)
					selected_interval = SelectedInterval.Curve;
				else
					selected_interval = selected_interval == SelectedInterval.Curve ? SelectedInterval.None : selected_interval;
			}
		}

		#endregion

		/// <summary>
		/// メイン画面のコントローラ
		/// </summary>
		public static FormMainController MainWindowController = null;
		/// <summary>
		/// keyWidth+keyOffsetの位置からが、0になってる
		/// </summary>
		public const int keyOffset = 6;

		#region used by SynthesizeWorker

		/// <summary>
		/// 最後にレンダリングが行われた時の、トラックの情報が格納されている。
		/// </summary>
		public static RenderedStatus[] LastRenderedStatus = new RenderedStatus[ApplicationGlobal.MAX_NUM_TRACK];
		/// <summary>
		/// RenderingStatusをXMLシリアライズするためのシリアライザ
		/// </summary>
		public static XmlSerializer RenderingStatusSerializer = new XmlSerializer (typeof(RenderedStatus));

		/// <summary>
		/// 指定したディレクトリにある合成ステータスのxmlデータを読み込みます
		/// </summary>
		/// <param name="directory">読み込むxmlが保存されたディレクトリ</param>
		/// <param name="track">読み込みを行うトラックの番号</param>
		public static void deserializeRenderingStatus (string directory, int track)
		{
			string xml = Path.Combine (directory, track + ".xml");
			RenderedStatus status = null;
			if (System.IO.File.Exists (xml)) {
				FileStream fs = null;
				try {
					fs = new FileStream (xml, FileMode.Open, FileAccess.Read);
					Object obj = EditorManager.RenderingStatusSerializer.deserialize (fs);
					if (obj != null && obj is RenderedStatus) {
						status = (RenderedStatus)obj;
					}
				} catch (Exception ex) {
					Logger.write (typeof(EditorManager) + ".deserializeRederingStatus; ex=" + ex + "\n");
					status = null;
					serr.println ("EditorManager#deserializeRederingStatus; ex=" + ex);
				} finally {
					if (fs != null) {
						try {
							fs.Close ();
						} catch (Exception ex2) {
							Logger.write (typeof(EditorManager) + ".deserializeRederingStatus; ex=" + ex2 + "\n");
							serr.println ("EditorManager#deserializeRederingStatus; ex2=" + ex2);
						}
					}
				}
			}
			EditorManager.LastRenderedStatus [track - 1] = status;
		}

		/// <summary>
		/// 指定したトラックの合成ステータスを，指定したxmlファイルに保存します．
		/// </summary>
		/// <param name="temppath"></param>
		/// <param name="track"></param>
		public static void serializeRenderingStatus (string temppath, int track)
		{
			FileStream fs = null;
			bool failed = true;
			string xml = Path.Combine (temppath, track + ".xml");
			try {
				fs = new FileStream (xml, FileMode.Create, FileAccess.Write);
				EditorManager.RenderingStatusSerializer.serialize (fs, EditorManager.LastRenderedStatus [track - 1]);
				failed = false;
			} catch (Exception ex) {
				serr.println ("FormMain#patchWorkToFreeze; ex=" + ex);
				Logger.write (typeof(EditorManager) + ".serializeRenderingStauts; ex=" + ex + "\n");
			} finally {
				if (fs != null) {
					try {
						fs.Close ();
					} catch (Exception ex2) {
						serr.println ("FormMain#patchWorkToFreeze; ex2=" + ex2);
						Logger.write (typeof(EditorManager) + ".serializeRenderingStatus; ex=" + ex2 + "\n");
					}
				}
			}

			// シリアライズに失敗した場合，該当するxmlを削除する
			if (failed) {
				if (System.IO.File.Exists (xml)) {
					try {
						PortUtil.deleteFile (xml);
					} catch (Exception ex) {
						Logger.write (typeof(EditorManager) + ".serializeRendererStatus; ex=" + ex + "\n");
					}
				}
			}
		}

		public static void setRenderRequired (int track, bool value)
		{
			var v = MusicManager.getVsqFile ();
			if (v == null) {
				return;
			}
			v.editorStatus.renderRequired [track - 1] = value;
		}

		public static void invokeWaveViewReloadRequiredEvent (int track, string wavePath, double secStart, double secEnd)
		{
			try {
				WaveViewRealoadRequiredEventArgs arg = new WaveViewRealoadRequiredEventArgs ();
				arg.track = track;
				arg.file = wavePath;
				arg.secStart = secStart;
				arg.secEnd = secEnd;
				if (WaveViewReloadRequired != null) {
					WaveViewReloadRequired.Invoke (typeof(EditorManager), arg);
				}
			} catch (Exception ex) {
				Logger.write (typeof(EditorManager) + ".invokeWaveViewReloadRequiredEvent; ex=" + ex + "\n");
				sout.println (typeof(EditorManager) + ".invokeWaveViewReloadRequiredEvent; ex=" + ex);
			}
		}

		/// <summary>
		/// 波形ビューのリロードが要求されたとき発生するイベント．
		/// GeneralEventArgsの引数は，トラック番号,waveファイル名,開始時刻(秒),終了時刻(秒)が格納されたObject[]配列
		/// 開始時刻＞終了時刻の場合は，partialではなく全体のリロード要求
		/// </summary>
		public static event WaveViewRealoadRequiredEventHandler WaveViewReloadRequired;

		#endregion

		#region serialize configuration

		public static void serializeEditorConfig (EditorConfig instance, string file)
		{
			FileStream fs = null;
			try {
				fs = new FileStream (file, FileMode.Create, FileAccess.Write);
				EditorConfig.getSerializer ().serialize (fs, instance);
			} catch (Exception ex) {
				Logger.write (typeof(EditorConfig) + ".serialize; ex=" + ex + "\n");
			} finally {
				if (fs != null) {
					try {
						fs.Close ();
					} catch (Exception ex2) {
						Logger.write (typeof(EditorConfig) + ".serialize; ex=" + ex2 + "\n");
					}
				}
			}
		}

		public static EditorConfig deserializeEditorConfig (string file)
		{
			EditorConfig ret = null;
			FileStream fs = null;
			try {
				fs = new FileStream (file, FileMode.Open, FileAccess.Read);
				ret = (EditorConfig)EditorConfig.getSerializer ().deserialize (fs);
			} catch (Exception ex) {
				Logger.write (typeof(EditorConfig) + ".deserialize; ex=" + ex + "\n");
			} finally {
				if (fs != null) {
					try {
						fs.Close ();
					} catch (Exception ex2) {
						Logger.write (typeof(EditorConfig) + ".deserialize; ex=" + ex2 + "\n");
					}
				}
			}

			if (ret == null) {
				return null;
			}

			if (EditorManager.MainWindow != null) {
				var defs = EditorManager.MainWindow.getDefaultShortcutKeys ();
				foreach (var def in defs) {
					bool found = false;
					for (int i = 0; i < ret.ShortcutKeys.Count; i++) {
						if (def.Key.Equals (ret.ShortcutKeys [i].Key)) {
							found = true;
							break;
						}
					}
					if (!found) {
						ret.ShortcutKeys.Add (def);
					}
				}
			}

			// バッファーサイズを正規化
			if (ApplicationGlobal.appConfig.BufferSizeMilliSeconds < cadencii.core.EditorConfig.MIN_BUFFER_MILLIXEC) {
				ApplicationGlobal.appConfig.BufferSizeMilliSeconds = cadencii.core.EditorConfig.MIN_BUFFER_MILLIXEC;
			} else if (cadencii.core.EditorConfig.MAX_BUFFER_MILLISEC < ApplicationGlobal.appConfig.BufferSizeMilliSeconds) {
				ApplicationGlobal.appConfig.BufferSizeMilliSeconds = cadencii.core.EditorConfig.MAX_BUFFER_MILLISEC;
			}
			return ret;
		}

		/// <summary>
		/// 現在の設定を設定ファイルに書き込みます。
		/// </summary>
		public static void saveConfig ()
		{
			// ユーザー辞書の情報を取り込む
			EditorManager.editorConfig.UserDictionaries.Clear ();
			int count = SymbolTable.getCount ();
			for (int i = 0; i < count; i++) {
				SymbolTable table = SymbolTable.getSymbolTable (i);
				EditorManager.editorConfig.UserDictionaries.Add (table.getName () + "\t" + (table.isEnabled () ? "T" : "F"));
			}
			EditorManager.editorConfig.KeyWidth = keyWidth;

			// FIXME: Enable this
/*
            // chevronの幅を保存
            if (Rebar.CHEVRON_WIDTH > 0) {
				EditorManager.editorConfig.ChevronWidth = Rebar.CHEVRON_WIDTH;
            }
            */

			// シリアライズして保存
			string file = Path.Combine (Utility.getConfigPath (), ApplicationGlobal.CONFIG_FILE_NAME);
#if DEBUG
			sout.println ("EditorManager#saveConfig; file=" + file);
#endif
			try {
				serializeEditorConfig (EditorManager.editorConfig, file);
			} catch (Exception ex) {
				serr.println ("EditorManager#saveConfig; ex=" + ex);
				Logger.write (typeof(EditorManager) + ".saveConfig; ex=" + ex + "\n");
			}
		}

		/// <summary>
		/// 設定ファイルを読み込みます。
		/// 設定ファイルが壊れていたり存在しない場合、デフォルトの設定が使われます。
		/// </summary>
		public static void loadConfig ()
		{
			string appdata = cadencii.core.ApplicationGlobal.getApplicationDataPath ();
#if DEBUG
			sout.println ("EditorManager#loadConfig; appdata=" + appdata);
#endif
			if (appdata.Equals ("")) {
				EditorManager.editorConfig = new EditorConfig ();
				return;
			}

			// バージョン番号付きのファイル
			string config_file = Path.Combine (Utility.getConfigPath (), ApplicationGlobal.CONFIG_FILE_NAME);
#if DEBUG
			sout.println ("EditorManager#loadConfig; config_file=" + config_file);
#endif
			EditorConfig ret = null;
			if (System.IO.File.Exists (config_file)) {
				// このバージョン用の設定ファイルがあればそれを利用
				try {
					ret = deserializeEditorConfig (config_file);
				} catch (Exception ex) {
					serr.println ("EditorManager#loadConfig; ex=" + ex);
					ret = null;
					Logger.write (typeof(EditorManager) + ".loadConfig; ex=" + ex + "\n");
				}
			} else {
				// このバージョン用の設定ファイルがなかった場合
				// まず，古いバージョン用の設定ファイルがないかどうか順に調べる
				string[] dirs0 = PortUtil.listDirectories (appdata);
				// 数字と，2個以下のピリオドからなるディレクトリ名のみを抽出
				List<VersionString> dirs = new List<VersionString> ();
				foreach (string s0 in dirs0) {
					string s = PortUtil.getFileName (s0);
					int length = PortUtil.getStringLength (s);
					bool register = true;
					int num_period = 0;
					for (int i = 0; i < length; i++) {
						char c = PortUtil.charAt (s, i);
						if (c == '.') {
							num_period++;
						} else {
							if (!char.IsNumber (c)) {
								register = false;
								break;
							}
						}
					}
					if (register && num_period <= 2) {
						try {
							VersionString vs = new VersionString (s);
							dirs.Add (vs);
						} catch (Exception ex) {
						}
					}
				}

				// 並べ替える
				bool changed = true;
				int size = dirs.Count;
				while (changed) {
					changed = false;
					for (int i = 0; i < size - 1; i++) {
						VersionString item1 = dirs [i];
						VersionString item2 = dirs [i + 1];
						if (item1.compareTo (item2) > 0) {
							dirs [i] = item2;
							dirs [i + 1] = item1;
							changed = true;
						}
					}
				}

				// バージョン番号付きの設定ファイルを新しい順に読み込みを試みる
				VersionString vs_this = new VersionString (BAssemblyInfo.fileVersionMeasure + "." + BAssemblyInfo.fileVersionMinor);
				for (int i = size - 1; i >= 0; i--) {
					VersionString vs = dirs [i];
					if (vs_this.compareTo (vs) < 0) {
						// 自分自身のバージョンより新しいものは
						// 読み込んではいけない
						continue;
					}
					config_file = Path.Combine (Path.Combine (appdata, vs.getRawString ()), ApplicationGlobal.CONFIG_FILE_NAME);
					if (System.IO.File.Exists (config_file)) {
						try {
							ret = deserializeEditorConfig (config_file);
						} catch (Exception ex) {
							Logger.write (typeof(EditorManager) + ".loadConfig; ex=" + ex + "\n");
							ret = null;
						}
						if (ret != null) {
							break;
						}
					}
				}

				// それでも読み込めなかった場合，旧来のデフォルトの位置にある
				// 設定ファイルを読みに行く
				if (ret == null) {
					config_file = Path.Combine (appdata, ApplicationGlobal.CONFIG_FILE_NAME);
					if (System.IO.File.Exists (config_file)) {
						try {
							ret = deserializeEditorConfig (config_file);
						} catch (Exception ex) {
							serr.println ("EditorManager#locdConfig; ex=" + ex);
							ret = null;
							Logger.write (typeof(EditorManager) + ".loadConfig; ex=" + ex + "\n");
						}
					}
				}
			}

			// 設定ファイルの読み込みが悉く失敗した場合，
			// デフォルトの設定とする．
			if (ret == null) {
				ret = new EditorConfig ();
			}
			EditorManager.editorConfig = ret;

			keyWidth = EditorManager.editorConfig.KeyWidth;
		}

		#endregion

		/// <summary>
		/// 位置クオンタイズ時の音符の最小単位を、クロック数に換算したものを取得します
		/// </summary>
		/// <returns></returns>
		public static int getPositionQuantizeClock ()
		{
			return QuantizeModeUtil.getQuantizeClock (EditorManager.editorConfig.getPositionQuantize (), EditorManager.editorConfig.isPositionQuantizeTriplet ());
		}

		/// <summary>
		/// 音符長さクオンタイズ時の音符の最小単位を、クロック数に換算したものを取得します
		/// </summary>
		/// <returns></returns>
		public static int getLengthQuantizeClock ()
		{
			return QuantizeModeUtil.getQuantizeClock (EditorManager.editorConfig.getLengthQuantize (), EditorManager.editorConfig.isLengthQuantizeTriplet ());
		}

		/// <summary>
		/// utauVoiceDBフィールドのリストを一度クリアし，
		/// editorConfig.Utausingersの情報を元に最新の情報に更新します
		/// </summary>
		public static void reloadUtauVoiceDB ()
		{
			UtauWaveGenerator.mUtauVoiceDB.Clear ();
			foreach (var config in ApplicationGlobal.appConfig.UtauSingers) {
				// 通常のUTAU音源
				UtauVoiceDB db = null;
				try {
					db = new UtauVoiceDB (config);
				} catch (Exception ex) {
					serr.println ("AppManager#reloadUtauVoiceDB; ex=" + ex);
					db = null;
					Logger.write (typeof(EditorManager) + ".reloadUtauVoiceDB; ex=" + ex + "\n");
				}
				if (db != null) {
					UtauWaveGenerator.mUtauVoiceDB [config.VOICEIDSTR] = db;
				}
			}
		}

		public static bool IsPreviewRepeatMode { get; set; }
	}
}

