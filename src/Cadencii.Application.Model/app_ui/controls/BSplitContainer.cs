using System;
using Cadencii.Gui;

namespace cadencii.apputil
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

		void AddControl (UiControl child);

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

		int Panel2MinSize { get; set; }
	}
}

