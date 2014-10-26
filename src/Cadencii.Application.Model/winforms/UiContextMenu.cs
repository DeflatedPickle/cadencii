using System;
using cadencii.java.awt;

namespace cadencii
{
	public interface UiContextMenu : UiControl
	{
		void Show (UiControl control, Point point);
	}
}

