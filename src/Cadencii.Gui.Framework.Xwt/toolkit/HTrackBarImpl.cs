using System;

namespace Cadencii.Gui.Toolkit
{
	// HTrackBarImpl and VTrackBarImpl have the identical code. 
	public partial class HTrackBarImpl
	{
		// FIXME: no effect
		TickStyle UiTrackBar.TickStyle { get; set; }

		int UiTrackBar.TickFrequency {
			get { return (int) base.StepIncrement; }
			set { base.StepIncrement = value; }
		}

		bool UiTrackBar.AutoSize {
			get { return base.SnapToTicks; }
			set { base.SnapToTicks = value; }
		}

		int UiTrackBar.Maximum {
			get { return (int) base.MaximumValue; }
			set { base.MaximumValue = value; }
		}

		int UiTrackBar.Minimum {
			get { return (int) base.MinimumValue; }
			set { base.MinimumValue = value; }
		}

		int UiTrackBar.Value {
			get { return (int) base.Value; }
			set { base.Value = value; }
		}
	}
}

