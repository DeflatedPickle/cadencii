using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using cadencii;

namespace Cadencii.Gui.Toolkit
{
	public class ToolBarButtonImpl : System.Windows.Forms.ToolBarButton, UiToolBarButton
	{
		Cadencii.Gui.Rectangle UiToolBarButton.Rectangle {
			get { return Rectangle.ToGui (); }
		}
	}

	public class ToolBarToggleButtonImpl : ToolBarButtonImpl, UiToolBarToggleButton
	{
		public ToolBarToggleButtonImpl ()
		{
			this.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
		}
	}

	public class ToolBarSeparatorButtonImpl : ToolBarButtonImpl, UiToolBarSeparatorButton
	{
		public ToolBarSeparatorButtonImpl ()
		{
			this.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
		}
	}
}
