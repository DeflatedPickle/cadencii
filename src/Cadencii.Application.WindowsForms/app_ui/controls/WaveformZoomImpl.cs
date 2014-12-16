/*
 * WaveformZoomUiImpl.cs
 * Copyright © 2011 kbinani
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
using System.Windows.Forms;

using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Models;
using Cadencii.Application.Forms;


namespace Cadencii.Application.Controls
{
    /// <summary>
    /// 波形表示の拡大・縮小を行うためのパネルです．
    /// </summary>
    class WaveformZoomImpl : UserControlImpl, WaveformZoom
    {
		WaveformZoomModel model;

		public WaveformZoomImpl (FormMain main, WaveView waveView)
		{
			model = new WaveformZoomModel (this, waveView);
		}
    }
}
