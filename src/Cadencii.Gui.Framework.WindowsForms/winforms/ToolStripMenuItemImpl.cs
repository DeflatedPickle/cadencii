using System;
using cadencii.java.awt;
using System.Collections.Generic;

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
			add { CheckedChanged += (o, e) => value (o, e); }
			remove { throw new NotImplementedException (); }
		}

		void UiToolStripMenuItem.Mnemonic (cadencii.java.awt.Keys keys)
		{
			throw new NotImplementedException ();
		}

		Image UiToolStripMenuItem.Image {
			get { return Image.ToAwt (); }
			set { Image = value.ToWF (); }
		}

		CheckState UiToolStripMenuItem.CheckState {
			get { return (CheckState)CheckState; }
			set { CheckState = (System.Windows.Forms.CheckState)value; }
		}

		ToolStripItemDisplayStyle UiToolStripMenuItem.DisplayStyle {
			get { return (ToolStripItemDisplayStyle)DisplayStyle; }
			set { DisplayStyle = (System.Windows.Forms.ToolStripItemDisplayStyle) value; }
		}

		ToolStripItemImageScaling UiToolStripMenuItem.ImageScaling {
			get { return (ToolStripItemImageScaling)ImageScaling; }
			set { ImageScaling = (System.Windows.Forms.ToolStripItemImageScaling) value; }
		}

		TextImageRelation UiToolStripMenuItem.TextImageRelation {
			get { return (TextImageRelation)TextImageRelation; }
			set { TextImageRelation = (System.Windows.Forms.TextImageRelation) value; }
		}

		Keys UiToolStripMenuItem.ShortcutKeys {
			get { return (Keys)ShortcutKeys; }
			set { ShortcutKeys = (System.Windows.Forms.Keys) value; }
		}

		event EventHandler UiToolStripDropDownItem.DropDownOpening {
			add { DropDownOpening += (sender, e) => value (sender, e); }
			remove {
				throw new NotImplementedException ();
			}
		}

		IList<UiToolStripItem> UiToolStripDropDownItem.DropDownItems {
			get { return new CastingList<UiToolStripItem,System.Windows.Forms.ToolStripItem> (DropDownItems, null, null); }
		}

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

