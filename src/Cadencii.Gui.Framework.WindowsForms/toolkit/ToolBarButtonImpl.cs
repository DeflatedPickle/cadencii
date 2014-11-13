using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using cadencii;

namespace Cadencii.Gui.Toolkit
{
	public class ToolBarButtonImpl : System.Windows.Forms.ToolBarButton, UiToolBarButton
	{
		ToolBarButtonStyle UiToolBarButton.Style {
			get { return (ToolBarButtonStyle) Style; }
			set { Style = (System.Windows.Forms.ToolBarButtonStyle) value; }
		}

		Cadencii.Gui.Rectangle UiToolBarButton.Rectangle {
			get { return Rectangle.ToAwt (); }
		}
	}
}
