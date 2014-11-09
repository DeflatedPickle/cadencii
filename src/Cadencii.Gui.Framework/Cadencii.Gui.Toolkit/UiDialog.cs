using System;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiDialog : IDisposable
	{
		object Native { get; }
	}

	public interface UiFileDialog : UiDialog
	{
		string FileName { get; }
		string Filter { get; set; }
		int FilterIndex { get; set; }
		void SetSelectedFile (string filename);
		string SelectedFilter ();
		string Title { get; set; }
		DialogResult ShowDialog ();
	}

	public interface UiOpenFileDialog : UiFileDialog
	{
	}

	public interface UiSaveFileDialog : UiFileDialog
	{
	}

	public interface UiFolderBrowserDialog : UiDialog
	{
		string Description { get; set; }
		string SelectedPath { get; set; }
	}
}

