/*
 * Util.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.apputil.
 *
 * cadencii.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using cadencii;

namespace cadencii.apputil
{
    using java = cadencii.java;
    using WORD = System.UInt16;
    using DWORD = System.UInt32;
    using LONG = System.Int32;
    using BYTE = System.Byte;
    using HANDLE = System.IntPtr;
    using WCHAR = Char;
    using ULONG = System.UInt32;

    public static partial class Util
    {
        /// <summary>
        /// 指定したコントロールと、その子コントロールのフォントを再帰的に変更します
        /// </summary>
        /// <param name="c"></param>
        /// <param name="font"></param>
        public static void applyFontRecurse(UiControl c, Cadencii.Gui.Font font)
        {
			applyFontRecurseW ((Control)c, font);
        }

		/// <summary>
		/// 指定したコントロールと、その子コントロールのフォントを再帰的に変更します
		/// </summary>
		/// <param name="c"></param>
		/// <param name="font"></param>
		public static void applyFontRecurseW(Control c, Cadencii.Gui.Font font)
		{
			if (!Utility.isApplyFontRecurseEnabled) {
				return;
			}
			c.Font = (Font) font.NativeFont;
			foreach (Control cc in c.Controls) {
				applyFontRecurseW (cc, font);
			}
		}
    }

}
