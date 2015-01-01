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
using cadencii;
using Cadencii.Gui.Toolkit;
using System.Linq;
using Cadencii.Utilities;

namespace Cadencii.Gui
{
	public class StrokeXwt
	{
		public enum LineCapXwt
		{
			Flat,
			Round,
			Square,
		}

		public enum LineJoinXwt
		{
			Miter,
			Round,
			Bevel,
		}

		public Xwt.Drawing.Color Color { get; set; }
		public float [] DashPattern { get; set; }
		public float DashOffset { get; set; }
		public float LineWidth { get; set; }
		// FIXME: cannot adjust those in Xwt.
		public LineCapXwt LineCap { get; set; }
		public LineJoinXwt LineJoin { get; set; }
		public float MiterLimit { get; set; }
	}
	
}
