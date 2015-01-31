using System;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{

	public interface UiTextBox : UiControl
	{
		ImeMode ImeMode { get; set; }

		event EventHandler TextChanged;

		void SelectAll ();

		bool AcceptsReturn { get; set; }

		BorderStyle BorderStyle { get; set; }

		HorizontalAlignment TextAlign { get; set; }

		int SelectionStart { get; set; }

		string Text { get; set; }

		bool ReadOnly { get; set; }

		string [] Lines { get; }

		bool HideSelection { get; set; }

		int GetFirstCharIndexFromLine (int line);
	}
	
}
