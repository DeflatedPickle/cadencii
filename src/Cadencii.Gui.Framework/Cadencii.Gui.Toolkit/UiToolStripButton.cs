using System;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiToolStripButton : UiToolStripItem
	{
		Color ImageTransparentColor {
			get;
			set;
		}

		bool CheckOnClick { get; set; }

		bool Checked { get; set; }
		event EventHandler CheckedChanged;
		Image Image { get; set; }
	}
}

