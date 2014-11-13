using System;
using System.Collections.Generic;
using Cadencii.Media.Vsq;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Forms
{
	public interface FormVibratoPreset : UiForm
	{
		List<VibratoHandle> getResult();
	}
}

