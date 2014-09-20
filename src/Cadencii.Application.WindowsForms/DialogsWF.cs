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
		public override bool ShowDialogTo (object formWorker, object mainWindow)
		{
			BeforeShowDialog ();
			bool ret = ((FormWorker) formWorker).getUi ().showDialogTo ((FormMain) mainWindow);
#if DEBUG
			sout.println ("AppManager#patchWorkToFreeze; showDialog returns " + ret);
#endif
			AfterShowDialog ();

			return ret;
		}
		/// <summary>
		/// モーダルなダイアログを出すために，プロパティウィンドウとミキサーウィンドウの「最前面に表示」設定を一時的にOFFにします
		/// </summary>
		public override void BeforeShowDialog ()
		{
			mShowingDialog = true;
#if ENABLE_PROPERTY
			if (DialogManager.propertyWindow != null) {
				bool previous = DialogManager.propertyWindow.getUi ().isAlwaysOnTop ();
				DialogManager.propertyWindow.setPreviousAlwaysOnTop (previous);
				if (previous) {
					DialogManager.propertyWindow.getUi ().setAlwaysOnTop (false);
				}
			}
#endif
			if (DialogManager.mMixerWindow != null) {
				bool previous = DialogManager.mMixerWindow.TopMost;
				DialogManager.mMixerWindow.setPreviousAlwaysOnTop (previous);
				if (previous) {
					DialogManager.mMixerWindow.TopMost = false;
				}
			}

			if (DialogManager.iconPalette != null) {
				bool previous = DialogManager.iconPalette.TopMost;
				DialogManager.iconPalette.setPreviousAlwaysOnTop (previous);
				if (previous) {
					DialogManager.iconPalette.TopMost = false;
				}
			}
		}
		/// <summary>
		/// beginShowDialogで一時的にOFFにした「最前面に表示」設定を元に戻します
		/// </summary>
		public override void AfterShowDialog ()
		{
			#if ENABLE_PROPERTY
			if (DialogManager.propertyWindow != null) {
				DialogManager.propertyWindow.getUi ().setAlwaysOnTop (DialogManager.propertyWindow.getPreviousAlwaysOnTop ());
			}
#endif
			if (DialogManager.mMixerWindow != null) {
				DialogManager.mMixerWindow.TopMost = DialogManager.mMixerWindow.getPreviousAlwaysOnTop ();
			}

			if (DialogManager.iconPalette != null) {
				DialogManager.iconPalette.TopMost = DialogManager.iconPalette.getPreviousAlwaysOnTop ();
			}

			try {
				if (DialogManager.MainWindowFocusRequired != null) {
					DialogManager.MainWindowFocusRequired.Invoke (typeof(AppManager), new EventArgs ());
				}
			} catch (Exception ex) {
				Logger.write (typeof(AppManager) + ".endShowDialog; ex=" + ex + "\n");
				sout.println (typeof(AppManager) + ".endShowDialog; ex=" + ex);
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

