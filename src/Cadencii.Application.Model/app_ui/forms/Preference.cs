using System;
using System.Collections.Generic;
using cadencii.vsq;

namespace cadencii
{
	public interface Preference : UiForm
	{
		bool isEnableWideCharacterWorkaround ();

		void setEnableWideCharacterWorkaround (bool value);

		bool isUseUserDefinedAutoVibratoType ();

		void setUseUserDefinedAutoVibratoType (bool value);

		void setDefaultSynthesizer (RendererKind value);

		RendererKind getDefaultSynthesizer ();

		int getBufferSize ();

		void setBufferSize (int value);

		bool isVocaloid1Required ();

		void setVocaloid1Required (bool value);

		bool isVocaloid2Required ();

		void setVocaloid2Required (bool value);

		bool isAquesToneRequired ();

		void setAquesToneRequired (bool value);

		bool isAquesTone2Required ();

		void setAquesTone2Requried (bool value);

		bool isUseProjectCache ();

		void setUseProjectCache (bool value);

		bool isUseSpaceKeyAsMiddleButtonModifier ();

		void setUseSpaceKeyAsMiddleButtonModifier (bool value);

		int getAutoBackupIntervalMinutes ();

		void setAutoBackupIntervalMinutes (int value);

		bool isSelfDeRomantization ();

		void setSelfDeRomantization (bool value);

		int getMidiInPort ();

		void setMidiInPort (int value);

		bool isCurveVisibleVel ();

		void setCurveVisibleVel (bool value);

		bool isCurveVisibleAccent ();

		void setCurveVisibleAccent (bool value);

		bool isCurveVisibleDecay ();

		void setCurveVisibleDecay (bool value);

		bool isCurveVisibleVibratoRate ();

		void setCurveVisibleVibratoRate (bool value);

		bool isCurveVisibleVibratoDepth ();

		void setCurveVisibleVibratoDepth (bool value);

		bool isCurveVisibleDyn ();

		void setCurveVisibleDyn (bool value);

		bool isCurveVisibleBre ();

		void setCurveVisibleBre (bool value);

		bool isCurveVisibleBri ();

		void setCurveVisibleBri (bool value);

		bool isCurveVisibleCle ();

		void setCurveVisibleCle (bool value);

		bool isCurveVisibleOpe ();

		void setCurveVisibleOpe (bool value);

		bool isCurveVisiblePor ();

		void setCurveVisiblePor (bool value);

		bool isCurveVisibleGen ();

		void setCurveVisibleGen (bool value);

		bool isCurveVisiblePit ();

		void setCurveVisiblePit (bool value);

		bool isCurveVisiblePbs ();

		void setCurveVisiblePbs (bool value);

		bool isCurveVisibleFx2Depth ();

		void setCurveVisibleFx2Depth (bool value);

		bool isCurveVisibleHarmonics ();

		void setCurveVisibleHarmonics (bool value);

		bool isCurveVisibleReso1 ();

		void setCurveVisibleReso1 (bool value);

		bool isCurveVisibleReso2 ();

		void setCurveVisibleReso2 (bool value);

		bool isCurveVisibleReso3 ();

		void setCurveVisibleReso3 (bool value);

		bool isCurveVisibleReso4 ();

		void setCurveVisibleReso4 (bool value);

		bool isCurveVisibleEnvelope ();

		void setCurveVisibleEnvelope (bool value);

		bool isCurveSelectingQuantized ();

		void setCurveSelectingQuantized (bool value);

		bool isPlayPreviewWhenRightClick ();

		void setPlayPreviewWhenRightClick (bool value);

		int getMouseHoverTime ();

		void setMouseHoverTime (int value);

		int getPxTrackHeight ();

		void setPxTrackHeight (int value);

		bool isKeepLyricInputMode ();

		void setKeepLyricInputMode (bool value);

		int getMaximumFrameRate ();

		void setMaximumFrameRate (int value);

		bool isScrollHorizontalOnWheel ();

		void setScrollHorizontalOnWheel (bool value);

		void applyLanguage ();

		string getLanguage ();

		ClockResolution getControlCurveResolution ();

		void setControlCurveResolution (ClockResolution value);

		int getPreSendTime ();

		void setPreSendTime (int value);

		bool isEnableAutoVibrato ();

		void setEnableAutoVibrato (bool value);

		string getAutoVibratoType1 ();

		void setAutoVibratoType1 (string value);

		string getAutoVibratoType2 ();

		void setAutoVibratoType2 (string value);

		string getAutoVibratoTypeCustom ();

		void setAutoVibratoTypeCustom (string icon_id);

		int getAutoVibratoThresholdLength ();

		void setAutoVibratoThresholdLength (int value);

		DefaultVibratoLengthEnum getDefaultVibratoLength ();

		void setDefaultVibratoLength (DefaultVibratoLengthEnum value);

		bool isCursorFixed ();

		void setCursorFixed (bool value);

		int getWheelOrder ();

		void setWheelOrder (int value);

		java.awt.Font getScreenFont ();

		void setScreenFont (java.awt.Font value);

		java.awt.Font getBaseFont ();

		void setBaseFont (java.awt.Font value);

		string getDefaultSingerName ();

		void setDefaultSingerName (string value);

		void copyResamplersConfig (List<string> ret);

		void setResamplersConfig (List<string> path);

		string getPathWavtool ();

		void setPathWavtool (string value);

		string getPathAquesTone ();

		void setPathAquesTone (string value);

		string getPathAquesTone2 ();

		void setPathAquesTone2 (string value);

		List<SingerConfig> getUtausingers ();

		void setUtausingers (List<SingerConfig> value);
	}
}

