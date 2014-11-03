﻿using System;

namespace cadencii
{
	public abstract class FileDialogImpl : UiFileDialog
	{
		System.Windows.Forms.FileDialog impl;

		protected FileDialogImpl (System.Windows.Forms.FileDialog impl)
		{
			this.impl = impl;
		}

		public string FileName {
			get { return impl.FileName; }
		}

		public string Filter {
			get { return impl.Filter; }
			set { impl.Filter = value; }
		}

		public int FilterIndex {
			get { return impl.FilterIndex; }
			set { impl.FilterIndex = value; }
		}

		public string SelectedFilter ()
		{
			return impl.SelectedFilter ();
		}

		public void SetSelectedFile (string filename)
		{
			impl.SetSelectedFile (filename);
		}
	}
}
