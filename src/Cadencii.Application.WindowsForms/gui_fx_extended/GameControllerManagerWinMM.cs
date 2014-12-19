using System;
using Cadencii.Platform.Windows;
using cadencii;

namespace Cadencii.Application.Forms
{
	public class GameControllerManagerWinMM : GameControllerManager
	{
		#region GameControllerManager implementation

		int GameControllerManager.InitializeJoyPad ()
		{
			return winmmhelp.JoyInit();
		}

		int GameControllerManager.GetNumberOfJoyPads ()
		{
			return winmmhelp.JoyGetNumJoyDev ();
		}

		int GameControllerManager.GetNumberOfButtons (int deviceIndex)
		{
			return winmmhelp.JoyGetNumButtons (deviceIndex);
		}

		JoyPadStatus GameControllerManager.GetJoyPadStatus ()
		{
			byte[] buttons;
			int pov0;
			bool ret = winmmhelp.JoyGetStatus (0, out buttons, out pov0);
			return new JoyPadStatus (buttons, pov0);
		}

		#endregion
	}
}

