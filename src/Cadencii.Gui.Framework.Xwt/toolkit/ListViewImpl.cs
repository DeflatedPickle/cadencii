using System;
using System.Collections.Generic;

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

		View UiListView.View {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		IList<UiListViewColumn> UiListView.Columns {
			get {
				throw new NotImplementedException ();
			}
		}

		System.Collections.IList UiListView.SelectedIndices {
			get {
				throw new NotImplementedException ();
			}
		}

		IList<UiListViewItem> UiListView.Items {
			get {
				throw new NotImplementedException ();
			}
		}

		IList<UiListViewGroup> UiListView.Groups {
			get {
				throw new NotImplementedException ();
			}
		}
	}
}

