using System;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Forms
{
	public enum FormMidiMode
        {
            IMPORT,
            EXPORT,
            IMPORT_VSQ,
        }

	public interface FormMidiImExport : UiForm
	{
		FormMidiMode Mode { get; set; }
		UiListView ListTrack { get; }
		void applyLanguage();

         double getOffsetSeconds();

         int getOffsetClocks();

        bool isSecondBasis();

         bool isVocaloidMetatext();

         bool isVocaloidNrpn();

         bool isTempo();

         void setTempo(bool value);

         bool isTimesig();

         void setTimesig(bool value);

         bool isNotes();

         bool isLyric();

        bool isPreMeasure();
	}
}

