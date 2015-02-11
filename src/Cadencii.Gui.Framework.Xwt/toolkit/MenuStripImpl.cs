using System;
using System.Collections.ObjectModel;

namespace Cadencii.Gui.Toolkit
{
	public class MenuStripImpl : UiMenuStrip
	{
		Xwt.Menu menu = new Xwt.Menu ();

		#region UiMenuStrip implementation

		object UiMenuStrip.Native {
			get { return menu; }
		}

		// ignore.
		ToolStripRenderMode UiMenuStrip.RenderMode { get; set; }

		// ignore. Why Text on Menu??
		string UiMenuStrip.Text { get; set; }

		System.Collections.Generic.IList<UiToolStripItem> UiMenuStrip.Items {
			get { return items ?? (items = new MenuItemCollection (menu)); }
		}

		#endregion

		MenuItemCollection items;

		class MenuItemCollection : Collection<UiToolStripItem>
		{
			Xwt.Menu menu;

			public MenuItemCollection (Xwt.Menu menu)
			{
				this.menu = menu;
			}

			protected override void InsertItem (int index, UiToolStripItem item)
			{
				base.InsertItem (index, item);
				menu.Items.Add ((Xwt.MenuItem) item);
			}

			protected override void ClearItems ()
			{
				base.ClearItems ();
				menu.Items.Clear ();
			}

			protected override void RemoveItem (int index)
			{
				base.RemoveItem (index);
				menu.Items.RemoveAt (index);
			}

			protected override void SetItem (int index, UiToolStripItem item)
			{
				throw new NotSupportedException ();
			}
		}
	}
}
