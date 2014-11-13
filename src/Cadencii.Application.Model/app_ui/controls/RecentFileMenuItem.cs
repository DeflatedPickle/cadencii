using System;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
{
	public interface RecentFileMenuItem : UiToolStripMenuItem
	{
		string getFilePath();
	}
}

