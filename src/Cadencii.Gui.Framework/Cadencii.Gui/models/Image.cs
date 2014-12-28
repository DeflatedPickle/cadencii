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

namespace Cadencii.Gui
{
	public class Image : IDisposable
	{
		ImageAdapter a;

		public Image ()
		{
			a = GuiHost.Current.New<ImageAdapter> ();
		}

		public Image (int width, int height)
		{
			a = GuiHost.Current.New<ImageAdapter> (width, height);
		}

		public object NativeImage { get { return a.NativeImage; } set { a.NativeImage = value; } }

		public void Dispose ()
		{
			a.Dispose ();
		}

		public int Width {
			get { return a.Width; }
		}

		public int Height {
			get { return a.Height; }
		}

		public void Save (System.IO.Stream stream)
		{
			a.Save (stream);
		}

		public abstract class ImageAdapter : IDisposable
		{
			public abstract object NativeImage { get; set; }

			public abstract void Dispose ();

			public abstract int Width { get; }

			public abstract int Height { get; }

			public abstract void Save (System.IO.Stream stream);
		}
	}
}
