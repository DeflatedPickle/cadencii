/*
 * ITextWriter.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of Cadencii.Media.Vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;

namespace Cadencii.Media.Vsq
{
    public interface ITextWriter
    {
        void write(string value);
        void writeLine(string value);
        void close();
        void newLine();
    }

}
