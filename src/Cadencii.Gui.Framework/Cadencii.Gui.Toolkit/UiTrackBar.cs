using System;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiHTrackBar : UiTrackBar
	{
	}

	public interface UiVTrackBar : UiTrackBar
	{
	}

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

