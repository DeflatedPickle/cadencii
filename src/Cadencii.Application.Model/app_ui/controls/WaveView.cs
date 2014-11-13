using System;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
{
	public interface WaveView : UiControl
	{
		void setScale (float value);

		float getScale ();

		void setAutoMaximize (bool value);

		void reloadPartial (int i, string file, double sec_start, double sec_end);

		void load (int track, string file);

		void unloadAll ();
	}
}

