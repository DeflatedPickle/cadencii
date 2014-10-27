using System;

namespace cadencii
{
	public interface UiHScrollBar : UiControl
	{
		event EventHandler ValueChanged;

		int SmallChange {
			get;
			set;
		}

		int LargeChange {
			get;
			set;
		}

		int Value {
			get;
			set;
		}

		int Maximum {
			get;
			set;
		}

		int Minimum {
			get;
			set;
		}
	}
}

