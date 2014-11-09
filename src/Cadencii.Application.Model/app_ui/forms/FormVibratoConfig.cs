using System;
using cadencii.vsq;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface FormVibratoConfig : UiForm
	{
		void applyLanguage();
		VibratoHandle getVibratoHandle();
	}
}

