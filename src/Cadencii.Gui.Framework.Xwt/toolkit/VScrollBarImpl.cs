using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class VScrollBarImpl
	{
		int Cadencii.Gui.Toolkit.UiVScrollBar.SmallChange {
			get { return (int) this.StepIncrement; }
			set { this.StepIncrement = value; }
		}

		int Cadencii.Gui.Toolkit.UiVScrollBar.LargeChange {
			get { return this.PageIncrement; }
			set { this.PageIncrement = value; }
		}

		int Cadencii.Gui.Toolkit.UiVScrollBar.Value {
			get { return (int) this.Value; }
			set { this.Value = value; }
		}

		int Cadencii.Gui.Toolkit.UiVScrollBar.Maximum {
			get { return (int) this.UpperValue; }
			set { this.UpperValue = value; }
		}

		int Cadencii.Gui.Toolkit.UiVScrollBar.Minimum {
			get { return (int)LowerValue; }
			set { LowerValue = value; }
		}
	}
}

