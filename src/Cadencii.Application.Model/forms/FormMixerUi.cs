using System;
using cadencii.java.awt;

namespace cadencii
{
	public interface FormMixerUi : UiBase
	{
		bool TopMost { get; set; }

		void setPreviousAlwaysOnTop (bool previous);

		bool getPreviousAlwaysOnTop ();

		VolumeTracker getVolumeTrackerMaster ();

		VolumeTracker getVolumeTrackerBgm (int i);

		VolumeTracker getVolumeTracker (int track);

		void applyLanguage ();

		void applyShortcut (Keys shortcut);

		void updateStatus ();

		event FederChangedEventHandler FederChanged;

		event PanpotChangedEventHandler PanpotChanged;

		event MuteChangedEventHandler MuteChanged;

		event SoloChangedEventHandler SoloChanged;

		event EventHandler FormClosing;
	}
}

