using System;
using System.Collections.Generic;

namespace cadencii
{
	public interface UiToolBar : UiControl
	{
		List<UiToolBarButton> Buttons { get; }
		event EventHandler<UiToolBarButtonEventArgs> ButtonClick;
	}
}

