using System;
using System.Linq;

namespace cadencii.dsp
{
	public abstract class UIHost
	{
		public static UIHost Create ()
		{
			var type = AppDomain.CurrentDomain.GetAssemblies ().SelectMany (a => a.GetTypes ()).First (t => t.IsSubclassOf (typeof(UIHost)));
			return (UIHost) Activator.CreateInstance (type);
		}

		public abstract object GetPluginUI (object nativeWindow);

		public abstract void ClosePluginUI ();
	}
}

