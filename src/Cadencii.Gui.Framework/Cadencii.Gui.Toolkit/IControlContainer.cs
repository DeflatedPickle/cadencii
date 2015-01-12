using System;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public interface IControlContainer
	{
		IList<UiControl> Controls { get; }
	}
}

