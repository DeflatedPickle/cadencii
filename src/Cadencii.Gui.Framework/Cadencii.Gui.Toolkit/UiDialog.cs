using System;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiDialog : IDisposable
	{
		object Native { get; }
	}

	public interface UiFontDialog : UiDialog
	{
		Font Font { get; set; }
		bool AllowVectorFonts { get; set; }
		bool AllowVerticalFonts { get; set; }
		bool FontMustExist { get; set; }
		bool ShowEffects { get; set; }
		DialogResult ShowDialog ();
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
		bool ShowNewFolderButton { get; set; }
		string Description { get; set; }
		string SelectedPath { get; set; }
		DialogResult ShowDialog (UiForm parentForm);
	}
}

