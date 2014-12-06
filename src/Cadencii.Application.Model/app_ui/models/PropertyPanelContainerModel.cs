#if ENABLE_PROPERTY
/*
 * PropertyPanelContainer.cs
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

using Cadencii.Gui.Toolkit;
using cadencii;
using Cadencii.Application.Controls;

namespace Cadencii.Application.Models
{

	public class PropertyPanelContainerModel
	{
		public const int _TITLE_HEIGHT = 29;
		public event StateChangeRequiredEventHandler StateChangeRequired;

		PropertyPanelContainer control;

		public PropertyPanelContainerModel(PropertyPanelContainer control)
		{
			this.control = control;
		}

		int Width { get { return control.Width; } }
		int Height { get { return control.Height; } }

		public void panelTitle_MouseDoubleClick(Object sender, MouseEventArgs e)
		{
			handleRestoreWindow();
		}

		public void btnClose_Click(Object sender, EventArgs e)
		{
			handleClose();
		}

		public void btnWindow_Click(Object sender, EventArgs e)
		{
			handleRestoreWindow();
		}

		private void handleClose()
		{
			invokeStateChangeRequiredEvent(PanelState.Hidden);
		}

		private void handleRestoreWindow()
		{
			invokeStateChangeRequiredEvent(PanelState.Window);
		}

		private void invokeStateChangeRequiredEvent(PanelState state)
		{
			if (StateChangeRequired != null) {
				StateChangeRequired(this, state);
			}
		}
	}

}
#endif
