using System;

namespace cadencii
{
	public interface FormImportLyric : UiForm
	{
		void setMaxNotes (int notes);
		string[] Letters { get; }
		void Hide ();
	}
}

