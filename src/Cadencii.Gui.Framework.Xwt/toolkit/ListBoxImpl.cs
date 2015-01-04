using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class ListBoxImpl : UiListBox
	{
		int UiListBox.ItemHeight {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		int UiListBox.SelectedIndex {
			get { return this.SelectedRow; }
			set { this.SelectRow (value); }
		}

		System.Collections.IList UiListBox.SelectedIndices {
			get { return this.SelectedRows; }
		}

		bool UiListBox.FormattingEnabled {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		event System.EventHandler Cadencii.Gui.Toolkit.UiListBox.SelectedIndexChanged {
			add { this.SelectionChanged += value; }
			remove { this.SelectionChanged -= value; }
		}

		System.Collections.IList Cadencii.Gui.Toolkit.UiListBox.Items {
			get { return this.Items; }
		}
	}
}

