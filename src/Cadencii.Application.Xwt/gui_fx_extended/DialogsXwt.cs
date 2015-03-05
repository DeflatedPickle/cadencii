using System;
using Xwt;
using Cadencii.Gui.Toolkit;
using cadencii;
using Cadencii.Utilities;

namespace Cadencii.Application.Forms
{
	public class DialogsXwt : Dialogs
	{
		/// <summary>
		/// ダイアログを表示中かどうか
		/// </summary>
		private bool mShowingDialog = false;

		#region implemented abstract members of Dialogs

		public override Cadencii.Gui.DialogResult ShowMessageBox (string text, string caption, Cadencii.Gui.Toolkit.MessageBoxButtons optionType, Cadencii.Gui.Toolkit.MessageBoxIcon messageType)
		{
			BeforeShowDialog ();
			var ret = (Cadencii.Gui.DialogResult) showMessageBoxCore (text, caption, optionType, messageType);
			AfterShowDialog ();
			return ret;
		}

		static Command showMessageBoxCore(string text, string caption, Cadencii.Gui.Toolkit.MessageBoxButtons optionType, Cadencii.Gui.Toolkit.MessageBoxIcon messageType)
		{
			Command ret = Command.Cancel;
			// FIXME: no support for icon

			switch (optionType) {
			case MessageBoxButtons.AbortRetryIgnore:
			case MessageBoxButtons.RetryCancel:
				throw new NotImplementedException ();
			case MessageBoxButtons.OK:
				MessageDialog.ShowMessage (caption, text);
				return Command.Ok;
			case MessageBoxButtons.OKCancel:
				return MessageDialog.AskQuestion (caption, text, new Command [] { Command.Ok, Command.Cancel });
			case MessageBoxButtons.YesNo:
				return MessageDialog.AskQuestion (caption, text, new Command [] { Command.Yes, Command.No });
			case MessageBoxButtons.YesNoCancel:
				return MessageDialog.AskQuestion (caption, text, new Command [] { Command.Yes, Command.No, Command.Cancel });
			}
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
			var ret = ((FileDialog) fileDialog.Native).Run ((Window) mainForm);
			AfterShowDialog ();
			return (Cadencii.Gui.DialogResult) ret;
		}
		public override Cadencii.Gui.DialogResult ShowModalFolderDialog (UiFolderBrowserDialog folderBrowserDialog, UiForm mainForm)
		{
			BeforeShowDialog ();
			var ret = ((SelectFolderDialog)folderBrowserDialog.Native).Run ((Window) mainForm);
			AfterShowDialog ();
			return (Cadencii.Gui.DialogResult) ret;
		}
		public override Cadencii.Gui.DialogResult ShowModalDialog (UiForm dialog, UiForm parentForm)
		{
			BeforeShowDialog ();
			var ret = ((Dialog) dialog).Run ((Window) parentForm);
			AfterShowDialog ();
			return (Cadencii.Gui.DialogResult) ret;
		}
		public override bool IsShowingDialog {
			get { return mShowingDialog; }
		}
		#endregion
	}
}

