using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class PictureBoxImpl : PictureBoxBase, UiPictureBox
	{
		public PictureBoxImpl ()
		{
		}

		Xwt.ImageView image_view;
		Xwt.ImageView ImageView {
			get {
				if (image_view == null) {
					image_view = new Xwt.ImageView ();
					this.Content = image_view;
				}
				return image_view;
			}
		}

		Xwt.Canvas canvas;
		Xwt.Canvas Canvas {
			get {
				if (canvas == null) {
					canvas = new Xwt.Canvas ();
					this.Content = canvas;
				}
				return canvas;
			}
		}

		event EventHandler<PaintEventArgs> UiPictureBox.Paint {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// ignore. insignificant.
		BorderStyle UiPictureBox.BorderStyle { get; set; }

		// ignore. It is used only for VST logo in AboutDialog.
		PictureBoxSizeMode UiPictureBox.SizeMode { get; set; }

		// ignorable
		Cadencii.Gui.Size UiPictureBox.MaximumSize { get; set; }

		Cadencii.Gui.Size UiPictureBox.MinimumSize {
			get { return new Size ((int) MinWidth, (int) MinHeight); }
			set {
				MinWidth = value.Width;
				MinHeight = value.Height;
			}
		}

		Image UiPictureBox.Image {
			get { return ImageView.Image.ToGui (); }
			set { ImageView.Image = value.ToWF (); }
		}

		// ignore. Xwt claims that it is always double buffered.
		bool UiPictureBox.DoubleBuffered { get; set; }
	}
}

