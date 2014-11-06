using System;
using cadencii.vsq;
using NMouseEventHandler = Cadencii.Gui.MouseEventHandler;

namespace cadencii
{
	public interface DraggableBButton : UiControl
	{
		string Text { get; set; }

		Cadencii.Gui.Image Image { get; set; }

		IconDynamicsHandle getHandle ();

		void setHandle (IconDynamicsHandle handle);

		void BringToFront ();

		void DoDragDrop (IconDynamicsHandle handle, Cadencii.Gui.DragDropEffects all);
	}
}

