using System;
using cadencii.java.awt;

namespace cadencii
{
	public abstract class Dialogs
	{
		public abstract bool ShowDialogTo (object formWorker, object mainWindow);
		public abstract void BeforeShowDialog ();
		public abstract void AfterShowDialog ();
		public abstract bool IsShowingDialog { get; }
		public abstract DialogResult ShowModalFileDialog (object fileDialog, bool openMode, object mainForm);
		public abstract DialogResult ShowModalFolderDialog (object folderBrowserDialog, object mainForm);
		public abstract int ShowModalDialog (UiBase dialog, object parentForm);
		public abstract DialogResult ShowModalDialog (object dialog, object parentForm);
		public abstract DialogResult ShowMessageBox(string text, string caption, int optionType, int messageType);
	}
}
