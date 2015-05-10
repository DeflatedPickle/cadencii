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
using System.Linq;
using System.Collections.Generic;
using cadencii.apputil;
using Cadencii.Gui;
using Cadencii.Media.Vsq;
using Keys = Cadencii.Gui.Toolkit.Keys;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
{
    class DraggableBButtonImpl : ButtonImpl, DraggableBButton
    {
		void DraggableBButton.DoDragDrop (IconDynamicsHandle handle, Cadencii.Gui.Toolkit.DragDropEffects all)
		{
			// FIXME: implement.
		}

		public DialogResult DialogResult { get; set; }
		public IconDynamicsHandle IconHandle { get; set; }
    }

}
