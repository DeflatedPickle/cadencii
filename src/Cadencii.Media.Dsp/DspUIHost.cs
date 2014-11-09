using System;
using System.Linq;
using Cadencii.Gui.Toolkit;

namespace cadencii.dsp
{
	public abstract class DspUIHost
	{
		public static Type CurrentType { get; set; }

		public static DspUIHost Create (VSTiDriverBase vstiDriverBase)
		{
			if (CurrentType == null)
				throw new InvalidOperationException ("DspUIHost type is not registered yet.");
			return (DspUIHost) Activator.CreateInstance (CurrentType, vstiDriverBase);
		}

		public abstract PluginUI GetPluginUI (UiForm window);

		public abstract void ClosePluginUI ();
	}
}

