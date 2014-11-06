using System;
using System.Linq;
using Cadencii.Gui;

namespace cadencii
{
	public interface UiToolStripMenuItem : UiToolStripDropDownItem
	{
		Image Image {
			get;
			set;
		}

		CheckState CheckState { get; set; }
		string ShortcutKeyDisplayString { get; set; }
		ToolStripItemDisplayStyle DisplayStyle { get; set; }
		ToolStripItemImageScaling ImageScaling { get; set; }
		TextImageRelation TextImageRelation { get; set; }
		Keys ShortcutKeys { get; set; }
		bool Checked { get; set; }
		bool CheckOnClick { get; set; }
		void Mnemonic (Keys keys);
		event EventHandler CheckedChanged;
	}
}

