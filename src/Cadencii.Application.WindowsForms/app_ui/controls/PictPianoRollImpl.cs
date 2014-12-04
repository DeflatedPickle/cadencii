/*
 * PictPianoRoll.cs
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
using System;
using System.Collections.Generic;
using cadencii.apputil;
using Cadencii.Gui;
using cadencii.java.util;
using Cadencii.Media.Vsq;

using cadencii.core;
using Cadencii.Utilities;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Forms;
using Cadencii.Application.Media;
using cadencii;
using Cadencii.Application.Drawing;
using Cadencii.Application.Models;

namespace Cadencii.Application.Controls
{
    /// <summary>
    /// ピアノロール用のコンポーネント
    /// </summary>
	public class PictPianoRollImpl : PictureBoxImpl, PictPianoRoll
    {
		PictPianoRollModel model;

        public PictPianoRollImpl()
        {
			model = new PictPianoRollModel (this);
		}

		public void setMainForm (FormMain formMain)
		{
			model.setMainForm (formMain);
		}

		public MouseTracer mMouseTracer {
			get { return model.mMouseTracer; }
			set { model.mMouseTracer = value; }
		}
    }
}
