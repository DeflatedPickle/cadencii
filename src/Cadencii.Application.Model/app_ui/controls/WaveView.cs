using System;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Models;

namespace Cadencii.Application.Controls
{
	public interface WaveView : UiUserControl
	{
		WaveViewModel Model { get; }
	}
}

