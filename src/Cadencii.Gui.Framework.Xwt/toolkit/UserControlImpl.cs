using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class UserControlImpl
	{
		public event EventHandler<PaintEventArgs> Paint;

		// FIXME: how to support this?
		public event EventHandler Load;

		// FIXME: ignore so far.
		BorderStyle UiUserControl.BorderStyle { get; set; }

		// actually ContainerControl member.
		// ignorable
		AutoScaleMode UiUserControl.AutoScaleMode { get; set; }

		// ignorable
		Size UiUserControl.AutoScaleDimensions { get; set; }

		// FIXME: no effect (Xwt claims that it defaults to double buffered by default).
		bool UiUserControl.DoubleBuffered { get; set; }

		// FIXME: no effect
		bool UiUserControl.DoubleBuffer { get; set; } // note that it is different from "DoubleBuffered"
		bool UiUserControl.UserPaint { get; set; }
		bool UiUserControl.AllPaintingInWmPaint { get; set; }

		void UiUserControl.OnPaint (PaintEventArgs e)
		{
			OnDraw ((Xwt.Drawing.Context)e.Graphics.NativeGraphics, e.ClipRectangle.ToWF ());
		}

		protected override void OnDraw (Xwt.Drawing.Context ctx, Xwt.Rectangle dirtyRect)
		{
			base.OnDraw (ctx, dirtyRect);
			if (Paint != null)
				Paint (this, new PaintEventArgs () {
					Graphics = new Graphics () { NativeGraphics = ctx },
					ClipRectangle = dirtyRect.ToGui ()
				});
		}
	}
}

