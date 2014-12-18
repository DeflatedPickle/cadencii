using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
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

		decimal Increment { get; set; }
	}
}

