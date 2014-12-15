/*
 * PictOverview.cs
 * Copyright © 2010-2011 kbinani
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
using System.Threading;
using System.Collections.Generic;
using Cadencii.Gui;
using cadencii.java.util;
using Cadencii.Media.Vsq;

using Consts = Cadencii.Application.Models.FormMainModel.Consts;
using cadencii.core;
using Cadencii.Utilities;
using Cadencii.Application.Controls;
using Cadencii.Gui.Toolkit;
using cadencii;
using Cadencii.Application.Forms;
using Cadencii.Application.Media;
using Cadencii.Application.Models;
using Cadencii.Application.Drawing;

namespace Cadencii.Application.Controls
{

    /// <summary>
    /// ナビゲーションバーを描画するコンポーネント
    /// </summary>
    public class PictOverviewImpl : PictureBoxImpl, PictOverview
    {
		void PictOverview.InvokeOnUiThread (Action action)
		{
			this.Invoke(action);
		}

		void PictOverview.updateCachedImage ()
		{
			model.updateCachedImage ();
		}

		void PictOverview.setMainForm (FormMain formMain)
		{
			model.setMainForm (formMain);
		}

		void IImageCachedComponentDrawer.draw (Graphics g, int width, int height)
		{
			model.draw (g, width, height);
		}

		PictOverviewModel model;

        public PictOverviewImpl()
        {
			DoubleBuffered = true;
			UserPaint = true;

			model = new PictOverviewModel (this);
        }

		public override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
			model.paint (pevent.Graphics);
        }
    }
}
