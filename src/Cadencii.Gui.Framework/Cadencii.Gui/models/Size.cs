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

    public struct Size
    {
        public int Height;
        public int Width;

        public Size(int width_, int height_)
        {
            Width = width_;
            Height = height_;
        }
    }

}
