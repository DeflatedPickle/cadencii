using System;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
{
	public interface BgmMenuItem : UiToolStripMenuItem
	{
		int BgmIndex { get; set; }
	}
}

