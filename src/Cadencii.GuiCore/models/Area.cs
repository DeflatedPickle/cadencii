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

namespace cadencii.java.awt.geom
{
    public class Area : Shape
    {
	public abstract class AreaAdapter
	{
		public abstract object NativeRegion { get; set; }
	        public abstract void add(Area rhs);
	        public abstract void subtract(Area rhs);
	        public abstract Area clone();
	}

    	AreaAdapter a;

		public AreaAdapter Adapter {
			get { return a; }
		}

        public Area()
        {
        	a = AwtHost.Current.New<AreaAdapter> ();
        }

        public Area(Shape s)
        {
		a = AwtHost.Current.New<AreaAdapter> (s);
        }

		public object NativeRegion {
			get { return a.NativeRegion; }
		set { a.NativeRegion = value; }
		}

        public void add(Area rhs)
        {
        	a.add(rhs);
        }

        public void subtract(Area rhs)
        {
		a.subtract(rhs);
        }

        public Object clone()
        {
		return a.clone();
        }
    }
}