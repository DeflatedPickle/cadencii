using System;
using cadencii.vsq;
using NMouseEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.MouseEventArgs>;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface DraggableBButton : UiControl
	{
		string Text { get; set; }

		Cadencii.Gui.Image Image { get; set; }

		IconDynamicsHandle getHandle ();

		void setHandle (IconDynamicsHandle handle);

		void BringToFront ();

		void DoDragDrop (IconDynamicsHandle handle, DragDropEffects all);
	}
}

