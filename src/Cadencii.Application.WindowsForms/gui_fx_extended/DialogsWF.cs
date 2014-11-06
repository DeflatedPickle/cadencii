using System;
using System.Windows.Forms;

namespace cadencii
{
	public class DialogsWF : Dialogs
	{
		/// <summary>
		/// ダイアログを表示中かどうか
		/// </summary>
		private bool mShowingDialog = false;

		#region implemented abstract members of Dialogs

		public override Cadencii.Gui.DialogResult ShowMessageBox (string text, string caption, int optionType, int messageType)
		{
			BeforeShowDialog ();
			var ret = (Cadencii.Gui.DialogResult) cadencii.windows.forms.Utility.showMessageBox (text, caption, optionType, messageType);
			AfterShowDialog ();
			return ret;
		}

		public override bool ShowDialogTo (FormWorker formWorker, object mainWindow)
		{
			BeforeShowDialog ();
			bool ret = formWorker.getUi ().showDialogTo ((FormMain) mainWindow);
#if DEBUG
			sout.println ("EditorManager#patchWorkToFreeze; showDialog returns " + ret);
#endif
			AfterShowDialog ();

			return ret;
		}
		/// <summary>
		/// モーダルなダイアログを出すために，プロパティウィンドウとミキサーウィンドウの「最前面に表示」設定を一時的にOFFにします
		/// </summary>
		void BeforeShowDialog ()
		{
			mShowingDialog = true;
#if ENABLE_PROPERTY
			if (EditorManager.propertyWindow != null) {
				bool previous = EditorManager.propertyWindow.getUi ().isAlwaysOnTop ();
				EditorManager.propertyWindow.setPreviousAlwaysOnTop (previous);
				if (previous) {
					EditorManager.propertyWindow.getUi ().setAlwaysOnTop (false);
				}
			}
#endif
			if (EditorManager.MixerWindow != null) {
				bool previous = EditorManager.MixerWindow.TopMost;
				EditorManager.MixerWindow.setPreviousAlwaysOnTop (previous);
				if (previous) {
					EditorManager.MixerWindow.TopMost = false;
				}
			}

			if (EditorManager.iconPalette != null) {
				bool previous = EditorManager.iconPalette.TopMost;
				EditorManager.iconPalette.setPreviousAlwaysOnTop (previous);
				if (previous) {
					EditorManager.iconPalette.TopMost = false;
				}
			}
		}
		/// <summary>
		/// beginShowDialogで一時的にOFFにした「最前面に表示」設定を元に戻します
		/// </summary>
		void AfterShowDialog ()
		{
			#if ENABLE_PROPERTY
			if (EditorManager.propertyWindow != null) {
				EditorManager.propertyWindow.getUi ().setAlwaysOnTop (EditorManager.propertyWindow.getPreviousAlwaysOnTop ());
			}
#endif
			if (EditorManager.MixerWindow != null) {
				EditorManager.MixerWindow.TopMost = EditorManager.MixerWindow.getPreviousAlwaysOnTop ();
			}

			if (EditorManager.iconPalette != null) {
				EditorManager.iconPalette.TopMost = EditorManager.iconPalette.getPreviousAlwaysOnTop ();
			}

			try {
				if (EditorManager.MainWindowFocusRequired != null) {
					EditorManager.MainWindowFocusRequired.Invoke (typeof(EditorManager), new EventArgs ());
				}
			} catch (Exception ex) {
				Logger.write (typeof(EditorManager) + ".endShowDialog; ex=" + ex + "\n");
				sout.println (typeof(EditorManager) + ".endShowDialog; ex=" + ex);
			}
			mShowingDialog = false;
		}
		public override Cadencii.Gui.DialogResult ShowModalFileDialog (UiFileDialog fileDialog, bool openMode, UiForm mainForm)
		{
			BeforeShowDialog ();
			var ret = ((FileDialog) fileDialog.Native).ShowDialog ((Form) mainForm);
			AfterShowDialog ();
			return (Cadencii.Gui.DialogResult) ret;
		}
		public override Cadencii.Gui.DialogResult ShowModalFolderDialog (UiFolderBrowserDialog folderBrowserDialog, UiForm mainForm)
		{
			BeforeShowDialog ();
			var ret = ((FolderBrowserDialog) folderBrowserDialog.Native).ShowDialog ((Form) mainForm);
			AfterShowDialog ();
			return (Cadencii.Gui.DialogResult) ret;
		}
		public override int ShowModalDialog (UiForm dialog, object parentForm)
		{
			BeforeShowDialog ();
			int ret = dialog.showDialog (parentForm);
			AfterShowDialog ();
			return ret;
		}
		public override Cadencii.Gui.DialogResult ShowModalDialog (object dialog, object parentForm)
		{
			BeforeShowDialog ();
			var ret = ((Form) dialog).ShowDialog ((Form) parentForm);
			AfterShowDialog ();
			return (Cadencii.Gui.DialogResult)ret;
		}
		public override bool IsShowingDialog {
			get { return mShowingDialog; }
		}
		#endregion
	}
}

