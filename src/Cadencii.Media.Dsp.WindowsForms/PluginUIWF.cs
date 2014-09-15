/*
 * FormPluginUi.cs
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
using cadencii.java.awt;

#if ENABLE_AQUESTONE
using System;
using System.Windows.Forms;

namespace cadencii
{

	class PluginUIWF : PluginUI
	{
		public PluginUIWF ()
		{
			UI = new FormPluginUi ();
		}

		public FormPluginUi UI { get; set; }

		public override bool IsOpened {
			get { return UI.IsOpened; }  
			set { UI.IsOpened = value; }
		}

		public override bool IsDisposed {
			get { return UI.IsDisposed; }
		}

		public override bool Visible {
			get { return UI.Visible; }
			set { UI.Visible = value; }
		}

		public override void InvalidateUI ()
		{
			UI.invalidateUi ();
		}
	}

}
#endif
