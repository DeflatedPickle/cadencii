using System;
using cadencii.java.awt;

namespace cadencii
{
	public interface UiToolStripMenuItem : UiToolStripDropDownItem
	{
		CheckState CheckState { get; set; }
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

