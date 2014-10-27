using System;
using cadencii.java.awt;

namespace cadencii
{
	public interface UiToolStripButton : UiToolStripItem
	{
		bool Checked { get; set; }
		event EventHandler CheckedChanged;
		Image Image { get; set; }
	}
}

