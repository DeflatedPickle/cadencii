/*
 * EditorManager.cs
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
using Cadencii.Gui;
using cadencii.java.util;
using Cadencii.Media;
using Cadencii.Media.Vsq;
using Cadencii.Xml;
using cadencii.utau;

using Keys = Cadencii.Gui.Toolkit.Keys;
using DialogResult = Cadencii.Gui.DialogResult;

namespace cadencii
{

    class RunGeneratorQueue
    {
        public WaveGenerator generator;
        public long samples;
    }

}
