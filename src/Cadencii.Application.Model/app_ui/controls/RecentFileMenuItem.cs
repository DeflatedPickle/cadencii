using System;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface RecentFileMenuItem : UiToolStripMenuItem
	{
		string getFilePath();
	}
}

