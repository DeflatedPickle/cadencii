using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiHBox : UiControl
	{
		void PackStart (UiControl control);
		void PackEnd (UiControl control);
	}
}

