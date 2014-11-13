using System;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
{
	public interface PropertyPanel : UiControl
	{
		event CommandExecuteRequiredEventHandler CommandExecuteRequired;

		bool isEditing ();

		void updateValue (int selected);
	}
}

