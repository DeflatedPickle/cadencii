/*
 * UiBase.cs
 * Copyright © 2011 kbinani
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

namespace cadencii
{

    using System;

    public interface UiBase
    {
    		bool TopMost { get; set; }
		bool Visible { get; set; }
		Point Location { get; set; }

		int showDialog(Object parent_form);

		event EventHandler FormClosing;

		event EventHandler LocationChanged;
    };

}
