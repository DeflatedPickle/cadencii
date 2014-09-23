/*
 * FormMixer.cs
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
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using cadencii.apputil;
using cadencii.java.awt;
using cadencii.java.util;
using cadencii.vsq;
using cadencii.windows.forms;
using Keys = cadencii.java.awt.Keys;

namespace cadencii
{
	public class FormUiBase : Form, UiBase
 	{
		event EventHandler UiBase.LocationChanged {
			add { this.LocationChanged += (object sender, EventArgs e) => value (sender, e); }
			remove { this.LocationChanged -= (object sender, EventArgs e) => value (sender, e); }
		}

		cadencii.java.awt.Point UiBase.Location {
			get { return new cadencii.java.awt.Point (Location.X, Location.Y); }
			set { Location = new System.Drawing.Point (value.X, value.Y); }
		}

		event EventHandler UiBase.FormClosing {
			add { this.FormClosing += (object sender, FormClosingEventArgs e) => value (sender, e); }
			remove { this.FormClosing -= (object sender, FormClosingEventArgs e) => value (sender, e); }
		}

		int UiBase.showDialog (object parent_form)
		{
			return ShowDialog ((IWin32Window) parent_form) == System.Windows.Forms.DialogResult.OK ? 1 : 0;
		}
 	}

}
