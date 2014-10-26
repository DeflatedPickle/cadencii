using System;
using System.Collections.Generic;
using cadencii.java.awt;

namespace cadencii
{
	public interface FormShortcutKeys : UiForm
	{
		SortedDictionary<string, ValuePair<string, Keys[]>> getResult ();
	}
}

