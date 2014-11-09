using System;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public class FolderBrowserDialogImpl : UiFolderBrowserDialog
	{
		System.Windows.Forms.FolderBrowserDialog impl;

		public FolderBrowserDialogImpl ()
			: this (new System.Windows.Forms.FolderBrowserDialog ())
		{
		}

		FolderBrowserDialogImpl (System.Windows.Forms.FolderBrowserDialog impl)
		{
			this.impl = impl;
		}

		public object Native {
			get { return impl; }
		}

		public void Dispose ()
		{
			impl.Dispose ();
		}

		public string Description {
			get { return impl.Description; }
			set { impl.Description = value; }
		}
		public string SelectedPath {
			get { return impl.SelectedPath; }
			set { impl.SelectedPath = value; }
		}
	}
}

