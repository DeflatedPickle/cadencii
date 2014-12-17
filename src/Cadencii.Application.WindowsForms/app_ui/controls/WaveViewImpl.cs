/*
 * WaveView.cs
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
using Cadencii.Gui;
using Cadencii.Media;

using cadencii.core;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Media;
using Cadencii.Application.Models;
 


namespace Cadencii.Application.Controls
{

    /// <summary>
    /// トラック16個分の波形描画コンテキストを保持し、それらの描画を行うコンポーネントです。
    /// </summary>
    public class WaveViewImpl : UserControlImpl, WaveView
    {
		public WaveViewImpl ()
		{
			model = new WaveViewModel (this);
		}

		WaveViewModel model;

		public WaveViewModel Model {
			get { return model; }
		}

		/// <summary>
		/// オーバーライドされます。
		/// <seealso cref="M:System.Windows.Forms.Control.OnPaint"/>
		/// </summary>
		/// <param name="e"></param>
		public override void OnPaint(PaintEventArgs e)
		{
			model.paint(e.Graphics);
		}
    }

}
