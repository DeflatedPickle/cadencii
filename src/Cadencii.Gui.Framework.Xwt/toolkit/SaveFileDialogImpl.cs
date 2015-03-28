using System;
using System.Linq;

namespace Cadencii.Gui.Toolkit
{
	public class SaveFileDialogImpl : Xwt.SaveFileDialog, UiSaveFileDialog
	{
		void UiFileDialog.SetSelectedFile (string filename)
		{
			this.InitialFileName = filename;
		}
		string UiFileDialog.SelectedFilter ()
		{
			// not expected to have multiple filters.
			return this.ActiveFilter != null ? this.ActiveFilter.Patterns.First () : null;
		}
		DialogResult UiFileDialog.ShowDialog ()
		{
			return this.Run () ? DialogResult.OK : DialogResult.Cancel;
		}
		string UiFileDialog.Filter {
			get { return Filters.Any () ? Filters.FirstOrDefault ().Patterns.First () : null; }
			set {
				Filters.Clear ();
				Filters.Add (new Xwt.FileDialogFilter (value, value));
			}
		}
		int UiFileDialog.FilterIndex { get; set; }

		object UiDialog.Native {
			get { return this; }
		}
	}
}
