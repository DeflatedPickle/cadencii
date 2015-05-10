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
		bool ShowGroups { get; set; }
		bool CheckBoxes { get; set; }
		bool FullRowSelect { get; set; }
		View View { get; set; } 
		IList<UiListViewColumn> Columns { get; }
		IList SelectedIndices { get; }
		IList<UiListViewItem> Items { get; }
		IList<UiListViewGroup> Groups { get; }
	}

	// OK, I'm redesigning this as a class, but keeping name as is (to not get confused as winforms class)
	public class UiListViewGroup
	{
		public object Native { get; set; }
		public string Header { get; set; }
		public string Name { get; set; }
		public HorizontalAlignment HeaderAlignment { get; set; }
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
		void AddSubItem (string subText);
	}
}

