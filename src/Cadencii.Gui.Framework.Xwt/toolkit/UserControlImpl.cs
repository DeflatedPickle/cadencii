using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class UserControlImpl
	{
		event EventHandler<PaintEventArgs> UiUserControl.Paint{
			add { throw new NotImplementedException (); }
			remove { throw new NotImplementedException (); }
		}

		event EventHandler UiUserControl.Load{
			add { throw new NotImplementedException (); }
			remove { throw new NotImplementedException (); }
		}

		BorderStyle UiUserControl.BorderStyle {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
		}

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
			throw new NotImplementedException ();
		}
	}
}

