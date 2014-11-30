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
using System.Windows.Forms;

using cadencii;

namespace Cadencii.Application.Controls
{
    /// <summary>
    /// 歌詞入力用のテキストボックス
    /// </summary>
    public class LyricTextBoxImpl : Cadencii.Gui.Toolkit.TextBoxImpl, LyricTextBox
    {
		PictPianoRoll LyricTextBox.ParentPianoRoll {
			get { return (PictPianoRoll) this.Parent; }
			set { Parent = (Control) value; }
		}

		public bool IsPhoneticSymbolEditMode { get; set; }

		public string BufferText { get; set; }

        /// <summary>
        /// オーバーライド．(Tab)または(Tab+Shift)も入力キーとみなすようオーバーライドされている
        /// </summary>
        /// <param name="keyData">キーの値の一つ</param>
        /// <returns>指定されているキーが入力キーである場合は true．それ以外の場合は false．</returns>
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData) {
                case Keys.Tab:
                case Keys.Tab | Keys.Shift:
                break;
                default:
                return base.IsInputKey(keyData);
            }
            return true;
        }
    }

}
