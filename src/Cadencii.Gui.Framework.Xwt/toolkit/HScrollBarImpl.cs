using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class HScrollBarImpl
	{
		int Cadencii.Gui.Toolkit.UiHScrollBar.SmallChange {
			get { return (int) this.StepIncrement; }
			set { this.StepIncrement = value; }
		}

		int Cadencii.Gui.Toolkit.UiHScrollBar.LargeChange {
			get { return this.PageIncrement; }
			set { this.PageIncrement = value; }
		}

		int Cadencii.Gui.Toolkit.UiHScrollBar.Value {
			get { return (int) this.Value; }
			set { this.Value = value; }
		}

		int Cadencii.Gui.Toolkit.UiHScrollBar.Maximum {
			get { return (int) this.UpperValue; }
			set { this.UpperValue = value; }
		}

		int Cadencii.Gui.Toolkit.UiHScrollBar.Minimum {
			get { return (int)LowerValue; }
			set { LowerValue = value; }
		}
	}
}

