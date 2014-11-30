using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
{
	public interface LyricTextBox : UiTextBox
	{
		bool IsPhoneticSymbolEditMode { get; set; }

		string BufferText { get; set; }

		PictPianoRoll ParentPianoRoll {
			get;
			set;
		}
	}
}

