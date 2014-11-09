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

    [Serializable]
    public struct Rectangle : Shape
    {
        public int Height;
        public int Width;
        public int X;
        public int Y;


        	public int Left {
			get { return X; }
		}
		public int Right {
			get { return X + Width; }
		}
		public int Top {
			get { return Y; }
		}
		public int Bottom {
			get { return Y + Height; }
		}

        public Rectangle(int width_, int height_)
        {
            X = 0;
            Y = 0;
            Width = width_;
            Height = height_;
        }

        public Rectangle(int x_, int y_, int width_, int height_)
        {
            X = x_;
            Y = y_;
            Width = width_;
            Height = height_;
        }

        public Rectangle(Rectangle r)
        {
            X = r.X;
            Y = r.Y;
            Width = r.Width;
            Height = r.Height;
        }

        public bool Contains (Point p)
		{
			return X <= p.X && p.X < X + Width && Y <= p.Y && p.Y < Y + Height;
		}

        public override string ToString()
        {
            return "{x=" + X + ", y=" + Y + ", width=" + Width + ", height=" + Height + "}";
        }
    }

}
