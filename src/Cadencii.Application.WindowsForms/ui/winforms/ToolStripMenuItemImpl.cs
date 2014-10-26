using System;

namespace cadencii
{
	public class ToolStripMenuItemImpl : System.Windows.Forms.ToolStripMenuItem, UiToolStripMenuItem
	{
		public ToolStripMenuItemImpl ()
		{
		}

		public ToolStripMenuItemImpl (string s, object o, EventHandler e)
			: base (s, o, e)
		{
		}
	}
}

