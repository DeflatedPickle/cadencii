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

    public class Font
    {
        public const int PLAIN = 0;
        public const int ITALIC = 2;
        public const int BOLD = 1;

	        public abstract class FontAdapter
	        {
			public abstract object NativeFont { get; set; }

			public abstract void Dispose ();
			public abstract string getName();

			public abstract int getSize();

			public abstract float getSize2D();
        }

        FontAdapter a;

	public Font(object value)
        {
        	a = AwtHost.Current.New<FontAdapter> (value);
        }

        public Font (string name, int style, int size)
		{
			a = AwtHost.Current.New<FontAdapter> (name, style, size);
		}

		public object NativeFont {
			get { return a.NativeFont; }
			set { a.NativeFont = value; }
		}

		public void Dispose ()
		{
			a.Dispose ();
		}

        public string getName()
        {
            return a.getName ();
        }

        public int getSize()
        {
            return a.getSize();
        }

        public float getSize2D()
        {
            return a.getSize2D();
        }
    }
    
}
