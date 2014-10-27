using System;

namespace cadencii
{
	public interface ProgressBarWithLabelUi : UiControl
	{
		int Progress { get; set; }
		string Text { get; set; }
	}
}

