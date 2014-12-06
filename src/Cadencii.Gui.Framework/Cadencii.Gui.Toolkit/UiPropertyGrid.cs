using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiPropertyGrid : UiControl
	{
		UiGridItem SelectedGridItem { get; }
		object [] SelectedObjects { get; set; }
		bool HelpVisible { get; set; }
		PropertySort PropertySort { get; set; }
		bool ToolbarVisible { get; set; }
		event EventHandler SelectedGridItemChanged;
		event EventHandler PropertyValueChanged;

	}
}

