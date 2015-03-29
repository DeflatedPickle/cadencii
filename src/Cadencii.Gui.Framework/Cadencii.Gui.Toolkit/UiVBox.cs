using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiVBox : UiControl
	{
		void PackStart (UiControl control);
		void PackEnd (UiControl control);
	}
}

