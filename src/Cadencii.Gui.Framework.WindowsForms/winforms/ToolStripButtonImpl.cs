using System;
using cadencii.java.awt;

namespace cadencii
{
	public class ToolStripButtonImpl : System.Windows.Forms.ToolStripButton, UiToolStripButton
	{
		event EventHandler UiToolStripButton.CheckedChanged {
			add { CheckedChanged += (sender, e) => value (sender, e); }
			remove {
				throw new NotImplementedException ();
			}
		}
		cadencii.java.awt.Color UiToolStripButton.ImageTransparentColor {
			get { return ImageTransparentColor.ToAwt (); }
			set { ImageTransparentColor = value.ToNative (); }
		}
		cadencii.java.awt.Image UiToolStripButton.Image {
			get { return Image.ToAwt (); }
			set { Image = value.ToWF (); }
		}

		// UiToolStripItem

		event EventHandler UiToolStripItem.MouseEnter {
			add { MouseEnter += (sender, e) => value (sender, e); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiToolStripItem.Click {
			add { Click += (sender, e) => value (sender, e); }
			remove {
				throw new NotImplementedException ();
			}
		}

		Font UiToolStripItem.Font {
			get { return Font.ToAwt (); }
			set { Font = value.ToWF (); }
		}

		Dimension UiToolStripItem.Size {
			get { return Size.ToAwt (); }
			set { Size = value.ToWF (); }
		}
	}
}

