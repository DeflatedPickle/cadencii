using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cadencii.Gui.Toolkit
{
	public partial class ListViewImpl : ListViewBase, UiListView
	{
		public ListViewImpl ()
		{
			var chkbox = new Xwt.DataField<bool> ();
			var ls = new Xwt.ListStore ();
			var cc = new Xwt.ListViewColumn ();
			cc.Views.Add (new Xwt.CellView () { VisibleField = chkbox });
			base.Columns.Add (cc);
			base.DataSource = ls;
		}

		event EventHandler UiListView.SelectedIndexChanged {
			add { base.SelectionChanged += value; }
			remove { base.SelectionChanged -= value; }
		}

		Dictionary<UiListViewColumn, Xwt.IDataField<string>> data_fields =
			new Dictionary<UiListViewColumn, Xwt.IDataField<string>> ();

		Xwt.IDataField<string> GetField (UiListViewColumn c)
		{
			Xwt.IDataField<string> f;
			if (!data_fields.TryGetValue (c, out f)) {
				f = new Xwt.DataField<string> ();
				data_fields [c] = f;
			}
			return f;
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

		// ignore. Always full row select.
		bool UiListView.FullRowSelect { get; set; }

		// ignore. There is only FormWordDictionaryUi that makes use of "List" and everything else uses "Details".
		View UiListView.View { get; set; }

		System.Collections.IList UiListView.SelectedIndices {
			get { return base.SelectedRows; }
		}

		// Columns

		ColumnCollection columns;

		public IList<UiListViewColumn> Columns {
			get { return columns ?? (columns = new ColumnCollection (this)); }
		}

		class ColumnCollection : Collection<UiListViewColumn>
		{
			Xwt.ListView lv;

			public ColumnCollection (Xwt.ListView lv)
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
			Xwt.ListView lv;

			public GroupCollection (Xwt.ListView lv)
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
			Xwt.ListView lv;

			public ItemCollection (Xwt.ListView lv)
			{
				this.lv = lv;
			}

			protected override void InsertItem (int index, UiListViewItem item)
			{
				base.InsertItem (index, item);
				lv.DataSource.SetValue (index, 0, item.Checked);
				lv.DataSource.SetValue (index, 1, item.Text);
				if (item.Selected)
					lv.SelectRow (index);
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
