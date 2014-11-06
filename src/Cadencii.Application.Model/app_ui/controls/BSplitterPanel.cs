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
using Cadencii.Gui;

namespace cadencii.apputil
{
	public interface BSplitterPanel : UiPanel
	{
		Color BorderColor { get; set; }
		event EventHandler BorderStyleChanged;
	}


}
