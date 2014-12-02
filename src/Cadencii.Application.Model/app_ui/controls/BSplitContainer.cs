using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
{

	public interface BSplitContainer : UiControl
	{
		Dimension MinimumSize {
			get;
			set;
		}

		string Text {
			get;
			set;
		}

		int SplitterWidth {
			get;
			set;
		}

		int SplitterDistance {
			get;
			set;
		}

		Orientation Orientation {
			get;
			set;
		}

		bool SplitterFixed {
			get;
			set;
		}

		bool Panel2Hidden {
			set;
		}

		int DividerLocation {
			get;
			set;
		}

		int Panel1MinSize {
			get;
			set;
		}

		int DividerSize {
			get;
			set;
		}

		bool Panel1Hidden {
			set;
		}

		FixedPanel FixedPanel { get; set; }

		//BorderStyle BorderStyle { get; set; }

		BSplitterPanel Panel1 { get; }

		BSplitterPanel Panel2 { get; }

		UiPictureBox Splitter { get; }

		int Panel2MinSize { get; set; }
	}
}

