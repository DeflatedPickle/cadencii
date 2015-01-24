using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cadencii.Gui.Toolkit
{
	public partial class ContextMenuStripImpl
	{
		class MenuItemImpl : Xwt.MenuItem, UiToolStripItem
		{
			event EventHandler UiToolStripItem.MouseEnter {
				add {
					throw new NotImplementedException ();
				}
				remove {
					throw new NotImplementedException ();
				}
			}

			event EventHandler UiToolStripItem.Click {
				add { base.Clicked += value; }
				remove { base.Clicked -= value; }
			}

			void UiToolStripItem.PerformClick ()
			{
				throw new NotImplementedException ();
			}

			// no effect
			Cadencii.Gui.Font UiToolStripItem.Font { get; set; }

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

			Cadencii.Gui.Size UiToolStripItem.Size {
				get {
					throw new NotImplementedException ();
				}
				set {
					throw new NotImplementedException ();
				}
			}
		}

		class MenuItemCollection : Collection<UiToolStripItem>
		{
			Xwt.Menu menu;

			public MenuItemCollection (Xwt.Menu menu)
			{
				this.menu = menu;
			}

			protected override void InsertItem (int index, UiToolStripItem item)
			{
				menu.Items.Insert (index, (MenuItemImpl) item);
			}

			protected override void RemoveItem (int index)
			{
				menu.Items.RemoveAt (index);
			}

			protected override void SetItem (int index, UiToolStripItem item)
			{
				var xitem = (Xwt.MenuItem)item;
				menu.Items.Remove (xitem);
				menu.Items.Insert (index, xitem);
			}
			protected override void ClearItems ()
			{
				menu.Items.Clear ();
			}
		}

		public ContextMenuStripImpl ()
		{
			items = new MenuItemCollection (menu);
		}

		Xwt.Menu menu = new Xwt.Menu ();
		MenuItemCollection items;

		// ignored
		public bool ShowImageMargin { get; set; }

		// ignored
		public bool ShowCheckMargin { get; set; }

		public IList<UiToolStripItem> Items {
			get { return items; }
		}

		// ignored
		public ToolStripRenderMode RenderMode { get; set; }

		public void Show (UiControl control, int x, int y)
		{
			menu.Popup ((Xwt.Widget) control, x, y);
		}

		public event EventHandler Opening {
			add { throw new NotImplementedException (); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler VisibleChanged {
			add { throw new NotImplementedException (); }
			remove { throw new NotImplementedException (); }
		}
	}
}
