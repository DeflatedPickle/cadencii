using System;

namespace cadencii
{
	public interface FormSingerStyleConfig : UiForm
	{
		void applyLanguage ();

		int PMBendDepth { get; set; }

		int PMBendLength { get; set; }

		int PMbPortamentoUse { get; set; }

		int DEMdecGainRate { get; set; }

		int DEMaccent { get; set; }

		bool ApplyCurrentTrack { get; }
	}
}

