using System;
using cadencii.java.awt;

namespace cadencii
{
	public class ToolBarButtonImpl : System.Windows.Forms.ToolBarButton, UiToolBarButton
	{
		ToolBarButtonStyle UiToolBarButton.Style {
			get { return (ToolBarButtonStyle) Style; }
			set { Style = (System.Windows.Forms.ToolBarButtonStyle) value; }
		}

		cadencii.java.awt.Rectangle UiToolBarButton.Rectangle {
			get { return Rectangle.ToAwt (); }
		}
	}
}
