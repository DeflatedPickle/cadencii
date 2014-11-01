using System;

namespace cadencii
{
	public interface UiToolStripContainer : UiControl
	{
		bool TopToolStripPanelVisible {
			get;
			set;
		}

		string Text {
			get;
			set;
		}

		bool RightToolStripPanelVisible {
			get;
			set;
		}

		bool LeftToolStripPanelVisible {
			get;
			set;
		}

		// maybe it should be UiToolStripContentPanel
		UiToolStripPanel ContentPanel { get; }

		UiToolStripPanel BottomToolStripPanel { get; }
	}
}

