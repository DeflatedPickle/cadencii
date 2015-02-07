using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class PictureBoxImpl : PictureBoxBase, UiPictureBox
	{
		public class CustomDrawnImage : Xwt.Drawing.DrawingImage
		{
			PictureBoxImpl owner;

			public CustomDrawnImage (PictureBoxImpl owner)
			{
				this.owner = owner;
			}

			public event EventHandler<PaintEventArgs> Paint;

			Xwt.Drawing.Image current_image, updated_image;

			public void DrawImage (Xwt.Drawing.Image image)
			{
				updated_image = image;
			}

			protected override void OnDraw (Xwt.Drawing.Context ctx, Xwt.Rectangle bounds)
			{
				base.OnDraw (ctx, bounds);
				if (updated_image != null) {
					ctx.DrawImage (current_image, 0, 0);
					current_image = updated_image;
					updated_image = null;
				}
				if (Paint != null)
					Paint (owner, new PaintEventArgs () { ClipRectangle = bounds.ToGui (), Graphics = ctx.ToGui () });
			}
		}
		
		public PictureBoxImpl ()
		{
			image_view = new Xwt.ImageView ();
			image_view.Image = new CustomDrawnImage (this);
			this.Content = image_view;
		}

		Xwt.ImageView image_view;

		event EventHandler<PaintEventArgs> UiPictureBox.Paint {
			add { ((CustomDrawnImage) image_view.Image).Paint += value; }
			remove { ((CustomDrawnImage) image_view.Image).Paint -= value; }
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
			get { return image_view.Image.ToGui (); }
			set { ((CustomDrawnImage)image_view.Image).DrawImage (value.ToWF ()); }
		}

		// ignore. Xwt claims that it is always double buffered.
		bool UiPictureBox.DoubleBuffered { get; set; }
	}
}

