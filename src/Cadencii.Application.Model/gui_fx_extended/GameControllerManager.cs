using System;

namespace Cadencii.Application
{
	public interface GameControllerManager
	{
		// returns number of joypad controllers, or negative number for failure
		int InitializeJoyPad ();
		JoyPadStatus GetJoyPadStatus ();
	}

	public struct JoyPadStatus
	{
		public readonly byte[] Buttons;
		public readonly int Pov0;

		public JoyPadStatus (byte [] buttons, int pov0)
		{
			Buttons = buttons;
			Pov0 = pov0;
		}
	}
}

