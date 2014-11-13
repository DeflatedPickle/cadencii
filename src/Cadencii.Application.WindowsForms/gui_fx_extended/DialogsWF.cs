using System;
using System.Windows.Forms;
using Cadencii.Gui.Toolkit;
using cadencii;

namespace Cadencii.Application.Forms
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
			var ret = (Cadencii.Gui.DialogResult) showMessageBoxCore (text, caption, optionType, messageType);
			AfterShowDialog ();
			return ret;
		}

		static DialogResult showMessageBoxCore(string text, string caption, int optionType, int messageType)
		{
			System.Windows.Forms.DialogResult ret = System.Windows.Forms.DialogResult.Cancel;
			System.Windows.Forms.MessageBoxButtons btn = System.Windows.Forms.MessageBoxButtons.OK;
			if (optionType == Dialog.MSGBOX_YES_NO_CANCEL_OPTION) {
				btn = System.Windows.Forms.MessageBoxButtons.YesNoCancel;
			} else if (optionType == Dialog.MSGBOX_YES_NO_OPTION) {
				btn = System.Windows.Forms.MessageBoxButtons.YesNo;
			} else if (optionType == Dialog.MSGBOX_OK_CANCEL_OPTION) {
				btn = System.Windows.Forms.MessageBoxButtons.OKCancel;
			} else {
				btn = System.Windows.Forms.MessageBoxButtons.OK;
			}

			System.Windows.Forms.MessageBoxIcon icon = System.Windows.Forms.MessageBoxIcon.None;
			if (messageType == Dialog.MSGBOX_ERROR_MESSAGE) {
				icon = System.Windows.Forms.MessageBoxIcon.Error;
			} else if (messageType == Dialog.MSGBOX_INFORMATION_MESSAGE) {
				icon = System.Windows.Forms.MessageBoxIcon.Information;
			} else if (messageType == Dialog.MSGBOX_PLAIN_MESSAGE) {
				icon = System.Windows.Forms.MessageBoxIcon.None;
			} else if (messageType == Dialog.MSGBOX_QUESTION_MESSAGE) {
				icon = System.Windows.Forms.MessageBoxIcon.Question;
			} else if (messageType == Dialog.MSGBOX_WARNING_MESSAGE) {
				icon = System.Windows.Forms.MessageBoxIcon.Warning;
			}

			return System.Windows.Forms.MessageBox.Show(text, caption, btn, icon);
		}

		public override bool ShowDialogTo (FormWorker formWorker, UiForm mainWindow)
		{
			BeforeShowDialog ();
			bool ret = formWorker.getUi ().showDialogTo ((FormMainImpl) mainWindow);
#if DEBUG
			Logger.StdOut ("EditorManager#patchWorkToFreeze; showDialog returns " + ret);
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
				Logger.StdOut (typeof(EditorManager) + ".endShowDialog; ex=" + ex);
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
		public override Cadencii.Gui.DialogResult ShowModalDialog (UiForm dialog, UiForm parentForm)
		{
			BeforeShowDialog ();
			var ret = ((Form) dialog).ShowDialog ((Form) parentForm);
			AfterShowDialog ();
			return (Cadencii.Gui.DialogResult) ret;
		}
		public override bool IsShowingDialog {
			get { return mShowingDialog; }
		}
		#endregion
	}
}

