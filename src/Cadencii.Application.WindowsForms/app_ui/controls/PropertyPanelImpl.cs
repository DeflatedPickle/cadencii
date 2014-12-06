#if ENABLE_PROPERTY
/*
 * PropertyPanel.cs
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
using cadencii.java.util;
using Cadencii.Media.Vsq;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using cadencii;
using Cadencii.Application.Models;

namespace Cadencii.Application.Controls
{

	public class PropertyPanelImpl : UserControlImpl, PropertyPanel
    {
		event CommandExecuteRequiredEventHandler PropertyPanel.CommandExecuteRequired {
			add { model.CommandExecuteRequired += value; }
			remove { model.CommandExecuteRequired -= value; }
		}

		bool PropertyPanel.isEditing ()
		{
			return model.isEditing ();
		}

		void PropertyPanel.updateValue (int selected)
		{
			model.updateValue (selected);
		}

		PropertyPanelModel model;

        public PropertyPanelImpl()
        {
			model = new PropertyPanelModel (this);
        }
    }

}
#endif
