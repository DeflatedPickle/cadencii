using System;
using System.Linq;

namespace Cadencii.Application
{
	public abstract class Clipboard
	{
		public abstract void SetDataObject (object data, bool copy);
		public abstract object GetDataObject (Type dataType);
		public abstract void SetText (string value);
		public abstract string GetText ();
	}
}

