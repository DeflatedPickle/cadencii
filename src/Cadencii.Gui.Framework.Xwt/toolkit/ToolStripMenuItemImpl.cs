using System;
using System.Linq;
using System.Collections.ObjectModel;

namespace Cadencii.Gui.Toolkit
{
	public class ToolStripMenuItemImpl : Xwt.MenuItem, UiToolStripMenuItem
	{
		public ToolStripMenuItemImpl ()
		{
			base.Clicked += (o, e) => {
				var x = this as UiToolStripMenuItem;
				if (x.CheckOnClick) {
					//base.Checked = !base.Checked;
					if (CheckedChanged != null)
						CheckedChanged (this, EventArgs.Empty);
				}
			};
		}

		public ToolStripMenuItemImpl (string s, Cadencii.Gui.Image o, EventHandler e)
			: this ()
		{
			Label = s;
			Image = (Xwt.Drawing.Image) o.NativeImage;
			Clicked += e;
		}

		// FIXME: implement
		public bool Checked { get; set; }

		public event EventHandler CheckedChanged;

		// FIXME: implement
		public event EventHandler MouseHover;

		void UiToolStripMenuItem.Mnemonic (Keys keys)
		{
			// no need to implement in Xwt, '_' is automatically recognized as mnemonic in Xwt,
			// and '&' is automatically converted to '_'.
			// FIXME: However, the app rewrites the text label without mnemonic and then calls
			// this method to make sure mnemonic works. So, if '_' is not on the label, add it...
		}

		Cadencii.Gui.Image UiToolStripMenuItem.Image {
			get { return base.Image.ToGui (); }
			set { base.Image = value.ToWF (); }
		}

		// ignore.
		string UiToolStripMenuItem.ShortcutKeyDisplayString { get; set; }

		// FIXME: implement
		Keys UiToolStripMenuItem.ShortcutKeys { get; set; }

		// this is taken care by the event handler which is registered at .ctor().
		bool UiToolStripMenuItem.CheckOnClick { get; set; }

		// UiToolStripDropDownItem

		// it sounds technically different, but Click event works for it. (At least on Gtk, and who else cares)
		event EventHandler UiToolStripDropDownItem.DropDownOpening {
			add { base.Clicked += value; }
			remove { base.Clicked -= value; }
		}

		class UiMenuItemCollection : Collection<UiToolStripItem>
		{
			Xwt.Menu menu;
			Action populating;

			public UiMenuItemCollection (Xwt.Menu menu, Action populating)
			{
				this.menu = menu;
				this.populating = populating;
			}

			protected override void ClearItems ()
			{
				base.ClearItems ();
				menu.Items.Clear ();
			}

			protected override void InsertItem (int index, UiToolStripItem item)
			{
				if (Count == 0)
					populating ();
				base.InsertItem (index, item);
				menu.Items.Insert (index, (Xwt.MenuItem) item);
			}

			protected override void RemoveItem (int index)
			{
				base.RemoveItem (index);
				menu.Items.RemoveAt (index);
			}

			protected override void SetItem (int index, UiToolStripItem item)
			{
				throw new NotImplementedException ();
			}
		}

		UiMenuItemCollection dropdown_items;

		public System.Collections.Generic.IList<UiToolStripItem> DropDownItems {
			get {
				if (dropdown_items == null) {
					var sub = new Xwt.Menu ();
					dropdown_items = new UiMenuItemCollection (sub, () => base.SubMenu = sub);
				}
				return dropdown_items;
			}
		}

		// UiToolStripItem

		event EventHandler UiToolStripItem.MouseEnter {
			add { throw new NotImplementedException (); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: this is actually different from Xwt.MenuItem.Clicked...
		event EventHandler UiToolStripItem.Click {
			add { base.Clicked += value; }
			remove { base.Clicked -= value; }
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
			get { return base.Label + (Checked ? " *" : string.Empty); }
			set { base.Label = value.Replace ('&', '_'); }
		}

		// FIXME: maybe we can do something but ignore this so far.
		string UiToolStripItem.ToolTipText { get; set; }

		// ignore
		Size UiToolStripItem.Size { get; set; }
	}
}

