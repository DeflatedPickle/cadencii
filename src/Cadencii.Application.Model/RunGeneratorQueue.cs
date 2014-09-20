/*
 * AppManager.cs
 * Copyright © 2009-2011 kbinani
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
//#define ENABLE_OBSOLUTE_COMMAND
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Threading;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.CSharp;
using cadencii.apputil;
using cadencii.java.awt;
using cadencii.java.io;
using cadencii.java.util;
using cadencii.media;
using cadencii.vsq;
using cadencii.xml;
using cadencii.utau;
using ApplicationGlobal = cadencii.core.ApplicationGlobal;
using Keys = cadencii.java.awt.Keys;
using DialogResult = cadencii.java.awt.DialogResult;

namespace cadencii
{

    class RunGeneratorQueue
    {
        public WaveGenerator generator;
        public long samples;
    }

}
