using System;
using Cadencii.Media.Vsq;
using NMouseEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.MouseEventArgs>;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
{
	public interface DraggableBButton : UiControl
	{
		string Text { get; set; }

		Cadencii.Gui.Image Image { get; set; }

		IconDynamicsHandle getHandle ();

		void setHandle (IconDynamicsHandle handle);

		void DoDragDrop (IconDynamicsHandle handle, DragDropEffects all);
	}
}

