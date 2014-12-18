using System;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;

namespace Cadencii.Application.Forms
{
	public interface FormWorkerUi : UiForm
	{
		void setTitle(string title);
		void setText(string text);
		bool showDialogTo(UiForm formMainWindow);
		void applyLanguage();
		void addProgressBar (ProgressBarWithLabel ui);
		void removeProgressBar (ProgressBarWithLabel progressBarWithLabelUi);
		void setTotalProgress (int i);
		void Refresh ();
	}
}

