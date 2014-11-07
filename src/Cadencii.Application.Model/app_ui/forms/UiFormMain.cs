/*
 * FormMainUi.cs
 * Copyright © 2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System.Collections.Generic;
using Cadencii.Gui;


namespace cadencii
{
    /// <summary>
    /// メイン画面の実装クラスが持つべきメソッドを定義するインターフェース
    /// </summary>
    public interface UiFormMain : UiForm
    {
		FormMainModel Model { get; }

		Image Resource_piano { get; }
		Image Resource_slash { get; }

		bool mFormActivated { get; set; }
		UiToolStripStatusLabel stripLblGameCtrlMode { get; set; }
		void forward();
		void rewind();
		bool isEdited();

		UiToolStripMenuItem cMenuTrackSelectorPointer { get; set; }
		UiToolStripMenuItem cMenuTrackSelectorPencil { get;set; }
		UiToolStripMenuItem cMenuTrackSelectorLine { get; set; }
		UiToolStripMenuItem cMenuTrackSelectorEraser { get; set; }
		UiToolStripMenuItem cMenuTrackSelectorCurve { get; set; }

		PencilMode mPencilMode { get; set; }
		UiToolStripMenuItem cMenuPianoGrid { get; set; }
		UiToolStripMenuItem cMenuPianoImportLyric { get; set; }
		void updateCopyAndPasteButtonStatus();
		void updateCMenuPianoFixed();
		void applySelectedTool();

		void ensureCursorVisible();
		void showInputTextBox (string phrase, string phonetic_symbol, Point position, bool phonetic_symbol_edit_mode);
		bool mLastSymbolEditMode { get; set; }
		void moveUpDownLeftRight (int upDown, int leftRight);

		UiToolStripMenuItem menuHelpLogSwitch { get; set; }

		UiToolStripMenuItem menuScriptUpdate { get; set; }

		void initializeRendererMenuHandler (FormMainModel model);
		void updateTrackMenuStatus();
		UiToolStripMenuItem menuTrackOn { get; set; }
		UiToolStripMenuItem cMenuTrackTabTrackOn { get; set; }

		UiToolStripMenuItem menuJobConnect { get; set; }
		UiToolStripMenuItem menuJobLyric { get; set; }
		FormImportLyric mDialogImportLyric { get; set; }

		void updateLayout();
		UiToolStripMenuItem menuVisualStartMarker { get; set; }
		UiToolStripMenuItem menuVisualEndMarker { get; set; }
		UiToolBarButton stripBtnEndMarker { get; set; }
		UiToolStripMenuItem menuVisualPluginUiAquesTone { get; set; }
		UiToolStripMenuItem menuVisualPluginUiVocaloid1 { get; set; }
		UiToolStripMenuItem menuVisualPluginUiVocaloid2 { get; set; }
		UiToolStripMenuItem menuTrackRendererAquesTone2 { get; set; }
		UiToolStripMenuItem menuVisualPluginUiAquesTone2 { get; set; }
		UiToolStripMenuItem menuVisualIconPalette { get; set; }
		UiToolStripMenuItem menuVisualWaveform { get; set; }
		UiToolStripMenuItem menuVisualPitchLine { get; set; }
		UiToolStripMenuItem menuVisualNoteProperty { get; set; }
		UiToolStripMenuItem menuVisualLyrics { get; set; }
		UiToolStripMenuItem menuVisualGridline { get; set; }
		UiToolStripMenuItem menuVisualMixer { get; set; }
		UiToolStripMenuItem menuVisualOverview { get; set; }
		UiToolStripMenuItem menuVisualProperty { get; set; }
		void updatePropertyPanelState(PanelState state);
		void updateSplitContainer2Size (bool save_to_config);

		UiToolStripMenuItem cMenuTrackSelectorUndo { get; set; }
		UiToolStripMenuItem cMenuTrackSelectorRedo { get; set; }
		UiToolStripMenuItem cMenuPianoUndo { get; set; }
		UiToolStripMenuItem cMenuPianoRedo { get; set; }
		UiToolStripMenuItem menuEditUndo { get; set; }
		UiToolStripMenuItem menuEditRedo { get; set; }
		UiToolStripMenuItem menuEditAutoNormalizeMode { get; set; }

		UiToolStripMenuItem menuLyricCopyVibratoToPreset { get; set; }

		object searchMenuItemFromName (string name, ByRef<object> parent);
		cadencii.apputil.BSplitContainer splitContainer1 { get; set;}
		UiToolStripMenuItem menuHidden { get; set; }
		UiToolStripMenuItem menuScript { get; set;}
		UiToolStripMenuItem menuVisualControlTrack { get; set; }
		UiVScrollBar vScroll { get; set; }
		int calculateStartToDrawY(int vscroll_value);
		void updateVibratoPresetMenu();
		void updateRendererMenu();
		void reloadMidiIn();
		void updateMidiInStatus();
		Timer timer { get; set; }
		VersionInfo mVersionInfo { get; set; }
		void applyShortcut ();
		void applyLanguage ();
		void updateMenuFonts ();
		Preference mDialogPreference { get; set; }

		FormMidiImExport mDialogMidiImportAndExport { get; set; }

		void updateScrollRangeHorizontal();

		UiToolStripStatusLabel statusLabel { get; set; }
		UiToolStripMenuItem menuFileRecentClear { get; set; }
		UiToolStripMenuItem menuFileRecent { get; set; }

		UiOpenFileDialog openXmlVsqDialog { get; set; }
		UiSaveFileDialog saveXmlVsqDialog { get; set; }
		UiOpenFileDialog openUstDialog { get; set; }
		UiOpenFileDialog openMidiDialog { get; set; }
		UiSaveFileDialog saveMidiDialog { get; set; }
		UiOpenFileDialog openWaveDialog { get; set; }

		WaveView waveView { get; set; }

		bool mEdited { get; set; }

		FormMainController controller { get; set; }

		int calculateStartToDrawX ();

		bool isMouseMiddleButtonDowned (Cadencii.Gui.MouseButtons mouseButtons);

		UiContextMenuStrip MenuTrackTab { get; set; }
		UiContextMenuStrip MenuTrackSelector { get; set; }

		TrackSelector TrackSelector { get; set; }

		UiHScrollBar hScroll { get; set; }

		PictPianoRoll pictPianoRoll { get; set; }

		IList<ValuePairOfStringArrayOfKeys> getDefaultShortcutKeys ();

		void refreshScreen ();
		void refreshScreen (bool b);

		void setEdited (bool b);

		void updateDrawObjectList ();

        /// <summary>
        /// ピアノロールの部品にフォーカスを持たせる
        /// </summary>
        [PureVirtualFunction]
        void focusPianoRoll();

	/// <summary>
        /// 指定したゲートタイムがピアノロール上で可視状態となるよう、横スクロールバーを移動させます。
        /// </summary>
        /// <param name="clock"></param>
	[PureVirtualFunction]
	void ensureVisible(int clock);
    }

}
