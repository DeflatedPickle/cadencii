using System;
using System.Collections.Generic;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public static class WinformsExtensions
	{
		public static void Add (this IList<UiToolStripItem> list, string s, Cadencii.Gui.Image o, EventHandler h)
		{
			list.Add (new ToolStripMenuItemImpl (s, o, h));
		}

		public static void Add<C> (this IList<C> list, string s, Cadencii.Gui.Image o, EventHandler h) where C : UiToolStripMenuItem
		{
			list.Add ((C) (object) new ToolStripMenuItemImpl (s, o, h));
		}

		public static void AddRow(this System.Windows.Forms.ListView list_view, string[] items, bool selected = false)
		{
			var item = new System.Windows.Forms.ListViewItem(items);
			item.Checked = selected;
			if (list_view.Columns.Count < items.Length) {
				for (int i = list_view.Columns.Count; i < items.Length; i++) {
					list_view.Columns.Add("");
				}
			}
			list_view.Items.Add(item);
		}

		public static void SetColumnHeaders(this System.Windows.Forms.ListView list_view, string[] headers)
		{
			if (list_view.Columns.Count < headers.Length) {
				for (int i = list_view.Columns.Count; i < headers.Length; i++) {
					list_view.Columns.Add("");
				}
			}
			for (int i = 0; i < headers.Length; i++) {
				list_view.Columns[i].Text = headers[i];
			}
		}

	}
}

