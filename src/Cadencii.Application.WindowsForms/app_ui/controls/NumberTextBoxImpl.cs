/*
 * NumberTextBox.cs
 * Copyright © 2009-2011 kbinani
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
#define COMPONENT_ENABLE_LOCATION
using System;
using cadencii;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
{

    public class NumberTextBoxImpl : TextBoxImpl, NumberTextBox
    {
        private NumberTextBoxValueType m_value_type = NumberTextBoxValueType.Double;
        private Color m_textcolor_normal = Cadencii.Gui.Colors.Black;
		private Color m_textcolor_invalid = Cadencii.Gui.Colors.White;
		private Color m_backcolor_normal = Cadencii.Gui.Colors.White;
        private Color m_backcolor_invalid = new Color(240, 128, 128);

        /// <summary>
        /// IDEでのデザイン用
        /// </summary>
        public NumberTextBoxValueType Type
        {
            get
            {
                return getType();
            }
            set
            {
                setType(value);
            }
        }

        public NumberTextBoxValueType getType()
        {
            return m_value_type;
        }

        public void setType(NumberTextBoxValueType value)
        {
            m_value_type = value;
        }


        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            validateText();
        }

        private void validateText()
        {
            bool valid = false;
            string text = this.Text;
            if (m_value_type == NumberTextBoxValueType.Double) {
                double dou;
                try {
                    dou = double.Parse(text);
                    valid = true;
                } catch (Exception ex) {
                    valid = false;
                }
            } else if (m_value_type == NumberTextBoxValueType.Float) {
                float flo;
                try {
                    flo = (float)double.Parse(text);
                    valid = true;
                } catch (Exception ex) {
                    valid = false;
                }
            } else if (m_value_type == NumberTextBoxValueType.Integer) {
                int inte;
                try {
                    inte = int.Parse(text);
                    valid = true;
                } catch (Exception ex) {
                    valid = false;
                }
            }
            if (valid) {
				ForeColor = m_textcolor_normal.ToNative ();
				BackColor = m_backcolor_normal.ToNative ();
            } else {
				ForeColor = m_textcolor_invalid.ToNative ();
				BackColor = m_backcolor_invalid.ToNative ();
            }
        }

    }

}
