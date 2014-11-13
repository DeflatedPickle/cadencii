using System;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Forms
{
	public interface FormTempoConfig : UiForm
	{
		int getBeatCount ();
		int getClock ();
		float getTempo ();
	}
}

