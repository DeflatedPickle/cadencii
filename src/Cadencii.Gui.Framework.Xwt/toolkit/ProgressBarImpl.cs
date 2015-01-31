using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class ProgressBarImpl : ProgressBarBase, UiProgressBar
	{
		// this doesn't take immediate effect. It supports only Value changes.
		int UiProgressBar.Minimum { get; set; }

		// this doesn't take immediate effect. It supports only Value changes.
		int UiProgressBar.Maximum { get; set; }

		double Delta {
			get {
				UiProgressBar c = this;
				return c.Maximum - c.Minimum;
			}
		}

		int UiProgressBar.Value {
			get {
				UiProgressBar c = this;
				return (int) (c.Minimum + (c.Maximum -c.Minimum) * base.Fraction);
			}
			set {
				UiProgressBar c = this;
				base.Fraction = (value - c.Minimum) / (c.Maximum - c.Minimum);
			}
		}
	}
}

