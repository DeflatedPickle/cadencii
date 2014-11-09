using System;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public class DrawToolTipEventArgs : EventArgs
	{
		Action drawBackground;
		Action drawBorder;
		Action<TextFormatFlags> drawText;
		public DrawToolTipEventArgs (
			Rectangle bounds, Action drawBackground, Action drawBorder, Action<TextFormatFlags> drawText)
		{
			Bounds = bounds;
			this.drawBackground = drawBackground;
			this.drawBorder = drawBorder;
			this.drawText = drawText;
		}
		public Rectangle Bounds { get; set; }
		public void DrawBackground() { drawBackground (); }
		public void DrawBorder() { drawBorder (); }
		public void DrawText(TextFormatFlags flags) { drawText (flags); }
	}
}

