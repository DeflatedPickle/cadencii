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
		void AddRow (string[] items, bool selected);
		View View { get; set; } 
		IList<UiListViewColumn> Columns { get; }
		IList SelectedIndices { get; }
		IList<UiListViewItem> Items { get; }
		IList<UiListViewGroup> Groups { get; }
	}

	public interface UiListViewGroup
	{
		object Native { get; set; }
		string Header { get; set; }
		string Name { get; set; }
		HorizontalAlignment HeaderAlignment { get; set; }
	}

	public interface UiListViewColumn
	{
        object Native { get; set;  }
		int Width { get; set; }
		string Text { get; set; }
	}

	public interface UiListViewItem
	{
		object Native { get; set; }
		string Text { get; set; }
		Color BackColor { get; set; }
		bool Checked { get; set; }
		bool Selected { get; set; }
		UiListViewItem GetSubItem (int i);
	}
}

