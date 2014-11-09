using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;

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
		Cadencii.Gui.Color UiToolStripButton.ImageTransparentColor {
			get { return ImageTransparentColor.ToAwt (); }
			set { ImageTransparentColor = value.ToNative (); }
		}
		Cadencii.Gui.Image UiToolStripButton.Image {
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

