/*
 * FormIconPalette.cs
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
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using cadencii.apputil;
using Cadencii.Gui;
using cadencii.java.util;
using Cadencii.Media.Vsq;
using Keys = Cadencii.Gui.Toolkit.Keys;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
{
    class DraggableBButtonImpl : ButtonImpl, DraggableBButton
    {
		void DraggableBButton.DoDragDrop (IconDynamicsHandle handle, Cadencii.Gui.Toolkit.DragDropEffects all)
		{
			DoDragDrop (handle, (System.Windows.Forms.DragDropEffects) all);
		}

		public IconDynamicsHandle IconHandle { get; set; }
    }

}
