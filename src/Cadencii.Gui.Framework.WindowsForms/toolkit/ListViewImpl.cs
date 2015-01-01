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

		void UiListView.AddItem (UiListViewItem item)
		{
			Items.Add ((System.Windows.Forms.ListViewItem) item.Native);
		}

		void UiListView.RemoveItemAt (int i)
		{
			Items.RemoveAt (i);
		}

		IList UiListView.SelectedIndices {
			get { return SelectedIndices; }
		}

		int UiListView.ItemCount {
			get { return Items.Count; }
		}

		void UiListView.AddRow(string[] items, bool selected)
		{
			WinformsExtensions.AddRow (this, items, selected);
		}

		UiListViewItem UiListView.GetItem (int i)
		{
			return new ListViewItemImpl (Items [i]);
		}
		void UiListView.ClearItems ()
		{
			Items.Clear ();
		}

		IList<UiListViewColumn> UiListView.Columns {
			get { return columns ?? (columns = new ColumnCollection (this)); }
		}

		Collection<UiListViewColumn> columns;

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

		/*
		void UiListView.AddGroups (IEnumerable<UiListViewGroup> groups)
		{
			Groups.AddRange (groups.Select (g => (System.Windows.Forms.ListViewGroup) g.Native).ToArray ());
		}
		*/
		Cadencii.Gui.Toolkit.View UiListView.View {
			get { return (Cadencii.Gui.Toolkit.View) View; }
			set { View = (System.Windows.Forms.View) value; } 
		}
	}
}

