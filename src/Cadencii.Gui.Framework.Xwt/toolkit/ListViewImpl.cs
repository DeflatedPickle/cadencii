using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cadencii.Gui.Toolkit
{
	public partial class ListViewImpl : ListViewBase, UiListView
	{
		event EventHandler UiListView.SelectedIndexChanged {
			add { base.SelectionChanged += value; }
			remove { base.SelectionChanged -= value; }
		}

		void UiListView.AddRow (string[] items, bool selected)
		{
			throw new NotImplementedException ();
		}

		bool UiListView.UseCompatibleStateImageBehavior {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiListView.CheckBoxes {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiListView.FullRowSelect {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		// ignore. There is only FormWordDictionaryUi that makes use of "List" and everything else uses "Details".
		View UiListView.View { get; set; }

		System.Collections.IList UiListView.SelectedIndices {
			get {
				throw new NotImplementedException ();
			}
		}

		ColumnCollection columns;

		public IList<UiListViewColumn> Columns {
			get { return columns ?? (columns = new ColumnCollection (this)); }
		}

		class ColumnCollection : Collection<UiListViewColumn>
		{
			Xwt.TreeView lv;

			public ColumnCollection (Xwt.TreeView lv)
			{
				this.lv = lv;
			}

			protected override void InsertItem (int index, UiListViewColumn item)
			{
				if (item.Native == null)
					item.Native = new Xwt.ListViewColumn (item.Text); // FIXME: item.Width is ignored.
				base.InsertItem (index, item);
				lv.Columns.Add ((Xwt.ListViewColumn) item.Native);
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

		// Groups - practically do nothing. Xwt doesn't support it.

		public IList<UiListViewGroup> Groups {
			get { return groups ?? (groups = new GroupCollection (this)); }
		}

		GroupCollection groups;

		class GroupCollection : Collection<UiListViewGroup>
		{
			Xwt.TreeView lv;

			public GroupCollection (Xwt.TreeView lv)
			{
				this.lv = lv;
			}

			protected override void InsertItem (int index, UiListViewGroup group)
			{
				if (group.Native == null)
					group.Native = new { Name = group.Name, Header = group.Header };
				base.InsertItem (index, group);
			}

			protected override void ClearItems ()
			{
				base.ClearItems ();
			}

			protected override void RemoveItem (int index)
			{
				base.RemoveItem (index);
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
			Xwt.TreeView lv;

			public ItemCollection (Xwt.TreeView lv)
			{
				this.lv = lv;
			}

			protected override void InsertItem (int index, UiListViewItem item)
			{
				if (item.Native == null)
					item.Native = new { Selected = item.Selected, Checked = item.Checked, BackColor = item.BackColor.ToNative () };
				base.InsertItem (index, item);
				//lv.Items.Add (item.Native);
			}

			protected override void ClearItems ()
			{
				base.ClearItems ();
				//lv.Items.Clear ();
			}

			protected override void RemoveItem (int index)
			{
				base.RemoveItem (index);
				//lv.Items.RemoveAt (index);
			}

			protected override void SetItem (int index, UiListViewItem item)
			{
				throw new NotSupportedException ();
			}
		}
	}
}
