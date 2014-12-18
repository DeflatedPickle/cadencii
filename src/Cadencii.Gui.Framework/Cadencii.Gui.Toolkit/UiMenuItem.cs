using System;
using Cadencii.Gui;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public interface UiMenuItem : IHasText
	{
		bool Enabled {
			get;
			set;
		}

		string Text {
			get;
			set;
		}

		int Index {
			get;
			set;
		}

		event EventHandler Click;

		bool Checked { get; set; }
	}

}

