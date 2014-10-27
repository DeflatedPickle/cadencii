using System;
using cadencii.java.awt;

namespace cadencii
{
	public interface UiTrackBar : UiControl
	{
		TickStyle TickStyle {
			get;
			set;
		}

		int TickFrequency {
			get;
			set;
		}

		bool AutoSize {
			get;
			set;
		}

		int Maximum { get; set; }
		int Minimum { get; set; }
		int Value { get; set; }
		event EventHandler ValueChanged;
	}
}

