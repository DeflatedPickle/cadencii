using System;
using cadencii.vsq;
using NMouseEventHandler = cadencii.java.awt.MouseEventHandler;

namespace cadencii
{
	public interface DraggableBButton : UiControl
	{
		string Text { get; set; }

		cadencii.java.awt.Image Image { get; set; }

		IconDynamicsHandle getHandle ();

		void setHandle (IconDynamicsHandle handle);

		void BringToFront ();

		void DoDragDrop (IconDynamicsHandle handle, cadencii.java.awt.DragDropEffects all);
	}
}

