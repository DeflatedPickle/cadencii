using System;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Forms
{
	public interface FormSplashUi : UiForm
	{
		void addIconThreadSafe (string path_image, string vOICENAME);
	}
}

