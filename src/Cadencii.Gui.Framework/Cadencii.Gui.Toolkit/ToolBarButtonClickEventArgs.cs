using System;

namespace Cadencii.Gui.Toolkit
{
	public class ToolBarButtonClickEventArgs : EventArgs
	{
		public ToolBarButtonClickEventArgs ()
		{
		}

		public UiToolBarButton Button { get; set; }
	}
}

