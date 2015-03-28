using System;

namespace Cadencii.Gui.Toolkit
{
	public class ToolStripSeparatorImpl : Xwt.SeparatorMenuItem, UiToolStripSeparator
	{
		#region UiToolStripItem implementation
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
		void UiToolStripItem.PerformClick ()
		{
			throw new NotImplementedException ();
		}

		// ignore
		Font UiToolStripItem.Font { get; set; }

		bool UiToolStripItem.Enabled {
			get { return base.Sensitive; }
			set { base.Sensitive = value; }
		}

		int UiToolStripItem.Height {
			// hacky
			get { return GuiHost.Current.SystemMenuFont.getSize (); }
		}

		string UiToolStripItem.Name { get; set; }

		string UiToolStripItem.Text {
			get { return base.Label; }
			set { base.Label = value; }
		}

		// FIXME: implement?
		string UiToolStripItem.ToolTipText { get; set; }

		// ignore.
		Size UiToolStripItem.Size { get; set; }

		#endregion
	}
}

