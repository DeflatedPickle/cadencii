using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiPropertyGrid : UiControl
	{
		UiGridItem SelectedGridItem { get; }
		object [] SelectedObjects { get; set; }
		PropertySort PropertySort { get; set; }
		event EventHandler SelectedGridItemChanged;
		event EventHandler PropertyValueChanged;

	}
}

