using System;
using Cadencii.Gui;
using System.Collections.Generic;

namespace cadencii
{
	public interface UiMenuItem
	{
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

