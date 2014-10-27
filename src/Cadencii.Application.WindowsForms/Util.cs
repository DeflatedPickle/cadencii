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
        public static readonly string PANGRAM = "cozy lummox gives smart squid who asks for job pen. 01234567890 THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS.";
        /// <summary>
        /// このクラスのメソッド'applyFontRecurse', 'applyToolStripFontRecurse', 'applyContextMenuFontRecurse'の呼び出しを有効とするかどうか。
        /// デフォルトではtrue
        /// </summary>
        public static bool isApplyFontRecurseEnabled = true;

        public static void applyContextMenuFontRecurse(UiContextMenuStrip item, cadencii.java.awt.Font font)
        {
            if (!isApplyFontRecurseEnabled) {
                return;
            }
            item.Font = (System.Drawing.Font) font.NativeFont;
            foreach (ToolStripItem tsi in item.Items) {
                applyToolStripFontRecurse(tsi, font);
            }
        }

        public static void applyToolStripFontRecurse(UiToolStripItem item, cadencii.java.awt.Font font)
        {
            if (!isApplyFontRecurseEnabled) {
                return;
            }
			item.Font = (System.Drawing.Font) font.NativeFont;
            if (item is ToolStripMenuItem) {
                ToolStripMenuItem tsmi = (ToolStripMenuItem)item;
                foreach (ToolStripItem tsi in tsmi.DropDownItems) {
                    applyToolStripFontRecurse(tsi, font);
                }
            } else if (item is ToolStripDropDownItem) {
                ToolStripDropDownItem tsdd = (ToolStripDropDownItem)item;
                foreach (ToolStripItem tsi in tsdd.DropDownItems) {
                    applyToolStripFontRecurse(tsi, font);
                }
            }
        }

        /// <summary>
        /// 指定したフォントを描画するとき、描画指定したy座標と、描かれる文字の中心線のズレを調べます
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public static int getStringDrawOffset(java.awt.Font font)
        {
            int ret = 0;
            java.awt.Dimension size = measureString(PANGRAM, font);
            if (size.Height <= 0) {
                return 0;
            }
            java.awt.Image b = null;
            java.awt.Graphics g = null;
            BitmapEx b2 = null;
            try {
                int string_desty = size.Height * 2; // 文字列が書き込まれるy座標
                int w = size.Width * 4;
                int h = size.Height * 4;
                b = new java.awt.Image(w, h);
                g = new java.awt.Graphics(b);
                g.setColor(java.awt.Colors.White);
                g.fillRect(0, 0, w, h);
                g.setFont(font);
                g.setColor(cadencii.java.awt.Colors.Black);
                g.drawString(PANGRAM, size.Width, string_desty);

                b2 = ApplicationUIHost.Create<BitmapEx> (b);
                // 上端に最初に現れる色つきピクセルを探す
                int firsty = 0;
                bool found = false;
                for (int y = 0; y < h; y++) {
                    for (int x = 0; x < w; x++) {
						var p = b2.GetPixel(x, y);
                        java.awt.Color c = new cadencii.java.awt.Color(p.R, p.G, p.B, p.A);
                        if (c.R != 255 || c.G != 255 || c.B != 255) {
                            found = true;
                            firsty = y;
                            break;
                        }
                    }
                    if (found) {
                        break;
                    }
                }

                // 下端
                int endy = h - 1;
                found = false;
                for (int y = h - 1; y >= 0; y--) {
                    for (int x = 0; x < w; x++) {
						var p = b2.GetPixel(x, y);
                        java.awt.Color c = new cadencii.java.awt.Color(p.R, p.G, p.B, p.A);
                        if (c.R != 255 || c.G != 255 || c.B != 255) {
                            found = true;
                            endy = y;
                            break;
                        }
                    }
                    if (found) {
                        break;
                    }
                }

                int center = (firsty + endy) / 2;
                ret = center - string_desty;
            } catch (Exception ex) {
                serr.println("Util#getStringDrawOffset; ex=" + ex);
            } finally {
                if (b != null) {
                    b.Dispose();
                }
                if (g != null) {
                    g.Dispose();
                }
                if (b2 != null && b2 != null) {
                    b2.Dispose();
                }
            }
            return ret;
        }

        /// <summary>
        /// 指定された文字列を指定されたフォントで描画したときのサイズを計測します。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static java.awt.Dimension measureString(string text, java.awt.Font font)
        {
            using (Bitmap dumy = new Bitmap(1, 1))
            using (Graphics g = Graphics.FromImage(dumy)) {
				SizeF tmp = g.MeasureString(text, (System.Drawing.Font) font.NativeFont);
                return new java.awt.Dimension((int)tmp.Width, (int)tmp.Height);
            }
        }

        public static java.awt.Dimension measureString(string text, Font font)
        {
            using (Bitmap dumy = new Bitmap(1, 1))
            using (Graphics g = Graphics.FromImage(dumy)) {
                SizeF tmp = g.MeasureString(text, font);
                return new java.awt.Dimension((int)tmp.Width, (int)tmp.Height);
            }
        }

        /// <summary>
        /// 指定したコントロールと、その子コントロールのフォントを再帰的に変更します
        /// </summary>
        /// <param name="c"></param>
        /// <param name="font"></param>
        public static void applyFontRecurse(UiControl c, cadencii.java.awt.Font font)
        {
            if (!isApplyFontRecurseEnabled) {
                return;
            }
            c.Font = (Font) font.NativeFont;
            for (int i = 0; i < c.Controls.Count; i++) {
                applyFontRecurse(c.Controls[i], font);
            }
        }
    }

}
