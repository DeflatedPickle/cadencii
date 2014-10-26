using System;
using cadencii.vsq;

namespace cadencii
{
	public interface FormNoteExpressionConfig : UiForm
	{
		NoteHeadHandle EditedNoteHeadHandle { get; }

		void applyLanguage ();

		int PMBendDepth { get; set; }

		int PMBendLength { get; set ; }

		int PMbPortamentoUse { get; set; }

		int DEMdecGainRate { get; set; }

		int DEMaccent { get; set; }

		bool ApplyCurrentTrack { get; }
	}
}

