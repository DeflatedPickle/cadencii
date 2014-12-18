using System;

namespace Cadencii.Gui
{
	public abstract class JoyPads
	{
		protected JoyPads ()
		{
		}

		public abstract int GetNumberOfDevices ();

		public abstract int GetNumberOfButtons (int deviceIndex);

		public abstract bool GetStatus (int deviceIndex, out byte [] buttons, out int pov);
	}
}

