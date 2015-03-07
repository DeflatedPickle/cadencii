/*
 * LyricTextBox.cs
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

using cadencii;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
{
    /// <summary>
    /// 歌詞入力用のテキストボックス
    /// </summary>
    public class LyricTextBoxImpl : TextBoxImpl, LyricTextBox
    {
		public event EventHandler<KeyPressEventArgs> KeyPress {
			add { base.KeyPress += (sender, e) => value (sender, new KeyPressEventArgs (e.KeyChar) { Handled = e.Handled }); }
			remove { throw new NotImplementedException (); }
		}

		public bool IsPhoneticSymbolEditMode { get; set; }

		public string BufferText { get; set; }

        /// <summary>
        /// オーバーライド．(Tab)または(Tab+Shift)も入力キーとみなすようオーバーライドされている
        /// </summary>
        /// <param name="keyData">キーの値の一つ</param>
        /// <returns>指定されているキーが入力キーである場合は true．それ以外の場合は false．</returns>
		//
		// FIXME: cannot implement in XWT. Not sure if it is even necessary. 
		/*
		protected override bool IsInputKey(System.Windows.Forms.Keys keyData)
        {
            switch (keyData) {
			case System.Windows.Forms.Keys.Tab:
			case (System.Windows.Forms.Keys) (Keys.Tab | Keys.Shift):
				break;
			default:
				return base.IsInputKey((System.Windows.Forms.Keys) keyData);
            }
            return true;
        }
        */
    }
}
