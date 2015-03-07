#define COMPONENT_ENABLE_LOCATION
//#define MONITOR_FPS
//#define OLD_IMPL_MOUSE_TRACER
/*
 * TrackSelector.cs
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
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using cadencii;
using cadencii.apputil;
using Cadencii.Gui;
using Cadencii.Media.Vsq;

using cadencii.core;
using Cadencii.Utilities;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;

using Keys = Cadencii.Gui.Toolkit.Keys;
using NMouseButtons = Cadencii.Gui.Toolkit.MouseButtons;
using NMouseEventArgs = Cadencii.Gui.Toolkit.MouseEventArgs;
using NMouseEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.MouseEventArgs>;
using ToolStripRenderMode = Cadencii.Gui.Toolkit.ToolStripRenderMode;
using TS = Cadencii.Application.Controls.TrackSelectorConsts;
using TextFormatFlags = Cadencii.Gui.Toolkit.TextFormatFlags;
using Cadencii.Application;
using Cadencii.Application.Forms;
using Cadencii.Application.Models;
using Cadencii.Application.Media;
using Cadencii.Application.Drawing;

namespace Cadencii.Application.Controls
{
    using Graphics = Cadencii.Gui.Graphics;

    /// <summary>
    /// コントロールカーブ，トラックの一覧，歌手変更イベントなどを表示するコンポーネント．
    /// </summary>
	public class TrackSelectorImpl : UserControlImpl, TrackSelector
    {
		public TrackSelectorImpl()
		{
			this.Model = new TrackSelectorModel (this);
			Model.Initialize ();
		}

		public FormMain MainWindow {
			get { return (FormMain) ParentForm; }
		}

		public TrackSelectorModel Model { get; private set; }

		void TrackSelector.RequestFocus ()
		{
			Model.RequestFocus ();
		}

		void TrackSelector.OnMouseDown (object sender, NMouseEventArgs e)
		{
			Model.OnMouseDown (sender, e);
		}

		void TrackSelector.OnMouseUp (object sender, NMouseEventArgs e)
		{
			Model.OnMouseUp (sender, e);
		}

		protected override void OnDraw (Xwt.Drawing.Context ctx, Xwt.Rectangle dirtyRect)
		{
			Model.paint (ctx.ToGui ());
		}
    }
}
