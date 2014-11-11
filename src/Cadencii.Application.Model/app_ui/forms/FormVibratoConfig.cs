using System;
using Cadencii.Media.Vsq;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface FormVibratoConfig : UiForm
	{
		void applyLanguage();
		VibratoHandle getVibratoHandle();
	}
}

