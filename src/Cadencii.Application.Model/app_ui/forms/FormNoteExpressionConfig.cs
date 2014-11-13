using System;
using Cadencii.Media.Vsq;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Forms
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

