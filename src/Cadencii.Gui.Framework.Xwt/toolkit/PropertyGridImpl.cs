using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class PropertyGridImpl : PropertyGridBase, UiPropertyGrid
	{
		// FIXME: implement
		public event EventHandler SelectedGridItemChanged;

		// FIXME: implement
		public event EventHandler PropertyValueChanged;

		UiGridItem UiPropertyGrid.SelectedGridItem {
			get {
				throw new NotImplementedException ();
			}
		}

		// FIXME: implement
		object[] UiPropertyGrid.SelectedObjects { get; set; }

		// FIXME: implement
		PropertySort UiPropertyGrid.PropertySort { get; set; }
	}
}

