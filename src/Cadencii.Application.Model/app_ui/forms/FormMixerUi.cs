using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;
using cadencii;

namespace Cadencii.Application.Forms
{
	public interface FormMixerUi : UiForm
	{
		// This is not a form specific feature, but since Control.DoubleBuffered is protected,
		// it cannot be an interface member (technically it can, but it's annoying to add impl.
		// all around) so expose it here.
		bool DoubleBuffered { get; set; }

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

