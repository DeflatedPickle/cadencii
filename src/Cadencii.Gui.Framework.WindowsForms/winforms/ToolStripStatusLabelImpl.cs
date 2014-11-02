using System;
using cadencii.java.awt;

namespace cadencii
{
	public class ToolStripStatusLabelImpl : System.Windows.Forms.ToolStripStatusLabel, UiToolStripStatusLabel
	{
		cadencii.java.awt.Image UiToolStripStatusLabel.Image {
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

