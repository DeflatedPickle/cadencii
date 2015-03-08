/*
 * BSplitterPanel.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.apputil.
 *
 * cadencii.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.ComponentModel;
using cadencii;
using Cadencii.Gui.Toolkit;
using Cadencii.Gui;

namespace Cadencii.Application.Controls
{

    public class BSplitterPanelImpl : Cadencii.Gui.Toolkit.PanelImpl, BSplitterPanel
    {
        private BorderStyle m_border_style = BorderStyle.None;

        public event EventHandler BorderStyleChanged;

        public BSplitterPanelImpl()
            : base()
        {
			((UiPanel) this).AutoScroll = false;
			BorderColor = Colors.Black;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public Color BorderColor { get; set; }

		// FIXME: ignore so far.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public new BorderStyle BorderStyle { get; set; }
    }

}
