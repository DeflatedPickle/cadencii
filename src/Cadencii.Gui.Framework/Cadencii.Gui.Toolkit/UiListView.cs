using System;
using System.Collections.Generic;
using Cadencii.Gui;
using System.Collections;

namespace Cadencii.Gui.Toolkit
{
	public interface UiListView : UiControl
	{
		event EventHandler SelectedIndexChanged;
		bool UseCompatibleStateImageBehavior { get; set; }
		bool CheckBoxes { get; set; }
		bool FullRowSelect { get; set; }
		int ItemCount { get; }
		UiListViewItem GetItem (int i);
		void RemoveItemAt (int i);
		void ClearItems ();
		void AddRow(string[] items, bool selected);
		//void AddGroups (IEnumerable<UiListViewGroup> groups);
		View View { get; set; } 
		IList<UiListViewColumn> Columns { get; }
		IList SelectedIndices { get; }
	}

	/*
	public interface UiListViewGroup
	{
		object Native { get; }
		string Header { get; set; }
		string Name { get; set; }
	}
	*/

	public interface UiListViewColumn
	{
		object Native { get; }
		int Width { get; set; }
		string Text { get; set; }
	}

	public interface UiListViewItem
	{
		object Native { get; }
		string Text { get; set; }
		Color BackColor { get; set; }
		bool Checked { get; set; }
		bool Selected { get; set; }
		UiListViewItem GetSubItem (int i);
	}
}

