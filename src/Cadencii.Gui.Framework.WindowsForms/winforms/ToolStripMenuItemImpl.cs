using System;

namespace cadencii
{
	public class ToolStripMenuItemImpl : System.Windows.Forms.ToolStripMenuItem, UiToolStripMenuItem
	{
		public ToolStripMenuItemImpl ()
		{
		}

		public ToolStripMenuItemImpl (string s, cadencii.java.awt.Image o, EventHandler e)
			: base (s, (System.Drawing.Image) o.NativeImage, e)
		{
		}

		event EventHandler UiToolStripMenuItem.CheckedChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiToolStripMenuItem.Mnemonic (cadencii.java.awt.Keys keys)
		{
			throw new NotImplementedException ();
		}

		cadencii.java.awt.Image UiToolStripMenuItem.Image {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		cadencii.java.awt.CheckState UiToolStripMenuItem.CheckState {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		cadencii.java.awt.ToolStripItemDisplayStyle UiToolStripMenuItem.DisplayStyle {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		cadencii.java.awt.ToolStripItemImageScaling UiToolStripMenuItem.ImageScaling {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		cadencii.java.awt.TextImageRelation UiToolStripMenuItem.TextImageRelation {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		cadencii.java.awt.Keys UiToolStripMenuItem.ShortcutKeys {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiToolStripDropDownItem.DropDownOpening {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		System.Collections.Generic.IList<UiToolStripItem> UiToolStripDropDownItem.DropDownItems {
			get {
				throw new NotImplementedException ();
			}
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

