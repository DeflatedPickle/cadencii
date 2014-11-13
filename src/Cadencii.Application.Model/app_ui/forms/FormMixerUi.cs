using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;
using cadencii;

namespace Cadencii.Application.Forms
{
	public interface FormMixerUi : UiForm
	{
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
	}
}

