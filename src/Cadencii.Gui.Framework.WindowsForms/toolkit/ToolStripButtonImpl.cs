using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using cadencii;

namespace Cadencii.Gui.Toolkit
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
			get { return ImageTransparentColor.ToGui (); }
			set { ImageTransparentColor = value.ToNative (); }
		}
		Cadencii.Gui.Image UiToolStripButton.Image {
			get { return Image.ToGui (); }
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
			get { return Font.ToGui (); }
			set { Font = value.ToWF (); }
		}

		Size UiToolStripItem.Size {
			get { return Size.ToGui (); }
			set { Size = value.ToWF (); }
		}
	}
}

