using System;

namespace Cadencii.Application
{
	public interface GameControllerManager
	{
		// returns number of joypad controllers, or negative number for failure
		int InitializeJoyPad ();
		int GetNumberOfJoyPads ();
		int GetNumberOfButtons (int deviceIndex);
		JoyPadStatus GetJoyPadStatus ();
	}

	public struct JoyPadStatus
	{
		public readonly byte[] Buttons;
		public readonly int Pov;

		public JoyPadStatus (byte [] buttons, int pov0)
		{
			Buttons = buttons;
			Pov = pov0;
		}
	}
}

