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

    [Serializable]
    public struct Rectangle : Shape
    {
        public int height;
        public int width;
        public int x;
        public int y;

        public Rectangle(int width_, int height_)
        {
            x = 0;
            y = 0;
            width = width_;
            height = height_;
        }

        public Rectangle(int x_, int y_, int width_, int height_)
        {
            x = x_;
            y = y_;
            width = width_;
            height = height_;
        }

        public Rectangle(Rectangle r)
        {
            x = r.x;
            y = r.y;
            width = r.width;
            height = r.height;
        }

        public override string ToString()
        {
            return "{x=" + x + ", y=" + y + ", width=" + width + ", height=" + height + "}";
        }
    }

}

namespace cadencii.java.awt.geom
{
    
}
