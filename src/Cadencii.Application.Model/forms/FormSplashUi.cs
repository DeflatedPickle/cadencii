using System;

namespace cadencii
{
	public interface FormSplashUi : UiBase
	{
		object Invoke (Delegate d);

		void Close ();

		void addIconThreadSafe (string path_image, string vOICENAME);
	}
}

