using System;

namespace cadencii
{
	public interface FormTempoConfig : UiForm
	{
		int getBeatCount ();
		int getClock ();
		float getTempo ();
	}
}
