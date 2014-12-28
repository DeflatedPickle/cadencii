using System;
using System.Collections.Generic;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
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

		/*
		// maybe it should be UiToolStripContentPanel
		UiToolStripPanel ContentPanel { get; }

		UiToolStripPanel BottomToolStripPanel { get; }
		*/

		IList<UiControl> BottomToolStripPanel_Controls { get; }

		ToolStripRenderMode BottomToolStripPanel_RenderMode {
			get;
			set;
		}

		IList<UiControl> ContentPanel_Controls { get; }

		Size ContentPanel_Size {
			get;
			set;
		}

		void BottomToolStripPanel_ResumeLayout (bool b);

		void BottomToolStripPanel_PerformLayout ();

		void BottomToolStripPanel_SuspendLayout ();

		void ContentPanel_ResumeLayout (bool b);

		void ContentPanel_SuspendLayout ();
	}
}

