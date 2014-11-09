using System;

namespace Cadencii.Gui.Toolkit
{
	public class FormClosingEventArgs : EventArgs
	{
		public FormClosingEventArgs ()
		{
		}

		public bool Cancel { get; set; }
	}
}

