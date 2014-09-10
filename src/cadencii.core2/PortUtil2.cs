/*
 * PortUtil.cs
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
using System.IO;
using System.Text;
using cadencii.java.awt;
using cadencii.java.util;

namespace cadencii.core2
{
    public static class PortUtil
    {
		
        public const int YES_OPTION = 0;
        public const int NO_OPTION = 1;
        public const int CANCEL_OPTION = 2;
        public const int OK_OPTION = 0;
        public const int CLOSED_OPTION = -1;

        public static Rectangle getScreenBounds(System.Windows.Forms.Control w)
        {
            System.Drawing.Rectangle rc = System.Windows.Forms.Screen.GetWorkingArea(w);
            return new Rectangle(rc.X, rc.Y, rc.Width, rc.Height);
        }

		
        public static void setMousePosition(Point p)
        {
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(p.X, p.Y);
        }

        public static Point getMousePosition()
        {
            System.Drawing.Point p = System.Windows.Forms.Control.MousePosition;
            return new Point(p.X, p.Y);
        }

        /// <summary>
        /// 指定した点が，コンピュータの画面のいずれかに含まれているかどうかを調べます
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool isPointInScreens(Point p)
        {
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens) {
                System.Drawing.Rectangle rc = screen.WorkingArea;
                if (rc.X <= p.X && p.X <= rc.X + rc.Width) {
                    if (rc.Y <= p.Y && p.Y <= rc.Y + rc.Height) {
                        return true;
                    }
                }
            }
            return false;
        }

        public static Rectangle getWorkingArea(System.Windows.Forms.Form w)
        {
            System.Drawing.Rectangle r = System.Windows.Forms.Screen.GetWorkingArea(w);
            return new Rectangle(r.X, r.Y, r.Width, r.Height);
        }
		
        /// <summary>
        /// java:コンポーネントのnameプロパティを返します。C#:コントロールのNameプロパティを返します。
        /// objがnullだったり、型がComponent/Controlでない場合は空文字を返します。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string getComponentName(Object obj)
        {
            if (obj == null) {
                return "";
            }
            if (obj is System.Windows.Forms.Control) {
                return ((System.Windows.Forms.Control)obj).Name;
            } else {
                return "";
            }
        }
    }
}
