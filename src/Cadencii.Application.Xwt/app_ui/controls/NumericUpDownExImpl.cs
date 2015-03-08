/*
 * NumericUpDownEx.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
{
	public class NumericUpDownExImpl : Cadencii.Gui.Toolkit.NumericUpDownImpl, NumericUpDownEx
    {
		// FIXME: cannot support in Xwt.
		Cadencii.Gui.Toolkit.HorizontalAlignment NumericUpDownEx.TextAlign { get; set; }

        //private const long serialVersionUID = -4608658084088065812L;

        public NumericUpDownExImpl()
        {
            this.GotFocus += new EventHandler(NumericUpDownEx_GotFocus);
        }

        private void NumericUpDownEx_GotFocus(Object sender, EventArgs e)
        {
            //this.Select(0, 10);
        }

		// FIXME: dunno if it is needed for Xwt
		/*
		protected override void OnMouseScrolled (Xwt.MouseScrolledEventArgs e)
        {
            decimal new_val;
            if (e.Delta > 0) {
                new_val = this.Value + this.Increment;
            } else if (e.Delta < 0) {
                new_val = this.Value - this.Increment;
            } else {
                return;
            }
            if (this.Minimum <= new_val && new_val <= this.Maximum) {
                this.Value = new_val;
            }
        }
        */

		int NumericUpDownEx.DecimalPlaces {
			get { return base.Digits; }
			set { base.Digits = value; }
		}

		decimal NumericUpDownEx.Value {
			get { return (decimal) base.Value; }
			set { base.Value = (double) value; }
		}

		decimal NumericUpDownEx.Minimum {
			get { return (decimal)base.MinimumValue; }
			set { base.MinimumValue = (double)value; }
		}

		decimal NumericUpDownEx.Maximum {
			get { return (decimal)base.MaximumValue; }
			set { base.MaximumValue = (double)value; }
		}

		decimal NumericUpDownEx.Increment {
			get { return (decimal) base.IncrementValue; }
			set { base.IncrementValue = (double)value; }
		}

		event EventHandler NumericUpDownEx.ValueChanged {
			add { ValueChanged += value; }
			remove { ValueChanged -= value; }
		}
    }
}
