using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class PropertyGridImpl : PropertyGridBase, UiPropertyGrid
	{
		// FIXME: implement
		public event EventHandler SelectedGridItemChanged;

		// FIXME: implement
		public event EventHandler PropertyValueChanged;

		internal UiGridItem SelectedGridItem;

		// FIXME: implement
		UiGridItem UiPropertyGrid.SelectedGridItem {
			get { return SelectedGridItem; }
		}

		// FIXME: implement
		object[] UiPropertyGrid.SelectedObjects { get; set; }

		// FIXME: implement
		PropertySort UiPropertyGrid.PropertySort { get; set; }
	}
}

