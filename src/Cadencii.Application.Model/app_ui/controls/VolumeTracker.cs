using System;
using Cadencii.Gui.Toolkit;
using cadencii;

namespace Cadencii.Application.Controls
{
	public static class VolumeTrackerController
	{
		public const int WIDTH = 85;
		public const int HEIGHT = 284;
	}

	public interface VolumeTracker : UiUserControl, IAmplifierView
	{
		bool DoubleBuffered { get; set; }

		int Track { get; set; }

		bool SoloButtonVisible { get; set; }

		bool Solo { get; set; }

		bool Muted { get; set; }

		string Number { set; }

		int Panpot { set; }

		int Feder { get; set; }

		string Title { get; set; }

		event PanpotChangedEventHandler PanpotChanged;

		event FederChangedEventHandler FederChanged;

		event EventHandler MuteButtonClick;

		event EventHandler SoloButtonClick;
	}
}

