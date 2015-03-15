using System;
using System.Linq;
using System.Collections.ObjectModel;

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

		class UiMenuItemCollection : Collection<UiToolStripItem>
		{
			Xwt.Menu menu;

			public UiMenuItemCollection (Xwt.Menu menu)
			{
				this.menu = menu;
			}

			protected override void ClearItems ()
			{
				base.ClearItems ();
				menu.Items.Clear ();
			}

			protected override void InsertItem (int index, UiToolStripItem item)
			{
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
					var sub = base.SubMenu ?? (base.SubMenu = new Xwt.Menu ());
					dropdown_items = new UiMenuItemCollection (sub);
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
			get { return base.Label; }
			set { base.Label = value; }
		}

		// FIXME: maybe we can do something but ignore this so far.
		string UiToolStripItem.ToolTipText { get; set; }

		// ignore
		Size UiToolStripItem.Size { get; set; }
	}
}

