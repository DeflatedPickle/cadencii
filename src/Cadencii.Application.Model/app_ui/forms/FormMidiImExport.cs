using System;
using Cadencii.Gui.Toolkit;

namespace cadencii
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
		UiListView listTrack { get ; set; }
		void applyLanguage();

         double getOffsetSeconds();

         int getOffsetClocks();

        bool isSecondBasis();

        FormMidiMode getMode();

        void setMode(FormMidiMode value);

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

