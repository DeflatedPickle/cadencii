/*
 * awt.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.core.
 *
 * cadencii.core is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.core is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using cadencii.java.awt;
using cadencii.java.awt.geom;

namespace cadencii.java.awt
{

    [Serializable]
    public struct Color
    {
        /// <summary>
        /// 黒を表します。
        /// </summary>
        public static Color black = new Color(System.Drawing.Color.Black);
        /// <summary>
        /// 黒を表します。
        /// </summary>
        public static Color BLACK = new Color(System.Drawing.Color.Black);
        /// <summary>
        /// 青を表します。
        /// </summary>
        public static Color blue = new Color(System.Drawing.Color.Blue);
        /// <summary>
        /// 青を表します。
        /// </summary>
        public static Color BLUE = new Color(System.Drawing.Color.Blue);
        /// <summary>
        /// シアンを表します。
        /// </summary>
        public static Color cyan = new Color(System.Drawing.Color.Cyan);
        /// <summary>
        /// シアンを表します。
        /// </summary>
        public static Color CYAN = new Color(System.Drawing.Color.Cyan);
        /// <summary>
        /// ダークグレイを表します。
        /// </summary>
        public static Color DARK_GRAY = new Color(System.Drawing.Color.DarkGray);
        /// <summary>
        /// ダークグレイを表します。
        /// </summary>
        public static Color darkGray = new Color(System.Drawing.Color.DarkGray);
        /// <summary>
        /// グレイを表します。
        /// </summary>
        public static Color gray = new Color(System.Drawing.Color.Gray);
        /// <summary>
        /// グレイを表します。
        /// </summary>
        public static Color GRAY = new Color(System.Drawing.Color.Gray);
        /// <summary>
        /// 緑を表します。
        /// </summary>
        public static Color green = new Color(System.Drawing.Color.Green);
        /// <summary>
        /// 緑を表します。
        /// </summary>
        public static Color GREEN = new Color(System.Drawing.Color.Green);
        /// <summary>
        /// ライトグレイを表します。
        /// </summary>
        public static Color LIGHT_GRAY = new Color(System.Drawing.Color.LightGray);
        /// <summary>
        /// ライトグレイを表します。
        /// </summary>
        public static Color lightGray = new Color(System.Drawing.Color.LightGray);
        /// <summary>
        /// マゼンタを表します。
        /// </summary>
        public static Color magenta = new Color(System.Drawing.Color.Magenta);
        /// <summary>
        /// マゼンタを表します。
        /// </summary>
        public static Color MAGENTA = new Color(System.Drawing.Color.Magenta);
        /// <summary>
        /// オレンジを表します。
        /// </summary>
        public static Color orange = new Color(System.Drawing.Color.Orange);
        /// <summary>
        /// オレンジを表します。
        /// </summary>
        public static Color ORANGE = new Color(System.Drawing.Color.Orange);
        /// <summary>
        /// ピンクを表します。
        /// </summary>
        public static Color pink = new Color(System.Drawing.Color.Pink);
        /// <summary>
        /// ピンクを表します。
        /// </summary>
        public static Color PINK = new Color(System.Drawing.Color.Pink);
        /// <summary>
        /// 赤を表します。
        /// </summary>
        public static Color red = new Color(System.Drawing.Color.Red);
        /// <summary>
        /// 赤を表します。
        /// </summary>
        public static Color RED = new Color(System.Drawing.Color.Red);
        /// <summary>
        /// 白を表します。
        /// </summary>
        public static Color white = new Color(System.Drawing.Color.White);
        /// <summary>
        /// 白を表します。
        /// </summary>
        public static Color WHITE = new Color(System.Drawing.Color.White);
        /// <summary>
        /// 黄を表します。
        /// </summary>
        public static Color yellow = new Color(System.Drawing.Color.Yellow);
        /// <summary>
        /// 黄を表します。 
        /// </summary>
        public static Color YELLOW = new Color(System.Drawing.Color.Yellow);

        System.Drawing.Color color;

        public System.Drawing.Color ToNative ()
		{
			return color;
		}

        Color(System.Drawing.Color value)
        {
            color = value;
        }

        public Color(int r, int g, int b)
        {
            color = System.Drawing.Color.FromArgb(r, g, b);
        }

        public Color(int r, int g, int b, int a)
        {
            color = System.Drawing.Color.FromArgb(a, r, g, b);
        }

        public int getRed()
        {
            return color.R;
        }

        public int getGreen()
        {
            return color.G;
        }

        public int getBlue()
        {
            return color.B;
        }
    }

}

namespace cadencii.java.awt.geom
{
    public class Area : Shape
    {
        System.Drawing.Region region;

        public Area()
        {
            region = new System.Drawing.Region();
            region.MakeEmpty();
        }

        public Area(Shape s)
        {
            if (s == null) {
                region = new System.Drawing.Region();
            } else if (s is Area) {
                Area a = (Area)s;
                if (a.region == null) {
                    region = new System.Drawing.Region();
                } else {
                    region = (System.Drawing.Region)a.region.Clone();
                }
            } else if (s is Rectangle) {
                Rectangle rc = (Rectangle)s;
                region = new System.Drawing.Region(new System.Drawing.Rectangle(rc.x, rc.y, rc.width, rc.height));
            } else {
                serr.println(
                    "fixme: org.kbinani.java.awt.Area#.ctor(org.kbinani.java.awt.Shape); type of argument s is not supported for '" +
                    s.GetType() + "'.");
                region = new System.Drawing.Region();
            }
        }

		public object NativeRegion {
			get { return region; }
			set { region = (System.Drawing.Region) value; }
		}

        public void add(Area rhs)
        {
            if (rhs == null) {
                return;
            }
            if (rhs.region == null) {
                return;
            }
            if (region == null) {
                region = new System.Drawing.Region();
            }
            region.Union(rhs.region);
        }

        public void subtract(Area rhs)
        {
            if (rhs == null) {
                return;
            }
            if (rhs.region == null) {
                return;
            }
            if (region == null) {
                region = new System.Drawing.Region();
            }
            region.Exclude(rhs.region);
        }

        public Object clone()
        {
            Area ret = new Area();
            if (region == null) {
                ret.region = new System.Drawing.Region();
            } else {
                ret.region = (System.Drawing.Region)region.Clone();
            }
            return ret;
        }
    }
}
