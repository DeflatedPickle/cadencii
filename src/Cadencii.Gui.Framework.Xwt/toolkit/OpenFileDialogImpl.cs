using System;
using System.Linq;
using Cadencii.Gui;
using MouseButtons = Xwt.PointerButton;
using MouseEventArgs = Xwt.ButtonEventArgs;
using MouseEventHandler = System.EventHandler<Xwt.ButtonEventArgs>;
using NMouseButtons = Cadencii.Gui.Toolkit.MouseButtons;
using NMouseEventArgs = Cadencii.Gui.Toolkit.MouseEventArgs;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Gui
{
	public class OpenFileDialogImpl : Xwt.OpenFileDialog, UiOpenFileDialog
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
