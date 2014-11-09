using System;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface FormCheckUnknownSingerAndResampler : UiForm
	{
		bool isSingerChecked ();

		string getSingerPath ();

		bool isResamplerChecked ();

		string getResamplerPath ();
	}
}

