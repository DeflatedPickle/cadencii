using System;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public class ListViewItemImpl : UiListViewItem
	{
		#region UiListViewItem implementation

		List<ListViewItemImpl> items = new List<ListViewItemImpl> ();

		UiListViewItem UiListViewItem.GetSubItem (int i)
		{
			return items [i];
		}

		void UiListViewItem.AddSubItem (string subText)
		{
			items.Add (new ListViewItemImpl () { Text = subText });
		}

		// FIXME: maybe we need ListStore(ListView.DataSource) to "implement" those properties, if ever possible.
		object UiListViewItem.Native { get; set; }

		public string Text { get; set; }

		Color UiListViewItem.BackColor { get; set; }

		bool UiListViewItem.Checked { get; set; }

		bool UiListViewItem.Selected { get; set; }
		#endregion
	}
}

