using System;
using Cadencii.Gui;
using System.Collections.Generic;
using Cadencii.Gui.Toolkit;
using cadencii;

namespace Cadencii.Gui.Toolkit
{
	public class ToolStripMenuItemImpl : System.Windows.Forms.ToolStripMenuItem, UiToolStripMenuItem
	{
		public ToolStripMenuItemImpl ()
		{
		}

		public ToolStripMenuItemImpl (string s, Cadencii.Gui.Image o, EventHandler e)
		{
			Text = s;
			Image = (System.Drawing.Image) o.NativeImage;
			Click += e;
		}

		void UiToolStripMenuItem.Mnemonic (Cadencii.Gui.Toolkit.Keys keys)
		{
			this.Mnemonic (keys);
		}

		Image UiToolStripMenuItem.Image {
			get { return Image.ToGui (); }
			set { Image = value.ToWF (); }
		}

		Keys UiToolStripMenuItem.ShortcutKeys {
			get { return (Keys)ShortcutKeys; }
			set { ShortcutKeys = (System.Windows.Forms.Keys) value; }
		}

		event EventHandler UiToolStripDropDownItem.DropDownOpening {
			add { DropDownOpening += value; }
			remove { base.DropDownOpening -= value; }
		}

		IList<UiToolStripItem> UiToolStripDropDownItem.DropDownItems {
			get { return new CastingList<UiToolStripItem,System.Windows.Forms.ToolStripItem> (DropDownItems, null, null); }
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

