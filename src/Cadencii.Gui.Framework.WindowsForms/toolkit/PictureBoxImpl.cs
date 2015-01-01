using System;
using System.Linq;
using Cadencii.Gui;
using Keys = Cadencii.Gui.Toolkit.Keys;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using KeyEventHandler = System.Windows.Forms.KeyEventHandler;
using MouseButtons = System.Windows.Forms.MouseButtons;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;
using NKeyEventArgs = Cadencii.Gui.Toolkit.KeyEventArgs;
using NKeyPressEventArgs = Cadencii.Gui.Toolkit.KeyPressEventArgs;
using NKeyEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.KeyEventArgs>;
using NMouseButtons = Cadencii.Gui.Toolkit.MouseButtons;
using NMouseEventArgs = Cadencii.Gui.Toolkit.MouseEventArgs;
using NMouseEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.MouseEventArgs>;
using System.Collections.Generic;
using Cadencii.Gui.Toolkit;
using cadencii;

namespace Cadencii.Gui.Toolkit
{
	public partial class PictureBoxImpl : System.Windows.Forms.PictureBox, UiPictureBox
	{
		// These are actually for Control, not PictureBox specific.
		public virtual void OnPaint (PaintEventArgs e)
		{
			// could be overriden.
			base.OnPaint (e.ToWF ());
		}

		protected override void OnPaint (System.Windows.Forms.PaintEventArgs e)
		{
			this.OnPaint (e.ToGui ());
		}

		bool UiPictureBox.DoubleBuffered {
			get { return DoubleBuffered; }
			set { DoubleBuffered = true; }
		}

		public bool UserPaint {
			get { return this.GetStyle (System.Windows.Forms.ControlStyles.UserPaint); }
			set { this.SetStyle (System.Windows.Forms.ControlStyles.UserPaint, value); }
		}

		event EventHandler<PaintEventArgs> UiPictureBox.Paint {
			add { Paint += (sender, e) => value (sender, e.ToGui ()); }
			remove {
				throw new NotImplementedException ();
			}
		}

		BorderStyle UiPictureBox.BorderStyle {
			get { return (BorderStyle) BorderStyle; }
			set { BorderStyle = (System.Windows.Forms.BorderStyle) value; }
		}

		PictureBoxSizeMode UiPictureBox.SizeMode {
			get { return (PictureBoxSizeMode) SizeMode; }
			set { SizeMode = (System.Windows.Forms.PictureBoxSizeMode) value; }
		}

		Size UiPictureBox.MaximumSize {
			get { return MaximumSize.ToGui (); }
			set { MaximumSize = value.ToWF (); }
		}

		Size UiPictureBox.MinimumSize {
			get { return MinimumSize.ToGui (); }
			set { MinimumSize = value.ToWF (); }
		}

		Image UiPictureBox.Image {
			get { return Image.ToGui (); }
			set { Image = (System.Drawing.Image) value.NativeImage; }
		}
	}
}

