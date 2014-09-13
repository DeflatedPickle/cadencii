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
 using System;

namespace cadencii
{
	public abstract class PluginUI
	{
		public abstract bool IsOpened { get; set; }
		public abstract bool IsDisposed { get; }
		public abstract bool Visible { get; set; }

		public abstract void InvalidateUI ();
	}

}
