using System;
using cadencii.java.awt;

namespace cadencii
{
	public abstract class Dialogs
	{
		public abstract bool ShowDialogTo (FormWorker formWorker, object mainWindow);
		public abstract bool IsShowingDialog { get; }
		public abstract DialogResult ShowModalFileDialog (UiFileDialog fileDialog, bool openMode, UiForm mainForm);
		public abstract DialogResult ShowModalFolderDialog (UiFolderBrowserDialog folderBrowserDialog, UiForm mainForm);
		public abstract int ShowModalDialog (UiForm dialog, object parentForm);
		public abstract DialogResult ShowModalDialog (object dialog, object parentForm);
		public abstract DialogResult ShowMessageBox(string text, string caption, int optionType, int messageType);
	}
}
