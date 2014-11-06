using System;
using Cadencii.Gui;

namespace cadencii
{
	public static class VolumeTrackerController
	{
		public const int WIDTH = 85;
		public const int HEIGHT = 284;
	}

	public interface VolumeTracker : UiUserControl, IAmplifierView
	{
		int getTrack ();

		void setLocation (int i, int i2);

		void setTrack (int i);

		void setTitle (string empty);

		void setSoloButtonVisible (bool b);

		void setPanpot (int i);

		void setNumber (string master);

		bool isSolo ();

		void setSolo (bool b);

		bool isMuted ();

		void setMuted (bool b);

		void setFeder (int i);

		event PanpotChangedEventHandler PanpotChanged;

		event FederChangedEventHandler FederChanged;

		event EventHandler MuteButtonClick;

		event EventHandler SoloButtonClick;
	}
}

