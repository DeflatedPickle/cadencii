using System;

namespace cadencii
{
	public class ToolStripSeparatorImpl : System.Windows.Forms.ToolStripSeparator, UiToolStripSeparator
	{
		public ToolStripSeparatorImpl ()
		{
		}

		event EventHandler UiToolStripItem.MouseEnter {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiToolStripItem.Click {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		cadencii.java.awt.Font UiToolStripItem.Font {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		string UiToolStripItem.ShortcutKeyDisplayString {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		cadencii.java.awt.Dimension UiToolStripItem.Size {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
	}
}

