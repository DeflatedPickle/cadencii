using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;
using cadencii;

namespace Cadencii.Application.Forms
{
    public partial class FormMainImpl
    {
        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMainImpl));

            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "FormMain.xml");

			stripBtnStepSequencer.Image = Resources.piano;
			imageListFile.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListFile.ImageStream")));
			imageListPosition.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListPosition.ImageStream")));
			imageListMeasure.ImageStream = ((System.Windows.Forms.ImageListStreamer) (resources.GetObject ("imageListMeasure.ImageStream")));
			imageListTool.ImageStream = ((System.Windows.Forms.ImageListStreamer) (resources.GetObject ("imageListTool.ImageStream")));
			imageListFile.SetImagesKeyName (0, "disk__plus.png");
			imageListFile.SetImagesKeyName (1, "folder_horizontal_open.png");
			imageListFile.SetImagesKeyName (2, "disk.png");
			imageListFile.SetImagesKeyName (3, "scissors.png");
			imageListFile.SetImagesKeyName (4, "documents.png");
			imageListFile.SetImagesKeyName (5, "clipboard_paste.png");
			imageListFile.SetImagesKeyName (6, "arrow_skip_180.png");
			imageListFile.SetImagesKeyName (7, "arrow_skip.png");
			imageListPosition.SetImagesKeyName (0, "control_stop_180.png");
			imageListPosition.SetImagesKeyName (1, "control_double_180.png");
			imageListPosition.SetImagesKeyName (2, "control_double.png");
			imageListPosition.SetImagesKeyName (3, "control_stop.png");
			imageListPosition.SetImagesKeyName (4, "control.png");
			imageListPosition.SetImagesKeyName (5, "control_pause.png");
			imageListPosition.SetImagesKeyName (6, "arrow_circle_double.png");
			imageListPosition.SetImagesKeyName (7, "arrow_return.png");
			imageListMeasure.SetImagesKeyName (0, "pin__arrow.png");
			imageListMeasure.SetImagesKeyName (1, "pin__arrow_inv.png");
			imageListMeasure.SetImagesKeyName (2, "note001.png");
			imageListMeasure.SetImagesKeyName (3, "note002.png");
			imageListMeasure.SetImagesKeyName (4, "note004.png");
			imageListMeasure.SetImagesKeyName (5, "note008.png");
			imageListMeasure.SetImagesKeyName (6, "note016.png");
			imageListMeasure.SetImagesKeyName (7, "note032.png");
			imageListMeasure.SetImagesKeyName (8, "note064.png");
			imageListMeasure.SetImagesKeyName (9, "note128.png");
			imageListMeasure.SetImagesKeyName (10, "notenull.png");
			imageListTool.SetImagesKeyName (0, "arrow_135.png");
			imageListTool.SetImagesKeyName (1, "pencil.png");
			imageListTool.SetImagesKeyName (2, "layer_shape_line.png");
			imageListTool.SetImagesKeyName (3, "eraser.png");
			imageListTool.SetImagesKeyName (4, "ruler_crop.png");
			imageListTool.SetImagesKeyName (5, "layer_shape_curve.png");

            this.ResumeLayout(false);
            this.PerformLayout();

        }

        System.ComponentModel.IContainer components;

		#pragma warning disable 0169,0649
		UiPanel PanelMain { get; set; }
        UiMenuStrip menuStripMain;
        UiToolStripMenuItem menuFile;
        UiToolStripMenuItem menuEdit;
        UiToolStripMenuItem menuVisual;
        UiToolStripMenuItem menuJob;
        UiToolStripMenuItem menuTrack;
        UiToolStripMenuItem menuLyric;
        UiToolStripMenuItem menuSetting;
        UiToolStripMenuItem menuHelp;
		public UiToolStripMenuItem menuVisualControlTrack { get; set; }
		public UiToolStripMenuItem menuVisualMixer { get; set; }
        UiToolStripSeparator toolStripMenuItem1;
		public UiToolStripMenuItem menuVisualGridline { get; set; }
        UiToolStripSeparator toolStripMenuItem2;
		public UiToolStripMenuItem menuVisualStartMarker { get; set; }
		public UiToolStripMenuItem menuVisualEndMarker { get; set; }
        UiToolStripSeparator toolStripMenuItem3;
		public UiToolStripMenuItem menuVisualLyrics { get; set; }
		public UiToolStripMenuItem menuVisualNoteProperty { get; set; }
        UiToolStripMenuItem menuSettingPreference;
        UiToolStripSeparator toolStripMenuItem6;
        UiToolStripMenuItem menuSettingDefaultSingerStyle;
        UiToolStripMenuItem menuSettingPositionQuantize;
        UiToolStripMenuItem menuSettingPositionQuantize04;
        UiToolStripMenuItem menuSettingPositionQuantize08;
        UiToolStripMenuItem menuSettingPositionQuantize16;
        UiToolStripMenuItem menuSettingPositionQuantize32;
        UiToolStripMenuItem menuSettingPositionQuantize64;
        UiToolStripMenuItem menuSettingPositionQuantizeOff;
        UiToolStripSeparator toolStripMenuItem9;
        UiToolStripSeparator toolStripMenuItem8;
        UiToolStripMenuItem menuSettingPositionQuantizeTriplet;
        UiToolStripMenuItem menuFileNew;
        UiToolStripMenuItem menuFileOpen;
        UiToolStripMenuItem menuFileSave;
        UiToolStripMenuItem menuFileSaveNamed;
        UiToolStripSeparator toolStripMenuItem10;
        UiToolStripMenuItem menuFileImport;
        UiToolStripMenuItem menuFileExport;
        UiToolStripSeparator toolStripMenuItem11;
        UiToolStripSeparator toolStripMenuItem12;
        UiToolStripMenuItem menuFileQuit;
		public UiToolStripMenuItem menuEditUndo { get; set; }
		public UiToolStripMenuItem menuEditRedo { get; set; }
        UiToolStripSeparator toolStripMenuItem5;
		public UiPictureBox pictureBox2{ get; set; }
        UiPictureBox pictureBox3;
		public UiPictureBox picturePositionIndicator { get; set; }
		public UiContextMenuStrip cMenuPiano { get; set; }
        UiToolStripMenuItem cMenuPianoPointer;
        UiToolStripMenuItem cMenuPianoPencil;
        UiToolStripMenuItem cMenuPianoEraser;
        UiToolStripSeparator toolStripMenuItem13;
        UiToolStripMenuItem cMenuPianoFixed;
        UiToolStripMenuItem cMenuPianoQuantize;
		public UiToolStripMenuItem cMenuPianoGrid { get; set; }
        UiToolStripSeparator toolStripMenuItem14;
		public UiToolStripMenuItem cMenuPianoUndo { get; set; }
		public UiToolStripMenuItem cMenuPianoRedo { get; set; }
        UiToolStripSeparator toolStripMenuItem15;
        UiToolStripMenuItem cMenuPianoCut;
        UiToolStripMenuItem cMenuPianoFixed01;
        UiToolStripMenuItem cMenuPianoFixed02;
        UiToolStripMenuItem cMenuPianoFixed04;
        UiToolStripMenuItem cMenuPianoFixed08;
        UiToolStripMenuItem cMenuPianoFixed16;
        UiToolStripMenuItem cMenuPianoFixed32;
        UiToolStripMenuItem cMenuPianoFixed64;
        UiToolStripMenuItem cMenuPianoFixedOff;
        UiToolStripSeparator toolStripMenuItem18;
        UiToolStripMenuItem cMenuPianoFixedTriplet;
        UiToolStripMenuItem cMenuPianoFixedDotted;
        UiToolStripMenuItem cMenuPianoCopy;
        UiToolStripMenuItem cMenuPianoPaste;
        UiToolStripMenuItem cMenuPianoDelete;
        UiToolStripSeparator toolStripMenuItem16;
        UiToolStripMenuItem cMenuPianoSelectAll;
        UiToolStripMenuItem cMenuPianoSelectAllEvents;
        UiToolStripSeparator toolStripMenuItem17;
		public UiToolStripMenuItem cMenuPianoImportLyric { get; set; }
        UiToolStripMenuItem cMenuPianoExpressionProperty;
        UiToolStripMenuItem cMenuPianoQuantize04;
        UiToolStripMenuItem cMenuPianoQuantize08;
        UiToolStripMenuItem cMenuPianoQuantize16;
        UiToolStripMenuItem cMenuPianoQuantize32;
        UiToolStripMenuItem cMenuPianoQuantize64;
        UiToolStripMenuItem cMenuPianoQuantizeOff;
        UiToolStripSeparator toolStripMenuItem26;
        UiToolStripMenuItem cMenuPianoQuantizeTriplet;
		public UiToolStripMenuItem menuFileRecent { get; set; }
        //UiToolTip toolTip;
        UiToolStripMenuItem menuEditCut;
        UiToolStripMenuItem menuEditCopy;
        UiToolStripMenuItem menuEditPaste;
        UiToolStripMenuItem menuEditDelete;
        UiToolStripSeparator toolStripMenuItem19;
		public UiToolStripMenuItem menuEditAutoNormalizeMode { get; set; }
        UiToolStripSeparator toolStripMenuItem20;
        UiToolStripMenuItem menuEditSelectAll;
        UiToolStripMenuItem menuEditSelectAllEvents;
		public UiToolStripMenuItem menuTrackOn { get; set; }
        UiToolStripSeparator toolStripMenuItem21;
        UiToolStripMenuItem menuTrackAdd;
        UiToolStripMenuItem menuTrackCopy;
        UiToolStripMenuItem menuTrackChangeName;
        UiToolStripMenuItem menuTrackDelete;
        UiToolStripSeparator toolStripMenuItem22;
        UiToolStripMenuItem menuTrackRenderCurrent;
        UiToolStripMenuItem menuTrackRenderAll;
        UiToolStripSeparator toolStripMenuItem23;
        UiToolStripMenuItem menuTrackOverlay;
	UiContextMenuStrip cMenuTrackTab;
		public UiToolStripMenuItem cMenuTrackTabTrackOn { get; set; }
        UiToolStripSeparator toolStripMenuItem24;
        UiToolStripMenuItem cMenuTrackTabAdd;
        UiToolStripMenuItem cMenuTrackTabCopy;
        UiToolStripMenuItem cMenuTrackTabChangeName;
        UiToolStripMenuItem cMenuTrackTabDelete;
        UiToolStripSeparator toolStripMenuItem25;
        UiToolStripMenuItem cMenuTrackTabRenderCurrent;
        UiToolStripMenuItem cMenuTrackTabRenderAll;
        UiToolStripSeparator toolStripMenuItem27;
        UiToolStripMenuItem cMenuTrackTabOverlay;
        UiContextMenuStrip cMenuTrackSelector;
		public UiToolStripMenuItem cMenuTrackSelectorPointer { get; set; }
		public UiToolStripMenuItem cMenuTrackSelectorPencil { get;set; }
		public UiToolStripMenuItem cMenuTrackSelectorLine { get; set; }
		public UiToolStripMenuItem cMenuTrackSelectorEraser { get; set; }
        UiToolStripSeparator toolStripMenuItem28;
		public UiToolStripMenuItem cMenuTrackSelectorUndo { get; set; }
		public UiToolStripMenuItem cMenuTrackSelectorRedo { get; set; }
        UiToolStripSeparator toolStripMenuItem29;
        UiToolStripMenuItem cMenuTrackSelectorCut;
        UiToolStripMenuItem cMenuTrackSelectorCopy;
        UiToolStripMenuItem cMenuTrackSelectorPaste;
        UiToolStripMenuItem cMenuTrackSelectorDelete;
        UiToolStripSeparator toolStripMenuItem31;
        UiToolStripMenuItem cMenuTrackSelectorSelectAll;
        UiToolStripMenuItem menuJobNormalize;
        UiToolStripMenuItem menuJobInsertBar;
        UiToolStripMenuItem menuJobDeleteBar;
        UiToolStripMenuItem menuJobRandomize;
		public UiToolStripMenuItem menuJobConnect { get; set; }
		public UiToolStripMenuItem menuJobLyric { get; set; }
        UiToolStripMenuItem menuJobRewire;
        UiToolStripMenuItem menuLyricExpressionProperty;
        UiToolStripMenuItem menuLyricPhonemeTransformation;
        UiToolStripMenuItem menuLyricDictionary;
        UiToolStripMenuItem menuHelpAbout;
        UiToolStripMenuItem menuHelpDebug;
		public UiToolStripMenuItem menuFileExportWave { get;set; }
        UiToolStripMenuItem menuFileExportMidi;
		public UiToolStripMenuItem menuScript { get; set;}
		public UiToolStripMenuItem menuHidden { get; set; }
        UiToolStripMenuItem menuHiddenEditLyric;
        UiToolStripMenuItem menuHiddenEditFlipToolPointerPencil;
        UiToolStripMenuItem menuHiddenEditFlipToolPointerEraser;
        UiToolStripMenuItem menuHiddenVisualForwardParameter;
        UiToolStripMenuItem menuHiddenVisualBackwardParameter;
        UiToolStripMenuItem menuHiddenTrackNext;
        UiToolStripMenuItem menuHiddenTrackBack;
        UiToolStripMenuItem menuJobReloadVsti;
        UiToolStripMenuItem cMenuPianoCurve;
		public UiToolStripMenuItem cMenuTrackSelectorCurve { get; set; }
		public UiTrackBar trackBar { get; set; }
        UiToolBarButton stripBtnPointer;
        UiToolBarButton stripBtnLine;
        UiToolBarButton stripBtnPencil;
        UiToolBarButton stripBtnEraser;
		public UiToolBarButton stripBtnGrid { get; set; }
        UiToolBarButton stripBtnMoveTop;
        UiToolBarButton stripBtnRewind;
        UiToolBarButton stripBtnForward;
        UiToolBarButton stripBtnMoveEnd;
		public UiToolBarButton stripBtnPlay { get; set; }
        UiToolBarButton stripBtnScroll;
        UiToolBarButton stripBtnLoop;
        UiToolBarButton stripBtnCurve;
        UiToolStripSeparator toolStripSeparator2;
		public UiContextMenu stripDDBtnQuantize { get;set; }
        UiMenuItem stripDDBtnQuantize04;
        UiMenuItem stripDDBtnQuantize08;
        UiMenuItem stripDDBtnQuantize16;
        UiMenuItem stripDDBtnQuantize32;
        UiMenuItem stripDDBtnQuantize64;
        UiMenuItem stripDDBtnQuantizeOff;
        UiToolStripSeparator toolStripSeparator3;
        UiMenuItem stripDDBtnQuantizeTriplet;
        UiToolBarButton stripBtnStartMarker;
		public UiToolBarButton stripBtnEndMarker { get; set; }
        	public UiHScrollBar hScroll { get; set; }
		public UiVScrollBar vScroll { get; set; }
        UiToolStripMenuItem menuLyricVibratoProperty;
        UiToolStripMenuItem cMenuPianoVibratoProperty;
		public UiToolStripMenuItem menuScriptUpdate { get; set; }
        UiToolStripMenuItem menuSettingGameControler;
		public UiToolStripStatusLabel stripLblGameCtrlMode { get; set; }
        UiToolStripSeparator toolStripSeparator10;
        UiToolStripMenuItem menuSettingGameControlerSetting;
        UiToolStripMenuItem menuSettingGameControlerLoad;
        UiMenuItem stripDDBtnQuantize128;
        UiToolStripMenuItem menuSettingPositionQuantize128;
        UiToolStripMenuItem cMenuPianoQuantize128;
        UiToolStripMenuItem cMenuPianoFixed128;
		public UiToolStripMenuItem menuVisualWaveform { get; set; }
        WaveformZoom panelWaveformZoom;
        UiToolStripMenuItem cMenuTrackSelectorDeleteBezier;
		public UiToolStripStatusLabel stripLblMidiIn { get;set ; }
        UiToolStripSeparator toolStripSeparator11;
        //UiToolStripMenuItem menuJobRealTime;
        UiToolStripMenuItem cMenuTrackTabRenderer;
        UiToolStripMenuItem cMenuTrackTabRendererVOCALOID1;
        UiToolStripMenuItem cMenuTrackTabRendererVOCALOID2;
        UiToolStripMenuItem cMenuTrackTabRendererUtau;
        UiToolStripMenuItem cMenuTrackTabRendererAquesTone2;
		public UiToolStripMenuItem menuVisualPitchLine { get;set; }
        UiToolStripMenuItem menuFileImportMidi;
        UiToolStripStatusLabel toolStripStatusLabel1;
        UiToolStripStatusLabel toolStripStatusLabel2;
        UiToolBarButton stripBtnFileSave;
        UiToolBarButton stripBtnFileOpen;
        UiToolBarButton stripBtnCut;
        UiToolBarButton stripBtnCopy;
        UiToolBarButton stripBtnPaste;
        UiToolBarButton stripBtnFileNew;
        UiToolBarButton stripBtnUndo;
        UiToolBarButton stripBtnRedo;
		public UiToolStripMenuItem cMenuTrackSelectorPaletteTool { get; set; }
		public UiToolStripMenuItem cMenuPianoPaletteTool { get; set; }
        UiToolStripSeparator toolStripSeparator14;
        UiToolStripSeparator toolStripSeparator15;
        UiToolStripMenuItem menuSettingPaletteTool;
        UiToolStripMenuItem menuTrackRenderer;
        UiToolStripMenuItem menuTrackRendererVOCALOID1;
        UiToolStripMenuItem menuTrackRendererVOCALOID2;
        UiToolStripMenuItem menuTrackRendererUtau;
        UiToolStripMenuItem menuFileImportVsq;
        UiToolStripMenuItem menuSettingShortcut;
		public UiToolStripMenuItem menuVisualProperty { get; set; }
        UiToolStripMenuItem menuFileOpenVsq;
        UiToolStripMenuItem menuFileOpenUst;
        UiToolStripMenuItem menuSettingGameControlerRemove;
        UiToolStripMenuItem menuHiddenCopy;
        UiToolStripMenuItem menuHiddenPaste;
        UiToolStripMenuItem menuHiddenCut;
        UiToolStrip toolStripBottom;
		public BSplitContainer splitContainerProperty { get; set; }
		public UiToolStripMenuItem menuVisualOverview { get; set; }
        PictOverview panelOverview;
		public BSplitContainer splitContainer1 { get; set; }
        UiToolStripSeparator toolStripMenuItem4;
        UiToolStripMenuItem menuTrackBgm;
        UiToolStripMenuItem menuTrackRendererVCNT;
        //UiToolStripMenuItem menuTrackManager;
        UiToolStripMenuItem cMenuTrackTabRendererStraight;
        	public PictPianoRoll pictPianoRoll { get; set; }
        TrackSelector trackSelector;
        UiPictureBox pictKeyLengthSplitter;
        UiToolStripMenuItem menuTrackRendererAquesTone;
        UiToolStripMenuItem cMenuTrackTabRendererAquesTone;
	UiToolStripMenuItem menuTrackRendererAquesTone2;
        UiToolStripMenuItem menuVisualPluginUi;
		public UiToolStripMenuItem menuVisualPluginUiAquesTone { get; set; }
		public UiToolStripMenuItem menuVisualPluginUiVocaloid1 { get; set; }
		public UiToolStripMenuItem menuVisualPluginUiVocaloid2 { get; set; }
		public UiToolStripMenuItem menuVisualIconPalette { get; set; }
        UiToolStripMenuItem menuFileExportMusicXml;
        UiToolStripMenuItem menuHiddenSelectForward;
        UiToolStripMenuItem menuHiddenSelectBackward;
        UiToolStripMenuItem menuHiddenMoveUp;
        UiToolStripMenuItem menuHiddenMoveDown;
        UiToolStripMenuItem menuHiddenMoveLeft;
        UiToolStripMenuItem menuHiddenMoveRight;
        UiToolStripMenuItem menuHiddenLengthen;
        UiToolStripMenuItem menuHiddenShorten;
        UiToolStripMenuItem menuHiddenGoToStartMarker;
        UiToolStripMenuItem menuHiddenGoToEndMarker;
        UiToolStripMenuItem menuHiddenPlayFromStartMarker;
        UiToolStripMenuItem menuHiddenFlipCurveOnPianorollMode;
        //CircuitView pictCircuit;
        UiToolStripMenuItem menuFileExportUst;
        UiToolStripMenuItem menuHelpLog;
		public UiToolStripMenuItem menuHelpLogSwitch { get; set; }
        UiToolStripMenuItem menuHelpLogOpen;
		public Rebar rebar { get; set; }
		public RebarBand bandFile { get; set; }
		public RebarBand bandPosition { get; set; }
		public RebarBand bandMeasure { get; set; }
		public RebarBand bandTool { get; set; }
		public BSplitContainer splitContainer2 { get;set; }
        UiPanel panel1;
		public UiToolBar toolBarFile { get; set; }
        UiImageList imageListFile;
        UiToolBarButton toolBarButton1;
        UiToolBarButton toolBarButton2;
		public UiToolBar toolBarTool { get; set; }
        UiToolBarButton toolBarButton3;
        UiImageList imageListTool;
		public UiToolBar toolBarPosition { get; set; }
        UiToolBarButton toolBarButton4;
        UiImageList imageListPosition;
		public UiToolBar toolBarMeasure { get; set; }
		public UiToolBarButton stripDDBtnQuantizeParent { get; set; }
        UiMenuItem menuItem2;
        UiToolBarButton toolBarButton5;
        UiImageList imageListMeasure;
        UiToolStripContainer toolStripContainer1;
        UiStatusStrip statusStrip;
		public UiToolStripStatusLabel statusLabel { get; set; }
        UiImageList imageListMenu;
        UiToolStripMenuItem menuFileExportVsq;
        UiToolStripMenuItem menuFileExportVxt;
		public UiToolStripMenuItem menuLyricCopyVibratoToPreset { get; set; }
        UiToolStripMenuItem menuSettingVibratoPreset;
        UiToolStripMenuItem menuSettingSequence;
        UiToolStripMenuItem menuHiddenPrintPoToCSV;
        UiToolStripMenuItem menuFileExportParaWave;
        UiToolStripMenuItem menuFileImportUst;
        UiToolStripButton stripBtnStepSequencer;
		public UiContextMenuStrip cMenuPositionIndicator { get; set; }
        UiToolStripMenuItem cMenuPositionIndicatorStartMarker;
        UiToolStripMenuItem cMenuPositionIndicatorEndMarker;
        UiToolStripMenuItem menuHelpManual;
		public WaveView waveView { get; set; }
		public UiToolStripMenuItem menuFileRecentClear { get; set; }
        UiToolStripMenuItem menuLyricApplyUtauParameters;
		public UiToolStripMenuItem menuVisualPluginUiAquesTone2 { get; set; }
        UiToolStripMenuItem menuFileExportVsqx;
        UiToolStripMenuItem menuTools;
        UiToolStripMenuItem menuToolsCreateVConnectSTANDDb;
        UiToolStripMenuItem menuHelpCheckForUpdates;
		#pragma warning restore 0169,0649
		   }
}