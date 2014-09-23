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
    public struct Color
    {
        int r, g, b, a;

        public Color(int r, int g, int b)
        {
        	this.r = r;
        	this.g = g;
        	this.b = b;
        	this.a = 0xFF;
        }

        public Color(int r, int g, int b, int a)
        {
		this.r = r;
        	this.g = g;
        	this.b = b;
        	this.a = a;
        }

        public int R { get { return r; } }
        public int G { get { return g; } }
        public int B { get { return b; } }
	public int A { get { return a; } }
    }
}
