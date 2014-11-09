using System;
using System.Collections.Generic;
using cadencii.vsq;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface FormVibratoPreset : UiForm
	{
		List<VibratoHandle> getResult();
	}
}

