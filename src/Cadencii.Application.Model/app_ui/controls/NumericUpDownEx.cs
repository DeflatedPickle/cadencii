using System;
using Cadencii.Gui;

namespace cadencii
{
	public interface NumericUpDownEx : UiControl
	{
		event EventHandler ValueChanged;

		int DecimalPlaces { get; set; }

		HorizontalAlignment TextAlign {
			get;
			set;
		}

		decimal Value {
			get;
			set;
		}

		decimal Minimum {
			get;
			set;
		}

		decimal Maximum {
			get;
			set;
		}
	}
}

