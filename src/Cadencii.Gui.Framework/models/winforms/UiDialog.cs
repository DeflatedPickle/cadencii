using System;

namespace cadencii
{
	public interface UiDialog
	{
	}

	public interface UiFileDialog : UiDialog
	{
		string FileName { get; }
		string Filter { get; set; }
		int FilterIndex { get; set; }
		void SetSelectedFile (string filename);
		string SelectedFilter ();
	}

	public interface UiOpenFileDialog : UiFileDialog
	{
	}

	public interface UiSaveFileDialog : UiFileDialog
	{
	}
}

