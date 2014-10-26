using System;

namespace cadencii
{
	public class UiToolBarButtonEventArgs : EventArgs
	{
		public UiToolBarButtonEventArgs ()
		{
		}

		public UiToolBarButton Button { get; set; }
	}
}

