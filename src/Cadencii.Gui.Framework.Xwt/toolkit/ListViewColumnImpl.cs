using System;

namespace Cadencii.Gui.Toolkit
{
	public class ListViewColumnImpl : UiListViewColumn
	{
		string text;
		Xwt.ListViewColumn impl;

		#region UiListViewColumn implementation

		object UiListViewColumn.Native {
			get { return impl; }
			set {
				impl = (Xwt.ListViewColumn) value;
				impl.Title = text;
			}
		}

		// ignore. No way to set it.
		int UiListViewColumn.Width { get; set; }

		string UiListViewColumn.Text {
			get { return text; }
			set {
				text = value;
				if (impl != null)
					impl.Title = value;
			}
		}

		#endregion
	}
}

