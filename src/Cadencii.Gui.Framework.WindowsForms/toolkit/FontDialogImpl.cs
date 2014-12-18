using System;

namespace Cadencii.Gui.Toolkit
{
	public class FontDialogImpl : System.Windows.Forms.FontDialog, UiFontDialog
	{
		public FontDialogImpl ()
		{
		}

		DialogResult UiFontDialog.ShowDialog ()
		{
			return (DialogResult) base.ShowDialog ();
		}

		Font UiFontDialog.Font {
			get { return this.Font.ToAwt (); }
			set { this.Font = value.ToWF (); }
		}

		object UiDialog.Native {
			get { return this; }
		}
	}
}

