using System;

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

