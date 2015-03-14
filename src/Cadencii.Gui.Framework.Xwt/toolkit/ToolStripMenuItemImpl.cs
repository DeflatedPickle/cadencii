using System;
using System.Linq;

namespace Cadencii.Gui.Toolkit
{
	public class ToolStripMenuItemImpl : Xwt.CheckBoxMenuItem, UiToolStripMenuItem
	{
		public ToolStripMenuItemImpl ()
		{
			base.Clicked += (o, e) => {
				var x = this as UiToolStripMenuItem;
				if (x.CheckOnClick)
					base.Checked = !base.Checked;
			};
		}

		public ToolStripMenuItemImpl (string s, Cadencii.Gui.Image o, EventHandler e)
			: this ()
		{
			Label = s;
			Image = (Xwt.Drawing.Image) o.NativeImage;
			Clicked += e;
		}

		event EventHandler UiToolStripMenuItem.CheckedChanged {
			add { throw new NotImplementedException (); }
			remove { throw new NotImplementedException (); }
		}

		event EventHandler UiToolStripMenuItem.MouseHover {
			add { throw new NotImplementedException (); }
			remove { throw new NotImplementedException (); }
		}

		void UiToolStripMenuItem.Mnemonic (Keys keys)
		{
			throw new NotImplementedException ();
		}

		Cadencii.Gui.Image UiToolStripMenuItem.Image {
			get { return base.Image.ToGui (); }
			set { base.Image = value.ToWF (); }
		}

		// ignore.
		string UiToolStripMenuItem.ShortcutKeyDisplayString { get; set; }

		Keys UiToolStripMenuItem.ShortcutKeys {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		// this is taken care by the event handler which is registered at .ctor().
		bool UiToolStripMenuItem.CheckOnClick { get; set; }

		// UiToolStripDropDownItem

		// it sounds technically different, but Click event works for it. (At least on Gtk, and who else cares)
		event EventHandler UiToolStripDropDownItem.DropDownOpening {
			add { base.Clicked += value; }
			remove { base.Clicked -= value; }
		}

		System.Collections.Generic.IList<UiToolStripItem> UiToolStripDropDownItem.DropDownItems {
			get {
				throw new NotImplementedException ();
			}
		}

		// UiToolStripItem

		event EventHandler UiToolStripItem.MouseEnter {
			add { throw new NotImplementedException (); }
			remove { throw new NotImplementedException (); }
		}

		// and this is actually different from Xwt.MenuItem.Clicked...
		event EventHandler UiToolStripItem.Click {
			add { throw new NotImplementedException (); }
			remove { throw new NotImplementedException (); }
		}

		void UiToolStripItem.PerformClick ()
		{
			throw new NotImplementedException ();
		}

		// ignore.
		Font UiToolStripItem.Font { get; set; }

		bool UiToolStripItem.Enabled {
			get { return base.Sensitive; }
			set { base.Sensitive = value; }
		}

		int UiToolStripItem.Height {
			get {
				throw new NotImplementedException ();
			}
		}

		string UiToolStripItem.Name { get; set; }

		string UiToolStripItem.Text {
			get { return base.Label; }
			set { base.Label = value; }
		}

		string UiToolStripItem.ToolTipText {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Size UiToolStripItem.Size {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
	}
}

