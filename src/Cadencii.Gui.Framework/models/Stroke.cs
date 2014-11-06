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

namespace Cadencii.Gui
{

	public class Stroke
	{
		public abstract class StrokeAdapter
		{
			public abstract object NativePen { get; set; }
		}

		public const int CAP_BUTT = 0;
		public const int CAP_ROUND = 1;
		public const int CAP_SQUARE = 2;
		public const int JOIN_BEVEL = 2;
		public const int JOIN_MITER = 0;
		public const int JOIN_ROUND = 1;

		StrokeAdapter a;

		public object NativePen { get { return a.NativePen; } set { a.NativePen = value; } }

		public Stroke()
		{
			a = AwtHost.Current.New<StrokeAdapter> ();
		}

		public Stroke(float width)
		{
			a = AwtHost.Current.New<StrokeAdapter> (width);
		}

		public Stroke(float width, int cap, int join)
		{
			a = AwtHost.Current.New<StrokeAdapter> (width, cap, join);
		}

		public Stroke(float width, int cap, int join, float miterlimit)
		{
			a = AwtHost.Current.New<StrokeAdapter> (width, cap, join, miterlimit);
		}

		public Stroke (float width, int cap, int join, float miterlimit, float[] dash, float dash_phase)
		{
			a = AwtHost.Current.New<StrokeAdapter> (width, cap, join, miterlimit, dash, dash_phase);
		}
	}

}
