using System;
using System.IO;
using cadencii.core;
using System.Collections.Generic;
using System.Reflection;

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
	}
}

