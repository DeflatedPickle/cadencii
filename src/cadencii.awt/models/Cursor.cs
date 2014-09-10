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

	public class Cursor
	{
		public abstract class CursorAdapter : IDisposable
		{
			public abstract object NativeCursor { get; set; }
			public abstract int Type { get; }

			public abstract void Dispose ();
		}

		CursorAdapter a;

		public Cursor (int type)
		{
			a = AwtHost.Current.New<CursorAdapter> (type);
		}

		public const int CROSSHAIR_CURSOR = 1;
		public const int CUSTOM_CURSOR = -1;
		public const int DEFAULT_CURSOR = 0;
		public const int E_RESIZE_CURSOR = 11;
		public const int HAND_CURSOR = 12;
		public const int MOVE_CURSOR = 13;
		public const int N_RESIZE_CURSOR = 8;
		public const int NE_RESIZE_CURSOR = 7;
		public const int NW_RESIZE_CURSOR = 6;
		public const int S_RESIZE_CURSOR = 9;
		public const int SE_RESIZE_CURSOR = 5;
		public const int SW_RESIZE_CURSOR = 4;
		public const int TEXT_CURSOR = 2;
		public const int W_RESIZE_CURSOR = 10;
		public const int WAIT_CURSOR = 3;

		public object NativeCursor {
			get { return a.NativeCursor; }
			set { a.NativeCursor = value; }
		}

		public int getType()
		{
			return a.Type;
		}
	}

    /*public interface Image{
        int getHeight( object observer );
        int getWidth( object observer );
    }*/

}

namespace cadencii.java.awt.geom
{
    
}
