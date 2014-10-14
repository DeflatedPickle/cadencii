using System;
using cadencii.java.awt;
using cadencii.vsq;

namespace cadencii
{

	public interface UiTextBox : UiControl
	{
		event EventHandler TextChanged;

		void SelectAll ();

		bool AcceptsReturn { get; set; }

		BorderStyle BorderStyle { get; set; }

		HorizontalAlignment TextAlign { get; set; }

		int SelectionStart { get; set; }

		string Text { get; set; }
	}
	
}