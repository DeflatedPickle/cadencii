using System;

namespace cadencii
{
	public interface PropertyPanel : UiControl
	{
		event CommandExecuteRequiredEventHandler CommandExecuteRequired;

		bool isEditing ();

		void updateValue (int selected);
	}
}

