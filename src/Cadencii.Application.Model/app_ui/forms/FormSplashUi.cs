using System;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface FormSplashUi : UiForm
	{
		void addIconThreadSafe (string path_image, string vOICENAME);
	}
}

