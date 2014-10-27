using System;
using System.Collections.Generic;
using cadencii.vsq;

namespace cadencii
{
	public interface FormVibratoPreset : UiForm
	{
		List<VibratoHandle> getResult();
	}
}

