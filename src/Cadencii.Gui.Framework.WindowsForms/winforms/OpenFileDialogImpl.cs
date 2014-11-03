using System;
using cadencii.java.awt;

namespace cadencii
{
	public class OpenFileDialogImpl : FileDialogImpl, UiOpenFileDialog
	{
		System.Windows.Forms.OpenFileDialog impl;

		public OpenFileDialogImpl ()
			: this (new System.Windows.Forms.OpenFileDialog ())
		{
		}

		OpenFileDialogImpl (System.Windows.Forms.OpenFileDialog impl)
			: base (impl)
		{
			this.impl = impl;
		}
	}
}

