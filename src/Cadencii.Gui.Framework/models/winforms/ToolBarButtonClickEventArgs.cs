using System;

namespace cadencii
{
	public class ToolBarButtonClickEventArgs : EventArgs
	{
		public ToolBarButtonClickEventArgs ()
		{
		}

		public UiToolBarButton Button { get; set; }
	}
}

