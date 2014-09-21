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

		public override cadencii.java.awt.DialogResult ShowMessageBox (string text, string caption, int optionType, int messageType)
		{
			BeforeShowDialog ();
			var ret = (cadencii.java.awt.DialogResult) cadencii.windows.forms.Utility.showMessageBox (text, caption, optionType, messageType);
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
			if (AppManager.propertyWindow != null) {
				bool previous = AppManager.propertyWindow.getUi ().isAlwaysOnTop ();
				AppManager.propertyWindow.setPreviousAlwaysOnTop (previous);
				if (previous) {
					AppManager.propertyWindow.getUi ().setAlwaysOnTop (false);
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

			if (AppManager.iconPalette != null) {
				bool previous = AppManager.iconPalette.TopMost;
				AppManager.iconPalette.setPreviousAlwaysOnTop (previous);
				if (previous) {
					AppManager.iconPalette.TopMost = false;
				}
			}
		}
		/// <summary>
		/// beginShowDialogで一時的にOFFにした「最前面に表示」設定を元に戻します
		/// </summary>
		void AfterShowDialog ()
		{
			#if ENABLE_PROPERTY
			if (AppManager.propertyWindow != null) {
				AppManager.propertyWindow.getUi ().setAlwaysOnTop (AppManager.propertyWindow.getPreviousAlwaysOnTop ());
			}
#endif
			if (EditorManager.MixerWindow != null) {
				EditorManager.MixerWindow.TopMost = EditorManager.MixerWindow.getPreviousAlwaysOnTop ();
			}

			if (AppManager.iconPalette != null) {
				AppManager.iconPalette.TopMost = AppManager.iconPalette.getPreviousAlwaysOnTop ();
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
		public override cadencii.java.awt.DialogResult ShowModalFileDialog (object fileDialog, bool openMode, object mainForm)
		{
			BeforeShowDialog ();
			var ret = ((FileDialog) fileDialog).ShowDialog ((Form) mainForm);
			AfterShowDialog ();
			return (cadencii.java.awt.DialogResult) ret;
		}
		public override cadencii.java.awt.DialogResult ShowModalFolderDialog (object folderBrowserDialog, object mainForm)
		{
			BeforeShowDialog ();
			var ret = ((FolderBrowserDialog) folderBrowserDialog).ShowDialog ((Form) mainForm);
			AfterShowDialog ();
			return (cadencii.java.awt.DialogResult) ret;
		}
		public override int ShowModalDialog (UiBase dialog, object parentForm)
		{
			BeforeShowDialog ();
			int ret = dialog.showDialog (parentForm);
			AfterShowDialog ();
			return ret;
		}
		public override cadencii.java.awt.DialogResult ShowModalDialog (object dialog, object parentForm)
		{
			BeforeShowDialog ();
			var ret = ((Form) dialog).ShowDialog ((Form) parentForm);
			AfterShowDialog ();
			return (cadencii.java.awt.DialogResult)ret;
		}
		public override bool IsShowingDialog {
			get { return mShowingDialog; }
		}
		#endregion
	}
}

