using System;
using Xwt;
using Xwt.Drawing;
using System.ComponentModel;

namespace Cadencii.Gui.Toolkit
{
	public class ToolTipImpl : UiToolTip
	{
		Popover impl = new Popover (new TextEntry ());

		public ToolTipImpl (IContainer components)
		{
		}

		#region UiToolTip implementation

		// FIXME: can we ignore this?
		public event EventHandler<DrawToolTipEventArgs> Draw;

		void UiToolTip.Show (string text, UiControl control, Point point, int duration)
		{
			((TextEntry)impl.Content).Text = text;
			impl.Show (Popover.Position.Top, (Widget) control.Native, new Xwt.Rectangle (point.ToWF (), new TextLayout () { Text = text }.GetSize ()));
		}

		void UiToolTip.Hide (UiControl control)
		{
			impl.Hide ();
		}

		// ignore so far.
		int UiToolTip.AutoPopDelay { get; set; }

		// ignore so far.
		int UiToolTip.InitialDelay { get; set; }

		// FIXME: can we ignore this?
		bool UiToolTip.OwnerDraw { get; set; }

		// ignore so far.
		int UiToolTip.ReshowDelay { get; set; }

		#endregion
	}
}

