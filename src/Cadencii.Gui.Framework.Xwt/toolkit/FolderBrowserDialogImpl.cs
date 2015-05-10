using System;
using Xwt;

namespace Cadencii.Gui.Toolkit
{
	public class FolderBrowserDialogImpl : UiFolderBrowserDialog
	{
		Xwt.SelectFolderDialog impl = new SelectFolderDialog ();

		public string Description {
			get { return impl.Title; }
			set { impl.Title = value; }
		}

		public object Native {
			get { return impl; }
		}

		public string SelectedPath {
			get { return impl.Folder; }
			// it is to set initial folder.
			set { impl.CurrentFolder = value; }
		}

		// ignore
		public bool ShowNewFolderButton { get; set; }

		public DialogResult ShowDialog (UiForm parentForm)
		{
			var ret = impl.Run ((WindowFrame)parentForm);
			return ret ? DialogResult.OK : DialogResult.Cancel;
		}

		public void Dispose ()
		{
		}
	}
}

