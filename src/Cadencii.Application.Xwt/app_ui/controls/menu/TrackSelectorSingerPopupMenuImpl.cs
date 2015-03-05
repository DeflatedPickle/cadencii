/*
 * TrackSelectorSingerPopupMenu.cs
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
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;
using cadencii;
using Cadencii.Gui;

namespace Cadencii.Application.Controls
{

	public class TrackSelectorSingerPopupMenuImpl : ContextMenuStripImpl, TrackSelectorSingerPopupMenu
    {
		public bool SingerChangeExists { get; set; }
		public int Clock { get; set; }
		public int InternalID { get; set; }

        public TrackSelectorSingerPopupMenuImpl(System.ComponentModel.IContainer cont)
            : base(cont)
        {
        }

		Cadencii.Gui.Point TrackSelectorSingerPopupMenu.PointToScreen (Cadencii.Gui.Point source)
		{
			return PointToScreen (source.ToWF ()).ToGui ();
		}
    }

}
