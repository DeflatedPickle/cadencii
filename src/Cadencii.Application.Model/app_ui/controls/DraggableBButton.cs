using System;
using Cadencii.Media.Vsq;
using NMouseEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.MouseEventArgs>;
using Cadencii.Gui.Toolkit;
using Cadencii.Gui;

namespace Cadencii.Application.Controls
{
	public interface DraggableBButton : UiButton
	{
		DialogResult DialogResult { get; set; }
		IconDynamicsHandle IconHandle { get; set; }

		void DoDragDrop (IconDynamicsHandle handle, DragDropEffects all);
	}
}

