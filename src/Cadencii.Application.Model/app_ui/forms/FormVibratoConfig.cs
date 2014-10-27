using System;
using cadencii.vsq;

namespace cadencii
{
	public interface FormVibratoConfig : UiForm
	{
		void applyLanguage();
		VibratoHandle getVibratoHandle();
	}
}

