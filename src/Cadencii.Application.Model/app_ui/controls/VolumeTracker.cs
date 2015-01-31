using System;
using Cadencii.Gui.Toolkit;
using cadencii;
using Cadencii.Application.Models;

namespace Cadencii.Application.Controls
{
	public static class VolumeTrackerController
	{
		public const int WIDTH = 85;
		public const int HEIGHT = 284;
	}

	public interface VolumeTracker : UiUserControl, IAmplifierView
	{
		VolumeTrackerModel Model { get; }

		UiVTrackBar trackFeder { get; }
		UiHTrackBar trackPanpot { get; }
		UiTextBox txtPanpot { get; }
		UiLabel lblTitle { get; }
		UiTextBox txtFeder { get; }
		UiCheckBox chkMute { get; }
		UiCheckBox chkSolo { get; }
	}
}

