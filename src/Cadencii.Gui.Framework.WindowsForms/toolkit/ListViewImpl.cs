using System;
using System.Linq;
using Cadencii.Gui;
using Keys = Cadencii.Gui.Toolkit.Keys;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using KeyEventHandler = System.Windows.Forms.KeyEventHandler;
using MouseButtons = System.Windows.Forms.MouseButtons;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;
using NKeyEventArgs = Cadencii.Gui.Toolkit.KeyEventArgs;
using NKeyPressEventArgs = Cadencii.Gui.Toolkit.KeyPressEventArgs;
using NKeyEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.KeyEventArgs>;
using NMouseButtons = Cadencii.Gui.Toolkit.MouseButtons;
using NMouseEventArgs = Cadencii.Gui.Toolkit.MouseEventArgs;
using NMouseEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.MouseEventArgs>;
using System.Collections.Generic;
using Cadencii.Gui.Toolkit;
using cadencii;
using System.Collections.ObjectModel;
using System.Collections;

namespace Cadencii.Gui.Toolkit
{
	public partial class ListViewImpl : System.Windows.Forms.ListView, UiListView
	{
		// UiListView

		IList UiListView.SelectedIndices {
			get { return SelectedIndices; }
		}

		void UiListView.AddRow(string[] items, bool selected)
		{
			WinformsExtensions.AddRow (this, items, selected);
		}

		Cadencii.Gui.Toolkit.View UiListView.View {
			get { return (Cadencii.Gui.Toolkit.View) View; }
			set { View = (System.Windows.Forms.View) value; } 
		}

		// Columns

		public IList<UiListViewColumn> Columns {
			get { return columns ?? (columns = new ColumnCollection (this)); }
		}

		ColumnCollection columns;

		class ColumnCollection : Collection<UiListViewColumn>
		{
			System.Windows.Forms.ListView lv;

			public ColumnCollection (System.Windows.Forms.ListView lv)
			{
				this.lv = lv;
			}

			protected override void InsertItem (int index, UiListViewColumn item)
			{
                if (item.Native == null)
                    item.Native = new System.Windows.Forms.ColumnHeader (item.Text) { Width = item.Width };
				base.InsertItem (index, item);
				lv.Columns.Add ((System.Windows.Forms.ColumnHeader) item.Native);
			}

			protected override void ClearItems ()
			{
				base.ClearItems ();
				lv.Columns.Clear ();
			}

			protected override void RemoveItem (int index)
			{
				base.RemoveItem (index);
				lv.Columns.RemoveAt (index);
			}

			protected override void SetItem (int index, UiListViewColumn item)
			{
				throw new NotSupportedException ();
			}
		}

		// Groups

		public IList<UiListViewGroup> Groups {
			get { return groups ?? (groups = new GroupCollection (this)); }
		}

		GroupCollection groups;

		class GroupCollection : Collection<UiListViewGroup>
		{
			System.Windows.Forms.ListView lv;

			public GroupCollection (System.Windows.Forms.ListView lv)
			{
				this.lv = lv;
			}

			protected override void InsertItem (int index, UiListViewGroup group)
			{
				if (group.Native == null)
					group.Native = new System.Windows.Forms.ListViewGroup (group.Name, group.Header);
				base.InsertItem (index, group);
				lv.Groups.Add ((System.Windows.Forms.ListViewGroup) group.Native);
			}

			protected override void ClearItems ()
			{
				base.ClearItems ();
				lv.Groups.Clear ();
			}

			protected override void RemoveItem (int index)
			{
				base.RemoveItem (index);
				lv.Groups.RemoveAt (index);
			}

			protected override void SetItem (int index, UiListViewGroup item)
			{
				throw new NotSupportedException ();
			}
		}

		// Items

		public IList<UiListViewItem> Items {
			get { return items ?? (items = new ItemCollection (this)); }
		}

		ItemCollection items;

		class ItemCollection : Collection<UiListViewItem>
		{
			System.Windows.Forms.ListView lv;

			public ItemCollection (System.Windows.Forms.ListView lv)
			{
				this.lv = lv;
			}

			protected override void InsertItem (int index, UiListViewItem item)
			{
				if (item.Native == null)
					item.Native = new System.Windows.Forms.ListViewItem (item.Text) { Selected = item.Selected, Checked = item.Checked, BackColor = item.BackColor.ToNative () };
				base.InsertItem (index, item);
				lv.Items.Add ((System.Windows.Forms.ListViewItem) item.Native);
			}

			protected override void ClearItems ()
			{
				base.ClearItems ();
				lv.Items.Clear ();
			}

			protected override void RemoveItem (int index)
			{
				base.RemoveItem (index);
				lv.Items.RemoveAt (index);
			}

			protected override void SetItem (int index, UiListViewItem item)
			{
				throw new NotSupportedException ();
			}
		}
	}
}

