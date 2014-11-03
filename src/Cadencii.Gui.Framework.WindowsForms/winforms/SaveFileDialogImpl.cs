using System;
using cadencii.java.awt;

namespace cadencii
{
	public class SaveFileDialogImpl : FileDialogImpl, UiSaveFileDialog
	{
		System.Windows.Forms.SaveFileDialog impl;

		public SaveFileDialogImpl ()
			: this (new System.Windows.Forms.SaveFileDialog ())
		{
		}

		SaveFileDialogImpl (System.Windows.Forms.SaveFileDialog impl)
			: base (impl)
		{
			this.impl = impl;
		}
	}
}

