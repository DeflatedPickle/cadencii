using System;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface FormWorkerUi : UiForm
	{
		void setTitle(string title);
		void setText(string text);
		void close();
		void close(bool value);
		void show(object obj);
		bool showDialogTo(UiForm formMainWindow);
		void applyLanguage();
		void addProgressBar (ProgressBarWithLabelUi ui);
		void removeProgressBar (ProgressBarWithLabelUi progressBarWithLabelUi);
		void setTotalProgress (int i);
		void Refresh ();
	}
}

