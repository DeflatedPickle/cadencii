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
using Cadencii.Media.Vsq;
using System;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Models;
using Cadencii.Application.Controls;
using cadencii;
using Cadencii.Utilities;

namespace Cadencii.Application.Forms
{
    /// <summary>
    /// メイン画面の実装クラスが持つべきメソッドを定義するインターフェース
    /// </summary>
    public interface FormMain : UiForm
    {
		FormMainModel Model { get; }

		UiToolBarButton stripBtnGrid { get; set; }

		BSplitContainer splitContainer2 { get;set; }
		BSplitContainer splitContainerProperty { get; set; }
		System.ComponentModel.BackgroundWorker bgWorkScreen { get; set; }
		UiToolBar toolBarFile { get; set; }
		UiToolBar toolBarPosition { get; set; }
		Rebar rebar { get; set; }

		void refreshScreenCore();
		void showUpdateInformationAsync(bool is_explicit_update_check);
		UiToolStripMenuItem menuFileExportWave { get;set; }
		Size getWindowMinimumSize ();
		RebarBand bandFile { get; set; }
		RebarBand bandPosition { get; set; }
		RebarBand bandMeasure { get; set; }
		RebarBand bandTool { get; set; }
		void updateBgmMenuState();
		UiPictureBox pictureBox2{ get; set; }
		Graphics mGraphicsPictureBox2 { get;set; }

		UiToolStripStatusLabel stripLblMidiIn { get;set ; }

		UiToolBar toolBarMeasure { get; set; }
		UiToolBar toolBarTool { get; set; }
		UiToolStripMenuItem cMenuPianoPaletteTool { get; set; }
		UiToolStripMenuItem cMenuTrackSelectorPaletteTool { get; set; }
		UiToolBarButton stripDDBtnQuantizeParent { get; set; }
		UiContextMenuStrip stripDDBtnQuantize { get;set; }

		void updateScrollRangeVertical ();

		UiPictureBox picturePositionIndicator { get; set; }
		UiContextMenuStrip cMenuPositionIndicator { get; set; }
		float mFps { get; set; }
		float mFps2 { get; set; }

		UiToolBarButton stripBtnPlay { get; set; }

		CurveEditMode mEditCurveMode { get; set; }

		UiContextMenuStrip cMenuPiano { get; set; }
		UiHTrackBar trackBar { get; set; }
		double mTimerDragLastIgnitted { get; set; }
		bool mMouseDowned { get; set; }
		Point mButtonInitial { get; set; }
		int mMiddleButtonVScroll { get; set; }
		int mMiddleButtonHScroll { get; set; }
		Keys s_modifier_key { get; set; }
		void updateContextMenuPiano (Point mouseAt);
		void fixAddingEvent ();
		int computeScrollValueFromWheelDelta(int delta);
		int computeHScrollValueForMiddleDrag(int mouse_x);
		int computeVScrollValueForMiddleDrag (int mouse_y);
		void processSpecialShortcutKey (KeyEventArgs e, bool onPreviewKeyDown);
		VsqEvent getItemAtClickedPosition (Point mouse_position, ByRef<Rectangle> rect);

		bool mFormActivated { get; set; }
		UiToolStripStatusLabel stripLblGameCtrlMode { get; set; }
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

		UiToolStripMenuItem cMenuTrackSelectorUndo { get; set; }
		UiToolStripMenuItem cMenuTrackSelectorRedo { get; set; }
		UiToolStripMenuItem cMenuPianoUndo { get; set; }
		UiToolStripMenuItem cMenuPianoRedo { get; set; }
		UiToolStripMenuItem menuEditUndo { get; set; }
		UiToolStripMenuItem menuEditRedo { get; set; }
		UiToolStripMenuItem menuEditAutoNormalizeMode { get; set; }

		UiToolStripMenuItem menuLyricCopyVibratoToPreset { get; set; }

		BSplitContainer splitContainer1 { get; set;}
		UiToolStripMenuItem menuHidden { get; set; }
		UiToolStripMenuItem menuScript { get; set;}
		UiToolStripMenuItem menuVisualControlTrack { get; set; }
		UiVScrollBar vScroll { get; set; }
		int calculateStartToDrawY(int vscroll_value);
		void updateVibratoPresetMenu();
		void updateRendererMenu();
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

		int calculateStartToDrawX ();

		UiContextMenuStrip MenuTrackTab { get; set; }
		UiContextMenuStrip MenuTrackSelector { get; set; }

		TrackSelector TrackSelector { get; }

		UiHScrollBar hScroll { get; set; }

		PictPianoRoll pictPianoRoll { get; set; }

		IList<ValuePairOfStringArrayOfKeys> getDefaultShortcutKeys ();

		void refreshScreen ();
		void refreshScreen (bool b);

		void setEdited (bool b);

		void updateDrawObjectList ();
    }

}
