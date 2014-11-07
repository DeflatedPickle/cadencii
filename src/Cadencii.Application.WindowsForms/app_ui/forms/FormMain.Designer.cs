using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using cadencii.windows.forms;

using ToolStripRenderMode = Cadencii.Gui.ToolStripRenderMode;
using ToolStripItemDisplayStyle = Cadencii.Gui.ToolStripItemDisplayStyle;
using ToolStripItemImageScaling = Cadencii.Gui.ToolStripItemImageScaling;
using TextImageRelation = Cadencii.Gui.TextImageRelation;
using CheckState = Cadencii.Gui.CheckState;
using TickStyle = Cadencii.Gui.TickStyle;
using Keys = Cadencii.Gui.Keys;
using AnchorStyles = Cadencii.Gui.AnchorStyles;
using Color = Cadencii.Gui.Color;
using BorderStyle = Cadencii.Gui.BorderStyle;
using DockStyle = Cadencii.Gui.DockStyle;
using ToolBarButtonStyle = Cadencii.Gui.ToolBarButtonStyle;
using ToolBarAppearance = Cadencii.Gui.ToolBarAppearance;
using ToolBarTextAlign = Cadencii.Gui.ToolBarTextAlign;

namespace cadencii
{
    public partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStripMain = new MenuStripImpl();
            this.menuFile = new ToolStripMenuItemImpl();
            this.menuFileNew = new ToolStripMenuItemImpl();
            this.menuFileOpen = new ToolStripMenuItemImpl();
            this.menuFileSave = new ToolStripMenuItemImpl();
            this.menuFileSaveNamed = new ToolStripMenuItemImpl();
            this.toolStripMenuItem10 = new ToolStripSeparatorImpl();
            this.menuFileOpenVsq = new ToolStripMenuItemImpl();
            this.menuFileOpenUst = new ToolStripMenuItemImpl();
            this.menuFileImport = new ToolStripMenuItemImpl();
            this.menuFileImportVsq = new ToolStripMenuItemImpl();
            this.menuFileImportMidi = new ToolStripMenuItemImpl();
            this.menuFileImportUst = new ToolStripMenuItemImpl();
            this.menuFileExport = new ToolStripMenuItemImpl();
            this.menuFileExportWave = new ToolStripMenuItemImpl();
            this.menuFileExportParaWave = new ToolStripMenuItemImpl();
            this.menuFileExportVsq = new ToolStripMenuItemImpl();
            this.menuFileExportVsqx = new ToolStripMenuItemImpl();
            this.menuFileExportMidi = new ToolStripMenuItemImpl();
            this.menuFileExportMusicXml = new ToolStripMenuItemImpl();
            this.menuFileExportUst = new ToolStripMenuItemImpl();
            this.menuFileExportVxt = new ToolStripMenuItemImpl();
            this.toolStripMenuItem11 = new ToolStripSeparatorImpl();
            this.menuFileRecent = new ToolStripMenuItemImpl();
            this.menuFileRecentClear = new ToolStripMenuItemImpl();
            this.toolStripMenuItem12 = new ToolStripSeparatorImpl();
            this.menuFileQuit = new ToolStripMenuItemImpl();
            this.menuEdit = new ToolStripMenuItemImpl();
            this.menuEditUndo = new ToolStripMenuItemImpl();
            this.menuEditRedo = new ToolStripMenuItemImpl();
            this.toolStripMenuItem5 = new ToolStripSeparatorImpl();
            this.menuEditCut = new ToolStripMenuItemImpl();
            this.menuEditCopy = new ToolStripMenuItemImpl();
            this.menuEditPaste = new ToolStripMenuItemImpl();
            this.menuEditDelete = new ToolStripMenuItemImpl();
            this.toolStripMenuItem19 = new ToolStripSeparatorImpl();
            this.menuEditAutoNormalizeMode = new ToolStripMenuItemImpl();
            this.toolStripMenuItem20 = new ToolStripSeparatorImpl();
            this.menuEditSelectAll = new ToolStripMenuItemImpl();
            this.menuEditSelectAllEvents = new ToolStripMenuItemImpl();
            this.menuVisual = new ToolStripMenuItemImpl();
            this.menuVisualControlTrack = new ToolStripMenuItemImpl();
            this.menuVisualMixer = new ToolStripMenuItemImpl();
            this.menuVisualWaveform = new ToolStripMenuItemImpl();
            this.menuVisualIconPalette = new ToolStripMenuItemImpl();
            this.menuVisualProperty = new ToolStripMenuItemImpl();
            this.menuVisualOverview = new ToolStripMenuItemImpl();
            this.menuVisualPluginUi = new ToolStripMenuItemImpl();
            this.menuVisualPluginUiVocaloid1 = new ToolStripMenuItemImpl();
            this.menuVisualPluginUiVocaloid2 = new ToolStripMenuItemImpl();
            this.menuVisualPluginUiAquesTone = new ToolStripMenuItemImpl();
            this.menuVisualPluginUiAquesTone2 = new ToolStripMenuItemImpl();
            this.toolStripMenuItem1 = new ToolStripSeparatorImpl();
            this.menuVisualGridline = new ToolStripMenuItemImpl();
            this.toolStripMenuItem2 = new ToolStripSeparatorImpl();
            this.menuVisualStartMarker = new ToolStripMenuItemImpl();
            this.menuVisualEndMarker = new ToolStripMenuItemImpl();
            this.toolStripMenuItem3 = new ToolStripSeparatorImpl();
            this.menuVisualLyrics = new ToolStripMenuItemImpl();
            this.menuVisualNoteProperty = new ToolStripMenuItemImpl();
            this.menuVisualPitchLine = new ToolStripMenuItemImpl();
            this.menuJob = new ToolStripMenuItemImpl();
            this.menuJobNormalize = new ToolStripMenuItemImpl();
            this.menuJobInsertBar = new ToolStripMenuItemImpl();
            this.menuJobDeleteBar = new ToolStripMenuItemImpl();
            this.menuJobRandomize = new ToolStripMenuItemImpl();
            this.menuJobConnect = new ToolStripMenuItemImpl();
            this.menuJobLyric = new ToolStripMenuItemImpl();
            this.menuJobRewire = new ToolStripMenuItemImpl();
            this.menuJobReloadVsti = new ToolStripMenuItemImpl();
            this.menuTrack = new ToolStripMenuItemImpl();
            this.menuTrackOn = new ToolStripMenuItemImpl();
            this.toolStripMenuItem21 = new ToolStripSeparatorImpl();
            this.menuTrackAdd = new ToolStripMenuItemImpl();
            this.menuTrackCopy = new ToolStripMenuItemImpl();
            this.menuTrackChangeName = new ToolStripMenuItemImpl();
            this.menuTrackDelete = new ToolStripMenuItemImpl();
            this.toolStripMenuItem22 = new ToolStripSeparatorImpl();
            this.menuTrackRenderCurrent = new ToolStripMenuItemImpl();
            this.menuTrackRenderAll = new ToolStripMenuItemImpl();
            this.toolStripMenuItem23 = new ToolStripSeparatorImpl();
            this.menuTrackOverlay = new ToolStripMenuItemImpl();
            this.menuTrackRenderer = new ToolStripMenuItemImpl();
            this.menuTrackRendererVOCALOID1 = new ToolStripMenuItemImpl();
            this.menuTrackRendererVOCALOID2 = new ToolStripMenuItemImpl();
            this.menuTrackRendererUtau = new ToolStripMenuItemImpl();
            this.menuTrackRendererVCNT = new ToolStripMenuItemImpl();
            this.menuTrackRendererAquesTone = new ToolStripMenuItemImpl();
            this.menuTrackRendererAquesTone2 = new ToolStripMenuItemImpl();
            this.toolStripMenuItem4 = new ToolStripSeparatorImpl();
            this.menuTrackBgm = new ToolStripMenuItemImpl();
            this.menuLyric = new ToolStripMenuItemImpl();
            this.menuLyricExpressionProperty = new ToolStripMenuItemImpl();
            this.menuLyricVibratoProperty = new ToolStripMenuItemImpl();
            this.menuLyricApplyUtauParameters = new ToolStripMenuItemImpl();
            this.menuLyricPhonemeTransformation = new ToolStripMenuItemImpl();
            this.menuLyricDictionary = new ToolStripMenuItemImpl();
            this.menuLyricCopyVibratoToPreset = new ToolStripMenuItemImpl();
            this.menuScript = new ToolStripMenuItemImpl();
            this.menuScriptUpdate = new ToolStripMenuItemImpl();
            this.menuSetting = new ToolStripMenuItemImpl();
            this.menuSettingPreference = new ToolStripMenuItemImpl();
            this.menuSettingSequence = new ToolStripMenuItemImpl();
            this.menuSettingPositionQuantize = new ToolStripMenuItemImpl();
            this.menuSettingPositionQuantize04 = new ToolStripMenuItemImpl();
            this.menuSettingPositionQuantize08 = new ToolStripMenuItemImpl();
            this.menuSettingPositionQuantize16 = new ToolStripMenuItemImpl();
            this.menuSettingPositionQuantize32 = new ToolStripMenuItemImpl();
            this.menuSettingPositionQuantize64 = new ToolStripMenuItemImpl();
            this.menuSettingPositionQuantize128 = new ToolStripMenuItemImpl();
            this.menuSettingPositionQuantizeOff = new ToolStripMenuItemImpl();
            this.toolStripMenuItem9 = new ToolStripSeparatorImpl();
            this.menuSettingPositionQuantizeTriplet = new ToolStripMenuItemImpl();
            this.toolStripMenuItem8 = new ToolStripSeparatorImpl();
            this.menuSettingGameControler = new ToolStripMenuItemImpl();
            this.menuSettingGameControlerSetting = new ToolStripMenuItemImpl();
            this.menuSettingGameControlerLoad = new ToolStripMenuItemImpl();
            this.menuSettingGameControlerRemove = new ToolStripMenuItemImpl();
            this.menuSettingPaletteTool = new ToolStripMenuItemImpl();
            this.menuSettingShortcut = new ToolStripMenuItemImpl();
            this.menuSettingVibratoPreset = new ToolStripMenuItemImpl();
            this.toolStripMenuItem6 = new ToolStripSeparatorImpl();
            this.menuSettingDefaultSingerStyle = new ToolStripMenuItemImpl();
            this.menuTools = new ToolStripMenuItemImpl();
            this.menuToolsCreateVConnectSTANDDb = new ToolStripMenuItemImpl();
            this.menuHelp = new ToolStripMenuItemImpl();
            this.menuHelpAbout = new ToolStripMenuItemImpl();
            this.menuHelpManual = new ToolStripMenuItemImpl();
            this.menuHelpLog = new ToolStripMenuItemImpl();
            this.menuHelpLogSwitch = new ToolStripMenuItemImpl();
            this.menuHelpLogOpen = new ToolStripMenuItemImpl();
            this.menuHelpDebug = new ToolStripMenuItemImpl();
            this.menuHidden = new ToolStripMenuItemImpl();
            this.menuHiddenEditLyric = new ToolStripMenuItemImpl();
            this.menuHiddenEditFlipToolPointerPencil = new ToolStripMenuItemImpl();
            this.menuHiddenEditFlipToolPointerEraser = new ToolStripMenuItemImpl();
            this.menuHiddenVisualForwardParameter = new ToolStripMenuItemImpl();
            this.menuHiddenVisualBackwardParameter = new ToolStripMenuItemImpl();
            this.menuHiddenTrackNext = new ToolStripMenuItemImpl();
            this.menuHiddenTrackBack = new ToolStripMenuItemImpl();
            this.menuHiddenCopy = new ToolStripMenuItemImpl();
            this.menuHiddenPaste = new ToolStripMenuItemImpl();
            this.menuHiddenCut = new ToolStripMenuItemImpl();
            this.menuHiddenSelectForward = new ToolStripMenuItemImpl();
            this.menuHiddenSelectBackward = new ToolStripMenuItemImpl();
            this.menuHiddenMoveUp = new ToolStripMenuItemImpl();
            this.menuHiddenMoveDown = new ToolStripMenuItemImpl();
            this.menuHiddenMoveLeft = new ToolStripMenuItemImpl();
            this.menuHiddenMoveRight = new ToolStripMenuItemImpl();
            this.menuHiddenLengthen = new ToolStripMenuItemImpl();
            this.menuHiddenShorten = new ToolStripMenuItemImpl();
            this.menuHiddenGoToStartMarker = new ToolStripMenuItemImpl();
            this.menuHiddenGoToEndMarker = new ToolStripMenuItemImpl();
            this.menuHiddenPlayFromStartMarker = new ToolStripMenuItemImpl();
            this.menuHiddenFlipCurveOnPianorollMode = new ToolStripMenuItemImpl();
            this.menuHiddenPrintPoToCSV = new ToolStripMenuItemImpl();
            this.cMenuPiano = new ContextMenuStripImpl(this.components);
            this.cMenuPianoPointer = new ToolStripMenuItemImpl();
            this.cMenuPianoPencil = new ToolStripMenuItemImpl();
            this.cMenuPianoEraser = new ToolStripMenuItemImpl();
            this.cMenuPianoPaletteTool = new ToolStripMenuItemImpl();
            this.toolStripSeparator15 = new ToolStripSeparatorImpl();
            this.cMenuPianoCurve = new ToolStripMenuItemImpl();
            this.toolStripMenuItem13 = new ToolStripSeparatorImpl();
            this.cMenuPianoFixed = new ToolStripMenuItemImpl();
            this.cMenuPianoFixed01 = new ToolStripMenuItemImpl();
            this.cMenuPianoFixed02 = new ToolStripMenuItemImpl();
            this.cMenuPianoFixed04 = new ToolStripMenuItemImpl();
            this.cMenuPianoFixed08 = new ToolStripMenuItemImpl();
            this.cMenuPianoFixed16 = new ToolStripMenuItemImpl();
            this.cMenuPianoFixed32 = new ToolStripMenuItemImpl();
            this.cMenuPianoFixed64 = new ToolStripMenuItemImpl();
            this.cMenuPianoFixed128 = new ToolStripMenuItemImpl();
            this.cMenuPianoFixedOff = new ToolStripMenuItemImpl();
            this.toolStripMenuItem18 = new ToolStripSeparatorImpl();
            this.cMenuPianoFixedTriplet = new ToolStripMenuItemImpl();
            this.cMenuPianoFixedDotted = new ToolStripMenuItemImpl();
            this.cMenuPianoQuantize = new ToolStripMenuItemImpl();
            this.cMenuPianoQuantize04 = new ToolStripMenuItemImpl();
            this.cMenuPianoQuantize08 = new ToolStripMenuItemImpl();
            this.cMenuPianoQuantize16 = new ToolStripMenuItemImpl();
            this.cMenuPianoQuantize32 = new ToolStripMenuItemImpl();
            this.cMenuPianoQuantize64 = new ToolStripMenuItemImpl();
            this.cMenuPianoQuantize128 = new ToolStripMenuItemImpl();
            this.cMenuPianoQuantizeOff = new ToolStripMenuItemImpl();
            this.toolStripMenuItem26 = new ToolStripSeparatorImpl();
            this.cMenuPianoQuantizeTriplet = new ToolStripMenuItemImpl();
            this.cMenuPianoGrid = new ToolStripMenuItemImpl();
            this.toolStripMenuItem14 = new ToolStripSeparatorImpl();
            this.cMenuPianoUndo = new ToolStripMenuItemImpl();
            this.cMenuPianoRedo = new ToolStripMenuItemImpl();
            this.toolStripMenuItem15 = new ToolStripSeparatorImpl();
            this.cMenuPianoCut = new ToolStripMenuItemImpl();
            this.cMenuPianoCopy = new ToolStripMenuItemImpl();
            this.cMenuPianoPaste = new ToolStripMenuItemImpl();
            this.cMenuPianoDelete = new ToolStripMenuItemImpl();
            this.toolStripMenuItem16 = new ToolStripSeparatorImpl();
            this.cMenuPianoSelectAll = new ToolStripMenuItemImpl();
            this.cMenuPianoSelectAllEvents = new ToolStripMenuItemImpl();
            this.toolStripMenuItem17 = new ToolStripSeparatorImpl();
            this.cMenuPianoImportLyric = new ToolStripMenuItemImpl();
            this.cMenuPianoExpressionProperty = new ToolStripMenuItemImpl();
            this.cMenuPianoVibratoProperty = new ToolStripMenuItemImpl();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.cMenuTrackTab = new ContextMenuStripImpl(this.components);
            this.cMenuTrackTabTrackOn = new ToolStripMenuItemImpl();
            this.toolStripMenuItem24 = new ToolStripSeparatorImpl();
            this.cMenuTrackTabAdd = new ToolStripMenuItemImpl();
            this.cMenuTrackTabCopy = new ToolStripMenuItemImpl();
            this.cMenuTrackTabChangeName = new ToolStripMenuItemImpl();
            this.cMenuTrackTabDelete = new ToolStripMenuItemImpl();
            this.toolStripMenuItem25 = new ToolStripSeparatorImpl();
            this.cMenuTrackTabRenderCurrent = new ToolStripMenuItemImpl();
            this.cMenuTrackTabRenderAll = new ToolStripMenuItemImpl();
            this.toolStripMenuItem27 = new ToolStripSeparatorImpl();
            this.cMenuTrackTabOverlay = new ToolStripMenuItemImpl();
            this.cMenuTrackTabRenderer = new ToolStripMenuItemImpl();
            this.cMenuTrackTabRendererVOCALOID1 = new ToolStripMenuItemImpl();
            this.cMenuTrackTabRendererVOCALOID2 = new ToolStripMenuItemImpl();
            this.cMenuTrackTabRendererUtau = new ToolStripMenuItemImpl();
            this.cMenuTrackTabRendererStraight = new ToolStripMenuItemImpl();
            this.cMenuTrackTabRendererAquesTone = new ToolStripMenuItemImpl();
            this.cMenuTrackTabRendererAquesTone2 = new ToolStripMenuItemImpl();
            this.cMenuTrackSelector = new ContextMenuStripImpl(this.components);
            this.cMenuTrackSelectorPointer = new ToolStripMenuItemImpl();
            this.cMenuTrackSelectorPencil = new ToolStripMenuItemImpl();
            this.cMenuTrackSelectorLine = new ToolStripMenuItemImpl();
            this.cMenuTrackSelectorEraser = new ToolStripMenuItemImpl();
            this.cMenuTrackSelectorPaletteTool = new ToolStripMenuItemImpl();
            this.toolStripSeparator14 = new ToolStripSeparatorImpl();
            this.cMenuTrackSelectorCurve = new ToolStripMenuItemImpl();
            this.toolStripMenuItem28 = new ToolStripSeparatorImpl();
            this.cMenuTrackSelectorUndo = new ToolStripMenuItemImpl();
            this.cMenuTrackSelectorRedo = new ToolStripMenuItemImpl();
            this.toolStripMenuItem29 = new ToolStripSeparatorImpl();
            this.cMenuTrackSelectorCut = new ToolStripMenuItemImpl();
            this.cMenuTrackSelectorCopy = new ToolStripMenuItemImpl();
            this.cMenuTrackSelectorPaste = new ToolStripMenuItemImpl();
            this.cMenuTrackSelectorDelete = new ToolStripMenuItemImpl();
            this.cMenuTrackSelectorDeleteBezier = new ToolStripMenuItemImpl();
            this.toolStripMenuItem31 = new ToolStripSeparatorImpl();
            this.cMenuTrackSelectorSelectAll = new ToolStripMenuItemImpl();
            this.trackBar = new TrackBarImpl();
            this.pictureBox3 = new PictureBoxImpl();
            this.pictKeyLengthSplitter = new PictureBoxImpl();
            this.pictureBox2 = new PictureBoxImpl();
            this.vScroll = new VScrollBarImpl();
            this.picturePositionIndicator = new PictureBoxImpl();
            this.toolStripBottom = new ToolStripImpl();
            this.toolStripStatusLabel1 = new ToolStripStatusLabelImpl();
            this.stripLblGameCtrlMode = new ToolStripStatusLabelImpl();
            this.toolStripSeparator10 = new ToolStripSeparatorImpl();
            this.toolStripStatusLabel2 = new ToolStripStatusLabelImpl();
            this.stripLblMidiIn = new ToolStripStatusLabelImpl();
            this.toolStripSeparator11 = new ToolStripSeparatorImpl();
            this.stripBtnStepSequencer = new ToolStripButtonImpl();
            this.splitContainerProperty = ApplicationUIHost.Create<cadencii.apputil.BSplitContainer>();
            this.splitContainer2 = ApplicationUIHost.Create<cadencii.apputil.BSplitContainer>();
            this.splitContainer1 = ApplicationUIHost.Create<cadencii.apputil.BSplitContainer>();
            this.toolStripSeparator2 = new ToolStripSeparatorImpl();
            this.stripDDBtnQuantize = new ContextMenuImpl();
            this.stripDDBtnQuantize04 = new MenuItemImpl();
            this.stripDDBtnQuantize08 = new MenuItemImpl();
            this.stripDDBtnQuantize16 = new MenuItemImpl();
            this.stripDDBtnQuantize32 = new MenuItemImpl();
            this.stripDDBtnQuantize64 = new MenuItemImpl();
            this.stripDDBtnQuantize128 = new MenuItemImpl();
            this.stripDDBtnQuantizeOff = new MenuItemImpl();
            this.menuItem2 = new MenuItemImpl();
            this.stripDDBtnQuantizeTriplet = new MenuItemImpl();
            this.toolStripSeparator3 = new ToolStripSeparatorImpl();
            this.imageListFile = new ImageListImpl(this.components);
            this.imageListPosition = new ImageListImpl(this.components);
            this.imageListMeasure = new ImageListImpl(this.components);
            this.imageListTool = new ImageListImpl(this.components);
            this.panel1 = new PanelImpl();
            this.panelOverview = ApplicationUIHost.Create<cadencii.PictOverview>();
            this.pictPianoRoll = new cadencii.PictPianoRollImpl();
            this.hScroll = new HScrollBarImpl();
            this.rebar = ApplicationUIHost.Create<cadencii.Rebar>();
            this.imageListMenu = new ImageListImpl(this.components);
            this.toolBarFile = new ToolBarImpl();
            this.stripBtnFileNew = new ToolBarButtonImpl ();
            this.stripBtnFileOpen = new ToolBarButtonImpl ();
            this.stripBtnFileSave = new ToolBarButtonImpl ();
            this.toolBarButton1 = new ToolBarButtonImpl ();
            this.stripBtnCut = new ToolBarButtonImpl ();
            this.stripBtnCopy = new ToolBarButtonImpl ();
            this.stripBtnPaste = new ToolBarButtonImpl ();
            this.toolBarButton2 = new ToolBarButtonImpl ();
            this.stripBtnUndo = new ToolBarButtonImpl ();
            this.stripBtnRedo = new ToolBarButtonImpl ();
            this.toolBarPosition = new ToolBarImpl();
            this.stripBtnMoveTop = new ToolBarButtonImpl ();
            this.stripBtnRewind = new ToolBarButtonImpl ();
            this.stripBtnForward = new ToolBarButtonImpl ();
            this.stripBtnMoveEnd = new ToolBarButtonImpl ();
            this.stripBtnPlay = new ToolBarButtonImpl ();
            this.toolBarButton4 = new ToolBarButtonImpl ();
            this.stripBtnScroll = new ToolBarButtonImpl ();
            this.stripBtnLoop = new ToolBarButtonImpl ();
            this.toolBarMeasure = new ToolBarImpl();
            this.stripDDBtnQuantizeParent = new ToolBarButtonImpl ();
            this.toolBarButton5 = new ToolBarButtonImpl ();
            this.stripBtnStartMarker = new ToolBarButtonImpl ();
            this.stripBtnEndMarker = new ToolBarButtonImpl ();
            this.toolBarTool = new ToolBarImpl();
            this.stripBtnPointer = new ToolBarButtonImpl ();
            this.stripBtnPencil = new ToolBarButtonImpl ();
            this.stripBtnLine = new ToolBarButtonImpl ();
            this.stripBtnEraser = new ToolBarButtonImpl ();
            this.toolBarButton3 = new ToolBarButtonImpl ();
            this.stripBtnGrid = new ToolBarButtonImpl ();
            this.stripBtnCurve = new ToolBarButtonImpl ();
            this.toolStripContainer1 = new ToolStripContainerImpl();
            this.statusStrip = new StatusStripImpl();
            this.statusLabel = new ToolStripStatusLabelImpl();
            this.cMenuPositionIndicator = new ContextMenuStripImpl(this.components);
            this.cMenuPositionIndicatorStartMarker = new ToolStripMenuItemImpl();
            this.cMenuPositionIndicatorEndMarker = new ToolStripMenuItemImpl();
            this.menuHelpCheckForUpdates = new ToolStripMenuItemImpl();
            this.menuStripMain.SuspendLayout();
            this.cMenuPiano.SuspendLayout();
            this.cMenuTrackTab.SuspendLayout();
            this.cMenuTrackSelector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictKeyLengthSplitter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePositionIndicator)).BeginInit();
            this.toolStripBottom.SuspendLayout();
            this.splitContainerProperty.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelOverview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictPianoRoll)).BeginInit();
            this.toolStripContainer1.BottomToolStripPanel_SuspendLayout();
            this.toolStripContainer1.ContentPanel_SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.cMenuPositionIndicator.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new UiToolStripItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuVisual,
            this.menuJob,
            this.menuTrack,
            this.menuLyric,
            this.menuScript,
            this.menuSetting,
            this.menuTools,
            this.menuHelp,
            this.menuHidden});
            this.menuStripMain.Location = new Cadencii.Gui.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.RenderMode = ToolStripRenderMode.System;
            this.menuStripMain.Size = new Cadencii.Gui.Dimension(955, 26);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuFile.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuFileNew,
            this.menuFileOpen,
            this.menuFileSave,
            this.menuFileSaveNamed,
            this.toolStripMenuItem10,
            this.menuFileOpenVsq,
            this.menuFileOpenUst,
            this.menuFileImport,
            this.menuFileExport,
            this.toolStripMenuItem11,
            this.menuFileRecent,
            this.toolStripMenuItem12,
            this.menuFileQuit});
            this.menuFile.ImageScaling = ToolStripItemImageScaling.None;
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new Cadencii.Gui.Dimension(57, 22);
            this.menuFile.Text = "File(&F)";
            // 
            // menuFileNew
            // 
            this.menuFileNew.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuFileNew.Name = "menuFileNew";
            this.menuFileNew.Size = new Cadencii.Gui.Dimension(270, 22);
            this.menuFileNew.Text = "New(N)";
            this.menuFileNew.TextImageRelation = TextImageRelation.Overlay;
            // 
            // menuFileOpen
            // 
            this.menuFileOpen.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuFileOpen.Name = "menuFileOpen";
            this.menuFileOpen.Size = new Cadencii.Gui.Dimension(270, 22);
            this.menuFileOpen.Text = "Open(&O)";
            this.menuFileOpen.TextImageRelation = TextImageRelation.Overlay;
            // 
            // menuFileSave
            // 
            this.menuFileSave.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuFileSave.Name = "menuFileSave";
            this.menuFileSave.Size = new Cadencii.Gui.Dimension(270, 22);
            this.menuFileSave.Text = "Save(&S)";
            this.menuFileSave.TextImageRelation = TextImageRelation.Overlay;
            // 
            // menuFileSaveNamed
            // 
            this.menuFileSaveNamed.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuFileSaveNamed.Name = "menuFileSaveNamed";
            this.menuFileSaveNamed.Size = new Cadencii.Gui.Dimension(270, 22);
            this.menuFileSaveNamed.Text = "Save As(&A)";
            this.menuFileSaveNamed.TextImageRelation = TextImageRelation.Overlay;
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new Cadencii.Gui.Dimension(267, 6);
            // 
            // menuFileOpenVsq
            // 
            this.menuFileOpenVsq.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuFileOpenVsq.Name = "menuFileOpenVsq";
            this.menuFileOpenVsq.Size = new Cadencii.Gui.Dimension(270, 22);
            this.menuFileOpenVsq.Text = "Open VSQX/VSQ/Vocaloid Midi(&V)";
            this.menuFileOpenVsq.TextImageRelation = TextImageRelation.Overlay;
            // 
            // menuFileOpenUst
            // 
            this.menuFileOpenUst.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuFileOpenUst.Name = "menuFileOpenUst";
            this.menuFileOpenUst.Size = new Cadencii.Gui.Dimension(270, 22);
            this.menuFileOpenUst.Text = "Open UTAU Project File(&U)";
            this.menuFileOpenUst.TextImageRelation = TextImageRelation.Overlay;
            // 
            // menuFileImport
            // 
            this.menuFileImport.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuFileImport.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuFileImportVsq,
            this.menuFileImportMidi,
            this.menuFileImportUst});
            this.menuFileImport.Name = "menuFileImport";
            this.menuFileImport.Size = new Cadencii.Gui.Dimension(270, 22);
            this.menuFileImport.Text = "Import(&I)";
            this.menuFileImport.TextImageRelation = TextImageRelation.Overlay;
            // 
            // menuFileImportVsq
            // 
            this.menuFileImportVsq.Name = "menuFileImportVsq";
            this.menuFileImportVsq.Size = new Cadencii.Gui.Dimension(175, 22);
            this.menuFileImportVsq.Text = "VSQ File";
            // 
            // menuFileImportMidi
            // 
            this.menuFileImportMidi.Name = "menuFileImportMidi";
            this.menuFileImportMidi.Size = new Cadencii.Gui.Dimension(175, 22);
            this.menuFileImportMidi.Text = "Standard MIDI";
            // 
            // menuFileImportUst
            // 
            this.menuFileImportUst.Name = "menuFileImportUst";
            this.menuFileImportUst.Size = new Cadencii.Gui.Dimension(175, 22);
            this.menuFileImportUst.Text = "UTAU project file";
            // 
            // menuFileExport
            // 
            this.menuFileExport.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuFileExport.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuFileExportWave,
            this.menuFileExportParaWave,
            this.menuFileExportVsq,
            this.menuFileExportVsqx,
            this.menuFileExportMidi,
            this.menuFileExportMusicXml,
            this.menuFileExportUst,
            this.menuFileExportVxt});
            this.menuFileExport.Name = "menuFileExport";
            this.menuFileExport.Size = new Cadencii.Gui.Dimension(270, 22);
            this.menuFileExport.Text = "Export(&E)";
            this.menuFileExport.TextImageRelation = TextImageRelation.Overlay;
            // 
            // menuFileExportWave
            // 
            this.menuFileExportWave.Name = "menuFileExportWave";
            this.menuFileExportWave.Size = new Cadencii.Gui.Dimension(268, 22);
            this.menuFileExportWave.Text = "Wave";
            // 
            // menuFileExportParaWave
            // 
            this.menuFileExportParaWave.Name = "menuFileExportParaWave";
            this.menuFileExportParaWave.Size = new Cadencii.Gui.Dimension(268, 22);
            this.menuFileExportParaWave.Text = "Serial numbered Wave";
            // 
            // menuFileExportVsq
            // 
            this.menuFileExportVsq.Name = "menuFileExportVsq";
            this.menuFileExportVsq.Size = new Cadencii.Gui.Dimension(268, 22);
            this.menuFileExportVsq.Text = "VSQ File";
            // 
            // menuFileExportVsqx
            // 
            this.menuFileExportVsqx.Name = "menuFileExportVsqx";
            this.menuFileExportVsqx.Size = new Cadencii.Gui.Dimension(268, 22);
            this.menuFileExportVsqx.Text = "VSQX File";
            // 
            // menuFileExportMidi
            // 
            this.menuFileExportMidi.Name = "menuFileExportMidi";
            this.menuFileExportMidi.Size = new Cadencii.Gui.Dimension(268, 22);
            this.menuFileExportMidi.Text = "MIDI";
            // 
            // menuFileExportMusicXml
            // 
            this.menuFileExportMusicXml.Name = "menuFileExportMusicXml";
            this.menuFileExportMusicXml.Size = new Cadencii.Gui.Dimension(268, 22);
            this.menuFileExportMusicXml.Text = "MusicXML";
            // 
            // menuFileExportUst
            // 
            this.menuFileExportUst.Name = "menuFileExportUst";
            this.menuFileExportUst.Size = new Cadencii.Gui.Dimension(268, 22);
            this.menuFileExportUst.Text = "UTAU Project File (current track)";
            // 
            // menuFileExportVxt
            // 
            this.menuFileExportVxt.Name = "menuFileExportVxt";
            this.menuFileExportVxt.Size = new Cadencii.Gui.Dimension(268, 22);
            this.menuFileExportVxt.Text = "Metatext for vConnect";
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new Cadencii.Gui.Dimension(267, 6);
            // 
            // menuFileRecent
            // 
            this.menuFileRecent.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuFileRecent.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuFileRecentClear});
            this.menuFileRecent.Name = "menuFileRecent";
            this.menuFileRecent.Size = new Cadencii.Gui.Dimension(270, 22);
            this.menuFileRecent.Text = "Recent Files(&R)";
            this.menuFileRecent.TextImageRelation = TextImageRelation.Overlay;
            // 
            // menuFileRecentClear
            // 
            this.menuFileRecentClear.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuFileRecentClear.Name = "menuFileRecentClear";
            this.menuFileRecentClear.Size = new Cadencii.Gui.Dimension(141, 22);
            this.menuFileRecentClear.Text = "Clear Menu";
            this.menuFileRecentClear.TextImageRelation = TextImageRelation.Overlay;
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new Cadencii.Gui.Dimension(267, 6);
            // 
            // menuFileQuit
            // 
            this.menuFileQuit.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuFileQuit.Name = "menuFileQuit";
            this.menuFileQuit.Size = new Cadencii.Gui.Dimension(270, 22);
            this.menuFileQuit.Text = "Quit(&Q)";
            this.menuFileQuit.TextImageRelation = TextImageRelation.Overlay;
            // 
            // menuEdit
            // 
            this.menuEdit.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuEditUndo,
            this.menuEditRedo,
            this.toolStripMenuItem5,
            this.menuEditCut,
            this.menuEditCopy,
            this.menuEditPaste,
            this.menuEditDelete,
            this.toolStripMenuItem19,
            this.menuEditAutoNormalizeMode,
            this.toolStripMenuItem20,
            this.menuEditSelectAll,
            this.menuEditSelectAllEvents});
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new Cadencii.Gui.Dimension(59, 22);
            this.menuEdit.Text = "Edit(&E)";
            // 
            // menuEditUndo
            // 
            this.menuEditUndo.Name = "menuEditUndo";
            this.menuEditUndo.Size = new Cadencii.Gui.Dimension(220, 22);
            this.menuEditUndo.Text = "Undo(&U)";
            // 
            // menuEditRedo
            // 
            this.menuEditRedo.Name = "menuEditRedo";
            this.menuEditRedo.Size = new Cadencii.Gui.Dimension(220, 22);
            this.menuEditRedo.Text = "Redo(&R)";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new Cadencii.Gui.Dimension(217, 6);
            // 
            // menuEditCut
            // 
            this.menuEditCut.Name = "menuEditCut";
            this.menuEditCut.Size = new Cadencii.Gui.Dimension(220, 22);
            this.menuEditCut.Text = "Cut(&T)";
            // 
            // menuEditCopy
            // 
            this.menuEditCopy.Name = "menuEditCopy";
            this.menuEditCopy.Size = new Cadencii.Gui.Dimension(220, 22);
            this.menuEditCopy.Text = "Copy(&C)";
            // 
            // menuEditPaste
            // 
            this.menuEditPaste.Name = "menuEditPaste";
            this.menuEditPaste.ShortcutKeyDisplayString = "";
            this.menuEditPaste.Size = new Cadencii.Gui.Dimension(220, 22);
            this.menuEditPaste.Text = "Paste(&P)";
            // 
            // menuEditDelete
            // 
            this.menuEditDelete.Name = "menuEditDelete";
            this.menuEditDelete.Size = new Cadencii.Gui.Dimension(220, 22);
            this.menuEditDelete.Text = "Delete(&D)";
            // 
            // toolStripMenuItem19
            // 
            this.toolStripMenuItem19.Name = "toolStripMenuItem19";
            this.toolStripMenuItem19.Size = new Cadencii.Gui.Dimension(217, 6);
            // 
            // menuEditAutoNormalizeMode
            // 
            this.menuEditAutoNormalizeMode.Name = "menuEditAutoNormalizeMode";
            this.menuEditAutoNormalizeMode.Size = new Cadencii.Gui.Dimension(220, 22);
            this.menuEditAutoNormalizeMode.Text = "Auto Normalize Mode(&N)";
            // 
            // toolStripMenuItem20
            // 
            this.toolStripMenuItem20.Name = "toolStripMenuItem20";
            this.toolStripMenuItem20.Size = new Cadencii.Gui.Dimension(217, 6);
            // 
            // menuEditSelectAll
            // 
            this.menuEditSelectAll.Name = "menuEditSelectAll";
            this.menuEditSelectAll.Size = new Cadencii.Gui.Dimension(220, 22);
            this.menuEditSelectAll.Text = "Select All(&A)";
            // 
            // menuEditSelectAllEvents
            // 
            this.menuEditSelectAllEvents.Name = "menuEditSelectAllEvents";
            this.menuEditSelectAllEvents.Size = new Cadencii.Gui.Dimension(220, 22);
            this.menuEditSelectAllEvents.Text = "Select All Events(&E)";
            // 
            // menuVisual
            // 
            this.menuVisual.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuVisual.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuVisualControlTrack,
            this.menuVisualMixer,
            this.menuVisualWaveform,
            this.menuVisualIconPalette,
            this.menuVisualProperty,
            this.menuVisualOverview,
            this.menuVisualPluginUi,
            this.toolStripMenuItem1,
            this.menuVisualGridline,
            this.toolStripMenuItem2,
            this.menuVisualStartMarker,
            this.menuVisualEndMarker,
            this.toolStripMenuItem3,
            this.menuVisualLyrics,
            this.menuVisualNoteProperty,
            this.menuVisualPitchLine});
            this.menuVisual.Name = "menuVisual";
            this.menuVisual.Size = new Cadencii.Gui.Dimension(66, 22);
            this.menuVisual.Text = "View(&V)";
            // 
            // menuVisualControlTrack
            // 
            this.menuVisualControlTrack.Checked = true;
            this.menuVisualControlTrack.CheckOnClick = true;
            this.menuVisualControlTrack.CheckState = CheckState.Checked;
            this.menuVisualControlTrack.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuVisualControlTrack.Name = "menuVisualControlTrack";
            this.menuVisualControlTrack.Size = new Cadencii.Gui.Dimension(237, 22);
            this.menuVisualControlTrack.Text = "Control Track(&C)";
            // 
            // menuVisualMixer
            // 
            this.menuVisualMixer.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuVisualMixer.Name = "menuVisualMixer";
            this.menuVisualMixer.Size = new Cadencii.Gui.Dimension(237, 22);
            this.menuVisualMixer.Text = "Mixer(&X)";
            // 
            // menuVisualWaveform
            // 
            this.menuVisualWaveform.CheckOnClick = true;
            this.menuVisualWaveform.Name = "menuVisualWaveform";
            this.menuVisualWaveform.Size = new Cadencii.Gui.Dimension(237, 22);
            this.menuVisualWaveform.Text = "Waveform(&W)";
            // 
            // menuVisualIconPalette
            // 
            this.menuVisualIconPalette.CheckOnClick = true;
            this.menuVisualIconPalette.Name = "menuVisualIconPalette";
            this.menuVisualIconPalette.Size = new Cadencii.Gui.Dimension(237, 22);
            this.menuVisualIconPalette.Text = "Icon Palette(&I)";
            // 
            // menuVisualProperty
            // 
            this.menuVisualProperty.CheckOnClick = true;
            this.menuVisualProperty.Name = "menuVisualProperty";
            this.menuVisualProperty.Size = new Cadencii.Gui.Dimension(237, 22);
            this.menuVisualProperty.Text = "Property Window(&C)";
            // 
            // menuVisualOverview
            // 
            this.menuVisualOverview.CheckOnClick = true;
            this.menuVisualOverview.Name = "menuVisualOverview";
            this.menuVisualOverview.Size = new Cadencii.Gui.Dimension(237, 22);
            this.menuVisualOverview.Text = "Overview(&O)";
            // 
            // menuVisualPluginUi
            // 
            this.menuVisualPluginUi.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuVisualPluginUiVocaloid1,
            this.menuVisualPluginUiVocaloid2,
            this.menuVisualPluginUiAquesTone,
            this.menuVisualPluginUiAquesTone2});
            this.menuVisualPluginUi.Name = "menuVisualPluginUi";
            this.menuVisualPluginUi.Size = new Cadencii.Gui.Dimension(237, 22);
            this.menuVisualPluginUi.Text = "VSTi Plugin UI(&U)";
            // 
            // menuVisualPluginUiVocaloid1
            // 
            this.menuVisualPluginUiVocaloid1.Name = "menuVisualPluginUiVocaloid1";
            this.menuVisualPluginUiVocaloid1.Size = new Cadencii.Gui.Dimension(157, 22);
            this.menuVisualPluginUiVocaloid1.Text = "VOCALOID1";
            // 
            // menuVisualPluginUiVocaloid2
            // 
            this.menuVisualPluginUiVocaloid2.Name = "menuVisualPluginUiVocaloid2";
            this.menuVisualPluginUiVocaloid2.Size = new Cadencii.Gui.Dimension(157, 22);
            this.menuVisualPluginUiVocaloid2.Text = "VOCALOID2";
            // 
            // menuVisualPluginUiAquesTone
            // 
            this.menuVisualPluginUiAquesTone.Name = "menuVisualPluginUiAquesTone";
            this.menuVisualPluginUiAquesTone.Size = new Cadencii.Gui.Dimension(157, 22);
            this.menuVisualPluginUiAquesTone.Text = "AquesTone(&A)";
            // 
            // menuVisualPluginUiAquesTone2
            // 
            this.menuVisualPluginUiAquesTone2.Name = "menuVisualPluginUiAquesTone2";
            this.menuVisualPluginUiAquesTone2.Size = new Cadencii.Gui.Dimension(157, 22);
            this.menuVisualPluginUiAquesTone2.Text = "AquesTone2";
			this.menuVisualPluginUiAquesTone2.Click += (o, e) => model.VisualMenu.RunVisualPluginUiAquesTone2Command ();
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new Cadencii.Gui.Dimension(234, 6);
            // 
            // menuVisualGridline
            // 
            this.menuVisualGridline.CheckOnClick = true;
            this.menuVisualGridline.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuVisualGridline.Name = "menuVisualGridline";
            this.menuVisualGridline.Size = new Cadencii.Gui.Dimension(237, 22);
            this.menuVisualGridline.Text = "Grid Line(&G)";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new Cadencii.Gui.Dimension(234, 6);
            // 
            // menuVisualStartMarker
            // 
            this.menuVisualStartMarker.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuVisualStartMarker.Name = "menuVisualStartMarker";
            this.menuVisualStartMarker.Size = new Cadencii.Gui.Dimension(237, 22);
            this.menuVisualStartMarker.Text = "Start Marker(&S)";
            // 
            // menuVisualEndMarker
            // 
            this.menuVisualEndMarker.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuVisualEndMarker.Name = "menuVisualEndMarker";
            this.menuVisualEndMarker.Size = new Cadencii.Gui.Dimension(237, 22);
            this.menuVisualEndMarker.Text = "End Marker(&E)";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new Cadencii.Gui.Dimension(234, 6);
            // 
            // menuVisualLyrics
            // 
            this.menuVisualLyrics.Checked = true;
            this.menuVisualLyrics.CheckOnClick = true;
            this.menuVisualLyrics.CheckState = CheckState.Checked;
            this.menuVisualLyrics.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuVisualLyrics.Name = "menuVisualLyrics";
            this.menuVisualLyrics.Size = new Cadencii.Gui.Dimension(237, 22);
            this.menuVisualLyrics.Text = "Lyric/Phoneme(&L)";
            // 
            // menuVisualNoteProperty
            // 
            this.menuVisualNoteProperty.Checked = true;
            this.menuVisualNoteProperty.CheckOnClick = true;
            this.menuVisualNoteProperty.CheckState = CheckState.Checked;
            this.menuVisualNoteProperty.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.menuVisualNoteProperty.Name = "menuVisualNoteProperty";
            this.menuVisualNoteProperty.Size = new Cadencii.Gui.Dimension(237, 22);
            this.menuVisualNoteProperty.Text = "Note Expression/Vibrato(&N)";
            // 
            // menuVisualPitchLine
            // 
            this.menuVisualPitchLine.CheckOnClick = true;
            this.menuVisualPitchLine.Name = "menuVisualPitchLine";
            this.menuVisualPitchLine.Size = new Cadencii.Gui.Dimension(237, 22);
            this.menuVisualPitchLine.Text = "Pitch Line(&P)";
            // 
            // menuJob
            // 
            this.menuJob.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuJobNormalize,
            this.menuJobInsertBar,
            this.menuJobDeleteBar,
            this.menuJobRandomize,
            this.menuJobConnect,
            this.menuJobLyric,
            this.menuJobRewire,
            this.menuJobReloadVsti});
            this.menuJob.Name = "menuJob";
            this.menuJob.Size = new Cadencii.Gui.Dimension(54, 22);
            this.menuJob.Text = "Job(&J)";
            // 
            // menuJobNormalize
            // 
            this.menuJobNormalize.Name = "menuJobNormalize";
            this.menuJobNormalize.Size = new Cadencii.Gui.Dimension(256, 22);
            this.menuJobNormalize.Text = "Normalize Notes(&N)";
            // 
            // menuJobInsertBar
            // 
            this.menuJobInsertBar.Name = "menuJobInsertBar";
            this.menuJobInsertBar.Size = new Cadencii.Gui.Dimension(256, 22);
            this.menuJobInsertBar.Text = "Insert Bars(&I)";
            // 
            // menuJobDeleteBar
            // 
            this.menuJobDeleteBar.Name = "menuJobDeleteBar";
            this.menuJobDeleteBar.Size = new Cadencii.Gui.Dimension(256, 22);
            this.menuJobDeleteBar.Text = "Delete Bars(&D)";
            // 
            // menuJobRandomize
            // 
            this.menuJobRandomize.Name = "menuJobRandomize";
            this.menuJobRandomize.Size = new Cadencii.Gui.Dimension(256, 22);
            this.menuJobRandomize.Text = "Randomize(&R)";
            // 
            // menuJobConnect
            // 
            this.menuJobConnect.Name = "menuJobConnect";
            this.menuJobConnect.Size = new Cadencii.Gui.Dimension(256, 22);
            this.menuJobConnect.Text = "Connect Notes(&C)";
            // 
            // menuJobLyric
            // 
            this.menuJobLyric.Name = "menuJobLyric";
            this.menuJobLyric.Size = new Cadencii.Gui.Dimension(256, 22);
            this.menuJobLyric.Text = "Insert Lyrics(&L)";
            // 
            // menuJobRewire
            // 
            this.menuJobRewire.Enabled = false;
            this.menuJobRewire.Name = "menuJobRewire";
            this.menuJobRewire.Size = new Cadencii.Gui.Dimension(256, 22);
            this.menuJobRewire.Text = "Import ReWire Host Tempo(&T)";
            // 
            // menuJobReloadVsti
            // 
            this.menuJobReloadVsti.Name = "menuJobReloadVsti";
            this.menuJobReloadVsti.Size = new Cadencii.Gui.Dimension(256, 22);
            this.menuJobReloadVsti.Text = "Reload VSTi(&R)";
            this.menuJobReloadVsti.Visible = false;
            // 
            // menuTrack
            // 
            this.menuTrack.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuTrackOn,
            this.toolStripMenuItem21,
            this.menuTrackAdd,
            this.menuTrackCopy,
            this.menuTrackChangeName,
            this.menuTrackDelete,
            this.toolStripMenuItem22,
            this.menuTrackRenderCurrent,
            this.menuTrackRenderAll,
            this.toolStripMenuItem23,
            this.menuTrackOverlay,
            this.menuTrackRenderer,
            this.toolStripMenuItem4,
            this.menuTrackBgm});
            this.menuTrack.Name = "menuTrack";
            this.menuTrack.Size = new Cadencii.Gui.Dimension(70, 22);
            this.menuTrack.Text = "Track(&T)";
            // 
            // menuTrackOn
            // 
            this.menuTrackOn.Name = "menuTrackOn";
            this.menuTrackOn.Size = new Cadencii.Gui.Dimension(219, 22);
            this.menuTrackOn.Text = "Track On(&K)";
            // 
            // toolStripMenuItem21
            // 
            this.toolStripMenuItem21.Name = "toolStripMenuItem21";
            this.toolStripMenuItem21.Size = new Cadencii.Gui.Dimension(216, 6);
            // 
            // menuTrackAdd
            // 
            this.menuTrackAdd.Name = "menuTrackAdd";
            this.menuTrackAdd.Size = new Cadencii.Gui.Dimension(219, 22);
            this.menuTrackAdd.Text = "Add Track(&A)";
            // 
            // menuTrackCopy
            // 
            this.menuTrackCopy.Name = "menuTrackCopy";
            this.menuTrackCopy.Size = new Cadencii.Gui.Dimension(219, 22);
            this.menuTrackCopy.Text = "Copy Track(&C)";
            // 
            // menuTrackChangeName
            // 
            this.menuTrackChangeName.Name = "menuTrackChangeName";
            this.menuTrackChangeName.Size = new Cadencii.Gui.Dimension(219, 22);
            this.menuTrackChangeName.Text = "Rename Track";
            // 
            // menuTrackDelete
            // 
            this.menuTrackDelete.Name = "menuTrackDelete";
            this.menuTrackDelete.Size = new Cadencii.Gui.Dimension(219, 22);
            this.menuTrackDelete.Text = "Delete Track(&D)";
            // 
            // toolStripMenuItem22
            // 
            this.toolStripMenuItem22.Name = "toolStripMenuItem22";
            this.toolStripMenuItem22.Size = new Cadencii.Gui.Dimension(216, 6);
            // 
            // menuTrackRenderCurrent
            // 
            this.menuTrackRenderCurrent.Name = "menuTrackRenderCurrent";
            this.menuTrackRenderCurrent.Size = new Cadencii.Gui.Dimension(219, 22);
            this.menuTrackRenderCurrent.Text = "Render Current Track(&T)";
            // 
            // menuTrackRenderAll
            // 
            this.menuTrackRenderAll.Enabled = false;
            this.menuTrackRenderAll.Name = "menuTrackRenderAll";
            this.menuTrackRenderAll.Size = new Cadencii.Gui.Dimension(219, 22);
            this.menuTrackRenderAll.Text = "Render All Tracks(&S)";
            // 
            // toolStripMenuItem23
            // 
            this.toolStripMenuItem23.Name = "toolStripMenuItem23";
            this.toolStripMenuItem23.Size = new Cadencii.Gui.Dimension(216, 6);
            // 
            // menuTrackOverlay
            // 
            this.menuTrackOverlay.Name = "menuTrackOverlay";
            this.menuTrackOverlay.Size = new Cadencii.Gui.Dimension(219, 22);
            this.menuTrackOverlay.Text = "Overlay(&O)";
            // 
            // menuTrackRenderer
            // 
            this.menuTrackRenderer.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuTrackRendererVOCALOID1,
            this.menuTrackRendererVOCALOID2,
            this.menuTrackRendererUtau,
            this.menuTrackRendererVCNT,
            this.menuTrackRendererAquesTone,
            this.menuTrackRendererAquesTone2});
            this.menuTrackRenderer.Name = "menuTrackRenderer";
            this.menuTrackRenderer.Size = new Cadencii.Gui.Dimension(219, 22);
            this.menuTrackRenderer.Text = "Renderer(&R)";
            // 
            // menuTrackRendererVOCALOID1
            // 
            this.menuTrackRendererVOCALOID1.Name = "menuTrackRendererVOCALOID1";
            this.menuTrackRendererVOCALOID1.Size = new Cadencii.Gui.Dimension(193, 22);
            this.menuTrackRendererVOCALOID1.Text = "VOCALOID1(&1)";
            // 
            // menuTrackRendererVOCALOID2
            // 
            this.menuTrackRendererVOCALOID2.Name = "menuTrackRendererVOCALOID2";
            this.menuTrackRendererVOCALOID2.Size = new Cadencii.Gui.Dimension(193, 22);
            this.menuTrackRendererVOCALOID2.Text = "VOCALOID2(&2)";
            // 
            // menuTrackRendererUtau
            // 
            this.menuTrackRendererUtau.Name = "menuTrackRendererUtau";
            this.menuTrackRendererUtau.Size = new Cadencii.Gui.Dimension(193, 22);
            this.menuTrackRendererUtau.Text = "UTAU(&3)";
            // 
            // menuTrackRendererVCNT
            // 
            this.menuTrackRendererVCNT.Name = "menuTrackRendererVCNT";
            this.menuTrackRendererVCNT.Size = new Cadencii.Gui.Dimension(193, 22);
            this.menuTrackRendererVCNT.Text = "vConnect-STAND(&4)";
            // 
            // menuTrackRendererAquesTone
            // 
            this.menuTrackRendererAquesTone.Name = "menuTrackRendererAquesTone";
            this.menuTrackRendererAquesTone.Size = new Cadencii.Gui.Dimension(193, 22);
            this.menuTrackRendererAquesTone.Text = "AquesTone(&5)";
            // 
            // menuTrackRendererAquesTone2
            // 
            this.menuTrackRendererAquesTone2.Name = "menuTrackRendererAquesTone2";
            this.menuTrackRendererAquesTone2.Size = new Cadencii.Gui.Dimension(193, 22);
            this.menuTrackRendererAquesTone2.Text = "AquesTone2(&6)";
			this.menuTrackRendererAquesTone2.Click += (o, e) => model.TrackMenu.RunChangeRendererCommand (RendererKind.AQUES_TONE2, -1);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new Cadencii.Gui.Dimension(216, 6);
            // 
            // menuTrackBgm
            // 
            this.menuTrackBgm.Name = "menuTrackBgm";
            this.menuTrackBgm.Size = new Cadencii.Gui.Dimension(219, 22);
            this.menuTrackBgm.Text = "BGM(&B)";
            // 
            // menuLyric
            // 
            this.menuLyric.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuLyricExpressionProperty,
            this.menuLyricVibratoProperty,
            this.menuLyricApplyUtauParameters,
            this.menuLyricPhonemeTransformation,
            this.menuLyricDictionary,
            this.menuLyricCopyVibratoToPreset});
            this.menuLyric.Name = "menuLyric";
            this.menuLyric.Size = new Cadencii.Gui.Dimension(70, 22);
            this.menuLyric.Text = "Lyrics(&L)";
            // 
            // menuLyricExpressionProperty
            // 
            this.menuLyricExpressionProperty.Name = "menuLyricExpressionProperty";
            this.menuLyricExpressionProperty.Size = new Cadencii.Gui.Dimension(262, 22);
            this.menuLyricExpressionProperty.Text = "Note Expression Property(&E)";
            // 
            // menuLyricVibratoProperty
            // 
            this.menuLyricVibratoProperty.Name = "menuLyricVibratoProperty";
            this.menuLyricVibratoProperty.Size = new Cadencii.Gui.Dimension(262, 22);
            this.menuLyricVibratoProperty.Text = "Note Vibrato Property(&V)";
            // 
            // menuLyricApplyUtauParameters
            // 
            this.menuLyricApplyUtauParameters.Name = "menuLyricApplyUtauParameters";
            this.menuLyricApplyUtauParameters.Size = new Cadencii.Gui.Dimension(262, 22);
            this.menuLyricApplyUtauParameters.Text = "Apply UTAU Parameters(&A)";
            // 
            // menuLyricPhonemeTransformation
            // 
            this.menuLyricPhonemeTransformation.Name = "menuLyricPhonemeTransformation";
            this.menuLyricPhonemeTransformation.Size = new Cadencii.Gui.Dimension(262, 22);
            this.menuLyricPhonemeTransformation.Text = "Phoneme Transformation(&T)";
            // 
            // menuLyricDictionary
            // 
            this.menuLyricDictionary.Name = "menuLyricDictionary";
            this.menuLyricDictionary.Size = new Cadencii.Gui.Dimension(262, 22);
            this.menuLyricDictionary.Text = "User Word Dictionary(&C)";
            // 
            // menuLyricCopyVibratoToPreset
            // 
            this.menuLyricCopyVibratoToPreset.Name = "menuLyricCopyVibratoToPreset";
            this.menuLyricCopyVibratoToPreset.Size = new Cadencii.Gui.Dimension(262, 22);
            this.menuLyricCopyVibratoToPreset.Text = "Copy vibrato config to preset(&P)";
            // 
            // menuScript
            // 
            this.menuScript.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuScriptUpdate});
            this.menuScript.Name = "menuScript";
            this.menuScript.Size = new Cadencii.Gui.Dimension(72, 22);
            this.menuScript.Text = "Script(&C)";
            // 
            // menuScriptUpdate
            // 
            this.menuScriptUpdate.Name = "menuScriptUpdate";
            this.menuScriptUpdate.Size = new Cadencii.Gui.Dimension(200, 22);
            this.menuScriptUpdate.Text = "Update Script List(&U)";
            // 
            // menuSetting
            // 
            this.menuSetting.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuSettingPreference,
            this.menuSettingSequence,
            this.menuSettingPositionQuantize,
            this.toolStripMenuItem8,
            this.menuSettingGameControler,
            this.menuSettingPaletteTool,
            this.menuSettingShortcut,
            this.menuSettingVibratoPreset,
            this.toolStripMenuItem6,
            this.menuSettingDefaultSingerStyle});
            this.menuSetting.Name = "menuSetting";
            this.menuSetting.Size = new Cadencii.Gui.Dimension(80, 22);
            this.menuSetting.Text = "Setting(&S)";
            // 
            // menuSettingPreference
            // 
            this.menuSettingPreference.Name = "menuSettingPreference";
            this.menuSettingPreference.Size = new Cadencii.Gui.Dimension(223, 22);
            this.menuSettingPreference.Text = "Preference(&P)";
            // 
            // menuSettingSequence
            // 
            this.menuSettingSequence.Name = "menuSettingSequence";
            this.menuSettingSequence.Size = new Cadencii.Gui.Dimension(223, 22);
            this.menuSettingSequence.Text = "Sequence config(&S)";
            // 
            // menuSettingPositionQuantize
            // 
            this.menuSettingPositionQuantize.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuSettingPositionQuantize04,
            this.menuSettingPositionQuantize08,
            this.menuSettingPositionQuantize16,
            this.menuSettingPositionQuantize32,
            this.menuSettingPositionQuantize64,
            this.menuSettingPositionQuantize128,
            this.menuSettingPositionQuantizeOff,
            this.toolStripMenuItem9,
            this.menuSettingPositionQuantizeTriplet});
            this.menuSettingPositionQuantize.Name = "menuSettingPositionQuantize";
            this.menuSettingPositionQuantize.Size = new Cadencii.Gui.Dimension(223, 22);
            this.menuSettingPositionQuantize.Text = "Quantize(&Q)";
            // 
            // menuSettingPositionQuantize04
            // 
            this.menuSettingPositionQuantize04.Name = "menuSettingPositionQuantize04";
            this.menuSettingPositionQuantize04.Size = new Cadencii.Gui.Dimension(113, 22);
            this.menuSettingPositionQuantize04.Text = "1/4";
            // 
            // menuSettingPositionQuantize08
            // 
            this.menuSettingPositionQuantize08.Name = "menuSettingPositionQuantize08";
            this.menuSettingPositionQuantize08.Size = new Cadencii.Gui.Dimension(113, 22);
            this.menuSettingPositionQuantize08.Text = "1/8";
            // 
            // menuSettingPositionQuantize16
            // 
            this.menuSettingPositionQuantize16.Name = "menuSettingPositionQuantize16";
            this.menuSettingPositionQuantize16.Size = new Cadencii.Gui.Dimension(113, 22);
            this.menuSettingPositionQuantize16.Text = "1/16";
            // 
            // menuSettingPositionQuantize32
            // 
            this.menuSettingPositionQuantize32.Name = "menuSettingPositionQuantize32";
            this.menuSettingPositionQuantize32.Size = new Cadencii.Gui.Dimension(113, 22);
            this.menuSettingPositionQuantize32.Text = "1/32";
            // 
            // menuSettingPositionQuantize64
            // 
            this.menuSettingPositionQuantize64.Name = "menuSettingPositionQuantize64";
            this.menuSettingPositionQuantize64.Size = new Cadencii.Gui.Dimension(113, 22);
            this.menuSettingPositionQuantize64.Text = "1/64";
            // 
            // menuSettingPositionQuantize128
            // 
            this.menuSettingPositionQuantize128.Name = "menuSettingPositionQuantize128";
            this.menuSettingPositionQuantize128.Size = new Cadencii.Gui.Dimension(113, 22);
            this.menuSettingPositionQuantize128.Text = "1/128";
            // 
            // menuSettingPositionQuantizeOff
            // 
            this.menuSettingPositionQuantizeOff.Name = "menuSettingPositionQuantizeOff";
            this.menuSettingPositionQuantizeOff.Size = new Cadencii.Gui.Dimension(113, 22);
            this.menuSettingPositionQuantizeOff.Text = "Off";
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new Cadencii.Gui.Dimension(110, 6);
            // 
            // menuSettingPositionQuantizeTriplet
            // 
            this.menuSettingPositionQuantizeTriplet.Name = "menuSettingPositionQuantizeTriplet";
            this.menuSettingPositionQuantizeTriplet.Size = new Cadencii.Gui.Dimension(113, 22);
            this.menuSettingPositionQuantizeTriplet.Text = "Triplet";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new Cadencii.Gui.Dimension(220, 6);
            // 
            // menuSettingGameControler
            // 
            this.menuSettingGameControler.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuSettingGameControlerSetting,
            this.menuSettingGameControlerLoad,
            this.menuSettingGameControlerRemove});
            this.menuSettingGameControler.Name = "menuSettingGameControler";
            this.menuSettingGameControler.Size = new Cadencii.Gui.Dimension(223, 22);
            this.menuSettingGameControler.Text = "Game Controler(&G)";
            // 
            // menuSettingGameControlerSetting
            // 
            this.menuSettingGameControlerSetting.Name = "menuSettingGameControlerSetting";
            this.menuSettingGameControlerSetting.Size = new Cadencii.Gui.Dimension(142, 22);
            this.menuSettingGameControlerSetting.Text = "Setting(&S)";
            // 
            // menuSettingGameControlerLoad
            // 
            this.menuSettingGameControlerLoad.Name = "menuSettingGameControlerLoad";
            this.menuSettingGameControlerLoad.Size = new Cadencii.Gui.Dimension(142, 22);
            this.menuSettingGameControlerLoad.Text = "Load(&L)";
            // 
            // menuSettingGameControlerRemove
            // 
            this.menuSettingGameControlerRemove.Name = "menuSettingGameControlerRemove";
            this.menuSettingGameControlerRemove.Size = new Cadencii.Gui.Dimension(142, 22);
            this.menuSettingGameControlerRemove.Text = "Remove(&R)";
            // 
            // menuSettingPaletteTool
            // 
            this.menuSettingPaletteTool.Name = "menuSettingPaletteTool";
            this.menuSettingPaletteTool.Size = new Cadencii.Gui.Dimension(223, 22);
            this.menuSettingPaletteTool.Text = "Palette Tool(&T)";
            // 
            // menuSettingShortcut
            // 
            this.menuSettingShortcut.Name = "menuSettingShortcut";
            this.menuSettingShortcut.Size = new Cadencii.Gui.Dimension(223, 22);
            this.menuSettingShortcut.Text = "Shortcut Key(&K)";
            // 
            // menuSettingVibratoPreset
            // 
            this.menuSettingVibratoPreset.Name = "menuSettingVibratoPreset";
            this.menuSettingVibratoPreset.Size = new Cadencii.Gui.Dimension(223, 22);
            this.menuSettingVibratoPreset.Text = "Vibrato preset(&V)";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new Cadencii.Gui.Dimension(220, 6);
            // 
            // menuSettingDefaultSingerStyle
            // 
            this.menuSettingDefaultSingerStyle.Name = "menuSettingDefaultSingerStyle";
            this.menuSettingDefaultSingerStyle.Size = new Cadencii.Gui.Dimension(223, 22);
            this.menuSettingDefaultSingerStyle.Text = "Singing Style Defaults(&D)";
            // 
            // menuTools
            // 
            this.menuTools.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuToolsCreateVConnectSTANDDb});
            this.menuTools.Name = "menuTools";
            this.menuTools.Size = new Cadencii.Gui.Dimension(69, 22);
            this.menuTools.Text = "Tools(&O)";
            // 
            // menuToolsCreateVConnectSTANDDb
            // 
            this.menuToolsCreateVConnectSTANDDb.Name = "menuToolsCreateVConnectSTANDDb";
            this.menuToolsCreateVConnectSTANDDb.Size = new Cadencii.Gui.Dimension(240, 22);
            this.menuToolsCreateVConnectSTANDDb.Text = "Create vConnect-STAND DB";
            this.menuToolsCreateVConnectSTANDDb.Click += new System.EventHandler(this.menuToolsCreateVConnectSTANDDb_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuHelpAbout,
            this.menuHelpManual,
            this.menuHelpCheckForUpdates,
            this.menuHelpLog,
            this.menuHelpDebug});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new Cadencii.Gui.Dimension(65, 22);
            this.menuHelp.Text = "Help(&H)";
            // 
            // menuHelpAbout
            // 
            this.menuHelpAbout.Name = "menuHelpAbout";
            this.menuHelpAbout.Size = new Cadencii.Gui.Dimension(186, 22);
            this.menuHelpAbout.Text = "About Cadencii(&A)";
            // 
            // menuHelpManual
            // 
            this.menuHelpManual.Name = "menuHelpManual";
            this.menuHelpManual.Size = new Cadencii.Gui.Dimension(186, 22);
            this.menuHelpManual.Text = "Manual (PDF)";
            // 
            // menuHelpLog
            // 
            this.menuHelpLog.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuHelpLogSwitch,
            this.menuHelpLogOpen});
            this.menuHelpLog.Name = "menuHelpLog";
            this.menuHelpLog.Size = new Cadencii.Gui.Dimension(186, 22);
            this.menuHelpLog.Text = "Log(&L)";
            // 
            // menuHelpLogSwitch
            // 
            this.menuHelpLogSwitch.CheckOnClick = true;
            this.menuHelpLogSwitch.Name = "menuHelpLogSwitch";
            this.menuHelpLogSwitch.Size = new Cadencii.Gui.Dimension(156, 22);
            this.menuHelpLogSwitch.Text = "Enable Log(&L)";
            // 
            // menuHelpLogOpen
            // 
            this.menuHelpLogOpen.Name = "menuHelpLogOpen";
            this.menuHelpLogOpen.Size = new Cadencii.Gui.Dimension(156, 22);
            this.menuHelpLogOpen.Text = "Open(&O)";
            // 
            // menuHelpDebug
            // 
            this.menuHelpDebug.Name = "menuHelpDebug";
            this.menuHelpDebug.Size = new Cadencii.Gui.Dimension(186, 22);
            this.menuHelpDebug.Text = "Debug";
            this.menuHelpDebug.Visible = false;
            // 
            // menuHidden
            // 
            this.menuHidden.DropDownItems.AddRange(new UiToolStripItem[] {
            this.menuHiddenEditLyric,
            this.menuHiddenEditFlipToolPointerPencil,
            this.menuHiddenEditFlipToolPointerEraser,
            this.menuHiddenVisualForwardParameter,
            this.menuHiddenVisualBackwardParameter,
            this.menuHiddenTrackNext,
            this.menuHiddenTrackBack,
            this.menuHiddenCopy,
            this.menuHiddenPaste,
            this.menuHiddenCut,
            this.menuHiddenSelectForward,
            this.menuHiddenSelectBackward,
            this.menuHiddenMoveUp,
            this.menuHiddenMoveDown,
            this.menuHiddenMoveLeft,
            this.menuHiddenMoveRight,
            this.menuHiddenLengthen,
            this.menuHiddenShorten,
            this.menuHiddenGoToStartMarker,
            this.menuHiddenGoToEndMarker,
            this.menuHiddenPlayFromStartMarker,
            this.menuHiddenFlipCurveOnPianorollMode,
            this.menuHiddenPrintPoToCSV});
            this.menuHidden.Name = "menuHidden";
            this.menuHidden.Size = new Cadencii.Gui.Dimension(91, 22);
            this.menuHidden.Text = "MenuHidden";
            this.menuHidden.Visible = false;
            // 
            // menuHiddenEditLyric
            // 
            this.menuHiddenEditLyric.Name = "menuHiddenEditLyric";
            this.menuHiddenEditLyric.ShortcutKeys = Keys.F2;
            this.menuHiddenEditLyric.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenEditLyric.Text = "Start Lyric Input";
            this.menuHiddenEditLyric.Visible = false;
            // 
            // menuHiddenEditFlipToolPointerPencil
            // 
            this.menuHiddenEditFlipToolPointerPencil.Name = "menuHiddenEditFlipToolPointerPencil";
            this.menuHiddenEditFlipToolPointerPencil.ShortcutKeys = ((Keys)((Keys.Control | Keys.W)));
            this.menuHiddenEditFlipToolPointerPencil.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenEditFlipToolPointerPencil.Text = "Change Tool Pointer / Pencil";
            this.menuHiddenEditFlipToolPointerPencil.Visible = false;
            // 
            // menuHiddenEditFlipToolPointerEraser
            // 
            this.menuHiddenEditFlipToolPointerEraser.Name = "menuHiddenEditFlipToolPointerEraser";
            this.menuHiddenEditFlipToolPointerEraser.ShortcutKeys = ((Keys)((Keys.Control | Keys.E)));
            this.menuHiddenEditFlipToolPointerEraser.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenEditFlipToolPointerEraser.Text = "Change Tool Pointer/ Eraser";
            this.menuHiddenEditFlipToolPointerEraser.Visible = false;
            // 
            // menuHiddenVisualForwardParameter
            // 
            this.menuHiddenVisualForwardParameter.Name = "menuHiddenVisualForwardParameter";
            this.menuHiddenVisualForwardParameter.ShortcutKeys = ((Keys)(((Keys.Control | Keys.Alt)
            | Keys.Next)));
            this.menuHiddenVisualForwardParameter.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenVisualForwardParameter.Text = "Next Control Curve";
            this.menuHiddenVisualForwardParameter.Visible = false;
            // 
            // menuHiddenVisualBackwardParameter
            // 
            this.menuHiddenVisualBackwardParameter.Name = "menuHiddenVisualBackwardParameter";
            this.menuHiddenVisualBackwardParameter.ShortcutKeys = ((Keys)(((Keys.Control | Keys.Alt)
            | Keys.PageUp)));
            this.menuHiddenVisualBackwardParameter.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenVisualBackwardParameter.Text = "Previous Control Curve";
            this.menuHiddenVisualBackwardParameter.Visible = false;
            // 
            // menuHiddenTrackNext
            // 
            this.menuHiddenTrackNext.Name = "menuHiddenTrackNext";
            this.menuHiddenTrackNext.ShortcutKeys = ((Keys)((Keys.Control | Keys.Next)));
            this.menuHiddenTrackNext.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenTrackNext.Text = "Next Track";
            this.menuHiddenTrackNext.Visible = false;
            // 
            // menuHiddenTrackBack
            // 
            this.menuHiddenTrackBack.Name = "menuHiddenTrackBack";
            this.menuHiddenTrackBack.ShortcutKeys = ((Keys)((Keys.Control | Keys.PageUp)));
            this.menuHiddenTrackBack.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenTrackBack.Text = "Previous Track";
            this.menuHiddenTrackBack.Visible = false;
            // 
            // menuHiddenCopy
            // 
            this.menuHiddenCopy.Name = "menuHiddenCopy";
            this.menuHiddenCopy.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenCopy.Text = "Copy";
            // 
            // menuHiddenPaste
            // 
            this.menuHiddenPaste.Name = "menuHiddenPaste";
            this.menuHiddenPaste.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenPaste.Text = "Paste";
            // 
            // menuHiddenCut
            // 
            this.menuHiddenCut.Name = "menuHiddenCut";
            this.menuHiddenCut.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenCut.Text = "Cut";
            // 
            // menuHiddenSelectForward
            // 
            this.menuHiddenSelectForward.Name = "menuHiddenSelectForward";
            this.menuHiddenSelectForward.ShortcutKeys = ((Keys)((Keys.Alt | Keys.Right)));
            this.menuHiddenSelectForward.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenSelectForward.Text = "Select Forward";
            // 
            // menuHiddenSelectBackward
            // 
            this.menuHiddenSelectBackward.Name = "menuHiddenSelectBackward";
            this.menuHiddenSelectBackward.ShortcutKeys = ((Keys)((Keys.Alt | Keys.Left)));
            this.menuHiddenSelectBackward.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenSelectBackward.Text = "Select Backward";
            // 
            // menuHiddenMoveUp
            // 
            this.menuHiddenMoveUp.Name = "menuHiddenMoveUp";
            this.menuHiddenMoveUp.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenMoveUp.Text = "Move Up";
            // 
            // menuHiddenMoveDown
            // 
            this.menuHiddenMoveDown.Name = "menuHiddenMoveDown";
            this.menuHiddenMoveDown.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenMoveDown.Text = "Move Down";
            // 
            // menuHiddenMoveLeft
            // 
            this.menuHiddenMoveLeft.Name = "menuHiddenMoveLeft";
            this.menuHiddenMoveLeft.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenMoveLeft.Text = "Move Left";
            // 
            // menuHiddenMoveRight
            // 
            this.menuHiddenMoveRight.Name = "menuHiddenMoveRight";
            this.menuHiddenMoveRight.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenMoveRight.Text = "Move Right";
            // 
            // menuHiddenLengthen
            // 
            this.menuHiddenLengthen.Name = "menuHiddenLengthen";
            this.menuHiddenLengthen.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenLengthen.Text = "Lengthen";
            // 
            // menuHiddenShorten
            // 
            this.menuHiddenShorten.Name = "menuHiddenShorten";
            this.menuHiddenShorten.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenShorten.Text = "Shorten";
            // 
            // menuHiddenGoToStartMarker
            // 
            this.menuHiddenGoToStartMarker.Name = "menuHiddenGoToStartMarker";
            this.menuHiddenGoToStartMarker.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenGoToStartMarker.Text = "GoTo Start Marker";
            // 
            // menuHiddenGoToEndMarker
            // 
            this.menuHiddenGoToEndMarker.Name = "menuHiddenGoToEndMarker";
            this.menuHiddenGoToEndMarker.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenGoToEndMarker.Text = "GoTo End Marker";
            // 
            // menuHiddenPlayFromStartMarker
            // 
            this.menuHiddenPlayFromStartMarker.Name = "menuHiddenPlayFromStartMarker";
            this.menuHiddenPlayFromStartMarker.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenPlayFromStartMarker.Text = "Play From Start Marker";
            // 
            // menuHiddenFlipCurveOnPianorollMode
            // 
            this.menuHiddenFlipCurveOnPianorollMode.Name = "menuHiddenFlipCurveOnPianorollMode";
            this.menuHiddenFlipCurveOnPianorollMode.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenFlipCurveOnPianorollMode.Text = "Change pitch drawing mode";
            // 
            // menuHiddenPrintPoToCSV
            // 
            this.menuHiddenPrintPoToCSV.Name = "menuHiddenPrintPoToCSV";
            this.menuHiddenPrintPoToCSV.Size = new Cadencii.Gui.Dimension(304, 22);
            this.menuHiddenPrintPoToCSV.Text = "Print language configs to CSV";
            // 
            // cMenuPiano
            // 
            this.cMenuPiano.Items.AddRange(new UiToolStripItem[] {
            this.cMenuPianoPointer,
            this.cMenuPianoPencil,
            this.cMenuPianoEraser,
            this.cMenuPianoPaletteTool,
            this.toolStripSeparator15,
            this.cMenuPianoCurve,
            this.toolStripMenuItem13,
            this.cMenuPianoFixed,
            this.cMenuPianoQuantize,
            this.cMenuPianoGrid,
            this.toolStripMenuItem14,
            this.cMenuPianoUndo,
            this.cMenuPianoRedo,
            this.toolStripMenuItem15,
            this.cMenuPianoCut,
            this.cMenuPianoCopy,
            this.cMenuPianoPaste,
            this.cMenuPianoDelete,
            this.toolStripMenuItem16,
            this.cMenuPianoSelectAll,
            this.cMenuPianoSelectAllEvents,
            this.toolStripMenuItem17,
            this.cMenuPianoImportLyric,
            this.cMenuPianoExpressionProperty,
            this.cMenuPianoVibratoProperty});
            this.cMenuPiano.Name = "cMenuPiano";
            this.cMenuPiano.RenderMode = ToolStripRenderMode.System;
            this.cMenuPiano.ShowCheckMargin = true;
            this.cMenuPiano.ShowImageMargin = false;
            this.cMenuPiano.Size = new Cadencii.Gui.Dimension(242, 458);
            // 
            // cMenuPianoPointer
            // 
            this.cMenuPianoPointer.Name = "cMenuPianoPointer";
            this.cMenuPianoPointer.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoPointer.Text = "Arrow(&A)";
            // 
            // cMenuPianoPencil
            // 
            this.cMenuPianoPencil.Name = "cMenuPianoPencil";
            this.cMenuPianoPencil.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoPencil.Text = "Pencil(&W)";
            // 
            // cMenuPianoEraser
            // 
            this.cMenuPianoEraser.Name = "cMenuPianoEraser";
            this.cMenuPianoEraser.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoEraser.Text = "Eraser(&E)";
            // 
            // cMenuPianoPaletteTool
            // 
            this.cMenuPianoPaletteTool.Name = "cMenuPianoPaletteTool";
            this.cMenuPianoPaletteTool.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoPaletteTool.Text = "Palette Tool";
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new Cadencii.Gui.Dimension(238, 6);
            // 
            // cMenuPianoCurve
            // 
            this.cMenuPianoCurve.Name = "cMenuPianoCurve";
            this.cMenuPianoCurve.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoCurve.Text = "Curve(&V)";
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new Cadencii.Gui.Dimension(238, 6);
            // 
            // cMenuPianoFixed
            // 
            this.cMenuPianoFixed.DropDownItems.AddRange(new UiToolStripItem[] {
            this.cMenuPianoFixed01,
            this.cMenuPianoFixed02,
            this.cMenuPianoFixed04,
            this.cMenuPianoFixed08,
            this.cMenuPianoFixed16,
            this.cMenuPianoFixed32,
            this.cMenuPianoFixed64,
            this.cMenuPianoFixed128,
            this.cMenuPianoFixedOff,
            this.toolStripMenuItem18,
            this.cMenuPianoFixedTriplet,
            this.cMenuPianoFixedDotted});
            this.cMenuPianoFixed.Name = "cMenuPianoFixed";
            this.cMenuPianoFixed.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoFixed.Text = "Note Fixed Length(&N)";
            // 
            // cMenuPianoFixed01
            // 
            this.cMenuPianoFixed01.Name = "cMenuPianoFixed01";
            this.cMenuPianoFixed01.Size = new Cadencii.Gui.Dimension(141, 22);
            this.cMenuPianoFixed01.Text = "1/ 1 [1920]";
            // 
            // cMenuPianoFixed02
            // 
            this.cMenuPianoFixed02.Name = "cMenuPianoFixed02";
            this.cMenuPianoFixed02.Size = new Cadencii.Gui.Dimension(141, 22);
            this.cMenuPianoFixed02.Text = "1/ 2 [960]";
            // 
            // cMenuPianoFixed04
            // 
            this.cMenuPianoFixed04.Name = "cMenuPianoFixed04";
            this.cMenuPianoFixed04.Size = new Cadencii.Gui.Dimension(141, 22);
            this.cMenuPianoFixed04.Text = "1/ 4 [480]";
            // 
            // cMenuPianoFixed08
            // 
            this.cMenuPianoFixed08.Name = "cMenuPianoFixed08";
            this.cMenuPianoFixed08.Size = new Cadencii.Gui.Dimension(141, 22);
            this.cMenuPianoFixed08.Text = "1/ 8 [240]";
            // 
            // cMenuPianoFixed16
            // 
            this.cMenuPianoFixed16.Name = "cMenuPianoFixed16";
            this.cMenuPianoFixed16.Size = new Cadencii.Gui.Dimension(141, 22);
            this.cMenuPianoFixed16.Text = "1/16 [120]";
            // 
            // cMenuPianoFixed32
            // 
            this.cMenuPianoFixed32.Name = "cMenuPianoFixed32";
            this.cMenuPianoFixed32.Size = new Cadencii.Gui.Dimension(141, 22);
            this.cMenuPianoFixed32.Text = "1/32 [60]";
            // 
            // cMenuPianoFixed64
            // 
            this.cMenuPianoFixed64.Name = "cMenuPianoFixed64";
            this.cMenuPianoFixed64.Size = new Cadencii.Gui.Dimension(141, 22);
            this.cMenuPianoFixed64.Text = "1/64 [30]";
            // 
            // cMenuPianoFixed128
            // 
            this.cMenuPianoFixed128.Name = "cMenuPianoFixed128";
            this.cMenuPianoFixed128.Size = new Cadencii.Gui.Dimension(141, 22);
            this.cMenuPianoFixed128.Text = "1/128[15]";
            // 
            // cMenuPianoFixedOff
            // 
            this.cMenuPianoFixedOff.Name = "cMenuPianoFixedOff";
            this.cMenuPianoFixedOff.Size = new Cadencii.Gui.Dimension(141, 22);
            this.cMenuPianoFixedOff.Text = "オフ";
            // 
            // toolStripMenuItem18
            // 
            this.toolStripMenuItem18.Name = "toolStripMenuItem18";
            this.toolStripMenuItem18.Size = new Cadencii.Gui.Dimension(138, 6);
            // 
            // cMenuPianoFixedTriplet
            // 
            this.cMenuPianoFixedTriplet.Name = "cMenuPianoFixedTriplet";
            this.cMenuPianoFixedTriplet.Size = new Cadencii.Gui.Dimension(141, 22);
            this.cMenuPianoFixedTriplet.Text = "3連符";
            // 
            // cMenuPianoFixedDotted
            // 
            this.cMenuPianoFixedDotted.Name = "cMenuPianoFixedDotted";
            this.cMenuPianoFixedDotted.Size = new Cadencii.Gui.Dimension(141, 22);
            this.cMenuPianoFixedDotted.Text = "付点";
            // 
            // cMenuPianoQuantize
            // 
            this.cMenuPianoQuantize.DropDownItems.AddRange(new UiToolStripItem[] {
            this.cMenuPianoQuantize04,
            this.cMenuPianoQuantize08,
            this.cMenuPianoQuantize16,
            this.cMenuPianoQuantize32,
            this.cMenuPianoQuantize64,
            this.cMenuPianoQuantize128,
            this.cMenuPianoQuantizeOff,
            this.toolStripMenuItem26,
            this.cMenuPianoQuantizeTriplet});
            this.cMenuPianoQuantize.Name = "cMenuPianoQuantize";
            this.cMenuPianoQuantize.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoQuantize.Text = "Quantize(&Q)";
            // 
            // cMenuPianoQuantize04
            // 
            this.cMenuPianoQuantize04.Name = "cMenuPianoQuantize04";
            this.cMenuPianoQuantize04.Size = new Cadencii.Gui.Dimension(109, 22);
            this.cMenuPianoQuantize04.Text = "1/4";
            // 
            // cMenuPianoQuantize08
            // 
            this.cMenuPianoQuantize08.Name = "cMenuPianoQuantize08";
            this.cMenuPianoQuantize08.Size = new Cadencii.Gui.Dimension(109, 22);
            this.cMenuPianoQuantize08.Text = "1/8";
            // 
            // cMenuPianoQuantize16
            // 
            this.cMenuPianoQuantize16.Name = "cMenuPianoQuantize16";
            this.cMenuPianoQuantize16.Size = new Cadencii.Gui.Dimension(109, 22);
            this.cMenuPianoQuantize16.Text = "1/16";
            // 
            // cMenuPianoQuantize32
            // 
            this.cMenuPianoQuantize32.Name = "cMenuPianoQuantize32";
            this.cMenuPianoQuantize32.Size = new Cadencii.Gui.Dimension(109, 22);
            this.cMenuPianoQuantize32.Text = "1/32";
            // 
            // cMenuPianoQuantize64
            // 
            this.cMenuPianoQuantize64.Name = "cMenuPianoQuantize64";
            this.cMenuPianoQuantize64.Size = new Cadencii.Gui.Dimension(109, 22);
            this.cMenuPianoQuantize64.Text = "1/64";
            // 
            // cMenuPianoQuantize128
            // 
            this.cMenuPianoQuantize128.Name = "cMenuPianoQuantize128";
            this.cMenuPianoQuantize128.Size = new Cadencii.Gui.Dimension(109, 22);
            this.cMenuPianoQuantize128.Text = "1/128";
            // 
            // cMenuPianoQuantizeOff
            // 
            this.cMenuPianoQuantizeOff.Name = "cMenuPianoQuantizeOff";
            this.cMenuPianoQuantizeOff.Size = new Cadencii.Gui.Dimension(109, 22);
            this.cMenuPianoQuantizeOff.Text = "オフ";
            // 
            // toolStripMenuItem26
            // 
            this.toolStripMenuItem26.Name = "toolStripMenuItem26";
            this.toolStripMenuItem26.Size = new Cadencii.Gui.Dimension(106, 6);
            // 
            // cMenuPianoQuantizeTriplet
            // 
            this.cMenuPianoQuantizeTriplet.Name = "cMenuPianoQuantizeTriplet";
            this.cMenuPianoQuantizeTriplet.Size = new Cadencii.Gui.Dimension(109, 22);
            this.cMenuPianoQuantizeTriplet.Text = "3連符";
            // 
            // cMenuPianoGrid
            // 
            this.cMenuPianoGrid.Name = "cMenuPianoGrid";
            this.cMenuPianoGrid.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoGrid.Text = "Show/Hide Grid Line(&S)";
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.Size = new Cadencii.Gui.Dimension(238, 6);
            // 
            // cMenuPianoUndo
            // 
            this.cMenuPianoUndo.Name = "cMenuPianoUndo";
            this.cMenuPianoUndo.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoUndo.Text = "Undo(&U)";
            // 
            // cMenuPianoRedo
            // 
            this.cMenuPianoRedo.Name = "cMenuPianoRedo";
            this.cMenuPianoRedo.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoRedo.Text = "Redo(&R)";
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new Cadencii.Gui.Dimension(238, 6);
            // 
            // cMenuPianoCut
            // 
            this.cMenuPianoCut.Name = "cMenuPianoCut";
            this.cMenuPianoCut.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoCut.Text = "Cut(&T)";
            // 
            // cMenuPianoCopy
            // 
            this.cMenuPianoCopy.Name = "cMenuPianoCopy";
            this.cMenuPianoCopy.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoCopy.Text = "Copy(&C)";
            // 
            // cMenuPianoPaste
            // 
            this.cMenuPianoPaste.Name = "cMenuPianoPaste";
            this.cMenuPianoPaste.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoPaste.Text = "Paste(&P)";
            // 
            // cMenuPianoDelete
            // 
            this.cMenuPianoDelete.Name = "cMenuPianoDelete";
            this.cMenuPianoDelete.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoDelete.Text = "Delete(&D)";
            // 
            // toolStripMenuItem16
            // 
            this.toolStripMenuItem16.Name = "toolStripMenuItem16";
            this.toolStripMenuItem16.Size = new Cadencii.Gui.Dimension(238, 6);
            // 
            // cMenuPianoSelectAll
            // 
            this.cMenuPianoSelectAll.Name = "cMenuPianoSelectAll";
            this.cMenuPianoSelectAll.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoSelectAll.Text = "Select All(&A)";
            // 
            // cMenuPianoSelectAllEvents
            // 
            this.cMenuPianoSelectAllEvents.Name = "cMenuPianoSelectAllEvents";
            this.cMenuPianoSelectAllEvents.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoSelectAllEvents.Text = "Select All Events(&E)";
            // 
            // toolStripMenuItem17
            // 
            this.toolStripMenuItem17.Name = "toolStripMenuItem17";
            this.toolStripMenuItem17.Size = new Cadencii.Gui.Dimension(238, 6);
            // 
            // cMenuPianoImportLyric
            // 
            this.cMenuPianoImportLyric.Name = "cMenuPianoImportLyric";
            this.cMenuPianoImportLyric.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoImportLyric.Text = "Insert Lyrics(&L)";
            // 
            // cMenuPianoExpressionProperty
            // 
            this.cMenuPianoExpressionProperty.Name = "cMenuPianoExpressionProperty";
            this.cMenuPianoExpressionProperty.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoExpressionProperty.Text = "Note Expression Property(&P)";
            // 
            // cMenuPianoVibratoProperty
            // 
            this.cMenuPianoVibratoProperty.Name = "cMenuPianoVibratoProperty";
            this.cMenuPianoVibratoProperty.Size = new Cadencii.Gui.Dimension(241, 22);
            this.cMenuPianoVibratoProperty.Text = "Note Vibrato Property";
            // 
            // cMenuTrackTab
            // 
            this.cMenuTrackTab.Items.AddRange(new UiToolStripItem[] {
            this.cMenuTrackTabTrackOn,
            this.toolStripMenuItem24,
            this.cMenuTrackTabAdd,
            this.cMenuTrackTabCopy,
            this.cMenuTrackTabChangeName,
            this.cMenuTrackTabDelete,
            this.toolStripMenuItem25,
            this.cMenuTrackTabRenderCurrent,
            this.cMenuTrackTabRenderAll,
            this.toolStripMenuItem27,
            this.cMenuTrackTabOverlay,
            this.cMenuTrackTabRenderer});
            this.cMenuTrackTab.Name = "cMenuTrackTab";
            this.cMenuTrackTab.RenderMode = ToolStripRenderMode.System;
            this.cMenuTrackTab.ShowCheckMargin = true;
            this.cMenuTrackTab.ShowImageMargin = false;
            this.cMenuTrackTab.Size = new Cadencii.Gui.Dimension(220, 220);
            // 
            // cMenuTrackTabTrackOn
            // 
            this.cMenuTrackTabTrackOn.Name = "cMenuTrackTabTrackOn";
            this.cMenuTrackTabTrackOn.Size = new Cadencii.Gui.Dimension(219, 22);
            this.cMenuTrackTabTrackOn.Text = "Track On(&K)";
            // 
            // toolStripMenuItem24
            // 
            this.toolStripMenuItem24.Name = "toolStripMenuItem24";
            this.toolStripMenuItem24.Size = new Cadencii.Gui.Dimension(216, 6);
            // 
            // cMenuTrackTabAdd
            // 
            this.cMenuTrackTabAdd.Name = "cMenuTrackTabAdd";
            this.cMenuTrackTabAdd.Size = new Cadencii.Gui.Dimension(219, 22);
            this.cMenuTrackTabAdd.Text = "Add Track(&A)";
            // 
            // cMenuTrackTabCopy
            // 
            this.cMenuTrackTabCopy.Name = "cMenuTrackTabCopy";
            this.cMenuTrackTabCopy.Size = new Cadencii.Gui.Dimension(219, 22);
            this.cMenuTrackTabCopy.Text = "Copy Track(&C)";
            // 
            // cMenuTrackTabChangeName
            // 
            this.cMenuTrackTabChangeName.Name = "cMenuTrackTabChangeName";
            this.cMenuTrackTabChangeName.Size = new Cadencii.Gui.Dimension(219, 22);
            this.cMenuTrackTabChangeName.Text = "Rename Track";
            // 
            // cMenuTrackTabDelete
            // 
            this.cMenuTrackTabDelete.Name = "cMenuTrackTabDelete";
            this.cMenuTrackTabDelete.Size = new Cadencii.Gui.Dimension(219, 22);
            this.cMenuTrackTabDelete.Text = "Delete Track(&D)";
            // 
            // toolStripMenuItem25
            // 
            this.toolStripMenuItem25.Name = "toolStripMenuItem25";
            this.toolStripMenuItem25.Size = new Cadencii.Gui.Dimension(216, 6);
            // 
            // cMenuTrackTabRenderCurrent
            // 
            this.cMenuTrackTabRenderCurrent.Name = "cMenuTrackTabRenderCurrent";
            this.cMenuTrackTabRenderCurrent.Size = new Cadencii.Gui.Dimension(219, 22);
            this.cMenuTrackTabRenderCurrent.Text = "Render Current Track(&T)";
            // 
            // cMenuTrackTabRenderAll
            // 
            this.cMenuTrackTabRenderAll.Name = "cMenuTrackTabRenderAll";
            this.cMenuTrackTabRenderAll.Size = new Cadencii.Gui.Dimension(219, 22);
            this.cMenuTrackTabRenderAll.Text = "Render All Tracks(&S)";
            // 
            // toolStripMenuItem27
            // 
            this.toolStripMenuItem27.Name = "toolStripMenuItem27";
            this.toolStripMenuItem27.Size = new Cadencii.Gui.Dimension(216, 6);
            // 
            // cMenuTrackTabOverlay
            // 
            this.cMenuTrackTabOverlay.Name = "cMenuTrackTabOverlay";
            this.cMenuTrackTabOverlay.Size = new Cadencii.Gui.Dimension(219, 22);
            this.cMenuTrackTabOverlay.Text = "Overlay(&O)";
            // 
            // cMenuTrackTabRenderer
            // 
            this.cMenuTrackTabRenderer.DropDownItems.AddRange(new UiToolStripItem[] {
            this.cMenuTrackTabRendererVOCALOID1,
            this.cMenuTrackTabRendererVOCALOID2,
            this.cMenuTrackTabRendererUtau,
            this.cMenuTrackTabRendererStraight,
            this.cMenuTrackTabRendererAquesTone,
            this.cMenuTrackTabRendererAquesTone2});
            this.cMenuTrackTabRenderer.Name = "cMenuTrackTabRenderer";
            this.cMenuTrackTabRenderer.Size = new Cadencii.Gui.Dimension(219, 22);
            this.cMenuTrackTabRenderer.Text = "Renderer(&R)";
            // 
            // cMenuTrackTabRendererVOCALOID1
            // 
            this.cMenuTrackTabRendererVOCALOID1.Name = "cMenuTrackTabRendererVOCALOID1";
            this.cMenuTrackTabRendererVOCALOID1.Size = new Cadencii.Gui.Dimension(197, 22);
            this.cMenuTrackTabRendererVOCALOID1.Text = "VOCALOID1(&1)";
            // 
            // cMenuTrackTabRendererVOCALOID2
            // 
            this.cMenuTrackTabRendererVOCALOID2.Name = "cMenuTrackTabRendererVOCALOID2";
            this.cMenuTrackTabRendererVOCALOID2.Size = new Cadencii.Gui.Dimension(197, 22);
            this.cMenuTrackTabRendererVOCALOID2.Text = "VOCALOID2(&2)";
            // 
            // cMenuTrackTabRendererUtau
            // 
            this.cMenuTrackTabRendererUtau.Name = "cMenuTrackTabRendererUtau";
            this.cMenuTrackTabRendererUtau.Size = new Cadencii.Gui.Dimension(197, 22);
            this.cMenuTrackTabRendererUtau.Text = "UTAU(&3)";
            // 
            // cMenuTrackTabRendererStraight
            // 
            this.cMenuTrackTabRendererStraight.Name = "cMenuTrackTabRendererStraight";
            this.cMenuTrackTabRendererStraight.Size = new Cadencii.Gui.Dimension(197, 22);
            this.cMenuTrackTabRendererStraight.Text = "vConnect-STAND(&4) ";
            // 
            // cMenuTrackTabRendererAquesTone
            // 
            this.cMenuTrackTabRendererAquesTone.Name = "cMenuTrackTabRendererAquesTone";
            this.cMenuTrackTabRendererAquesTone.Size = new Cadencii.Gui.Dimension(197, 22);
            this.cMenuTrackTabRendererAquesTone.Text = "AquesTone(&5)";
            // 
            // cMenuTrackTabRendererAquesTone2
            // 
            this.cMenuTrackTabRendererAquesTone2.Name = "cMenuTrackTabRendererAquesTone2";
            this.cMenuTrackTabRendererAquesTone2.Size = new Cadencii.Gui.Dimension(197, 22);
            this.cMenuTrackTabRendererAquesTone2.Text = "AquesTone2(&6)";
			this.cMenuTrackTabRendererAquesTone2.Click += (o, e) => model.TrackMenu.RunChangeRendererCommand (RendererKind.AQUES_TONE2, -1);
            // 
            // cMenuTrackSelector
            // 
            this.cMenuTrackSelector.Items.AddRange(new UiToolStripItem[] {
            this.cMenuTrackSelectorPointer,
            this.cMenuTrackSelectorPencil,
            this.cMenuTrackSelectorLine,
            this.cMenuTrackSelectorEraser,
            this.cMenuTrackSelectorPaletteTool,
            this.toolStripSeparator14,
            this.cMenuTrackSelectorCurve,
            this.toolStripMenuItem28,
            this.cMenuTrackSelectorUndo,
            this.cMenuTrackSelectorRedo,
            this.toolStripMenuItem29,
            this.cMenuTrackSelectorCut,
            this.cMenuTrackSelectorCopy,
            this.cMenuTrackSelectorPaste,
            this.cMenuTrackSelectorDelete,
            this.cMenuTrackSelectorDeleteBezier,
            this.toolStripMenuItem31,
            this.cMenuTrackSelectorSelectAll});
            this.cMenuTrackSelector.Name = "cMenuTrackSelector";
            this.cMenuTrackSelector.RenderMode = ToolStripRenderMode.System;
            this.cMenuTrackSelector.ShowCheckMargin = true;
            this.cMenuTrackSelector.ShowImageMargin = false;
            this.cMenuTrackSelector.Size = new Cadencii.Gui.Dimension(206, 336);
            // 
            // cMenuTrackSelectorPointer
            // 
            this.cMenuTrackSelectorPointer.Name = "cMenuTrackSelectorPointer";
            this.cMenuTrackSelectorPointer.Size = new Cadencii.Gui.Dimension(205, 22);
            this.cMenuTrackSelectorPointer.Text = "Arrow(&A)";
            // 
            // cMenuTrackSelectorPencil
            // 
            this.cMenuTrackSelectorPencil.Name = "cMenuTrackSelectorPencil";
            this.cMenuTrackSelectorPencil.Size = new Cadencii.Gui.Dimension(205, 22);
            this.cMenuTrackSelectorPencil.Text = "Pencil(&W)";
            // 
            // cMenuTrackSelectorLine
            // 
            this.cMenuTrackSelectorLine.Name = "cMenuTrackSelectorLine";
            this.cMenuTrackSelectorLine.Size = new Cadencii.Gui.Dimension(205, 22);
            this.cMenuTrackSelectorLine.Text = "Line(&L)";
            // 
            // cMenuTrackSelectorEraser
            // 
            this.cMenuTrackSelectorEraser.Name = "cMenuTrackSelectorEraser";
            this.cMenuTrackSelectorEraser.Size = new Cadencii.Gui.Dimension(205, 22);
            this.cMenuTrackSelectorEraser.Text = "Eraser(&E)";
            // 
            // cMenuTrackSelectorPaletteTool
            // 
            this.cMenuTrackSelectorPaletteTool.Name = "cMenuTrackSelectorPaletteTool";
            this.cMenuTrackSelectorPaletteTool.Size = new Cadencii.Gui.Dimension(205, 22);
            this.cMenuTrackSelectorPaletteTool.Text = "Palette Tool";
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new Cadencii.Gui.Dimension(202, 6);
            // 
            // cMenuTrackSelectorCurve
            // 
            this.cMenuTrackSelectorCurve.Name = "cMenuTrackSelectorCurve";
            this.cMenuTrackSelectorCurve.Size = new Cadencii.Gui.Dimension(205, 22);
            this.cMenuTrackSelectorCurve.Text = "Curve(&V)";
            // 
            // toolStripMenuItem28
            // 
            this.toolStripMenuItem28.Name = "toolStripMenuItem28";
            this.toolStripMenuItem28.Size = new Cadencii.Gui.Dimension(202, 6);
            // 
            // cMenuTrackSelectorUndo
            // 
            this.cMenuTrackSelectorUndo.Name = "cMenuTrackSelectorUndo";
            this.cMenuTrackSelectorUndo.Size = new Cadencii.Gui.Dimension(205, 22);
            this.cMenuTrackSelectorUndo.Text = "Undo(&U)";
            // 
            // cMenuTrackSelectorRedo
            // 
            this.cMenuTrackSelectorRedo.Name = "cMenuTrackSelectorRedo";
            this.cMenuTrackSelectorRedo.Size = new Cadencii.Gui.Dimension(205, 22);
            this.cMenuTrackSelectorRedo.Text = "Redo(&R)";
            // 
            // toolStripMenuItem29
            // 
            this.toolStripMenuItem29.Name = "toolStripMenuItem29";
            this.toolStripMenuItem29.Size = new Cadencii.Gui.Dimension(202, 6);
            // 
            // cMenuTrackSelectorCut
            // 
            this.cMenuTrackSelectorCut.Name = "cMenuTrackSelectorCut";
            this.cMenuTrackSelectorCut.Size = new Cadencii.Gui.Dimension(205, 22);
            this.cMenuTrackSelectorCut.Text = "Cut(&T)";
            // 
            // cMenuTrackSelectorCopy
            // 
            this.cMenuTrackSelectorCopy.Name = "cMenuTrackSelectorCopy";
            this.cMenuTrackSelectorCopy.Size = new Cadencii.Gui.Dimension(205, 22);
            this.cMenuTrackSelectorCopy.Text = "Copy(&C)";
            // 
            // cMenuTrackSelectorPaste
            // 
            this.cMenuTrackSelectorPaste.Name = "cMenuTrackSelectorPaste";
            this.cMenuTrackSelectorPaste.Size = new Cadencii.Gui.Dimension(205, 22);
            this.cMenuTrackSelectorPaste.Text = "Paste(&P)";
            // 
            // cMenuTrackSelectorDelete
            // 
            this.cMenuTrackSelectorDelete.Name = "cMenuTrackSelectorDelete";
            this.cMenuTrackSelectorDelete.Size = new Cadencii.Gui.Dimension(205, 22);
            this.cMenuTrackSelectorDelete.Text = "Delete(&D)";
            // 
            // cMenuTrackSelectorDeleteBezier
            // 
            this.cMenuTrackSelectorDeleteBezier.Name = "cMenuTrackSelectorDeleteBezier";
            this.cMenuTrackSelectorDeleteBezier.Size = new Cadencii.Gui.Dimension(205, 22);
            this.cMenuTrackSelectorDeleteBezier.Text = "Delete Bezier Point(&B)";
            // 
            // toolStripMenuItem31
            // 
            this.toolStripMenuItem31.Name = "toolStripMenuItem31";
            this.toolStripMenuItem31.Size = new Cadencii.Gui.Dimension(202, 6);
            // 
            // cMenuTrackSelectorSelectAll
            // 
            this.cMenuTrackSelectorSelectAll.Name = "cMenuTrackSelectorSelectAll";
            this.cMenuTrackSelectorSelectAll.Size = new Cadencii.Gui.Dimension(205, 22);
            this.cMenuTrackSelectorSelectAll.Text = "Select All Events(&E)";
            // 
            // trackBar
            // 
            this.trackBar.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.trackBar.AutoSize = false;
            this.trackBar.Location = new Cadencii.Gui.Point(322, 263);
            this.trackBar.Margin = new  Cadencii.Gui.Padding(0);
            this.trackBar.Maximum = 609;
            this.trackBar.Minimum = 17;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new Cadencii.Gui.Dimension(83, 16);
            this.trackBar.TabIndex = 15;
            this.trackBar.TabStop = false;
            this.trackBar.TickFrequency = 100;
            this.trackBar.TickStyle = TickStyle.None;
            this.trackBar.Value = 17;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            this.pictureBox3.BackColor = new Color (((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pictureBox3.Location = new Cadencii.Gui.Point (0, 263);
            this.pictureBox3.Margin = new Cadencii.Gui.Padding(0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new Cadencii.Gui.Dimension(49, 16);
            this.pictureBox3.TabIndex = 8;
            this.pictureBox3.TabStop = false;
            // 
            // pictKeyLengthSplitter
            // 
            this.pictKeyLengthSplitter.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            this.pictKeyLengthSplitter.BackColor = System.Drawing.SystemColors.Control.ToAwt ();
            this.pictKeyLengthSplitter.BorderStyle = BorderStyle.Fixed3D;
			this.pictKeyLengthSplitter.Cursor = System.Windows.Forms.Cursors.NoMoveHoriz.ToAwt ();
            this.pictKeyLengthSplitter.Location = new  Cadencii.Gui.Point(49, 263);
            this.pictKeyLengthSplitter.Margin = new Cadencii.Gui.Padding(0);
            this.pictKeyLengthSplitter.Name = "pictKeyLengthSplitter";
            this.pictKeyLengthSplitter.Size = new Cadencii.Gui.Dimension(16, 16);
            this.pictKeyLengthSplitter.TabIndex = 20;
            this.pictKeyLengthSplitter.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.pictureBox2.BackColor = new Color(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pictureBox2.Location = new Cadencii.Gui.Point(405, 231);
            this.pictureBox2.Margin = new Cadencii.Gui.Padding(0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new Cadencii.Gui.Dimension(16, 48);
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // vScroll
            // 
            this.vScroll.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Right)));
            this.vScroll.Location = new Cadencii.Gui.Point(405, 94);
            this.vScroll.Name = "vScroll";
            this.vScroll.Size = new Cadencii.Gui.Dimension(16, 137);
            this.vScroll.TabIndex = 17;
            // 
            // picturePositionIndicator
            // 
            this.picturePositionIndicator.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.picturePositionIndicator.BackColor = Cadencii.Gui.Colors.DarkGray;
            this.picturePositionIndicator.Location = new Cadencii.Gui.Point(0, 46);
            this.picturePositionIndicator.Margin = new Cadencii.Gui.Padding(0);
            this.picturePositionIndicator.Name = "picturePositionIndicator";
            this.picturePositionIndicator.Size = new Cadencii.Gui.Dimension(700, 48);
            this.picturePositionIndicator.TabIndex = 10;
            this.picturePositionIndicator.TabStop = false;
            // 
            // toolStripBottom
            // 
            this.toolStripBottom.Dock = DockStyle.None;
            this.toolStripBottom.Items.AddRange(new UiToolStripItem[] {
            this.toolStripStatusLabel1,
            this.stripLblGameCtrlMode,
            this.toolStripSeparator10,
            this.toolStripStatusLabel2,
            this.stripLblMidiIn,
            this.toolStripSeparator11,
            this.stripBtnStepSequencer});
			this.toolStripBottom.Location = new Cadencii.Gui.Point(15, 0);
            this.toolStripBottom.Name = "toolStripBottom";
            this.toolStripBottom.RenderMode = ToolStripRenderMode.System;
            this.toolStripBottom.Size = new Cadencii.Gui.Dimension(347, 25);
            this.toolStripBottom.TabIndex = 22;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new Cadencii.Gui.Dimension(101, 20);
            this.toolStripStatusLabel1.Text = "Game Controler";
            // 
            // stripLblGameCtrlMode
            // 
            this.stripLblGameCtrlMode.Name = "stripLblGameCtrlMode";
            this.stripLblGameCtrlMode.Size = new Cadencii.Gui.Dimension(57, 20);
            this.stripLblGameCtrlMode.Text = "Disabled";
            this.stripLblGameCtrlMode.ToolTipText = "Game Controler";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new Cadencii.Gui.Dimension(6, 25);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new Cadencii.Gui.Dimension(53, 20);
            this.toolStripStatusLabel2.Text = "MIDI In";
            // 
            // stripLblMidiIn
            // 
            this.stripLblMidiIn.Name = "stripLblMidiIn";
            this.stripLblMidiIn.Size = new Cadencii.Gui.Dimension(57, 20);
            this.stripLblMidiIn.Text = "Disabled";
            this.stripLblMidiIn.ToolTipText = "Midi In Device";
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new Cadencii.Gui.Dimension(6, 25);
            // 
            // stripBtnStepSequencer
            // 
            this.stripBtnStepSequencer.CheckOnClick = true;
			this.stripBtnStepSequencer.Image = ((System.Drawing.Image)(resources.GetObject("stripBtnStepSequencer.Image"))).ToAwt ();
			this.stripBtnStepSequencer.ImageTransparentColor = Cadencii.Gui.Colors.Magenta;
            this.stripBtnStepSequencer.Name = "stripBtnStepSequencer";
            this.stripBtnStepSequencer.Size = new Cadencii.Gui.Dimension(55, 22);
            this.stripBtnStepSequencer.Text = "Step";
            // 
            // splitContainerProperty
            // 
            this.splitContainerProperty.AddControl(this.splitContainer2);
            this.splitContainerProperty.FixedPanel = Cadencii.Gui.FixedPanel.None;
            this.splitContainerProperty.SplitterFixed = false;
            this.splitContainerProperty.Location = new Cadencii.Gui.Point(448, 14);
            this.splitContainerProperty.Margin = new Cadencii.Gui.Padding(0);
            this.splitContainerProperty.Name = "splitContainerProperty";
            this.splitContainerProperty.Orientation = Cadencii.Gui.Orientation.Horizontal;
            // 
            // 
            // 
            this.splitContainerProperty.Panel1.Anchor = Cadencii.Gui.AnchorStyles.Top | Cadencii.Gui.AnchorStyles.Bottom | Cadencii.Gui.AnchorStyles.Left | Cadencii.Gui.AnchorStyles.Right;
            this.splitContainerProperty.Panel1.BorderColor = Cadencii.Gui.Colors.Black;
            this.splitContainerProperty.Panel1.Location = new Cadencii.Gui.Point(0, 0);
            this.splitContainerProperty.Panel1.Margin = new Cadencii.Gui.Padding(0, 0, 0, 4);
            this.splitContainerProperty.Panel1.Name = "m_panel1";
            this.splitContainerProperty.Panel1.Size = new Cadencii.Gui.Dimension(42, 348);
            this.splitContainerProperty.Panel1.TabIndex = 0;
            this.splitContainerProperty.Panel1MinSize = 25;
            // 
            // 
            // 
			this.splitContainerProperty.Panel2.Anchor = Cadencii.Gui.AnchorStyles.Top | Cadencii.Gui.AnchorStyles.Bottom | Cadencii.Gui.AnchorStyles.Left | Cadencii.Gui.AnchorStyles.Right;
            this.splitContainerProperty.Panel2.BorderColor = Cadencii.Gui.Colors.Black;
            this.splitContainerProperty.Panel2.Location = new Cadencii.Gui.Point(46, 0);
            this.splitContainerProperty.Panel2.Margin = new Cadencii.Gui.Padding(0);
            this.splitContainerProperty.Panel2.Name = "m_panel2";
            this.splitContainerProperty.Panel2.Size = new Cadencii.Gui.Dimension(66, 348);
            this.splitContainerProperty.Panel2.TabIndex = 1;
            this.splitContainerProperty.Panel2MinSize = 25;
            this.splitContainerProperty.Size = new Cadencii.Gui.Dimension(112, 348);
            this.splitContainerProperty.SplitterDistance = 42;
            this.splitContainerProperty.SplitterWidth = 4;
            this.splitContainerProperty.TabIndex = 20;
            this.splitContainerProperty.TabStop = false;
            this.splitContainerProperty.Text = "bSplitContainer1";
            // 
            // splitContainer2
            // 
            this.splitContainer2.FixedPanel = Cadencii.Gui.FixedPanel.Panel2;
            this.splitContainer2.SplitterFixed = false;
            this.splitContainer2.Location = new Cadencii.Gui.Point(0, 345);
            this.splitContainer2.Margin = new Cadencii.Gui.Padding(0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = Cadencii.Gui.Orientation.Vertical;
            // 
            // 
            // 
			this.splitContainer2.Panel2.Anchor = Cadencii.Gui.AnchorStyles.Top | Cadencii.Gui.AnchorStyles.Bottom | Cadencii.Gui.AnchorStyles.Left | Cadencii.Gui.AnchorStyles.Right;
            this.splitContainer2.Panel1.BorderColor = Cadencii.Gui.Colors.Black;
            this.splitContainer2.Panel1.Location = new Cadencii.Gui.Point(0, 0);
            this.splitContainer2.Panel1.Margin = new Cadencii.Gui.Padding(0);
            this.splitContainer2.Panel1.Name = "m_panel1";
            this.splitContainer2.Panel1.Size = new Cadencii.Gui.Dimension(115, 25);
            this.splitContainer2.Panel1.TabIndex = 0;
            this.splitContainer2.Panel1MinSize = 25;
            // 
            // 
            // 
			this.splitContainer2.Panel2.Anchor = Cadencii.Gui.AnchorStyles.Bottom | Cadencii.Gui.AnchorStyles.Left | Cadencii.Gui.AnchorStyles.Right;
            this.splitContainer2.Panel2.BorderColor = Cadencii.Gui.Colors.Black;
            this.splitContainer2.Panel2.Location = new Cadencii.Gui.Point(0, 29);
            this.splitContainer2.Panel2.Margin = new Cadencii.Gui.Padding(0);
            this.splitContainer2.Panel2.Name = "m_panel2";
            this.splitContainer2.Panel2.Size = new Cadencii.Gui.Dimension(115, 105);
            this.splitContainer2.Panel2.TabIndex = 1;
            this.splitContainer2.Panel2MinSize = 25;
            this.splitContainer2.Size = new Cadencii.Gui.Dimension(115, 134);
            this.splitContainer2.SplitterDistance = 25;
            this.splitContainer2.SplitterWidth = 4;
            this.splitContainer2.TabIndex = 23;
            this.splitContainer2.TabStop = false;
            this.splitContainer2.Text = "bSplitContainer1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.FixedPanel = Cadencii.Gui.FixedPanel.Panel2;
            this.splitContainer1.SplitterFixed = false;
            this.splitContainer1.Location = new Cadencii.Gui.Point(2, 2);
            this.splitContainer1.Margin = new Cadencii.Gui.Padding(0);
			this.splitContainer1.MinimumSize = new Cadencii.Gui.Dimension(0, 54);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = Cadencii.Gui.Orientation.Vertical;
            // 
            // 
            // 
			this.splitContainer1.Panel1.Anchor = Cadencii.Gui.AnchorStyles.Top | Cadencii.Gui.AnchorStyles.Bottom | Cadencii.Gui.AnchorStyles.Left | Cadencii.Gui.AnchorStyles.Right;
            this.splitContainer1.Panel1.BorderColor = Cadencii.Gui.Colors.Black;
            this.splitContainer1.Panel1.Location = new Cadencii.Gui.Point(0, 0);
            this.splitContainer1.Panel1.Margin = new Cadencii.Gui.Padding(0);
            this.splitContainer1.Panel1.Name = "m_panel1";
            this.splitContainer1.Panel1.Size = new Cadencii.Gui.Dimension(953, 50);
            this.splitContainer1.Panel1.TabIndex = 0;
            this.splitContainer1.Panel1MinSize = 25;
            // 
            // 
            // 
			this.splitContainer1.Panel2.Anchor = Cadencii.Gui.AnchorStyles.Bottom | Cadencii.Gui.AnchorStyles.Left | Cadencii.Gui.AnchorStyles.Right;
            this.splitContainer1.Panel2.BorderColor = Cadencii.Gui.Colors.Black;
            this.splitContainer1.Panel2.Location = new Cadencii.Gui.Point(0, 54);
            this.splitContainer1.Panel2.Margin = new Cadencii.Gui.Padding(0);
            this.splitContainer1.Panel2.Name = "m_panel2";
            this.splitContainer1.Panel2.Size = new Cadencii.Gui.Dimension(953, 25);
            this.splitContainer1.Panel2.TabIndex = 1;
            this.splitContainer1.Panel2MinSize = 25;
            this.splitContainer1.Size = new Cadencii.Gui.Dimension(953, 54);
            this.splitContainer1.SplitterDistance = 50;
            this.splitContainer1.SplitterWidth = 4;
            this.splitContainer1.TabIndex = 4;
            this.splitContainer1.TabStop = false;
            this.splitContainer1.Text = "splitContainerEx1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Cadencii.Gui.Dimension(57, 6);
            // 
            // stripDDBtnQuantize
            // 
            this.stripDDBtnQuantize.MenuItems.AddRange(new UiMenuItem[] {
            this.stripDDBtnQuantize04,
            this.stripDDBtnQuantize08,
            this.stripDDBtnQuantize16,
            this.stripDDBtnQuantize32,
            this.stripDDBtnQuantize64,
            this.stripDDBtnQuantize128,
            this.stripDDBtnQuantizeOff,
            this.menuItem2,
            this.stripDDBtnQuantizeTriplet});
            // 
            // stripDDBtnQuantize04
            // 
            this.stripDDBtnQuantize04.Index = 0;
            this.stripDDBtnQuantize04.Text = "1/4";
            // 
            // stripDDBtnQuantize08
            // 
            this.stripDDBtnQuantize08.Index = 1;
            this.stripDDBtnQuantize08.Text = "1/8";
            // 
            // stripDDBtnQuantize16
            // 
            this.stripDDBtnQuantize16.Index = 2;
            this.stripDDBtnQuantize16.Text = "1/16";
            // 
            // stripDDBtnQuantize32
            // 
            this.stripDDBtnQuantize32.Index = 3;
            this.stripDDBtnQuantize32.Text = "1/32";
            // 
            // stripDDBtnQuantize64
            // 
            this.stripDDBtnQuantize64.Index = 4;
            this.stripDDBtnQuantize64.Text = "1/64";
            // 
            // stripDDBtnQuantize128
            // 
            this.stripDDBtnQuantize128.Index = 5;
            this.stripDDBtnQuantize128.Text = "1/128";
            // 
            // stripDDBtnQuantizeOff
            // 
            this.stripDDBtnQuantizeOff.Index = 6;
            this.stripDDBtnQuantizeOff.Text = "Off";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 7;
            this.menuItem2.Text = "-";
            // 
            // stripDDBtnQuantizeTriplet
            // 
            this.stripDDBtnQuantizeTriplet.Index = 8;
            this.stripDDBtnQuantizeTriplet.Text = "Triplet";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Cadencii.Gui.Dimension(57, 6);
            // 
            // imageListFile
            // 
            this.imageListFile.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListFile.ImageStream")));
            this.imageListFile.TransparentColor = Cadencii.Gui.Colors.Transparent;
			this.imageListFile.SetImagesKeyName(0, "disk__plus.png");
            this.imageListFile.SetImagesKeyName(1, "folder_horizontal_open.png");
            this.imageListFile.SetImagesKeyName(2, "disk.png");
            this.imageListFile.SetImagesKeyName(3, "scissors.png");
            this.imageListFile.SetImagesKeyName(4, "documents.png");
            this.imageListFile.SetImagesKeyName(5, "clipboard_paste.png");
            this.imageListFile.SetImagesKeyName(6, "arrow_skip_180.png");
            this.imageListFile.SetImagesKeyName(7, "arrow_skip.png");
            // 
            // imageListPosition
            // 
            this.imageListPosition.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListPosition.ImageStream")));
            this.imageListPosition.TransparentColor = Cadencii.Gui.Colors.Transparent;
            this.imageListPosition.SetImagesKeyName(0, "control_stop_180.png");
            this.imageListPosition.SetImagesKeyName(1, "control_double_180.png");
            this.imageListPosition.SetImagesKeyName(2, "control_double.png");
            this.imageListPosition.SetImagesKeyName(3, "control_stop.png");
            this.imageListPosition.SetImagesKeyName(4, "control.png");
            this.imageListPosition.SetImagesKeyName(5, "control_pause.png");
            this.imageListPosition.SetImagesKeyName(6, "arrow_circle_double.png");
            this.imageListPosition.SetImagesKeyName(7, "arrow_return.png");
            // 
            // imageListMeasure
            // 
            this.imageListMeasure.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMeasure.ImageStream")));
			this.imageListMeasure.TransparentColor = Cadencii.Gui.Colors.Transparent;
            this.imageListMeasure.SetImagesKeyName(0, "pin__arrow.png");
            this.imageListMeasure.SetImagesKeyName(1, "pin__arrow_inv.png");
            this.imageListMeasure.SetImagesKeyName(2, "note001.png");
            this.imageListMeasure.SetImagesKeyName(3, "note002.png");
            this.imageListMeasure.SetImagesKeyName(4, "note004.png");
            this.imageListMeasure.SetImagesKeyName(5, "note008.png");
            this.imageListMeasure.SetImagesKeyName(6, "note016.png");
            this.imageListMeasure.SetImagesKeyName(7, "note032.png");
            this.imageListMeasure.SetImagesKeyName(8, "note064.png");
            this.imageListMeasure.SetImagesKeyName(9, "note128.png");
            this.imageListMeasure.SetImagesKeyName(10, "notenull.png");
            // 
            // imageListTool
            // 
            this.imageListTool.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTool.ImageStream")));
			this.imageListTool.TransparentColor = Cadencii.Gui.Colors.Transparent;
            this.imageListTool.SetImagesKeyName(0, "arrow_135.png");
            this.imageListTool.SetImagesKeyName(1, "pencil.png");
            this.imageListTool.SetImagesKeyName(2, "layer_shape_line.png");
            this.imageListTool.SetImagesKeyName(3, "eraser.png");
            this.imageListTool.SetImagesKeyName(4, "ruler_crop.png");
            this.imageListTool.SetImagesKeyName(5, "layer_shape_curve.png");
            // 
            // panel1
            // 
            this.panel1.AddControl(this.pictKeyLengthSplitter);
			this.panel1.AddControl(this.panelOverview);
			this.panel1.AddControl(this.picturePositionIndicator);
			this.panel1.AddControl(this.pictPianoRoll);
			this.panel1.AddControl(this.vScroll);
			this.panel1.AddControl(this.pictureBox3);
			this.panel1.AddControl(this.pictureBox2);
			this.panel1.AddControl(this.hScroll);
			this.panel1.AddControl(this.trackBar);
            this.panel1.Location = new Cadencii.Gui.Point(15, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Cadencii.Gui.Dimension(421, 279);
            this.panel1.TabIndex = 24;
            // 
            // panelOverview
            // 
	this.panelOverview.Anchor = ((Cadencii.Gui.AnchorStyles)(((Cadencii.Gui.AnchorStyles.Top | Cadencii.Gui.AnchorStyles.Left) | Cadencii.Gui.AnchorStyles.Right)));
            this.panelOverview.BackColor = new Cadencii.Gui.Color(((int)(((byte)(106)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.panelOverview.Location = new Cadencii.Gui.Point(0, 1);
            this.panelOverview.Margin = new Cadencii.Gui.Padding(0);
            this.panelOverview.Name = "panelOverview";
            this.panelOverview.Size = new Cadencii.Gui.Dimension(700, 45);
            this.panelOverview.TabIndex = 19;
            this.panelOverview.TabStop = false;
            // 
            // pictPianoRoll
            // 
			this.pictPianoRoll.Anchor = ((Cadencii.Gui.AnchorStyles)((((Cadencii.Gui.AnchorStyles.Top | Cadencii.Gui.AnchorStyles.Bottom)
				| Cadencii.Gui.AnchorStyles.Left)
				| Cadencii.Gui.AnchorStyles.Right)));
			this.pictPianoRoll.BackColor = new Cadencii.Gui.Color(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.pictPianoRoll.Location = new Cadencii.Gui.Point(0, 94);
			this.pictPianoRoll.Margin = new Cadencii.Gui.Padding(0);
            this.pictPianoRoll.Name = "pictPianoRoll";
			this.pictPianoRoll.Size = new Cadencii.Gui.Dimension(405, 169);
            this.pictPianoRoll.TabIndex = 12;
            this.pictPianoRoll.TabStop = false;
            // 
            // hScroll
            // 
            this.hScroll.Anchor = ((Cadencii.Gui.AnchorStyles)(((AnchorStyles.Bottom | AnchorStyles.Left) | AnchorStyles.Right)));
            this.hScroll.Location = new Cadencii.Gui.Point(65, 263);
            this.hScroll.Name = "hScroll";
            this.hScroll.Size = new Cadencii.Gui.Dimension(257, 16);
            this.hScroll.TabIndex = 16;
            // 
            // rebar
            // 
            this.rebar.Dock = Cadencii.Gui.DockStyle.Top;
            this.rebar.Location = new Cadencii.Gui.Point(0, 26);
            this.rebar.Name = "rebar";
            this.rebar.Size = new Cadencii.Gui.Dimension(955, 4);
            this.rebar.TabIndex = 19;
            this.rebar.ToggleDoubleClick = true;
            // 
            // imageListMenu
            // 
			this.imageListMenu.ColorDepth = Cadencii.Gui.ColorDepth.Depth8Bit;
			this.imageListMenu.ImageSize = new Cadencii.Gui.Dimension(1, 16);
            this.imageListMenu.TransparentColor = Cadencii.Gui.Colors.Transparent;
            // 
            // toolBarFile
            // 
			this.toolBarFile.Appearance = Cadencii.Gui.ToolBarAppearance.Flat;
            this.toolBarFile.Buttons.AddRange(new UiToolBarButton[] {
            this.stripBtnFileNew,
            this.stripBtnFileOpen,
            this.stripBtnFileSave,
            this.toolBarButton1,
            this.stripBtnCut,
            this.stripBtnCopy,
            this.stripBtnPaste,
            this.toolBarButton2,
            this.stripBtnUndo,
            this.stripBtnRedo});
			this.toolBarFile.ButtonSize = new Cadencii.Gui.Dimension(23, 22);
            this.toolBarFile.Divider = false;
            this.toolBarFile.Dock = DockStyle.None;
            this.toolBarFile.DropDownArrows = true;
            this.toolBarFile.ImageList = this.imageListFile;
			this.toolBarFile.Location = new Cadencii.Gui.Point(11, 2);
            this.toolBarFile.Name = "toolBarFile";
            this.toolBarFile.ShowToolTips = true;
            this.toolBarFile.Size = new Cadencii.Gui.Dimension(944, 26);
            this.toolBarFile.TabIndex = 25;
            this.toolBarFile.Wrappable = false;
            // 
            // stripBtnFileNew
            // 
            this.stripBtnFileNew.ImageIndex = 0;
            this.stripBtnFileNew.Name = "stripBtnFileNew";
            this.stripBtnFileNew.ToolTipText = "New";
            // 
            // stripBtnFileOpen
            // 
            this.stripBtnFileOpen.ImageIndex = 1;
            this.stripBtnFileOpen.Name = "stripBtnFileOpen";
            this.stripBtnFileOpen.ToolTipText = "Open";
            // 
            // stripBtnFileSave
            // 
            this.stripBtnFileSave.ImageIndex = 2;
            this.stripBtnFileSave.Name = "stripBtnFileSave";
            this.stripBtnFileSave.ToolTipText = "Save";
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.Style = ToolBarButtonStyle.Separator;
            // 
            // stripBtnCut
            // 
            this.stripBtnCut.ImageIndex = 3;
            this.stripBtnCut.Name = "stripBtnCut";
            this.stripBtnCut.ToolTipText = "Cut";
            // 
            // stripBtnCopy
            // 
            this.stripBtnCopy.ImageIndex = 4;
            this.stripBtnCopy.Name = "stripBtnCopy";
            this.stripBtnCopy.ToolTipText = "Copy";
            // 
            // stripBtnPaste
            // 
            this.stripBtnPaste.ImageIndex = 5;
            this.stripBtnPaste.Name = "stripBtnPaste";
            this.stripBtnPaste.ToolTipText = "Paste";
            // 
            // toolBarButton2
            // 
            this.toolBarButton2.Name = "toolBarButton2";
            this.toolBarButton2.Style = ToolBarButtonStyle.Separator;
            // 
            // stripBtnUndo
            // 
            this.stripBtnUndo.ImageIndex = 6;
            this.stripBtnUndo.Name = "stripBtnUndo";
            this.stripBtnUndo.ToolTipText = "Undo";
            // 
            // stripBtnRedo
            // 
            this.stripBtnRedo.ImageIndex = 7;
            this.stripBtnRedo.Name = "stripBtnRedo";
            this.stripBtnRedo.ToolTipText = "Redo";
            // 
            // toolBarPosition
            // 
            this.toolBarPosition.Appearance = ToolBarAppearance.Flat;
            this.toolBarPosition.Buttons.AddRange(new UiToolBarButton[] {
            this.stripBtnMoveTop,
            this.stripBtnRewind,
            this.stripBtnForward,
            this.stripBtnMoveEnd,
            this.stripBtnPlay,
            this.toolBarButton4,
            this.stripBtnScroll,
            this.stripBtnLoop});
            this.toolBarPosition.Divider = false;
            this.toolBarPosition.Dock = DockStyle.None;
            this.toolBarPosition.DropDownArrows = true;
            this.toolBarPosition.ImageList = this.imageListPosition;
			this.toolBarPosition.Location = new Cadencii.Gui.Point(11, 32);
            this.toolBarPosition.Name = "toolBarPosition";
            this.toolBarPosition.ShowToolTips = true;
            this.toolBarPosition.Size = new Cadencii.Gui.Dimension(944, 40);
            this.toolBarPosition.TabIndex = 25;
            this.toolBarPosition.TextAlign = ToolBarTextAlign.Right;
            this.toolBarPosition.Wrappable = false;
            // 
            // stripBtnMoveTop
            // 
            this.stripBtnMoveTop.ImageIndex = 0;
            this.stripBtnMoveTop.Name = "stripBtnMoveTop";
            this.stripBtnMoveTop.ToolTipText = "MoveTop";
            // 
            // stripBtnRewind
            // 
            this.stripBtnRewind.ImageIndex = 1;
            this.stripBtnRewind.Name = "stripBtnRewind";
            this.stripBtnRewind.ToolTipText = "Rewind";
            // 
            // stripBtnForward
            // 
            this.stripBtnForward.ImageIndex = 2;
            this.stripBtnForward.Name = "stripBtnForward";
            this.stripBtnForward.ToolTipText = "Forward";
            // 
            // stripBtnMoveEnd
            // 
            this.stripBtnMoveEnd.ImageIndex = 3;
            this.stripBtnMoveEnd.Name = "stripBtnMoveEnd";
            this.stripBtnMoveEnd.ToolTipText = "MoveEnd";
            // 
            // stripBtnPlay
            // 
            this.stripBtnPlay.ImageIndex = 4;
            this.stripBtnPlay.Name = "stripBtnPlay";
            this.stripBtnPlay.ToolTipText = "Play";
            // 
            // toolBarButton4
            // 
            this.toolBarButton4.Name = "toolBarButton4";
            this.toolBarButton4.Style = ToolBarButtonStyle.Separator;
            // 
            // stripBtnScroll
            // 
            this.stripBtnScroll.ImageIndex = 6;
            this.stripBtnScroll.Name = "stripBtnScroll";
            // 
            // stripBtnLoop
            // 
            this.stripBtnLoop.ImageIndex = 7;
            this.stripBtnLoop.Name = "stripBtnLoop";
            // 
            // toolBarMeasure
            // 
            this.toolBarMeasure.Appearance = ToolBarAppearance.Flat;
            this.toolBarMeasure.Buttons.AddRange(new UiToolBarButton[] {
            this.stripDDBtnQuantizeParent,
            this.toolBarButton5,
            this.stripBtnStartMarker,
            this.stripBtnEndMarker});
            this.toolBarMeasure.Divider = false;
            this.toolBarMeasure.Dock = DockStyle.None;
            this.toolBarMeasure.DropDownArrows = true;
            this.toolBarMeasure.ImageList = this.imageListMeasure;
			this.toolBarMeasure.Location = new Cadencii.Gui.Point(11, 62);
            this.toolBarMeasure.Name = "toolBarMeasure";
            this.toolBarMeasure.ShowToolTips = true;
            this.toolBarMeasure.Size = new Cadencii.Gui.Dimension(944, 40);
            this.toolBarMeasure.TabIndex = 25;
            this.toolBarMeasure.TextAlign = ToolBarTextAlign.Right;
            this.toolBarMeasure.Wrappable = false;
            // 
            // stripDDBtnQuantizeParent
            // 
            this.stripDDBtnQuantizeParent.Name = "stripDDBtnQuantizeParent";
            // 
            // toolBarButton5
            // 
            this.toolBarButton5.Name = "toolBarButton5";
            this.toolBarButton5.Style = ToolBarButtonStyle.Separator;
            // 
            // stripBtnStartMarker
            // 
            this.stripBtnStartMarker.ImageIndex = 0;
            this.stripBtnStartMarker.Name = "stripBtnStartMarker";
            this.stripBtnStartMarker.Style = ToolBarButtonStyle.ToggleButton;
            // 
            // stripBtnEndMarker
            // 
            this.stripBtnEndMarker.ImageIndex = 1;
            this.stripBtnEndMarker.Name = "stripBtnEndMarker";
            this.stripBtnEndMarker.Style = ToolBarButtonStyle.ToggleButton;
            // 
            // toolBarTool
            // 
            this.toolBarTool.Appearance = ToolBarAppearance.Flat;
            this.toolBarTool.Buttons.AddRange(new UiToolBarButton[] {
            this.stripBtnPointer,
            this.stripBtnPencil,
            this.stripBtnLine,
            this.stripBtnEraser,
            this.toolBarButton3,
            this.stripBtnGrid,
            this.stripBtnCurve});
            this.toolBarTool.Divider = false;
            this.toolBarTool.Dock = DockStyle.None;
            this.toolBarTool.DropDownArrows = true;
            this.toolBarTool.ImageList = this.imageListTool;
			this.toolBarTool.Location = new Cadencii.Gui.Point(11, 92);
            this.toolBarTool.Name = "toolBarTool";
            this.toolBarTool.ShowToolTips = true;
            this.toolBarTool.Size = new Cadencii.Gui.Dimension(944, 40);
            this.toolBarTool.TabIndex = 25;
            this.toolBarTool.TextAlign = ToolBarTextAlign.Right;
            this.toolBarTool.Wrappable = false;
            // 
            // stripBtnPointer
            // 
            this.stripBtnPointer.ImageIndex = 0;
            this.stripBtnPointer.Name = "stripBtnPointer";
            this.stripBtnPointer.Pushed = true;
            this.stripBtnPointer.ToolTipText = "Pointer";
            // 
            // stripBtnPencil
            // 
            this.stripBtnPencil.ImageIndex = 1;
            this.stripBtnPencil.Name = "stripBtnPencil";
            this.stripBtnPencil.ToolTipText = "Pencil";
            // 
            // stripBtnLine
            // 
            this.stripBtnLine.ImageIndex = 2;
            this.stripBtnLine.Name = "stripBtnLine";
            this.stripBtnLine.ToolTipText = "Line";
            // 
            // stripBtnEraser
            // 
            this.stripBtnEraser.ImageIndex = 3;
            this.stripBtnEraser.Name = "stripBtnEraser";
            this.stripBtnEraser.ToolTipText = "Eraser";
            // 
            // toolBarButton3
            // 
            this.toolBarButton3.Name = "toolBarButton3";
            this.toolBarButton3.Style = ToolBarButtonStyle.Separator;
            // 
            // stripBtnGrid
            // 
            this.stripBtnGrid.ImageIndex = 4;
            this.stripBtnGrid.Name = "stripBtnGrid";
            this.stripBtnGrid.ToolTipText = "Grid";
            // 
            // stripBtnCurve
            // 
            this.stripBtnCurve.ImageIndex = 5;
            this.stripBtnCurve.Name = "stripBtnCurve";
            this.stripBtnCurve.ToolTipText = "Curve";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel_Controls.Add(this.toolStripBottom);
            this.toolStripContainer1.BottomToolStripPanel_Controls.Add(this.statusStrip);
            this.toolStripContainer1.BottomToolStripPanel_RenderMode = ToolStripRenderMode.System;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel_Controls.Add(this.panel1);
            this.toolStripContainer1.ContentPanel_Controls.Add(this.splitContainerProperty);
            this.toolStripContainer1.ContentPanel_Size = new Cadencii.Gui.Dimension(955, 612);
            this.toolStripContainer1.Dock = DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
			this.toolStripContainer1.Location = new Cadencii.Gui.Point(0, 30);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new Cadencii.Gui.Dimension(955, 659);
            this.toolStripContainer1.TabIndex = 26;
            this.toolStripContainer1.Text = "toolStripContainer1";
            this.toolStripContainer1.TopToolStripPanelVisible = false;
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = DockStyle.None;
            this.statusStrip.Items.AddRange(new UiToolStripItem[] {
            this.statusLabel});
			this.statusStrip.Location = new Cadencii.Gui.Point(0, 25);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new Cadencii.Gui.Dimension(955, 22);
            this.statusStrip.TabIndex = 25;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new Cadencii.Gui.Dimension(0, 17);
            // 
            // cMenuPositionIndicator
            // 
            this.cMenuPositionIndicator.Items.AddRange(new UiToolStripItem[] {
            this.cMenuPositionIndicatorStartMarker,
            this.cMenuPositionIndicatorEndMarker});
            this.cMenuPositionIndicator.Name = "cMenuTrackTab";
            this.cMenuPositionIndicator.RenderMode = ToolStripRenderMode.System;
            this.cMenuPositionIndicator.ShowImageMargin = false;
            this.cMenuPositionIndicator.Size = new Cadencii.Gui.Dimension(151, 48);
            // 
            // cMenuPositionIndicatorStartMarker
            // 
            this.cMenuPositionIndicatorStartMarker.Name = "cMenuPositionIndicatorStartMarker";
            this.cMenuPositionIndicatorStartMarker.Size = new Cadencii.Gui.Dimension(150, 22);
            this.cMenuPositionIndicatorStartMarker.Text = "Set start marker";
            // 
            // cMenuPositionIndicatorEndMarker
            // 
            this.cMenuPositionIndicatorEndMarker.Name = "cMenuPositionIndicatorEndMarker";
            this.cMenuPositionIndicatorEndMarker.Size = new Cadencii.Gui.Dimension(150, 22);
            this.cMenuPositionIndicatorEndMarker.Text = "Set end marker";
            // 
            // menuHelpCheckForUpdates
            // 
            this.menuHelpCheckForUpdates.Name = "menuHelpCheckForUpdates";
            this.menuHelpCheckForUpdates.Size = new Cadencii.Gui.Dimension(186, 22);
            this.menuHelpCheckForUpdates.Text = "Check For Updates";
            this.menuHelpCheckForUpdates.Click += new System.EventHandler(this.menuHelpCheckForUpdates_Click);
            // 
            // FormMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(955, 689);
			this.Controls.Add((Control)this.toolStripContainer1.Native);
            this.Controls.Add((Control) this.rebar.Native);
			this.Controls.Add((Control)this.menuStripMain.Native);
            this.Controls.Add((Control)this.splitContainer1.Native);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
			this.MainMenuStrip = (MenuStrip) this.menuStripMain.Native;
            this.Name = "FormMain";
            this.Text = "Cadencii";
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.cMenuPiano.ResumeLayout(false);
            this.cMenuTrackTab.ResumeLayout(false);
            this.cMenuTrackSelector.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictKeyLengthSplitter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePositionIndicator)).EndInit();
            this.toolStripBottom.ResumeLayout(false);
            this.toolStripBottom.PerformLayout();
            this.splitContainerProperty.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelOverview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictPianoRoll)).EndInit();
            this.toolStripContainer1.BottomToolStripPanel_ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel_PerformLayout();
            this.toolStripContainer1.ContentPanel_ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.cMenuPositionIndicator.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.ComponentModel.IContainer components;

        public UiMenuStrip menuStripMain;
        public UiToolStripMenuItem menuFile;
        public UiToolStripMenuItem menuEdit;
        public UiToolStripMenuItem menuVisual;
        public UiToolStripMenuItem menuJob;
        public UiToolStripMenuItem menuTrack;
        public UiToolStripMenuItem menuLyric;
        public UiToolStripMenuItem menuSetting;
        public UiToolStripMenuItem menuHelp;
		public UiToolStripMenuItem menuVisualControlTrack { get; set; }
		public UiToolStripMenuItem menuVisualMixer { get; set; }
        public UiToolStripSeparator toolStripMenuItem1;
		public UiToolStripMenuItem menuVisualGridline { get; set; }
        public UiToolStripSeparator toolStripMenuItem2;
		public UiToolStripMenuItem menuVisualStartMarker { get; set; }
		public UiToolStripMenuItem menuVisualEndMarker { get; set; }
        public UiToolStripSeparator toolStripMenuItem3;
		public UiToolStripMenuItem menuVisualLyrics { get; set; }
		public UiToolStripMenuItem menuVisualNoteProperty { get; set; }
        public UiToolStripMenuItem menuSettingPreference;
        public UiToolStripSeparator toolStripMenuItem6;
        public UiToolStripMenuItem menuSettingDefaultSingerStyle;
        public UiToolStripMenuItem menuSettingPositionQuantize;
        public UiToolStripMenuItem menuSettingPositionQuantize04;
        public UiToolStripMenuItem menuSettingPositionQuantize08;
        public UiToolStripMenuItem menuSettingPositionQuantize16;
        public UiToolStripMenuItem menuSettingPositionQuantize32;
        public UiToolStripMenuItem menuSettingPositionQuantize64;
        public UiToolStripMenuItem menuSettingPositionQuantizeOff;
        public UiToolStripSeparator toolStripMenuItem9;
        public UiToolStripSeparator toolStripMenuItem8;
        public UiToolStripMenuItem menuSettingPositionQuantizeTriplet;
        public UiToolStripMenuItem menuFileNew;
        public UiToolStripMenuItem menuFileOpen;
        public UiToolStripMenuItem menuFileSave;
        public UiToolStripMenuItem menuFileSaveNamed;
        public UiToolStripSeparator toolStripMenuItem10;
        public UiToolStripMenuItem menuFileImport;
        public UiToolStripMenuItem menuFileExport;
        public UiToolStripSeparator toolStripMenuItem11;
        public UiToolStripSeparator toolStripMenuItem12;
        public UiToolStripMenuItem menuFileQuit;
		public UiToolStripMenuItem menuEditUndo { get; set; }
		public UiToolStripMenuItem menuEditRedo { get; set; }
        public UiToolStripSeparator toolStripMenuItem5;
        public UiPictureBox pictureBox2;
        public UiPictureBox pictureBox3;
        public UiPictureBox picturePositionIndicator;
        public UiContextMenuStrip cMenuPiano;
        public UiToolStripMenuItem cMenuPianoPointer;
        public UiToolStripMenuItem cMenuPianoPencil;
        public UiToolStripMenuItem cMenuPianoEraser;
        public UiToolStripSeparator toolStripMenuItem13;
        public UiToolStripMenuItem cMenuPianoFixed;
        public UiToolStripMenuItem cMenuPianoQuantize;
		public UiToolStripMenuItem cMenuPianoGrid { get; set; }
        public UiToolStripSeparator toolStripMenuItem14;
		public UiToolStripMenuItem cMenuPianoUndo { get; set; }
		public UiToolStripMenuItem cMenuPianoRedo { get; set; }
        public UiToolStripSeparator toolStripMenuItem15;
        public UiToolStripMenuItem cMenuPianoCut;
        public UiToolStripMenuItem cMenuPianoFixed01;
        public UiToolStripMenuItem cMenuPianoFixed02;
        public UiToolStripMenuItem cMenuPianoFixed04;
        public UiToolStripMenuItem cMenuPianoFixed08;
        public UiToolStripMenuItem cMenuPianoFixed16;
        public UiToolStripMenuItem cMenuPianoFixed32;
        public UiToolStripMenuItem cMenuPianoFixed64;
        public UiToolStripMenuItem cMenuPianoFixedOff;
        public UiToolStripSeparator toolStripMenuItem18;
        public UiToolStripMenuItem cMenuPianoFixedTriplet;
        public UiToolStripMenuItem cMenuPianoFixedDotted;
        public UiToolStripMenuItem cMenuPianoCopy;
        public UiToolStripMenuItem cMenuPianoPaste;
        public UiToolStripMenuItem cMenuPianoDelete;
        public UiToolStripSeparator toolStripMenuItem16;
        public UiToolStripMenuItem cMenuPianoSelectAll;
        public UiToolStripMenuItem cMenuPianoSelectAllEvents;
        public UiToolStripSeparator toolStripMenuItem17;
		public UiToolStripMenuItem cMenuPianoImportLyric { get; set; }
        public UiToolStripMenuItem cMenuPianoExpressionProperty;
        public UiToolStripMenuItem cMenuPianoQuantize04;
        public UiToolStripMenuItem cMenuPianoQuantize08;
        public UiToolStripMenuItem cMenuPianoQuantize16;
        public UiToolStripMenuItem cMenuPianoQuantize32;
        public UiToolStripMenuItem cMenuPianoQuantize64;
        public UiToolStripMenuItem cMenuPianoQuantizeOff;
        public UiToolStripSeparator toolStripMenuItem26;
        public UiToolStripMenuItem cMenuPianoQuantizeTriplet;
		public UiToolStripMenuItem menuFileRecent { get; set; }
        public System.Windows.Forms.ToolTip toolTip;
        public UiToolStripMenuItem menuEditCut;
        public UiToolStripMenuItem menuEditCopy;
        public UiToolStripMenuItem menuEditPaste;
        public UiToolStripMenuItem menuEditDelete;
        public UiToolStripSeparator toolStripMenuItem19;
		public UiToolStripMenuItem menuEditAutoNormalizeMode { get; set; }
        public UiToolStripSeparator toolStripMenuItem20;
        public UiToolStripMenuItem menuEditSelectAll;
        public UiToolStripMenuItem menuEditSelectAllEvents;
		public UiToolStripMenuItem menuTrackOn { get; set; }
        public UiToolStripSeparator toolStripMenuItem21;
        public UiToolStripMenuItem menuTrackAdd;
        public UiToolStripMenuItem menuTrackCopy;
        public UiToolStripMenuItem menuTrackChangeName;
        public UiToolStripMenuItem menuTrackDelete;
        public UiToolStripSeparator toolStripMenuItem22;
        public UiToolStripMenuItem menuTrackRenderCurrent;
        public UiToolStripMenuItem menuTrackRenderAll;
        public UiToolStripSeparator toolStripMenuItem23;
        public UiToolStripMenuItem menuTrackOverlay;
		public UiContextMenuStrip cMenuTrackTab;
		public UiToolStripMenuItem cMenuTrackTabTrackOn { get; set; }
        public UiToolStripSeparator toolStripMenuItem24;
        public UiToolStripMenuItem cMenuTrackTabAdd;
        public UiToolStripMenuItem cMenuTrackTabCopy;
        public UiToolStripMenuItem cMenuTrackTabChangeName;
        public UiToolStripMenuItem cMenuTrackTabDelete;
        public UiToolStripSeparator toolStripMenuItem25;
        public UiToolStripMenuItem cMenuTrackTabRenderCurrent;
        public UiToolStripMenuItem cMenuTrackTabRenderAll;
        public UiToolStripSeparator toolStripMenuItem27;
        public UiToolStripMenuItem cMenuTrackTabOverlay;
        public UiContextMenuStrip cMenuTrackSelector;
        public UiToolStripMenuItem cMenuTrackSelectorPointer;
        public UiToolStripMenuItem cMenuTrackSelectorPencil;
        public UiToolStripMenuItem cMenuTrackSelectorLine;
        public UiToolStripMenuItem cMenuTrackSelectorEraser;
        public UiToolStripSeparator toolStripMenuItem28;
		public UiToolStripMenuItem cMenuTrackSelectorUndo { get; set; }
		public UiToolStripMenuItem cMenuTrackSelectorRedo { get; set; }
        public UiToolStripSeparator toolStripMenuItem29;
        public UiToolStripMenuItem cMenuTrackSelectorCut;
        public UiToolStripMenuItem cMenuTrackSelectorCopy;
        public UiToolStripMenuItem cMenuTrackSelectorPaste;
        public UiToolStripMenuItem cMenuTrackSelectorDelete;
        public UiToolStripSeparator toolStripMenuItem31;
        public UiToolStripMenuItem cMenuTrackSelectorSelectAll;
        public UiToolStripMenuItem menuJobNormalize;
        public UiToolStripMenuItem menuJobInsertBar;
        public UiToolStripMenuItem menuJobDeleteBar;
        public UiToolStripMenuItem menuJobRandomize;
		public UiToolStripMenuItem menuJobConnect { get; set; }
		public UiToolStripMenuItem menuJobLyric { get; set; }
        public UiToolStripMenuItem menuJobRewire;
        public UiToolStripMenuItem menuLyricExpressionProperty;
        public UiToolStripMenuItem menuLyricPhonemeTransformation;
        public UiToolStripMenuItem menuLyricDictionary;
        public UiToolStripMenuItem menuHelpAbout;
        public UiToolStripMenuItem menuHelpDebug;
        public UiToolStripMenuItem menuFileExportWave;
        public UiToolStripMenuItem menuFileExportMidi;
		public UiToolStripMenuItem menuScript { get; set;}
		public UiToolStripMenuItem menuHidden { get; set; }
        public UiToolStripMenuItem menuHiddenEditLyric;
        public UiToolStripMenuItem menuHiddenEditFlipToolPointerPencil;
        public UiToolStripMenuItem menuHiddenEditFlipToolPointerEraser;
        public UiToolStripMenuItem menuHiddenVisualForwardParameter;
        public UiToolStripMenuItem menuHiddenVisualBackwardParameter;
        public UiToolStripMenuItem menuHiddenTrackNext;
        public UiToolStripMenuItem menuHiddenTrackBack;
        public UiToolStripMenuItem menuJobReloadVsti;
        public UiToolStripMenuItem cMenuPianoCurve;
        public UiToolStripMenuItem cMenuTrackSelectorCurve;
        public UiTrackBar trackBar;
        public UiToolBarButton stripBtnPointer;
        public UiToolBarButton stripBtnLine;
        public UiToolBarButton stripBtnPencil;
        public UiToolBarButton stripBtnEraser;
        public UiToolBarButton stripBtnGrid;
        public UiToolBarButton stripBtnMoveTop;
        public UiToolBarButton stripBtnRewind;
        public UiToolBarButton stripBtnForward;
        public UiToolBarButton stripBtnMoveEnd;
        public UiToolBarButton stripBtnPlay;
        public UiToolBarButton stripBtnScroll;
        public UiToolBarButton stripBtnLoop;
        public UiToolBarButton stripBtnCurve;
        public UiToolStripSeparator toolStripSeparator2;
        public UiContextMenu stripDDBtnQuantize;
        public UiMenuItem stripDDBtnQuantize04;
        public UiMenuItem stripDDBtnQuantize08;
        public UiMenuItem stripDDBtnQuantize16;
        public UiMenuItem stripDDBtnQuantize32;
        public UiMenuItem stripDDBtnQuantize64;
        public UiMenuItem stripDDBtnQuantizeOff;
        public UiToolStripSeparator toolStripSeparator3;
        public UiMenuItem stripDDBtnQuantizeTriplet;
        public UiToolBarButton stripBtnStartMarker;
		public UiToolBarButton stripBtnEndMarker { get; set; }
        public UiHScrollBar hScroll { get; set; }
		public UiVScrollBar vScroll { get; set; }
        public UiToolStripMenuItem menuLyricVibratoProperty;
        public UiToolStripMenuItem cMenuPianoVibratoProperty;
		public UiToolStripMenuItem menuScriptUpdate { get; set; }
        public UiToolStripMenuItem menuSettingGameControler;
        public UiToolStripStatusLabel stripLblGameCtrlMode;
        public UiToolStripSeparator toolStripSeparator10;
        public UiToolStripMenuItem menuSettingGameControlerSetting;
        public UiToolStripMenuItem menuSettingGameControlerLoad;
        public UiMenuItem stripDDBtnQuantize128;
        public UiToolStripMenuItem menuSettingPositionQuantize128;
        public UiToolStripMenuItem cMenuPianoQuantize128;
        public UiToolStripMenuItem cMenuPianoFixed128;
		public UiToolStripMenuItem menuVisualWaveform { get; set; }
        private WaveformZoomUi panelWaveformZoom;
        public UiToolStripMenuItem cMenuTrackSelectorDeleteBezier;
        public UiToolStripStatusLabel stripLblMidiIn;
        public UiToolStripSeparator toolStripSeparator11;
        //public UiToolStripMenuItem menuJobRealTime;
        public UiToolStripMenuItem cMenuTrackTabRenderer;
        public UiToolStripMenuItem cMenuTrackTabRendererVOCALOID1;
        public UiToolStripMenuItem cMenuTrackTabRendererVOCALOID2;
        public UiToolStripMenuItem cMenuTrackTabRendererUtau;
        private UiToolStripMenuItem cMenuTrackTabRendererAquesTone2;
		public UiToolStripMenuItem menuVisualPitchLine { get;set; }
        public UiToolStripMenuItem menuFileImportMidi;
        public UiToolStripStatusLabel toolStripStatusLabel1;
        public UiToolStripStatusLabel toolStripStatusLabel2;
        public UiToolBarButton stripBtnFileSave;
        public UiToolBarButton stripBtnFileOpen;
        public UiToolBarButton stripBtnCut;
        public UiToolBarButton stripBtnCopy;
        public UiToolBarButton stripBtnPaste;
        public UiToolBarButton stripBtnFileNew;
        public UiToolBarButton stripBtnUndo;
        public UiToolBarButton stripBtnRedo;
        public UiToolStripMenuItem cMenuTrackSelectorPaletteTool;
        public UiToolStripMenuItem cMenuPianoPaletteTool;
        public UiToolStripSeparator toolStripSeparator14;
        public UiToolStripSeparator toolStripSeparator15;
        public UiToolStripMenuItem menuSettingPaletteTool;
        public UiToolStripMenuItem menuTrackRenderer;
        public UiToolStripMenuItem menuTrackRendererVOCALOID1;
        public UiToolStripMenuItem menuTrackRendererVOCALOID2;
        public UiToolStripMenuItem menuTrackRendererUtau;
        public UiToolStripMenuItem menuFileImportVsq;
        public UiToolStripMenuItem menuSettingShortcut;
		public UiToolStripMenuItem menuVisualProperty { get; set; }
        public UiToolStripMenuItem menuFileOpenVsq;
        public UiToolStripMenuItem menuFileOpenUst;
        public UiToolStripMenuItem menuSettingGameControlerRemove;
        public UiToolStripMenuItem menuHiddenCopy;
        public UiToolStripMenuItem menuHiddenPaste;
        public UiToolStripMenuItem menuHiddenCut;
        public UiToolStrip toolStripBottom;
        public cadencii.apputil.BSplitContainer splitContainerProperty;
		public UiToolStripMenuItem menuVisualOverview { get; set; }
        public PictOverview panelOverview;
		public cadencii.apputil.BSplitContainer splitContainer1 { get; set; }
        public UiToolStripSeparator toolStripMenuItem4;
        public UiToolStripMenuItem menuTrackBgm;
        public UiToolStripMenuItem menuTrackRendererVCNT;
        //public UiToolStripMenuItem menuTrackManager;
        public UiToolStripMenuItem cMenuTrackTabRendererStraight;
        public PictPianoRoll pictPianoRoll { get; set; }
        public TrackSelector trackSelector;
        public UiPictureBox pictKeyLengthSplitter;
        private UiToolStripMenuItem menuTrackRendererAquesTone;
        private UiToolStripMenuItem cMenuTrackTabRendererAquesTone;
        private UiToolStripMenuItem menuVisualPluginUi;
		public UiToolStripMenuItem menuVisualPluginUiAquesTone { get; set; }
		public UiToolStripMenuItem menuVisualPluginUiVocaloid1 { get; set; }
		public UiToolStripMenuItem menuVisualPluginUiVocaloid2 { get; set; }
		public UiToolStripMenuItem menuTrackRendererAquesTone2 { get; set; }
		public UiToolStripMenuItem menuVisualIconPalette { get; set; }
        private UiToolStripMenuItem menuFileExportMusicXml;
        public UiToolStripMenuItem menuHiddenSelectForward;
        public UiToolStripMenuItem menuHiddenSelectBackward;
        public UiToolStripMenuItem menuHiddenMoveUp;
        public UiToolStripMenuItem menuHiddenMoveDown;
        public UiToolStripMenuItem menuHiddenMoveLeft;
        public UiToolStripMenuItem menuHiddenMoveRight;
        public UiToolStripMenuItem menuHiddenLengthen;
        public UiToolStripMenuItem menuHiddenShorten;
        public UiToolStripMenuItem menuHiddenGoToStartMarker;
        public UiToolStripMenuItem menuHiddenGoToEndMarker;
        public UiToolStripMenuItem menuHiddenPlayFromStartMarker;
        public UiToolStripMenuItem menuHiddenFlipCurveOnPianorollMode;
        //public CircuitView pictCircuit;
        private UiToolStripMenuItem menuFileExportUst;
        private UiToolStripMenuItem menuHelpLog;
		public UiToolStripMenuItem menuHelpLogSwitch { get; set; }
        private UiToolStripMenuItem menuHelpLogOpen;
        private Rebar rebar;
        private RebarBand bandFile;
        private RebarBand bandPosition;
        private RebarBand bandMeasure;
        private RebarBand bandTool;
        public cadencii.apputil.BSplitContainer splitContainer2;
        private UiPanel panel1;
        private UiToolBar toolBarFile;
        private UiImageList imageListFile;
        private UiToolBarButton toolBarButton1;
        private UiToolBarButton toolBarButton2;
        private UiToolBar toolBarTool;
        private UiToolBarButton toolBarButton3;
        private UiImageList imageListTool;
        private UiToolBar toolBarPosition;
        private UiToolBarButton toolBarButton4;
        private UiImageList imageListPosition;
        private UiToolBar toolBarMeasure;
        private UiToolBarButton stripDDBtnQuantizeParent;
        private UiMenuItem menuItem2;
        private UiToolBarButton toolBarButton5;
        private UiImageList imageListMeasure;
        private UiToolStripContainer toolStripContainer1;
        private UiStatusStrip statusStrip;
		public UiToolStripStatusLabel statusLabel { get; set; }
        private UiImageList imageListMenu;
        public UiToolStripMenuItem menuFileExportVsq;
        private UiToolStripMenuItem menuFileExportVxt;
		public UiToolStripMenuItem menuLyricCopyVibratoToPreset { get; set; }
        public UiToolStripMenuItem menuSettingVibratoPreset;
        public UiToolStripMenuItem menuSettingSequence;
        public UiToolStripMenuItem menuHiddenPrintPoToCSV;
        public UiToolStripMenuItem menuFileExportParaWave;
        public UiToolStripMenuItem menuFileImportUst;
        private UiToolStripButton stripBtnStepSequencer;
        public UiContextMenuStrip cMenuPositionIndicator;
        public UiToolStripMenuItem cMenuPositionIndicatorStartMarker;
        public UiToolStripMenuItem cMenuPositionIndicatorEndMarker;
        public UiToolStripMenuItem menuHelpManual;
		public WaveView waveView { get; set; }
		public UiToolStripMenuItem menuFileRecentClear { get; set; }
        public UiToolStripMenuItem menuLyricApplyUtauParameters;
		public UiToolStripMenuItem menuVisualPluginUiAquesTone2 { get; set; }
        private UiToolStripMenuItem menuFileExportVsqx;
        private UiToolStripMenuItem menuTools;
        private UiToolStripMenuItem menuToolsCreateVConnectSTANDDb;
        private UiToolStripMenuItem menuHelpCheckForUpdates;
    }
}