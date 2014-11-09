using System;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface FormTempoConfig : UiForm
	{
		int getBeatCount ();
		int getClock ();
		float getTempo ();
	}
}

