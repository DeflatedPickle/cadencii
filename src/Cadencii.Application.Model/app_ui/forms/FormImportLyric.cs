using System;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Forms
{
	public interface FormImportLyric : UiForm
	{
		void setMaxNotes (int notes);
		string[] Letters { get; }
		void Hide ();
	}
}

