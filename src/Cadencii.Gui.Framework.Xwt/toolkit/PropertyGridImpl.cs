using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class PropertyGridImpl : PropertyGridBase, UiPropertyGrid
	{
		event EventHandler UiPropertyGrid.SelectedGridItemChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiPropertyGrid.PropertyValueChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		UiGridItem UiPropertyGrid.SelectedGridItem {
			get {
				throw new NotImplementedException ();
			}
		}

		object[] UiPropertyGrid.SelectedObjects {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		PropertySort UiPropertyGrid.PropertySort {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
	}
}

