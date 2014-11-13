using System;
using System.Collections.Generic;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using cadencii;

namespace Cadencii.Application.Forms
{
	public interface FormShortcutKeys : UiForm
	{
		SortedDictionary<string, ValuePair<string, Keys[]>> getResult ();
	}
}

