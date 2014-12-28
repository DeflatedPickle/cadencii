using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using cadencii;

namespace Cadencii.Gui.Toolkit
{
	public class ToolStripStatusLabelImpl : System.Windows.Forms.ToolStripStatusLabel, UiToolStripStatusLabel
	{
		Cadencii.Gui.Image UiToolStripStatusLabel.Image {
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

