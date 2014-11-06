using System;
using System.Collections.Generic;
using Cadencii.Gui;

namespace cadencii
{
	public interface UiListView : UiControl
	{
		bool UseCompatibleStateImageBehavior { get; set; }
		bool CheckBoxes { get; set; }
		bool FullRowSelect { get; set; }
		int ItemCount { get; }
		UiListViewItem GetItem (int i);
		void ClearItems ();
		void AddRow(string[] items, bool selected);
		void SetColumnHeaders(string[] headers);
		UiListViewColumn GetColumn(int i);
		void AddGroups (IEnumerable<UiListViewGroup> groups);
		View View { get; set; } 
	}

	public interface UiListViewGroup
	{
		object Native { get; }
		string Header { get; set; }
		string Name { get; set; }
	}

	public interface UiListViewColumn
	{
		object Native { get; }
		int Width { get; set; }
	}

	public interface UiListViewItem
	{
		object Native { get; }
		string Text { get; }
		bool Checked { get; set; }
		UiListViewItem GetSubItem (int i);
	}
}

