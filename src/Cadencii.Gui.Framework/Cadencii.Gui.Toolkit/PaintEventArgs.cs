using System;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public class PaintEventArgs : EventArgs
	{
		public Rectangle ClipRectangle { get; set; }
		public Graphics Graphics { get; set; }
	}
}

