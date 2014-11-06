using System;
using Cadencii.Gui;

namespace cadencii
{
	public interface UiToolTip : UiComponent
	{
		int AutoPopDelay { get; set; }
		int InitialDelay { get; set; }
		bool OwnerDraw { get; set; }
		int ReshowDelay { get; set; }
		void Show (string text, UiControl control, Point point, int duration);
		void Hide (UiControl control);
		event EventHandler<DrawToolTipEventArgs> Draw;
	}
}

