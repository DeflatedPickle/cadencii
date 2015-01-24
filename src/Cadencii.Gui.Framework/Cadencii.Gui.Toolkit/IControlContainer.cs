using System;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public interface IControlContainer
	{
		void AddControl (UiControl control);
		void RemoveControl (UiControl control);
		//IList<UiControl> Controls { get; }
	}
}

