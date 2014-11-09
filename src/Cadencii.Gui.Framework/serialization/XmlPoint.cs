/*
 * XmlPoint.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of Cadencii.Xml.
 *
 * cadencii.xml is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.xml is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System.Xml.Serialization;
using Cadencii.Gui;

namespace Cadencii.Xml
{

    public class XmlPoint
    {
        [XmlIgnore]
        public int x;
        [XmlIgnore]
        public int y;

        public XmlPoint()
        {
        }

        public XmlPoint(int x_, int y_)
        {
            x = x_;
            y = y_;
        }

        public XmlPoint(Point p)
        {
            x = p.X;
            y = p.Y;
        }

        public Point toPoint()
        {
            return new Point(x, y);
        }

        public int getX()
        {
            return x;
        }

        public void setX(int value)
        {
            x = value;
        }

        public int getY()
        {
            return y;
        }

        public void setY(int value)
        {
            y = value;
        }

        public int X
        {
            get
            {
                return getX();
            }
            set
            {
                setX(value);
            }
        }

        public int Y
        {
            get
            {
                return getY();
            }
            set
            {
                setY(value);
            }
        }

    }

}
