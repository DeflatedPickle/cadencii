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
using Cadencii.Gui;
using Cadencii.Gui.geom;
using System.Linq;

namespace Cadencii.Gui
{

    public class Polygon
    {
        /// <summary>
        /// 点の総数です。
        /// </summary>
        public int npoints;
        /// <summary>
        /// X 座標の配列です。
        /// </summary>
        public int[] xpoints;
        /// <summary>
        /// Y 座標の配列です。
        /// </summary>
        public int[] ypoints;

        public Polygon()
        {
            npoints = 0;
            xpoints = new int[0];
            ypoints = new int[0];
        }

        public Polygon(int[] xpoints_, int[] ypoints_, int npoints_)
        {
            npoints = npoints_;
            xpoints = xpoints_;
            ypoints = ypoints_;
        }

        public Polygon(Point[] points)
        {
                npoints = points.Length;
                xpoints = points.Select (p => p.X).ToArray ();
		xpoints = points.Select (p => p.Y).ToArray ();
        }
    }

}
