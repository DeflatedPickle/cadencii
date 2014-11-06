using System;
using cadencii.apputil;
using System.IO;
using System.Linq;

namespace cadencii
{
	public partial class FormMainModel
	{
		public class HelpMenuModel
		{
			readonly FormMainModel parent;

			public HelpMenuModel (FormMainModel parent)
			{
				this.parent = parent;
			}
			//BOOKMARK: menuHelp

			#region menuHelp

			public void RunHelpAboutCommand ()
			{
				#if DEBUG
				sout.println ("FormMain#menuHelpAbout_Click");
				#endif

				string version_str = EditorManager.getVersion () + "\n\n" +
				                     string.Join ("\n",
					                     AppDomain.CurrentDomain.GetAssemblies ()
							.Where (a => a.GetName ().Name.StartsWith ("Cadencii."))
							.Select (a => a.GetTypes ().First ())
							.Select (t => EditorManager.getAssemblyNameAndFileVersion (t)));
				if (parent.form.mVersionInfo == null) {
					parent.form.mVersionInfo = ApplicationUIHost.Create<VersionInfo> (FormMainModel.ApplicationName, version_str);
					parent.form.mVersionInfo.setAuthorList (_CREDIT);
					parent.form.mVersionInfo.Show ();
				} else {
					if (parent.form.mVersionInfo.IsDisposed) {
						parent.form.mVersionInfo = ApplicationUIHost.Create<VersionInfo> (FormMainModel.ApplicationName, version_str);
						parent.form.mVersionInfo.setAuthorList (_CREDIT);
					}
					parent.form.mVersionInfo.Show ();
				}
			}

			public void RunHelpDebugCommand ()
			{
				#if DEBUG
				int.Parse ("X");
				sout.println ("FormMain#menuHelpDebug_Click");
				#endif
			}

			public void RunHelpManualCommand ()
			{
				// 現在のUI言語と同じ版のマニュアルファイルがあるかどうか探す
				string lang = Messaging.getLanguage ();
				string pdf = Path.Combine (PortUtil.getApplicationStartupPath (), "manual_" + lang + ".pdf");
				if (!System.IO.File.Exists (pdf)) {
					// 無ければ英語版のマニュアルを表示することにする
					pdf = Path.Combine (PortUtil.getApplicationStartupPath (), "manual_en.pdf");
				}
				if (!System.IO.File.Exists (pdf)) {
					DialogManager.showMessageBox (
						_ ("file not found"),
						FormMainModel.ApplicationName,
						cadencii.Dialog.MSGBOX_DEFAULT_OPTION,
						cadencii.Dialog.MSGBOX_WARNING_MESSAGE);
					return;
				}
				System.Diagnostics.Process.Start (pdf);
			}

			public void RunHelpLogSwitchCheckedChanged ()
			{
				Logger.setEnabled (parent.form.menuHelpLogSwitch.Checked);
				if (parent.form.menuHelpLogSwitch.Checked) {
					parent.form.menuHelpLogSwitch.Text = _ ("Enabled");
				} else {
					parent.form.menuHelpLogSwitch.Text = _ ("Disabled");
				}
			}

			public void RunHelpLogOpenCommand ()
			{
				string file = Logger.getPath ();
				if (file == null || (file != null && (!System.IO.File.Exists (file)))) {
					// ログがまだできてないのでダイアログ出す
					DialogManager.showMessageBox (
						_ ("Log file has not generated yet."),
						_ ("Info"),
						cadencii.java.awt.AwtHost.OK_OPTION,
						cadencii.Dialog.MSGBOX_INFORMATION_MESSAGE);
					return;
				}

				// ログファイルを開く
				try {
					System.Diagnostics.Process.Start (file);
				} catch (Exception ex) {
					Logger.write (GetType () + ".menuHelpLogOpen_Click; ex=" + ex + "\n");
				}
			}

			#endregion

		}
	}
}
