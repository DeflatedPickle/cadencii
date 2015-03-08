/*
 * BitmapEx.cs
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
using Xwt;
using Xwt.Drawing;
using System.IO;
using Cadencii.Gui;

namespace Cadencii.Application.Controls
{

    unsafe class BitmapExImpl : BitmapEx, IDisposable
    {
        private BitmapImage m_base;

        public void Dispose ()
        {
            m_base.Dispose ();
        }

        public Cadencii.Gui.Color GetPixel(int x, int y)
        {
			return m_base.GetPixel (x, y).ToGui ();
        }

        public BitmapExImpl(Cadencii.Gui.Image original)
        {
			m_base = ((Xwt.Drawing.Image) original.NativeImage).ToBitmap (ImageFormat.ARGB32);
        }
    }

}
