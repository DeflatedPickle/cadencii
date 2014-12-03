using System;
using Cadencii.Media.Vsq;
using NMouseEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.MouseEventArgs>;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
{
	public interface DraggableBButton : UiButton
	{
		IconDynamicsHandle IconHandle { get; set; }

		void DoDragDrop (IconDynamicsHandle handle, DragDropEffects all);
	}
}

