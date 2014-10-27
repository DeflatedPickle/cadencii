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
        public static Rectangle getScreenBounds(object nativeControl)
        {
		return Screen.Instance.getScreenBounds (nativeControl);
        }

		
        public static void setMousePosition(Point p)
        {
		Screen.Instance.setMousePosition (p);
        }

        public static Point getMousePosition()
        {
		return Screen.Instance.getMousePosition ();
        }

        /// <summary>
        /// 指定した点が，コンピュータの画面のいずれかに含まれているかどうかを調べます
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool isPointInScreens(Point p)
        {
            return Screen.Instance.isPointInScreens (p);
        }

        public static Rectangle getWorkingArea(object nativeWindow)
        {
        	return Screen.Instance.getWorkingArea (nativeWindow);
        }
		
        /// <summary>
        /// java:コンポーネントのnameプロパティを返します。C#:コントロールのNameプロパティを返します。
        /// objがnullだったり、型がComponent/Controlでない場合は空文字を返します。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string getComponentName(Object obj)
        {
        	return AwtHost.Current.getComponentName (obj);
        }
    }
}
