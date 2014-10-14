using System;

namespace cadencii
{
	public interface FormSequenceConfig : UiForm
	{
		void applyLanguage ();

		int getPreMeasure ();

		void setPreMeasure (int value);

		int getSampleRate ();

		void setSampleRate (int value);

		bool isWaveFileOutputFromMasterTrack ();

		void setWaveFileOutputFromMasterTrack (bool value);

		int getWaveFileOutputChannel ();

		void setWaveFileOutputChannel (int value);
	}
}

