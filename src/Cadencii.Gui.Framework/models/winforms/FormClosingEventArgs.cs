using System;

namespace Cadencii.Gui
{
	public class FormClosingEventArgs : EventArgs
	{
		public FormClosingEventArgs ()
		{
		}

		public bool Cancel { get; set; }
	}
}

