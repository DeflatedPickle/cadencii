using System;
using System.Collections.Generic;

namespace cadencii
{
	public static class WinformsExtensions
	{
		public static void Add<C> (this List<C> list, string s, object o, EventHandler h) where C : UiToolStripItem
		{
			list.Add (new ToolStripMenuItemImpl (s, o, h));
		}
	}
}

