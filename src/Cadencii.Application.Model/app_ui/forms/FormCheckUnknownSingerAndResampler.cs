using System;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Forms
{
	public interface FormCheckUnknownSingerAndResampler : UiForm
	{
		bool isSingerChecked ();

		string getSingerPath ();

		bool isResamplerChecked ();

		string getResamplerPath ();
	}
}

