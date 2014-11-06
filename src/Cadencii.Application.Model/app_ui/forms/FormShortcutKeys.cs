using System;
using System.Collections.Generic;
using Cadencii.Gui;

namespace cadencii
{
	public interface FormShortcutKeys : UiForm
	{
		SortedDictionary<string, ValuePair<string, Keys[]>> getResult ();
	}
}

