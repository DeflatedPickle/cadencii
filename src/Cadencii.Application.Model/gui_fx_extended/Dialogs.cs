using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application
{
	public abstract class Dialogs
	{
		public abstract bool ShowDialogTo (FormWorker formWorker, UiForm mainWindow);
		public abstract bool IsShowingDialog { get; }
		public abstract DialogResult ShowModalFileDialog (UiFileDialog fileDialog, bool openMode, UiForm mainForm);
		public abstract DialogResult ShowModalFolderDialog (UiFolderBrowserDialog folderBrowserDialog, UiForm mainForm);
		public abstract DialogResult ShowModalDialog (UiForm dialog, UiForm parentForm);
		public abstract DialogResult ShowMessageBox(string text, string caption, int optionType, int messageType);
	}
}
