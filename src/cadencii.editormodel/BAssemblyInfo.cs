/*
 * BAssemblyInfo.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace cadencii
{

    public class BAssemblyInfo
    {
        public const string id = "$Id$";
        public const string fileVersionMeasure = "3";
        public const string fileVersionMinor = "5";
        public const string fileVersion = fileVersionMeasure + "." + fileVersionMinor + ".4";
        public const string downloadUrl = "http://sourceforge.jp/projects/cadencii/releases/59580/" + "Cadencii_v" + fileVersion;
    }

}
