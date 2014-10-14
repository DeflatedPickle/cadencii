using System;
using cadencii.java.awt;

namespace cadencii
{
	public interface UiPanel : UiControl
	{
		BorderStyle BorderStyle { get; set; }
		void AddControl (UiControl child);
		void ClearControls ();

		event EventHandler SizeChanged;
	}
}

