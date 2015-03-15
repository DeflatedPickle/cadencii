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
		Font UiToolStripItem.Font {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		bool UiToolStripItem.Enabled {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		int UiToolStripItem.Height {
			get {
				throw new NotImplementedException ();
			}
		}
		string UiToolStripItem.Name { get; set; }
		string UiToolStripItem.Text {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		string UiToolStripItem.ToolTipText {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		// ignore.
		Size UiToolStripItem.Size { get; set; }

		#endregion
	}
}

