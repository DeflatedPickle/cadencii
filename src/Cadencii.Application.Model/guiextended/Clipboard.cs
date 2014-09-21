using System;
using System.Linq;

namespace cadencii
{
	public abstract class Clipboard
	{
		public abstract void SetDataObject (ClipboardEntry data, bool copy);
		public abstract ClipboardEntry GetDataObject ();
		public abstract void SetText (string value);
		public abstract string GetText ();
	}
}

