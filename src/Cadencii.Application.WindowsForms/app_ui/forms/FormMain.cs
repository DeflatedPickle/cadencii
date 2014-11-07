/*
 * FormMain.cs
 * Copyright © 2008-2011 kbinani
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
//#define USE_BGWORK_SCREEN
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Text;
using System.Threading;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.ComponentModel;
using cadencii.apputil;
using Cadencii.Gui;
using cadencii.java.io;
using cadencii.java.util;
using cadencii.javax.sound.midi;
using cadencii.media;
using cadencii.vsq;
using cadencii.vsq.io;
using cadencii.windows.forms;
using cadencii.xml;
using cadencii.utau;
using cadencii.ui;
using ApplicationGlobal = cadencii.core.ApplicationGlobal;
using Keys = Cadencii.Gui.Keys;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using KeyEventHandler = System.Windows.Forms.KeyEventHandler;
using MouseButtons = System.Windows.Forms.MouseButtons;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;
using NMouseButtons = Cadencii.Gui.MouseButtons;
using NMouseEventArgs = Cadencii.Gui.MouseEventArgs;
using NMouseEventHandler = Cadencii.Gui.MouseEventHandler;
using NKeyEventArgs = Cadencii.Gui.KeyEventArgs;
using NKeyEventHandler = Cadencii.Gui.KeyEventHandler;
using PaintEventArgs = cadencii.PaintEventArgs;

namespace cadencii
{
    /// <summary>
    /// エディタのメイン画面クラス
    /// </summary>
	public partial class FormMain : FormImpl, UiFormMain, PropertyWindowListener
    {
		FormMainModel model;

		FormMainModel UiFormMain.Model {
			get { return model; }
		}

		UiContextMenuStrip UiFormMain.MenuTrackTab {
			get { return cMenuTrackTab; }
			set { cMenuTrackTab = value; }
		}

		UiContextMenuStrip UiFormMain.MenuTrackSelector {
			get { return cMenuTrackSelector; }
			set { cMenuTrackSelector = value; }
		}

		TrackSelector UiFormMain.TrackSelector {
			get { return trackSelector; }
			set { trackSelector = value; }
		}

        /// <summary>
        /// 特殊なキーの組み合わせのショートカットと、メニューアイテムとの紐付けを保持するクラスです。
        /// </summary>
        private class SpecialShortcutHolder
        {
            /// <summary>
            /// ショートカットキーを表すKeyStrokeクラスのインスタンス
            /// </summary>
            public Keys shortcut;
            /// <summary>
            /// ショートカットキーとの紐付けを行う相手先のメニューアイテム
            /// </summary>
            public UiToolStripMenuItem menu;

            /// <summary>
            /// ショートカットキーとメニューアイテムを指定したコンストラクタ
            /// </summary>
            /// <param name="shortcut">ショートカットキー</param>
            /// <param name="menu">ショートカットキーとの紐付けを行うメニューアイテム</param>
            public SpecialShortcutHolder(Keys shortcut, UiToolStripMenuItem menu)
            {
                this.shortcut = shortcut;
                this.menu = menu;
            }
        }

        /// <summary>
        /// refreshScreenを呼び出す時に使うデリゲート
        /// </summary>
        /// <param name="value"></param>
        delegate void DelegateRefreshScreen(bool value);

        #region constants and internal enums
        /// <summary>
        /// カーブエディタ画面の編集モード
        /// </summary>
        enum CurveEditMode
        {
            /// <summary>
            /// 何もしていない
            /// </summary>
            NONE,
            /// <summary>
            /// 鉛筆ツールで編集するモード
            /// </summary>
            EDIT,
            /// <summary>
            /// ラインツールで編集するモード
            /// </summary>
            LINE,
            /// <summary>
            /// 鉛筆ツールでVELを編集するモード
            /// </summary>
            EDIT_VEL,
            /// <summary>
            /// ラインツールでVELを編集するモード
            /// </summary>
            LINE_VEL,
            /// <summary>
            /// 真ん中ボタンでドラッグ中
            /// </summary>
            MIDDLE_DRAG,
        }

        enum ExtDragXMode
        {
            RIGHT,
            LEFT,
            NONE,
        }

        enum ExtDragYMode
        {
            UP,
            DOWN,
            NONE,
        }

        enum GameControlMode
        {
            DISABLED,
            NORMAL,
            KEYBOARD,
            CURSOR,
        }

        enum PositionIndicatorMouseDownMode
        {
            NONE,
            MARK_START,
            MARK_END,
            TEMPO,
            TIMESIG,
        }

        /// <summary>
        /// スクロールバーの最小サイズ(ピクセル)
        /// </summary>
        public const int MIN_BAR_ACTUAL_LENGTH = 17;
        /// <summary>
        /// エントリの端を移動する時の、ハンドル許容範囲の幅
        /// </summary>
        public const int _EDIT_HANDLE_WIDTH = 7;
        public const int _TOOL_BAR_HEIGHT = 46;
        /// <summary>
        /// 単音プレビュー時に、wave生成完了を待つ最大の秒数
        /// </summary>
        public const double _WAIT_LIMIT = 5.0;
        /// <summary>
        /// 表情線の先頭部分のピクセル幅
        /// </summary>
        public const int _PX_ACCENT_HEADER = 21;
        public const string RECENT_UPDATE_INFO_URL = "http://www.cadencii.info/recent.php";
        /// <summary>
        /// splitContainer2.Panel2の最小サイズ
        /// </summary>
        public const int _SPL2_PANEL2_MIN_HEIGHT = 25;
        const int _PICT_POSITION_INDICATOR_HEIGHT = 48;
        const int _SCROLL_WIDTH = 16;
        /// <summary>
        /// Overviewペインの高さ
        /// </summary>
        public const int _OVERVIEW_HEIGHT = 50;
        /// <summary>
        /// splitContainerPropertyの最小幅
        /// </summary>
        const int _PROPERTY_DOCK_MIN_WIDTH = 50;
        /// <summary>
        /// WAVE再生時のバッファーサイズの最大値
        /// </summary>
        const int MAX_WAVE_MSEC_RESOLUTION = 1000;
        /// <summary>
        /// WAVE再生時のバッファーサイズの最小値
        /// </summary>
        const int MIN_WAVE_MSEC_RESOLUTION = 100;
        #endregion

        #region static field
        /// <summary>
        /// refreshScreenが呼ばれている最中かどうか
        /// </summary>
        private static bool mIsRefreshing = false;
        /// <summary>
        /// CTRLキー。MacOSXの場合はMenu
        /// </summary>
        public Keys s_modifier_key = Keys.Control;
        #endregion

        #region fields
        string appId;

        /// <summary>
        /// コントローラ
        /// </summary>
		public FormMainController controller { get; set; }
		public VersionInfo mVersionInfo { get; set; }
        public System.Windows.Forms.Cursor HAND;
        /// <summary>
        /// ボタンがDownされた位置。(座標はpictureBox基準)
        /// </summary>
        public Point mButtonInitial = new Point();
        /// <summary>
        /// 真ん中ボタンがダウンされたときのvscrollのvalue値
        /// </summary>
        public int mMiddleButtonVScroll;
        /// <summary>
        /// 真ん中ボタンがダウンされたときのhscrollのvalue値
        /// </summary>
        public int mMiddleButtonHScroll;
		public bool mEdited { get; set; }
        /// <summary>
        /// 最後にメイン画面が更新された時刻(秒単位)
        /// </summary>
        private double mLastScreenRefreshedSec;
        /// <summary>
        /// カーブエディタの編集モード
        /// </summary>
        private CurveEditMode mEditCurveMode = CurveEditMode.NONE;
        /// <summary>
        /// ピアノロールの右クリックが表示される直前のマウスの位置
        /// </summary>
        public Point mContextMenuOpenedPosition = new Point();
        /// <summary>
        /// ピアノロールの画面外へのドラッグ時、前回自動スクロール操作を行った時刻
        /// </summary>
        public double mTimerDragLastIgnitted;
        /// <summary>
        /// 画面外への自動スクロールモード
        /// </summary>
        private ExtDragXMode mExtDragX = ExtDragXMode.NONE;
        private ExtDragYMode mExtDragY = ExtDragYMode.NONE;
        /// <summary>
        /// EditMode=MoveEntryで，移動を開始する直前のマウスの仮想スクリーン上の位置
        /// </summary>
        public Point mMouseMoveInit = new Point();
        /// <summary>
        /// EditMode=MoveEntryで，移動を開始する直前のマウスの位置と，音符の先頭との距離(ピクセル)
        /// </summary>
        public int mMouseMoveOffset;
        /// <summary>
        /// マウスが降りているかどうかを表すフラグ．EditorManager.isPointerDownedとは別なので注意
        /// </summary>
        public bool mMouseDowned = false;
        public int mTempoDraggingDeltaClock = 0;
        public int mTimesigDraggingDeltaClock = 0;
        public bool mMouseDownedTrackSelector = false;
        private ExtDragXMode mExtDragXTrackSelector = ExtDragXMode.NONE;
        public bool mMouseMoved = false;
#if ENABLE_MOUSEHOVER
        /// <summary>
        /// マウスホバーを発生させるスレッド
        /// </summary>
        public Thread mMouseHoverThread = null;
#endif
        public bool mLastIsImeModeOn = true;
		public bool mLastSymbolEditMode { get; set; }
        /// <summary>
        /// 鉛筆のモード
        /// </summary>
		public PencilMode mPencilMode { get; set; } = new PencilMode();
        /// <summary>
        /// ビブラート範囲を編集中の音符のInternalID
        /// </summary>
        public int mVibratoEditingId = -1;
        /// <summary>
        /// このフォームがアクティブ化されているかどうか
        /// </summary>
        public bool mFormActivated = true;
        private GameControlMode mGameMode = GameControlMode.DISABLED;
        public System.Windows.Forms.Timer mTimer;
        public bool mLastPovR = false;
        public bool mLastPovL = false;
        public bool mLastPovU = false;
        public bool mLastPovD = false;
        public bool mLastBtnX = false;
        public bool mLastBtnO = false;
        public bool mLastBtnRe = false;
        public bool mLastBtnTr = false;
        public bool mLastBtnSelect = false;
        /// <summary>
        /// 前回ゲームコントローラのイベントを処理した時刻
        /// </summary>
        public double mLastEventProcessed;
        public bool mSpacekeyDowned = false;
#if ENABLE_MIDI
        public MidiInDevice     mMidiIn = null;
#endif
#if ENABLE_MTC
        public MidiInDevice m_midi_in_mtc = null;
#endif
		public FormMidiImExport mDialogMidiImportAndExport { get; set; }
        public SortedDictionary<EditTool, Cadencii.Gui.Cursor> mCursor = new SortedDictionary<EditTool, Cadencii.Gui.Cursor>();
		public Preference mDialogPreference { get; set; }
#if ENABLE_PROPERTY
        public PropertyPanelContainer mPropertyPanelContainer;
#endif
#if ENABLE_SCRIPT
        public List<UiToolBarButton> mPaletteTools = new List<UiToolBarButton>();
#endif

        /// <summary>
        /// PositionIndicatorのマウスモード
        /// </summary>
        private PositionIndicatorMouseDownMode mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.NONE;
        /// <summary>
        /// EditorManager.keyWidthを調節するモードに入ったかどうか
        /// </summary>
        public bool mKeyLengthSplitterMouseDowned = false;
        /// <summary>
        /// EditorManager.keyWidthを調節するモードに入る直前での、マウスのスクリーン座標
        /// </summary>
        public Point mKeyLengthSplitterInitialMouse = new Point();
        /// <summary>
        /// EditorManager.keyWidthを調節するモードに入る直前での、keyWidthの値
        /// </summary>
        public int mKeyLengthInitValue = 68;
        /// <summary>
        /// EditorManager.keyWidthを調節するモードに入る直前での、trackSelectorのgetRowsPerColumn()の値
        /// </summary>
        public int mKeyLengthTrackSelectorRowsPerColumn = 1;
        /// <summary>
        /// EditorManager.keyWidthを調節するモードに入る直前での、splitContainer1のSplitterLocationの値
        /// </summary>
        public int mKeyLengthSplitterDistance = 0;
		public UiOpenFileDialog openXmlVsqDialog { get; set; }
		public UiSaveFileDialog saveXmlVsqDialog { get; set; }
		public UiOpenFileDialog openUstDialog { get; set; }
		public UiOpenFileDialog openMidiDialog { get; set; }
		public UiSaveFileDialog saveMidiDialog { get; set; }
		public UiOpenFileDialog openWaveDialog { get; set; }
		public Cadencii.Gui.Timer timer { get; set; }
        public System.ComponentModel.BackgroundWorker bgWorkScreen;
        /// <summary>
        /// アイコンパレットのドラッグ＆ドロップ処理中，一度でもpictPianoRoll内にマウスが入ったかどうか
        /// </summary>
        private bool mIconPaletteOnceDragEntered = false;
        private byte mMtcFrameLsb;
        private byte mMtcFrameMsb;
        private byte mMtcSecLsb;
        private byte mMtcSecMsb;
        private byte mMtcMinLsb;
        private byte mMtcMinMsb;
        private byte mMtcHourLsb;
        private byte mMtcHourMsb;
        /// <summary>
        /// MTCを最後に受信した時刻
        /// </summary>
        private double mMtcLastReceived = 0.0;
        /// <summary>
        /// 特殊な取り扱いが必要なショートカットのキー列と、対応するメニューアイテムを保存しておくリスト。
        /// </summary>
        private List<SpecialShortcutHolder> mSpecialShortcutHolders = new List<SpecialShortcutHolder>();
        /// <summary>
        /// 歌詞流し込み用のダイアログ
        /// </summary>
		public FormImportLyric mDialogImportLyric { get; set; }
        /// <summary>
        /// デフォルトのストローク
        /// </summary>
        private Stroke mStrokeDefault = null;
        /// <summary>
        /// 描画幅2pxのストローク
        /// </summary>
        private Stroke mStroke2px = null;
        /// <summary>
        /// pictureBox2の描画ループで使うグラフィックス
        /// </summary>
        private Graphics mGraphicsPictureBox2 = null;
        /// <summary>
        /// ピアノロールの縦方向の拡大率を変更するパネル上でのマウスの状態。
        /// 0がデフォルト、&gt;0は+ボタンにマウスが降りた状態、&lt;0は-ボタンにマウスが降りた状態
        /// </summary>
        private int mPianoRollScaleYMouseStatus = 0;
        /// <summary>
        /// 再生中にソングポジションが前進だけしてほしいので，逆行を防ぐために前回のソングポジションを覚えておく
        /// </summary>
        private int mLastClock = 0;
        /// <summary>
        /// PositionIndicatorに表示しているポップアップのクロック位置
        /// </summary>
        private int mPositionIndicatorPopupShownClock;
        private FormWindowState mWindowState = FormWindowState.Normal;
#if MONITOR_FPS
        /// <summary>
        /// パフォーマンスカウンタ
        /// </summary>
        private double[] mFpsDrawTime = new double[128];
        private int mFpsDrawTimeIndex = 0;
        /// <summary>
        /// パフォーマンスカウンタから算出される画面の更新速度
        /// </summary>
        private float mFps = 0f;
        private double[] mFpsDrawTime2 = new double[128];
        private float mFps2 = 0f;
#endif
        #endregion

        #region constructor
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="file">最初に開くxvsq，vsqファイルのパス</param>
        public FormMain(FormMainController controller, string file)
        {
			model = new FormMainModel (this);

		this.appId = Handle.ToString ("X32");
            this.controller = controller;
            this.controller.setupUi(this);

            // 言語設定を反映させる
            Messaging.setLanguage(ApplicationGlobal.appConfig.Language);

#if ENABLE_PROPERTY
            EditorManager.propertyPanel = new PropertyPanelImpl();
            EditorManager.propertyWindow = new FormNotePropertyController(c => new FormNotePropertyUiImpl(c), this);
            EditorManager.propertyWindow.getUi().addComponent(EditorManager.propertyPanel);
#endif

#if DEBUG
            CDebug.WriteLine("FormMain..ctor()");
#endif
			cadencii.core.EditorConfig.baseFont10Bold = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.BOLD, cadencii.core.EditorConfig.FONT_SIZE10);
			cadencii.core.EditorConfig.baseFont8 = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.PLAIN, cadencii.core.EditorConfig.FONT_SIZE8);
			cadencii.core.EditorConfig.baseFont10 = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.PLAIN, cadencii.core.EditorConfig.FONT_SIZE10);
			cadencii.core.EditorConfig.baseFont9 = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.PLAIN, cadencii.core.EditorConfig.FONT_SIZE9);
			cadencii.core.EditorConfig.baseFont50Bold = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.BOLD, cadencii.core.EditorConfig.FONT_SIZE50);

            s_modifier_key = Keys.Control;
            VsqFileEx tvsq =
                new VsqFileEx(
					ApplicationGlobal.appConfig.DefaultSingerName,
                    1,
                    4,
                    4,
                    500000);
			RendererKind kind = ApplicationGlobal.appConfig.DefaultSynthesizer;
            string renderer = kind.getVersionString();
            List<VsqID> singers = MusicManager.getSingerListFromRendererKind(kind);
            tvsq.Track[1].changeRenderer(renderer, singers);
            EditorManager.setVsqFile(tvsq);

            trackSelector = new TrackSelectorImpl(this); // initializeで引数なしのコンストラクタが呼ばれるのを予防
            //TODO: javaのwaveViewはどこで作られるんだっけ？
            waveView = new WaveViewImpl();
            //TODO: これはひどい
            panelWaveformZoom = (WaveformZoomUiImpl)(new WaveformZoomController(this, waveView)).getUi();

            InitializeComponent();
			timer = ApplicationUIHost.Create<Cadencii.Gui.Timer> (this.components);

            panelOverview.setMainForm(this);
            pictPianoRoll.setMainForm(this);
            bgWorkScreen = new System.ComponentModel.BackgroundWorker();

			this.panelWaveformZoom.AddControl (this.waveView);
			this.waveView.Anchor = ((Cadencii.Gui.AnchorStyles)((((Cadencii.Gui.AnchorStyles.Top | Cadencii.Gui.AnchorStyles.Bottom)
				| Cadencii.Gui.AnchorStyles.Left)
                        | Cadencii.Gui.AnchorStyles.Right)));
            this.waveView.BackColor = new Cadencii.Gui.Color(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.waveView.Location = new Cadencii.Gui.Point(66, 0);
            this.waveView.Margin = new Cadencii.Gui.Padding(0);
            this.waveView.Name = "waveView";
            this.waveView.Size = new Cadencii.Gui.Dimension(355, 59);
            this.waveView.TabIndex = 17;
			openXmlVsqDialog = new OpenFileDialogImpl();
            openXmlVsqDialog.Filter = string.Join("|", new[] { "VSQ Format(*.vsq)|*.vsq", "XML-VSQ Format(*.xvsq)|*.xvsq" });

            saveXmlVsqDialog = new SaveFileDialogImpl();
            saveXmlVsqDialog.Filter = string.Join("|", new[] { "VSQ Format(*.vsq)|*.vsq", "XML-VSQ Format(*.xvsq)|*.xvsq", "All files(*.*)|*.*" });

            openUstDialog = new OpenFileDialogImpl();
            openUstDialog.Filter = string.Join("|", new[] { "UTAU Project File(*.ust)|*.ust", "All Files(*.*)|*.*" });

            openMidiDialog = new OpenFileDialogImpl();
            saveMidiDialog = new SaveFileDialogImpl();
            openWaveDialog = new OpenFileDialogImpl();

            /*mOverviewScaleCount = EditorManager.editorConfig.OverviewScaleCount;
            mOverviewPixelPerClock = getOverviewScaleX( mOverviewScaleCount );*/

            menuVisualOverview.Checked = EditorManager.editorConfig.OverviewEnabled;
#if ENABLE_PROPERTY
            mPropertyPanelContainer = ApplicationUIHost.Create<PropertyPanelContainer> ();
#endif

            registerEventHandlers();
            setResources();

#if !ENABLE_SCRIPT
            menuSettingPaletteTool.setVisible( false );
            menuScript.setVisible( false );
#endif
            trackSelector.updateVisibleCurves();
            trackSelector.setBackground(new Color(108, 108, 108));
            trackSelector.setCurveVisible(true);
            trackSelector.setSelectedCurve(CurveType.VEL);
            trackSelector.setLocation(new Point(0, 242));
            trackSelector.Margin = new Cadencii.Gui.Padding(0);
            trackSelector.Name = "trackSelector";
            trackSelector.setSize(446, 250);
            trackSelector.TabIndex = 0;
            trackSelector.MouseClick += new Cadencii.Gui.MouseEventHandler(trackSelector_MouseClick);
			trackSelector.MouseUp += new Cadencii.Gui.MouseEventHandler(trackSelector_MouseUp);
			trackSelector.MouseDown += new Cadencii.Gui.MouseEventHandler(trackSelector_MouseDown);
			trackSelector.MouseMove += new Cadencii.Gui.MouseEventHandler(trackSelector_MouseMove);
            trackSelector.KeyDown += new Cadencii.Gui.KeyEventHandler(handleSpaceKeyDown);
			trackSelector.KeyUp += new Cadencii.Gui.KeyEventHandler(handleSpaceKeyUp);
            trackSelector.PreviewKeyDown += new Cadencii.Gui.KeyEventHandler(trackSelector_PreviewKeyDown);
            trackSelector.SelectedTrackChanged += new SelectedTrackChangedEventHandler(trackSelector_SelectedTrackChanged);
            trackSelector.SelectedCurveChanged += new SelectedCurveChangedEventHandler(trackSelector_SelectedCurveChanged);
            trackSelector.RenderRequired += new RenderRequiredEventHandler(trackSelector_RenderRequired);
            trackSelector.PreferredMinHeightChanged += new EventHandler(trackSelector_PreferredMinHeightChanged);
			trackSelector.MouseDoubleClick += new Cadencii.Gui.MouseEventHandler(trackSelector_MouseDoubleClick);

            splitContainer1.Panel2MinSize = trackSelector.getPreferredMinSize();
            var minimum_size = getWindowMinimumSize();
            this.MinimumSize = new System.Drawing.Size(minimum_size.Width, minimum_size.Height);
            stripBtnScroll.Pushed = EditorManager.mAutoScroll;

            applySelectedTool();
            applyQuantizeMode();

            // Palette Toolの読み込み
#if ENABLE_SCRIPT
            updatePaletteTool();
#endif

            splitContainer1.Panel1.BorderStyle = Cadencii.Gui.BorderStyle.None;
            splitContainer1.Panel2.BorderStyle = Cadencii.Gui.BorderStyle.None;
            splitContainer1.BackColor = new Cadencii.Gui.Color(212, 212, 212);
            splitContainer2.Panel1.AddControl(panel1);
            panel1.Dock = Cadencii.Gui.DockStyle.Fill;
            splitContainer2.Panel2.AddControl(panelWaveformZoom);
            //splitContainer2.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            splitContainer2.Panel2.BorderColor = new Cadencii.Gui.Color(112, 112, 112);
            splitContainer1.Panel1.AddControl(splitContainer2);
            panelWaveformZoom.Dock = Cadencii.Gui.DockStyle.Fill;
			splitContainer2.Dock = Cadencii.Gui.DockStyle.Fill;
            splitContainer1.Panel2.AddControl(trackSelector);
            trackSelector.Dock = Cadencii.Gui.DockStyle.Fill;
			splitContainer1.Dock = Cadencii.Gui.DockStyle.Fill;
            splitContainer1.Panel2MinSize = trackSelector.getPreferredMinSize();
            splitContainerProperty.FixedPanel = Cadencii.Gui.FixedPanel.Panel1;

#if ENABLE_PROPERTY
            splitContainerProperty.Panel1.AddControl(mPropertyPanelContainer);
            mPropertyPanelContainer.Dock = Cadencii.Gui.DockStyle.Fill;
#else
            splitContainerProperty.setDividerLocation( 0 );
            splitContainerProperty.setEnabled( false );
            menuVisualProperty.setVisible( false );
#endif

            splitContainerProperty.Panel2.AddControl(splitContainer1);
            splitContainerProperty.Dock = Cadencii.Gui.DockStyle.Fill;

            // コントロールの位置・サイズを調節
            splitContainer2.Panel1.SuspendLayout();
            panel1.SuspendLayout();
            pictPianoRoll.SuspendLayout();
            vScroll.SuspendLayout();
            // panel1の中身は
            // picturePositionIndicator
            picturePositionIndicator.Left = 0;
            picturePositionIndicator.Top = 0;
            picturePositionIndicator.Width = panel1.Width;
            // pictPianoRoll
            pictPianoRoll.Bounds = new Cadencii.Gui.Rectangle(0, picturePositionIndicator.Height, panel1.Width - vScroll.Width, panel1.Height - picturePositionIndicator.Height - hScroll.Height);
            // vScroll
            vScroll.Left = pictPianoRoll.Width;
            vScroll.Top = picturePositionIndicator.Height;
            vScroll.Height = pictPianoRoll.Height;
            // pictureBox3
            pictureBox3.Left = 0;
            pictureBox3.Top = panel1.Height - hScroll.Height;
            pictureBox3.Height = hScroll.Height;
            // hScroll
            hScroll.Left = pictureBox3.Width;
            hScroll.Top = panel1.Height - hScroll.Height;
            hScroll.Width = panel1.Width - pictureBox3.Width - trackBar.Width - pictureBox2.Width;
            // trackBar
            trackBar.Left = pictureBox3.Width + hScroll.Width;
            trackBar.Top = panel1.Height - hScroll.Height;
            // pictureBox2
            pictureBox2.Left = panel1.Width - vScroll.Width;
            pictureBox2.Top = panel1.Height - hScroll.Height;

            vScroll.ResumeLayout();
            pictPianoRoll.ResumeLayout();
            panel1.ResumeLayout();
            splitContainer2.Panel1.ResumeLayout();

            updatePropertyPanelState(EditorManager.editorConfig.PropertyWindowStatus.State);

            pictPianoRoll.MouseWheel += pictPianoRoll_MouseWheel;
            trackSelector.MouseWheel += trackSelector_MouseWheel;
            picturePositionIndicator.MouseWheel += picturePositionIndicator_MouseWheel;

			menuVisualOverview.CheckedChanged += (o, e) => model.VisualMenu.RunVisualOverviewCheckedChanged ();

            hScroll.Maximum = MusicManager.getVsqFile().TotalClocks + 240;
            hScroll.LargeChange = 240 * 4;

            vScroll.Maximum = (int)(controller.getScaleY() * 100 * 128);
            vScroll.LargeChange = 24 * 4;
            hScroll.SmallChange = 240;
            vScroll.SmallChange = 24;

            trackSelector.setCurveVisible(true);

            // inputTextBoxの初期化
            EditorManager.InputTextBox = new LyricTextBoxImpl();
            EditorManager.InputTextBox.Visible = false;
            EditorManager.InputTextBox.BorderStyle = Cadencii.Gui.BorderStyle.None;
            EditorManager.InputTextBox.Width = 80;
            EditorManager.InputTextBox.AcceptsReturn = true;
            EditorManager.InputTextBox.BackColor = Colors.White;
			EditorManager.InputTextBox.Font = new Cadencii.Gui.Font (new System.Drawing.Font (EditorManager.editorConfig.BaseFontName, cadencii.core.EditorConfig.FONT_SIZE9, System.Drawing.FontStyle.Regular));
            EditorManager.InputTextBox.Enabled = false;
            EditorManager.InputTextBox.KeyPress += mInputTextBox_KeyPress;
            EditorManager.InputTextBox.Parent = pictPianoRoll;
            panel1.AddControl(EditorManager.InputTextBox);

            int fps = 1000 / EditorManager.editorConfig.MaximumFrameRate;
            timer.Interval = (fps <= 0) ? 1 : fps;

#if DEBUG
            menuHelpDebug.Visible = true;
#endif // DEBUG

            string _HAND = "AAACAAEAICAAABAAEADoAgAAFgAAACgAAAAgAAAAQAAAAAEABAAAAAAAgAIAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                "AAAgAAAAACAAACAgAAAAACAAIAAgAAAgIAAwMDAAICAgAD/AAAAAP8AAP//AAAAAP8A/wD/AAD//wD///8AAAAAAAAAAAAAAAAAAAA" +
                "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/wAAAAAAAAAAAAAAAAAAD" +
                "//wAAAAAAAAAAAAAAAAAA//8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP8AAAAA/wAAAAAAAAAAA" +
                "A//AAAAAP/wAAAAAAAAAAAP/wAAAAD/8AAAAAAAAAAAAP8AAAAA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                "AAAAAAAAA//8AAAAAAAAAAAAAAAAAAP//AAAAAAAAAAAAAAAAAAAP8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD////////////////////////////////////////////+f////" +
                "D////gf///4H////D///8/z//+H4f//B+D//wfg//+H4f//z/P///w////4H///+B////w////+f//////////////////////////" +
                "//////////////////w==";
            System.IO.MemoryStream ms = null;
            try {
                ms = new System.IO.MemoryStream(Base64.decode(_HAND));
                HAND = new System.Windows.Forms.Cursor(ms);
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".ctor; ex=" + ex + "\n");
            } finally {
                if (ms != null) {
                    try {
                        ms.Close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".ctor; ex=" + ex2 + "\n");
                    }
                }
            }

            menuHelpLogSwitch.Checked = Logger.isEnabled();
            applyShortcut();

			EditorManager.MixerWindow = ApplicationUIHost.Create<FormMixerUi> (this);
            EditorManager.iconPalette = ApplicationUIHost.Create<FormIconPaletteUi> (this);

            // ファイルを開く
            if (file != "") {
                if (System.IO.File.Exists(file)) {
                    string low_file = file.ToLower();
                    if (low_file.EndsWith(".xvsq")) {
                        model.OpenVsqCor(low_file);
                        //EditorManager.readVsq( file );
                    } else if (low_file.EndsWith(".vsq")) {
                        VsqFileEx vsq = null;
                        try {
                            vsq = new VsqFileEx(file, "Shift_JIS");
                            EditorManager.setVsqFile(vsq);
                            updateBgmMenuState();
                        } catch (Exception ex) {
                            Logger.write(typeof(FormMain) + ".ctor; ex=" + ex + "\n");
                            serr.println("FormMain#.ctor; ex=" + ex);
                        }
                    }
                }
            }

            trackBar.Value = EditorManager.editorConfig.DefaultXScale;
            EditorManager.setCurrentClock(0);
            setEdited(false);

			EditorManager.PreviewStarted += new EventHandler(EditorManager_PreviewStarted);
			EditorManager.PreviewAborted += new EventHandler(EditorManager_PreviewAborted);
            EditorManager.GridVisibleChanged += new EventHandler(EditorManager_GridVisibleChanged);
            EditorManager.itemSelection.SelectedEventChanged += new SelectedEventChangedEventHandler(ItemSelectionModel_SelectedEventChanged);
            EditorManager.SelectedToolChanged += new EventHandler(EditorManager_SelectedToolChanged);
            EditorManager.UpdateBgmStatusRequired += new EventHandler(EditorManager_UpdateBgmStatusRequired);
            EditorManager.MainWindowFocusRequired = new EventHandler(EditorManager_MainWindowFocusRequired);
            EditorManager.EditedStateChanged += new EditedStateChangedEventHandler(EditorManager_EditedStateChanged);
            EditorManager.WaveViewReloadRequired += new WaveViewRealoadRequiredEventHandler(EditorManager_WaveViewRealoadRequired);
            EditorConfig.QuantizeModeChanged += new EventHandler(handleEditorConfig_QuantizeModeChanged);

#if ENABLE_PROPERTY
            mPropertyPanelContainer.StateChangeRequired += new StateChangeRequiredEventHandler(mPropertyPanelContainer_StateChangeRequired);
#endif

            model.UpdateRecentFileMenu();

            // C3が画面中央に来るように調整
            int draft_start_to_draw_y = 68 * (int)(100 * controller.getScaleY()) - pictPianoRoll.Height / 2;
            int draft_vscroll_value = (int)((draft_start_to_draw_y * (double)vScroll.Maximum) / (128 * (int)(100 * controller.getScaleY()) - vScroll.Height));
            try {
                vScroll.Value = draft_vscroll_value;
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".FormMain_Load; ex=" + ex + "\n");
            }

            // x=97がプリメジャークロックになるように調整
            int cp = MusicManager.getVsqFile().getPreMeasureClocks();
            int draft_hscroll_value = (int)(cp - 24.0 * controller.getScaleXInv());
            try {
                hScroll.Value = draft_hscroll_value;
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".FormMain_Load; ex=" + ex + "\n");
            }

            //s_pen_dashed_171_171_171.DashPattern = new float[] { 3, 3 };
            //s_pen_dashed_209_204_172.DashPattern = new float[] { 3, 3 };

            menuVisualNoteProperty.Checked = EditorManager.editorConfig.ShowExpLine;
            menuVisualLyrics.Checked = EditorManager.editorConfig.ShowLyric;
            menuVisualMixer.Checked = EditorManager.editorConfig.MixerVisible;
			menuVisualPitchLine.Checked = ApplicationGlobal.appConfig.ViewAtcualPitch;

            updateMenuFonts();

            EditorManager.MixerWindow.FederChanged += new FederChangedEventHandler(mixerWindow_FederChanged);
            EditorManager.MixerWindow.PanpotChanged += new PanpotChangedEventHandler(mixerWindow_PanpotChanged);
            EditorManager.MixerWindow.MuteChanged += new MuteChangedEventHandler(mixerWindow_MuteChanged);
            EditorManager.MixerWindow.SoloChanged += new SoloChangedEventHandler(mixerWindow_SoloChanged);
            EditorManager.MixerWindow.updateStatus();
            if (EditorManager.editorConfig.MixerVisible) {
                EditorManager.MixerWindow.Visible = true;
            }
            EditorManager.MixerWindow.FormClosing += (object sender, EventArgs e) => mixerWindow_FormClosing (sender, e);

            Point p1 = EditorManager.editorConfig.FormIconPaletteLocation.toPoint();
			if (!cadencii.core2.PortUtil.isPointInScreens(p1)) {
				Rectangle workingArea = cadencii.core2.PortUtil.getWorkingArea(this);
                p1 = new Point(workingArea.X, workingArea.Y);
            }
            EditorManager.iconPalette.Location = new Point(p1.X, p1.Y);
            if (EditorManager.editorConfig.IconPaletteVisible) {
                EditorManager.iconPalette.Visible = true;
            }
            EditorManager.iconPalette.FormClosing += new EventHandler(iconPalette_FormClosing);
            EditorManager.iconPalette.LocationChanged += new EventHandler(iconPalette_LocationChanged);

            trackSelector.CommandExecuted += new EventHandler(trackSelector_CommandExecuted);

#if ENABLE_SCRIPT
            model.UpdateScriptShortcut();
            // RunOnceという名前のスクリプトがあれば，そいつを実行
            foreach (var id in ScriptServer.getScriptIdIterator()) {
                if (PortUtil.getFileNameWithoutExtension(id).ToLower() == "runonce") {
                    ScriptServer.invokeScript(id, MusicManager.getVsqFile(), (x1,x2,x3,x4) => DialogManager.showMessageBox (x1, x2, x3, x4));
                    break;
                }
            }
#endif

            model.ClearTempWave();
            updateVibratoPresetMenu();
            mPencilMode.setMode(PencilModeEnum.Off);
            updateCMenuPianoFixed();
            loadGameController();
#if ENABLE_MIDI
            reloadMidiIn();
#endif
			menuVisualWaveform.Checked = ApplicationGlobal.appConfig.ViewWaveform;

            updateRendererMenu();

            // ウィンドウの位置・サイズを再現
            if (EditorManager.editorConfig.WindowMaximized) {
                this.WindowState = FormWindowState.Maximized;
            } else {
                this.WindowState = FormWindowState.Normal;
            }
            Rectangle bounds = EditorManager.editorConfig.WindowRect;
            this.Bounds = new System.Drawing.Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            // ウィンドウ位置・サイズの設定値が、使えるディスプレイのどれにも被っていない場合
			Rectangle rc2 = cadencii.core2.PortUtil.getScreenBounds(this);
            if (bounds.X < rc2.X ||
                 rc2.X + rc2.Width < bounds.X + bounds.Width ||
                 bounds.Y < rc2.Y ||
                 rc2.Y + rc2.Height < bounds.Y + bounds.Height) {
                bounds.X = rc2.X;
                bounds.Y = rc2.Y;
                this.Bounds = new System.Drawing.Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                EditorManager.editorConfig.WindowRect = bounds;
            }
            this.LocationChanged += new EventHandler(FormMain_LocationChanged);

            updateScrollRangeHorizontal();
            updateScrollRangeVertical();

            // プロパティウィンドウの位置を復元
			Rectangle rc1 = cadencii.core2.PortUtil.getScreenBounds(this);
            Rectangle rcScreen = new Rectangle(rc1.X, rc1.Y, rc1.Width, rc1.Height);
            var p = this.Location;
            XmlRectangle xr = EditorManager.editorConfig.PropertyWindowStatus.Bounds;
            Point p0 = new Point(xr.x, xr.y);
            Point a = new Point(p.X + p0.X, p.Y + p0.Y);
            Rectangle rc = new Rectangle(a.X,
                                          a.Y,
                                          EditorManager.editorConfig.PropertyWindowStatus.Bounds.getWidth(),
                                          EditorManager.editorConfig.PropertyWindowStatus.Bounds.getHeight());

            if (a.Y > rcScreen.Y + rcScreen.Height) {
                a = new Point(a.X, rcScreen.Y + rcScreen.Height - rc.Height);
            }
            if (a.Y < rcScreen.Y) {
                a = new Point(a.X, rcScreen.Y);
            }
            if (a.X > rcScreen.X + rcScreen.Width) {
                a = new Point(rcScreen.X + rcScreen.Width - rc.Width, a.Y);
            }
            if (a.X < rcScreen.X) {
                a = new Point(rcScreen.X, a.Y);
            }
#if DEBUG
            CDebug.WriteLine("FormMain_Load; a=" + a);
#endif

#if ENABLE_PROPERTY
            EditorManager.propertyWindow.getUi().setBounds(a.X, a.Y, rc.Width, rc.Height);
            EditorManager.propertyPanel.CommandExecuteRequired += new CommandExecuteRequiredEventHandler(propertyPanel_CommandExecuteRequired);
#endif
            updateBgmMenuState();
            EditorManager.mLastTrackSelectorHeight = trackSelector.getPreferredMinSize();
            model.FlipControlCurveVisible(true);

            Refresh();
            updateLayout();
#if DEBUG
            menuHidden.Visible = true;
#else
            menuHidden.Visible = false;
#endif

#if !ENABLE_VOCALOID
            menuTrackRenderer.remove( menuTrackRendererVOCALOID2 );
            menuTrackRenderer.remove( menuTrackRendererVOCALOID1 );
            cMenuTrackTabRenderer.remove( cMenuTrackTabRendererVOCALOID2 );
            cMenuTrackTabRenderer.remove( cMenuTrackTabRendererVOCALOID1 );
#endif

#if !ENABLE_AQUESTONE
            menuTrackRenderer.DropDownItems.Remove( menuTrackRendererAquesTone );
            menuTrackRenderer.DropDownItems.Remove( menuTrackRendererAquesTone2 );
            cMenuTrackTabRenderer.DropDownItems.Remove( cMenuTrackTabRendererAquesTone );
            cMenuTrackTabRenderer.DropDownItems.Remove( cMenuTrackTabRendererAquesTone2 );
#endif

#if DEBUG
            System.Collections.Generic.List<ValuePair<string, string>> list = new System.Collections.Generic.List<ValuePair<string, string>>();
            foreach (System.Reflection.FieldInfo fi in typeof(EditorConfig).GetFields()) {
                if (fi.IsPublic && !fi.IsStatic) {
                    list.Add(new ValuePair<string, string>(fi.Name, fi.FieldType.ToString()));
                }
            }

            foreach (System.Reflection.PropertyInfo pi in typeof(EditorConfig).GetProperties()) {
                if (!pi.CanRead || !pi.CanWrite) {
                    continue;
                }
                System.Reflection.MethodInfo getmethod = pi.GetGetMethod();
                System.Reflection.MethodInfo setmethod = pi.GetSetMethod();
                if (!setmethod.IsPublic || setmethod.IsStatic) {
                    continue;
                }
                if (!getmethod.IsPublic || getmethod.IsStatic) {
                    continue;
                }
                list.Add(new ValuePair<string, string>(pi.Name, pi.PropertyType.ToString()));
            }

            list.Sort();

            System.IO.StreamWriter sw = null;
            try {
                sw = new System.IO.StreamWriter("EditorConfig.txt");
                foreach (ValuePair<string, string> s in list) {
                    sw.WriteLine(s.Key);
                }
                sw.WriteLine("--------------------------------------------");
                foreach (ValuePair<string, string> s in list) {
                    sw.WriteLine(s.Value + "\t" + s.Key + ";");
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".ctor; ex=" + ex + "\n");
            } finally {
                if (sw != null) {
                    try {
                        sw.Close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".ctor; ex=" + ex2 + "\n");
                    }
                }
            }
#endif
        }
        #endregion

        #region FormMainUiの実装

        public void focusPianoRoll()
        {
            pictPianoRoll.Focus();
        }

        #endregion

        #region helper methods
        /// <summary>
        /// renderer_menu_handler_ を初期化する
        /// </summary>
        public void initializeRendererMenuHandler()
        {
            var renderer_menu_handler_ = new List<RendererMenuHandler>();
			model.RendererMenuHandlers = renderer_menu_handler_;
            renderer_menu_handler_.Clear();
			var icon = Properties.Resources.slash.ToAwt ();
			renderer_menu_handler_.Add(new RendererMenuHandler(icon, RendererKind.VOCALOID1,
                                                                 menuTrackRendererVOCALOID1,
                                                                 cMenuTrackTabRendererVOCALOID1,
                                                                 menuVisualPluginUiVocaloid1));
			renderer_menu_handler_.Add(new RendererMenuHandler(icon, RendererKind.VOCALOID2,
                                                                 menuTrackRendererVOCALOID2,
                                                                 cMenuTrackTabRendererVOCALOID2,
                                                                 menuVisualPluginUiVocaloid2));
			renderer_menu_handler_.Add(new RendererMenuHandler(icon, RendererKind.STRAIGHT_UTAU,
                                                                 menuTrackRendererVCNT,
                                                                 cMenuTrackTabRendererStraight,
                                                                 null));
			renderer_menu_handler_.Add(new RendererMenuHandler(icon, RendererKind.UTAU,
                                                                 menuTrackRendererUtau,
                                                                 cMenuTrackTabRendererUtau,
                                                                 null));
			renderer_menu_handler_.Add(new RendererMenuHandler(icon, RendererKind.AQUES_TONE,
                                                                 menuTrackRendererAquesTone,
                                                                 cMenuTrackTabRendererAquesTone,
                                                                 menuVisualPluginUiAquesTone));
			renderer_menu_handler_.Add(new RendererMenuHandler(icon, RendererKind.AQUES_TONE2,
                                                                 menuTrackRendererAquesTone2,
                                                                 cMenuTrackTabRendererAquesTone2,
                                                                 menuVisualPluginUiAquesTone2));
        }


        /// <summary>
        /// ピアノロールの縦軸の拡大率をdelta段階上げます
        /// </summary>
        /// <param name="delta"></param>
        private void zoomY(int delta)
        {
            int scaley = EditorManager.editorConfig.PianoRollScaleY;
            int draft = scaley + delta;
            if (draft < EditorConfig.MIN_PIANOROLL_SCALEY) {
                draft = EditorConfig.MIN_PIANOROLL_SCALEY;
            }
            if (EditorConfig.MAX_PIANOROLL_SCALEY < draft) {
                draft = EditorConfig.MAX_PIANOROLL_SCALEY;
            }
            if (scaley != draft) {
                EditorManager.editorConfig.PianoRollScaleY = draft;
                updateScrollRangeVertical();
                controller.setStartToDrawY(calculateStartToDrawY(vScroll.Value));
                updateDrawObjectList();
            }
        }

        /// <summary>
        /// ズームスライダの現在の値から，横方向の拡大率を計算します
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private float getScaleXFromTrackBarValue(int value)
        {
            return value / 480.0f;
        }

        /// <summary>
        /// ユーザー定義のビブラートのプリセット関係のメニューの表示状態を更新します
        /// </summary>
        public void updateVibratoPresetMenu()
        {
            // 現在の項目数に過不足があれば調節する
            int size = EditorManager.editorConfig.AutoVibratoCustom.Count;
            int delta = size - menuLyricCopyVibratoToPreset.DropDownItems.Count;
            if (delta > 0) {
                // 項目を増やさないといけない
                for (int i = 0; i < delta; i++) {
                    var item = new ToolStripMenuItemImpl(
                            "", null, new EventHandler(handleVibratoPresetSubelementClick));
                    menuLyricCopyVibratoToPreset.DropDownItems.Add(item);
                }
            } else if (delta < 0) {
                // 項目を減らさないといけない
                for (int i = 0; i < -delta; i++) {
                    var item = menuLyricCopyVibratoToPreset.DropDownItems[0];
                    menuLyricCopyVibratoToPreset.DropDownItems.RemoveAt(0);
                    item.Dispose();
                }
            }

            // 表示状態を更新
            for (int i = 0; i < size; i++) {
                VibratoHandle handle = EditorManager.editorConfig.AutoVibratoCustom[i];
                menuLyricCopyVibratoToPreset.DropDownItems[i].Text = handle.getCaption();
            }
        }

        /// <summary>
        /// MIDIステップ入力中に，ソングポジションが動いたときの処理を行います
        /// EditorManager.mAddingEventが非nullの時，音符の先頭は決まっているので，
        /// ソングポジションと，音符の先頭との距離から音符の長さを算出し，更新する
        /// EditorManager.mAddingEventがnullの時は何もしない
        /// </summary>
        private void updateNoteLengthStepSequencer()
        {
            if (!controller.isStepSequencerEnabled()) {
                return;
            }

            VsqEvent item = EditorManager.mAddingEvent;
            if (item == null) {
                return;
            }

            int song_position = EditorManager.getCurrentClock();
            int start = item.Clock;
            int length = song_position - start;
            if (length < 0) length = 0;
            EditorManager.editLengthOfVsqEvent(
                item,
                length,
				EditorManager.vibratoLengthEditingRule);
        }

        /// <summary>
        /// 現在追加しようとしている音符の内容(EditorManager.mAddingEvent)をfixします
        /// </summary>
        /// <returns></returns>
        private void fixAddingEvent()
        {
            VsqFileEx vsq = MusicManager.getVsqFile();
            int selected = EditorManager.Selected;
            VsqTrack vsq_track = vsq.Track[selected];
            LyricHandle lyric = new LyricHandle("あ", "a");
            VibratoHandle vibrato = null;
            int vibrato_delay = 0;
            if (EditorManager.editorConfig.EnableAutoVibrato) {
                int note_length = EditorManager.mAddingEvent.ID.getLength();
                // 音符位置での拍子を調べる
                Timesig timesig = vsq.getTimesigAt(EditorManager.mAddingEvent.Clock);

                // ビブラートを自動追加するかどうかを決める閾値
                int threshold = EditorManager.editorConfig.AutoVibratoThresholdLength;
                if (note_length >= threshold) {
                    int vibrato_clocks = 0;
                    if (ApplicationGlobal.appConfig.DefaultVibratoLength == DefaultVibratoLengthEnum.L100) {
                        vibrato_clocks = note_length;
                    } else if (ApplicationGlobal.appConfig.DefaultVibratoLength == DefaultVibratoLengthEnum.L50) {
                        vibrato_clocks = note_length / 2;
                    } else if (ApplicationGlobal.appConfig.DefaultVibratoLength == DefaultVibratoLengthEnum.L66) {
                        vibrato_clocks = note_length * 2 / 3;
                    } else if (ApplicationGlobal.appConfig.DefaultVibratoLength == DefaultVibratoLengthEnum.L75) {
                        vibrato_clocks = note_length * 3 / 4;
                    }
                    SynthesizerType type = SynthesizerType.VOCALOID2;
                    RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
                    if (kind == RendererKind.VOCALOID1) {
                        type = SynthesizerType.VOCALOID1;
                    }
                    vibrato = EditorManager.editorConfig.createAutoVibrato(type, vibrato_clocks);
                    vibrato_delay = note_length - vibrato_clocks;
                }
            }

            // oto.iniの設定を反映
            VsqEvent item = vsq_track.getSingerEventAt(EditorManager.mAddingEvent.Clock);
            SingerConfig singerConfig = null;
            if (item != null && item.ID != null && item.ID.IconHandle != null) {
                singerConfig = MusicManager.getSingerInfoUtau(item.ID.IconHandle.Language, item.ID.IconHandle.Program);
            }

            if (singerConfig != null && UtauWaveGenerator.mUtauVoiceDB.ContainsKey(singerConfig.VOICEIDSTR)) {
                UtauVoiceDB utauVoiceDb = UtauWaveGenerator.mUtauVoiceDB[singerConfig.VOICEIDSTR];
                OtoArgs otoArgs = utauVoiceDb.attachFileNameFromLyric(lyric.L0.Phrase, EditorManager.mAddingEvent.ID.Note);
                EditorManager.mAddingEvent.UstEvent.setPreUtterance(otoArgs.msPreUtterance);
                EditorManager.mAddingEvent.UstEvent.setVoiceOverlap(otoArgs.msOverlap);
            }

            // 自動ノーマライズのモードで、処理を分岐
            if (EditorManager.mAutoNormalize) {
                VsqTrack work = (VsqTrack)vsq_track.clone();
                EditorManager.mAddingEvent.ID.type = VsqIDType.Anote;
                EditorManager.mAddingEvent.ID.Dynamics = 64;
                EditorManager.mAddingEvent.ID.VibratoHandle = vibrato;
                EditorManager.mAddingEvent.ID.LyricHandle = lyric;
                EditorManager.mAddingEvent.ID.VibratoDelay = vibrato_delay;

                bool changed = true;
                while (changed) {
                    changed = false;
                    for (int i = 0; i < work.getEventCount(); i++) {
                        int start_clock = work.getEvent(i).Clock;
                        int end_clock = work.getEvent(i).ID.getLength() + start_clock;
                        if (start_clock < EditorManager.mAddingEvent.Clock && EditorManager.mAddingEvent.Clock < end_clock) {
                            work.getEvent(i).ID.setLength(EditorManager.mAddingEvent.Clock - start_clock);
                            changed = true;
                        } else if (start_clock == EditorManager.mAddingEvent.Clock) {
                            work.removeEvent(i);
                            changed = true;
                            break;
                        } else if (EditorManager.mAddingEvent.Clock < start_clock && start_clock < EditorManager.mAddingEvent.Clock + EditorManager.mAddingEvent.ID.getLength()) {
                            EditorManager.mAddingEvent.ID.setLength(start_clock - EditorManager.mAddingEvent.Clock);
                            changed = true;
                        }
                    }
                }
                VsqEvent add = (VsqEvent)EditorManager.mAddingEvent.clone();
                work.addEvent(add);
                CadenciiCommand run = VsqFileEx.generateCommandTrackReplace(selected,
                                                                             work,
                                                                             MusicManager.getVsqFile().AttachedCurves.get(selected - 1));
                EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
                setEdited(true);
            } else {
                VsqEvent[] items = new VsqEvent[1];
                EditorManager.mAddingEvent.ID.type = VsqIDType.Anote;
                EditorManager.mAddingEvent.ID.Dynamics = 64;
                items[0] = (VsqEvent)EditorManager.mAddingEvent.clone();// new VsqEvent( 0, EditorManager.addingEvent.ID );
                items[0].Clock = EditorManager.mAddingEvent.Clock;
                items[0].ID.LyricHandle = lyric;
                items[0].ID.VibratoDelay = vibrato_delay;
                items[0].ID.VibratoHandle = vibrato;

                CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventAddRange(EditorManager.Selected, items));
                EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
                setEdited(true);
            }
        }

        /// <summary>
        /// 現在のツールバーの場所を保存します
        /// </summary>
        private void saveToolbarLocation()
        {
            if (this.WindowState == System.Windows.Forms.FormWindowState.Minimized) return;
            // どのツールバーが一番上かつ左にあるか？
            var list = new System.Collections.Generic.List<RebarBand>();
            list.AddRange(new RebarBand[]{
                bandFile,
                bandMeasure,
                bandPosition,
                bandTool });
            // ソートする
            bool changed = true;
            while (changed) {
                changed = false;
                for (int i = 0; i < list.Count - 1; i++) {
                    // y座標が大きいか，y座標が同じでもx座標が大きい場合に入れ替える
                    bool swap =
                        (list[i].Location.Y > list[i + 1].Location.Y) ||
                        (list[i].Location.Y == list[i + 1].Location.Y && list[i].Location.X > list[i + 1].Location.X);
                    if (swap) {
                        var a = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = a;
                        changed = true;
                    }
                }
            }
            // 各ツールバー毎に，ツールバーの状態を検出して保存
            saveToolbarLocationCore(
                list,
                bandFile,
                out EditorManager.editorConfig.BandSizeFile,
                out EditorManager.editorConfig.BandNewRowFile,
                out EditorManager.editorConfig.BandOrderFile);
            saveToolbarLocationCore(
                list,
                bandMeasure,
                out EditorManager.editorConfig.BandSizeMeasure,
                out EditorManager.editorConfig.BandNewRowMeasure,
                out EditorManager.editorConfig.BandOrderMeasure);
            saveToolbarLocationCore(
                list,
                bandPosition,
                out EditorManager.editorConfig.BandSizePosition,
                out EditorManager.editorConfig.BandNewRowPosition,
                out EditorManager.editorConfig.BandOrderPosition);
            saveToolbarLocationCore(
                list,
                bandTool,
                out EditorManager.editorConfig.BandSizeTool,
                out EditorManager.editorConfig.BandNewRowTool,
                out EditorManager.editorConfig.BandOrderTool);
        }

        /// <summary>
        /// ツールバーの位置の順に並べ替えたリストの中の一つのツールバーに対して，その状態を検出して保存
        /// </summary>
        /// <param name="list"></param>
        /// <param name="band"></param>
        /// <param name="band_size"></param>
        /// <param name="new_row"></param>
        private void saveToolbarLocationCore(
            System.Collections.Generic.List<RebarBand> list,
            RebarBand band,
            out int band_size,
            out bool new_row,
            out int band_order)
        {
            band_size = 0;
            new_row = true;
            band_order = 0;
            var indx = list.IndexOf(band);
            if (indx < 0) return;
            new_row = (indx == 0) ? false : (list[indx - 1].Location.Y < list[indx].Location.Y);
            band_size = band.BandSize;
            band_order = indx;
        }

        /// <summary>
        /// デフォルトのストロークを取得します
        /// </summary>
        /// <returns></returns>
        private Stroke getStrokeDefault()
        {
            if (mStrokeDefault == null) {
                mStrokeDefault = new Stroke();
            }
            return mStrokeDefault;
        }

        /// <summary>
        /// 描画幅が2pxのストロークを取得します
        /// </summary>
        /// <returns></returns>
        private Stroke getStroke2px()
        {
            if (mStroke2px == null) {
                mStroke2px = new Stroke(2.0f);
            }
            return mStroke2px;
        }

        /// <summary>
        /// 選択された音符の音程とゲートタイムを、指定されたノートナンバーおよびゲートタイム分上下させます。
        /// </summary>
        /// <param name="delta_note"></param>
        /// <param name="delta_clock"></param>
        public void moveUpDownLeftRight(int delta_note, int delta_clock)
        {
            VsqFileEx vsq = MusicManager.getVsqFile();
            if (vsq == null) {
                return;
            }

            List<VsqEvent> items = new List<VsqEvent>();
            int selected = EditorManager.Selected;
            int note_max = -1;
            int note_min = 129;
            int clock_max = int.MinValue;
            int clock_min = int.MaxValue;
            foreach (var item in EditorManager.itemSelection.getEventIterator()) {
                if (item.editing.ID.type != VsqIDType.Anote) {
                    continue;
                }
                VsqEvent add = null;

                // 音程
                int note = item.editing.ID.Note;
                if (delta_note != 0 && 0 <= note + delta_note && note + delta_note <= 127) {
                    add = (VsqEvent)item.editing.clone();
                    add.ID.Note += delta_note;
                    note_max = Math.Max(note_max, add.ID.Note);
                    note_min = Math.Min(note_min, add.ID.Note);
                }

                // ゲートタイム
                int clockstart = item.editing.Clock;
                int clockend = clockstart + item.editing.ID.getLength();
                if (delta_clock != 0) {
                    if (add == null) {
                        add = (VsqEvent)item.editing.clone();
                    }
                    add.Clock += delta_clock;
                    clock_max = Math.Max(clock_max, clockend + delta_clock);
                    clock_min = Math.Min(clock_min, clockstart);
                }

                if (add != null) {
                    items.Add(add);
                }
            }
            if (items.Count <= 0) {
                return;
            }

            // コマンドを発行
            CadenciiCommand run = new CadenciiCommand(
                VsqCommand.generateCommandEventReplaceRange(
                    selected, items.ToArray()));
            EditorManager.editHistory.register(vsq.executeCommand(run));

            // 編集されたものを再選択する
            foreach (var item in items) {
                EditorManager.itemSelection.addEvent(item.InternalID);
            }

            // 編集が施された。
            setEdited(true);
            updateDrawObjectList();

            // 音符が見えるようにする。音程方向
            if (delta_note > 0) {
                note_max++;
                if (127 < note_max) {
                    note_max = 127;
                }
                ensureVisibleY(note_max);
            } else if (delta_note < 0) {
                note_min -= 2;
                if (note_min < 0) {
                    note_min = 0;
                }
                ensureVisibleY(note_min);
            }

            // 音符が見えるようにする。時間方向
            if (delta_clock > 0) {
                ensureVisible(clock_max);
            } else if (delta_clock < 0) {
                ensureVisible(clock_min);
            }
            refreshScreen();
        }

        /// <summary>
        /// マウス位置におけるIDを返します。該当するIDが無ければnullを返します
        /// rectには、該当するIDがあればその画面上での形状を、該当するIDがなければ、
        /// 画面上で最も近かったIDの画面上での形状を返します
        /// </summary>
        /// <param name="mouse_position"></param>
        /// <returns></returns>
        private VsqEvent getItemAtClickedPosition(Point mouse_position, ByRef<Rectangle> rect)
        {
            rect.value = new Rectangle();
            int width = pictPianoRoll.Width;
            int height = pictPianoRoll.Height;
            int key_width = EditorManager.keyWidth;

            // マウスが可視範囲になければ死ぬ
            if (mouse_position.X < key_width || width < mouse_position.X) {
                return null;
            }
            if (mouse_position.Y < 0 || height < mouse_position.Y) {
                return null;
            }

            // 表示中のトラック番号が異常だったら死ぬ
            int selected = EditorManager.Selected;
            if (selected < 1) {
                return null;
            }
            lock (EditorManager.mDrawObjects) {
                List<DrawObject> dobj_list = EditorManager.mDrawObjects[selected - 1];
                int count = dobj_list.Count;
                int start_to_draw_x = controller.getStartToDrawX();
                int start_to_draw_y = controller.getStartToDrawY();
                VsqFileEx vsq = MusicManager.getVsqFile();
                VsqTrack vsq_track = vsq.Track[selected];

                for (int i = 0; i < count; i++) {
                    DrawObject dobj = dobj_list[i];
                    int x = dobj.mRectangleInPixel.X + key_width - start_to_draw_x;
                    int y = dobj.mRectangleInPixel.Y - start_to_draw_y;
                    if (mouse_position.X < x) {
                        continue;
                    }
                    if (x + dobj.mRectangleInPixel.Width < mouse_position.X) {
                        continue;
                    }
                    if (width < x) {
                        break;
                    }
                    if (mouse_position.Y < y) {
                        continue;
                    }
                    if (y + dobj.mRectangleInPixel.Height < mouse_position.Y) {
                        continue;
                    }
                    int internal_id = dobj.mInternalID;
                    for (Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
                        VsqEvent item = itr.next();
                        if (item.InternalID == internal_id) {
                            rect.value = new Rectangle(x, y, dobj.mRectangleInPixel.Width, dobj.mRectangleInPixel.Height);
                            return item;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 真ん中ボタンで画面を移動させるときの、vScrollの値を計算します。
        /// 計算には、mButtonInitial, mMiddleButtonVScrollの値が使われます。
        /// </summary>
        /// <returns></returns>
        private int computeVScrollValueForMiddleDrag(int mouse_y)
        {
            int dy = mouse_y - mButtonInitial.Y;
            int max = vScroll.Maximum - vScroll.LargeChange;
            int min = vScroll.Minimum;
            double new_vscroll_value = (double)mMiddleButtonVScroll - dy * max / (128.0 * (int)(100.0 * controller.getScaleY()) - (double)pictPianoRoll.Height);
            int value = (int)new_vscroll_value;
            if (value < min) {
                value = min;
            } else if (max < value) {
                value = max;
            }
            return value;
        }

        /// <summary>
        /// 真ん中ボタンで画面を移動させるときの、hScrollの値を計算します。
        /// 計算には、mButtonInitial, mMiddleButtonHScrollの値が使われます。
        /// </summary>
        /// <returns></returns>
        private int computeHScrollValueForMiddleDrag(int mouse_x)
        {
            int dx = mouse_x - mButtonInitial.X;
            int max = hScroll.Maximum - hScroll.LargeChange;
            int min = hScroll.Minimum;
            double new_hscroll_value = (double)mMiddleButtonHScroll - (double)dx * controller.getScaleXInv();
            int value = (int)new_hscroll_value;
            if (value < min) {
                value = min;
            } else if (max < value) {
                value = max;
            }
            return value;
        }

        /// <summary>
        /// 仮想スクリーン上でみた時の，現在のピアノロール画面の上端のy座標が指定した値とするための，vScrollの値を計算します
        /// calculateStartToDrawYの逆関数です
        /// </summary>
        private int calculateVScrollValueFromStartToDrawY(int start_to_draw_y)
        {
            return (int)(start_to_draw_y / controller.getScaleY());
        }

        /// <summary>
        /// 現在表示されているピアノロール画面の右上の、仮想スクリーン上座標で見たときのy座標(pixel)を取得します
        /// </summary>
        public int calculateStartToDrawY(int vscroll_value)
        {
            int min = vScroll.Minimum;
            int max = vScroll.Maximum - vScroll.LargeChange;
            int value = vscroll_value;
            if (value < min) {
                value = min;
            } else if (max < value) {
                value = max;
            }
            return (int)(value * controller.getScaleY());
        }

        /// <summary>
        /// Downloads update information xml, and deserialize it.
        /// </summary>
        /// <returns></returns>
        private updater.UpdateInfo downloadUpdateInfo()
        {
            var xml_contents = "";
            try {
                var url = RECENT_UPDATE_INFO_URL;
                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream())) {
                    xml_contents = reader.ReadToEnd();
                }
            } catch {
                return null;
            }

            updater.UpdateInfo update_info = null;
            var xml = new System.Xml.XmlDocument();
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(updater.UpdateInfo));
            try {
                xml.LoadXml(xml_contents);
                using (var stream = new MemoryStream()) {
                    xml.Save(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    update_info = serializer.Deserialize(stream) as updater.UpdateInfo;
                }
            } catch { }
            return update_info;
        }

        /// <summary>
        /// Show update information async.
        /// </summary>
        private void showUpdateInformationAsync(bool is_explicit_update_check)
        {
			MessageBox.Show ("Automatic Update is not supported in this buid");
			#if SUPPORT_UPDATE_FORM
            menuHelpCheckForUpdates.Enabled = false;
            updater.UpdateInfo update_info = null;
            var worker = new System.ComponentModel.BackgroundWorker();
            worker.DoWork += (s, e) => {
                update_info = downloadUpdateInfo();
            };
            worker.RunWorkerCompleted += (s, e) => {
                if (update_info != null && update_info.DownloadUrl != "") {
                    var current_version = new Version(BAssemblyInfo.fileVersion);
                    var recent_version_string = string.Format("{0}.{1}.{2}", update_info.Major, update_info.Minor, update_info.Build);
                    var recent_version = new Version(recent_version_string);
                    if (current_version < recent_version) {
                        var form = Factory.createUpdateCheckForm();
                        form.setDownloadUrl(update_info.DownloadUrl);
                        form.setFont(EditorManager.editorConfig.getBaseFont());
                        form.setOkButtonText(_("OK"));
                        form.setTitle(_("Check For Updates"));
                        form.setMessage(string.Format(_("New version {0} is available."), recent_version_string));
                        form.setAutomaticallyCheckForUpdates(!ApplicationGlobal.appConfig.DoNotAutomaticallyCheckForUpdates);
                        form.setAutomaticallyCheckForUpdatesMessage(_("Automatically check for updates"));
                        form.okButtonClicked += () => form.close();
                        form.downloadLinkClicked += () => {
                            form.close();
                            System.Diagnostics.Process.Start(update_info.DownloadUrl);
                        };
                        form.showDialog(this);
						ApplicationGlobal.appConfig.DoNotAutomaticallyCheckForUpdates = !form.isAutomaticallyCheckForUpdates();
                    } else if (is_explicit_update_check) {
                        MessageBox.Show(_("Cadencii is up to date"),
                                        _("Info"),
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    }
                } else if (is_explicit_update_check) {
                    MessageBox.Show(_("Can't get update information. Please retry after few minutes."),
                                    _("Error"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                var t = new System.Windows.Forms.Timer();
                t.Tick += (timer_sender, timer_args) => {
                    menuHelpCheckForUpdates.Enabled = true;
                    t.Stop();
                };
                t.Interval = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
                t.Start();
            };
            worker.RunWorkerAsync();
			#endif
        }
        #endregion

        #region public methods
        /// <summary>
        /// デフォルトのショートカットキーを格納したリストを取得します
        /// </summary>
        public IList<ValuePairOfStringArrayOfKeys> getDefaultShortcutKeys()
        {
#if JAVA_MAC
            Keys ctrl = Keys.Menu;
#else
            Keys ctrl = Keys.Control;
#endif
            List<ValuePairOfStringArrayOfKeys> ret = new List<ValuePairOfStringArrayOfKeys>(new ValuePairOfStringArrayOfKeys[]{
                new ValuePairOfStringArrayOfKeys( menuFileNew.Name, new Keys[]{ ctrl, Keys.N } ),
                new ValuePairOfStringArrayOfKeys( menuFileOpen.Name, new Keys[]{ ctrl, Keys.O } ),
                new ValuePairOfStringArrayOfKeys( menuFileOpenVsq.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuFileSave.Name, new Keys[]{ ctrl, Keys.S } ),
                new ValuePairOfStringArrayOfKeys( menuFileQuit.Name, new Keys[]{ ctrl, Keys.Q } ),
                new ValuePairOfStringArrayOfKeys( menuFileSaveNamed.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuFileImportVsq.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuFileOpenUst.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuFileImportMidi.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuFileExportWave.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuFileExportMidi.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuEditUndo.Name, new Keys[]{ ctrl, Keys.Z } ),
                new ValuePairOfStringArrayOfKeys( menuEditRedo.Name, new Keys[]{ ctrl, Keys.Shift, Keys.Z } ),
                new ValuePairOfStringArrayOfKeys( menuEditCut.Name, new Keys[]{ ctrl, Keys.X } ),
                new ValuePairOfStringArrayOfKeys( menuEditCopy.Name, new Keys[]{ ctrl, Keys.C } ),
                new ValuePairOfStringArrayOfKeys( menuEditPaste.Name, new Keys[]{ ctrl, Keys.V } ),
                new ValuePairOfStringArrayOfKeys( menuEditSelectAll.Name, new Keys[]{ ctrl, Keys.A } ),
                new ValuePairOfStringArrayOfKeys( menuEditSelectAllEvents.Name, new Keys[]{ ctrl, Keys.Shift, Keys.A } ),
                new ValuePairOfStringArrayOfKeys( menuEditDelete.Name, new Keys[]{ Keys.Back } ),
                new ValuePairOfStringArrayOfKeys( menuVisualMixer.Name, new Keys[]{ Keys.F3 } ),
                new ValuePairOfStringArrayOfKeys( menuVisualWaveform.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuVisualProperty.Name, new Keys[]{ Keys.F6 } ),
                new ValuePairOfStringArrayOfKeys( menuVisualGridline.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuVisualStartMarker.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuVisualEndMarker.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuVisualLyrics.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuVisualNoteProperty.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuVisualPitchLine.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuVisualIconPalette.Name, new Keys[]{ Keys.F4 } ),
                new ValuePairOfStringArrayOfKeys( menuJobNormalize.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuJobInsertBar.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuJobDeleteBar.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuJobRandomize.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuJobConnect.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuJobLyric.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackOn.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackAdd.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackCopy.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackChangeName.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackDelete.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackRenderCurrent.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackRenderAll.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackOverlay.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackRendererVOCALOID1.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackRendererVOCALOID2.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackRendererUtau.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuLyricExpressionProperty.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuLyricVibratoProperty.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuLyricDictionary.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuScriptUpdate.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuSettingPreference.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuSettingGameControlerSetting.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuSettingGameControlerLoad.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuSettingPaletteTool.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuSettingShortcut.Name, new Keys[]{} ),
                //new ValuePairOfStringArrayOfKeys( menuSettingSingerProperty.getName(), new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuHelpAbout.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuHiddenEditLyric.Name, new Keys[]{ Keys.F2 } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenEditFlipToolPointerPencil.Name, new Keys[]{ ctrl, Keys.W } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenEditFlipToolPointerEraser.Name, new Keys[]{ ctrl, Keys.E } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenVisualForwardParameter.Name, new Keys[]{ ctrl, Keys.Alt, Keys.PageDown } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenVisualBackwardParameter.Name, new Keys[]{ ctrl, Keys.Alt, Keys.PageUp } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenTrackNext.Name, new Keys[]{ ctrl, Keys.PageDown } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenTrackBack.Name, new Keys[]{ ctrl, Keys.PageUp } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenSelectBackward.Name, new Keys[]{ Keys.Alt, Keys.Left } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenSelectForward.Name, new Keys[]{ Keys.Alt, Keys.Right } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenMoveUp.Name, new Keys[]{ Keys.Shift, Keys.Up } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenMoveDown.Name, new Keys[]{ Keys.Shift, Keys.Down } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenMoveLeft.Name, new Keys[]{ Keys.Shift, Keys.Left } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenMoveRight.Name, new Keys[]{ Keys.Shift, Keys.Right } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenLengthen.Name, new Keys[]{ ctrl, Keys.Right } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenShorten.Name, new Keys[]{ ctrl, Keys.Left } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenGoToEndMarker.Name, new Keys[]{ ctrl, Keys.End } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenGoToStartMarker.Name, new Keys[]{ ctrl, Keys.Home } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenPlayFromStartMarker.Name, new Keys[]{ ctrl, Keys.Enter } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenFlipCurveOnPianorollMode.Name, new Keys[]{ Keys.Tab } ),
            });
            return ret;
        }

        /// <summary>
        /// マウスの真ん中ボタンが押されたかどうかを調べます。
        /// スペースキー+左ボタンで真ん中ボタンとみなすかどうか、というオプションも考慮される。
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool isMouseMiddleButtonDowned(NMouseButtons button)
        {
            bool ret = false;
            if (EditorManager.editorConfig.UseSpaceKeyAsMiddleButtonModifier) {
                if (mSpacekeyDowned && button == NMouseButtons.Left) {
                    ret = true;
                }
            } else {
                if (button == NMouseButtons.Middle) {
                    ret = true;
                }
            }
            return ret;
        }

        /// <summary>
#if USE_BGWORK_SCREEN
        /// 画面をメインスレッドとは別のワーカースレッドを用いて再描画します。
#else
        /// 画面を再描画します。
#endif
        /// 再描画間隔が設定値より短い場合再描画がスキップされます。
        /// </summary>
        public void refreshScreen(bool force)
        {
#if USE_BGWORK_SCREEN
            if ( !bgWorkScreen.IsBusy ) {
                double now = PortUtil.getCurrentTime();
                double dt = now - mLastScreenRefreshedSec;
                double mindt = 1.0 / EditorManager.editorConfig.MaximumFrameRate;
                if ( dt > mindt ) {
                    mLastScreenRefreshedSec = now;
                    bgWorkScreen.RunWorkerAsync();
                }
            }
#else
            if (mIsRefreshing) {
                return;
            } else {
                double now = PortUtil.getCurrentTime();
                double dt = now - mLastScreenRefreshedSec;
                double mindt = 1.0 / EditorManager.editorConfig.MaximumFrameRate;
                if (force || (!force && dt > mindt)) {
                    mIsRefreshing = true;

                    mLastScreenRefreshedSec = now;
                    refreshScreenCore(this, null);

                    mIsRefreshing = false;
                }
            }
#endif
        }

        public void refreshScreen()
        {
            refreshScreen(false);
        }

        public void refreshScreenCore(Object sender, EventArgs e)
        {
#if MONITOR_FPS
            double t0 = PortUtil.getCurrentTime();
#endif
            pictPianoRoll.Refresh();
            picturePositionIndicator.Refresh();
            trackSelector.Refresh();
            pictureBox2.Refresh();
            if (menuVisualWaveform.Checked) {
                waveView.Refresh();
            }
            if (EditorManager.editorConfig.OverviewEnabled) {
                panelOverview.Refresh();
            }
#if MONITOR_FPS
            double t = PortUtil.getCurrentTime();
            mFpsDrawTime[mFpsDrawTimeIndex] = t;
            mFpsDrawTime2[mFpsDrawTimeIndex] = t - t0;

            mFpsDrawTimeIndex++;
            if (mFpsDrawTimeIndex >= mFpsDrawTime.Length) {
                mFpsDrawTimeIndex = 0;
            }
            mFps = (float)(mFpsDrawTime.Length / (t - mFpsDrawTime[mFpsDrawTimeIndex]));

            int cnt = 0;
            double sum = 0.0;
            for (int i = 0; i < mFpsDrawTime2.Length; i++) {
                double v = mFpsDrawTime2[i];
                if (v > 0.0f) {
                    cnt++;
                }
                sum += v;
            }
            mFps2 = (float)(cnt / sum);
#endif
        }

        /// <summary>
        /// 現在のゲームコントローラのモードに応じてstripLblGameCtrlModeの表示状態を更新します。
        /// </summary>
        public void updateGameControlerStatus(Object sender, EventArgs e)
        {
            if (mGameMode == GameControlMode.DISABLED) {
                stripLblGameCtrlMode.Text = _("Disabled");
                stripLblGameCtrlMode.Image = Properties.Resources.slash.ToAwt ();
            } else if (mGameMode == GameControlMode.CURSOR) {
                stripLblGameCtrlMode.Text = _("Cursor");
                stripLblGameCtrlMode.Image = null;
            } else if (mGameMode == GameControlMode.KEYBOARD) {
                stripLblGameCtrlMode.Text = _("Keyboard");
				stripLblGameCtrlMode.Image = Properties.Resources.piano.ToAwt ();
            } else if (mGameMode == GameControlMode.NORMAL) {
                stripLblGameCtrlMode.Text = _("Normal");
                stripLblGameCtrlMode.Image = null;
            }
        }

        public int calculateStartToDrawX()
        {
            return (int)(hScroll.Value * controller.getScaleX());
        }


        public void invalidatePictOverview(Object sender, EventArgs e)
        {
            panelOverview.Invalidate();
        }

        public void updateBgmMenuState()
        {
            menuTrackBgm.DropDownItems.Clear();
            int count = MusicManager.getBgmCount();
            if (count > 0) {
                for (int i = 0; i < count; i++) {
                    BgmFile item = MusicManager.getBgm(i);
					UiToolStripMenuItem menu = new ToolStripMenuItemImpl();
                    menu.Text = PortUtil.getFileName(item.file);
                    menu.ToolTipText = item.file;

					BgmMenuItem menu_remove = ApplicationUIHost.Create<BgmMenuItem>(i);
                    menu_remove.Text = _("Remove");
                    menu_remove.ToolTipText = item.file;
                    menu_remove.Click += new EventHandler(handleBgmRemove_Click);
                    menu.DropDownItems.Add(menu_remove);

					BgmMenuItem menu_start_after_premeasure = ApplicationUIHost.Create<BgmMenuItem>(i);
                    menu_start_after_premeasure.Text = _("Start After Premeasure");
                    menu_start_after_premeasure.Name = "menu_start_after_premeasure" + i;
                    menu_start_after_premeasure.CheckOnClick = true;
                    menu_start_after_premeasure.Checked = item.startAfterPremeasure;
                    menu_start_after_premeasure.CheckedChanged += new EventHandler(handleBgmStartAfterPremeasure_CheckedChanged);
                    menu.DropDownItems.Add(menu_start_after_premeasure);

					BgmMenuItem menu_offset_second = ApplicationUIHost.Create<BgmMenuItem>(i);
                    menu_offset_second.Text = _("Set Offset Seconds");
                    menu_offset_second.ToolTipText = item.readOffsetSeconds + " " + _("seconds");
                    menu_offset_second.Click += new EventHandler(handleBgmOffsetSeconds_Click);
                    menu.DropDownItems.Add(menu_offset_second);

                    menuTrackBgm.DropDownItems.Add(menu);
                }
                menuTrackBgm.DropDownItems.Add(new ToolStripSeparatorImpl());
            }
            var menu_add = new ToolStripMenuItemImpl();
            menu_add.Text = _("Add");
            menu_add.Click += new EventHandler(handleBgmAdd_Click);
            menuTrackBgm.DropDownItems.Add(menu_add);
        }


#if ENABLE_PROPERTY
        public void updatePropertyPanelState(PanelState state)
        {
#if DEBUG
            sout.println("FormMain#updatePropertyPanelState; state=" + state);
#endif
            if (state == PanelState.Docked) {
                mPropertyPanelContainer.addComponent(EditorManager.propertyPanel);
                menuVisualProperty.Checked = true;
                EditorManager.editorConfig.PropertyWindowStatus.State = PanelState.Docked;
                splitContainerProperty.Panel1Hidden = (false);
                splitContainerProperty.SplitterFixed = (false);
				splitContainerProperty.DividerSize = (FormMainModel._SPL_SPLITTER_WIDTH);
                splitContainerProperty.Panel1MinSize = _PROPERTY_DOCK_MIN_WIDTH;
                int w = EditorManager.editorConfig.PropertyWindowStatus.DockWidth;
                if (w < _PROPERTY_DOCK_MIN_WIDTH) {
                    w = _PROPERTY_DOCK_MIN_WIDTH;
                }
                splitContainerProperty.DividerLocation = (w);
#if DEBUG
                sout.println("FormMain#updatePropertyPanelState; state=Docked; w=" + w);
#endif
                EditorManager.editorConfig.PropertyWindowStatus.IsMinimized = true;
                EditorManager.propertyWindow.getUi().hideWindow();
            } else if (state == PanelState.Hidden) {
                if (EditorManager.propertyWindow.getUi().isVisible()) {
                    EditorManager.propertyWindow.getUi().hideWindow();
                }
                menuVisualProperty.Checked = false;
                if (EditorManager.editorConfig.PropertyWindowStatus.State == PanelState.Docked) {
                    EditorManager.editorConfig.PropertyWindowStatus.DockWidth = splitContainerProperty.DividerLocation;
                }
                EditorManager.editorConfig.PropertyWindowStatus.State = PanelState.Hidden;
                splitContainerProperty.Panel1MinSize = 0;
                splitContainerProperty.Panel1Hidden = (true);
                splitContainerProperty.DividerLocation = (0);
                splitContainerProperty.DividerSize = (0);
                splitContainerProperty.SplitterFixed = (true);
            } else if (state == PanelState.Window) {
                EditorManager.propertyWindow.getUi().addComponent(EditorManager.propertyPanel);
                var parent = this.Location;
                XmlRectangle rc = EditorManager.editorConfig.PropertyWindowStatus.Bounds;
                Point property = new Point(rc.x, rc.y);
                int x = parent.X + property.X;
                int y = parent.Y + property.Y;
                int width = rc.Width;
                int height = rc.Height;
                EditorManager.propertyWindow.getUi().setBounds(x, y, width, height);
                int workingAreaX = EditorManager.propertyWindow.getUi().getWorkingAreaX();
                int workingAreaY = EditorManager.propertyWindow.getUi().getWorkingAreaY();
                int workingAreaWidth = EditorManager.propertyWindow.getUi().getWorkingAreaWidth();
                int workingAreaHeight = EditorManager.propertyWindow.getUi().getWorkingAreaHeight();
                Point appropriateLocation = getAppropriateDialogLocation(
                    x, y, width, height,
                    workingAreaX, workingAreaY, workingAreaWidth, workingAreaHeight
                );
                EditorManager.propertyWindow.getUi().setBounds(appropriateLocation.X, appropriateLocation.Y, width, height);
                // setVisible -> NORMALとすると，javaの場合見栄えが悪くなる
                EditorManager.propertyWindow.getUi().setVisible(true);
                if (EditorManager.propertyWindow.getUi().isWindowMinimized()) {
                    EditorManager.propertyWindow.getUi().deiconfyWindow();
                }
                menuVisualProperty.Checked = true;
                if (EditorManager.editorConfig.PropertyWindowStatus.State == PanelState.Docked) {
                    EditorManager.editorConfig.PropertyWindowStatus.DockWidth = splitContainerProperty.DividerLocation;
                }
                EditorManager.editorConfig.PropertyWindowStatus.State = PanelState.Window;
                splitContainerProperty.Panel1MinSize = 0;
                splitContainerProperty.Panel1Hidden = (true);
                splitContainerProperty.DividerLocation = (0);
                splitContainerProperty.DividerSize = (0);
                splitContainerProperty.SplitterFixed = (true);
                EditorManager.editorConfig.PropertyWindowStatus.IsMinimized = false;
            }
        }
#endif


        /// <summary>
        /// メインメニュー項目の中から，Nameプロパティがnameであるものを検索します．見つからなければnullを返す．
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public Object searchMenuItemFromName(string name, ByRef<Object> parent)
        {
            int count = menuStripMain.Items.Count;
            for (int i = 0; i < count; i++) {
                Object tsi = menuStripMain.Items[i];
                Object ret = searchMenuItemRecurse(name, tsi, parent);
                if (ret != null) {
                    if (parent.value == null) {
                        parent.value = tsi;
                    }
                    return ret;
                }
            }
            return null;
        }

        /// <summary>
        /// 指定されたメニューアイテムから，Nameプロパティがnameであるものを再帰的に検索します．見つからなければnullを返す
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tree"></param>
        /// <returns></returns>
        public Object searchMenuItemRecurse(string name, Object tree, ByRef<Object> parent)
        {
            string tree_name = "";
            UiToolStripMenuItem menu = null;
            if (tree is UiToolStripItem) {
                if (tree is UiToolStripMenuItem) {
                    menu = (UiToolStripMenuItem)tree;
                }
                tree_name = ((UiToolStripItem)tree).Name;
            } else {
                return null;
            }

            if (tree_name == name) {
                parent.value = null;
                return tree;
            } else {
                if (menu == null) {
                    return null;
                }
                int count = menu.DropDownItems.Count;
                for (int i = 0; i < count; i++) {
                    UiToolStripItem tsi = menu.DropDownItems[i];
                    string tsi_name = "";
                    if (tsi is UiToolStripItem) {
                        tsi_name = ((UiToolStripItem)tsi).Name;
                    } else {
                        continue;
                    }

                    if (tsi_name == name) {
                        return tsi;
                    }
                    Object ret = searchMenuItemRecurse(name, tsi, parent);
                    if (ret != null) {
                        parent.value = tsi;
                        return ret;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// フォームをマウス位置に出す場合に推奨されるフォーム位置を計算します
        /// </summary>
        /// <param name="dlg"></param>
        /// <returns></returns>
        public System.Drawing.Point getFormPreferedLocation(int dialogWidth, int dialogHeight)
        {
			return model.GetFormPreferedLocation (dialogWidth, dialogHeight).ToWF ();
        }

        /// <summary>
        /// フォームをマウス位置に出す場合に推奨されるフォーム位置を計算します
        /// </summary>
        /// <param name="dlg"></param>
        /// <returns></returns>
        public System.Drawing.Point getFormPreferedLocation(Form dlg)
        {
            return getFormPreferedLocation(dlg.Width, dlg.Height);
        }

        public void updateLayout()
        {
            int width = panel1.Width;
            int height = panel1.Height;

            if (EditorManager.editorConfig.OverviewEnabled) {
                panelOverview.Height = _OVERVIEW_HEIGHT;
            } else {
                panelOverview.Height = 0;
            }
            panelOverview.Width = width;
            int key_width = EditorManager.keyWidth;

            /*btnMooz.setBounds( 3, 12, 23, 23 );
            btnZoom.setBounds( 26, 12, 23, 23 );*/

            picturePositionIndicator.Width = width;
            picturePositionIndicator.Height = _PICT_POSITION_INDICATOR_HEIGHT;

            hScroll.Top = 0;
            hScroll.Left = key_width;
            hScroll.Width = width - key_width - _SCROLL_WIDTH - trackBar.Width;
            hScroll.Height = _SCROLL_WIDTH;

            vScroll.Width = _SCROLL_WIDTH;
            vScroll.Height = height - _PICT_POSITION_INDICATOR_HEIGHT - _SCROLL_WIDTH * 4 - panelOverview.Height;

            pictPianoRoll.Width = width - _SCROLL_WIDTH;
            pictPianoRoll.Height = height - _PICT_POSITION_INDICATOR_HEIGHT - _SCROLL_WIDTH - panelOverview.Height;

            pictureBox3.Width = key_width - _SCROLL_WIDTH;
            pictKeyLengthSplitter.Width = _SCROLL_WIDTH;
            pictureBox3.Height = _SCROLL_WIDTH;
            pictureBox2.Height = _SCROLL_WIDTH * 4;
            trackBar.Height = _SCROLL_WIDTH;

            panelOverview.Top = 0;
            panelOverview.Left = 0;

            picturePositionIndicator.Top = panelOverview.Height;
            picturePositionIndicator.Left = 0;

            pictPianoRoll.Top = _PICT_POSITION_INDICATOR_HEIGHT + panelOverview.Height;
            pictPianoRoll.Left = 0;

            vScroll.Top = _PICT_POSITION_INDICATOR_HEIGHT + panelOverview.Height;
            vScroll.Left = width - _SCROLL_WIDTH;

            pictureBox3.Top = height - _SCROLL_WIDTH;
            pictureBox3.Left = 0;
            pictKeyLengthSplitter.Top = height - _SCROLL_WIDTH;
            pictKeyLengthSplitter.Left = pictureBox3.Width;

            hScroll.Top = height - _SCROLL_WIDTH;
            hScroll.Left = pictureBox3.Width + pictKeyLengthSplitter.Width;

            trackBar.Top = height - _SCROLL_WIDTH;
            trackBar.Left = width - _SCROLL_WIDTH - trackBar.Width;

            pictureBox2.Top = height - _SCROLL_WIDTH * 4;
            pictureBox2.Left = width - _SCROLL_WIDTH;

            waveView.Top = 0;
            waveView.Left = key_width;
            waveView.Width = width - key_width;
            waveView.Height = panelWaveformZoom.Height;
        }

        public void updateRendererMenu()
        {
			model.RendererMenuHandlers.ForEach((handler) => handler.updateRendererAvailability(EditorManager.editorConfig));

            // UTAU用のサブアイテムを更新
            int count = ApplicationGlobal.appConfig.getResamplerCount();
            // サブアイテムの個数を整える
            int delta = count - menuTrackRendererUtau.DropDownItems.Count;
            if (delta > 0) {
                // 増やす
                for (int i = 0; i < delta; i++) {
					cMenuTrackTabRendererUtau.DropDownItems.Add("", null, (o, e) => model.TrackMenu.RunChangeRendererCommand (RendererKind.UTAU, count + i));
					menuTrackRendererUtau.DropDownItems.Add("", null, (o, e) => model.TrackMenu.RunChangeRendererCommand (RendererKind.UTAU, count + i));
                }
            } else if (delta < 0) {
                // 減らす
                for (int i = 0; i < -delta; i++) {
                    cMenuTrackTabRendererUtau.DropDownItems.RemoveAt(0);
                    menuTrackRendererUtau.DropDownItems.RemoveAt(0);
                }
            }

            for (int i = 0; i < count; i++) {
                string path = ApplicationGlobal.appConfig.getResamplerAt(i);
                string name = PortUtil.getFileNameWithoutExtension(path);
                menuTrackRendererUtau.DropDownItems[i].Text = name;
                cMenuTrackTabRendererUtau.DropDownItems[i].Text = name;

                menuTrackRendererUtau.DropDownItems[i].ToolTipText = path;
                cMenuTrackTabRendererUtau.DropDownItems[i].ToolTipText = path;
            }
        }

        public void drawUtauVibrato(Graphics g, UstVibrato vibrato, int note, int clock_start, int clock_width)
        {
            //SmoothingMode old = g.SmoothingMode;
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            // 魚雷を描いてみる
            int y0 = EditorManager.yCoordFromNote(note - 0.5f);
            int x0 = EditorManager.xCoordFromClocks(clock_start);
            int px_width = EditorManager.xCoordFromClocks(clock_start + clock_width) - x0;
            int boxheight = (int)(vibrato.Depth * 2 / 100.0 * (int)(100.0 * controller.getScaleY()));
            int px_shift = (int)(vibrato.Shift / 100.0 * vibrato.Depth / 100.0 * (int)(100.0 * controller.getScaleY()));

            // vibrato in
            int cl_vibin_end = clock_start + (int)(clock_width * vibrato.In / 100.0);
            int x_vibin_end = EditorManager.xCoordFromClocks(cl_vibin_end);
            Point ul = new Point(x_vibin_end, y0 - boxheight / 2 - px_shift);
            Point dl = new Point(x_vibin_end, y0 + boxheight / 2 - px_shift);
            g.setColor(Cadencii.Gui.Colors.Black);
            g.drawPolyline(new int[] { x0, ul.X, dl.X },
                            new int[] { y0, ul.Y, dl.Y },
                            3);

            // vibrato out
            int cl_vibout_start = clock_start + clock_width - (int)(clock_width * vibrato.Out / 100.0);
            int x_vibout_start = EditorManager.xCoordFromClocks(cl_vibout_start);
            Point ur = new Point(x_vibout_start, y0 - boxheight / 2 - px_shift);
            Point dr = new Point(x_vibout_start, y0 + boxheight / 2 - px_shift);
            g.drawPolyline(new int[] { x0 + px_width, ur.X, dr.X },
                           new int[] { y0, ur.Y, dr.Y },
                           3);

            // box
            int boxwidth = x_vibout_start - x_vibin_end;
            if (boxwidth > 0) {
                g.drawPolyline(new int[] { ul.X, dl.X, dr.X, ur.X },
                               new int[] { ul.Y, dl.Y, dr.Y, ur.Y },
                               4);
            }

            // buf1に、vibrato in/outによる増幅率を代入
            float[] buf1 = new float[clock_width + 1];
            for (int clock = clock_start; clock <= clock_start + clock_width; clock++) {
                buf1[clock - clock_start] = 1.0f;
            }
            // vibin
            if (cl_vibin_end - clock_start > 0) {
                for (int clock = clock_start; clock <= cl_vibin_end; clock++) {
                    int i = clock - clock_start;
                    buf1[i] *= i / (float)(cl_vibin_end - clock_start);
                }
            }
            if (clock_start + clock_width - cl_vibout_start > 0) {
                for (int clock = clock_start + clock_width; clock >= cl_vibout_start; clock--) {
                    int i = clock - clock_start;
                    float v = (clock_start + clock_width - clock) / (float)(clock_start + clock_width - cl_vibout_start);
                    buf1[i] = buf1[i] * v;
                }
            }

            // buf2に、shiftによるy座標のシフト量を代入
            float[] buf2 = new float[clock_width + 1];
            for (int i = 0; i < clock_width; i++) {
                buf2[i] = px_shift * buf1[i];
            }
            try {
                double phase = 2.0 * Math.PI * vibrato.Phase / 100.0;
                double omega = 2.0 * Math.PI / vibrato.Period;   //角速度(rad/msec)
                double msec = MusicManager.getVsqFile().getSecFromClock(clock_start - 1) * 1000.0;
                float px_track_height = (int)(controller.getScaleY() * 100.0f);
                phase -= (MusicManager.getVsqFile().getSecFromClock(clock_start) * 1000.0 - msec) * omega;
                for (int clock = clock_start; clock <= clock_start + clock_width; clock++) {
                    int i = clock - clock_start;
                    double t_msec = MusicManager.getVsqFile().getSecFromClock(clock) * 1000.0;
                    phase += (t_msec - msec) * omega;
                    msec = t_msec;
                    buf2[i] += (float)(vibrato.Depth * 0.01f * px_track_height * buf1[i] * Math.Sin(phase));
                }
                int[] listx = new int[clock_width + 1];
                int[] listy = new int[clock_width + 1];
                for (int clock = clock_start; clock <= clock_start + clock_width; clock++) {
                    int i = clock - clock_start;
                    listx[i] = EditorManager.xCoordFromClocks(clock);
                    listy[i] = (int)(y0 + buf2[i]);
                }
                if (listx.Length >= 2) {
                    g.setColor(Cadencii.Gui.Colors.Red);
                    g.drawPolyline(listx, listy, listx.Length);
                }
                //g.SmoothingMode = old;
            } catch (Exception oex) {
                Logger.write(typeof(FormMain) + ".drawUtauVibato; ex=" + oex + "\n");
#if DEBUG
                CDebug.WriteLine("DrawUtauVibrato; oex=" + oex);
#endif
            }
        }

#if ENABLE_SCRIPT
        /// <summary>
        /// Palette Toolの表示を更新します
        /// </summary>
        public void updatePaletteTool()
        {
            int count = 0;
            int num_has_dialog = 0;
            foreach (var item in mPaletteTools) {
                toolBarTool.Buttons.Add(item);
            }
            string lang = Messaging.getLanguage();
            bool first = true;
            foreach (var id in PaletteToolServer.loadedTools.Keys) {
                count++;
                IPaletteTool ipt = (IPaletteTool)PaletteToolServer.loadedTools[id];
                var icon = ipt.getIcon();
                string name = ipt.getName(lang);
                string desc = ipt.getDescription(lang);

                // toolStripPaletteTools
                UiToolBarButton tsb = new ToolBarButtonImpl();
                tsb.Style = Cadencii.Gui.ToolBarButtonStyle.ToggleButton;
                if (icon != null) {
                    imageListTool.Images.Add(icon);
                    tsb.ImageIndex = imageListTool.Images.Count - 1;
                }
                tsb.Text = name;
                tsb.ToolTipText = desc;
                tsb.Tag = id;
                if (first) {
                    UiToolBarButton sep = new ToolBarButtonImpl();
                    sep.Style = Cadencii.Gui.ToolBarButtonStyle.Separator;
                    toolBarTool.Buttons.Add(sep);
                    first = false;
                }
                mPaletteTools.Add(tsb);
                toolBarTool.Buttons.Add(tsb);

                // cMenuTrackSelector
				PaletteToolMenuItem tsmi = ApplicationUIHost.Create<PaletteToolMenuItem> (id);
                tsmi.Text = name;
                tsmi.ToolTipText = desc;
                tsmi.Click += new EventHandler(handleStripPaletteTool_Click);
                cMenuTrackSelectorPaletteTool.DropDownItems.Add(tsmi);

                // cMenuPiano
				PaletteToolMenuItem tsmi2 = ApplicationUIHost.Create<PaletteToolMenuItem> (id);
                tsmi2.Text = name;
                tsmi2.ToolTipText = desc;
                tsmi2.Click += new EventHandler(handleStripPaletteTool_Click);
                cMenuPianoPaletteTool.DropDownItems.Add(tsmi2);

                // menuSettingPaletteTool
                if (ipt.hasDialog()) {
					PaletteToolMenuItem tsmi3 = ApplicationUIHost.Create<PaletteToolMenuItem> (id);
                    tsmi3.Text = name;
                    tsmi3.Click += new EventHandler(handleSettingPaletteTool);
                    menuSettingPaletteTool.DropDownItems.Add(tsmi3);
                    num_has_dialog++;
                }
            }
            if (count == 0) {
                cMenuTrackSelectorPaletteTool.Visible = false;
                cMenuPianoPaletteTool.Visible = false;
            }
            if (num_has_dialog == 0) {
                menuSettingPaletteTool.Visible = false;
            }
        }
#endif

        public void updateCopyAndPasteButtonStatus()
        {
            // copy cut deleteの表示状態更新
            bool selected_is_null = (EditorManager.itemSelection.getEventCount() == 0) &&
                                       (EditorManager.itemSelection.getTempoCount() == 0) &&
                                       (EditorManager.itemSelection.getTimesigCount() == 0) &&
                                       (EditorManager.itemSelection.getPointIDCount() == 0);

            int selected_point_id_count = EditorManager.itemSelection.getPointIDCount();
            cMenuTrackSelectorCopy.Enabled = selected_point_id_count > 0;
            cMenuTrackSelectorCut.Enabled = selected_point_id_count > 0;
            cMenuTrackSelectorDeleteBezier.Enabled = (EditorManager.isCurveMode() && EditorManager.itemSelection.getLastBezier() != null);
            if (selected_point_id_count > 0) {
                cMenuTrackSelectorDelete.Enabled = true;
            } else {
                SelectedEventEntry last = EditorManager.itemSelection.getLastEvent();
                if (last == null) {
                    cMenuTrackSelectorDelete.Enabled = false;
                } else {
                    cMenuTrackSelectorDelete.Enabled = last.original.ID.type == VsqIDType.Singer;
                }
            }

            cMenuPianoCopy.Enabled = !selected_is_null;
            cMenuPianoCut.Enabled = !selected_is_null;
            cMenuPianoDelete.Enabled = !selected_is_null;

            menuEditCopy.Enabled = !selected_is_null;
            menuEditCut.Enabled = !selected_is_null;
            menuEditDelete.Enabled = !selected_is_null;

            ClipboardEntry ce = EditorManager.clipboard.getCopiedItems();
            int copy_started_clock = ce.copyStartedClock;
            SortedDictionary<CurveType, VsqBPList> copied_curve = ce.points;
            SortedDictionary<CurveType, List<BezierChain>> copied_bezier = ce.beziers;
            bool copied_is_null = (ce.events.Count == 0) &&
                                  (ce.tempo.Count == 0) &&
                                  (ce.timesig.Count == 0) &&
                                  (copied_curve.Count == 0) &&
                                  (copied_bezier.Count == 0);
            bool enabled = !copied_is_null;
            if (copied_curve.Count == 1) {
                // 1種類のカーブがコピーされている場合→コピーされているカーブの種類と、現在選択されているカーブの種類とで、最大値と最小値が一致している場合のみ、ペースト可能
                CurveType ct = CurveType.Empty;
                foreach (var c in copied_curve.Keys) {
                    ct = c;
                }
                CurveType selected = trackSelector.getSelectedCurve();
                if (ct.getMaximum() == selected.getMaximum() &&
                     ct.getMinimum() == selected.getMinimum() &&
                     !selected.isScalar() && !selected.isAttachNote()) {
                    enabled = true;
                } else {
                    enabled = false;
                }
            } else if (copied_curve.Count >= 2) {
                // 複数種類のカーブがコピーされている場合→そのままペーストすればOK
                enabled = true;
            }
            cMenuTrackSelectorPaste.Enabled = enabled;
            cMenuPianoPaste.Enabled = enabled;
            menuEditPaste.Enabled = enabled;

            /*int copy_started_clock;
            bool copied_is_null = (EditorManager.GetCopiedEvent().Count == 0) &&
                                  (EditorManager.GetCopiedTempo( out copy_started_clock ).Count == 0) &&
                                  (EditorManager.GetCopiedTimesig( out copy_started_clock ).Count == 0) &&
                                  (EditorManager.GetCopiedCurve( out copy_started_clock ).Count == 0) &&
                                  (EditorManager.GetCopiedBezier( out copy_started_clock ).Count == 0);
            menuEditCut.isEnabled() = !selected_is_null;
            menuEditCopy.isEnabled() = !selected_is_null;
            menuEditDelete.isEnabled() = !selected_is_null;
            //stripBtnCopy.isEnabled() = !selected_is_null;
            //stripBtnCut.isEnabled() = !selected_is_null;

            if ( EditorManager.GetCopiedEvent().Count != 0 ) {
                menuEditPaste.isEnabled() = (EditorManager.CurrentClock >= EditorManager.VsqFile.getPreMeasureClocks());
                //stripBtnPaste.isEnabled() = (EditorManager.CurrentClock >= EditorManager.VsqFile.getPreMeasureClocks());
            } else {
                menuEditPaste.isEnabled() = !copied_is_null;
                //stripBtnPaste.isEnabled() = !copied_is_null;
            }*/
        }

	/*
        public void loadWave(Object arg)
        {
            Object[] argArr = (Object[])arg;
            string file = (string)argArr[0];
            int track = (int)argArr[1];
            waveView.load(track, file);
        }
        */

        /// <summary>
        /// EditorManager.editorConfig.ViewWaveformの値をもとに、splitterContainer2の表示状態を更新します
        /// </summary>
        public void updateSplitContainer2Size(bool save_to_config)
        {
			if (ApplicationGlobal.appConfig.ViewWaveform) {
                splitContainer2.Panel2MinSize = (_SPL2_PANEL2_MIN_HEIGHT);
                splitContainer2.SplitterFixed = (false);
                splitContainer2.Panel2Hidden = (false);
                splitContainer2.DividerSize = (FormMainModel._SPL_SPLITTER_WIDTH);
                int lastloc = EditorManager.editorConfig.SplitContainer2LastDividerLocation;
                if (lastloc <= 0 || lastloc > splitContainer2.Height) {
                    int draft = splitContainer2.Height- 100;
                    if (draft <= 0) {
                        draft = splitContainer2.Height/ 2;
                    }
                    splitContainer2.DividerLocation = (draft);
                } else {
                    splitContainer2.DividerLocation = (lastloc);
                }
            } else {
                if (save_to_config) {
                    EditorManager.editorConfig.SplitContainer2LastDividerLocation = splitContainer2.DividerLocation;
                }
                splitContainer2.Panel2MinSize = (0);
                splitContainer2.Panel2Hidden = (true);
                splitContainer2.DividerSize = (0);
                splitContainer2.DividerLocation = (splitContainer2.Height);
                splitContainer2.SplitterFixed = (true);
            }
        }

        /// <summary>
        /// ウィンドウの表示内容に応じて、ウィンドウサイズの最小値を計算します
        /// </summary>
        /// <returns></returns>
        public Dimension getWindowMinimumSize()
        {
            Dimension current_minsize = new Dimension(MinimumSize.Width, MinimumSize.Height);
            Dimension client = new Dimension(this.ClientSize.Width, this.ClientSize.Height);
            Dimension current = new Dimension(this.Size.Width, this.Size.Height);
            return new Dimension(current_minsize.Width,
                                  splitContainer1.Panel2MinSize +
                                  _SCROLL_WIDTH + _PICT_POSITION_INDICATOR_HEIGHT + pictPianoRoll.MinimumSize.Height +
                                  rebar.Height +
                                  menuStripMain.Height + statusStrip.Height +
                                  (current.Height - client.Height) +
                                  20);
        }

        /// <summary>
        /// 現在のEditorManager.InputTextBoxの状態を元に、歌詞の変更を反映させるコマンドを実行します
        /// </summary>
        public void executeLyricChangeCommand()
        {
            if (!EditorManager.InputTextBox.Enabled) {
                return;
            }
            if (EditorManager.InputTextBox.IsDisposed) {
                return;
            }
            SelectedEventEntry last_selected_event = EditorManager.itemSelection.getLastEvent();
            bool phonetic_symbol_edit_mode = EditorManager.InputTextBox.isPhoneticSymbolEditMode();

            int selected = EditorManager.Selected;
            VsqFileEx vsq = MusicManager.getVsqFile();
            VsqTrack vsq_track = vsq.Track[selected];

            // 後続に、連続している音符が何個あるか検査
            int maxcount = SymbolTable.getMaxDivisions(); // 音節の分割によって，高々maxcount個までにしか分割されない
            bool check_started = false;
            int endclock = 0;  // 直前の音符の終了クロック
            int count = 0;     // 後続音符の連続個数
            int start_index = -1;
            int indx = -1;
            for (Iterator<int> itr = vsq_track.indexIterator(IndexIteratorKind.NOTE); itr.hasNext(); ) {
                indx = itr.next();
                VsqEvent itemi = vsq_track.getEvent(indx);
                if (itemi.InternalID == last_selected_event.original.InternalID) {
                    check_started = true;
                    endclock = itemi.Clock + itemi.ID.getLength();
                    count = 1;
                    start_index = indx;
                    continue;
                }
                if (check_started) {
                    if (count + 1 > maxcount) {
                        break;
                    }
                    if (itemi.Clock <= endclock) {
                        count++;
                        endclock = itemi.Clock + itemi.ID.getLength();
                    } else {
                        break;
                    }
                }
            }

            // 後続の音符をリストアップ
            VsqEvent[] items = new VsqEvent[count];
            string[] original_symbol = new string[count];
            string[] original_phrase = new string[count];
            bool[] symbol_protected = new bool[count];
            indx = -1;
            for (Iterator<int> itr = vsq_track.indexIterator(IndexIteratorKind.NOTE); itr.hasNext(); ) {
                int index = itr.next();
                if (index < start_index) {
                    continue;
                }
                indx++;
                if (count <= indx) {
                    break;
                }
                VsqEvent ve = vsq_track.getEvent(index);
                items[indx] = (VsqEvent)ve.clone();
                original_symbol[indx] = ve.ID.LyricHandle.L0.getPhoneticSymbol();
                original_phrase[indx] = ve.ID.LyricHandle.L0.Phrase;
                symbol_protected[indx] = ve.ID.LyricHandle.L0.PhoneticSymbolProtected;
            }

#if DEBUG
            CDebug.WriteLine("    original_phase,symbol=" + original_phrase + "," + original_symbol[0]);
            CDebug.WriteLine("    phonetic_symbol_edit_mode=" + phonetic_symbol_edit_mode);
            CDebug.WriteLine("    EditorManager.InputTextBox.setText(=" + EditorManager.InputTextBox.Text);
#endif
            string[] phrase = new string[count];
            string[] phonetic_symbol = new string[count];
            for (int i = 0; i < count; i++) {
                phrase[i] = original_phrase[i];
                phonetic_symbol[i] = original_symbol[i];
            }
            string txt = EditorManager.InputTextBox.Text;
            int txtlen = PortUtil.getStringLength(txt);
            if (txtlen > 0) {
                // 1文字目は，UTAUの連続音入力のハイフンの可能性があるので，無駄に置換されるのを回避
                phrase[0] = txt.Substring(0, 1) + ((txtlen > 1) ? txt.Substring(1).Replace("-", "") : "");
            } else {
                phrase[0] = "";
            }
            if (!phonetic_symbol_edit_mode) {
                // 歌詞を編集するモードで、
                if (EditorManager.editorConfig.SelfDeRomanization) {
                    // かつローマ字の入力を自動でひらがなに展開する設定だった場合。
                    // ローマ字をひらがなに展開
                    phrase[0] = KanaDeRomanization.Attach(phrase[0]);
                }
            }

            // 発音記号または歌詞が変更された場合の処理
            if ((phonetic_symbol_edit_mode && EditorManager.InputTextBox.Text != original_symbol[0]) ||
                 (!phonetic_symbol_edit_mode && phrase[0] != original_phrase[0])) {
                if (phonetic_symbol_edit_mode) {
                    // 発音記号を編集するモード
                    phrase[0] = EditorManager.InputTextBox.getBufferText();
                    phonetic_symbol[0] = EditorManager.InputTextBox.Text;

                    // 入力された発音記号のうち、有効なものだけをピックアップ
                    string[] spl = PortUtil.splitString(phonetic_symbol[0], new char[] { ' ' }, true);
                    List<string> list = new List<string>();
                    for (int i = 0; i < spl.Length; i++) {
                        string s = spl[i];
                        if (VsqPhoneticSymbol.isValidSymbol(s)) {
                            list.Add(s);
                        }
                    }

                    // ピックアップした発音記号をスペース区切りで結合
                    phonetic_symbol[0] = "";
                    bool first = true;
                    foreach (var s in list) {
                        if (first) {
                            phonetic_symbol[0] += s;
                            first = false;
                        } else {
                            phonetic_symbol[0] += " " + s;
                        }
                    }

                    // 発音記号を編集すると、自動で「発音記号をプロテクトする」モードになるよ
                    symbol_protected[0] = true;
                } else {
                    // 歌詞を編集するモード
                    if (!symbol_protected[0]) {
                        // 発音記号をプロテクトしない場合、歌詞から発音記号を引当てる
                        SymbolTableEntry entry = SymbolTable.attatch(phrase[0]);
                        if (entry == null) {
                            phonetic_symbol[0] = "a";
                        } else {
                            phonetic_symbol[0] = entry.getSymbol();
                            // 分節の分割記号'-'が入っている場合
#if DEBUG
                            sout.println("FormMain#executeLyricChangeCommand; word=" + entry.Word + "; symbol=" + entry.getSymbol() + "; rawSymbol=" + entry.getRawSymbol());
#endif
                            if (entry.Word.IndexOf('-') >= 0) {
                                string[] spl_phrase = PortUtil.splitString(entry.Word, '\t');
                                if (spl_phrase.Length <= count) {
                                    // 分節の分割数が，後続の音符数と同じか少ない場合
                                    string[] spl_symbol = PortUtil.splitString(entry.getRawSymbol(), '\t');
                                    for (int i = 0; i < spl_phrase.Length; i++) {
                                        phrase[i] = spl_phrase[i];
                                        phonetic_symbol[i] = spl_symbol[i];
                                    }
                                } else {
                                    // 後続の音符の個数が足りない
                                    phrase[0] = entry.Word.Replace("\t", "");
                                }
                            }
                        }
                    } else {
                        // 発音記号をプロテクトする場合、発音記号は最初のやつを使う
                        phonetic_symbol[0] = original_symbol[0];
                    }
                }
#if DEBUG
                CDebug.WriteLine("    phrase,phonetic_symbol=" + phrase + "," + phonetic_symbol);
#endif

                for (int j = 0; j < count; j++) {
                    if (phonetic_symbol_edit_mode) {
                        items[j].ID.LyricHandle.L0.setPhoneticSymbol(phonetic_symbol[j]);
                    } else {
                        items[j].ID.LyricHandle.L0.Phrase = phrase[j];
                        items[j].ID.LyricHandle.L0.setPhoneticSymbol(phonetic_symbol[j]);
                        MusicManager.applyUtauParameter(vsq_track, items[j]);
                    }
                    if (original_symbol[j] != phonetic_symbol[j]) {
                        List<string> spl = items[j].ID.LyricHandle.L0.getPhoneticSymbolList();
                        List<int> adjustment = new List<int>();
                        for (int i = 0; i < spl.Count; i++) {
                            string s = spl[i];
                            adjustment.Add(VsqPhoneticSymbol.isConsonant(s) ? 64 : 0);
                        }
                        items[j].ID.LyricHandle.L0.setConsonantAdjustmentList(adjustment);
                    }
                }

                CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventReplaceRange(selected, items));
                EditorManager.editHistory.register(vsq.executeCommand(run));
                setEdited(true);
            }
        }

        /// <summary>
        /// 識別済みのゲームコントローラを取り外します
        /// </summary>
        public void removeGameControler()
        {
            if (mTimer != null) {
                mTimer.Stop();
                mTimer.Dispose();
                mTimer = null;
            }
            mGameMode = GameControlMode.DISABLED;
            updateGameControlerStatus(null, null);
        }

        /// <summary>
        /// PCに接続されているゲームコントローラを識別・接続します
        /// </summary>
        public void loadGameController()
        {
            try {
                bool init_success = false;
                int num_joydev = winmmhelp.JoyInit();
                if (num_joydev <= 0) {
                    init_success = false;
                } else {
                    init_success = true;
                }
                if (init_success) {
                    mGameMode = GameControlMode.NORMAL;
                    stripLblGameCtrlMode.Image = null;
                    stripLblGameCtrlMode.Text = mGameMode.ToString();
                    mTimer = new System.Windows.Forms.Timer();
                    mTimer.Interval = 10;
                    mTimer.Tick += new EventHandler(mTimer_Tick);
                    mTimer.Start();
                } else {
                    mGameMode = GameControlMode.DISABLED;
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".loadGameControler; ex=" + ex + "\n");
                mGameMode = GameControlMode.DISABLED;
#if DEBUG
                CDebug.WriteLine("FormMain+ReloadGameControler");
                CDebug.WriteLine("    ex=" + ex);
#endif
            }
            updateGameControlerStatus(null, null);
        }

#if ENABLE_MIDI
        /// <summary>
        /// MIDI入力句デバイスを再読込みします
        /// </summary>
        public void reloadMidiIn()
        {
            if (mMidiIn != null) {
                mMidiIn.MidiReceived -= new MidiReceivedEventHandler(mMidiIn_MidiReceived);
                mMidiIn.close();
                mMidiIn = null;
            }
            int portNumber = EditorManager.editorConfig.MidiInPort.PortNumber;
            int portNumberMtc = EditorManager.editorConfig.MidiInPortMtc.PortNumber;
#if DEBUG
            sout.println("FormMain#reloadMidiIn; portNumber=" + portNumber + "; portNumberMtc=" + portNumberMtc);
#endif
            try {
                mMidiIn = new MidiInDevice(portNumber);
                mMidiIn.MidiReceived += new MidiReceivedEventHandler(mMidiIn_MidiReceived);
#if ENABLE_MTC
                if ( portNumber == portNumberMtc ) {
                    m_midi_in.setReceiveSystemCommonMessage( true );
                    m_midi_in.setReceiveSystemRealtimeMessage( true );
                    m_midi_in.MidiReceived += handleMtcMidiReceived;
                    m_midi_in.Start();
                } else {
                    m_midi_in.setReceiveSystemCommonMessage( false );
                    m_midi_in.setReceiveSystemRealtimeMessage( false );
                }
#else
                mMidiIn.setReceiveSystemCommonMessage(false);
                mMidiIn.setReceiveSystemRealtimeMessage(false);
#endif
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".reloadMidiIn; ex=" + ex + "\n");
                serr.println("FormMain#reloadMidiIn; ex=" + ex);
            }

#if ENABLE_MTC
            if ( m_midi_in_mtc != null ) {
                m_midi_in_mtc.MidiReceived -= handleMtcMidiReceived;
                m_midi_in_mtc.Dispose();
                m_midi_in_mtc = null;
            }
            if ( portNumber != portNumberMtc ) {
                try {
                    m_midi_in_mtc = new MidiInDevice( EditorManager.editorConfig.MidiInPortMtc.PortNumber );
                    m_midi_in_mtc.MidiReceived += handleMtcMidiReceived;
                    m_midi_in_mtc.setReceiveSystemCommonMessage( true );
                    m_midi_in_mtc.setReceiveSystemRealtimeMessage( true );
                    m_midi_in_mtc.Start();
                } catch ( Exception ex ) {
                    Logger.write( typeof( FormMain ) + ".reloadMidiIn; ex=" + ex + "\n" );
                    serr.println( "FormMain#reloadMidiIn; ex=" + ex );
                }
            }
#endif
            updateMidiInStatus();
        }
#endif

#if ENABLE_MIDI
        public void updateMidiInStatus()
        {
            int midiport = EditorManager.editorConfig.MidiInPort.PortNumber;
            List<MidiDevice.Info> devices = new List<MidiDevice.Info>();
            foreach (MidiDevice.Info info in MidiSystem.getMidiDeviceInfo()) {
                MidiDevice device = null;
                try {
                    device = MidiSystem.getMidiDevice(info);
                } catch (Exception ex) {
                    device = null;
                }
                if (device == null) continue;
                int max = device.getMaxTransmitters();
                if (max > 0 || max == -1) {
                    devices.Add(info);
                }
            }
            if (midiport < 0 || devices.Count <= 0) {
                stripLblMidiIn.Text = _("Disabled");
				stripLblMidiIn.Image = Properties.Resources.slash.ToAwt ();
            } else {
                if (midiport >= devices.Count) {
                    midiport = 0;
                    EditorManager.editorConfig.MidiInPort.PortNumber = midiport;
                }
                stripLblMidiIn.Text = devices[midiport].getName();
				stripLblMidiIn.Image = Properties.Resources.piano.ToAwt ();
            }
        }
#endif
        /// <summary>
        /// 指定したノートナンバーが可視状態となるよう、縦スクロールバーを移動させます。
        /// </summary>
        /// <param name="note"></param>
        public void ensureVisibleY(int note)
        {
            Action<int> setVScrollValue = (value) => {
                int draft = Math.Min(Math.Max(value, vScroll.Minimum), vScroll.Maximum);
                vScroll.Value = draft;
            };
            if (note <= 0) {
                setVScrollValue(vScroll.Maximum - vScroll.LargeChange);
                return;
            } else if (note >= 127) {
                vScroll.Value = vScroll.Minimum;
                return;
            }
            int height = pictPianoRoll.Height;
            int noteTop = EditorManager.noteFromYCoord(0); //画面上端でのノートナンバー
            int noteBottom = EditorManager.noteFromYCoord(height); // 画面下端でのノートナンバー

            int maximum = vScroll.Maximum;
            int track_height = (int)(100 * controller.getScaleY());
            // ノートナンバーnoteの現在のy座標がいくらか？
            int note_y = EditorManager.yCoordFromNote(note);
            if (note < noteBottom) {
                // ノートナンバーnoteBottomの現在のy座標が新しいnoteのy座標と同一になるよう，startToDrawYを変える
                // startToDrawYを次の値にする必要がある
                int new_start_to_draw_y = controller.getStartToDrawY() + (note_y - height);
                int value = calculateVScrollValueFromStartToDrawY(new_start_to_draw_y);
                setVScrollValue(value);
            } else if (noteTop < note) {
                // ノートナンバーnoteTopの現在のy座標が，ノートナンバーnoteの新しいy座標と同一になるよう，startToDrawYを変える
                int new_start_to_draw_y = controller.getStartToDrawY() + (note_y - 0);
                int value = calculateVScrollValueFromStartToDrawY(new_start_to_draw_y);
                setVScrollValue(value);
            }
        }

        /// <summary>
        /// 指定したゲートタイムがピアノロール上で可視状態となるよう、横スクロールバーを移動させます。
        /// </summary>
        /// <param name="clock"></param>
        public void ensureVisible(int clock)
        {
            // カーソルが画面内にあるかどうか検査
            int clock_left = EditorManager.clockFromXCoord(EditorManager.keyWidth);
            int clock_right = EditorManager.clockFromXCoord(pictPianoRoll.Width);
            int uwidth = clock_right - clock_left;
            if (clock < clock_left || clock_right < clock) {
                int cl_new_center = (clock / uwidth) * uwidth + uwidth / 2;
                float f_draft = cl_new_center - (pictPianoRoll.Width / 2 + 34 - 70) * controller.getScaleXInv();
                if (f_draft < 0f) {
                    f_draft = 0;
                }
                int draft = (int)(f_draft);
                if (draft < hScroll.Minimum) {
                    draft = hScroll.Minimum;
                } else if (hScroll.Maximum < draft) {
                    draft = hScroll.Maximum;
                }
                if (hScroll.Value != draft) {
                    EditorManager.mDrawStartIndex[EditorManager.Selected - 1] = 0;
                    hScroll.Value = draft;
                }
            }
        }

        /// <summary>
        /// プレイカーソルが見えるようスクロールする
        /// </summary>
        public void ensureCursorVisible()
        {
            ensureVisible(EditorManager.getCurrentClock());
        }

        /// <summary>
        /// 特殊なショートカットキーを処理します。
        /// </summary>
        /// <param name="e"></param>
        /// <param name="onPreviewKeyDown">PreviewKeyDownイベントから送信されてきた場合、true（送る側が設定する）</param>
        public void processSpecialShortcutKey(NKeyEventArgs e, bool onPreviewKeyDown)
        {
#if DEBUG
            sout.println("FormMain#processSpecialShortcutKey");
#endif
            // 歌詞入力用のテキストボックスが表示されていたら，何もしない
            if (EditorManager.InputTextBox.Enabled) {
                EditorManager.InputTextBox.Focus();
                return;
            }

            bool flipPlaying = false; // 再生/停止状態の切り替えが要求されたらtrue

            // 最初に、特殊な取り扱いが必要なショートカット、について、
            // 該当するショートカットがあればそいつらを発動する。
            Keys stroke = (Keys) e.KeyCode | (Keys) e.Modifiers;

            if (onPreviewKeyDown && (Keys) e.KeyCode != Keys.None) {
                foreach (SpecialShortcutHolder holder in mSpecialShortcutHolders) {
                    if (stroke == holder.shortcut) {
                        try {
#if DEBUG
                            sout.println("FormMain#processSpecialShortcutKey; perform click: name=" + holder.menu.Name);
#endif
                            holder.menu.PerformClick();
                        } catch (Exception ex) {
                            Logger.write(typeof(FormMain) + ".processSpecialShortcutKey; ex=" + ex + "\n");
                            serr.println("FormMain#processSpecialShortcutKey; ex=" + ex);
                        }
                        if ((Keys) e.KeyCode == Keys.Tab) {
                            focusPianoRoll();
                        }
                        return;
                    }
                }
            }

            if ((Keys) e.Modifiers != Keys.None) {
#if DEBUG
                sout.println("FormMain#processSpecialShortcutKey; bailout with (modifier != VK_UNDEFINED)");
#endif
                return;
            }

            EditMode edit_mode = EditorManager.EditMode;

            if ((Keys) e.KeyCode == Keys.Return) {
                // MIDIステップ入力のときの処理
                if (controller.isStepSequencerEnabled()) {
                    if (EditorManager.mAddingEvent != null) {
                        fixAddingEvent();
                        EditorManager.mAddingEvent = null;
                        refreshScreen(true);
                    }
                }
            } else if ((Keys) e.KeyCode == Keys.Space) {
                if (!EditorManager.editorConfig.UseSpaceKeyAsMiddleButtonModifier) {
                    flipPlaying = true;
                }
            } else if ((Keys) e.KeyCode == Keys.OemPeriod) {
                if (!onPreviewKeyDown) {

                    if (EditorManager.isPlaying()) {
                        EditorManager.setPlaying(false, this);
                    } else {
                        VsqFileEx vsq = MusicManager.getVsqFile();
                        if (!vsq.config.StartMarkerEnabled) {
                            EditorManager.setCurrentClock(0);
                        } else {
                            EditorManager.setCurrentClock(vsq.config.StartMarker);
                        }
                        refreshScreen();
                    }
                }
            } else if ((Keys) e.KeyCode == Keys.Add || (Keys) e.KeyCode == Keys.Oemplus || (Keys) e.KeyCode == Keys.Right) {
                if (onPreviewKeyDown) {
                    forward();
                }
            } else if ((Keys) e.KeyCode == Keys.Subtract || (Keys) e.KeyCode == Keys.OemMinus || (Keys) e.KeyCode == Keys.Left) {
                if (onPreviewKeyDown) {
                    rewind();
                }
            } else if ((Keys) e.KeyCode == Keys.Escape) {
                // ステップ入力中の場合，入力中の音符をクリアする
                VsqEvent item = EditorManager.mAddingEvent;
                if (controller.isStepSequencerEnabled() && item != null) {
                    // 入力中だった音符の長さを取得し，
                    int length = item.ID.getLength();
                    EditorManager.mAddingEvent = null;
                    int clock = EditorManager.getCurrentClock();
                    int clock_draft = clock - length;
                    if (clock_draft < 0) {
                        clock_draft = 0;
                    }
                    // その分だけソングポジションを戻す．
                    EditorManager.setCurrentClock(clock_draft);
                    refreshScreen(true);
                }
            } else {
                if (!EditorManager.isPlaying()) {
                    // 最初に戻る、の機能を発動
                    Keys[] specialGoToFirst = EditorManager.editorConfig.SpecialShortcutGoToFirst;
                    if (specialGoToFirst != null && specialGoToFirst.Length > 0) {
                        Keys shortcut = specialGoToFirst.Aggregate(Keys.None, (seed, key) => seed | key);
                        if ((Keys) e.KeyCode == shortcut) {
                            EditorManager.setCurrentClock(0);
                            ensureCursorVisible();
                            refreshScreen();
                        }
                    }
                }
            }
            if (!onPreviewKeyDown && flipPlaying) {
                if (EditorManager.isPlaying()) {
                    double elapsed = PlaySound.getPosition();
                    double threshold = EditorManager.mForbidFlipPlayingThresholdSeconds;
                    if (threshold < 0) {
                        threshold = 0.0;
                    }
                    if (elapsed > threshold) {
                        timer.Stop();
                        EditorManager.setPlaying(false, this);
                    }
                } else {
                    EditorManager.setPlaying(true, this);
                }
            }
            if ((Keys) e.KeyCode == Keys.Tab) {
                focusPianoRoll();
            }
        }

        public void updateScrollRangeHorizontal()
        {
            // コンポーネントの高さが0の場合，スクロールの設定が出来ないので．
            int pwidth = pictPianoRoll.Width;
            int hwidth = hScroll.Width;
            if (pwidth <= 0 || hwidth <= 0) {
                return;
            }

            VsqFileEx vsq = MusicManager.getVsqFile();
            if (vsq == null) return;
            int l = vsq.TotalClocks;
            float scalex = controller.getScaleX();
            int key_width = EditorManager.keyWidth;
            int pict_piano_roll_width = pwidth - key_width;
            int large_change = (int)(pict_piano_roll_width / scalex);
            int maximum = (int)(l + large_change);

            int thumb_width = System.Windows.Forms.SystemInformation.HorizontalScrollBarThumbWidth;
            int box_width = (int)(large_change / (float)maximum * (hwidth - 2 * thumb_width));
            if (box_width < EditorManager.editorConfig.MinimumScrollHandleWidth) {
                box_width = EditorManager.editorConfig.MinimumScrollHandleWidth;
                if (hwidth - 2 * thumb_width > box_width) {
                    maximum = l * (hwidth - 2 * thumb_width) / (hwidth - 2 * thumb_width - box_width);
                    large_change = l * box_width / (hwidth - 2 * thumb_width - box_width);
                }
            }

            if (large_change <= 0) large_change = 1;
            if (maximum <= 0) maximum = 1;
            hScroll.LargeChange = large_change;
            hScroll.Maximum = maximum;

            int old_value = hScroll.Value;
            if (old_value > maximum - large_change) {
                hScroll.Value = maximum - large_change;
            }
        }

        public void updateScrollRangeVertical()
        {
            // コンポーネントの高さが0の場合，スクロールの設定が出来ないので．
            int pheight = pictPianoRoll.Height;
            int vheight = vScroll.Height;
            if (pheight <= 0 || vheight <= 0) {
                return;
            }

            float scaley = controller.getScaleY();

            int maximum = (int)(128 * (int)(100 * scaley) / scaley);
            int large_change = (int)(pheight / scaley);

            int thumb_height = System.Windows.Forms.SystemInformation.VerticalScrollBarThumbHeight;
            int box_height = (int)(large_change / (float)maximum * (vheight - 2 * thumb_height));
            if (box_height < EditorManager.editorConfig.MinimumScrollHandleWidth) {
                box_height = EditorManager.editorConfig.MinimumScrollHandleWidth;
                maximum = (int)(((128.0 * (int)(100 * scaley) - pheight) / scaley) * (vheight - 2 * thumb_height) / (vheight - 2 * thumb_height - box_height));
                large_change = (int)(((128.0 * (int)(100 * scaley) - pheight) / scaley) * box_height / (vheight - 2 * thumb_height - box_height));
            }

            if (large_change <= 0) large_change = 1;
            if (maximum <= 0) maximum = 1;
            vScroll.LargeChange = large_change;
            vScroll.Maximum = maximum;
            vScroll.SmallChange = 100;

            int new_value = maximum - large_change;
            if (new_value < vScroll.Minimum) {
                new_value = vScroll.Minimum;
            }
            if (vScroll.Value > new_value) {
                vScroll.Value = new_value;
            }
        }

        /// <summary>
        /// メニューのショートカットキーを、EditorManager.EditorConfig.ShorcutKeysの内容に応じて変更します
        /// </summary>
        public void applyShortcut()
        {
            mSpecialShortcutHolders.Clear();

            SortedDictionary<string, Keys[]> dict = EditorManager.editorConfig.getShortcutKeysDictionary(this.getDefaultShortcutKeys());
            #region menuStripMain
            ByRef<Object> parent = new ByRef<Object>(null);
            foreach (var key in dict.Keys) {
                if (key == "menuEditCopy" || key == "menuEditCut" || key == "menuEditPaste" || key == "SpecialShortcutGoToFirst") {
                    continue;
                }
                Object menu = searchMenuItemFromName(key, parent);
                if (menu != null) {
                    string menu_name = "";
                    if (menu is ToolStripMenuItem) {
                        menu_name = ((ToolStripMenuItem)menu).Name;
                    } else {
                        continue;
                    }
                    applyMenuItemShortcut(dict, menu, menu_name);
                }
            }
            if (dict.ContainsKey("menuEditCopy")) {
                applyMenuItemShortcut(dict, menuHiddenCopy, "menuEditCopy");
            }
            if (dict.ContainsKey("menuEditCut")) {
                applyMenuItemShortcut(dict, menuHiddenCut, "menuEditCut");
            }
            if (dict.ContainsKey("menuEditCopy")) {
                applyMenuItemShortcut(dict, menuHiddenPaste, "menuEditPaste");
            }
            #endregion

            var work = new List<ValuePair<string, UiToolStripMenuItem[]>>();
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuEditUndo", new UiToolStripMenuItem[] { cMenuPianoUndo, cMenuTrackSelectorUndo }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuEditRedo", new UiToolStripMenuItem[] { cMenuPianoRedo, cMenuTrackSelectorRedo }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuEditCut", new UiToolStripMenuItem[] { cMenuPianoCut, cMenuTrackSelectorCut, menuEditCut }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuEditCopy", new UiToolStripMenuItem[] { cMenuPianoCopy, cMenuTrackSelectorCopy, menuEditCopy }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuEditPaste", new UiToolStripMenuItem[] { cMenuPianoPaste, cMenuTrackSelectorPaste, menuEditPaste }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuEditSelectAll", new UiToolStripMenuItem[] { cMenuPianoSelectAll, cMenuTrackSelectorSelectAll }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuEditSelectAllEvents", new UiToolStripMenuItem[] { cMenuPianoSelectAllEvents }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuEditDelete", new UiToolStripMenuItem[] { menuEditDelete }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuVisualGridline", new UiToolStripMenuItem[] { cMenuPianoGrid }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuJobLyric", new UiToolStripMenuItem[] { cMenuPianoImportLyric }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuLyricExpressionProperty", new UiToolStripMenuItem[] { cMenuPianoExpressionProperty }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuLyricVibratoProperty", new UiToolStripMenuItem[] { cMenuPianoVibratoProperty }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuTrackOn", new UiToolStripMenuItem[] { cMenuTrackTabTrackOn }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuTrackAdd", new UiToolStripMenuItem[] { cMenuTrackTabAdd }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuTrackCopy", new UiToolStripMenuItem[] { cMenuTrackTabCopy }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuTrackDelete", new UiToolStripMenuItem[] { cMenuTrackTabDelete }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuTrackRenderCurrent", new UiToolStripMenuItem[] { cMenuTrackTabRenderCurrent }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuTrackRenderAll", new UiToolStripMenuItem[] { cMenuTrackTabRenderAll }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuTrackOverlay", new UiToolStripMenuItem[] { cMenuTrackTabOverlay }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuTrackRendererVOCALOID1", new UiToolStripMenuItem[] { cMenuTrackTabRendererVOCALOID1 }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuTrackRendererVOCALOID2", new UiToolStripMenuItem[] { cMenuTrackTabRendererVOCALOID2 }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuTrackRendererAquesTone", new UiToolStripMenuItem[] { menuTrackRendererAquesTone }));
            work.Add(new ValuePair<string, UiToolStripMenuItem[]>("menuTrackRendererVCNT", new UiToolStripMenuItem[] { menuTrackRendererVCNT }));
            int c = work.Count;
            for (int j = 0; j < c; j++) {
                var item = work[j];
                if (dict.ContainsKey(item.getKey())) {
                    Keys[] k = dict[item.getKey()];
                    string s = Utility.getShortcutDisplayString(k);
                    if (s != "") {
                        for (int i = 0; i < item.getValue().Length; i++) {
                            item.getValue()[i].ShortcutKeyDisplayString = s;
                        }
                    }
                }
            }

            // ミキサーウィンドウ
            if (EditorManager.MixerWindow != null) {
                if (dict.ContainsKey("menuVisualMixer")) {
                    Keys shortcut = dict["menuVisualMixer"].Aggregate(Keys.None, (seed, key) => seed | key);
                    EditorManager.MixerWindow.applyShortcut(shortcut);
                }
            }

            // アイコンパレット
            if (EditorManager.iconPalette != null) {
                if (dict.ContainsKey("menuVisualIconPalette")) {
                    Keys shortcut = dict["menuVisualIconPalette"].Aggregate(Keys.None, (seed, key) => seed | key);
                    EditorManager.iconPalette.applyShortcut(shortcut);
                }
            }

#if ENABLE_PROPERTY
            // プロパティ
            if (EditorManager.propertyWindow != null) {
                if (dict.ContainsKey(menuVisualProperty.Name)) {
                    Keys shortcut = dict[menuVisualProperty.Name].Aggregate(Keys.None, (seed, key) => seed | key);
                    EditorManager.propertyWindow.applyShortcut(shortcut);
                }
            }
#endif

            // スクリプトにショートカットを適用
            int count = menuScript.DropDownItems.Count;
            for (int i = 0; i < count; i++) {
                var tsi = menuScript.DropDownItems[i];
                if (tsi is UiToolStripMenuItem) {
                    UiToolStripMenuItem tsmi = (UiToolStripMenuItem)tsi;
                    if (tsmi.DropDownItems.Count == 1) {
                        UiToolStripItem subtsi_tsmi = tsmi.DropDownItems[0];
                        if (subtsi_tsmi is UiToolStripMenuItem) {
                            UiToolStripMenuItem dd_run = (UiToolStripMenuItem)subtsi_tsmi;
							if (dict.ContainsKey(cadencii.core2.PortUtil.getComponentName(dd_run))) {
                                applyMenuItemShortcut(dict, tsmi, cadencii.core2.PortUtil.getComponentName(tsi));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// dictの中から
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="item"></param>
        /// <param name="item_name"></param>
        /// <param name="default_shortcut"></param>
        public void applyMenuItemShortcut(SortedDictionary<string, Keys[]> dict, Object item, string item_name)
        {
            try {
                if (dict.ContainsKey(item_name)) {
#if DEBUG
                    if (!(item is ToolStripMenuItem)) {
                        throw new Exception("FormMain#applyMenuItemShortcut; item is NOT BMenuItem");
                    }
#endif // DEBUG
                    if (item is UiToolStripMenuItem) {
                        var menu = (UiToolStripMenuItem)item;
                        Keys[] keys = dict[item_name];
                        Keys shortcut = keys.Aggregate(Keys.None, (seed, key) => seed | key);

                        if (shortcut == Keys.Delete) {
                            menu.ShortcutKeyDisplayString = "Delete";
                            menu.ShortcutKeys = Keys.None;
                            mSpecialShortcutHolders.Add(new SpecialShortcutHolder(shortcut, menu));
                        } else {
                            try {
                                menu.ShortcutKeyDisplayString = "";
						menu.ShortcutKeys = shortcut;
                            } catch (Exception ex) {
                                // ショートカットの適用に失敗する→特殊な取り扱いが必要
                                menu.ShortcutKeyDisplayString = Utility.getShortcutDisplayString(keys);
                                menu.ShortcutKeys = Keys.None;
                                mSpecialShortcutHolders.Add(new SpecialShortcutHolder(shortcut, menu));
                            }
                        }
                    }
                } else {
                    if (item is UiToolStripMenuItem) {
                        ((UiToolStripMenuItem)item).ShortcutKeys = Keys.None;
                    }
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".applyMenuItemShortcut; ex=" + ex + "\n");
            }
        }

        /// <summary>
        /// ソングポジションを1小節進めます
        /// </summary>
        public void forward()
        {
            bool playing = EditorManager.isPlaying();
            if (playing) {
                return;
            }
            VsqFileEx vsq = MusicManager.getVsqFile();
            if (vsq == null) {
                return;
            }
            int cl_clock = EditorManager.getCurrentClock();
            int unit = QuantizeModeUtil.getQuantizeClock(
                EditorManager.editorConfig.getPositionQuantize(),
                EditorManager.editorConfig.isPositionQuantizeTriplet());
            int cl_new = FormMainModel.Quantize(cl_clock + unit, unit);

            if (cl_new <= hScroll.Maximum + (pictPianoRoll.Width - EditorManager.keyWidth) * controller.getScaleXInv()) {
                // 表示の更新など
                EditorManager.setCurrentClock(cl_new);

                // ステップ入力時の処理
                updateNoteLengthStepSequencer();

                ensureCursorVisible();
                EditorManager.setPlaying(playing, this);
                refreshScreen();
            }
        }

        /// <summary>
        /// ソングポジションを1小節戻します
        /// </summary>
        public void rewind()
        {
            bool playing = EditorManager.isPlaying();
            if (playing) {
                return;
            }
            VsqFileEx vsq = MusicManager.getVsqFile();
            if (vsq == null) {
                return;
            }
            int cl_clock = EditorManager.getCurrentClock();
            int unit = QuantizeModeUtil.getQuantizeClock(
                EditorManager.editorConfig.getPositionQuantize(),
                EditorManager.editorConfig.isPositionQuantizeTriplet());
            int cl_new = FormMainModel.Quantize(cl_clock - unit, unit);
            if (cl_new < 0) {
                cl_new = 0;
            }

            EditorManager.setCurrentClock(cl_new);

            // ステップ入力時の処理
            updateNoteLengthStepSequencer();

            ensureCursorVisible();
            EditorManager.setPlaying(playing, this);
            refreshScreen();
        }

        /// <summary>
        /// cMenuPianoの固定長音符入力の各メニューのチェック状態をm_pencil_modeを元に更新します
        /// </summary>
        public void updateCMenuPianoFixed()
        {
            cMenuPianoFixed01.Checked = false;
            cMenuPianoFixed02.Checked = false;
            cMenuPianoFixed04.Checked = false;
            cMenuPianoFixed08.Checked = false;
            cMenuPianoFixed16.Checked = false;
            cMenuPianoFixed32.Checked = false;
            cMenuPianoFixed64.Checked = false;
            cMenuPianoFixed128.Checked = false;
            cMenuPianoFixedOff.Checked = false;
            cMenuPianoFixedTriplet.Checked = false;
            cMenuPianoFixedDotted.Checked = false;
            PencilModeEnum mode = mPencilMode.getMode();
            if (mode == PencilModeEnum.L1) {
                cMenuPianoFixed01.Checked = true;
            } else if (mode == PencilModeEnum.L2) {
                cMenuPianoFixed02.Checked = true;
            } else if (mode == PencilModeEnum.L4) {
                cMenuPianoFixed04.Checked = true;
            } else if (mode == PencilModeEnum.L8) {
                cMenuPianoFixed08.Checked = true;
            } else if (mode == PencilModeEnum.L16) {
                cMenuPianoFixed16.Checked = true;
            } else if (mode == PencilModeEnum.L32) {
                cMenuPianoFixed32.Checked = true;
            } else if (mode == PencilModeEnum.L64) {
                cMenuPianoFixed64.Checked = true;
            } else if (mode == PencilModeEnum.L128) {
                cMenuPianoFixed128.Checked = true;
            } else if (mode == PencilModeEnum.Off) {
                cMenuPianoFixedOff.Checked = true;
            }
            cMenuPianoFixedTriplet.Checked = mPencilMode.isTriplet();
            cMenuPianoFixedDotted.Checked = mPencilMode.isDot();
        }

        /// <summary>
        /// 鍵盤音キャッシュの中から指定したノートナンバーの音源を捜し、再生します。
        /// </summary>
        /// <param name="note">再生する音の高さを指定するノートナンバー</param>
        public void playPreviewSound(int note)
        {
            KeySoundPlayer.play(note);
        }

#if ENABLE_MOUSEHOVER
        public void MouseHoverEventGenerator( Object arg ) {
            int note = (int)arg;
            if ( EditorManager.editorConfig.MouseHoverTime > 0 ) {
                Thread.Sleep( EditorManager.editorConfig.MouseHoverTime );
            }
            KeySoundPlayer.play( note );
        }
#endif

        /// <summary>
        /// このコンポーネントの表示言語を、現在の言語設定に従って更新します。
        /// </summary>
        public void applyLanguage()
        {
            openXmlVsqDialog.Filter = string.Empty;
            try {
                openXmlVsqDialog.Filter = string.Join("|", new[] { _("XML-VSQ Format(*.xvsq)|*.xvsq"), _("All Files(*.*)|*.*") });
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".applyLanguage; ex=" + ex + "\n");
                openXmlVsqDialog.Filter = string.Join("|", new[] { "XML-VSQ Format(*.xvsq)|*.xvsq", "All Files(*.*)|*.*" });
            }

            saveXmlVsqDialog.Filter = string.Empty;
            try {
                saveXmlVsqDialog.Filter = string.Join("|", new[] { _("XML-VSQ Format(*.xvsq)|*.xvsq"), _("All Files(*.*)|*.*") });
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".applyLanguage; ex=" + ex + "\n");
                saveXmlVsqDialog.Filter = string.Join("|", new[] { "XML-VSQ Format(*.xvsq)|*.xvsq", "All Files(*.*)|*.*" });
            }

            openUstDialog.Filter = string.Empty;
            try {
                openUstDialog.Filter = string.Join("|", new[] { _("UTAU Script Format(*.ust)|*.ust"), _("All Files(*.*)|*.*") });
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".applyLanguage; ex=" + ex + "\n");
                openUstDialog.Filter = string.Join("|", new[] { "UTAU Script Format(*.ust)|*.ust", "All Files(*.*)|*.*" });
            }

            openMidiDialog.Filter = string.Empty;
            try {
                openMidiDialog.Filter = string.Join("|", new[] {
                    _( "MIDI Format(*.mid)|*.mid" ),
                    _( "VSQ Format(*.vsq)|*.vsq" ),
                    _( "VSQX Format(*.vsqx)|*.vsqx" ),
                    _( "All Files(*.*)|*.*" ) });
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".applyLanguage; ex=" + ex + "\n");
                openMidiDialog.Filter = string.Join("|", new[] {
                    "MIDI Format(*.mid)|*.mid",
                    "VSQ Format(*.vsq)|*.vsq",
                    "VSQX Format(*.vsqx)|*.vsqx",
                    "All Files(*.*)|*.*" });
            }

            saveMidiDialog.Filter = string.Empty;
            try {
                saveMidiDialog.Filter = string.Join("|", new[] {
                    _( "MIDI Format(*.mid)|*.mid" ),
                    _( "VSQ Format(*.vsq)|*.vsq" ),
                    _( "All Files(*.*)|*.*" ) });
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".applyLanguage; ex=" + ex + "\n");
                saveMidiDialog.Filter = string.Join("|", new[] {
                    "MIDI Format(*.mid)|*.mid",
                    "VSQ Format(*.vsq)|*.vsq",
                    "All Files(*.*)|*.*" });
            }

            openWaveDialog.Filter = string.Empty;
            try {
                openWaveDialog.Filter = string.Join("|", new[] {
                    _( "Wave File(*.wav)|*.wav" ),
                    _( "All Files(*.*)|*.*" ) });
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".applyLanguage; ex=" + ex + "\n");
                openWaveDialog.Filter = string.Join("|", new[] {
                    "Wave File(*.wav)|*.wav",
                    "All Files(*.*)|*.*" });
            }

            stripLblGameCtrlMode.ToolTipText = _("Game controler");

            this.Invoke(new EventHandler(updateGameControlerStatus));

            stripBtnPointer.Text = _("Pointer");
            stripBtnPointer.ToolTipText = _("Pointer");
            stripBtnPencil.Text = _("Pencil");
            stripBtnPencil.ToolTipText = _("Pencil");
            stripBtnLine.Text = _("Line");
            stripBtnLine.ToolTipText = _("Line");
            stripBtnEraser.Text = _("Eraser");
            stripBtnEraser.ToolTipText = _("Eraser");
            stripBtnCurve.Text = _("Curve");
            stripBtnCurve.ToolTipText = _("Curve");
            stripBtnGrid.Text = _("Grid");
            stripBtnGrid.ToolTipText = _("Grid");
            if (EditorManager.isPlaying()) {
                stripBtnPlay.Text = _("Stop");
            } else {
                stripBtnPlay.Text = _("Play");
            }

            stripBtnMoveTop.ToolTipText = _("Move to beginning measure");
            stripBtnMoveEnd.ToolTipText = _("Move to end measure");
            stripBtnForward.ToolTipText = _("Move forward");
            stripBtnRewind.ToolTipText = _("Move backwared");
            stripBtnLoop.ToolTipText = _("Repeat");
            stripBtnScroll.ToolTipText = _("Auto scroll");

            #region main menu
            menuFile.Text = _("File");
            menuFile.Mnemonic(Keys.F);
            menuFileNew.Text = _("New");
            menuFileNew.Mnemonic(Keys.N);
            menuFileOpen.Text = _("Open");
            menuFileOpen.Mnemonic(Keys.O);
            menuFileOpenVsq.Text = _("Open VSQX/VSQ/Vocaloid MIDI");
            menuFileOpenVsq.Mnemonic(Keys.V);
            menuFileOpenUst.Text = _("Open UTAU project file");
            menuFileOpenUst.Mnemonic(Keys.U);
            menuFileSave.Text = _("Save");
            menuFileSave.Mnemonic(Keys.S);
            menuFileSaveNamed.Text = _("Save as");
            menuFileSaveNamed.Mnemonic(Keys.A);
            menuFileImport.Text = _("Import");
            menuFileImport.Mnemonic(Keys.I);
            menuFileImportVsq.Text = _("VSQ / Vocaloid Midi");
            menuFileExport.Text = _("Export");
            menuFileExport.Mnemonic(Keys.E);
            menuFileExportWave.Text = _("WAVE");
            menuFileExportParaWave.Text = _("Serial numbered WAVEs");
            menuFileExportUst.Text = _("UTAU project file");
            menuFileExportVxt.Text = _("Metatext for vConnect");
            menuFileExportVsq.Text = _("VSQ File");
            menuFileExportVsqx.Text = _("VSQX File");
            menuFileRecent.Text = _("Open Recent");
            menuFileRecent.Mnemonic(Keys.R);
            menuFileRecentClear.Text = _("Clear Menu");
            menuFileQuit.Text = _("Quit");
            menuFileQuit.Mnemonic(Keys.Q);

            menuEdit.Text = _("Edit");
            menuEdit.Mnemonic(Keys.E);
            menuEditUndo.Text = _("Undo");
            menuEditUndo.Mnemonic(Keys.U);
            menuEditRedo.Text = _("Redo");
            menuEditRedo.Mnemonic(Keys.R);
            menuEditCut.Text = _("Cut");
            menuEditCut.Mnemonic(Keys.T);
            menuEditCopy.Text = _("Copy");
            menuEditCopy.Mnemonic(Keys.C);
            menuEditPaste.Text = _("Paste");
            menuEditPaste.Mnemonic(Keys.P);
            menuEditDelete.Text = _("Delete");
            menuEditDelete.Mnemonic(Keys.D);
            menuEditAutoNormalizeMode.Text = _("Auto normalize mode");
            menuEditAutoNormalizeMode.Mnemonic(Keys.N);
            menuEditSelectAll.Text = _("Select All");
            menuEditSelectAll.Mnemonic(Keys.A);
            menuEditSelectAllEvents.Text = _("Select all events");
            menuEditSelectAllEvents.Mnemonic(Keys.E);

            menuVisual.Text = _("View");
            menuVisual.Mnemonic(Keys.V);
            menuVisualControlTrack.Text = _("Control track");
            menuVisualControlTrack.Mnemonic(Keys.C);
            menuVisualMixer.Text = _("Mixer");
            menuVisualMixer.Mnemonic(Keys.X);
            menuVisualWaveform.Text = _("Waveform");
            menuVisualWaveform.Mnemonic(Keys.W);
            menuVisualProperty.Text = _("Property window");
            menuVisualOverview.Text = _("Navigation");
            menuVisualOverview.Mnemonic(Keys.V);
            menuVisualGridline.Text = _("Grid line");
            menuVisualGridline.Mnemonic(Keys.G);
            menuVisualStartMarker.Text = _("Start marker");
            menuVisualStartMarker.Mnemonic(Keys.S);
            menuVisualEndMarker.Text = _("End marker");
            menuVisualEndMarker.Mnemonic(Keys.E);
            menuVisualLyrics.Text = _("Lyrics/Phoneme");
            menuVisualLyrics.Mnemonic(Keys.L);
            menuVisualNoteProperty.Text = _("Note expression/vibrato");
            menuVisualNoteProperty.Mnemonic(Keys.N);
            menuVisualPitchLine.Text = _("Pitch line");
            menuVisualPitchLine.Mnemonic(Keys.P);
            menuVisualPluginUi.Text = _("VSTi plugin UI");
            menuVisualPluginUi.Mnemonic(Keys.U);
            menuVisualIconPalette.Text = _("Icon palette");
            menuVisualIconPalette.Mnemonic(Keys.I);

            menuJob.Text = _("Job");
            menuJob.Mnemonic(Keys.J);
            menuJobNormalize.Text = _("Normalize notes");
            menuJobNormalize.Mnemonic(Keys.N);
            menuJobInsertBar.Text = _("Insert bars");
            menuJobInsertBar.Mnemonic(Keys.I);
            menuJobDeleteBar.Text = _("Delete bars");
            menuJobDeleteBar.Mnemonic(Keys.D);
            menuJobRandomize.Text = _("Randomize");
            menuJobRandomize.Mnemonic(Keys.R);
            menuJobConnect.Text = _("Connect notes");
            menuJobConnect.Mnemonic(Keys.C);
            menuJobLyric.Text = _("Insert lyrics");
            menuJobLyric.Mnemonic(Keys.L);

            menuTrack.Text = _("Track");
            menuTrack.Mnemonic(Keys.T);
            menuTrackOn.Text = _("Track on");
            menuTrackOn.Mnemonic(Keys.K);
            menuTrackAdd.Text = _("Add track");
            menuTrackAdd.Mnemonic(Keys.A);
            menuTrackCopy.Text = _("Copy track");
            menuTrackCopy.Mnemonic(Keys.C);
            menuTrackChangeName.Text = _("Rename track");
            menuTrackDelete.Text = _("Delete track");
            menuTrackDelete.Mnemonic(Keys.D);
            menuTrackRenderCurrent.Text = _("Render current track");
            menuTrackRenderCurrent.Mnemonic(Keys.T);
            menuTrackRenderAll.Text = _("Render all tracks");
            menuTrackRenderAll.Mnemonic(Keys.S);
            menuTrackOverlay.Text = _("Overlay");
            menuTrackOverlay.Mnemonic(Keys.O);
            menuTrackRenderer.Text = _("Renderer");
            menuTrackRenderer.Mnemonic(Keys.R);
            menuTrackRendererVOCALOID1.Mnemonic(Keys.D1);
            menuTrackRendererVOCALOID2.Mnemonic(Keys.D3);
            menuTrackRendererUtau.Mnemonic(Keys.D4);
            menuTrackRendererVCNT.Mnemonic(Keys.D5);
            menuTrackRendererAquesTone.Mnemonic(Keys.D6);

            menuLyric.Text = _("Lyrics");
            menuLyric.Mnemonic(Keys.L);
            menuLyricExpressionProperty.Text = _("Note expression property");
            menuLyricExpressionProperty.Mnemonic(Keys.E);
            menuLyricVibratoProperty.Text = _("Note vibrato property");
            menuLyricVibratoProperty.Mnemonic(Keys.V);
            menuLyricApplyUtauParameters.Text = _("Apply UTAU Parameters");
            menuLyricApplyUtauParameters.Mnemonic(Keys.A);
            menuLyricPhonemeTransformation.Text = _("Phoneme transformation");
            menuLyricPhonemeTransformation.Mnemonic(Keys.T);
            menuLyricDictionary.Text = _("User word dictionary");
            menuLyricDictionary.Mnemonic(Keys.C);
            menuLyricCopyVibratoToPreset.Text = _("Copy vibrato config to preset");
            menuLyricCopyVibratoToPreset.Mnemonic(Keys.P);

            menuScript.Text = _("Script");
            menuScript.Mnemonic(Keys.C);
            menuScriptUpdate.Text = _("Update script list");
            menuScriptUpdate.Mnemonic(Keys.U);

            menuSetting.Text = _("Setting");
            menuSetting.Mnemonic(Keys.S);
            menuSettingPreference.Text = _("Preference");
            menuSettingPreference.Mnemonic(Keys.P);
            menuSettingGameControler.Text = _("Game controler");
            menuSettingGameControler.Mnemonic(Keys.G);
            menuSettingGameControlerLoad.Text = _("Load");
            menuSettingGameControlerLoad.Mnemonic(Keys.L);
            menuSettingGameControlerRemove.Text = _("Remove");
            menuSettingGameControlerRemove.Mnemonic(Keys.R);
            menuSettingGameControlerSetting.Text = _("Setting");
            menuSettingGameControlerSetting.Mnemonic(Keys.S);
            menuSettingSequence.Text = _("Sequence config");
            menuSettingSequence.Mnemonic(Keys.S);
            menuSettingShortcut.Text = _("Shortcut key");
            menuSettingShortcut.Mnemonic(Keys.K);
            menuSettingDefaultSingerStyle.Text = _("Singing style defaults");
            menuSettingDefaultSingerStyle.Mnemonic(Keys.D);
            menuSettingPositionQuantize.Text = _("Quantize");
            menuSettingPositionQuantize.Mnemonic(Keys.Q);
            menuSettingPositionQuantizeOff.Text = _("Off");
            menuSettingPositionQuantizeTriplet.Text = _("Triplet");
            //menuSettingSingerProperty.setText( _( "Singer Properties" ) );
            //menuSettingSingerProperty.setMnemonic( Keys.S );
            menuSettingPaletteTool.Text = _("Palette Tool");
            menuSettingPaletteTool.Mnemonic(Keys.T);
            menuSettingVibratoPreset.Text = _("Vibrato preset");
            menuSettingVibratoPreset.Mnemonic(Keys.V);

            menuTools.Text = _("Tools");
            menuTools.Mnemonic(Keys.O);
            menuToolsCreateVConnectSTANDDb.Text = _("Create vConnect-STAND DB");

            menuHelp.Text = _("Help");
            menuHelp.Mnemonic(Keys.H);
            menuHelpCheckForUpdates.Text = _("Check For Updates");
            menuHelpLog.Text = _("Log");
            menuHelpLog.Mnemonic(Keys.L);
            menuHelpLogSwitch.Text = Logger.isEnabled() ? _("Disable") : _("Enable");
            menuHelpLogSwitch.Mnemonic(Keys.L);
            menuHelpLogOpen.Text = _("Open");
            menuHelpLogOpen.Mnemonic(Keys.O);
            menuHelpAbout.Text = _("About Cadencii");
            menuHelpAbout.Mnemonic(Keys.A);
            menuHelpManual.Text = _("Manual") + " (PDF)";

            menuHiddenCopy.Text = _("Copy");
            menuHiddenCut.Text = _("Cut");
            menuHiddenEditFlipToolPointerEraser.Text = _("Chagne tool pointer / eraser");
            menuHiddenEditFlipToolPointerPencil.Text = _("Change tool pointer / pencil");
            menuHiddenEditLyric.Text = _("Start lyric input");
            menuHiddenGoToEndMarker.Text = _("GoTo end marker");
            menuHiddenGoToStartMarker.Text = _("Goto start marker");
            menuHiddenLengthen.Text = _("Lengthen");
            menuHiddenMoveDown.Text = _("Move down");
            menuHiddenMoveLeft.Text = _("Move left");
            menuHiddenMoveRight.Text = _("Move right");
            menuHiddenMoveUp.Text = _("Move up");
            menuHiddenPaste.Text = _("Paste");
            menuHiddenPlayFromStartMarker.Text = _("Play from start marker");
            menuHiddenSelectBackward.Text = _("Select backward");
            menuHiddenSelectForward.Text = _("Select forward");
            menuHiddenShorten.Text = _("Shorten");
            menuHiddenTrackBack.Text = _("Previous track");
            menuHiddenTrackNext.Text = _("Next track");
            menuHiddenVisualBackwardParameter.Text = _("Previous control curve");
            menuHiddenVisualForwardParameter.Text = _("Next control curve");
            menuHiddenFlipCurveOnPianorollMode.Text = _("Change pitch drawing mode");
            #endregion

            #region cMenuPiano
            cMenuPianoPointer.Text = _("Arrow");
            cMenuPianoPointer.Mnemonic(Keys.A);
            cMenuPianoPencil.Text = _("Pencil");
            cMenuPianoPencil.Mnemonic(Keys.W);
            cMenuPianoEraser.Text = _("Eraser");
            cMenuPianoEraser.Mnemonic(Keys.E);
            cMenuPianoPaletteTool.Text = _("Palette Tool");

            cMenuPianoCurve.Text = _("Curve");
            cMenuPianoCurve.Mnemonic(Keys.V);

            cMenuPianoFixed.Text = _("Note Fixed Length");
            cMenuPianoFixed.Mnemonic(Keys.N);
            cMenuPianoFixedTriplet.Text = _("Triplet");
            cMenuPianoFixedOff.Text = _("Off");
            cMenuPianoFixedDotted.Text = _("Dot");
            cMenuPianoQuantize.Text = _("Quantize");
            cMenuPianoQuantize.Mnemonic(Keys.Q);
            cMenuPianoQuantizeTriplet.Text = _("Triplet");
            cMenuPianoQuantizeOff.Text = _("Off");
            cMenuPianoGrid.Text = _("Show/Hide Grid Line");
            cMenuPianoGrid.Mnemonic(Keys.S);

            cMenuPianoUndo.Text = _("Undo");
            cMenuPianoUndo.Mnemonic(Keys.U);
            cMenuPianoRedo.Text = _("Redo");
            cMenuPianoRedo.Mnemonic(Keys.R);

            cMenuPianoCut.Text = _("Cut");
            cMenuPianoCut.Mnemonic(Keys.T);
            cMenuPianoPaste.Text = _("Paste");
            cMenuPianoPaste.Mnemonic(Keys.P);
            cMenuPianoCopy.Text = _("Copy");
            cMenuPianoCopy.Mnemonic(Keys.C);
            cMenuPianoDelete.Text = _("Delete");
            cMenuPianoDelete.Mnemonic(Keys.D);

            cMenuPianoSelectAll.Text = _("Select All");
            cMenuPianoSelectAll.Mnemonic(Keys.A);
            cMenuPianoSelectAllEvents.Text = _("Select All Events");
            cMenuPianoSelectAllEvents.Mnemonic(Keys.E);

            cMenuPianoExpressionProperty.Text = _("Note Expression Property");
            cMenuPianoExpressionProperty.Mnemonic(Keys.P);
            cMenuPianoVibratoProperty.Text = _("Note Vibrato Property");
            cMenuPianoImportLyric.Text = _("Insert Lyrics");
            cMenuPianoImportLyric.Mnemonic(Keys.P);
            #endregion

            #region cMenuTrackTab
            cMenuTrackTabTrackOn.Text = _("Track On");
            cMenuTrackTabTrackOn.Mnemonic(Keys.K);
            cMenuTrackTabAdd.Text = _("Add Track");
            cMenuTrackTabAdd.Mnemonic(Keys.A);
            cMenuTrackTabCopy.Text = _("Copy Track");
            cMenuTrackTabCopy.Mnemonic(Keys.C);
            cMenuTrackTabChangeName.Text = _("Rename Track");
            cMenuTrackTabDelete.Text = _("Delete Track");
            cMenuTrackTabDelete.Mnemonic(Keys.D);

            cMenuTrackTabRenderCurrent.Text = _("Render Current Track");
            cMenuTrackTabRenderCurrent.Mnemonic(Keys.T);
            cMenuTrackTabRenderAll.Text = _("Render All Tracks");
            cMenuTrackTabRenderAll.Mnemonic(Keys.S);
            cMenuTrackTabOverlay.Text = _("Overlay");
            cMenuTrackTabOverlay.Mnemonic(Keys.O);
            cMenuTrackTabRenderer.Text = _("Renderer");
            cMenuTrackTabRenderer.Mnemonic(Keys.R);
            #endregion

            #region cMenuTrackSelector
            cMenuTrackSelectorPointer.Text = _("Arrow");
            cMenuTrackSelectorPointer.Mnemonic(Keys.A);
            cMenuTrackSelectorPencil.Text = _("Pencil");
            cMenuTrackSelectorPencil.Mnemonic(Keys.W);
            cMenuTrackSelectorLine.Text = _("Line");
            cMenuTrackSelectorLine.Mnemonic(Keys.L);
            cMenuTrackSelectorEraser.Text = _("Eraser");
            cMenuTrackSelectorEraser.Mnemonic(Keys.E);
            cMenuTrackSelectorPaletteTool.Text = _("Palette Tool");

            cMenuTrackSelectorCurve.Text = _("Curve");
            cMenuTrackSelectorCurve.Mnemonic(Keys.V);

            cMenuTrackSelectorUndo.Text = _("Undo");
            cMenuTrackSelectorUndo.Mnemonic(Keys.U);
            cMenuTrackSelectorRedo.Text = _("Redo");
            cMenuTrackSelectorRedo.Mnemonic(Keys.R);

            cMenuTrackSelectorCut.Text = _("Cut");
            cMenuTrackSelectorCut.Mnemonic(Keys.T);
            cMenuTrackSelectorCopy.Text = _("Copy");
            cMenuTrackSelectorCopy.Mnemonic(Keys.C);
            cMenuTrackSelectorPaste.Text = _("Paste");
            cMenuTrackSelectorPaste.Mnemonic(Keys.P);
            cMenuTrackSelectorDelete.Text = _("Delete");
            cMenuTrackSelectorDelete.Mnemonic(Keys.D);
            cMenuTrackSelectorDeleteBezier.Text = _("Delete Bezier Point");
            cMenuTrackSelectorDeleteBezier.Mnemonic(Keys.B);

            cMenuTrackSelectorSelectAll.Text = _("Select All Events");
            cMenuTrackSelectorSelectAll.Mnemonic(Keys.E);
            #endregion

            #region cMenuPositionIndicator
            cMenuPositionIndicatorStartMarker.Text = _("Set start marker");
            cMenuPositionIndicatorEndMarker.Text = _("Set end marker");
            #endregion

            stripLblGameCtrlMode.ToolTipText = _("Game Controler");

            // Palette Tool
#if DEBUG
            CDebug.WriteLine("FormMain#applyLanguage; Messaging.Language=" + Messaging.getLanguage());
#endif
#if ENABLE_SCRIPT
            int count = toolBarTool.Buttons.Count;// toolStripTool.getComponentCount();
            for (int i = 0; i < count; i++) {
                Object tsi = toolBarTool.Buttons[i];// toolStripTool.getComponentAtIndex( i );
                if (tsi is UiToolBarButton) {
                    UiToolBarButton tsb = (UiToolBarButton)tsi;
                    if (tsb.Style == Cadencii.Gui.ToolBarButtonStyle.ToggleButton && tsb.Tag != null && tsb.Tag is string) {
                        string id = (string)tsb.Tag;
                        if (PaletteToolServer.loadedTools.ContainsKey(id)) {
                            IPaletteTool ipt = (IPaletteTool)PaletteToolServer.loadedTools[id];
                            tsb.Text = ipt.getName(Messaging.getLanguage());
                            tsb.ToolTipText = ipt.getDescription(Messaging.getLanguage());
                        }
                    }
                }
            }

            foreach (var tsi in cMenuPianoPaletteTool.DropDownItems) {
                if (tsi is UiToolStripMenuItem) {
                    var tsmi = (UiToolStripMenuItem)tsi;
                    if (tsmi.Tag != null && tsmi.Tag is string) {
                        string id = (string)tsmi.Tag;
                        if (PaletteToolServer.loadedTools.ContainsKey(id)) {
                            IPaletteTool ipt = (IPaletteTool)PaletteToolServer.loadedTools[id];
                            tsmi.Text = ipt.getName(Messaging.getLanguage());
                            tsmi.ToolTipText = ipt.getDescription(Messaging.getLanguage());
                        }
                    }
                }
            }

            foreach (var tsi in cMenuTrackSelectorPaletteTool.DropDownItems) {
                if (tsi is UiToolStripMenuItem) {
                    var tsmi = (UiToolStripMenuItem)tsi;
                    if (tsmi.Tag != null && tsmi.Tag is string) {
                        string id = (string)tsmi.Tag;
                        if (PaletteToolServer.loadedTools.ContainsKey(id)) {
                            IPaletteTool ipt = (IPaletteTool)PaletteToolServer.loadedTools[id];
                            tsmi.Text = ipt.getName(Messaging.getLanguage());
                            tsmi.ToolTipText = ipt.getDescription(Messaging.getLanguage());
                        }
                    }
                }
            }

            foreach (var tsi in menuSettingPaletteTool.DropDownItems) {
                if (tsi is UiToolStripMenuItem) {
                    var tsmi = (UiToolStripMenuItem)tsi;
                    if (tsmi.Tag != null && tsmi.Tag is string) {
                        string id = (string)tsmi.Tag;
                        if (PaletteToolServer.loadedTools.ContainsKey(id)) {
                            IPaletteTool ipt = (IPaletteTool)PaletteToolServer.loadedTools[id];
                            tsmi.Text = ipt.getName(Messaging.getLanguage());
                        }
                    }
                }
            }

            foreach (var id in PaletteToolServer.loadedTools.Keys) {
                IPaletteTool ipt = (IPaletteTool)PaletteToolServer.loadedTools[id];
                ipt.applyLanguage(Messaging.getLanguage());
            }
#endif
        }

        /// <summary>
        /// マウスのスクロールによって受け取ったスクロール幅から、実際に縦スクロールバーに渡す値(候補値)を計算します。
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public int computeScrollValueFromWheelDelta(int delta)
        {
            double new_val = (double)hScroll.Value - delta * EditorManager.editorConfig.WheelOrder / (5.0 * controller.getScaleX());
            if (new_val < 0.0) {
                new_val = 0;
            }
            int max = hScroll.Maximum - hScroll.LargeChange;
            int draft = (int)new_val;
            if (draft > max) {
                draft = max;
            } else if (draft < hScroll.Minimum) {
                draft = hScroll.Minimum;
            }
            return draft;
        }

        /// <summary>
        /// length, positionの各Quantizeモードに応じて、
        /// 関連する全てのメニュー・コンテキストメニューの表示状態を更新します。
        /// </summary>
        public void applyQuantizeMode()
        {
            cMenuPianoQuantize04.Checked = false;
            cMenuPianoQuantize08.Checked = false;
            cMenuPianoQuantize16.Checked = false;
            cMenuPianoQuantize32.Checked = false;
            cMenuPianoQuantize64.Checked = false;
            cMenuPianoQuantize128.Checked = false;
            cMenuPianoQuantizeOff.Checked = false;

#if ENABLE_STRIP_DROPDOWN
            stripDDBtnQuantize04.Checked = false;
            stripDDBtnQuantize08.Checked = false;
            stripDDBtnQuantize16.Checked = false;
            stripDDBtnQuantize32.Checked = false;
            stripDDBtnQuantize64.Checked = false;
            stripDDBtnQuantize128.Checked = false;
            stripDDBtnQuantizeOff.Checked = false;
#endif

            menuSettingPositionQuantize04.Checked = false;
            menuSettingPositionQuantize08.Checked = false;
            menuSettingPositionQuantize16.Checked = false;
            menuSettingPositionQuantize32.Checked = false;
            menuSettingPositionQuantize64.Checked = false;
            menuSettingPositionQuantize128.Checked = false;
            menuSettingPositionQuantizeOff.Checked = false;

            QuantizeMode qm = EditorManager.editorConfig.getPositionQuantize();
            bool triplet = EditorManager.editorConfig.isPositionQuantizeTriplet();
            stripDDBtnQuantizeParent.Text =
                "QUANTIZE " + QuantizeModeUtil.getString(qm) +
                ((qm != QuantizeMode.off && triplet) ? " [3]" : "");
            if (EditorManager.editorConfig.getPositionQuantize() == QuantizeMode.p4) {
                cMenuPianoQuantize04.Checked = true;
#if ENABLE_STRIP_DROPDOWN
                stripDDBtnQuantize04.Checked = true;
#endif
                stripDDBtnQuantizeParent.ImageKey = "note004.png";
                menuSettingPositionQuantize04.Checked = true;
            } else if (EditorManager.editorConfig.getPositionQuantize() == QuantizeMode.p8) {
                cMenuPianoQuantize08.Checked = true;
#if ENABLE_STRIP_DROPDOWN
                stripDDBtnQuantize08.Checked = true;
#endif
                stripDDBtnQuantizeParent.ImageKey = "note008.png";
                menuSettingPositionQuantize08.Checked = true;
            } else if (EditorManager.editorConfig.getPositionQuantize() == QuantizeMode.p16) {
                cMenuPianoQuantize16.Checked = true;
#if ENABLE_STRIP_DROPDOWN
                stripDDBtnQuantize16.Checked = true;
#endif
                stripDDBtnQuantizeParent.ImageKey = "note016.png";
                menuSettingPositionQuantize16.Checked = true;
            } else if (EditorManager.editorConfig.getPositionQuantize() == QuantizeMode.p32) {
                cMenuPianoQuantize32.Checked = true;
#if ENABLE_STRIP_DROPDOWN
                stripDDBtnQuantize32.Checked = true;
#endif
                stripDDBtnQuantizeParent.ImageKey = "note032.png";
                menuSettingPositionQuantize32.Checked = true;
            } else if (EditorManager.editorConfig.getPositionQuantize() == QuantizeMode.p64) {
                cMenuPianoQuantize64.Checked = true;
#if ENABLE_STRIP_DROPDOWN
                stripDDBtnQuantize64.Checked = true;
#endif
                stripDDBtnQuantizeParent.ImageKey = "note064.png";
                menuSettingPositionQuantize64.Checked = true;
            } else if (EditorManager.editorConfig.getPositionQuantize() == QuantizeMode.p128) {
                cMenuPianoQuantize128.Checked = true;
#if ENABLE_STRIP_DROPDOWN
                stripDDBtnQuantize128.Checked = true;
#endif
                stripDDBtnQuantizeParent.ImageKey = "note128.png";
                menuSettingPositionQuantize128.Checked = true;
            } else if (EditorManager.editorConfig.getPositionQuantize() == QuantizeMode.off) {
                cMenuPianoQuantizeOff.Checked = true;
#if ENABLE_STRIP_DROPDOWN
                stripDDBtnQuantizeOff.Checked = true;
#endif
                stripDDBtnQuantizeParent.ImageKey = "notenull.png";
                menuSettingPositionQuantizeOff.Checked = true;
            }
            cMenuPianoQuantizeTriplet.Checked = EditorManager.editorConfig.isPositionQuantizeTriplet();
#if ENABLE_STRIP_DROPDOWN
            stripDDBtnQuantizeTriplet.Checked = EditorManager.editorConfig.isPositionQuantizeTriplet();
#endif
            menuSettingPositionQuantizeTriplet.Checked = EditorManager.editorConfig.isPositionQuantizeTriplet();
        }

        /// <summary>
        /// 現在選択されている編集ツールに応じて、メニューのチェック状態を更新します
        /// </summary>
        public void applySelectedTool()
        {
            EditTool tool = EditorManager.SelectedTool;

            int count = toolBarTool.Buttons.Count;
            for (int i = 0; i < count; i++) {
                Object tsi = toolBarTool.Buttons[i];
                if (tsi is UiToolBarButton) {
                    UiToolBarButton tsb = (UiToolBarButton)tsi;
                    Object tag = tsb.Tag;
                    if (tsb.Style == Cadencii.Gui.ToolBarButtonStyle.ToggleButton && tag != null && tag is string) {
#if ENABLE_SCRIPT
                        if (tool == EditTool.PALETTE_TOOL) {
                            string id = (string)tag;
                            tsb.Pushed = (EditorManager.mSelectedPaletteTool == id);
                        } else
#endif // ENABLE_SCRIPT
 {
                            tsb.Pushed = false;
                        }
                    }
                }
            }
            foreach (var tsi in cMenuTrackSelectorPaletteTool.DropDownItems) {
                if (tsi is PaletteToolMenuItem) {
                    PaletteToolMenuItem tsmi = (PaletteToolMenuItem)tsi;
                    string id = tsmi.getPaletteToolID();
                    bool sel = false;
#if ENABLE_SCRIPT
                    if (tool == EditTool.PALETTE_TOOL) {
                        sel = (EditorManager.mSelectedPaletteTool == id);
                    }
#endif
                    tsmi.Checked = sel;
                }
            }

            foreach (var tsi in cMenuPianoPaletteTool.DropDownItems) {
                if (tsi is PaletteToolMenuItem) {
                    PaletteToolMenuItem tsmi = (PaletteToolMenuItem)tsi;
                    string id = tsmi.getPaletteToolID();
                    bool sel = false;
#if ENABLE_SCRIPT
                    if (tool == EditTool.PALETTE_TOOL) {
                        sel = (EditorManager.mSelectedPaletteTool == id);
                    }
#endif
                    tsmi.Checked = sel;
                }
            }

            EditTool selected_tool = EditorManager.SelectedTool;
            cMenuPianoPointer.Checked = (selected_tool == EditTool.ARROW);
            cMenuPianoPencil.Checked = (selected_tool == EditTool.PENCIL);
            cMenuPianoEraser.Checked = (selected_tool == EditTool.ERASER);

            cMenuTrackSelectorPointer.Checked = (selected_tool == EditTool.ARROW);
            cMenuTrackSelectorPencil.Checked = (selected_tool == EditTool.PENCIL);
            cMenuTrackSelectorLine.Checked = (selected_tool == EditTool.LINE);
            cMenuTrackSelectorEraser.Checked = (selected_tool == EditTool.ERASER);

            stripBtnPointer.Pushed = (selected_tool == EditTool.ARROW);
            stripBtnPencil.Pushed = (selected_tool == EditTool.PENCIL);
            stripBtnLine.Pushed = (selected_tool == EditTool.LINE);
            stripBtnEraser.Pushed = (selected_tool == EditTool.ERASER);

            cMenuPianoCurve.Checked = EditorManager.isCurveMode();
            cMenuTrackSelectorCurve.Checked = EditorManager.isCurveMode();
            stripBtnCurve.Pushed = EditorManager.isCurveMode();
        }

        /// <summary>
        /// 描画すべきオブジェクトのリスト，EditorManager.drawObjectsを更新します
        /// </summary>
        public void updateDrawObjectList()
        {
            lock (EditorManager.mDrawObjects) {
                if (MusicManager.getVsqFile() == null) {
                    return;
                }
                for (int i = 0; i < EditorManager.mDrawStartIndex.Length; i++) {
                    EditorManager.mDrawStartIndex[i] = 0;
                }
                for (int i = 0; i < EditorManager.mDrawObjects.Length; i++) {
                    EditorManager.mDrawObjects[i].Clear();
                }

                int xoffset = EditorManager.keyOffset;// 6 + EditorManager.keyWidth;
                int yoffset = (int)(127 * (int)(100 * controller.getScaleY()));
                float scalex = controller.getScaleX();
                Font SMALL_FONT = null;
                try {
                    SMALL_FONT = new Font(EditorManager.editorConfig.ScreenFontName, Cadencii.Gui.Font.PLAIN, cadencii.core.EditorConfig.FONT_SIZE8);
                    int track_height = (int)(100 * controller.getScaleY());
                    VsqFileEx vsq = MusicManager.getVsqFile();
                    int track_count = vsq.Track.Count;
                    Polygon env = new Polygon(new int[7], new int[7], 7);
                    ByRef<int> overlap_x = new ByRef<int>(0);
                    for (int track = 1; track < track_count; track++) {
                        VsqTrack vsq_track = vsq.Track[track];
                        List<DrawObject> tmp = EditorManager.mDrawObjects[track - 1];
                        RendererKind kind = VsqFileEx.getTrackRendererKind(vsq_track);
                        EditorManager.mDrawIsUtau[track - 1] = kind == RendererKind.UTAU;

                        // 音符イベント
                        foreach (var item in vsq_track.getNoteEventIterator()) {
                            if (item.ID.LyricHandle == null) {
                                continue;
                            }
                            int timesig = item.Clock;
                            int length = item.ID.getLength();
                            int note = item.ID.Note;
                            int x = (int)(timesig * scalex + xoffset);
                            int y = -note * track_height + yoffset;
                            int lyric_width = (int)(length * scalex);
                            string lyric_jp = item.ID.LyricHandle.L0.Phrase;
                            string lyric_en = item.ID.LyricHandle.L0.getPhoneticSymbol();
                            string title = cadencii.windows.forms.Utility.trimString(lyric_jp + " [" + lyric_en + "]", SMALL_FONT, lyric_width);
                            int accent = item.ID.DEMaccent;
                            int px_vibrato_start = x + lyric_width;
                            int px_vibrato_end = x;
                            int px_vibrato_delay = lyric_width * 2;
                            int vib_delay = length;
                            if (item.ID.VibratoHandle != null) {
                                vib_delay = item.ID.VibratoDelay;
                                double rate = (double)vib_delay / (double)length;
                                px_vibrato_delay = _PX_ACCENT_HEADER + (int)((lyric_width - _PX_ACCENT_HEADER) * rate);
                            }
                            VibratoBPList rate_bp = null;
                            VibratoBPList depth_bp = null;
                            int rate_start = 0;
                            int depth_start = 0;
                            if (item.ID.VibratoHandle != null) {
                                rate_bp = item.ID.VibratoHandle.getRateBP();
                                depth_bp = item.ID.VibratoHandle.getDepthBP();
                                rate_start = item.ID.VibratoHandle.getStartRate();
                                depth_start = item.ID.VibratoHandle.getStartDepth();
                            }

                            // analyzed/のSTFが引き当てられるかどうか
                            // UTAUのWAVが引き当てられるかどうか
                            bool is_valid_for_utau = false;
                            VsqEvent singer_at_clock = vsq_track.getSingerEventAt(timesig);
                            int program = singer_at_clock.ID.IconHandle.Program;
                            if (0 <= program && program < ApplicationGlobal.appConfig.UtauSingers.Count) {
                                SingerConfig sc = ApplicationGlobal.appConfig.UtauSingers[program];
                                // 通常のUTAU音源
                                if (UtauWaveGenerator.mUtauVoiceDB.ContainsKey(sc.VOICEIDSTR)) {
                                    UtauVoiceDB db = UtauWaveGenerator.mUtauVoiceDB[sc.VOICEIDSTR];
                                    OtoArgs oa = db.attachFileNameFromLyric(lyric_jp, note);
                                    if (oa.fileName == null ||
                                        (oa.fileName != null && oa.fileName == "")) {
                                        is_valid_for_utau = false;
                                    } else {
                                        is_valid_for_utau = System.IO.File.Exists(Path.Combine(sc.VOICEIDSTR, oa.fileName));
                                    }
                                }
                            }
                            int intensity = item.UstEvent == null ? 100 : item.UstEvent.getIntensity();

                            //追加
                            tmp.Add(new DrawObject(DrawObjectType.Note,
                                                     vsq,
                                                     new Rectangle(x, y, lyric_width, track_height),
                                                     title,
                                                     accent,
                                                     item.ID.DEMdecGainRate,
                                                     item.ID.Dynamics,
                                                     item.InternalID,
                                                     px_vibrato_delay,
                                                     false,
                                                     item.ID.LyricHandle.L0.PhoneticSymbolProtected,
                                                     rate_bp,
                                                     depth_bp,
                                                     rate_start,
                                                     depth_start,
                                                     item.ID.Note,
                                                     item.UstEvent.getEnvelope(),
                                                     length,
                                                     timesig,
                                                     is_valid_for_utau,
                                                     is_valid_for_utau, // vConnect-STANDはstfファイルを必要としないので，
                                                     vib_delay,
                                                     intensity));
                        }

                        // Dynaff, Crescendイベント
                        for (Iterator<VsqEvent> itr = vsq_track.getDynamicsEventIterator(); itr.hasNext(); ) {
                            VsqEvent item_itr = itr.next();
                            IconDynamicsHandle handle = item_itr.ID.IconDynamicsHandle;
                            if (handle == null) {
                                continue;
                            }
                            int clock = item_itr.Clock;
                            int length = item_itr.ID.getLength();
                            if (length <= 0) {
                                length = 1;
                            }
                            int raw_width = (int)(length * scalex);
                            DrawObjectType type = DrawObjectType.Note;
                            int width = 0;
                            string str = "";
                            if (handle.isDynaffType()) {
                                // 強弱記号
                                type = DrawObjectType.Dynaff;
                                width = EditorConfig.DYNAFF_ITEM_WIDTH;
                                int startDyn = handle.getStartDyn();
                                if (startDyn == 120) {
                                    str = "fff";
                                } else if (startDyn == 104) {
                                    str = "ff";
                                } else if (startDyn == 88) {
                                    str = "f";
                                } else if (startDyn == 72) {
                                    str = "mf";
                                } else if (startDyn == 56) {
                                    str = "mp";
                                } else if (startDyn == 40) {
                                    str = "p";
                                } else if (startDyn == 24) {
                                    str = "pp";
                                } else if (startDyn == 8) {
                                    str = "ppp";
                                } else {
                                    str = "?";
                                }
                            } else if (handle.isCrescendType()) {
                                // クレッシェンド
                                type = DrawObjectType.Crescend;
                                width = raw_width;
                                str = handle.IDS;
                            } else if (handle.isDecrescendType()) {
                                // デクレッシェンド
                                type = DrawObjectType.Decrescend;
                                width = raw_width;
                                str = handle.IDS;
                            }
                            if (type == DrawObjectType.Note) {
                                continue;
                            }
                            int note = item_itr.ID.Note;
                            int x = (int)(clock * scalex + xoffset);
                            int y = -note * (int)(100 * controller.getScaleY()) + yoffset;
                            tmp.Add(new DrawObject(type,
                                                     vsq,
                                                     new Rectangle(x, y, width, track_height),
                                                     str,
                                                     0,
                                                     0,
                                                     0,
                                                     item_itr.InternalID,
                                                     0,
                                                     false,
                                                     false,
                                                     null,
                                                     null,
                                                     0,
                                                     0,
                                                     item_itr.ID.Note,
                                                     null,
                                                     length,
                                                     clock,
                                                     true,
                                                     true,
                                                     length,
                                                     0));
                        }

                        // 重複部分があるかどうかを判定
                        int count = tmp.Count;
                        for (int i = 0; i < count - 1; i++) {
                            DrawObject itemi = tmp[i];
                            DrawObjectType parent_type = itemi.mType;
                            /*if ( itemi.type != DrawObjectType.Note ) {
                                continue;
                            }*/
                            bool overwrapped = false;
                            int istart = itemi.mClock;
                            int iend = istart + itemi.mLength;
                            if (itemi.mIsOverlapped) {
                                continue;
                            }
                            for (int j = i + 1; j < count; j++) {
                                DrawObject itemj = tmp[j];
                                if ((itemj.mType == DrawObjectType.Note && parent_type != DrawObjectType.Note) ||
                                     (itemj.mType != DrawObjectType.Note && parent_type == DrawObjectType.Note)) {
                                    continue;
                                }
                                int jstart = itemj.mClock;
                                int jend = jstart + itemj.mLength;
                                if (jstart <= istart) {
                                    if (istart < jend) {
                                        overwrapped = true;
                                        itemj.mIsOverlapped = true;
                                        // breakできない．2個以上の重複を検出する必要があるので．
                                    }
                                }
                                if (istart <= jstart) {
                                    if (jstart < iend) {
                                        overwrapped = true;
                                        itemj.mIsOverlapped = true;
                                    }
                                }
                            }
                            if (overwrapped) {
                                itemi.mIsOverlapped = true;
                            }
                        }
                        tmp.Sort();
                    }
                } catch (Exception ex) {
                    Logger.write(typeof(FormMain) + ".updateDrawObjectList; ex=" + ex + "\n");
                    serr.println("FormMain#updateDrawObjectList; ex=" + ex);
                } finally {
                    if (SMALL_FONT != null) {
                        SMALL_FONT.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 最後に保存したときから変更されているかどうかを取得または設定します
        /// </summary>
        public bool isEdited()
        {
            return mEdited;
        }

        public void setEdited(bool value)
        {
            mEdited = value;
            string file = MusicManager.getFileName();
            if (file == "") {
                file = "Untitled";
            } else {
                file = PortUtil.getFileNameWithoutExtension(file);
            }
            if (mEdited) {
                file += " *";
            }
			string title = file + " - " + FormMainModel.ApplicationName;
            if (this.Text != title) {
                this.Text = title;
            }
            bool redo = EditorManager.editHistory.hasRedoHistory();
            bool undo = EditorManager.editHistory.hasUndoHistory();
            menuEditRedo.Enabled = redo;
            menuEditUndo.Enabled = undo;
            cMenuPianoRedo.Enabled = redo;
            cMenuPianoUndo.Enabled = undo;
            cMenuTrackSelectorRedo.Enabled = redo;
            cMenuTrackSelectorUndo.Enabled = undo;
            stripBtnUndo.Enabled = undo;
            stripBtnRedo.Enabled = redo;
            //EditorManager.setRenderRequired( EditorManager.Selected, true );
            updateScrollRangeHorizontal();
            updateDrawObjectList();
            panelOverview.updateCachedImage();

#if ENABLE_PROPERTY
            EditorManager.propertyPanel.updateValue(EditorManager.Selected);
#endif
        }

        /// <summary>
        /// 入力用のテキストボックスを初期化します
        /// </summary>
        public void showInputTextBox(string phrase, string phonetic_symbol, Point position, bool phonetic_symbol_edit_mode)
        {
#if DEBUG
            CDebug.WriteLine("InitializeInputTextBox");
#endif
            hideInputTextBox();

            EditorManager.InputTextBox.KeyUp += new NKeyEventHandler(mInputTextBox_KeyUp);
            EditorManager.InputTextBox.KeyDown += new NKeyEventHandler(mInputTextBox_KeyDown);
            EditorManager.InputTextBox.ImeModeChanged += mInputTextBox_ImeModeChanged;
            EditorManager.InputTextBox.ImeMode = mLastIsImeModeOn ? Cadencii.Gui.ImeMode.Hiragana : Cadencii.Gui.ImeMode.Off;
            if (phonetic_symbol_edit_mode) {
                EditorManager.InputTextBox.setBufferText(phrase);
                EditorManager.InputTextBox.setPhoneticSymbolEditMode(true);
                EditorManager.InputTextBox.Text = phonetic_symbol;
				EditorManager.InputTextBox.BackColor = FormMainModel.ColorTextboxBackcolor;
            } else {
                EditorManager.InputTextBox.setBufferText(phonetic_symbol);
                EditorManager.InputTextBox.setPhoneticSymbolEditMode(false);
                EditorManager.InputTextBox.Text = phrase;
                EditorManager.InputTextBox.BackColor = Colors.White;
            }
            EditorManager.InputTextBox.Font = new Cadencii.Gui.Font (new System.Drawing.Font(EditorManager.editorConfig.BaseFontName, cadencii.core.EditorConfig.FONT_SIZE9, System.Drawing.FontStyle.Regular));
            var p = new Cadencii.Gui.Point(position.X + 4, position.Y + 2);
            EditorManager.InputTextBox.Location = p;

            EditorManager.InputTextBox.Parent = pictPianoRoll;
            EditorManager.InputTextBox.Enabled = true;
            EditorManager.InputTextBox.Visible = true;
            EditorManager.InputTextBox.Focus();
            EditorManager.InputTextBox.SelectAll();
        }

        public void hideInputTextBox()
        {
            EditorManager.InputTextBox.KeyUp -= new NKeyEventHandler(mInputTextBox_KeyUp);
            EditorManager.InputTextBox.KeyDown -= new NKeyEventHandler(mInputTextBox_KeyDown);
            EditorManager.InputTextBox.ImeModeChanged -= mInputTextBox_ImeModeChanged;
            mLastSymbolEditMode = EditorManager.InputTextBox.isPhoneticSymbolEditMode();
            EditorManager.InputTextBox.Visible = false;
            EditorManager.InputTextBox.Parent = null;
            EditorManager.InputTextBox.Enabled = false;
            focusPianoRoll();
        }

        public void updateMenuFonts()
        {
            if (EditorManager.editorConfig.BaseFontName == "") {
                return;
            }
            Font font = EditorManager.editorConfig.getBaseFont();
            Util.applyFontRecurse((UiForm) this, font);
#if !JAVA_MAC
            Utility.applyContextMenuFontRecurse(cMenuPiano, font);
            Utility.applyContextMenuFontRecurse(cMenuTrackSelector, font);
            if (EditorManager.MixerWindow != null) {
                Util.applyFontRecurse(EditorManager.MixerWindow, font);
            }
            Utility.applyContextMenuFontRecurse(cMenuTrackTab, font);
            trackSelector.applyFont(font);
            Utility.applyToolStripFontRecurse(menuFile, font);
            Utility.applyToolStripFontRecurse(menuEdit, font);
            Utility.applyToolStripFontRecurse(menuVisual, font);
            Utility.applyToolStripFontRecurse(menuJob, font);
            Utility.applyToolStripFontRecurse(menuTrack, font);
            Utility.applyToolStripFontRecurse(menuLyric, font);
            Utility.applyToolStripFontRecurse(menuScript, font);
            Utility.applyToolStripFontRecurse(menuSetting, font);
            Utility.applyToolStripFontRecurse(menuHelp, font);
#endif
            Util.applyFontRecurse(toolBarFile, font);
            Util.applyFontRecurse(toolBarMeasure, font);
            Util.applyFontRecurse(toolBarPosition, font);
            Util.applyFontRecurse(toolBarTool, font);
            if (mDialogPreference != null) {
                Util.applyFontRecurse(mDialogPreference, font);
            }

			cadencii.core.EditorConfig.baseFont10Bold = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.BOLD, cadencii.core.EditorConfig.FONT_SIZE10);
			cadencii.core.EditorConfig.baseFont8 = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.PLAIN, cadencii.core.EditorConfig.FONT_SIZE8);
			cadencii.core.EditorConfig.baseFont10 = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.PLAIN, cadencii.core.EditorConfig.FONT_SIZE10);
			cadencii.core.EditorConfig.baseFont9 = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.PLAIN, cadencii.core.EditorConfig.FONT_SIZE9);
			cadencii.core.EditorConfig.baseFont50Bold = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.BOLD, cadencii.core.EditorConfig.FONT_SIZE50);
			EditorConfig.baseFont10OffsetHeight = Util.getStringDrawOffset(cadencii.core.EditorConfig.baseFont10);
			EditorConfig.baseFont8OffsetHeight = Util.getStringDrawOffset(cadencii.core.EditorConfig.baseFont8);
			EditorConfig.baseFont9OffsetHeight = Util.getStringDrawOffset(cadencii.core.EditorConfig.baseFont9);
			EditorConfig.baseFont50OffsetHeight = Util.getStringDrawOffset(cadencii.core.EditorConfig.baseFont50Bold);
			EditorConfig.baseFont8Height = Util.measureString(Util.PANGRAM, cadencii.core.EditorConfig.baseFont8).Height;
			EditorConfig.baseFont9Height = Util.measureString(Util.PANGRAM, cadencii.core.EditorConfig.baseFont9).Height;
			EditorConfig.baseFont10Height = Util.measureString(Util.PANGRAM, cadencii.core.EditorConfig.baseFont10).Height;
			EditorConfig.baseFont50Height = Util.measureString(Util.PANGRAM, cadencii.core.EditorConfig.baseFont50Bold).Height;
        }

        public void picturePositionIndicatorDrawTo(Cadencii.Gui.Graphics g1)
        {
            Graphics g = (Graphics)g1;
			Font SMALL_FONT = cadencii.core.EditorConfig.baseFont8;
            int small_font_offset = EditorConfig.baseFont8OffsetHeight;
            try {
                int key_width = EditorManager.keyWidth;
                int width = picturePositionIndicator.Width;
                int height = picturePositionIndicator.Height;
                VsqFileEx vsq = MusicManager.getVsqFile();

                #region 小節ごとの線
                int dashed_line_step = EditorManager.getPositionQuantizeClock();
                for (Iterator<VsqBarLineType> itr = vsq.getBarLineIterator(EditorManager.clockFromXCoord(width)); itr.hasNext(); ) {
                    VsqBarLineType blt = itr.next();
                    int local_clock_step = 480 * 4 / blt.getLocalDenominator();
                    int x = EditorManager.xCoordFromClocks(blt.clock());
                    if (blt.isSeparator()) {
                        int current = blt.getBarCount() - vsq.getPreMeasure() + 1;
                        g.setColor(FormMainModel.ColorR105G105B105);
                        g.drawLine(x, 0, x, 49);
                        // 小節の数字
                        //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.setColor(Cadencii.Gui.Colors.Black);
                        g.setFont(SMALL_FONT);
                        g.drawString(current + "", x + 4, 8 - small_font_offset + 1);
                        //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                    } else {
						g.setColor(FormMainModel.ColorR105G105B105);
                        g.drawLine(x, 11, x, 16);
                        g.drawLine(x, 26, x, 31);
                        g.drawLine(x, 41, x, 46);
                    }
                    if (dashed_line_step > 1 && EditorManager.isGridVisible()) {
                        int numDashedLine = local_clock_step / dashed_line_step;
                        for (int i = 1; i < numDashedLine; i++) {
                            int x2 = EditorManager.xCoordFromClocks(blt.clock() + i * dashed_line_step);
							g.setColor(FormMainModel.ColorR065G065B065);
                            g.drawLine(x2, 9 + 5, x2, 14 + 3);
                            g.drawLine(x2, 24 + 5, x2, 29 + 3);
                            g.drawLine(x2, 39 + 5, x2, 44 + 3);
                        }
                    }
                }
                #endregion

                if (vsq != null) {
                    #region 拍子の変更
                    int c = vsq.TimesigTable.Count;
                    for (int i = 0; i < c; i++) {
                        TimeSigTableEntry itemi = vsq.TimesigTable[i];
                        int clock = itemi.Clock;
                        int barcount = itemi.BarCount;
                        int x = EditorManager.xCoordFromClocks(clock);
                        if (width < x) {
                            break;
                        }
                        string s = itemi.Numerator + "/" + itemi.Denominator;
                        g.setFont(SMALL_FONT);
                        if (EditorManager.itemSelection.isTimesigContains(barcount)) {
                            g.setColor(EditorManager.getHilightColor());
                            g.drawString(s, x + 4, 40 - small_font_offset + 1);
                        } else {
                            g.setColor(Cadencii.Gui.Colors.Black);
                            g.drawString(s, x + 4, 40 - small_font_offset + 1);
                        }

                        if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.TIMESIG) {
                            if (EditorManager.itemSelection.isTimesigContains(barcount)) {
                                int edit_clock_x = EditorManager.xCoordFromClocks(vsq.getClockFromBarCount(EditorManager.itemSelection.getTimesig(barcount).editing.BarCount));
								g.setColor(FormMainModel.ColorR187G187B255);
                                g.drawLine(edit_clock_x - 1, 32,
                                            edit_clock_x - 1, picturePositionIndicator.Height - 1);
								g.setColor(FormMainModel.ColorR007G007B151);
                                g.drawLine(edit_clock_x, 32,
                                            edit_clock_x, picturePositionIndicator.Height - 1);
                            }
                        }
                    }
                    #endregion

                    #region テンポの変更
                    g.setFont(SMALL_FONT);
                    c = vsq.TempoTable.Count;
                    for (int i = 0; i < c; i++) {
                        TempoTableEntry itemi = vsq.TempoTable[i];
                        int clock = itemi.Clock;
                        int x = EditorManager.xCoordFromClocks(clock);
                        if (width < x) {
                            break;
                        }
                        string s = PortUtil.formatDecimal("#.00", 60e6 / (float)itemi.Tempo);
                        if (EditorManager.itemSelection.isTempoContains(clock)) {
                            g.setColor(EditorManager.getHilightColor());
                            g.drawString(s, x + 4, 24 - small_font_offset + 1);
                        } else {
                            g.setColor(Cadencii.Gui.Colors.Black);
                            g.drawString(s, x + 4, 24 - small_font_offset + 1);
                        }

                        if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.TEMPO) {
                            if (EditorManager.itemSelection.isTempoContains(clock)) {
                                int edit_clock_x = EditorManager.xCoordFromClocks(EditorManager.itemSelection.getTempo(clock).editing.Clock);
								g.setColor(FormMainModel.ColorR187G187B255);
                                g.drawLine(edit_clock_x - 1, 18,
                                            edit_clock_x - 1, 32);
								g.setColor(FormMainModel.ColorR007G007B151);
                                g.drawLine(edit_clock_x, 18,
                                            edit_clock_x, 32);
                            }
                        }
                    }
                    #endregion
                }

                #region 現在のマーカー
                // ソングポジション
                float xoffset = key_width + EditorManager.keyOffset - controller.getStartToDrawX();
                int marker_x = (int)(EditorManager.getCurrentClock() * controller.getScaleX() + xoffset);
                if (key_width <= marker_x && marker_x <= width) {
                    g.setStroke(new Stroke(2.0f));
                    g.setColor(Cadencii.Gui.Colors.White);
                    g.drawLine(marker_x, 0, marker_x, height);
                    g.setStroke(new Stroke());
                }

                // スタートマーカーとエンドマーカー
                bool right = false;
                bool left = false;
                if (vsq.config.StartMarkerEnabled) {
                    int x = EditorManager.xCoordFromClocks(vsq.config.StartMarker);
                    if (x < key_width) {
                        left = true;
                    } else if (width < x) {
                        right = true;
                    } else {
                        g.drawImage(
                            new Cadencii.Gui.Image () { NativeImage = Properties.Resources.start_marker }, x, 3, this);
                    }
                }
                if (vsq.config.EndMarkerEnabled) {
                    int x = EditorManager.xCoordFromClocks(vsq.config.EndMarker) - 6;
                    if (x < key_width) {
                        left = true;
                    } else if (width < x) {
                        right = true;
                    } else {
                        g.drawImage(
                            new Cadencii.Gui.Image () { NativeImage = Properties.Resources.end_marker }, x, 3, this);
                    }
                }

                // 範囲外にスタートマーカーとエンドマーカーがある場合のマーク
                if (right) {
                    g.setColor(Cadencii.Gui.Colors.White);
                    g.fillPolygon(
                        new int[] { width - 6, width, width - 6 },
                        new int[] { 3, 10, 16 },
                        3);
                }
                if (left) {
                    g.setColor(Cadencii.Gui.Colors.White);
                    g.fillPolygon(
                        new int[] { key_width + 7, key_width + 1, key_width + 7 },
                        new int[] { 3, 10, 16 },
                        3);
                }
                #endregion

                #region TEMPO & BEAT
                // TEMPO BEATの文字の部分。小節数が被っている可能性があるので、塗り潰す
				var col = picturePositionIndicator.BackColor;
                g.setColor(new Color(col.R, col.G, col.B, col.A));
                g.fillRect(0, 0, EditorManager.keyWidth, 48);
                // 横ライン上
                g.setColor(new Color(104, 104, 104));
                g.drawLine(0, 17, width, 17);
                // 横ライン中央
                g.drawLine(0, 32, width, 32);
                // 横ライン下
                g.drawLine(0, 47, width, 47);
                // 縦ライン
                g.drawLine(EditorManager.keyWidth, 0, EditorManager.keyWidth, 48);
                /* TEMPO&BEATとピアノロールの境界 */
                g.drawLine(EditorManager.keyWidth, 48, width - 18, 48);
                g.setFont(SMALL_FONT);
                g.setColor(Cadencii.Gui.Colors.Black);
                g.drawString("TEMPO", 11, 24 - small_font_offset + 1);
                g.drawString("BEAT", 11, 40 - small_font_offset + 1);
                #endregion
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".picturePositionIndicatorDrawTo; ex=" + ex + "\n");
                serr.println("FormMain#picturePositionIndicatorDrawTo; ex=" + ex);
            }
        }

        /// <summary>
        /// イベントハンドラを登録します。
        /// </summary>
        public void registerEventHandlers()
        {
            this.Load += new EventHandler(FormMain_Load);

#if ENABLE_MOUSE_ENTER_STATUS_LABEL
			menuFileNew.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileOpen.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileSaveNamed.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileSave.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileOpenVsq.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileOpenUst.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileImport.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileImportMidi.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileImportUst.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileImportVsq.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileExport.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileExportWave.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileExportParaWave.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileExportMidi.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileExportMusicXml.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileExportUst.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileExportVsq.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileExportVsqx.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileExportVxt.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileRecent.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileRecentClear.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuFileQuit.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuEditUndo.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuEditRedo.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuEditCut.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuEditCopy.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuEditPaste.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuEditDelete.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuEditAutoNormalizeMode.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuEditSelectAll.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuEditSelectAllEvents.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuVisualStartMarker.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuVisualEndMarker.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuVisualIconPalette.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuVisualOverview.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuVisualControlTrack.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuVisualMixer.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuVisualWaveform.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuVisualProperty.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuVisualGridline.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuVisualLyrics.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuVisualNoteProperty.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuVisualPitchLine.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuVisualPluginUi.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuJobNormalize.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuJobInsertBar.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuJobDeleteBar.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuJobRandomize.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuJobConnect.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuJobLyric.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuTrackOn.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuTrackBgm.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuTrackAdd.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuTrackCopy.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuTrackChangeName.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuTrackDelete.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuTrackRenderCurrent.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuTrackRenderAll.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuTrackOverlay.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuTrackRenderer.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuTrackRendererVOCALOID1.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuTrackRendererVOCALOID2.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuTrackRendererUtau.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuTrackRendererVCNT.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuTrackRendererAquesTone.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuLyricCopyVibratoToPreset.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuLyricExpressionProperty.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuLyricVibratoProperty.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuLyricPhonemeTransformation.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuLyricDictionary.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuScriptUpdate.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuSettingPreference.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuSettingGameControler.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuSettingSequence.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuSettingShortcut.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuSettingVibratoPreset.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuSettingDefaultSingerStyle.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuSettingPaletteTool.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuSettingPositionQuantize.MouseEnter += new EventHandler(handleMenuMouseEnter);
			menuTrackRendererAquesTone2.MouseEnter += new System.EventHandler(this.handleMenuMouseEnter);
#endif

			menuFileNew.Click += (o, e) => model.FileMenu.RunFileNewCommand ();
			menuFileOpen.Click += (o, e) => model.FileMenu.RunFileOpenCommand ();
			menuFileSave.Click += (o, e) => model.FileMenu.RunFileSaveCommand ();
			menuFileSaveNamed.Click += (o, e) => model.FileMenu.RunFileSaveNamedCommand ();
			menuFileOpenVsq.Click += (o, e) => model.FileMenu.RunFileOpenVsqCommand ();
			menuFileOpenUst.Click += (o, e) => model.FileMenu.RunFileOpenUstCommand ();
			menuFileImportMidi.Click += (o, e) => model.FileMenu.RunFileImportMidiCommand ();
			menuFileImportUst.Click += (o, e) => model.FileMenu.RunFileImportUstCommand ();
			menuFileImportVsq.Click += (o, e) => model.FileMenu.RunFileImportVsqCommand ();
		menuFileExport.DropDownOpening += new EventHandler(menuFileExport_DropDownOpening);
			menuFileExportWave.Click += (o, e) => model.FileMenu.RunFileExportWaveCommand ();
			menuFileExportParaWave.Click += (o, e) => model.FileMenu.RunFileExportParaWaveCommand ();
			menuFileExportMidi.Click += (o, e) => model.FileMenu.RunFileExportMidiCommand ();
			menuFileExportMusicXml.Click += (o, e) => model.FileMenu.RunFileExportMusicXmlCommand ();
			menuFileExportUst.Click += (o, e) => model.FileMenu.RunFileExportUstCommand ();
			menuFileExportVsq.Click += (o, e) => model.FileMenu.RunFileExportVsqCommand ();
			menuFileExportVsqx.Click += (o, e) => model.FileMenu.RunFileExportVsqxCommand ();
			menuFileExportVxt.Click += (o, e) => model.FileMenu.RunFileExportVxtCommand ();
			menuFileRecentClear.Click += (o, e) => model.FileMenu.RunFileRecentClearCommand ();
			menuFileQuit.Click += (o, e) => model.FileMenu.RunFileQuitCommand ();
		menuEdit.DropDownOpening += new EventHandler(menuEdit_DropDownOpening);
			menuEditUndo.Click += (o, e) => model.EditMenu.RunEditUndoCommand();
			menuEditRedo.Click += (o, e) => model.EditMenu.RunEditRedoCommand();

			menuEditCut.Click += (o, e) => model.Cut ();
			menuEditCopy.Click += (o, e) => model.Copy ();
			menuEditPaste.Click += (o, e) => model.Paste ();
			menuEditDelete.Click += (o, e) => model.EditMenu.RunEditDeleteCommand();
			menuEditAutoNormalizeMode.Click += (o, e) => model.EditMenu.RunEditAutoNormalizeModeCommand();
			menuEditSelectAll.Click += (o, e) => model.EditMenu.RunEditSelectAllCommand();
			menuEditSelectAllEvents.Click += (o, e) => model.EditMenu.RunEditSelectAllEventsCommand();
			menuVisualControlTrack.CheckedChanged += (o, e) => model.VisualMenu.RunVisualControlTrackCheckedChanged ();
			menuVisualMixer.Click += (o, e) => model.VisualMenu.RunVisualMixerCommand ();
			menuVisualWaveform.CheckedChanged += (o, e) => model.VisualMenu.RunVisualWaveformCheckedChanged ();
			menuVisualProperty.CheckedChanged += (o, e) => model.VisualMenu.RunVisualPropertyCheckedChanged ();
			menuVisualGridline.CheckedChanged += (o, e) => model.VisualMenu.RunVisualGridlineCheckedChanged ();
			menuVisualIconPalette.Click += (o, e) => model.VisualMenu.RunVisualIconPaletteCommand ();
			menuVisualStartMarker.Click += (o, e) => model.VisualMenu.RunStartMarkerCommand ();
			menuVisualEndMarker.Click += (o, e) => model.VisualMenu.RunEndMarkerCommand ();
			menuVisualLyrics.CheckedChanged += (o, e) => model.VisualMenu.RunVisualLyricsCheckedChanged ();
			menuVisualNoteProperty.CheckedChanged += (o, e) => model.VisualMenu.RunVisualNotePropertyCheckedChanged ();
			menuVisualPitchLine.CheckedChanged += (o, e) => model.VisualMenu.RunVisualPitchLineCheckedChanged ();
			menuVisualPluginUi.DropDownOpening += (o, e) => model.VisualMenu.RunVisualPluginUiDropDownOpening ();
			menuVisualPluginUiVocaloid1.Click += (o, e) => model.VisualMenu.RunVisualPluginUiVocaloidCommonCommand (RendererKind.VOCALOID1);
			menuVisualPluginUiVocaloid2.Click += (o, e) => model.VisualMenu.RunVisualPluginUiVocaloidCommonCommand (RendererKind.VOCALOID2);
			menuVisualPluginUiAquesTone.Click += (o, e) => model.VisualMenu.RunVisualPluginUiAquesToneCommand ();
			menuJob.DropDownOpening += (o, e) => model.JobMenu.RunJobDropDownOpening ();
			menuJobNormalize.Click += (o, e) => model.JobMenu.RunJobNormalizeCommand ();
			menuJobInsertBar.Click += (o, e) => model.JobMenu.RunJobInsertBarCommand ();
			menuJobDeleteBar.Click += (o, e) => model.JobMenu.RunJobDeleteBarCommand ();
			menuJobRandomize.Click += (o, e) => model.JobMenu.RunJobRandomizeCommand ();
			menuJobConnect.Click += (o, e) => model.JobMenu.RunJobConnectCommand ();
			menuJobLyric.Click += (o, e) => model.JobMenu.RunJobLyricCommand ();
			menuTrack.DropDownOpening += (o, e) => model.TrackMenu.RunTrackDropDownOpening ();
			menuTrackOn.Click += (o, e) => model.TrackMenu.RunTrackOnCommand ();
			menuTrackAdd.Click += (o, e) => model.AddTrack ();
			menuTrackCopy.Click += (o, e) => model.CopyTrack ();
			menuTrackChangeName.Click += (o, e) => model.ChangeTrackName ();
			menuTrackDelete.Click += (o, e) => model.DeleteTrack ();
			menuTrackRenderCurrent.Click += (o, e) => model.TrackMenu.RunTrackRenderCurrentCommand ();
			menuTrackRenderAll.Click += (o, e) => model.TrackMenu.RunTrackRenderAllCommand ();
			menuTrackOverlay.Click += (o, e) => model.TrackMenu.RunTrackOverlayCommand ();
			menuTrackRenderer.DropDownOpening += (o, e) => model.TrackMenu.RunTrackRendererDropDownOpening ();
			menuTrackRendererVOCALOID1.Click += (o, e) => model.TrackMenu.RunChangeRendererCommand (RendererKind.VOCALOID1, -1);
			menuTrackRendererVOCALOID2.Click += (o, e) => model.TrackMenu.RunChangeRendererCommand (RendererKind.VOCALOID2, -1);
			//UTAUはresamplerを識別するのでmenuTrackRendererUtauのサブアイテムのClickイベントを拾う
			//menuTrackRendererUtau.Click += (o, e) => model.TrackMenu.RunChangeRendererCommand (RendererKind.UTAU, xxx);
			menuTrackRendererVCNT.Click += (o, e) => model.TrackMenu.RunChangeRendererCommand (RendererKind.VCNT, -1);
			menuTrackRendererAquesTone.Click += (o, e) => model.TrackMenu.RunChangeRendererCommand (RendererKind.AQUES_TONE, -1);
			menuLyric.DropDownOpening += (o, e) => model.LyricMenu.RunLyricDropDownOpening ();
			menuLyricExpressionProperty.Click += (o, e) => model.LyricMenu.RunLyricExpressionPropertyCommand ();
			menuLyricVibratoProperty.Click += (o, e) => model.LyricMenu.RunLyricVibratoPropertyCommand ();
			menuLyricDictionary.Click += (o, e) => model.LyricMenu.RunLyricDictionaryCommand ();
			menuLyricPhonemeTransformation.Click += (o, e) => model.LyricMenu.RunLyricPhonemeTransformationCommand ();
			menuLyricApplyUtauParameters.Click += (o, e) => model.LyricMenu.RunLyricApplyUtauParametersCommand ();
			menuScriptUpdate.Click += (o, e) => model.ScriptMenu.RunScriptUpdateCommand ();
			menuSettingPreference.Click += (o, e) => model.SettingsMenu.RunSettingPreferenceCommand ();
			menuSettingGameControlerSetting.Click += (o, e) => model.SettingsMenu.RunSettingGameControlerSettingCommand();
			menuSettingGameControlerLoad.Click += (o, e) => model.SettingsMenu.RunSettingGameControlerLoadCommand();
			menuSettingGameControlerRemove.Click += (o, e) => model.SettingsMenu.RunSettingGameControlerRemoveCommand();
			menuSettingSequence.Click += (o, e) => model.SettingsMenu.RunSettingSequenceCommand ();
			menuSettingShortcut.Click += (o, e) => model.SettingsMenu.RunSettingShortcutCommand();
			menuSettingVibratoPreset.Click += (o, e) => model.SettingsMenu.RunSettingVibratoPresetCommand();
			menuSettingDefaultSingerStyle.Click += (o, e) => model.SettingsMenu.RunSettingDefaultSingerStyleCommand();
			menuSettingPositionQuantize04.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p4);
			menuSettingPositionQuantize08.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p8);
			menuSettingPositionQuantize16.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p16);
			menuSettingPositionQuantize32.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p32);
			menuSettingPositionQuantize64.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p64);
			menuSettingPositionQuantize128.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p128);
			menuSettingPositionQuantizeOff.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.off);
			menuSettingPositionQuantizeTriplet.Click += (o, e) => model.HandlePositionQuantizeTriplet ();
			menuHelpAbout.Click += (o, e) => model.HelpMenu.RunHelpAboutCommand ();
			menuHelpManual.Click += (o, e) => model.HelpMenu.RunHelpManualCommand ();
			menuHelpLogSwitch.CheckedChanged += (o, e) => model.HelpMenu.RunHelpLogSwitchCheckedChanged ();
			menuHelpLogOpen.Click += (o, e) => model.HelpMenu.RunHelpLogOpenCommand ();
			menuHelpDebug.Click += (o, e) => model.HelpMenu.RunHelpDebugCommand ();
			menuHiddenEditLyric.Click += (o, e) => model.HiddenMenu.RunHiddenEditLyricCommand ();
			menuHiddenEditFlipToolPointerPencil.Click += (o, e) => model.HiddenMenu.RunHiddenEditFlipToolPointerPencilCommand ();
			menuHiddenEditFlipToolPointerEraser.Click += (o, e) => model.HiddenMenu.RunHiddenEditFlipToolPointerEraserCommand ();
			menuHiddenVisualForwardParameter.Click += (o, e) => model.HiddenMenu.RunHiddenVisualForwardParameterCommand ();
			menuHiddenVisualBackwardParameter.Click += (o, e) => model.HiddenMenu.RunHiddenVisualBackwardParameterCommand ();
			menuHiddenTrackNext.Click += (o, e) => model.HiddenMenu.RunHiddenTrackNextCommand ();
			menuHiddenTrackBack.Click += (o, e) => model.HiddenMenu.RunHiddenTrackBackCommand ();
			menuHiddenCopy.Click += (o, e) => model.Copy ();
			menuHiddenPaste.Click += (o, e) => model.Paste ();
			menuHiddenCut.Click += (o, e) => model.Cut ();
			menuHiddenSelectBackward.Click += (o, e) => model.HiddenMenu.RunHiddenSelectBackwardCommand ();
			menuHiddenSelectForward.Click += (o, e) => model.HiddenMenu.RunHiddenSelectForwardCommand ();
			menuHiddenMoveUp.Click += (o, e) => model.HiddenMenu.RunHiddenMoveUpCommand ();
			menuHiddenMoveDown.Click += (o, e) => model.HiddenMenu.RunHiddenMoveDownCommand ();
			menuHiddenMoveLeft.Click += (o, e) => model.HiddenMenu.RunHiddenMoveLeftCommand ();
			menuHiddenMoveRight.Click += (o, e) => model.HiddenMenu.RunHiddenMoveRightCommand ();
			menuHiddenLengthen.Click += (o, e) => model.HiddenMenu.RunHiddenLengthenCommand ();
			menuHiddenShorten.Click += (o, e) => model.HiddenMenu.RunHiddenShortenCommand ();
			menuHiddenGoToEndMarker.Click += (o, e) => model.HiddenMenu.RunHiddenGoToEndMarkerCommand ();
			menuHiddenGoToStartMarker.Click += (o, e) => model.HiddenMenu.RunHiddenGoToStartMarkerCommand ();
			menuHiddenPlayFromStartMarker.Click += (o, e) => model.HiddenMenu.RunHiddenPlayFromStartMarkerCommand ();
			menuHiddenPrintPoToCSV.Click += (o, e) => model.HiddenMenu.RunHiddenPrintPoToCSVCommand ();
			menuHiddenFlipCurveOnPianorollMode.Click += (o, e) => model.HiddenMenu.RunHiddenFlipCurveOnPianorollModeCommand ();

			cMenuPiano.Opening += (o, e) => model.PianoMenu.RunPianoOpeningCommand ();
			cMenuPianoPointer.Click += (o, e) => model.PianoMenu.RunPianoPointerCommand ();
			cMenuPianoPencil.Click += (o, e) => model.PianoMenu.RunPianoPencilCommand ();
			cMenuPianoEraser.Click += (o, e) => model.PianoMenu.RunPianoEraserCommand ();
			cMenuPianoCurve.Click += (o, e) => model.PianoMenu.RunPianoCurveCommand ();
			cMenuPianoFixed01.Click += (o, e) => model.PianoMenu.RunPianoFixed01Command ();
			cMenuPianoFixed02.Click += (o, e) => model.PianoMenu.RunPianoFixed02Command ();
			cMenuPianoFixed04.Click += (o, e) => model.PianoMenu.RunPianoFixed04Command ();
			cMenuPianoFixed08.Click += (o, e) => model.PianoMenu.RunPianoFixed08Command ();
			cMenuPianoFixed16.Click += (o, e) => model.PianoMenu.RunPianoFixed16Command ();
			cMenuPianoFixed32.Click += (o, e) => model.PianoMenu.RunPianoFixed32Command ();
			cMenuPianoFixed64.Click += (o, e) => model.PianoMenu.RunPianoFixed64Command ();
			cMenuPianoFixed128.Click += (o, e) => model.PianoMenu.RunPianoFixed128Command ();
			cMenuPianoFixedOff.Click += (o, e) => model.PianoMenu.RunPianoFixedOffCommand ();
			cMenuPianoFixedTriplet.Click += (o, e) => model.PianoMenu.RunPianoFixedTripletCommand ();
			cMenuPianoFixedDotted.Click += (o, e) => model.PianoMenu.RunPianoFixedDottedCommand ();
			cMenuPianoQuantize04.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p4);
			cMenuPianoQuantize08.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p8);
			cMenuPianoQuantize16.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p16);
			cMenuPianoQuantize32.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p32);
			cMenuPianoQuantize64.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p64);
			cMenuPianoQuantize128.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p128);
			cMenuPianoQuantizeOff.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.off);
			cMenuPianoQuantizeTriplet.Click += (o, e) => model.HandlePositionQuantizeTriplet ();
			cMenuPianoGrid.Click += (o, e) => model.PianoMenu.RunPianoGridCommand ();
			cMenuPianoUndo.Click += (o, e) => model.Undo ();
			cMenuPianoRedo.Click += (o, e) => model.Redo ();
			cMenuPianoCut.Click += (o, e) => model.Cut ();
			cMenuPianoCopy.Click += (o, e) => model.Copy ();
			cMenuPianoPaste.Click += (o, e) => model.Paste ();
			cMenuPianoDelete.Click += (o, e) => model.DeleteEvent ();
			cMenuPianoSelectAll.Click += (o, e) => model.SelectAll ();
			cMenuPianoSelectAllEvents.Click += (o, e) => model.SelectAllEvent ();
			cMenuPianoImportLyric.Click += (o, e) => model.ImportLyric ();
			cMenuPianoExpressionProperty.Click += (o, e) => model.EditNoteExpressionProperty ();
			cMenuPianoVibratoProperty.Click += (o, e) => model.EditNoteVibratoProperty ();
            cMenuTrackTab.Opening += (o, e) => cMenuTrackTab_Opening();
			cMenuTrackTabTrackOn.Click += (o, e) => model.TrackMenu.RunTrackOnCommand ();
			cMenuTrackTabAdd.Click += (o, e) => model.AddTrack ();
			cMenuTrackTabCopy.Click += (o, e) => model.CopyTrack ();
			cMenuTrackTabChangeName.Click += (o, e) => model.ChangeTrackName ();
			cMenuTrackTabDelete.Click += (o, e) => model.DeleteTrack ();
            cMenuTrackTabRenderCurrent.Click += new EventHandler(cMenuTrackTabRenderCurrent_Click);
			cMenuTrackTabRenderAll.Click += (o, e) => model.TrackMenu.RunTrackRenderAllCommand ();
            cMenuTrackTabOverlay.Click += new EventHandler(cMenuTrackTabOverlay_Click);
			cMenuTrackTabRenderer.DropDownOpening += (o, e) => updateRendererMenu ();
			cMenuTrackTabRendererVOCALOID1.Click += (o, e) => model.TrackMenu.RunChangeRendererCommand (RendererKind.VOCALOID1, -1);
			cMenuTrackTabRendererVOCALOID2.Click += (o, e) => model.TrackMenu.RunChangeRendererCommand (RendererKind.VOCALOID2, -1);
			cMenuTrackTabRendererStraight.Click += (o, e) => model.TrackMenu.RunChangeRendererCommand (RendererKind.STRAIGHT_UTAU, -1);
			cMenuTrackTabRendererAquesTone.Click += (o, e) => model.TrackMenu.RunChangeRendererCommand (RendererKind.AQUES_TONE, -1);
			cMenuTrackSelector.Opening += (o, e) => model.TrackSelectorMenu.RunOpening ();
			cMenuTrackSelectorPointer.Click += (o, e) => model.TrackSelectorMenu.RunPointerCommand ();
			cMenuTrackSelectorPencil.Click += (o, e) => model.TrackSelectorMenu.RunPencilCommand ();
			cMenuTrackSelectorLine.Click += (o, e) => model.TrackSelectorMenu.RunLineCommand ();
			cMenuTrackSelectorEraser.Click += (o, e) => model.TrackSelectorMenu.RunEraserCommand ();
			cMenuTrackSelectorCurve.Click += (o, e) => model.TrackSelectorMenu.RunCurveCommand ();
			cMenuTrackSelectorUndo.Click += (o, e) => model.Undo ();
			cMenuTrackSelectorRedo.Click += (o, e) => model.Redo ();
			cMenuTrackSelectorCut.Click += (o, e) => model.Cut ();
			cMenuTrackSelectorCopy.Click += (o, e) => model.Copy ();
			cMenuTrackSelectorPaste.Click += (o, e) => model.Paste ();
			cMenuTrackSelectorDelete.Click += (o, e) => model.DeleteEvent ();
			cMenuTrackSelectorDeleteBezier.Click += (o, e) => model.TrackSelectorMenu.RunDeleteBezierCommand ();
			cMenuTrackSelectorSelectAll.Click += (o, e) => model.SelectAllEvent ();
            cMenuPositionIndicatorEndMarker.Click += new EventHandler(cMenuPositionIndicatorEndMarker_Click);
            cMenuPositionIndicatorStartMarker.Click += new EventHandler(cMenuPositionIndicatorStartMarker_Click);
            trackBar.ValueChanged += new EventHandler(trackBar_ValueChanged);
            trackBar.Enter += new EventHandler(trackBar_Enter);
            bgWorkScreen.DoWork += new DoWorkEventHandler(bgWorkScreen_DoWork);
            timer.Tick += new EventHandler(timer_Tick);
            pictKeyLengthSplitter.MouseMove += pictKeyLengthSplitter_MouseMove;
            pictKeyLengthSplitter.MouseDown += pictKeyLengthSplitter_MouseDown;
            pictKeyLengthSplitter.MouseUp += pictKeyLengthSplitter_MouseUp;
            panelOverview.KeyUp += new NKeyEventHandler(handleSpaceKeyUp);
            panelOverview.KeyDown += new NKeyEventHandler(handleSpaceKeyDown);
            vScroll.ValueChanged += new EventHandler(vScroll_ValueChanged);
            //this.Resize += new EventHandler( handleVScrollResize );
            pictPianoRoll.Resize += new EventHandler(handleVScrollResize);
            vScroll.Enter += new EventHandler(vScroll_Enter);
            hScroll.ValueChanged += new EventHandler(hScroll_ValueChanged);
            hScroll.Resize += new EventHandler(hScroll_Resize);
            hScroll.Enter += new EventHandler(hScroll_Enter);
            picturePositionIndicator.PreviewKeyDown += picturePositionIndicator_PreviewKeyDown;
            picturePositionIndicator.MouseMove += picturePositionIndicator_MouseMove;
            picturePositionIndicator.MouseClick += picturePositionIndicator_MouseClick;
            picturePositionIndicator.MouseDoubleClick += picturePositionIndicator_MouseDoubleClick;
            picturePositionIndicator.MouseDown += picturePositionIndicator_MouseDown;
            picturePositionIndicator.MouseUp += picturePositionIndicator_MouseUp;
            picturePositionIndicator.Paint += picturePositionIndicator_Paint;
            pictPianoRoll.PreviewKeyDown += new NKeyEventHandler(pictPianoRoll_PreviewKeyDown2);
            pictPianoRoll.KeyUp += new NKeyEventHandler(handleSpaceKeyUp);
            pictPianoRoll.KeyUp += new NKeyEventHandler(pictPianoRoll_KeyUp);
            pictPianoRoll.MouseMove += new NMouseEventHandler(pictPianoRoll_MouseMove);
            pictPianoRoll.MouseDoubleClick += new NMouseEventHandler(pictPianoRoll_MouseDoubleClick);
            pictPianoRoll.MouseClick += new NMouseEventHandler(pictPianoRoll_MouseClick);
            pictPianoRoll.MouseDown += new NMouseEventHandler(pictPianoRoll_MouseDown);
            pictPianoRoll.MouseUp += new NMouseEventHandler(pictPianoRoll_MouseUp);
            pictPianoRoll.KeyDown += new NKeyEventHandler(handleSpaceKeyDown);
            waveView.MouseDoubleClick += new NMouseEventHandler(waveView_MouseDoubleClick);
            waveView.MouseDown += new NMouseEventHandler(waveView_MouseDown);
            waveView.MouseUp += new NMouseEventHandler(waveView_MouseUp);
            waveView.MouseMove += new NMouseEventHandler(waveView_MouseMove);
            this.DragEnter += FormMain_DragEnter;
            this.DragDrop += FormMain_DragDrop;
            this.DragOver += FormMain_DragOver;
            this.DragLeave += new EventHandler(FormMain_DragLeave);

            pictureBox2.MouseDown += pictureBox2_MouseDown;
            pictureBox2.MouseUp += pictureBox2_MouseUp;
            pictureBox2.Paint += pictureBox2_Paint;
            toolBarTool.ButtonClick += toolBarTool_ButtonClick;
            rebar.SizeChanged += new EventHandler(toolStripContainer_TopToolStripPanel_SizeChanged);
			stripDDBtnQuantize04.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p4);
			stripDDBtnQuantize08.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p8);
			stripDDBtnQuantize16.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p16);
			stripDDBtnQuantize32.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p32);
			stripDDBtnQuantize64.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p64);
			stripDDBtnQuantize128.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p128);
			stripDDBtnQuantizeOff.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.off);
			stripDDBtnQuantizeTriplet.Click += (o, e) => model.HandlePositionQuantizeTriplet ();
            toolBarFile.ButtonClick += toolBarFile_ButtonClick;
            toolBarPosition.ButtonClick += toolBarPosition_ButtonClick;
            toolBarMeasure.ButtonClick += toolBarMeasure_ButtonClick;
            toolBarMeasure.MouseDown += toolBarMeasure_MouseDown;
            stripBtnStepSequencer.CheckedChanged += new EventHandler(stripBtnStepSequencer_CheckedChanged);
            this.Deactivate += new EventHandler(FormMain_Deactivate);
            this.Activated += new EventHandler(FormMain_Activated);
            this.FormClosed += new FormClosedEventHandler(FormMain_FormClosed);
            this.FormClosing += new FormClosingEventHandler(FormMain_FormClosing);
            this.PreviewKeyDown += FormMain_PreviewKeyDown;
            this.SizeChanged += FormMain_SizeChanged;
            panelOverview.Enter += new EventHandler(panelOverview_Enter);
        }

        public void setResources()
        {
            try {
				this.stripLblGameCtrlMode.Image = Properties.Resources.slash.ToAwt ();
				this.stripLblMidiIn.Image = Properties.Resources.slash.ToAwt ();

                this.stripBtnStepSequencer.Image = Properties.Resources.piano.ToAwt ();
                this.Icon = Properties.Resources.Icon1;
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".setResources; ex=" + ex + "\n");
                serr.println("FormMain#setResources; ex=" + ex);
            }
        }
        #endregion // public methods

        #region event handlers
        public void menuWindowMinimize_Click(Object sender, EventArgs e)
        {
            var state = this.WindowState;
            if (state != FormWindowState.Minimized) {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        //BOOKMARK: panelOverview
        #region panelOverview
        public void panelOverview_Enter(Object sender, EventArgs e)
        {
            controller.navigationPanelGotFocus();
        }
        #endregion

        //BOOKMARK: inputTextBox
        #region EditorManager.InputTextBox
	public void mInputTextBox_KeyDown(Object sender, NKeyEventArgs e)
        {
#if DEBUG
            sout.println("FormMain#mInputTextBox_KeyDown");
#endif
            bool shift = ((Keys) e.Modifiers & Keys.Shift) == Keys.Shift;
            bool tab = (Keys) e.KeyCode == Keys.Tab;
            bool enter = (Keys) e.KeyCode == Keys.Return;
            if (tab || enter) {
                executeLyricChangeCommand();
                int selected = EditorManager.Selected;
                int index = -1;
                int width = pictPianoRoll.Width;
                int height = pictPianoRoll.Height;
                int key_width = EditorManager.keyWidth;
                VsqTrack track = MusicManager.getVsqFile().Track[selected];
                track.sortEvent();
                if (tab) {
                    int clock = 0;
                    int search_index = EditorManager.itemSelection.getLastEvent().original.InternalID;
                    int c = track.getEventCount();
                    for (int i = 0; i < c; i++) {
                        VsqEvent item = track.getEvent(i);
                        if (item.InternalID == search_index) {
                            index = i;
                            clock = item.Clock;
                            break;
                        }
                    }
                    if (shift) {
                        // 1個前の音符イベントを検索
                        int tindex = -1;
                        for (int i = track.getEventCount() - 1; i >= 0; i--) {
                            VsqEvent ve = track.getEvent(i);
                            if (ve.ID.type == VsqIDType.Anote && i != index && ve.Clock <= clock) {
                                tindex = i;
                                break;
                            }
                        }
                        index = tindex;
                    } else {
                        // 1個後の音符イベントを検索
                        int tindex = -1;
                        int c2 = track.getEventCount();
                        for (int i = 0; i < c2; i++) {
                            VsqEvent ve = track.getEvent(i);
                            if (ve.ID.type == VsqIDType.Anote && i != index && ve.Clock >= clock) {
                                tindex = i;
                                break;
                            }
                        }
                        index = tindex;
                    }
                }
                if (0 <= index && index < track.getEventCount()) {
                    EditorManager.itemSelection.clearEvent();
                    VsqEvent item = track.getEvent(index);
                    EditorManager.itemSelection.addEvent(item.InternalID);
                    int x = EditorManager.xCoordFromClocks(item.Clock);
                    int y = EditorManager.yCoordFromNote(item.ID.Note);
                    bool phonetic_symbol_edit_mode = EditorManager.InputTextBox.isPhoneticSymbolEditMode();
                    showInputTextBox(
                        item.ID.LyricHandle.L0.Phrase,
                        item.ID.LyricHandle.L0.getPhoneticSymbol(),
                        new Point(x, y),
                        phonetic_symbol_edit_mode);
                    int clWidth = (int)(EditorManager.InputTextBox.Width * controller.getScaleXInv());

                    // 画面上にEditorManager.InputTextBoxが見えるように，移動
                    int SPACE = 20;
                    // vScrollやhScrollをいじった場合はfalseにする．
                    bool refresh_screen = true;
                    // X軸方向について，見えるように移動
                    if (x < key_width || width < x + EditorManager.InputTextBox.Width) {
                        int clock, clock_x;
                        if (x < key_width) {
                            // 左に隠れてしまう場合
                            clock = item.Clock;
                        } else {
                            // 右に隠れてしまう場合
                            clock = item.Clock + clWidth;
                        }
                        if (shift) {
                            // 左方向に移動していた場合
                            // 右から３分の１の位置に移動させる
                            clock_x = width - (width - key_width) / 3;
                        } else {
                            // 右方向に移動していた場合
                            clock_x = key_width + (width - key_width) / 3;
                        }
                        double draft_d = (key_width + EditorManager.keyOffset - clock_x) * controller.getScaleXInv() + clock;
                        if (draft_d < 0.0) {
                            draft_d = 0.0;
                        }
                        int draft = (int)draft_d;
                        if (draft < hScroll.Minimum) {
                            draft = hScroll.Minimum;
                        } else if (hScroll.Maximum < draft) {
                            draft = hScroll.Maximum;
                        }
                        refresh_screen = false;
                        hScroll.Value = draft;
                    }
                    // y軸方向について，見えるように移動
                    int track_height = (int)(100 * controller.getScaleY());
                    if (y <= 0 || height - track_height <= y) {
                        int note = item.ID.Note;
                        if (y <= 0) {
                            // 上にはみ出してしまう場合
                            note = item.ID.Note + 1;
                        } else {
                            // 下にはみ出してしまう場合
                            note = item.ID.Note - 2;
                        }
                        if (127 < note) {
                            note = 127;
                        }
                        if (note < 0) {
                            note = 0;
                        }
                        ensureVisibleY(note);
                    }
                    if (refresh_screen) {
                        refreshScreen();
                    }
                } else {
                    int id = EditorManager.itemSelection.getLastEvent().original.InternalID;
                    EditorManager.itemSelection.clearEvent();
                    EditorManager.itemSelection.addEvent(id);
                    hideInputTextBox();
                }
            }
        }

        public void mInputTextBox_KeyUp(Object sender, NKeyEventArgs e)
        {
#if DEBUG
            sout.println("FormMain#mInputTextBox_KeyUp");
#endif
            bool flip = ((Keys) e.KeyCode == Keys.Up || (Keys) e.KeyCode == Keys.Down) && ((Keys) e.Modifiers == Keys.Alt);
            bool hide = (Keys) e.KeyCode == Keys.Escape;

            if (flip) {
                if (EditorManager.InputTextBox.Visible) {
                    model.FlipInputTextBoxMode();
                }
            } else if (hide) {
                hideInputTextBox();
            }
        }

        public void mInputTextBox_ImeModeChanged(Object sender, EventArgs e)
        {
            mLastIsImeModeOn = EditorManager.InputTextBox.ImeMode == Cadencii.Gui.ImeMode.Hiragana;
        }

        public void mInputTextBox_KeyPress(Object sender, Cadencii.Gui.KeyPressEventArgs e)
        {
#if DEBUG
            sout.println("FormMain#mInputTextBox_KeyPress");
#endif
            //           Enter                                  Tab
            e.Handled = (e.KeyChar == Convert.ToChar(13)) || (e.KeyChar == Convert.ToChar(09));
        }
        #endregion

        //BOOKMARK: EditorManager
        #region EditorManager
        public void EditorManager_EditedStateChanged(Object sender, bool edited)
        {
            setEdited(edited);
        }

        public void EditorManager_GridVisibleChanged(Object sender, EventArgs e)
        {
            menuVisualGridline.Checked = EditorManager.isGridVisible();
            stripBtnGrid.Pushed = EditorManager.isGridVisible();
            cMenuPianoGrid.Checked = EditorManager.isGridVisible();
        }

        public void EditorManager_MainWindowFocusRequired(Object sender, EventArgs e)
        {
            this.Focus();
        }

        public void EditorManager_PreviewAborted(Object sender, EventArgs e)
        {
#if DEBUG
            sout.println("FormMain#EditorManager_PreviewAborted");
#endif
            stripBtnPlay.ImageKey = "control.png";
            stripBtnPlay.Text = _("Play");
            timer.Stop();

            for (int i = 0; i < EditorManager.mDrawStartIndex.Length; i++) {
                EditorManager.mDrawStartIndex[i] = 0;
            }
#if ENABLE_MIDI
            //MidiPlayer.stop();
#endif // ENABLE_MIDI
        }

        public void EditorManager_PreviewStarted(Object sender, EventArgs e)
        {
#if DEBUG
            sout.println("FormMain#EditorManager_PreviewStarted");
#endif
            EditorManager.mAddingEvent = null;
            int selected = EditorManager.Selected;
            VsqFileEx vsq = MusicManager.getVsqFile();
            RendererKind renderer = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
            int clock = EditorManager.getCurrentClock();
            mLastClock = clock;
            double now = PortUtil.getCurrentTime();
            EditorManager.mPreviewStartedTime = now;
            timer.Start();
            stripBtnPlay.ImageKey = "control_pause.png";
            stripBtnPlay.Text = _("Stop");
        }

        public void EditorManager_SelectedToolChanged(Object sender, EventArgs e)
        {
            applySelectedTool();
        }

        public void ItemSelectionModel_SelectedEventChanged(Object sender, bool selected_event_is_null)
        {
            menuEditCut.Enabled = !selected_event_is_null;
            menuEditPaste.Enabled = !selected_event_is_null;
            menuEditDelete.Enabled = !selected_event_is_null;
            cMenuPianoCut.Enabled = !selected_event_is_null;
            cMenuPianoCopy.Enabled = !selected_event_is_null;
            cMenuPianoDelete.Enabled = !selected_event_is_null;
            cMenuPianoExpressionProperty.Enabled = !selected_event_is_null;
            menuLyricVibratoProperty.Enabled = !selected_event_is_null;
            menuLyricExpressionProperty.Enabled = !selected_event_is_null;
            stripBtnCut.Enabled = !selected_event_is_null;
            stripBtnCopy.Enabled = !selected_event_is_null;
        }

        public void EditorManager_UpdateBgmStatusRequired(Object sender, EventArgs e)
        {
            updateBgmMenuState();
        }

        public void EditorManager_WaveViewRealoadRequired(Object sender, WaveViewRealoadRequiredEventArgs arg)
        {
            int track = arg.track;
            string file = arg.file;
            double sec_start = arg.secStart;
            double sec_end = arg.secEnd;
            if (sec_start <= sec_end) {
                waveView.reloadPartial(track - 1, file, sec_start, sec_end);
            } else {
                waveView.load(track - 1, file);
            }
        }
        #endregion

        //BOOKMARK: pictPianoRoll
        #region pictPianoRoll
        public void pictPianoRoll_KeyUp(Object sender, NKeyEventArgs e)
        {
            processSpecialShortcutKey(e, false);
        }

        public void pictPianoRoll_MouseClick(Object sender, NMouseEventArgs e)
        {
#if DEBUG
            CDebug.WriteLine("pictPianoRoll_MouseClick");
#endif
            Keys modefiers = (Keys) Control.ModifierKeys;
            EditMode edit_mode = EditorManager.EditMode;

            bool is_button_left = e.Button == NMouseButtons.Left;
            int selected = EditorManager.Selected;

            if (e.Button == NMouseButtons.Left) {
#if ENABLE_MOUSEHOVER
                if ( mMouseHoverThread != null ) {
                    mMouseHoverThread.Abort();
                }
#endif

                // クリック位置にIDが無いかどうかを検査
                ByRef<Rectangle> out_id_rect = new ByRef<Rectangle>(new Rectangle());
                VsqEvent item = getItemAtClickedPosition(new Point(e.X, e.Y), out_id_rect);
                Rectangle id_rect = out_id_rect.value;
#if DEBUG
                CDebug.WriteLine("    (item==null)=" + (item == null));
#endif
                if (item != null &&
                     edit_mode != EditMode.MOVE_ENTRY_WAIT_MOVE &&
                     edit_mode != EditMode.MOVE_ENTRY &&
                     edit_mode != EditMode.MOVE_ENTRY_WHOLE_WAIT_MOVE &&
                     edit_mode != EditMode.MOVE_ENTRY_WHOLE &&
                     edit_mode != EditMode.EDIT_LEFT_EDGE &&
                     edit_mode != EditMode.EDIT_RIGHT_EDGE &&
                     edit_mode != EditMode.MIDDLE_DRAG &&
                     edit_mode != EditMode.CURVE_ON_PIANOROLL) {
                    if ((modefiers & Keys.Shift) != Keys.Shift && (modefiers & s_modifier_key) != s_modifier_key) {
                        EditorManager.itemSelection.clearEvent();
                    }
                    EditorManager.itemSelection.addEvent(item.InternalID);
                    int internal_id = item.InternalID;
                    hideInputTextBox();
                    if (EditorManager.SelectedTool == EditTool.ERASER) {
                        CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventDelete(selected, internal_id));
                        EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
                        setEdited(true);
                        EditorManager.itemSelection.clearEvent();
                        return;
#if ENABLE_SCRIPT
                    } else if (EditorManager.SelectedTool == EditTool.PALETTE_TOOL) {
                        List<int> internal_ids = new List<int>();
                        foreach (var see in EditorManager.itemSelection.getEventIterator()) {
                            internal_ids.Add(see.original.InternalID);
                        }
                        var btn = e.Button;
                        if (isMouseMiddleButtonDowned(btn)) {
                            btn = NMouseButtons.Middle;
                        }
                        bool result = PaletteToolServer.invokePaletteTool(EditorManager.mSelectedPaletteTool,
                                                                              selected,
                                                                              internal_ids.ToArray(),
                                                                              (Cadencii.Gui.MouseButtons) btn);
                        if (result) {
                            setEdited(true);
                            EditorManager.itemSelection.clearEvent();
                            return;
                        }
#endif
                    }
                } else {
                    if (edit_mode != EditMode.MOVE_ENTRY_WAIT_MOVE &&
                         edit_mode != EditMode.MOVE_ENTRY &&
                         edit_mode != EditMode.MOVE_ENTRY_WHOLE_WAIT_MOVE &&
                         edit_mode != EditMode.MOVE_ENTRY_WHOLE &&
                         edit_mode != EditMode.EDIT_LEFT_EDGE &&
                         edit_mode != EditMode.EDIT_RIGHT_EDGE &&
                         edit_mode != EditMode.EDIT_VIBRATO_DELAY) {
                        if (!EditorManager.mIsPointerDowned) {
                            EditorManager.itemSelection.clearEvent();
                        }
                        hideInputTextBox();
                    }
                    if (EditorManager.SelectedTool == EditTool.ERASER) {
                        // マウス位置にビブラートの波波があったら削除する
                        int stdx = controller.getStartToDrawX();
                        int stdy = controller.getStartToDrawY();
                        for (int i = 0; i < EditorManager.mDrawObjects[selected - 1].Count; i++) {
                            DrawObject dobj = EditorManager.mDrawObjects[selected - 1][i];
                            if (dobj.mRectangleInPixel.X + controller.getStartToDrawX() + dobj.mRectangleInPixel.Width - stdx < 0) {
                                continue;
                            } else if (pictPianoRoll.Width < dobj.mRectangleInPixel.X + EditorManager.keyWidth - stdx) {
                                break;
                            }
                            Rectangle rc = new Rectangle(dobj.mRectangleInPixel.X + EditorManager.keyWidth + dobj.mVibratoDelayInPixel - stdx,
                                                          dobj.mRectangleInPixel.Y + (int)(100 * controller.getScaleY()) - stdy,
                                                          dobj.mRectangleInPixel.Width - dobj.mVibratoDelayInPixel,
                                                          (int)(100 * controller.getScaleY()));
                            if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                //ビブラートの範囲なのでビブラートを消す
                                VsqEvent item3 = null;
                                VsqID item2 = null;
                                int internal_id = -1;
                                internal_id = dobj.mInternalID;
                                foreach (var ve in MusicManager.getVsqFile().Track[selected].getNoteEventIterator()) {
                                    if (ve.InternalID == dobj.mInternalID) {
                                        item2 = (VsqID)ve.ID.clone();
                                        item3 = ve;
                                        break;
                                    }
                                }
                                if (item2 != null) {
                                    item2.VibratoHandle = null;
                                    CadenciiCommand run = new CadenciiCommand(
                                        VsqCommand.generateCommandEventChangeIDContaints(selected,
                                                                                          internal_id,
                                                                                          item2));
                                    EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
                                    setEdited(true);
                                }
                                break;
                            }
                        }
                    }
                }
            } else if (e.Button == NMouseButtons.Right) {
                bool show_context_menu = (e.X > EditorManager.keyWidth);
#if ENABLE_MOUSEHOVER
                if ( mMouseHoverThread != null ) {
                    if ( !mMouseHoverThread.IsAlive && EditorManager.editorConfig.PlayPreviewWhenRightClick ) {
                        show_context_menu = false;
                    }
                } else {
                    if ( EditorManager.editorConfig.PlayPreviewWhenRightClick ) {
                        show_context_menu = false;
                    }
                }
#endif
                show_context_menu = EditorManager.showContextMenuWhenRightClickedOnPianoroll ? (show_context_menu && !mMouseMoved) : false;
                if (show_context_menu) {
#if ENABLE_MOUSEHOVER
                    if ( mMouseHoverThread != null ) {
                        mMouseHoverThread.Abort();
                    }
#endif
                    ByRef<Rectangle> out_id_rect = new ByRef<Rectangle>();
                    VsqEvent item = getItemAtClickedPosition(new Point(e.X, e.Y), out_id_rect);
                    Rectangle id_rect = out_id_rect.value;
                    if (item != null) {
                        if (!EditorManager.itemSelection.isEventContains(EditorManager.Selected, item.InternalID)) {
                            EditorManager.itemSelection.clearEvent();
                        }
                        EditorManager.itemSelection.addEvent(item.InternalID);
                    }
                    bool item_is_null = (item == null);
                    cMenuPianoCopy.Enabled = !item_is_null;
                    cMenuPianoCut.Enabled = !item_is_null;
                    cMenuPianoDelete.Enabled = !item_is_null;
                    cMenuPianoImportLyric.Enabled = !item_is_null;
                    cMenuPianoExpressionProperty.Enabled = !item_is_null;

                    int clock = EditorManager.clockFromXCoord(e.X);
                    cMenuPianoPaste.Enabled = ((EditorManager.clipboard.getCopiedItems().events.Count != 0) && (clock >= MusicManager.getVsqFile().getPreMeasureClocks()));
                    refreshScreen();

                    mContextMenuOpenedPosition = new Point(e.X, e.Y);
                    cMenuPiano.Show(pictPianoRoll, e.X, e.Y);
                } else {
                    ByRef<Rectangle> out_id_rect = new ByRef<Rectangle>();
                    VsqEvent item = getItemAtClickedPosition(mButtonInitial, out_id_rect);
                    Rectangle id_rect = out_id_rect.value;
#if DEBUG
                    CDebug.WriteLine("pitcPianoRoll_MouseClick; button is right; (item==null)=" + (item == null));
#endif
                    if (item != null) {
                        int itemx = EditorManager.xCoordFromClocks(item.Clock);
                        int itemy = EditorManager.yCoordFromNote(item.ID.Note);
                    }
                }
            } else if (e.Button == NMouseButtons.Middle) {
#if ENABLE_SCRIPT
                if (EditorManager.SelectedTool == EditTool.PALETTE_TOOL) {
                    ByRef<Rectangle> out_id_rect = new ByRef<Rectangle>();
                    VsqEvent item = getItemAtClickedPosition(new Point(e.X, e.Y), out_id_rect);
                    Rectangle id_rect = out_id_rect.value;
                    if (item != null) {
                        EditorManager.itemSelection.clearEvent();
                        EditorManager.itemSelection.addEvent(item.InternalID);
                        List<int> internal_ids = new List<int>();
                        foreach (var see in EditorManager.itemSelection.getEventIterator()) {
                            internal_ids.Add(see.original.InternalID);
                        }
                        bool result = PaletteToolServer.invokePaletteTool(EditorManager.mSelectedPaletteTool,
                                                                           EditorManager.Selected,
                                                                           internal_ids.ToArray(),
                                                                           (Cadencii.Gui.MouseButtons) e.Button);
                        if (result) {
                            setEdited(true);
                            EditorManager.itemSelection.clearEvent();
                            return;
                        }
                    }
                }
#endif
            }
        }

        public void pictPianoRoll_MouseDoubleClick(Object sender, NMouseEventArgs e)
        {
#if DEBUG
            CDebug.WriteLine("FormMain#pictPianoRoll_MouseDoubleClick");
#endif
            ByRef<Rectangle> out_rect = new ByRef<Rectangle>();
            VsqEvent item = getItemAtClickedPosition(new Point(e.X, e.Y), out_rect);
            Rectangle rect = out_rect.value;
            int selected = EditorManager.Selected;
            VsqFileEx vsq = MusicManager.getVsqFile();
            if (item != null && item.ID.type == VsqIDType.Anote) {
#if ENABLE_SCRIPT
                if (EditorManager.SelectedTool != EditTool.PALETTE_TOOL)
#endif

 {
                    EditorManager.itemSelection.clearEvent();
                    EditorManager.itemSelection.addEvent(item.InternalID);
#if ENABLE_MOUSEHOVER
                    mMouseHoverThread.Abort();
#endif
                    if (!EditorManager.editorConfig.KeepLyricInputMode) {
                        mLastSymbolEditMode = false;
                    }
                    showInputTextBox(
                        item.ID.LyricHandle.L0.Phrase,
                        item.ID.LyricHandle.L0.getPhoneticSymbol(),
                        new Point(rect.X, rect.Y),
                        mLastSymbolEditMode);
                    refreshScreen();
                    return;
                }
            } else {
                EditorManager.itemSelection.clearEvent();
                hideInputTextBox();
                if (EditorManager.editorConfig.ShowExpLine && EditorManager.keyWidth <= e.X) {
                    int stdx = controller.getStartToDrawX();
                    int stdy = controller.getStartToDrawY();
                    foreach (var dobj in EditorManager.mDrawObjects[selected - 1]) {
                        // 表情コントロールプロパティを表示するかどうかを決める
                        rect = new Rectangle(
                            dobj.mRectangleInPixel.X + EditorManager.keyWidth - stdx,
                            dobj.mRectangleInPixel.Y - stdy + (int)(100 * controller.getScaleY()),
                            21,
                            (int)(100 * controller.getScaleY()));
                        if (Utility.isInRect(new Point(e.X, e.Y), rect)) {
                            VsqEvent selectedEvent = null;
                            for (Iterator<VsqEvent> itr2 = vsq.Track[selected].getEventIterator(); itr2.hasNext(); ) {
                                VsqEvent ev = itr2.next();
                                if (ev.InternalID == dobj.mInternalID) {
                                    selectedEvent = ev;
                                    break;
                                }
                            }
                            if (selectedEvent != null) {
#if ENABLE_MOUSEHOVER
                                if ( mMouseHoverThread != null ) {
                                    mMouseHoverThread.Abort();
                                }
#endif
                                SynthesizerType type = SynthesizerType.VOCALOID2;
                                RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
                                if (kind == RendererKind.VOCALOID1) {
                                    type = SynthesizerType.VOCALOID1;
                                }
                                FormNoteExpressionConfig dlg = null;
                                try {
                                    dlg = ApplicationUIHost.Create<FormNoteExpressionConfig>(type, selectedEvent.ID.NoteHeadHandle);
                                    dlg.PMBendDepth = (selectedEvent.ID.PMBendDepth);
                                    dlg.PMBendLength = (selectedEvent.ID.PMBendLength);
                                    dlg.PMbPortamentoUse = (selectedEvent.ID.PMbPortamentoUse);
                                    dlg.DEMdecGainRate = (selectedEvent.ID.DEMdecGainRate);
                                    dlg.DEMaccent = (selectedEvent.ID.DEMaccent);
                                    dlg.Location = model.GetFormPreferedLocation(dlg);
                                    var dr = DialogManager.showModalDialog(dlg, this);
					if (dr == 1) {
                                        VsqID id = (VsqID)selectedEvent.ID.clone();
                                        id.PMBendDepth = dlg.PMBendDepth;
                                        id.PMBendLength = dlg.PMBendLength;
                                        id.PMbPortamentoUse = dlg.PMbPortamentoUse;
                                        id.DEMdecGainRate = dlg.DEMdecGainRate;
                                        id.DEMaccent = dlg.DEMaccent;
                                        id.NoteHeadHandle = dlg.EditedNoteHeadHandle;
                                        CadenciiCommand run = new CadenciiCommand(
                                            VsqCommand.generateCommandEventChangeIDContaints(selected, selectedEvent.InternalID, id));
                                        EditorManager.editHistory.register(vsq.executeCommand(run));
                                        setEdited(true);
                                        refreshScreen();
                                    }
                                } catch (Exception ex) {
                                    Logger.write(typeof(FormMain) + ".pictPianoRoll_MouseDoubleClick; ex=" + ex + "\n");
                                    serr.println(typeof(FormMain) + ".pictPianoRoll_MouseDoubleClick" + ex);
                                } finally {
                                    if (dlg != null) {
                                        try {
                                            dlg.Close();
                                        } catch (Exception ex2) {
                                            Logger.write(typeof(FormMain) + ".pictPianoRoll_MouseDoubleClick; ex=" + ex2 + "\n");
                                            serr.println(typeof(FormMain) + ".pictPianoRoll_MouseDoubleClick");
                                        }
                                    }
                                }
                                return;
                            }
                            break;
                        }

                        // ビブラートプロパティダイアログを表示するかどうかを決める
                        rect = new Rectangle(
                            dobj.mRectangleInPixel.X + EditorManager.keyWidth - stdx + 21,
                            dobj.mRectangleInPixel.Y - stdy + (int)(100 * controller.getScaleY()),
                            dobj.mRectangleInPixel.Width - 21,
                            (int)(100 * controller.getScaleY()));
                        if (Utility.isInRect(new Point(e.X, e.Y), rect)) {
                            VsqEvent selectedEvent = null;
                            for (Iterator<VsqEvent> itr2 = vsq.Track[selected].getEventIterator(); itr2.hasNext(); ) {
                                VsqEvent ev = itr2.next();
                                if (ev.InternalID == dobj.mInternalID) {
                                    selectedEvent = ev;
                                    break;
                                }
                            }
                            if (selectedEvent != null) {
#if ENABLE_MOUSEHOVER
                                if ( mMouseHoverThread != null ) {
                                    mMouseHoverThread.Abort();
                                }
#endif
                                SynthesizerType type = SynthesizerType.VOCALOID2;
                                RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
#if DEBUG
                                sout.println("FormMain#pictPianoRoll_MouseDoubleClick; kind=" + kind);
#endif
                                if (kind == RendererKind.VOCALOID1) {
                                    type = SynthesizerType.VOCALOID1;
                                }
                                FormVibratoConfig dlg = null;
                                try {
                                    dlg = ApplicationUIHost.Create<FormVibratoConfig>(
                                        selectedEvent.ID.VibratoHandle,
                                        selectedEvent.ID.getLength(),
                                        ApplicationGlobal.appConfig.DefaultVibratoLength,
                                        type,
                                        ApplicationGlobal.appConfig.UseUserDefinedAutoVibratoType);
                                    dlg.Location = model.GetFormPreferedLocation(dlg);
                                    var dr = DialogManager.showModalDialog(dlg, this);
									if (dr == 1) {
                                        VsqID t = (VsqID)selectedEvent.ID.clone();
                                        VibratoHandle handle = dlg.getVibratoHandle();
#if DEBUG
                                        sout.println("FormMain#pictPianoRoll_MouseDoubleClick; (handle==null)=" + (handle == null));
#endif
                                        if (handle != null) {
                                            string iconid = handle.IconID;
                                            int vibrato_length = handle.getLength();
                                            int note_length = selectedEvent.ID.getLength();
                                            t.VibratoDelay = note_length - vibrato_length;
                                            t.VibratoHandle = handle;
                                        } else {
                                            t.VibratoHandle = null;
                                        }
                                        CadenciiCommand run = new CadenciiCommand(
                                            VsqCommand.generateCommandEventChangeIDContaints(
                                                selected,
                                                selectedEvent.InternalID,
                                                t));
                                        EditorManager.editHistory.register(vsq.executeCommand(run));
                                        setEdited(true);
                                        refreshScreen();
                                    }
                                } catch (Exception ex) {
                                    Logger.write(typeof(FormMain) + ".pictPianoRoll_MouseDoubleClick; ex=" + ex + "\n");
                                } finally {
                                    if (dlg != null) {
                                        try {
                                            dlg.Close();
                                        } catch (Exception ex2) {
                                            Logger.write(typeof(FormMain) + ".pictPianoRoll_MouseDoubleClick; ex=" + ex2 + "\n");
                                        }
                                    }
                                }
                                return;
                            }
                            break;
                        }

                    }
                }
            }

            if (e.Button == NMouseButtons.Left) {
                // 必要な操作が何も無ければ，クリック位置にソングポジションを移動
                if (EditorManager.keyWidth < e.X) {
                    int clock = FormMainModel.Quantize(EditorManager.clockFromXCoord(e.X), EditorManager.getPositionQuantizeClock());
                    EditorManager.setCurrentClock(clock);
                }
            } else if (e.Button == NMouseButtons.Middle) {
                // ツールをポインター <--> 鉛筆に切り替える
                if (EditorManager.keyWidth < e.X) {
                    if (EditorManager.SelectedTool == EditTool.ARROW) {
                        EditorManager.SelectedTool = EditTool.PENCIL;
                    } else {
                        EditorManager.SelectedTool = EditTool.ARROW;
                    }
                }
            }
        }

        public void pictPianoRoll_MouseDown(Object sender, NMouseEventArgs e0)
        {
#if DEBUG
            CDebug.WriteLine("pictPianoRoll_MouseDown");
#endif
            var btn0 = e0.Button;
            if (isMouseMiddleButtonDowned(btn0)) {
                btn0 = NMouseButtons.Middle;
            }
            var e = new NMouseEventArgs(btn0, e0.Clicks, e0.X, e0.Y, e0.Delta);

            mMouseMoved = false;
            if (!EditorManager.isPlaying() && 0 <= e.X && e.X <= EditorManager.keyWidth) {
                int note = EditorManager.noteFromYCoord(e.Y);
                if (0 <= note && note <= 126) {
                    if (e.Button == NMouseButtons.Left) {
                        KeySoundPlayer.play(note);
                    }
                    return;
                }
            }

            EditorManager.itemSelection.clearTempo();
            EditorManager.itemSelection.clearTimesig();
            EditorManager.itemSelection.clearPoint();
            /*if ( e.Button == BMouseButtons.Left ) {
                EditorManager.selectedRegionEnabled = false;
            }*/

            mMouseDowned = true;
            mButtonInitial = new Point(e.X, e.Y);
            Keys modefier = (Keys) Control.ModifierKeys;

            EditTool selected_tool = EditorManager.SelectedTool;
#if ENABLE_SCRIPT
            if (selected_tool != EditTool.PALETTE_TOOL && e.Button == NMouseButtons.Middle) {
#else
            if ( e.Button == BMouseButtons.Middle ) {
#endif
                EditorManager.EditMode =EditMode.MIDDLE_DRAG;
                mMiddleButtonVScroll = vScroll.Value;
                mMiddleButtonHScroll = hScroll.Value;
                return;
            }

            int stdx = controller.getStartToDrawX();
            int stdy = controller.getStartToDrawY();
            if (e.Button == NMouseButtons.Left && EditorManager.mCurveOnPianoroll && (selected_tool == EditTool.PENCIL || selected_tool == EditTool.LINE)) {
                pictPianoRoll.mMouseTracer.clear();
                pictPianoRoll.mMouseTracer.appendFirst(e.X + stdx, e.Y + stdy);
                this.Cursor = Cursors.Default;
                EditorManager.EditMode =EditMode.CURVE_ON_PIANOROLL;
                return;
            }

            ByRef<Rectangle> out_rect = new ByRef<Rectangle>();
            VsqEvent item = getItemAtClickedPosition(new Point(e.X, e.Y), out_rect);
            Rectangle rect = out_rect.value;

#if ENABLE_SCRIPT
            if (selected_tool == EditTool.PALETTE_TOOL && item == null && e.Button == NMouseButtons.Middle) {
                EditorManager.EditMode =EditMode.MIDDLE_DRAG;
                mMiddleButtonVScroll = vScroll.Value;
                mMiddleButtonHScroll = hScroll.Value;
                return;
            }
#endif

            int selected = EditorManager.Selected;
            VsqFileEx vsq = MusicManager.getVsqFile();
            VsqTrack vsq_track = vsq.Track[selected];
            int key_width = EditorManager.keyWidth;

            // マウス位置にある音符を検索
            if (item == null) {
                if (e.Button == NMouseButtons.Left) {
                    EditorManager.IsWholeSelectedIntervalEnabled = false;
                }
                #region 音符がなかった時
#if DEBUG
                CDebug.WriteLine("    No Event");
#endif
                if (EditorManager.itemSelection.getLastEvent() != null) {
                    executeLyricChangeCommand();
                }
                bool start_mouse_hover_generator = true;

                // CTRLキーを押しながら範囲選択
                if ((modefier & s_modifier_key) == s_modifier_key) {
                    EditorManager.IsWholeSelectedIntervalEnabled = true;
                    EditorManager.IsCurveSelectedIntervalEnabled = false;
                    EditorManager.itemSelection.clearPoint();
                    int startClock = EditorManager.clockFromXCoord(e.X);
                    if (EditorManager.editorConfig.CurveSelectingQuantized) {
                        int unit = EditorManager.getPositionQuantizeClock();
                        startClock = FormMainModel.Quantize(startClock, unit);
                    }
                    EditorManager.mWholeSelectedInterval = new SelectedRegion(startClock);
                    EditorManager.mWholeSelectedInterval.setEnd(startClock);
                    EditorManager.mIsPointerDowned = true;
                } else {
                    DrawObject vibrato_dobj = null;
                    if (selected_tool == EditTool.LINE || selected_tool == EditTool.PENCIL) {
                        // ビブラート範囲の編集
                        int px_vibrato_length = 0;
                        mVibratoEditingId = -1;
                        Rectangle pxFound = new Rectangle();
                        List<DrawObject> target_list = EditorManager.mDrawObjects[selected - 1];
                        int count = target_list.Count;
                        for (int i = 0; i < count; i++) {
                            DrawObject dobj = target_list[i];
                            if (dobj.mRectangleInPixel.Width <= dobj.mVibratoDelayInPixel) {
                                continue;
                            }
                            if (dobj.mRectangleInPixel.X + key_width + dobj.mRectangleInPixel.Width - stdx < 0) {
                                continue;
                            } else if (pictPianoRoll.Width < dobj.mRectangleInPixel.X + key_width - stdx) {
                                break;
                            }
                            Rectangle rc = new Rectangle(dobj.mRectangleInPixel.X + key_width + dobj.mVibratoDelayInPixel - stdx - _EDIT_HANDLE_WIDTH / 2,
                                                dobj.mRectangleInPixel.Y + (int)(100 * controller.getScaleY()) - stdy,
                                                _EDIT_HANDLE_WIDTH,
                                                (int)(100 * controller.getScaleY()));
                            if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                vibrato_dobj = dobj;
                                //vibrato_found = true;
                                mVibratoEditingId = dobj.mInternalID;
                                pxFound.X = dobj.mRectangleInPixel.X;
                                pxFound.Y = dobj.mRectangleInPixel.Y;
                                pxFound.Width = dobj.mRectangleInPixel.Width;
                                pxFound.Height = dobj.mRectangleInPixel.Height;// = new Rectangle dobj.mRectangleInPixel;
                                pxFound.X += key_width;
                                px_vibrato_length = dobj.mRectangleInPixel.Width - dobj.mVibratoDelayInPixel;
                                break;
                            }
                        }
                        if (vibrato_dobj != null) {
                            int clock = EditorManager.clockFromXCoord(pxFound.X + pxFound.Width - px_vibrato_length - stdx);
                            int note = vibrato_dobj.mNote - 1;// EditorManager.noteFromYCoord( pxFound.y + (int)(100 * EditorManager.getScaleY()) - stdy );
                            int length = vibrato_dobj.mClock + vibrato_dobj.mLength - clock;// (int)(pxFound.Width * EditorManager.getScaleXInv());
                            EditorManager.mAddingEvent = new VsqEvent(clock, new VsqID(0));
                            EditorManager.mAddingEvent.ID.type = VsqIDType.Anote;
                            EditorManager.mAddingEvent.ID.Note = note;
                            EditorManager.mAddingEvent.ID.setLength(length);
                            EditorManager.mAddingEventLength = vibrato_dobj.mLength;
                            EditorManager.mAddingEvent.ID.VibratoDelay = length - (int)(px_vibrato_length * controller.getScaleXInv());
                            EditorManager.EditMode = EditMode.EDIT_VIBRATO_DELAY;
                            start_mouse_hover_generator = false;
                        }
                    }
                    if (vibrato_dobj == null) {
                        if ((selected_tool == EditTool.PENCIL || selected_tool == EditTool.LINE) &&
                            e.Button == NMouseButtons.Left &&
                            e.X >= key_width) {
                            int clock = EditorManager.clockFromXCoord(e.X);
                            if (MusicManager.getVsqFile().getPreMeasureClocks() - EditorManager.editorConfig.PxTolerance * controller.getScaleXInv() <= clock) {
                                //10ピクセルまでは許容範囲
                                if (MusicManager.getVsqFile().getPreMeasureClocks() > clock) { //だけど矯正するよ。
                                    clock = MusicManager.getVsqFile().getPreMeasureClocks();
                                }
                                int note = EditorManager.noteFromYCoord(e.Y);
                                EditorManager.itemSelection.clearEvent();
                                int unit = EditorManager.getPositionQuantizeClock();
                                int new_clock = FormMainModel.Quantize(clock, unit);
                                EditorManager.mAddingEvent = new VsqEvent(new_clock, new VsqID(0));
                                // デフォルトの歌唱スタイルを適用する
                                EditorManager.editorConfig.applyDefaultSingerStyle(EditorManager.mAddingEvent.ID);
                                if (mPencilMode.getMode() == PencilModeEnum.Off) {
                                    EditorManager.EditMode = EditMode.ADD_ENTRY;
                                    mButtonInitial = new Point(e.X, e.Y);
                                    EditorManager.mAddingEvent.ID.setLength(0);
                                    EditorManager.mAddingEvent.ID.Note = note;
                                    this.Cursor = Cursors.Default;
#if DEBUG
                                    CDebug.WriteLine("    EditMode=" + EditorManager.EditMode);
#endif
                                } else {
                                    EditorManager.EditMode = EditMode.ADD_FIXED_LENGTH_ENTRY;
                                    EditorManager.mAddingEvent.ID.setLength(mPencilMode.getUnitLength());
                                    EditorManager.mAddingEvent.ID.Note = note;
                                    this.Cursor = Cursors.Default;
                                }
                            } else {
                                SystemSounds.Asterisk.Play();
                            }
#if ENABLE_SCRIPT
                        } else if ((selected_tool == EditTool.ARROW || selected_tool == EditTool.PALETTE_TOOL) && e.Button == NMouseButtons.Left) {
#else
                        } else if ( (selected_tool == EditTool.ARROW) && e.Button == BMouseButtons.Left ) {
#endif
                            EditorManager.IsWholeSelectedIntervalEnabled = false;
                            EditorManager.itemSelection.clearEvent();
                            EditorManager.mMouseDownLocation = new Point(e.X + stdx, e.Y + stdy);
                            EditorManager.mIsPointerDowned = true;
#if DEBUG
                            CDebug.WriteLine("    EditMode=" + EditorManager.EditMode);
#endif
                        }
                    }
                }
                if (e.Button == NMouseButtons.Right && !EditorManager.editorConfig.PlayPreviewWhenRightClick) {
                    start_mouse_hover_generator = false;
                }
#if ENABLE_MOUSEHOVER
                if ( start_mouse_hover_generator ) {
                    mMouseHoverThread = new Thread( new ParameterizedThreadStart( MouseHoverEventGenerator ) );
                    mMouseHoverThread.Start( EditorManager.noteFromYCoord( e.Y ) );
                }
#endif
                #endregion
            } else {
                #region 音符があった時
#if DEBUG
                CDebug.WriteLine("    Event Found");
#endif
                if (EditorManager.itemSelection.isEventContains(selected, item.InternalID)) {
                    executeLyricChangeCommand();
                }
                hideInputTextBox();
                if (selected_tool != EditTool.ERASER) {
#if ENABLE_MOUSEHOVER
                    mMouseHoverThread = new Thread( new ParameterizedThreadStart( MouseHoverEventGenerator ) );
                    mMouseHoverThread.Start( item.ID.Note );
#endif
                }

                // まず、両端の編集モードに移行可能かどうか調べる
                if (item.ID.type != VsqIDType.Aicon ||
                     (item.ID.type == VsqIDType.Aicon && !item.ID.IconDynamicsHandle.isDynaffType())) {
#if ENABLE_SCRIPT
                    if (selected_tool != EditTool.ERASER && selected_tool != EditTool.PALETTE_TOOL && e.Button == NMouseButtons.Left) {
#else
                    if ( selected_tool != EditTool.ERASER && e.Button == BMouseButtons.Left ) {
#endif
                        int min_width = 4 * _EDIT_HANDLE_WIDTH;
                        foreach (var dobj in EditorManager.mDrawObjects[selected - 1]) {
                            int edit_handle_width = _EDIT_HANDLE_WIDTH;
                            if (dobj.mRectangleInPixel.Width < min_width) {
                                edit_handle_width = dobj.mRectangleInPixel.Width / 4;
                            }

                            // 左端の"のり代"にマウスがあるかどうか
                            Rectangle rc = new Rectangle(dobj.mRectangleInPixel.X - stdx + key_width,
                                                          dobj.mRectangleInPixel.Y - stdy,
                                                          edit_handle_width,
                                                          dobj.mRectangleInPixel.Height);
                            if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                EditorManager.IsWholeSelectedIntervalEnabled = false;
                                EditorManager.EditMode = EditMode.EDIT_LEFT_EDGE;
                                if (!EditorManager.itemSelection.isEventContains(selected, item.InternalID)) {
                                    EditorManager.itemSelection.clearEvent();
                                }
                                EditorManager.itemSelection.addEvent(item.InternalID);
                                this.Cursor = System.Windows.Forms.Cursors.VSplit;
                                refreshScreen();
#if DEBUG
                                CDebug.WriteLine("    EditMode=" + EditorManager.EditMode);
#endif
                                return;
                            }

                            // 右端の糊代にマウスがあるかどうか
                            rc = new Rectangle(dobj.mRectangleInPixel.X + key_width + dobj.mRectangleInPixel.Width - stdx - edit_handle_width,
                                                dobj.mRectangleInPixel.Y - stdy,
                                                edit_handle_width,
                                                dobj.mRectangleInPixel.Height);
                            if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                EditorManager.IsWholeSelectedIntervalEnabled = false;
                                EditorManager.EditMode = EditMode.EDIT_RIGHT_EDGE;
                                if (!EditorManager.itemSelection.isEventContains(selected, item.InternalID)) {
                                    EditorManager.itemSelection.clearEvent();
                                }
                                EditorManager.itemSelection.addEvent(item.InternalID);
                                this.Cursor = System.Windows.Forms.Cursors.VSplit;
                                refreshScreen();
                                return;
                            }
                        }
                    }
                }

                if (e.Button == NMouseButtons.Left || e.Button == NMouseButtons.Middle) {
#if ENABLE_SCRIPT
                    if (selected_tool == EditTool.PALETTE_TOOL) {
                        EditorManager.IsWholeSelectedIntervalEnabled = false;
                        EditorManager.EditMode = EditMode.NONE;
                        EditorManager.itemSelection.clearEvent();
                        EditorManager.itemSelection.addEvent(item.InternalID);
                    } else
#endif
                        if (selected_tool != EditTool.ERASER) {
                            mMouseMoveInit = new Point(e.X + stdx, e.Y + stdy);
                            int head_x = EditorManager.xCoordFromClocks(item.Clock);
                            mMouseMoveOffset = e.X - head_x;
                            if ((modefier & Keys.Shift) == Keys.Shift) {
                                // シフトキー同時押しによる範囲選択
                                List<int> add_required = new List<int>();
                                add_required.Add(item.InternalID);

                                // 現在の選択アイテムがある場合，
                                // 直前に選択したアイテムと，現在選択しようとしているアイテムとの間にあるアイテムを
                                // 全部選択する
                                SelectedEventEntry sel = EditorManager.itemSelection.getLastEvent();
                                if (sel != null) {
                                    int last_id = sel.original.InternalID;
                                    int last_clock = 0;
                                    int this_clock = 0;
                                    bool this_found = false, last_found = false;
                                    for (Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
                                        VsqEvent ev = itr.next();
                                        if (ev.InternalID == last_id) {
                                            last_clock = ev.Clock;
                                            last_found = true;
                                        } else if (ev.InternalID == item.InternalID) {
                                            this_clock = ev.Clock;
                                            this_found = true;
                                        }
                                        if (last_found && this_found) {
                                            break;
                                        }
                                    }
                                    int start = Math.Min(last_clock, this_clock);
                                    int end = Math.Max(last_clock, this_clock);
                                    for (Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
                                        VsqEvent ev = itr.next();
                                        if (start <= ev.Clock && ev.Clock <= end) {
                                            if (!add_required.Contains(ev.InternalID)) {
                                                add_required.Add(ev.InternalID);
                                            }
                                        }
                                    }
                                }
                                EditorManager.itemSelection.addEventAll(add_required);
                            } else if ((modefier & s_modifier_key) == s_modifier_key) {
                                // CTRLキーを押しながら選択／選択解除
                                if (EditorManager.itemSelection.isEventContains(selected, item.InternalID)) {
                                    EditorManager.itemSelection.removeEvent(item.InternalID);
                                } else {
                                    EditorManager.itemSelection.addEvent(item.InternalID);
                                }
                            } else {
                                if (!EditorManager.itemSelection.isEventContains(selected, item.InternalID)) {
                                    // MouseDownしたアイテムが、まだ選択されていなかった場合。当該アイテム単独に選択しなおす
                                    EditorManager.itemSelection.clearEvent();
                                }
                                EditorManager.itemSelection.addEvent(item.InternalID);
                            }

                            // 範囲選択モードで、かつマウス位置の音符がその範囲に入っていた場合にのみ、MOVE_ENTRY_WHOLE_WAIT_MOVEに移行
                            if (EditorManager.IsWholeSelectedIntervalEnabled &&
                                 EditorManager.mWholeSelectedInterval.getStart() <= item.Clock &&
                                 item.Clock <= EditorManager.mWholeSelectedInterval.getEnd()) {
                                EditorManager.EditMode = EditMode.MOVE_ENTRY_WHOLE_WAIT_MOVE;
                                EditorManager.mWholeSelectedIntervalStartForMoving = EditorManager.mWholeSelectedInterval.getStart();
                            } else {
                                EditorManager.IsWholeSelectedIntervalEnabled = false;
                                EditorManager.EditMode = EditMode.MOVE_ENTRY_WAIT_MOVE;
                            }

                            this.Cursor = Cursors.Hand;
#if DEBUG
                            CDebug.WriteLine("    EditMode=" + EditorManager.EditMode);
                            CDebug.WriteLine("    m_config.SelectedEvent.Count=" + EditorManager.itemSelection.getEventCount());
#endif
                        }
                }
                #endregion
            }
            refreshScreen();
        }

        public void pictPianoRoll_MouseMove(Object sender, NMouseEventArgs e)
        {
            lock (EditorManager.mDrawObjects) {
                if (mFormActivated) {
#if ENABLE_PROPERTY
                    if (EditorManager.InputTextBox != null && !EditorManager.InputTextBox.IsDisposed && !EditorManager.InputTextBox.Visible && !EditorManager.propertyPanel.isEditing()) {
#else
                    if (EditorManager.InputTextBox != null && !EditorManager.InputTextBox.IsDisposed && !EditorManager.InputTextBox.Visible) {
#endif
                        focusPianoRoll();
                    }
                }

                EditMode edit_mode = EditorManager.EditMode;
                int stdx = controller.getStartToDrawX();
                int stdy = controller.getStartToDrawY();
                int selected = EditorManager.Selected;
                EditTool selected_tool = EditorManager.SelectedTool;

                if (edit_mode == EditMode.CURVE_ON_PIANOROLL && EditorManager.mCurveOnPianoroll) {
                    pictPianoRoll.mMouseTracer.append(e.X + stdx, e.Y + stdy);
                    if (!timer.Enabled) {
                        refreshScreen();
                    }
                    return;
                }

                if (!mMouseMoved && edit_mode == EditMode.MIDDLE_DRAG) {
                    this.Cursor = HAND;
                }

                if (e.X != mButtonInitial.X || e.Y != mButtonInitial.Y) {
                    mMouseMoved = true;
                }
                if (!(edit_mode == EditMode.MIDDLE_DRAG) && EditorManager.isPlaying()) {
                    return;
                }

                if (edit_mode == EditMode.MOVE_ENTRY_WAIT_MOVE ||
                     edit_mode == EditMode.MOVE_ENTRY_WHOLE_WAIT_MOVE) {
                    int x = e.X + stdx;
                    int y = e.Y + stdy;
                    if (mMouseMoveInit.X != x || mMouseMoveInit.Y != y) {
                        if (edit_mode == EditMode.MOVE_ENTRY_WAIT_MOVE) {
                            EditorManager.EditMode = EditMode.MOVE_ENTRY;
                            edit_mode = EditMode.MOVE_ENTRY;
                        } else {
                            EditorManager.EditMode = EditMode.MOVE_ENTRY_WHOLE;
                            edit_mode = EditMode.MOVE_ENTRY_WHOLE;
                        }
                    }
                }

#if ENABLE_MOUSEHOVER
                if (mMouseMoved && mMouseHoverThread != null) {
                    mMouseHoverThread.Abort();
                }
#endif

                int clock = EditorManager.clockFromXCoord(e.X);
                if (mMouseDowned) {
                    if (mExtDragX == ExtDragXMode.NONE) {
                        if (EditorManager.keyWidth > e.X) {
                            mExtDragX = ExtDragXMode.LEFT;
                        } else if (pictPianoRoll.Width < e.X) {
                            mExtDragX = ExtDragXMode.RIGHT;
                        }
                    } else {
                        if (EditorManager.keyWidth <= e.X && e.X <= pictPianoRoll.Width) {
                            mExtDragX = ExtDragXMode.NONE;
                        }
                    }

                    if (mExtDragY == ExtDragYMode.NONE) {
                        if (0 > e.Y) {
                            mExtDragY = ExtDragYMode.UP;
                        } else if (pictPianoRoll.Height < e.Y) {
                            mExtDragY = ExtDragYMode.DOWN;
                        }
                    } else {
                        if (0 <= e.Y && e.Y <= pictPianoRoll.Height) {
                            mExtDragY = ExtDragYMode.NONE;
                        }
                    }
                } else {
                    mExtDragX = ExtDragXMode.NONE;
                    mExtDragY = ExtDragYMode.NONE;
                }

                if (edit_mode == EditMode.MIDDLE_DRAG) {
                    mExtDragX = ExtDragXMode.NONE;
                    mExtDragY = ExtDragYMode.NONE;
                }

                double now = 0, dt = 0;
                if (mExtDragX != ExtDragXMode.NONE || mExtDragY != ExtDragYMode.NONE) {
                    now = PortUtil.getCurrentTime();
                    dt = now - mTimerDragLastIgnitted;
                }
                if (mExtDragX == ExtDragXMode.RIGHT || mExtDragX == ExtDragXMode.LEFT) {
                    int px_move = EditorManager.editorConfig.MouseDragIncrement;
                    if (px_move / dt > EditorManager.editorConfig.MouseDragMaximumRate) {
                        px_move = (int)(dt * EditorManager.editorConfig.MouseDragMaximumRate);
                    }
                    double d_draft;
                    if (mExtDragX == ExtDragXMode.LEFT) {
                        px_move *= -1;
                    }
                    int left_clock = EditorManager.clockFromXCoord(EditorManager.keyWidth);
                    float inv_scale_x = controller.getScaleXInv();
                    int dclock = (int)(px_move * inv_scale_x);
                    d_draft = 5 * inv_scale_x + left_clock + dclock;
                    if (d_draft < 0.0) {
                        d_draft = 0.0;
                    }
                    int draft = (int)d_draft;
                    if (hScroll.Maximum < draft) {
                        if (edit_mode == EditMode.ADD_ENTRY ||
                             edit_mode == EditMode.MOVE_ENTRY ||
                             edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY ||
                             edit_mode == EditMode.DRAG_DROP) {
                            hScroll.Maximum = draft;
                        } else {
                            draft = hScroll.Maximum;
                        }
                    }
                    if (draft < hScroll.Minimum) {
                        draft = hScroll.Minimum;
                    }
                    hScroll.Value = draft;
                }
                if (mExtDragY == ExtDragYMode.UP || mExtDragY == ExtDragYMode.DOWN) {
                    int min = vScroll.Minimum;
                    int max = vScroll.Maximum - vScroll.LargeChange;
                    int px_move = EditorManager.editorConfig.MouseDragIncrement;
                    if (px_move / dt > EditorManager.editorConfig.MouseDragMaximumRate) {
                        px_move = (int)(dt * EditorManager.editorConfig.MouseDragMaximumRate);
                    }
                    px_move += 50;
                    if (mExtDragY == ExtDragYMode.UP) {
                        px_move *= -1;
                    }
                    int draft = vScroll.Value + px_move;
                    if (draft < 0) {
                        draft = 0;
                    }
                    int df = (int)draft;
                    if (df < min) {
                        df = min;
                    } else if (max < df) {
                        df = max;
                    }
                    vScroll.Value = df;
                }
                if (mExtDragX != ExtDragXMode.NONE || mExtDragY != ExtDragYMode.NONE) {
                    mTimerDragLastIgnitted = now;
                }

                // 選択範囲にあるイベントを選択．
                if (EditorManager.mIsPointerDowned) {
                    if (EditorManager.IsWholeSelectedIntervalEnabled) {
                        int endClock = EditorManager.clockFromXCoord(e.X);
						if (EditorManager.editorConfig.CurveSelectingQuantized) {
                            int unit = EditorManager.getPositionQuantizeClock();
                            endClock = FormMainModel.Quantize(endClock, unit);
                        }
                        EditorManager.mWholeSelectedInterval.setEnd(endClock);
                    } else {
                        Point mouse = new Point(e.X + stdx, e.Y + stdy);
                        int tx, ty, twidth, theight;
                        int lx = EditorManager.mMouseDownLocation.X;
                        if (lx < mouse.X) {
                            tx = lx;
                            twidth = mouse.X - lx;
                        } else {
                            tx = mouse.X;
                            twidth = lx - mouse.X;
                        }
                        int ly = EditorManager.mMouseDownLocation.Y;
                        if (ly < mouse.Y) {
                            ty = ly;
                            theight = mouse.Y - ly;
                        } else {
                            ty = mouse.Y;
                            theight = ly - mouse.Y;
                        }

                        Rectangle rect = new Rectangle(tx, ty, twidth, theight);
                        List<int> add_required = new List<int>();
                        int internal_id = -1;
                        foreach (var dobj in EditorManager.mDrawObjects[selected - 1]) {
                            int x0 = dobj.mRectangleInPixel.X + EditorManager.keyWidth;
                            int x1 = dobj.mRectangleInPixel.X + EditorManager.keyWidth + dobj.mRectangleInPixel.Width;
                            int y0 = dobj.mRectangleInPixel.Y;
                            int y1 = dobj.mRectangleInPixel.Y + dobj.mRectangleInPixel.Height;
                            internal_id = dobj.mInternalID;
                            if (x1 < tx) {
                                continue;
                            }
                            if (tx + twidth < x0) {
                                break;
                            }
                            bool found = Utility.isInRect(new Point(x0, y0), rect) |
                                            Utility.isInRect(new Point(x0, y1), rect) |
                                            Utility.isInRect(new Point(x1, y0), rect) |
                                            Utility.isInRect(new Point(x1, y1), rect);
                            if (found) {
                                add_required.Add(internal_id);
                            } else {
                                if (x0 <= tx && tx + twidth <= x1) {
                                    if (ty < y0) {
                                        if (y0 <= ty + theight) {
                                            add_required.Add(internal_id);
                                        }
                                    } else if (y0 <= ty && ty < y1) {
                                        add_required.Add(internal_id);
                                    }
                                } else if (y0 <= ty && ty + theight <= y1) {
                                    if (tx < x0) {
                                        if (x0 <= tx + twidth) {
                                            add_required.Add(internal_id);
                                        }
                                    } else if (x0 <= tx && tx < x1) {
                                        add_required.Add(internal_id);
                                    }
                                }
                            }
                        }
                        List<int> remove_required = new List<int>();
                        foreach (var selectedEvent in EditorManager.itemSelection.getEventIterator()) {
                            if (!add_required.Contains(selectedEvent.original.InternalID)) {
                                remove_required.Add(selectedEvent.original.InternalID);
                            }
                        }
                        if (remove_required.Count > 0) {
                            EditorManager.itemSelection.removeEventRange(PortUtil.convertIntArray(remove_required.ToArray()));
                        }
                        add_required.RemoveAll((id) => EditorManager.itemSelection.isEventContains(selected, id));
                        EditorManager.itemSelection.addEventAll(add_required);
                    }
                }

                if (edit_mode == EditMode.MIDDLE_DRAG) {
                    #region MiddleDrag
                    int drafth = computeHScrollValueForMiddleDrag(e.X);
                    int draftv = computeVScrollValueForMiddleDrag(e.Y);
                    bool moved = false;
                    if (drafth != hScroll.Value) {
                        //moved = true;
                        //hScroll.beQuiet();
                        hScroll.Value = drafth;
                    }
                    if (draftv != vScroll.Value) {
                        //moved = true;
                        //vScroll.beQuiet();
                        vScroll.Value = draftv;
                    }
                    //if ( moved ) {
                    //    vScroll.setQuiet( false );
                    //    hScroll.setQuiet( false );
                    //    refreshScreen( true );
                    //}
                    refreshScreen(true);
                    if (EditorManager.isPlaying()) {
                        return;
                    }
                    #endregion
                    return;
                } else if (edit_mode == EditMode.ADD_ENTRY) {
                    #region ADD_ENTRY
                    int unit = EditorManager.getLengthQuantizeClock();
                    int length = clock - EditorManager.mAddingEvent.Clock;
                    int odd = length % unit;
                    int new_length = length - odd;

                    if (unit * controller.getScaleX() > 10) { //これをしないと、グリッド2個分増えることがある
                        int next_clock = EditorManager.clockFromXCoord(e.X + 10);
                        int next_length = next_clock - EditorManager.mAddingEvent.Clock;
                        int next_new_length = next_length - (next_length % unit);
                        if (next_new_length == new_length + unit) {
                            new_length = next_new_length;
                        }
                    }

                    if (new_length <= 0) {
                        new_length = 0;
                    }
                    EditorManager.mAddingEvent.ID.setLength(new_length);
                    #endregion
                } else if (edit_mode == EditMode.MOVE_ENTRY || edit_mode == EditMode.MOVE_ENTRY_WHOLE) {
                    #region MOVE_ENTRY, MOVE_ENTRY_WHOLE
                    if (EditorManager.itemSelection.getEventCount() > 0) {
                        VsqEvent original = EditorManager.itemSelection.getLastEvent().original;
                        int note = EditorManager.noteFromYCoord(e.Y);                           // 現在のマウス位置でのnote
                        int note_init = original.ID.Note;
                        int dnote = (edit_mode == EditMode.MOVE_ENTRY) ? note - note_init : 0;

                        int tclock = EditorManager.clockFromXCoord(e.X - mMouseMoveOffset);
                        int clock_init = original.Clock;

                        int dclock = tclock - clock_init;

                        if (EditorManager.editorConfig.getPositionQuantize() != QuantizeMode.off) {
                            int unit = EditorManager.getPositionQuantizeClock();
                            int new_clock = FormMainModel.Quantize(original.Clock + dclock, unit);
                            dclock = new_clock - clock_init;
                        }

                        EditorManager.mWholeSelectedIntervalStartForMoving = EditorManager.mWholeSelectedInterval.getStart() + dclock;

                        foreach (var item in EditorManager.itemSelection.getEventIterator()) {
                            int new_clock = item.original.Clock + dclock;
                            int new_note = item.original.ID.Note + dnote;
                            item.editing.Clock = new_clock;
                            item.editing.ID.Note = new_note;
                        }
                    }
                    #endregion
                } else if (edit_mode == EditMode.EDIT_LEFT_EDGE) {
                    #region EditLeftEdge
                    int unit = EditorManager.getLengthQuantizeClock();
                    VsqEvent original = EditorManager.itemSelection.getLastEvent().original;
                    int clock_init = original.Clock;
                    int dclock = clock - clock_init;
                    foreach (var item in EditorManager.itemSelection.getEventIterator()) {
                        int end_clock = item.original.Clock + item.original.ID.getLength();
                        int new_clock = item.original.Clock + dclock;
                        int new_length = FormMainModel.Quantize(end_clock - new_clock, unit);
                        if (new_length <= 0) {
                            new_length = unit;
                        }
                        item.editing.Clock = end_clock - new_length;
				if (EditorManager.vibratoLengthEditingRule == VibratoLengthEditingRule.PERCENTAGE) {
                            double percentage = item.original.ID.VibratoDelay / (double)item.original.ID.getLength() * 100.0;
                            int newdelay = (int)(new_length * percentage / 100.0);
                            item.editing.ID.VibratoDelay = newdelay;
                        }
                        item.editing.ID.setLength(new_length);
                    }
                    #endregion
                } else if (edit_mode == EditMode.EDIT_RIGHT_EDGE) {
                    #region EditRightEdge
                    int unit = EditorManager.getLengthQuantizeClock();

                    VsqEvent original = EditorManager.itemSelection.getLastEvent().original;
                    int dlength = clock - (original.Clock + original.ID.getLength());
                    foreach (var item in EditorManager.itemSelection.getEventIterator()) {
                        int new_length = FormMainModel.Quantize(item.original.ID.getLength() + dlength, unit);
                        if (new_length <= 0) {
                            new_length = unit;
                        }
				if (EditorManager.vibratoLengthEditingRule == VibratoLengthEditingRule.PERCENTAGE) {
                            double percentage = item.original.ID.VibratoDelay / (double)item.original.ID.getLength() * 100.0;
                            int newdelay = (int)(new_length * percentage / 100.0);
                            item.editing.ID.VibratoDelay = newdelay;
                        }
                        item.editing.ID.setLength(new_length);
#if DEBUG
                        sout.println("FormMain#pictPianoRoll_MouseMove; length(before,after)=(" + item.original.ID.getLength() + "," + item.editing.ID.getLength() + ")");
#endif
                    }
                    #endregion
                } else if (edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY) {
                    #region AddFixedLengthEntry
                    int note = EditorManager.noteFromYCoord(e.Y);
                    int unit = EditorManager.getPositionQuantizeClock();
                    int new_clock = FormMainModel.Quantize(EditorManager.clockFromXCoord(e.X), unit);
                    EditorManager.mAddingEvent.ID.Note = note;
                    EditorManager.mAddingEvent.Clock = new_clock;
                    #endregion
                } else if (edit_mode == EditMode.EDIT_VIBRATO_DELAY) {
                    #region EditVibratoDelay
                    int new_vibrato_start = clock;
                    int old_vibrato_end = EditorManager.mAddingEvent.Clock + EditorManager.mAddingEvent.ID.getLength();
                    int new_vibrato_length = old_vibrato_end - new_vibrato_start;
                    int max_length = (int)(EditorManager.mAddingEventLength - _PX_ACCENT_HEADER * controller.getScaleXInv());
                    if (max_length < 0) {
                        max_length = 0;
                    }
                    if (new_vibrato_length > max_length) {
                        new_vibrato_start = old_vibrato_end - max_length;
                        new_vibrato_length = max_length;
                    }
                    if (new_vibrato_length < 0) {
                        new_vibrato_start = old_vibrato_end;
                        new_vibrato_length = 0;
                    }
                    EditorManager.mAddingEvent.Clock = new_vibrato_start;
                    EditorManager.mAddingEvent.ID.setLength(new_vibrato_length);
                    if (!timer.Enabled) {
                        refreshScreen();
                    }
                    #endregion
                    return;
                } else if (edit_mode == EditMode.DRAG_DROP) {
                    #region DRAG_DROP
                    // クオンタイズの処理
                    int unit = EditorManager.getPositionQuantizeClock();
                    int clock1 = FormMainModel.Quantize(clock, unit);
                    int note = EditorManager.noteFromYCoord(e.Y);
                    EditorManager.mAddingEvent.Clock = clock1;
                    EditorManager.mAddingEvent.ID.Note = note;
                    #endregion
                }

                // カーソルの形を決める
                if (!mMouseDowned &&
                     edit_mode != EditMode.CURVE_ON_PIANOROLL &&
                     !(EditorManager.mCurveOnPianoroll && (selected_tool == EditTool.PENCIL || selected_tool == EditTool.LINE))) {
                    bool split_cursor = false;
                    bool hand_cursor = false;
                    int min_width = 4 * _EDIT_HANDLE_WIDTH;
                    foreach (var dobj in EditorManager.mDrawObjects[selected - 1]) {
                        Rectangle rc;
                        if (dobj.mType != DrawObjectType.Dynaff) {
                            int edit_handle_width = _EDIT_HANDLE_WIDTH;
                            if (dobj.mRectangleInPixel.Width < min_width) {
                                edit_handle_width = dobj.mRectangleInPixel.Width / 4;
                            }

                            // 音符左側の編集領域
                            rc = new Rectangle(
                                                dobj.mRectangleInPixel.X + EditorManager.keyWidth - stdx,
                                                dobj.mRectangleInPixel.Y - stdy,
                                                edit_handle_width,
                                                (int)(100 * controller.getScaleY()));
                            if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                split_cursor = true;
                                break;
                            }

                            // 音符右側の編集領域
                            rc = new Rectangle(dobj.mRectangleInPixel.X + EditorManager.keyWidth + dobj.mRectangleInPixel.Width - stdx - edit_handle_width,
                                                dobj.mRectangleInPixel.Y - stdy,
                                                edit_handle_width,
                                                (int)(100 * controller.getScaleY()));
                            if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                split_cursor = true;
                                break;
                            }
                        }

                        // 音符本体
                        rc = new Rectangle(dobj.mRectangleInPixel.X + EditorManager.keyWidth - stdx,
                                            dobj.mRectangleInPixel.Y - stdy,
                                            dobj.mRectangleInPixel.Width,
                                            dobj.mRectangleInPixel.Height);
                        if (dobj.mType == DrawObjectType.Note) {
                            if (EditorManager.editorConfig.ShowExpLine && !dobj.mIsOverlapped) {
                                rc.Height *= 2;
                                if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                    // ビブラートの開始位置
                                    rc = new Rectangle(dobj.mRectangleInPixel.X + EditorManager.keyWidth + dobj.mVibratoDelayInPixel - stdx - _EDIT_HANDLE_WIDTH / 2,
                                                        dobj.mRectangleInPixel.Y + (int)(100 * controller.getScaleY()) - stdy,
                                                        _EDIT_HANDLE_WIDTH,
                                                        (int)(100 * controller.getScaleY()));
                                    if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                        split_cursor = true;
                                        break;
                                    } else {
                                        hand_cursor = true;
                                        break;
                                    }
                                }
                            } else {
                                if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                    hand_cursor = true;
                                    break;
                                }
                            }
                        } else {
                            if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                hand_cursor = true;
                                break;
                            }
                        }
                    }

                    if (split_cursor) {
                        Cursor = System.Windows.Forms.Cursors.VSplit;
                    } else if (hand_cursor) {
                        this.Cursor = Cursors.Hand;
                    } else {
                        this.Cursor = Cursors.Default;
                    }
                }
                if (!timer.Enabled) {
                    refreshScreen(true);
                }
            }
        }

        /// <summary>
        /// ピアノロールからマウスボタンが離れたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void pictPianoRoll_MouseUp(Object sender, NMouseEventArgs e)
        {
#if DEBUG
            CDebug.WriteLine("pictureBox1_MouseUp");
            CDebug.WriteLine("    m_config.EditMode=" + EditorManager.EditMode);
#endif
            EditorManager.mIsPointerDowned = false;
            mMouseDowned = false;

            Keys modefiers = (Keys) Control.ModifierKeys;

            EditMode edit_mode = EditorManager.EditMode;
            VsqFileEx vsq = MusicManager.getVsqFile();
            int selected = EditorManager.Selected;
            VsqTrack vsq_track = vsq.Track[selected];
            CurveType selected_curve = trackSelector.getSelectedCurve();
            int stdx = controller.getStartToDrawX();
            int stdy = controller.getStartToDrawY();
            double d2_13 = 8192; // = 2^13
            int track_height = (int)(100 * controller.getScaleY());
            int half_track_height = track_height / 2;

            if (edit_mode == EditMode.CURVE_ON_PIANOROLL) {
                if (pictPianoRoll.mMouseTracer.size() > 1) {
                    // マウスの軌跡の左右端(px)
                    int px_start = pictPianoRoll.mMouseTracer.firstKey();
                    int px_end = pictPianoRoll.mMouseTracer.lastKey();

                    // マウスの軌跡の左右端(クロック)
                    int cl_start = EditorManager.clockFromXCoord(px_start - stdx);
                    int cl_end = EditorManager.clockFromXCoord(px_end - stdx);

                    // 編集が行われたかどうか
                    bool edited = false;
                    // 作業用のPITカーブのコピー
                    VsqBPList pit = (VsqBPList)vsq_track.getCurve("pit").clone();
                    VsqBPList pbs = (VsqBPList)vsq_track.getCurve("pbs"); // こっちはクローンしないよ

                    // トラック内の全音符に対して、マウス軌跡と被っている部分のPITを編集する
                    foreach (var item in vsq_track.getNoteEventIterator()) {
                        int cl_item_start = item.Clock;
                        if (cl_end < cl_item_start) {
                            break;
                        }
                        int cl_item_end = cl_item_start + item.ID.getLength();
                        if (cl_item_end < cl_start) {
                            continue;
                        }

                        // ここに到達するってことは、pitに編集が加えられるってこと。
                        edited = true;

                        // マウス軌跡と被っている部分のPITを削除
                        int cl_remove_start = Math.Max(cl_item_start, cl_start);
                        int cl_remove_end = Math.Min(cl_item_end, cl_end);
                        int value_at_remove_end = pit.getValue(cl_remove_end);
                        int value_at_remove_start = pit.getValue(cl_remove_start);
                        List<int> remove = new List<int>();
                        foreach (var clock in pit.keyClockIterator()) {
                            if (cl_remove_start <= clock && clock <= cl_remove_end) {
                                remove.Add(clock);
                            }
                        }
                        foreach (var clock in remove) {
                            pit.remove(clock);
                        }
                        remove = null;

                        int px_item_start = EditorManager.xCoordFromClocks(cl_item_start) + stdx;
                        int px_item_end = EditorManager.xCoordFromClocks(cl_item_end) + stdx;

                        int lastv = value_at_remove_start;
                        bool cl_item_end_added = false;
                        bool cl_item_start_added = false;
                        int last_px = 0, last_py = 0;
                        foreach (var p in pictPianoRoll.mMouseTracer.iterator()) {
                            if (p.X < px_item_start) {
                                last_px = p.X;
                                last_py = p.Y;
                                continue;
                            }
                            if (px_item_end < p.X) {
                                break;
                            }

                            int clock = EditorManager.clockFromXCoord(p.X - stdx);
                            if (clock < cl_item_start) {
                                last_px = p.X;
                                last_py = p.Y;
                                continue;
                            } else if (cl_item_end < clock) {
                                break;
                            }
                            double note = EditorManager.noteFromYCoordDoublePrecision(p.Y - stdy - half_track_height);
                            int v_pit = (int)(d2_13 / (double)pbs.getValue(clock) * (note - item.ID.Note));

                            // 正規化
                            if (v_pit < pit.getMinimum()) {
                                v_pit = pit.getMinimum();
                            } else if (pit.getMaximum() < v_pit) {
                                v_pit = pit.getMaximum();
                            }

                            if (cl_item_start < clock && !cl_item_start_added &&
                                 cl_start <= cl_item_start && cl_item_start < cl_end) {
                                // これから追加しようとしているデータ点の時刻が、音符の開始時刻よりも後なんだけれど、
                                // 音符の開始時刻におけるデータをまだ書き込んでない場合
                                double a = (p.Y - last_py) / (double)(p.X - last_px);
                                double x_at_clock = EditorManager.xCoordFromClocks(cl_item_start) + stdx;
                                double ext_y = last_py + a * (x_at_clock - last_px);
                                double tnote = EditorManager.noteFromYCoordDoublePrecision((int)(ext_y - stdy - half_track_height));
                                int t_vpit = (int)(d2_13 / (double)pbs.getValue(cl_item_start) * (tnote - item.ID.Note));
                                pit.add(cl_item_start, t_vpit);
                                lastv = t_vpit;
                                cl_item_start_added = true;
                            }

                            // 直前の値と違っている場合にのみ追加
                            if (v_pit != lastv) {
                                pit.add(clock, v_pit);
                                lastv = v_pit;
                                if (clock == cl_item_end) {
                                    cl_item_end_added = true;
                                } else if (clock == cl_item_start) {
                                    cl_item_start_added = true;
                                }
                            }
                        }

                        if (!cl_item_end_added &&
                             cl_start <= cl_item_end && cl_item_end <= cl_end) {
                            pit.add(cl_item_end, lastv);
                        }

                        pit.add(cl_remove_end, value_at_remove_end);
                    }

                    // 編集操作が行われた場合のみ、コマンドを発行
                    if (edited) {
                        CadenciiCommand run = new CadenciiCommand(
                            VsqCommand.generateCommandTrackCurveReplace(selected, "PIT", pit));
                        EditorManager.editHistory.register(vsq.executeCommand(run));
                        setEdited(true);
                    }
                }
                pictPianoRoll.mMouseTracer.clear();
                EditorManager.EditMode = EditMode.NONE;
                return;
            }

            if (edit_mode == EditMode.MIDDLE_DRAG) {
                this.Cursor = Cursors.Default;
            } else if (edit_mode == EditMode.ADD_ENTRY || edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY) {
                #region AddEntry || AddFixedLengthEntry
                if (EditorManager.Selected >= 0) {
                    if ((edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY) ||
                         (edit_mode == EditMode.ADD_ENTRY && (mButtonInitial.X != e.X || mButtonInitial.Y != e.Y) && EditorManager.mAddingEvent.ID.getLength() > 0)) {
                        if (EditorManager.mAddingEvent.Clock < vsq.getPreMeasureClocks()) {
                            SystemSounds.Asterisk.Play();
                        } else {
                            fixAddingEvent();
                        }
                    }
                }
                #endregion
            } else if (edit_mode == EditMode.MOVE_ENTRY) {
                #region MoveEntry
#if DEBUG
                sout.println("FormMain#pictPianoRoll_MouseUp; edit_mode is MOVE_ENTRY");
#endif
                if (EditorManager.itemSelection.getEventCount() > 0) {
                    SelectedEventEntry last_selected_event = EditorManager.itemSelection.getLastEvent();
#if DEBUG
                    sout.println("FormMain#pictPianoRoll_MouseUp; last_selected_event.original.InternalID=" + last_selected_event.original.InternalID);
#endif
                    VsqEvent original = last_selected_event.original;
                    if (original.Clock != last_selected_event.editing.Clock ||
                         original.ID.Note != last_selected_event.editing.ID.Note) {
                        bool out_of_range = false; // プリメジャーにめり込んでないかどうか
                        bool contains_dynamics = false; // Dynaff, Crescend, Desrecendが含まれているかどうか
                        VsqTrack copied = (VsqTrack)vsq_track.clone();
                        int clockAtPremeasure = vsq.getPreMeasureClocks();
                        foreach (var ev in EditorManager.itemSelection.getEventIterator()) {
                            int internal_id = ev.original.InternalID;
                            if (ev.editing.Clock < clockAtPremeasure) {
                                out_of_range = true;
                                break;
                            }
                            if (ev.editing.ID.Note < 0 || 128 < ev.editing.ID.Note) {
                                out_of_range = true;
                                break;
                            }
                            for (Iterator<VsqEvent> itr2 = copied.getEventIterator(); itr2.hasNext(); ) {
                                VsqEvent item = itr2.next();
                                if (item.InternalID == internal_id) {
                                    item.Clock = ev.editing.Clock;
                                    item.ID = (VsqID)ev.editing.ID.clone();
                                    break;
                                }
                            }
                            if (ev.original.ID.type == VsqIDType.Aicon) {
                                contains_dynamics = true;
                            }
                        }
                        if (out_of_range) {
                            SystemSounds.Asterisk.Play();
                        } else {
                            if (contains_dynamics) {
                                copied.reflectDynamics();
                            }
                            CadenciiCommand run = VsqFileEx.generateCommandTrackReplace(selected,
                                                                                         copied,
                                                                                         vsq.AttachedCurves.get(selected - 1));
                            EditorManager.editHistory.register(vsq.executeCommand(run));
                            EditorManager.itemSelection.updateSelectedEventInstance();
                            setEdited(true);
                        }
                    } else {
                        /*if ( (modefier & Keys.Shift) == Keys.Shift || (modefier & Keys.Control) == Keys.Control ) {
                            Rectangle rc;
                            VsqEvent select = IdOfClickedPosition( e.Location, out rc );
                            if ( select != null ) {
                                m_config.addSelectedEvent( item.InternalID );
                            }
                        }*/
                    }
                    lock (EditorManager.mDrawObjects) {
                        EditorManager.mDrawObjects[selected - 1].Sort();
                    }
                }
                #endregion
            } else if (edit_mode == EditMode.EDIT_LEFT_EDGE || edit_mode == EditMode.EDIT_RIGHT_EDGE) {
                #region EDIT_LEFT_EDGE | EDIT_RIGHT_EDGE
                if (mMouseMoved) {
                    VsqEvent original = EditorManager.itemSelection.getLastEvent().original;
                    int count = EditorManager.itemSelection.getEventCount();
                    int[] ids = new int[count];
                    int[] clocks = new int[count];
                    VsqID[] values = new VsqID[count];
                    int i = -1;
                    bool contains_aicon = false; // dynaff, crescend, decrescendが含まれていればtrue
                    foreach (var ev in EditorManager.itemSelection.getEventIterator()) {
                        if (ev.original.ID.type == VsqIDType.Aicon) {
                            contains_aicon = true;
                        }
                        i++;

				EditorManager.editLengthOfVsqEvent(ev.editing, ev.editing.ID.getLength(), EditorManager.vibratoLengthEditingRule);
                        ids[i] = ev.original.InternalID;
                        clocks[i] = ev.editing.Clock;
                        values[i] = ev.editing.ID;
                    }

                    CadenciiCommand run = null;
                    if (contains_aicon) {
                        VsqFileEx copied_vsq = (VsqFileEx)vsq.clone();
                        VsqCommand vsq_command =
                            VsqCommand.generateCommandEventChangeClockAndIDContaintsRange(selected,
                                                                                           ids,
                                                                                           clocks,
                                                                                           values);
                        copied_vsq.executeCommand(vsq_command);
                        VsqTrack copied = (VsqTrack)copied_vsq.Track[selected].clone();
                        copied.reflectDynamics();
                        run = VsqFileEx.generateCommandTrackReplace(selected,
                                                                     copied,
                                                                     vsq.AttachedCurves.get(selected - 1));
                    } else {
                        run = new CadenciiCommand(
                            VsqCommand.generateCommandEventChangeClockAndIDContaintsRange(selected,
                                                                                 ids,
                                                                                 clocks,
                                                                                 values));
                    }
                    EditorManager.editHistory.register(vsq.executeCommand(run));
                    setEdited(true);
                }
                #endregion
            } else if (edit_mode == EditMode.EDIT_VIBRATO_DELAY) {
                #region EditVibratoDelay
                if (mMouseMoved) {
                    double max_length = EditorManager.mAddingEventLength - _PX_ACCENT_HEADER * controller.getScaleXInv();
                    double rate = EditorManager.mAddingEvent.ID.getLength() / max_length;
                    if (rate > 0.99) {
                        rate = 1.0;
                    }
                    int vibrato_length = (int)(EditorManager.mAddingEventLength * rate);
                    VsqEvent item = null;
                    foreach (var ve in vsq_track.getNoteEventIterator()) {
                        if (ve.InternalID == mVibratoEditingId) {
                            item = (VsqEvent)ve.clone();
                            break;
                        }
                    }
                    if (item != null) {
                        if (vibrato_length <= 0) {
                            item.ID.VibratoHandle = null;
                            item.ID.VibratoDelay = item.ID.getLength();
                        } else {
                            item.ID.VibratoHandle.setLength(vibrato_length);
                            item.ID.VibratoDelay = item.ID.getLength() - vibrato_length;
                        }
                        CadenciiCommand run = new CadenciiCommand(
                            VsqCommand.generateCommandEventChangeIDContaints(selected, mVibratoEditingId, item.ID));
                        EditorManager.editHistory.register(vsq.executeCommand(run));
                        setEdited(true);
                    }
                }
                #endregion
            } else if (edit_mode == EditMode.MOVE_ENTRY_WHOLE) {
#if DEBUG
                sout.println("FormMain#pictPianoRoll_MouseUp; EditMode.MOVE_ENTRY_WHOLE");
#endif
                #region MOVE_ENTRY_WHOLE
                int src_clock_start = EditorManager.mWholeSelectedInterval.getStart();
                int src_clock_end = EditorManager.mWholeSelectedInterval.getEnd();
                int dst_clock_start = EditorManager.mWholeSelectedIntervalStartForMoving;
                int dst_clock_end = dst_clock_start + (src_clock_end - src_clock_start);
                int dclock = dst_clock_start - src_clock_start;

                int num = EditorManager.itemSelection.getEventCount();
                int[] selected_ids = new int[num]; // 後段での再選択用のInternalIDのリスト
                int last_selected_id = EditorManager.itemSelection.getLastEvent().original.InternalID;

                // 音符イベントを移動
                VsqTrack work = (VsqTrack)vsq_track.clone();
                int k = 0;
                foreach (var item in EditorManager.itemSelection.getEventIterator()) {
                    int internal_id = item.original.InternalID;
                    selected_ids[k] = internal_id;
                    k++;
#if DEBUG
                    sout.println("FormMain#pictPianoRoll_MouseUp; internal_id=" + internal_id);
#endif
                    foreach (var vsq_event in work.getNoteEventIterator()) {
                        if (internal_id == vsq_event.InternalID) {
#if DEBUG
                            sout.println("FormMain#pictPianoRoll_MouseUp; before: clock=" + vsq_event.Clock + "; after: clock=" + item.editing.Clock);
#endif
                            vsq_event.Clock = item.editing.Clock;
                            break;
                        }
                    }
                }

                // 全てのコントロールカーブのデータ点を移動
                for (int i = 0; i < BezierCurves.CURVE_USAGE.Length; i++) {
                    CurveType curve_type = BezierCurves.CURVE_USAGE[i];
                    VsqBPList bplist = work.getCurve(curve_type.getName());
                    if (bplist == null) {
                        continue;
                    }

                    // src_clock_startからsrc_clock_endの範囲にあるデータ点をコピー＆削除
                    VsqBPList copied = new VsqBPList(bplist.getName(), bplist.getDefault(), bplist.getMinimum(), bplist.getMaximum());
                    int size = bplist.size();
                    for (int j = size - 1; j >= 0; j--) {
                        int clock = bplist.getKeyClock(j);
                        if (src_clock_start <= clock && clock <= src_clock_end) {
                            VsqBPPair bppair = bplist.getElementB(j);
                            copied.add(clock, bppair.value);
                            bplist.removeElementAt(j);
                        }
                    }

                    // dst_clock_startからdst_clock_endの範囲にあるコントロールカーブのデータ点をすべて削除
                    size = bplist.size();
                    for (int j = size - 1; j >= 0; j--) {
                        int clock = bplist.getKeyClock(j);
                        if (dst_clock_start <= clock && clock <= dst_clock_end) {
                            bplist.removeElementAt(j);
                        }
                    }

                    // コピーしたデータを、クロックをずらしながら追加
                    size = copied.size();
                    for (int j = 0; j < size; j++) {
                        int clock = copied.getKeyClock(j);
                        VsqBPPair bppair = copied.getElementB(j);
                        bplist.add(clock + dclock, bppair.value);
                    }
                }

                // コマンドを作成＆実行
                CadenciiCommand run = VsqFileEx.generateCommandTrackReplace(selected,
                                                                             work,
                                                                             vsq.AttachedCurves.get(selected - 1));
                EditorManager.editHistory.register(vsq.executeCommand(run));

                // 選択範囲を更新
                EditorManager.mWholeSelectedInterval = new SelectedRegion(dst_clock_start);
                EditorManager.mWholeSelectedInterval.setEnd(dst_clock_end);
                EditorManager.mWholeSelectedIntervalStartForMoving = dst_clock_start;

                // 音符の再選択
                EditorManager.itemSelection.clearEvent();
                List<int> list_selected_ids = new List<int>();
                for (int i = 0; i < num; i++) {
                    list_selected_ids.Add(selected_ids[i]);
                }
                EditorManager.itemSelection.addEventAll(list_selected_ids);
                EditorManager.itemSelection.addEvent(last_selected_id);

                setEdited(true);
                #endregion
            } else if (EditorManager.IsWholeSelectedIntervalEnabled) {
                int start = EditorManager.mWholeSelectedInterval.getStart();
                int end = EditorManager.mWholeSelectedInterval.getEnd();
#if DEBUG
                sout.println("FormMain#pictPianoRoll_MouseUp; WholeSelectedInterval; (start,end)=" + start + ", " + end);
#endif
                EditorManager.itemSelection.clearEvent();

                // 音符の選択状態を更新
                List<int> add_required_event = new List<int>();
                for (Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
                    VsqEvent ve = itr.next();
                    if (start <= ve.Clock && ve.Clock + ve.ID.getLength() <= end) {
                        add_required_event.Add(ve.InternalID);
                    }
                }
                EditorManager.itemSelection.addEventAll(add_required_event);

                // コントロールカーブ点の選択状態を更新
                List<long> add_required_point = new List<long>();
                VsqBPList list = vsq_track.getCurve(selected_curve.getName());
                if (list != null) {
                    int count = list.size();
                    for (int i = 0; i < count; i++) {
                        int clock = list.getKeyClock(i);
                        if (clock < start) {
                            continue;
                        } else if (end < clock) {
                            break;
                        } else {
                            VsqBPPair v = list.getElementB(i);
                            add_required_point.Add(v.id);
                        }
                    }
                }
                if (add_required_point.Count > 0) {
                    EditorManager.itemSelection.addPointAll(selected_curve,
                                                    PortUtil.convertLongArray(add_required_point.ToArray()));
                }
            }
        heaven:
            EditorManager.EditMode = EditMode.NONE;
            refreshScreen(true);
        }

        public void pictPianoRoll_MouseWheel(Object sender, NMouseEventArgs e)
        {
            Keys modifier = (Keys) Control.ModifierKeys;
            bool horizontal = (modifier & Keys.Shift) == Keys.Shift;
            if (EditorManager.editorConfig.ScrollHorizontalOnWheel) {
                horizontal = !horizontal;
            }
            if ((modifier & Keys.Control) == Keys.Control) {
                // ピアノロール拡大率を変更
                if (horizontal) {
                    int max = trackBar.Maximum;
                    int min = trackBar.Minimum;
                    int width = max - min;
                    int delta = (width / 10) * (e.Delta > 0 ? 1 : -1);
                    int old_tbv = trackBar.Value;
                    int draft = old_tbv + delta;
                    if (draft < min) {
                        draft = min;
                    }
                    if (max < draft) {
                        draft = max;
                    }
                    if (old_tbv != draft) {

                        // マウス位置を中心に拡大されるようにしたいので．
                        // マウスのスクリーン座標
						Point screen_p_at_mouse = cadencii.core2.PortUtil.getMousePosition();
                        // ピアノロール上でのマウスのx座標
                        int x_at_mouse = pictPianoRoll.PointToClient(new Cadencii.Gui.Point(screen_p_at_mouse.X, screen_p_at_mouse.Y)).X;
                        // マウス位置でのクロック -> こいつが保存される
                        int clock_at_mouse = EditorManager.clockFromXCoord(x_at_mouse);
                        // 古い拡大率
                        float scale0 = controller.getScaleX();
                        // 新しい拡大率
                        float scale1 = getScaleXFromTrackBarValue(draft);
                        // 古いstdx
                        int stdx0 = controller.getStartToDrawX();
                        int stdx1 = (int)(clock_at_mouse * (scale1 - scale0) + stdx0);
                        // 新しいhScroll.Value
                        int hscroll_value = (int)(stdx1 / scale1);
                        if (hscroll_value < hScroll.Minimum) {
                            hscroll_value = hScroll.Minimum;
                        }
                        if (hScroll.Maximum < hscroll_value) {
                            hscroll_value = hScroll.Maximum;
                        }

                        controller.setScaleX(scale1);
                        controller.setStartToDrawX(stdx1);
                        hScroll.Value = hscroll_value;
                        trackBar.Value = draft;
                    }
                } else {
                    zoomY(e.Delta > 0 ? 1 : -1);
                }
            } else {
                // スクロール操作
                if (e.X <= EditorManager.keyWidth || pictPianoRoll.Width < e.X) {
                    horizontal = false;
                }
                if (horizontal) {
                    hScroll.Value = computeScrollValueFromWheelDelta(e.Delta);
                } else {
                    double new_val = (double)vScroll.Value - e.Delta * 10;
                    int min = vScroll.Minimum;
                    int max = vScroll.Maximum - vScroll.LargeChange;
                    if (new_val > max) {
                        vScroll.Value = max;
                    } else if (new_val < min) {
                        vScroll.Value = min;
                    } else {
                        vScroll.Value = (int)new_val;
                    }
                }
            }
            refreshScreen();
        }

	public void pictPianoRoll_PreviewKeyDown(Object sender, PreviewKeyDownEventArgs e)
        {
            var e0 = new NKeyEventArgs((Keys) e.KeyData);
            processSpecialShortcutKey(e0, true);
        }
        public void pictPianoRoll_PreviewKeyDown2(Object sender, NKeyEventArgs e)
        {
            var e0 = new NKeyEventArgs(e.KeyData);
            processSpecialShortcutKey(e0, true);
        }
        #endregion

        //BOOKMARK: iconPalette
        #region iconPalette
        public void iconPalette_LocationChanged(Object sender, EventArgs e)
        {
            var point = EditorManager.iconPalette.Location;
            EditorManager.editorConfig.FormIconPaletteLocation = new XmlPoint(point.X, point.Y);
        }

        public void iconPalette_FormClosing(Object sender, EventArgs e)
        {
            model.FlipIconPaletteVisible(EditorManager.iconPalette.Visible);
        }
        #endregion


        //BOOKMARK: mixerWindow
        #region mixerWindow
        public void mixerWindow_FormClosing(Object sender, EventArgs e)
        {
            model.FlipMixerDialogVisible(EditorManager.MixerWindow.Visible);
        }

        public void mixerWindow_SoloChanged(int track, bool solo)
        {
#if DEBUG
            CDebug.WriteLine("FormMain#mixerWindow_SoloChanged");
            CDebug.WriteLine("    track=" + track);
            CDebug.WriteLine("    solo=" + solo);
#endif
            VsqFileEx vsq = MusicManager.getVsqFile();
            if (vsq == null) {
                return;
            }
            vsq.setSolo(track, solo);
            if (EditorManager.MixerWindow != null) {
                EditorManager.MixerWindow.updateStatus();
            }
        }

        public void mixerWindow_MuteChanged(int track, bool mute)
        {
#if DEBUG
            CDebug.WriteLine("FormMain#mixerWindow_MuteChanged");
            CDebug.WriteLine("    track=" + track);
            CDebug.WriteLine("    mute=" + mute);
#endif
            VsqFileEx vsq = MusicManager.getVsqFile();
            if (vsq == null) {
                return;
            }
            if (track < 0) {
                MusicManager.getBgm(-track - 1).mute = mute ? 1 : 0;
            } else {
                vsq.setMute(track, mute);
            }
            if (EditorManager.MixerWindow != null) {
                EditorManager.MixerWindow.updateStatus();
            }
        }

        public void mixerWindow_PanpotChanged(int track, int panpot)
        {
            if (track == 0) {
                // master
                MusicManager.getVsqFile().Mixer.MasterPanpot = panpot;
            } else if (track > 0) {
                // slave
                MusicManager.getVsqFile().Mixer.Slave[track - 1].Panpot = panpot;
            } else {
                MusicManager.getBgm(-track - 1).panpot = panpot;
            }
        }

        public void mixerWindow_FederChanged(int track, int feder)
        {
#if DEBUG
            sout.println("FormMain#mixerWindow_FederChanged; track=" + track + "; feder=" + feder);
#endif
            if (track == 0) {
                MusicManager.getVsqFile().Mixer.MasterFeder = feder;
            } else if (track > 0) {
                MusicManager.getVsqFile().Mixer.Slave[track - 1].Feder = feder;
            } else {
                MusicManager.getBgm(-track - 1).feder = feder;
            }
        }
        #endregion

        #region mPropertyPanelContainer
#if ENABLE_PROPERTY
        public void mPropertyPanelContainer_StateChangeRequired(Object sender, PanelState arg)
        {
            updatePropertyPanelState(arg);
        }
#endif
        #endregion

        #region propertyPanel
#if ENABLE_PROPERTY
        public void propertyPanel_CommandExecuteRequired(Object sender, CadenciiCommand command)
        {
#if DEBUG
            CDebug.WriteLine("m_note_property_dlg_CommandExecuteRequired");
#endif
            EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(command));
            updateDrawObjectList();
            refreshScreen();
            setEdited(true);
        }
#endif
        #endregion

        //BOOKMARK: propertyWindow
        #region PropertyWindowListenerの実装

#if ENABLE_PROPERTY
        public void propertyWindowFormClosing()
        {
#if DEBUG
            sout.println("FormMain#propertyWindowFormClosing");
#endif
            updatePropertyPanelState(PanelState.Hidden);
        }
#endif

#if ENABLE_PROPERTY
        public void propertyWindowStateChanged()
        {
#if DEBUG
            sout.println("FormMain#propertyWindow_WindowStateChanged");
#endif
            if (EditorManager.editorConfig.PropertyWindowStatus.State == PanelState.Window) {
#if DEBUG
                sout.println("FormMain#proprtyWindow_WindowStateChanged; isWindowMinimized=" + EditorManager.propertyWindow.getUi().isWindowMinimized());
#endif
                if (EditorManager.propertyWindow.getUi().isWindowMinimized()) {
                    updatePropertyPanelState(PanelState.Docked);
                }
            }
        }

        public void propertyWindowLocationOrSizeChanged()
        {
#if DEBUG
            sout.println("FormMain#propertyWindow_LocationOrSizeChanged");
#endif
            if (EditorManager.editorConfig.PropertyWindowStatus.State == PanelState.Window) {
                if (EditorManager.propertyWindow != null && false == EditorManager.propertyWindow.getUi().isWindowMinimized()) {
                    var parent = this.Location;
                    int propertyX = EditorManager.propertyWindow.getUi().getX();
                    int propertyY = EditorManager.propertyWindow.getUi().getY();
                    EditorManager.editorConfig.PropertyWindowStatus.Bounds =
                        new XmlRectangle(propertyX - parent.X,
                                          propertyY - parent.Y,
                                          EditorManager.propertyWindow.getUi().getWidth(),
                                          EditorManager.propertyWindow.getUi().getHeight());
                }
            }
        }
#endif
        #endregion

        //BOOKMARK: FormMain
        #region FormMain
        public void handleDragExit()
        {
            EditorManager.EditMode = EditMode.NONE;
            mIconPaletteOnceDragEntered = false;
        }

        private void FormMain_DragLeave(Object sender, EventArgs e)
        {
            handleDragExit();
        }

        /// <summary>
        /// アイテムがドラッグされている最中の処理を行います
        /// </summary>
        public void handleDragOver(int screen_x, int screen_y)
        {
            if (EditorManager.EditMode != EditMode.DRAG_DROP) {
                return;
            }
            var pt = pictPianoRoll.PointToScreen(Cadencii.Gui.Point.Empty);
            if (!mIconPaletteOnceDragEntered) {
                int keywidth = EditorManager.keyWidth;
                Rectangle rc = new Rectangle(pt.X + keywidth, pt.Y, pictPianoRoll.Width - keywidth, pictPianoRoll.Height);
                if (Utility.isInRect(new Point(screen_x, screen_y), rc)) {
                    mIconPaletteOnceDragEntered = true;
                } else {
                    return;
                }
            }
            var e0 = new NMouseEventArgs(NMouseButtons.Left,
                                                    1,
                                                    screen_x - pt.X,
                                                    screen_y - pt.Y,
                                                    0);
            pictPianoRoll_MouseMove(this, e0);
        }

        private void FormMain_DragOver(Object sender, System.Windows.Forms.DragEventArgs e)
        {
            handleDragOver(e.X, e.Y);
        }

        /// <summary>
        /// ピアノロールにドロップされたIconDynamicsHandleの受け入れ処理を行います
        /// </summary>
        public void handleDragDrop(IconDynamicsHandle handle, int screen_x, int screen_y)
        {
            if (handle == null) {
                return;
            }
			var locPianoroll = pictPianoRoll.PointToScreen(Cadencii.Gui.Point.Empty);
            // ドロップ位置を特定して，アイテムを追加する
            int x = screen_x - locPianoroll.X;
            int y = screen_y - locPianoroll.Y;
            int clock1 = EditorManager.clockFromXCoord(x);

            // クオンタイズの処理
            int unit = EditorManager.getPositionQuantizeClock();
            int clock = FormMainModel.Quantize(clock1, unit);

            int note = EditorManager.noteFromYCoord(y);
            VsqFileEx vsq = MusicManager.getVsqFile();
            int clockAtPremeasure = vsq.getPreMeasureClocks();
            if (clock < clockAtPremeasure) {
                return;
            }
            if (note < 0 || 128 < note) {
                return;
            }

            int selected = EditorManager.Selected;
            VsqTrack vsq_track = vsq.Track[selected];
            VsqTrack work = (VsqTrack)vsq_track.clone();

            if (EditorManager.mAddingEvent == null) {
                // ここは多分起こらない
                return;
            }
            VsqEvent item = (VsqEvent)EditorManager.mAddingEvent.clone();
            item.Clock = clock;
            item.ID.Note = note;
            work.addEvent(item);
            work.reflectDynamics();
            CadenciiCommand run = VsqFileEx.generateCommandTrackReplace(selected, work, vsq.AttachedCurves.get(selected - 1));
            EditorManager.editHistory.register(vsq.executeCommand(run));
            setEdited(true);
            EditorManager.EditMode = EditMode.NONE;
            refreshScreen();
        }

        private void FormMain_DragDrop(Object sender, System.Windows.Forms.DragEventArgs e)
        {
            EditorManager.EditMode = EditMode.NONE;
            mIconPaletteOnceDragEntered = false;
            mMouseDowned = false;
            if (!e.Data.GetDataPresent(typeof(IconDynamicsHandle))) {
                return;
            }
			var locPianoroll = pictPianoRoll.PointToScreen(Cadencii.Gui.Point.Empty);
            int keywidth = EditorManager.keyWidth;
            Rectangle rcPianoroll = new Rectangle(locPianoroll.X + keywidth,
                                                   locPianoroll.Y,
                                                   pictPianoRoll.Width - keywidth,
                                                   pictPianoRoll.Height);
            if (!Utility.isInRect(new Point(e.X, e.Y), rcPianoroll)) {
                return;
            }

            // dynaff, crescend, decrescend のどれがドロップされたのか検査
            IconDynamicsHandle this_is_it = (IconDynamicsHandle)e.Data.GetData(typeof(IconDynamicsHandle));
            if (this_is_it == null) {
                return;
            }

            handleDragDrop(this_is_it, e.X, e.Y);
        }

        /// <summary>
        /// ドラッグの開始処理を行います
        /// </summary>
        public void handleDragEnter()
        {
            EditorManager.EditMode = EditMode.DRAG_DROP;
            mMouseDowned = true;
        }

        private void FormMain_DragEnter(Object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(IconDynamicsHandle))) {
                // ドロップ可能
                e.Effect = System.Windows.Forms.DragDropEffects.All;
                handleDragEnter();
            } else {
                e.Effect = System.Windows.Forms.DragDropEffects.None;
                EditorManager.EditMode = EditMode.NONE;
            }
        }
        public void FormMain_FormClosed(Object sender, FormClosedEventArgs e)
        {
#if DEBUG
            sout.println("FormMain#FormMain_FormClosed");
#endif
            model.ClearTempWave();
            string tempdir = Path.Combine(ApplicationGlobal.getCadenciiTempDir(), ApplicationGlobal.getID());
            if (!Directory.Exists(tempdir)) {
                PortUtil.createDirectory(tempdir);
            }
            string log = Path.Combine(tempdir, "run.log");
            cadencii.core2.debug.close();
            try {
                if (System.IO.File.Exists(log)) {
                    PortUtil.deleteFile(log);
                }
                PortUtil.deleteDirectory(tempdir, true);
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".FormMain_FormClosed; ex=" + ex + "\n");
                serr.println("FormMain#FormMain_FormClosed; ex=" + ex);
            }
            EditorManager.stopGenerator();
            VSTiDllManager.terminate();
#if ENABLE_MIDI
            //MidiPlayer.stop();
            if (mMidiIn != null) {
                mMidiIn.close();
            }
#endif
#if ENABLE_MTC
            if ( m_midi_in_mtc != null ) {
                m_midi_in_mtc.Close();
            }
#endif
            PlaySound.kill();
            PluginLoader.cleanupUnusedAssemblyCache();
        }

        public void FormMain_FormClosing(Object sender, FormClosingEventArgs e)
        {
            // 設定値を格納
			if (ApplicationGlobal.appConfig.ViewWaveform) {
				EditorManager.editorConfig.SplitContainer2LastDividerLocation = splitContainer2.DividerLocation;
            }
            if (EditorManager.editorConfig.PropertyWindowStatus.State == PanelState.Docked) {
                EditorManager.editorConfig.PropertyWindowStatus.DockWidth = splitContainerProperty.DividerLocation;
            }
            if (e.CloseReason == System.Windows.Forms.CloseReason.WindowsShutDown) {
                return;
            }
            bool cancel = handleFormClosing();
            e.Cancel = cancel;
        }

        /// <summary>
        /// ウィンドウが閉じようとしているときの処理を行う
        /// 戻り値がtrueの場合，ウィンドウが閉じるのをキャンセルする処理が必要
        /// </summary>
        /// <returns></returns>
        public bool handleFormClosing()
        {
            if (isEdited()) {
                string file = MusicManager.getFileName();
                if (file.Equals("")) {
                    file = "Untitled";
                } else {
                    file = PortUtil.getFileName(file);
                }
                var ret = DialogManager.showMessageBox(_("Save this sequence?"),
                                                               _("Affirmation"),
                                                               cadencii.Dialog.MSGBOX_YES_NO_CANCEL_OPTION,
                                                               cadencii.Dialog.MSGBOX_QUESTION_MESSAGE);
				if (ret == Cadencii.Gui.DialogResult.Yes) {
                    if (MusicManager.getFileName().Equals("")) {
                        var dr = DialogManager.showModalFileDialog(saveXmlVsqDialog, false, this);
						if (dr == Cadencii.Gui.DialogResult.OK) {
                            EditorManager.saveTo(saveXmlVsqDialog.FileName);
                        } else {
                            return true;
                        }
                    } else {
                        EditorManager.saveTo(MusicManager.getFileName());
                    }

				} else if (ret == Cadencii.Gui.DialogResult.Cancel) {
                    return true;
                }
            }
            EditorManager.editorConfig.WindowMaximized = (this.WindowState == FormWindowState.Maximized);
            EditorManager.saveConfig();
            UtauWaveGenerator.clearCache();
            VConnectWaveGenerator.clearCache();

#if ENABLE_MIDI
            if (mMidiIn != null) {
                mMidiIn.close();
            }
#endif
            bgWorkScreen.Dispose();
            return false;
        }

        public void FormMain_LocationChanged(Object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal) {
                var bounds = this.Bounds;
                EditorManager.editorConfig.WindowRect = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            }
        }

        public void FormMain_Load(Object sender, EventArgs e)
        {
            applyLanguage();

            // ツールバーの位置を復帰させる
            // toolStipの位置を，前回終了時の位置に戻す
            int chevron_width = EditorManager.editorConfig.ChevronWidth;
            this.bandFile = ApplicationUIHost.Create<RebarBand>();
            this.bandPosition = ApplicationUIHost.Create<RebarBand>();
            this.bandMeasure = ApplicationUIHost.Create<RebarBand>();
            this.bandTool = ApplicationUIHost.Create<RebarBand>();

            bool variant_height = false;
            this.bandFile.VariantHeight = variant_height;
            this.bandPosition.VariantHeight = variant_height;
            this.bandMeasure.VariantHeight = variant_height;
            this.bandTool.VariantHeight = variant_height;

            int MAX_BAND_HEIGHT = 26;// toolBarTool.Height;

            this.rebar.AddControl(this.toolBarFile);
            this.rebar.AddControl(this.toolBarTool);
            this.rebar.AddControl(this.toolBarPosition);
            this.rebar.AddControl(this.toolBarMeasure);
            // bandFile
            this.bandFile.AllowVertical = false;
            this.bandFile.Child = this.toolBarFile;
            this.bandFile.Header = -1;
            this.bandFile.Integral = 1;
            this.bandFile.MaxHeight = MAX_BAND_HEIGHT;
            this.bandFile.UseChevron = true;
            if (toolBarFile.Buttons.Count > 0) {
                this.bandFile.IdealWidth =
                    toolBarFile.Buttons[toolBarFile.Buttons.Count - 1].Rectangle.Right + chevron_width;
            }
            this.bandFile.BandSize = EditorManager.editorConfig.BandSizeFile;
            this.bandFile.NewRow = EditorManager.editorConfig.BandNewRowFile;
            // bandPosition
            this.bandPosition.AllowVertical = false;
            this.bandPosition.Child = this.toolBarPosition;
            this.bandPosition.Header = -1;
            this.bandPosition.Integral = 1;
            this.bandPosition.MaxHeight = MAX_BAND_HEIGHT;
            this.bandPosition.UseChevron = true;
            if (toolBarPosition.Buttons.Count > 0) {
                this.bandPosition.IdealWidth =
                    toolBarPosition.Buttons[toolBarPosition.Buttons.Count - 1].Rectangle.Right + chevron_width;
            }
            this.bandPosition.BandSize = EditorManager.editorConfig.BandSizePosition;
            this.bandPosition.NewRow = EditorManager.editorConfig.BandNewRowPosition;
            // bandMeasure
            this.bandMeasure.AllowVertical = false;
            this.bandMeasure.Child = this.toolBarMeasure;
            this.bandMeasure.Header = -1;
            this.bandMeasure.Integral = 1;
            this.bandMeasure.MaxHeight = MAX_BAND_HEIGHT;
            this.bandMeasure.UseChevron = true;
            if (toolBarMeasure.Buttons.Count > 0) {
                this.bandMeasure.IdealWidth =
                    toolBarMeasure.Buttons[toolBarMeasure.Buttons.Count - 1].Rectangle.Right + chevron_width;
            }
            this.bandMeasure.BandSize = EditorManager.editorConfig.BandSizeMeasure;
            this.bandMeasure.NewRow = EditorManager.editorConfig.BandNewRowMeasure;
            // bandTool
            this.bandTool.AllowVertical = false;
            this.bandTool.Child = this.toolBarTool;
            this.bandTool.Header = -1;
            this.bandTool.Integral = 1;
            this.bandTool.MaxHeight = MAX_BAND_HEIGHT;
            this.bandTool.UseChevron = true;
            if (toolBarTool.Buttons.Count > 0) {
                this.bandTool.IdealWidth =
                    toolBarTool.Buttons[toolBarTool.Buttons.Count - 1].Rectangle.Right + chevron_width;
            }
            this.bandTool.BandSize = EditorManager.editorConfig.BandSizeTool;
            this.bandTool.NewRow = EditorManager.editorConfig.BandNewRowTool;
            // 一度リストに入れてから追加する
            var bands = new RebarBand[] { null, null, null, null };
            // 番号がおかしくないかチェック
            if (EditorManager.editorConfig.BandOrderFile < 0 || bands.Length <= EditorManager.editorConfig.BandOrderFile) EditorManager.editorConfig.BandOrderFile = 0;
            if (EditorManager.editorConfig.BandOrderMeasure < 0 || bands.Length <= EditorManager.editorConfig.BandOrderMeasure) EditorManager.editorConfig.BandOrderMeasure = 0;
            if (EditorManager.editorConfig.BandOrderPosition < 0 || bands.Length <= EditorManager.editorConfig.BandOrderPosition) EditorManager.editorConfig.BandOrderPosition = 0;
            if (EditorManager.editorConfig.BandOrderTool < 0 || bands.Length <= EditorManager.editorConfig.BandOrderTool) EditorManager.editorConfig.BandOrderTool = 0;
            bands[EditorManager.editorConfig.BandOrderFile] = bandFile;
            bands[EditorManager.editorConfig.BandOrderMeasure] = bandMeasure;
            bands[EditorManager.editorConfig.BandOrderPosition] = bandPosition;
            bands[EditorManager.editorConfig.BandOrderTool] = bandTool;
            // nullチェック
            bool null_exists = false;
            for (var i = 0; i < bands.Length; i++) {
                if (bands[i] == null) {
                    null_exists = true;
                    break;
                }
            }
            if (null_exists) {
                // 番号に矛盾があれば，デフォルトの並び方で
                bands[0] = bandFile;
                bands[1] = bandMeasure;
                bands[2] = bandPosition;
                bands[3] = bandTool;
                bandFile.NewRow = true;
                bandMeasure.NewRow = true;
                bandPosition.NewRow = true;
                bandTool.NewRow = true;
            }

            // 追加
            for (var i = 0; i < bands.Length; i++) {
                if (i == 0) bands[i].NewRow = true;
                bands[i].MinHeight = 24;
                this.rebar.Bands.Add(bands[i]);
            }

#if DEBUG
            sout.println("FormMain#.ctor; this.Width=" + this.Width);
#endif
            bandTool.Resize += this.toolStripEdit_Resize;
            bandMeasure.Resize += this.toolStripMeasure_Resize;
            bandPosition.Resize += this.toolStripPosition_Resize;
            bandFile.Resize += this.toolStripFile_Resize;

            updateSplitContainer2Size(false);

            ensureVisibleY(60);

            // 鍵盤用の音源の準備．Javaはこの機能は削除で．
            // 鍵盤用のキャッシュが古い位置に保存されている場合。
            string cache_new = ApplicationGlobal.getKeySoundPath();
            string cache_old = Path.Combine(PortUtil.getApplicationStartupPath(), "cache");
            if (Directory.Exists(cache_old)) {
                bool exists = false;
                for (int i = 0; i < 127; i++) {
                    string s = Path.Combine(cache_new, i + ".wav");
                    if (System.IO.File.Exists(s)) {
                        exists = true;
                        break;
                    }
                }

                // 新しいキャッシュが1つも無い場合に、古いディレクトリからコピーする
                if (!exists) {
                    for (int i = 0; i < 127; i++) {
                        string wav_from = Path.Combine(cache_old, i + ".wav");
                        string wav_to = Path.Combine(cache_new, i + ".wav");
                        if (System.IO.File.Exists(wav_from)) {
                            try {
                                PortUtil.copyFile(wav_from, wav_to);
                                PortUtil.deleteFile(wav_from);
                            } catch (Exception ex) {
                                Logger.write(typeof(FormMain) + ".FormMain_Load; ex=" + ex + "\n");
                                serr.println("FormMain#FormMain_Load; ex=" + ex);
                            }
                        }
                    }
                }
            }

            // 足りてないキャッシュがひとつでもあればFormGenerateKeySound発動する
            bool cache_is_incomplete = false;
            for (int i = 0; i < 127; i++) {
                string wav = Path.Combine(cache_new, i + ".wav");
                if (!System.IO.File.Exists(wav)) {
                    cache_is_incomplete = true;
                    break;
                }
            }

            bool init_key_sound_player_immediately = true; //FormGenerateKeySoundの終了を待たずにKeySoundPlayer.initするかどうか。
            if (!ApplicationGlobal.appConfig.DoNotAskKeySoundGeneration && cache_is_incomplete) {
                FormAskKeySoundGenerationController dialog = null;
                int dialog_result = 0;
                bool always_check_this = !ApplicationGlobal.appConfig.DoNotAskKeySoundGeneration;
                try {
                    dialog = new FormAskKeySoundGenerationController();
                    dialog.setupUi(new FormAskKeySoundGenerationUiImpl(dialog));
                    dialog.getUi().setAlwaysPerformThisCheck(always_check_this);
                    dialog_result = DialogManager.showModalDialog(dialog.getUi(), this);
                    always_check_this = dialog.getUi().isAlwaysPerformThisCheck();
                } catch (Exception ex) {
                    Logger.write(typeof(FormMain) + ".FormMain_Load; ex=" + ex + "\n");
                    serr.println("FormMain#FormMain_Load; ex=" + ex);
                } finally {
                    if (dialog != null) {
                        try {
                            dialog.getUi().close(true);
                        } catch (Exception ex2) {
                            Logger.write(typeof(FormMain) + ".FormMain_Load; ex=" + ex2 + "\n");
                            serr.println("FormMain#FormMain_Load; ex2=" + ex2);
                        }
                    }
                }
                ApplicationGlobal.appConfig.DoNotAskKeySoundGeneration = !always_check_this;

                if (dialog_result == 1) {
                    FormGenerateKeySound form = null;
                    try {
                        form = ApplicationUIHost.Create<FormGenerateKeySound>(true);
                        form.FormClosed += FormGenerateKeySound_FormClosed;
                        form.ShowDialog();
                    } catch (Exception ex) {
                        Logger.write(typeof(FormMain) + ".FormMain_Load; ex=" + ex + "\n");
                        serr.println("FormMain#FormMain_Load; ex=" + ex);
                    }
                    init_key_sound_player_immediately = false;
                }
            }

            if (init_key_sound_player_immediately) {
                try {
                    KeySoundPlayer.init();
                } catch (Exception ex) {
                    Logger.write(typeof(FormMain) + ".FormMain_Load; ex=" + ex + "\n");
                    serr.println("FormMain#FormMain_Load; ex=" + ex);
                }
            }

            if (!ApplicationGlobal.appConfig.DoNotAutomaticallyCheckForUpdates) {
                showUpdateInformationAsync(false);
            }
        }

        public void FormGenerateKeySound_FormClosed(Object sender, EventArgs e)
        {
            try {
                KeySoundPlayer.init();
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".FormGenerateKeySound_FormClosed; ex=" + ex + "\n");
                serr.println("FormMain#FormGenerateKeySound_FormClosed; ex=" + ex);
            }
        }

        void FormMain_SizeChanged(object sender, EventArgs e)
        {
            if (mWindowState == this.WindowState) {
                return;
            }
            var state = this.WindowState;
            if (state == FormWindowState.Normal || state == FormWindowState.Maximized) {
                if (state == FormWindowState.Normal) {
                    var bounds = this.Bounds;
                    EditorManager.editorConfig.WindowRect = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                }
#if ENABLE_PROPERTY
                // プロパティウィンドウの状態を更新
                if (EditorManager.editorConfig.PropertyWindowStatus.State == PanelState.Window) {
                    if (EditorManager.propertyWindow.getUi().isWindowMinimized()) {
                        EditorManager.propertyWindow.getUi().deiconfyWindow();
                    }
                    if (!EditorManager.propertyWindow.getUi().isVisible()) {
                        EditorManager.propertyWindow.getUi().setVisible(true);
                    }
                }
#endif
                // ミキサーウィンドウの状態を更新
                bool vm = EditorManager.editorConfig.MixerVisible;
                if (vm != EditorManager.MixerWindow.Visible) {
                    EditorManager.MixerWindow.Visible = vm;
                }

                // アイコンパレットの状態を更新
                if (EditorManager.iconPalette != null && menuVisualIconPalette.Checked) {
                    if (!EditorManager.iconPalette.Visible) {
                        EditorManager.iconPalette.Visible = true;
                    }
                }
                updateLayout();
                this.Focus();
            } else if (state == FormWindowState.Minimized) {
#if ENABLE_PROPERTY
                EditorManager.propertyWindow.getUi().setVisible(false);
#endif
                EditorManager.MixerWindow.Visible = false;
                if (EditorManager.iconPalette != null) {
                    EditorManager.iconPalette.Visible = false;
                }
            }/* else if ( state == BForm.MAXIMIZED_BOTH ) {
#if ENABLE_PROPERTY
                EditorManager.propertyWindow.setExtendedState( BForm.NORMAL );
                EditorManager.propertyWindow.setVisible( EditorManager.editorConfig.PropertyWindowStatus.State == PanelState.Window );
#endif
                EditorManager.MixerWindow.setVisible( EditorManager.editorConfig.MixerVisible );
                if ( EditorManager.iconPalette != null && menuVisualIconPalette.isSelected() ) {
                    EditorManager.iconPalette.setVisible( true );
                }
                this.requestFocus();
            }*/
        }

        public void FormMain_MouseWheel(Object sender, MouseEventArgs e)
        {
#if DEBUG
            sout.println("FormMain#FormMain_MouseWheel");
#endif
            if (((Keys) Control.ModifierKeys & Keys.Shift) == Keys.Shift) {
                hScroll.Value = computeScrollValueFromWheelDelta(e.Delta);
            } else {
                int max = vScroll.Maximum - vScroll.LargeChange;
                int min = vScroll.Minimum;
                double new_val = (double)vScroll.Value - e.Delta;
                if (new_val > max) {
                    vScroll.Value = max;
                } else if (new_val < min) {
                    vScroll.Value = min;
                } else {
                    vScroll.Value = (int)new_val;
                }
            }
            refreshScreen();
        }

        public void FormMain_PreviewKeyDown(Object sender, PreviewKeyDownEventArgs e)
        {
#if DEBUG
            sout.println("FormMain#FormMain_PreviewKeyDown");
#endif
            var ex = new NKeyEventArgs((Keys) e.KeyData);
            processSpecialShortcutKey(ex, true);
        }

        public void handleVScrollResize(Object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized) {
                updateScrollRangeVertical();
                controller.setStartToDrawY(calculateStartToDrawY(vScroll.Value));
            }
        }

        public void FormMain_Deactivate(Object sender, EventArgs e)
        {
            mFormActivated = false;
        }

        public void FormMain_Activated(Object sender, EventArgs e)
        {
            mFormActivated = true;
        }
        #endregion

        #region mTimer
        public void mTimer_Tick(Object sender, EventArgs e)
        {
            if (!mFormActivated) {
                return;
            }
            try {
                double now = PortUtil.getCurrentTime();
                byte[] buttons;
                int pov0;
                bool ret = winmmhelp.JoyGetStatus(0, out buttons, out pov0);
                bool event_processed = false;
                double dt_ms = (now - mLastEventProcessed) * 1000.0;

                EditorConfig m = EditorManager.editorConfig;
                bool btn_x = (0 <= m.GameControlerCross && m.GameControlerCross < buttons.Length && buttons[m.GameControlerCross] > 0x00);
                bool btn_o = (0 <= m.GameControlerCircle && m.GameControlerCircle < buttons.Length && buttons[m.GameControlerCircle] > 0x00);
                bool btn_tr = (0 <= m.GameControlerTriangle && m.GameControlerTriangle < buttons.Length && buttons[m.GameControlerTriangle] > 0x00);
                bool btn_re = (0 <= m.GameControlerRectangle && m.GameControlerRectangle < buttons.Length && buttons[m.GameControlerRectangle] > 0x00);
                bool pov_r = pov0 == m.GameControlPovRight;
                bool pov_l = pov0 == m.GameControlPovLeft;
                bool pov_u = pov0 == m.GameControlPovUp;
                bool pov_d = pov0 == m.GameControlPovDown;
                bool L1 = (0 <= m.GameControlL1 && m.GameControlL1 < buttons.Length && buttons[m.GameControlL1] > 0x00);
                bool R1 = (0 <= m.GameControlL2 && m.GameControlL2 < buttons.Length && buttons[m.GameControlR1] > 0x00);
                bool L2 = (0 <= m.GameControlR1 && m.GameControlR1 < buttons.Length && buttons[m.GameControlL2] > 0x00);
                bool R2 = (0 <= m.GameControlR2 && m.GameControlR2 < buttons.Length && buttons[m.GameControlR2] > 0x00);
                bool SELECT = (0 <= m.GameControlSelect && m.GameControlSelect <= buttons.Length && buttons[m.GameControlSelect] > 0x00);
                if (mGameMode == GameControlMode.NORMAL) {
                    mLastBtnX = btn_x;

                    if (!event_processed && !btn_o && mLastBtnO) {
                        if (EditorManager.isPlaying()) {
                            timer.Stop();
                        }
                        EditorManager.setPlaying(!EditorManager.isPlaying(), this);
                        mLastEventProcessed = now;
                        event_processed = true;
                    }
                    mLastBtnO = btn_o;

                    if (!event_processed && pov_r && dt_ms > EditorManager.editorConfig.GameControlerMinimumEventInterval) {
                        forward();
                        mLastEventProcessed = now;
                        event_processed = true;
                    }
                    mLastPovR = pov_r;

                    if (!event_processed && pov_l && dt_ms > EditorManager.editorConfig.GameControlerMinimumEventInterval) {
                        rewind();
                        mLastEventProcessed = now;
                        event_processed = true;
                    }
                    mLastPovL = pov_l;

                    if (!event_processed && pov_u && dt_ms > EditorManager.editorConfig.GameControlerMinimumEventInterval) {
                        int draft_vscroll = vScroll.Value - (int)(100 * controller.getScaleY()) * 3;
                        if (draft_vscroll < vScroll.Minimum) {
                            draft_vscroll = vScroll.Minimum;
                        }
                        vScroll.Value = draft_vscroll;
                        refreshScreen();
                        mLastEventProcessed = now;
                        event_processed = true;
                    }

                    if (!event_processed && pov_d && dt_ms > EditorManager.editorConfig.GameControlerMinimumEventInterval) {
                        int draft_vscroll = vScroll.Value + (int)(100 * controller.getScaleY()) * 3;
                        if (draft_vscroll > vScroll.Maximum) {
                            draft_vscroll = vScroll.Maximum;
                        }
                        vScroll.Value = draft_vscroll;
                        refreshScreen();
                        mLastEventProcessed = now;
                        event_processed = true;
                    }

                    if (!event_processed && !SELECT && mLastBtnSelect) {
                        event_processed = true;
                        mGameMode = GameControlMode.KEYBOARD;
                        stripLblGameCtrlMode.Text = mGameMode.ToString();
						stripLblGameCtrlMode.Image = Properties.Resources.piano.ToAwt ();
                    }
                    mLastBtnSelect = SELECT;
                } else if (mGameMode == GameControlMode.KEYBOARD) {
                    if (!event_processed && !SELECT && mLastBtnSelect) {
                        event_processed = true;
                        mGameMode = GameControlMode.NORMAL;
                        updateGameControlerStatus(null, null);
                        mLastBtnSelect = SELECT;
                        return;
                    }
                    mLastBtnSelect = SELECT;

                    int note = -1;
                    if (pov_r && !mLastPovR) {
                        note = 60;
                    } else if (btn_re && !mLastBtnRe) {
                        note = 62;
                    } else if (btn_tr && !mLastBtnTr) {
                        note = 64;
                    } else if (btn_o && !mLastBtnO) {
                        note = 65;
                    } else if (btn_x && !mLastBtnX) {
                        note = 67;
                    } else if (pov_u && !mLastPovU) {
                        note = 59;
                    } else if (pov_l && !mLastPovL) {
                        note = 57;
                    } else if (pov_d && !mLastPovD) {
                        note = 55;
                    }
                    if (note >= 0) {
                        if (L1) {
                            note += 12;
                        } else if (L2) {
                            note -= 12;
                        }
                        if (R1) {
                            note += 1;
                        } else if (R2) {
                            note -= 1;
                        }
                    }
                    mLastBtnO = btn_o;
                    mLastBtnX = btn_x;
                    mLastBtnRe = btn_re;
                    mLastBtnTr = btn_tr;
                    mLastPovL = pov_l;
                    mLastPovD = pov_d;
                    mLastPovR = pov_r;
                    mLastPovU = pov_u;
                    if (note >= 0) {
#if DEBUG
                        CDebug.WriteLine("FormMain#mTimer_Tick");
                        CDebug.WriteLine("    note=" + note);
#endif
                        if (EditorManager.isPlaying()) {
                            int clock = EditorManager.getCurrentClock();
                            int selected = EditorManager.Selected;
                            if (EditorManager.mAddingEvent != null) {
                                EditorManager.mAddingEvent.ID.setLength(clock - EditorManager.mAddingEvent.Clock);
                                CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventAdd(selected,
                                                                                                               EditorManager.mAddingEvent));
                                EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
                                if (!isEdited()) {
                                    setEdited(true);
                                }
                                updateDrawObjectList();
                            }
                            EditorManager.mAddingEvent = new VsqEvent(clock, new VsqID(0));
                            EditorManager.mAddingEvent.ID.type = VsqIDType.Anote;
                            EditorManager.mAddingEvent.ID.Dynamics = 64;
                            EditorManager.mAddingEvent.ID.VibratoHandle = null;
                            EditorManager.mAddingEvent.ID.LyricHandle = new LyricHandle("a", "a");
                            EditorManager.mAddingEvent.ID.Note = note;
                        }
                        KeySoundPlayer.play(note);
                    } else {
                        if (EditorManager.isPlaying() && EditorManager.mAddingEvent != null) {
                            EditorManager.mAddingEvent.ID.setLength(EditorManager.getCurrentClock() - EditorManager.mAddingEvent.Clock);
                        }
                    }
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".mTimer_Tick; ex=" + ex + "\n");
#if DEBUG
                CDebug.WriteLine("    ex=" + ex);
#endif
                mGameMode = GameControlMode.DISABLED;
                updateGameControlerStatus(null, null);
                mTimer.Stop();
            }
        }
        #endregion

        //BOOKMARK: menuFile
        #region menuFile*
        public void menuFileExport_DropDownOpening(Object sender, EventArgs e)
        {
            menuFileExportWave.Enabled = (MusicManager.getVsqFile().Track[EditorManager.Selected].getEventCount() > 0);
        }


        #endregion

        //BOOKMARK: menuSetting
        #region menuSetting*
        #endregion

        //BOOKMARK: menuEdit
        #region menuEdit*
        public void menuEdit_DropDownOpening(Object sender, EventArgs e)
        {
            updateCopyAndPasteButtonStatus();
        }

        #endregion

        //BOOKMARK: vScroll
        #region vScroll
        public void vScroll_Enter(Object sender, EventArgs e)
        {
            focusPianoRoll();
        }

        public void vScroll_ValueChanged(Object sender, EventArgs e)
        {
            controller.setStartToDrawY(calculateStartToDrawY(vScroll.Value));
            if (EditorManager.EditMode != EditMode.MIDDLE_DRAG) {
                // MIDDLE_DRAGのときは，pictPianoRoll_MouseMoveでrefreshScreenされるので，それ以外のときだけ描画・
                refreshScreen(true);
            }
        }
        #endregion

        //BOOKMARK: waveView
        #region waveView
        public void waveView_MouseDoubleClick(Object sender, NMouseEventArgs e)
        {
            if (e.Button == NMouseButtons.Middle) {
                // ツールをポインター <--> 鉛筆に切り替える
                if (e.Y < trackSelector.Height - TrackSelectorConsts.OFFSET_TRACK_TAB * 2) {
                    if (EditorManager.SelectedTool == EditTool.ARROW) {
                        EditorManager.SelectedTool = (EditTool.PENCIL);
                    } else {
                        EditorManager.SelectedTool = (EditTool.ARROW);
                    }
                }
            }
        }

        public void waveView_MouseDown(Object sender, NMouseEventArgs e)
        {
#if DEBUG
            sout.println("waveView_MouseDown; isMiddleButtonDowned=" + isMouseMiddleButtonDowned(e.Button));
#endif
            if (isMouseMiddleButtonDowned(e.Button)) {
                mEditCurveMode = CurveEditMode.MIDDLE_DRAG;
                mButtonInitial = new Point(e.X, e.Y);
                mMiddleButtonHScroll = hScroll.Value;
                this.Cursor = HAND;
            }
        }

        public void waveView_MouseUp(Object sender, NMouseEventArgs e)
        {
            if (mEditCurveMode == CurveEditMode.MIDDLE_DRAG) {
                mEditCurveMode = CurveEditMode.NONE;
                this.Cursor = Cursors.Default;
            }
        }

        public void waveView_MouseMove(Object sender, NMouseEventArgs e)
        {
            if (mEditCurveMode == CurveEditMode.MIDDLE_DRAG) {
                int draft = computeHScrollValueForMiddleDrag(e.X);
                if (hScroll.Value != draft) {
                    hScroll.Value = draft;
                }
            }
        }
        #endregion

        //BOOKMARK: hScroll
        #region hScroll
        public void hScroll_Enter(Object sender, EventArgs e)
        {
            focusPianoRoll();
        }

        public void hScroll_Resize(Object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized) {
                updateScrollRangeHorizontal();
            }
        }

        public void hScroll_ValueChanged(Object sender, EventArgs e)
        {
            int stdx = calculateStartToDrawX();
            controller.setStartToDrawX(stdx);
            if (EditorManager.EditMode != EditMode.MIDDLE_DRAG) {
                // MIDDLE_DRAGのときは，pictPianoRoll_MouseMoveでrefreshScreenされるので，それ以外のときだけ描画・
                refreshScreen(true);
            }
        }
        #endregion

        //BOOKMARK: picturePositionIndicator
        #region picturePositionIndicator
        public void picturePositionIndicator_MouseWheel(Object sender, NMouseEventArgs e)
        {
#if DEBUG
            sout.println("FormMain#picturePositionIndicator_MouseWheel");
#endif
            hScroll.Value = computeScrollValueFromWheelDelta(e.Delta);
        }

        public void picturePositionIndicator_MouseClick(Object sender, NMouseEventArgs e)
        {
            if (e.Button == NMouseButtons.Right && 0 < e.Y && e.Y <= 18 && EditorManager.keyWidth < e.X) {
                // クリックされた位置でのクロックを保存
                int clock = EditorManager.clockFromXCoord(e.X);
                int unit = EditorManager.getPositionQuantizeClock();
                clock = FormMainModel.Quantize(clock, unit);
                if (clock < 0) {
                    clock = 0;
                }
                mPositionIndicatorPopupShownClock = clock;
                cMenuPositionIndicator.Show(picturePositionIndicator, e.X, e.Y);
            }
        }

        public void picturePositionIndicator_MouseDoubleClick(Object sender, NMouseEventArgs e)
        {
            if (e.X < EditorManager.keyWidth || this.Width - 3 < e.X) {
                return;
            }
            if (e.Button == NMouseButtons.Left) {
                VsqFileEx vsq = MusicManager.getVsqFile();
                if (18 < e.Y && e.Y <= 32) {
                    #region テンポの変更
#if DEBUG
                    CDebug.WriteLine("TempoChange");
#endif
                    EditorManager.itemSelection.clearEvent();
                    EditorManager.itemSelection.clearTimesig();

                    if (EditorManager.itemSelection.getTempoCount() > 0) {
                        #region テンポ変更があった場合
                        int index = -1;
                        int clock = EditorManager.itemSelection.getLastTempoClock();
                        for (int i = 0; i < vsq.TempoTable.Count; i++) {
                            if (clock == vsq.TempoTable[i].Clock) {
                                index = i;
                                break;
                            }
                        }
                        if (index >= 0) {
                            if (EditorManager.SelectedTool == EditTool.ERASER) {
                                #region ツールがEraser
                                if (vsq.TempoTable[index].Clock == 0) {
                                    string msg = _("Cannot remove first symbol of track!");
                                    statusLabel.Text = msg;
                                    SystemSounds.Asterisk.Play();
                                    return;
                                }
                                CadenciiCommand run = new CadenciiCommand(
                                    VsqCommand.generateCommandUpdateTempo(vsq.TempoTable[index].Clock,
                                                                           vsq.TempoTable[index].Clock,
                                                                           -1));
                                EditorManager.editHistory.register(vsq.executeCommand(run));
                                setEdited(true);
                                #endregion
                            } else {
                                #region ツールがEraser以外
                                TempoTableEntry tte = vsq.TempoTable[index];
                                EditorManager.itemSelection.clearTempo();
                                EditorManager.itemSelection.addTempo(tte.Clock);
                                int bar_count = vsq.getBarCountFromClock(tte.Clock);
                                int bar_top_clock = vsq.getClockFromBarCount(bar_count);
                                //int local_denominator, local_numerator;
                                Timesig timesig = vsq.getTimesigAt(tte.Clock);
                                int clock_per_beat = 480 * 4 / timesig.denominator;
                                int clocks_in_bar = tte.Clock - bar_top_clock;
                                int beat_in_bar = clocks_in_bar / clock_per_beat + 1;
                                int clocks_in_beat = clocks_in_bar - (beat_in_bar - 1) * clock_per_beat;
                                FormTempoConfig dlg = null;
                                try {
                                    dlg = ApplicationUIHost.Create<FormTempoConfig>(bar_count, beat_in_bar, timesig.numerator, clocks_in_beat, clock_per_beat, (float)(6e7 / tte.Tempo), MusicManager.getVsqFile().getPreMeasure());
                                    dlg.Location = getFormPreferedLocation((Form)dlg.Native).ToAwt ();
                                    var dr = DialogManager.showModalDialog(dlg, this);
                                    if (dr == 1) {
                                        int new_beat = dlg.getBeatCount();
                                        int new_clocks_in_beat = dlg.getClock();
                                        int new_clock = bar_top_clock + (new_beat - 1) * clock_per_beat + new_clocks_in_beat;
                                        CadenciiCommand run = new CadenciiCommand(
                                            VsqCommand.generateCommandUpdateTempo(new_clock, new_clock, (int)(6e7 / (double)dlg.getTempo())));
                                        EditorManager.editHistory.register(vsq.executeCommand(run));
                                        setEdited(true);
                                        refreshScreen();
                                    }
                                } catch (Exception ex) {
                                    Logger.write(typeof(FormMain) + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex + "\n");
                                    serr.println("FormMain#picturePositionIndicator_MouseDoubleClick; ex=" + ex);
                                } finally {
                                    if (dlg != null) {
                                        try {
                                            dlg.Close();
                                        } catch (Exception ex2) {
                                            Logger.write(typeof(FormMain) + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex2 + "\n");
                                            serr.println("FormMain#picturePositionIndicator_MouseDoubleClick; ex2=" + ex2);
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion
                    } else {
                        #region テンポ変更がなかった場合
                        EditorManager.itemSelection.clearEvent();
                        EditorManager.itemSelection.clearTempo();
                        EditorManager.itemSelection.clearTimesig();
                        EditTool selected = EditorManager.SelectedTool;
                        if (selected == EditTool.PENCIL ||
                            selected == EditTool.LINE) {
                            int changing_clock = EditorManager.clockFromXCoord(e.X);
                            int changing_tempo = vsq.getTempoAt(changing_clock);
                            int bar_count;
                            int bar_top_clock;
                            int local_denominator, local_numerator;
                            bar_count = vsq.getBarCountFromClock(changing_clock);
                            bar_top_clock = vsq.getClockFromBarCount(bar_count);
                            int index2 = -1;
                            for (int i = 0; i < vsq.TimesigTable.Count; i++) {
                                if (vsq.TimesigTable[i].BarCount > bar_count) {
                                    index2 = i;
                                    break;
                                }
                            }
                            if (index2 >= 1) {
                                local_denominator = vsq.TimesigTable[index2 - 1].Denominator;
                                local_numerator = vsq.TimesigTable[index2 - 1].Numerator;
                            } else {
                                local_denominator = vsq.TimesigTable[0].Denominator;
                                local_numerator = vsq.TimesigTable[0].Numerator;
                            }
                            int clock_per_beat = 480 * 4 / local_denominator;
                            int clocks_in_bar = changing_clock - bar_top_clock;
                            int beat_in_bar = clocks_in_bar / clock_per_beat + 1;
                            int clocks_in_beat = clocks_in_bar - (beat_in_bar - 1) * clock_per_beat;
                            FormTempoConfig dlg = null;
                            try {
                                dlg = ApplicationUIHost.Create<FormTempoConfig>(bar_count - vsq.getPreMeasure() + 1,
                                                           beat_in_bar,
                                                           local_numerator,
                                                           clocks_in_beat,
                                                           clock_per_beat,
                                                           (float)(6e7 / changing_tempo),
                                                           vsq.getPreMeasure());
                                dlg.Location = model.GetFormPreferedLocation(dlg);
                                var dr = DialogManager.showModalDialog(dlg, this);
                                if (dr == 1) {
                                    int new_beat = dlg.getBeatCount();
                                    int new_clocks_in_beat = dlg.getClock();
                                    int new_clock = bar_top_clock + (new_beat - 1) * clock_per_beat + new_clocks_in_beat;
#if DEBUG
                                    CDebug.WriteLine("    new_beat=" + new_beat);
                                    CDebug.WriteLine("    new_clocks_in_beat=" + new_clocks_in_beat);
                                    CDebug.WriteLine("    changing_clock=" + changing_clock);
                                    CDebug.WriteLine("    new_clock=" + new_clock);
#endif
                                    CadenciiCommand run = new CadenciiCommand(
                                        VsqCommand.generateCommandUpdateTempo(new_clock, new_clock, (int)(6e7 / (double)dlg.getTempo())));
                                    EditorManager.editHistory.register(vsq.executeCommand(run));
                                    setEdited(true);
                                    refreshScreen();
                                }
                            } catch (Exception ex) {
                                Logger.write(typeof(FormMain) + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex + "\n");
                            } finally {
                                if (dlg != null) {
                                    try {
                                        dlg.Close();
                                    } catch (Exception ex2) {
                                        Logger.write(typeof(FormMain) + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex2 + "\n");
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.NONE;
                    #endregion
                } else if (32 < e.Y && e.Y <= picturePositionIndicator.Height - 1) {
                    #region 拍子の変更
                    EditorManager.itemSelection.clearEvent();
                    EditorManager.itemSelection.clearTempo();
                    if (EditorManager.itemSelection.getTimesigCount() > 0) {
                        #region 拍子変更があった場合
                        int index = 0;
                        int last_barcount = EditorManager.itemSelection.getLastTimesigBarcount();
                        for (int i = 0; i < vsq.TimesigTable.Count; i++) {
                            if (vsq.TimesigTable[i].BarCount == last_barcount) {
                                index = i;
                                break;
                            }
                        }
                        if (EditorManager.SelectedTool == EditTool.ERASER) {
                            #region ツールがEraser
                            if (vsq.TimesigTable[index].Clock == 0) {
                                string msg = _("Cannot remove first symbol of track!");
                                statusLabel.Text = msg;
                                SystemSounds.Asterisk.Play();
                                return;
                            }
                            int barcount = vsq.TimesigTable[index].BarCount;
                            CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandUpdateTimesig(barcount, barcount, -1, -1));
                            EditorManager.editHistory.register(vsq.executeCommand(run));
                            setEdited(true);
                            #endregion
                        } else {
                            #region ツールがEraser以外
                            int pre_measure = vsq.getPreMeasure();
                            int clock = EditorManager.clockFromXCoord(e.X);
                            int bar_count = vsq.getBarCountFromClock(clock);
                            int total_clock = vsq.TotalClocks;
                            Timesig timesig = vsq.getTimesigAt(clock);
                            bool num_enabled = !(bar_count == 0);
                            FormBeatConfigController dlg = null;
                            try {
                                dlg = new FormBeatConfigController(c => new FormBeatConfigUiImpl (c), bar_count - pre_measure + 1, timesig.numerator, timesig.denominator, num_enabled, pre_measure);
                                var p = getFormPreferedLocation(dlg.getWidth(), dlg.getHeight());
                                dlg.setLocation(p.X, p.Y);
                                int dr = DialogManager.showModalDialog(dlg.getUi(), this);
                                if (dr == 1) {
                                    if (dlg.isEndSpecified()) {
                                        int[] new_barcounts = new int[2];
                                        int[] numerators = new int[2];
                                        int[] denominators = new int[2];
                                        int[] barcounts = new int[2];
                                        new_barcounts[0] = dlg.getStart() + pre_measure - 1;
                                        new_barcounts[1] = dlg.getEnd() + pre_measure - 1;
                                        numerators[0] = dlg.getNumerator();
                                        denominators[0] = dlg.getDenominator();
                                        numerators[1] = timesig.numerator;
                                        denominators[1] = timesig.denominator;
                                        barcounts[0] = bar_count;
                                        barcounts[1] = dlg.getEnd() + pre_measure - 1;
                                        CadenciiCommand run = new CadenciiCommand(
                                            VsqCommand.generateCommandUpdateTimesigRange(barcounts, new_barcounts, numerators, denominators));
                                        EditorManager.editHistory.register(vsq.executeCommand(run));
                                        setEdited(true);
                                    } else {
#if DEBUG
                                        sout.println("picturePositionIndicator_MouseDoubleClick");
                                        sout.println("    bar_count=" + bar_count);
                                        sout.println("    dlg.Start+pre_measure-1=" + (dlg.getStart() + pre_measure - 1));
#endif
                                        CadenciiCommand run = new CadenciiCommand(
                                            VsqCommand.generateCommandUpdateTimesig(bar_count,
                                                                                     dlg.getStart() + pre_measure - 1,
                                                                                     dlg.getNumerator(),
                                                                                     dlg.getDenominator()));
                                        EditorManager.editHistory.register(vsq.executeCommand(run));
                                        setEdited(true);
                                    }
                                }
                            } catch (Exception ex) {
                                Logger.write(typeof(FormMain) + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex + "\n");
                                serr.println("FormMain#picturePositionIndicator_MouseDoubleClick; ex=" + ex);
                            } finally {
                                if (dlg != null) {
                                    try {
                                        dlg.close();
                                    } catch (Exception ex2) {
                                        Logger.write(typeof(FormMain) + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex2 + "\n");
                                        serr.println("FormMain#picturePositionIndicator_MouseDoubleClic; ex2=" + ex2);
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion
                    } else {
                        #region 拍子変更がなかった場合
                        EditorManager.itemSelection.clearEvent();
                        EditorManager.itemSelection.clearTempo();
                        EditorManager.itemSelection.clearTimesig();
                        EditTool selected = EditorManager.SelectedTool;
                        if (selected == EditTool.PENCIL ||
                            selected == EditTool.LINE) {
                            int pre_measure = MusicManager.getVsqFile().getPreMeasure();
                            int clock = EditorManager.clockFromXCoord(e.X);
                            int bar_count = MusicManager.getVsqFile().getBarCountFromClock(clock);
                            int numerator, denominator;
                            Timesig timesig = MusicManager.getVsqFile().getTimesigAt(clock);
                            int total_clock = MusicManager.getVsqFile().TotalClocks;
                            //int max_barcount = EditorManager.VsqFile.getBarCountFromClock( total_clock ) - pre_measure + 1;
                            //int min_barcount = 1;
#if DEBUG
                            CDebug.WriteLine("FormMain.picturePositionIndicator_MouseClick; bar_count=" + (bar_count - pre_measure + 1));
#endif
                            FormBeatConfigController dlg = null;
                            try {
                                dlg = new FormBeatConfigController(c => new FormBeatConfigUiImpl (c), bar_count - pre_measure + 1, timesig.numerator, timesig.denominator, true, pre_measure);
                                var p = getFormPreferedLocation(dlg.getWidth(), dlg.getHeight());
                                dlg.setLocation(p.X, p.Y);
                                int dr = DialogManager.showModalDialog(dlg.getUi(), this);
                                if (dr == 1) {
                                    if (dlg.isEndSpecified()) {
                                        int[] new_barcounts = new int[2];
                                        int[] numerators = new int[2];
                                        int[] denominators = new int[2];
                                        int[] barcounts = new int[2];
                                        new_barcounts[0] = dlg.getStart() + pre_measure - 1;
                                        new_barcounts[1] = dlg.getEnd() + pre_measure - 1 + 1;
                                        numerators[0] = dlg.getNumerator();
                                        numerators[1] = timesig.numerator;

                                        denominators[0] = dlg.getDenominator();
                                        denominators[1] = timesig.denominator;

                                        barcounts[0] = dlg.getStart() + pre_measure - 1;
                                        barcounts[1] = dlg.getEnd() + pre_measure - 1 + 1;
                                        CadenciiCommand run = new CadenciiCommand(
                                            VsqCommand.generateCommandUpdateTimesigRange(barcounts, new_barcounts, numerators, denominators));
                                        EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
                                        setEdited(true);
                                    } else {
                                        CadenciiCommand run = new CadenciiCommand(
                                            VsqCommand.generateCommandUpdateTimesig(bar_count,
                                                                           dlg.getStart() + pre_measure - 1,
                                                                           dlg.getNumerator(),
                                                                           dlg.getDenominator()));
                                        EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
                                        setEdited(true);
                                    }
                                }
                            } catch (Exception ex) {
                                Logger.write(typeof(FormMain) + ".picutrePositionIndicator_MouseDoubleClick; ex=" + ex + "\n");
                            } finally {
                                if (dlg != null) {
                                    try {
                                        dlg.close();
                                    } catch (Exception ex2) {
                                        Logger.write(typeof(FormMain) + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex2 + "\n");
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.NONE;
                    #endregion
                }
                picturePositionIndicator.Refresh();
                pictPianoRoll.Refresh();
            }
        }

        public void picturePositionIndicator_MouseDown(Object sender, NMouseEventArgs e)
        {
            if (e.X < EditorManager.keyWidth || this.Width - 3 < e.X) {
                return;
            }

            mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.NONE;
            Keys modifiers = (Keys) Control.ModifierKeys;
            VsqFileEx vsq = MusicManager.getVsqFile();
            if (e.Button == NMouseButtons.Left) {
                if (0 <= e.Y && e.Y <= 18) {
                    #region スタート/エンドマーク
                    int tolerance = EditorManager.editorConfig.PxTolerance;
                    int start_marker_width = Properties.Resources.start_marker.Width;
                    int end_marker_width = Properties.Resources.end_marker.Width;
                    int startx = EditorManager.xCoordFromClocks(vsq.config.StartMarker);
                    int endx = EditorManager.xCoordFromClocks(vsq.config.EndMarker);

                    // マウスの当たり判定が重なるようなら，判定幅を最小にする
                    int start0 = startx - tolerance;
                    int start1 = startx + start_marker_width + tolerance;
                    int end0 = endx - end_marker_width - tolerance;
                    int end1 = endx + tolerance;
                    if (vsq.config.StartMarkerEnabled && vsq.config.EndMarkerEnabled) {
                        if (start0 < end1 && end1 < start1 ||
                            start1 < end0 && end0 < start1) {
                            start0 = startx;
                            start1 = startx + start_marker_width;
                            end0 = endx - end_marker_width;
                            end1 = endx;
                        }
                    }

                    if (vsq.config.StartMarkerEnabled) {
                        if (start0 <= e.X && e.X <= start1) {
                            mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.MARK_START;
                        }
                    }
                    if (vsq.config.EndMarkerEnabled && mPositionIndicatorMouseDownMode != PositionIndicatorMouseDownMode.MARK_START) {
                        if (end0 <= e.X && e.X <= end1) {
                            mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.MARK_END;
                        }
                    }
                    #endregion
                } else if (18 < e.Y && e.Y <= 32) {
                    #region テンポ
                    int index = -1;
                    int count = MusicManager.getVsqFile().TempoTable.Count;
                    for (int i = 0; i < count; i++) {
                        int clock = MusicManager.getVsqFile().TempoTable[i].Clock;
                        int x = EditorManager.xCoordFromClocks(clock);
                        if (x < 0) {
                            continue;
                        } else if (this.Width < x) {
                            break;
                        }
                        string s = PortUtil.formatDecimal("#.00", 60e6 / (float)MusicManager.getVsqFile().TempoTable[i].Tempo);
                        Dimension size = Util.measureString(s, new Font(EditorManager.editorConfig.ScreenFontName, Cadencii.Gui.Font.PLAIN, cadencii.core.EditorConfig.FONT_SIZE8));
                        if (Utility.isInRect(new Point(e.X, e.Y), new Rectangle(x, 14, (int)size.Width, 14))) {
                            index = i;
                            break;
                        }
                    }

                    if (index >= 0) {
                        int clock = MusicManager.getVsqFile().TempoTable[index].Clock;
                        if (EditorManager.SelectedTool != EditTool.ERASER) {
                            int mouse_clock = EditorManager.clockFromXCoord(e.X);
                            mTempoDraggingDeltaClock = mouse_clock - clock;
                            mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.TEMPO;
                        }
                        if ((modifiers & Keys.Shift) == Keys.Shift) {
                            if (EditorManager.itemSelection.getTempoCount() > 0) {
                                int last_clock = EditorManager.itemSelection.getLastTempoClock();
                                int start = Math.Min(last_clock, clock);
                                int end = Math.Max(last_clock, clock);
                                for (int i = 0; i < MusicManager.getVsqFile().TempoTable.Count; i++) {
                                    int tclock = MusicManager.getVsqFile().TempoTable[i].Clock;
                                    if (tclock < start) {
                                        continue;
                                    } else if (end < tclock) {
                                        break;
                                    }
                                    if (start <= tclock && tclock <= end) {
                                        EditorManager.itemSelection.addTempo(tclock);
                                    }
                                }
                            } else {
                                EditorManager.itemSelection.addTempo(clock);
                            }
                        } else if ((modifiers & s_modifier_key) == s_modifier_key) {
                            if (EditorManager.itemSelection.isTempoContains(clock)) {
                                EditorManager.itemSelection.removeTempo(clock);
                            } else {
                                EditorManager.itemSelection.addTempo(clock);
                            }
                        } else {
                            if (!EditorManager.itemSelection.isTempoContains(clock)) {
                                EditorManager.itemSelection.clearTempo();
                            }
                            EditorManager.itemSelection.addTempo(clock);
                        }
                    } else {
                        EditorManager.itemSelection.clearEvent();
                        EditorManager.itemSelection.clearTempo();
                        EditorManager.itemSelection.clearTimesig();
                    }
                    #endregion
                } else if (32 < e.Y && e.Y <= picturePositionIndicator.Height - 1) {
                    #region 拍子
                    // クリック位置に拍子が表示されているかどうか検査
                    int index = -1;
                    for (int i = 0; i < MusicManager.getVsqFile().TimesigTable.Count; i++) {
                        string s = MusicManager.getVsqFile().TimesigTable[i].Numerator + "/" + MusicManager.getVsqFile().TimesigTable[i].Denominator;
                        int x = EditorManager.xCoordFromClocks(MusicManager.getVsqFile().TimesigTable[i].Clock);
                        Dimension size = Util.measureString(s, new Font(EditorManager.editorConfig.ScreenFontName, Cadencii.Gui.Font.PLAIN, cadencii.core.EditorConfig.FONT_SIZE8));
                        if (Utility.isInRect(new Point(e.X, e.Y), new Rectangle(x, 28, (int)size.Width, 14))) {
                            index = i;
                            break;
                        }
                    }

                    if (index >= 0) {
                        int barcount = MusicManager.getVsqFile().TimesigTable[index].BarCount;
                        if (EditorManager.SelectedTool != EditTool.ERASER) {
                            int barcount_clock = MusicManager.getVsqFile().getClockFromBarCount(barcount);
                            int mouse_clock = EditorManager.clockFromXCoord(e.X);
                            mTimesigDraggingDeltaClock = mouse_clock - barcount_clock;
                            mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.TIMESIG;
                        }
                        if ((modifiers & Keys.Shift) == Keys.Shift) {
                            if (EditorManager.itemSelection.getTimesigCount() > 0) {
                                int last_barcount = EditorManager.itemSelection.getLastTimesigBarcount();
                                int start = Math.Min(last_barcount, barcount);
                                int end = Math.Max(last_barcount, barcount);
                                for (int i = 0; i < MusicManager.getVsqFile().TimesigTable.Count; i++) {
                                    int tbarcount = MusicManager.getVsqFile().TimesigTable[i].BarCount;
                                    if (tbarcount < start) {
                                        continue;
                                    } else if (end < tbarcount) {
                                        break;
                                    }
                                    if (start <= tbarcount && tbarcount <= end) {
                                        EditorManager.itemSelection.addTimesig(MusicManager.getVsqFile().TimesigTable[i].BarCount);
                                    }
                                }
                            } else {
                                EditorManager.itemSelection.addTimesig(barcount);
                            }
                        } else if ((modifiers & s_modifier_key) == s_modifier_key) {
                            if (EditorManager.itemSelection.isTimesigContains(barcount)) {
                                EditorManager.itemSelection.removeTimesig(barcount);
                            } else {
                                EditorManager.itemSelection.addTimesig(barcount);
                            }
                        } else {
                            if (!EditorManager.itemSelection.isTimesigContains(barcount)) {
                                EditorManager.itemSelection.clearTimesig();
                            }
                            EditorManager.itemSelection.addTimesig(barcount);
                        }
                    } else {
                        EditorManager.itemSelection.clearEvent();
                        EditorManager.itemSelection.clearTempo();
                        EditorManager.itemSelection.clearTimesig();
                    }
                    #endregion
                }
            }
            refreshScreen();
        }

        public void picturePositionIndicator_MouseUp(Object sender, NMouseEventArgs e)
        {
            Keys modifiers = (Keys) Control.ModifierKeys;
#if DEBUG
            CDebug.WriteLine("picturePositionIndicator_MouseClick");
#endif
            if (e.Button == NMouseButtons.Left) {
                VsqFileEx vsq = MusicManager.getVsqFile();
                if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.NONE) {
                    if (4 <= e.Y && e.Y <= 18) {
                        #region マーカー位置の変更
                        int clock = EditorManager.clockFromXCoord(e.X);
                        if (EditorManager.editorConfig.getPositionQuantize() != QuantizeMode.off) {
                            int unit = EditorManager.getPositionQuantizeClock();
                            clock = FormMainModel.Quantize(clock, unit);
                        }
                        if (EditorManager.isPlaying()) {
                            EditorManager.setPlaying(false, this);
                            EditorManager.setCurrentClock(clock);
                            EditorManager.setPlaying(true, this);
                        } else {
                            EditorManager.setCurrentClock(clock);
                        }
                        refreshScreen();
                        #endregion
                    } else if (18 < e.Y && e.Y <= 32) {
                        #region テンポの変更
#if DEBUG
                        CDebug.WriteLine("TempoChange");
#endif
                        EditorManager.itemSelection.clearEvent();
                        EditorManager.itemSelection.clearTimesig();
                        if (EditorManager.itemSelection.getTempoCount() > 0) {
                            #region テンポ変更があった場合
                            int index = -1;
                            int clock = EditorManager.itemSelection.getLastTempoClock();
                            for (int i = 0; i < vsq.TempoTable.Count; i++) {
                                if (clock == vsq.TempoTable[i].Clock) {
                                    index = i;
                                    break;
                                }
                            }
                            if (index >= 0 && EditorManager.SelectedTool == EditTool.ERASER) {
                                #region ツールがEraser
                                if (vsq.TempoTable[index].Clock == 0) {
                                    string msg = _("Cannot remove first symbol of track!");
                                    statusLabel.Text = msg;
                                    SystemSounds.Asterisk.Play();
                                    return;
                                }
                                CadenciiCommand run = new CadenciiCommand(
                                    VsqCommand.generateCommandUpdateTempo(vsq.TempoTable[index].Clock,
                                                                           vsq.TempoTable[index].Clock,
                                                                           -1));
                                EditorManager.editHistory.register(vsq.executeCommand(run));
                                setEdited(true);
                                #endregion
                            }
                            #endregion
                        }
                        mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.NONE;
                        #endregion
                    } else if (32 < e.Y && e.Y <= picturePositionIndicator.Height - 1) {
                        #region 拍子の変更
                        EditorManager.itemSelection.clearEvent();
                        EditorManager.itemSelection.clearTempo();
                        if (EditorManager.itemSelection.getTimesigCount() > 0) {
                            #region 拍子変更があった場合
                            int index = 0;
                            int last_barcount = EditorManager.itemSelection.getLastTimesigBarcount();
                            for (int i = 0; i < vsq.TimesigTable.Count; i++) {
                                if (vsq.TimesigTable[i].BarCount == last_barcount) {
                                    index = i;
                                    break;
                                }
                            }
                            if (EditorManager.SelectedTool == EditTool.ERASER) {
                                #region ツールがEraser
                                if (vsq.TimesigTable[index].Clock == 0) {
                                    string msg = _("Cannot remove first symbol of track!");
                                    statusLabel.Text = msg;
                                    SystemSounds.Asterisk.Play();
                                    return;
                                }
                                int barcount = vsq.TimesigTable[index].BarCount;
                                CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandUpdateTimesig(barcount, barcount, -1, -1));
                                EditorManager.editHistory.register(vsq.executeCommand(run));
                                setEdited(true);
                                #endregion
                            }
                            #endregion
                        }
                        #endregion
                    }
                } else if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.TEMPO) {
                    int count = EditorManager.itemSelection.getTempoCount();
                    int[] clocks = new int[count];
                    int[] new_clocks = new int[count];
                    int[] tempos = new int[count];
                    int i = -1;
                    bool contains_first_tempo = false;
                    foreach (var item in EditorManager.itemSelection.getTempoIterator()) {
                        int clock = item.getKey();
                        i++;
                        clocks[i] = clock;
                        if (clock == 0) {
                            contains_first_tempo = true;
                            break;
                        }
                        TempoTableEntry editing = EditorManager.itemSelection.getTempo(clock).editing;
                        new_clocks[i] = editing.Clock;
                        tempos[i] = editing.Tempo;
                    }
                    if (contains_first_tempo) {
                        SystemSounds.Asterisk.Play();
                    } else {
                        CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandUpdateTempoRange(clocks, new_clocks, tempos));
                        EditorManager.editHistory.register(vsq.executeCommand(run));
                        setEdited(true);
                    }
                } else if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.TIMESIG) {
                    int count = EditorManager.itemSelection.getTimesigCount();
                    int[] barcounts = new int[count];
                    int[] new_barcounts = new int[count];
                    int[] numerators = new int[count];
                    int[] denominators = new int[count];
                    int i = -1;
                    bool contains_first_bar = false;
                    foreach (var item in EditorManager.itemSelection.getTimesigIterator()) {
                        int bar = item.getKey();
                        i++;
                        barcounts[i] = bar;
                        if (bar == 0) {
                            contains_first_bar = true;
                            break;
                        }
                        TimeSigTableEntry editing = EditorManager.itemSelection.getTimesig(bar).editing;
                        new_barcounts[i] = editing.BarCount;
                        numerators[i] = editing.Numerator;
                        denominators[i] = editing.Denominator;
                    }
                    if (contains_first_bar) {
                        SystemSounds.Asterisk.Play();
                    } else {
                        CadenciiCommand run = new CadenciiCommand(
                            VsqCommand.generateCommandUpdateTimesigRange(barcounts, new_barcounts, numerators, denominators));
                        EditorManager.editHistory.register(vsq.executeCommand(run));
                        setEdited(true);
                    }
                }
            }
            mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.NONE;
            pictPianoRoll.Refresh();
            picturePositionIndicator.Refresh();
        }

        public void picturePositionIndicator_MouseMove(Object sender, NMouseEventArgs e)
        {
            VsqFileEx vsq = MusicManager.getVsqFile();
            if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.TEMPO) {
                int clock = EditorManager.clockFromXCoord(e.X) - mTempoDraggingDeltaClock;
                int step = EditorManager.getPositionQuantizeClock();
                clock = FormMainModel.Quantize(clock, step);
                int last_clock = EditorManager.itemSelection.getLastTempoClock();
                int dclock = clock - last_clock;
                foreach (var item in EditorManager.itemSelection.getTempoIterator()) {
                    int key = item.getKey();
                    EditorManager.itemSelection.getTempo(key).editing.Clock = EditorManager.itemSelection.getTempo(key).original.Clock + dclock;
                }
                picturePositionIndicator.Refresh();
            } else if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.TIMESIG) {
                int clock = EditorManager.clockFromXCoord(e.X) - mTimesigDraggingDeltaClock;
                int barcount = vsq.getBarCountFromClock(clock);
                int last_barcount = EditorManager.itemSelection.getLastTimesigBarcount();
                int dbarcount = barcount - last_barcount;
                foreach (var item in EditorManager.itemSelection.getTimesigIterator()) {
                    int bar = item.getKey();
                    EditorManager.itemSelection.getTimesig(bar).editing.BarCount = EditorManager.itemSelection.getTimesig(bar).original.BarCount + dbarcount;
                }
                picturePositionIndicator.Refresh();
            } else if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.MARK_START) {
                int clock = EditorManager.clockFromXCoord(e.X);
                int unit = EditorManager.getPositionQuantizeClock();
                clock = FormMainModel.Quantize(clock, unit);
                if (clock < 0) {
                    clock = 0;
                }
                int draft_start = Math.Min(clock, vsq.config.EndMarker);
                int draft_end = Math.Max(clock, vsq.config.EndMarker);
                if (draft_start != vsq.config.StartMarker) {
                    vsq.config.StartMarker = draft_start;
                    setEdited(true);
                }
                if (draft_end != vsq.config.EndMarker) {
                    vsq.config.EndMarker = draft_end;
                    setEdited(true);
                }
                refreshScreen();
            } else if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.MARK_END) {
                int clock = EditorManager.clockFromXCoord(e.X);
                int unit = EditorManager.getPositionQuantizeClock();
                clock = FormMainModel.Quantize(clock, unit);
                if (clock < 0) {
                    clock = 0;
                }
                int draft_start = Math.Min(clock, vsq.config.StartMarker);
                int draft_end = Math.Max(clock, vsq.config.StartMarker);
                if (draft_start != vsq.config.StartMarker) {
                    vsq.config.StartMarker = draft_start;
                    setEdited(true);
                }
                if (draft_end != vsq.config.EndMarker) {
                    vsq.config.EndMarker = draft_end;
                    setEdited(true);
                }
                refreshScreen();
            }
        }

        public void picturePositionIndicator_Paint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            picturePositionIndicatorDrawTo(g);
#if MONITOR_FPS
            g.setColor(Cadencii.Gui.Colors.Red);
            g.setFont(cadencii.core.EditorConfig.baseFont10Bold);
            g.drawString(PortUtil.formatDecimal("#.00", mFps) + " / " + PortUtil.formatDecimal("#.00", mFps2), 5, 5);
#endif
        }

        public void picturePositionIndicator_PreviewKeyDown(Object sender, NKeyEventArgs e)
        {
            processSpecialShortcutKey(e, true);
        }
        #endregion

        //BOOKMARK: trackBar
        #region trackBar
        public void trackBar_Enter(Object sender, EventArgs e)
        {
            focusPianoRoll();
        }

        public void trackBar_ValueChanged(Object sender, EventArgs e)
        {
            controller.setScaleX(getScaleXFromTrackBarValue(trackBar.Value));
            controller.setStartToDrawX(calculateStartToDrawX());
            updateDrawObjectList();
            Refresh();
        }
        #endregion

        //BOOKMARK: trackSelector
        #region trackSelector
        public void trackSelector_CommandExecuted(Object sender, EventArgs e)
        {
            setEdited(true);
            refreshScreen();
        }

        public void trackSelector_MouseClick(Object sender, NMouseEventArgs e)
        {
            if (e.Button == NMouseButtons.Right) {
                if (EditorManager.keyWidth < e.X && e.X < trackSelector.Width) {
                    if (trackSelector.Height - TrackSelectorConsts.OFFSET_TRACK_TAB <= e.Y && e.Y <= trackSelector.Height) {
                        cMenuTrackTab.Show(trackSelector, e.X, e.Y);
                    } else {
                        cMenuTrackSelector.Show(trackSelector, e.X, e.Y);
                    }
                }
            }
        }

        public void trackSelector_MouseDoubleClick(Object sender, NMouseEventArgs e)
        {
            if (e.Button == NMouseButtons.Middle) {
                // ツールをポインター <--> 鉛筆に切り替える
                if (EditorManager.keyWidth < e.X &&
                     e.Y < trackSelector.Height - TrackSelectorConsts.OFFSET_TRACK_TAB * 2) {
                    if (EditorManager.SelectedTool == EditTool.ARROW) {
                        EditorManager.SelectedTool = (EditTool.PENCIL);
                    } else {
                        EditorManager.SelectedTool = (EditTool.ARROW);
                    }
                }
            }
        }

        public void trackSelector_MouseDown(Object sender, NMouseEventArgs e)
        {
            if (EditorManager.keyWidth < e.X) {
                mMouseDownedTrackSelector = true;
                if (isMouseMiddleButtonDowned(e.Button)) {
                    mEditCurveMode = CurveEditMode.MIDDLE_DRAG;
                    mButtonInitial = new Point(e.X, e.Y);
                    mMiddleButtonHScroll = hScroll.Value;
                    this.Cursor = HAND;
                }
            }
        }

        public void trackSelector_MouseMove(Object sender, NMouseEventArgs e)
        {
            if (mFormActivated && EditorManager.InputTextBox != null) {
                bool input_visible = !EditorManager.InputTextBox.IsDisposed && EditorManager.InputTextBox.Visible;
#if ENABLE_PROPERTY
                bool prop_editing = EditorManager.propertyPanel.isEditing();
#else
                bool prop_editing = false;
#endif
                if (!input_visible && !prop_editing) {
                    trackSelector.requestFocus();
                }
            }
            if (e.Button == NMouseButtons.None) {
                if (!timer.Enabled) {
                    refreshScreen(true);
                }
                return;
            }
            int parent_width = ((TrackSelector)sender).Width;
            if (mEditCurveMode == CurveEditMode.MIDDLE_DRAG) {
                if (EditorManager.isPlaying()) {
                    return;
                }

                int draft = computeHScrollValueForMiddleDrag(e.X);
                if (hScroll.Value != draft) {
                    hScroll.Value = draft;
                }
            } else {
                if (mMouseDownedTrackSelector) {
                    if (mExtDragXTrackSelector == ExtDragXMode.NONE) {
                        if (EditorManager.keyWidth > e.X) {
                            mExtDragXTrackSelector = ExtDragXMode.LEFT;
                        } else if (parent_width < e.X) {
                            mExtDragXTrackSelector = ExtDragXMode.RIGHT;
                        }
                    } else {
                        if (EditorManager.keyWidth <= e.X && e.X <= parent_width) {
                            mExtDragXTrackSelector = ExtDragXMode.NONE;
                        }
                    }
                } else {
                    mExtDragXTrackSelector = ExtDragXMode.NONE;
                }

                if (mExtDragXTrackSelector != ExtDragXMode.NONE) {
                    double now = PortUtil.getCurrentTime();
                    double dt = now - mTimerDragLastIgnitted;
                    mTimerDragLastIgnitted = now;
                    int px_move = EditorManager.editorConfig.MouseDragIncrement;
                    if (px_move / dt > EditorManager.editorConfig.MouseDragMaximumRate) {
                        px_move = (int)(dt * EditorManager.editorConfig.MouseDragMaximumRate);
                    }
                    px_move += 5;
                    if (mExtDragXTrackSelector == ExtDragXMode.LEFT) {
                        px_move *= -1;
                    }
                    double d_draft = hScroll.Value + px_move * controller.getScaleXInv();
                    if (d_draft < 0.0) {
                        d_draft = 0.0;
                    }
                    int draft = (int)d_draft;
                    if (hScroll.Maximum < draft) {
                        hScroll.Maximum = draft;
                    }
                    if (draft < hScroll.Minimum) {
                        draft = hScroll.Minimum;
                    }
                    hScroll.Value = draft;
                }
            }
            if (!timer.Enabled) {
                refreshScreen(true);
            }
        }

        public void trackSelector_MouseUp(Object sender, NMouseEventArgs e)
        {
            mMouseDownedTrackSelector = false;
            if (mEditCurveMode == CurveEditMode.MIDDLE_DRAG) {
                mEditCurveMode = CurveEditMode.NONE;
                this.Cursor = Cursors.Default;
            }
        }

        public void trackSelector_MouseWheel(Object sender, NMouseEventArgs e)
        {
#if DEBUG
            sout.println("FormMain#trackSelector_MouseWheel");
#endif
            if (((Keys) Control.ModifierKeys & Keys.Shift) == Keys.Shift) {
                double new_val = (double)vScroll.Value - e.Delta;
                int max = vScroll.Maximum - vScroll.Minimum;
                int min = vScroll.Minimum;
                if (new_val > max) {
                    vScroll.Value = max;
                } else if (new_val < min) {
                    vScroll.Value = min;
                } else {
                    vScroll.Value = (int)new_val;
                }
            } else {
                hScroll.Value = computeScrollValueFromWheelDelta(e.Delta);
            }
            refreshScreen();
        }

        public void trackSelector_PreferredMinHeightChanged(Object sender, EventArgs e)
        {
            if (menuVisualControlTrack.Checked) {
                splitContainer1.Panel2MinSize = (trackSelector.getPreferredMinSize());
#if DEBUG
                sout.println("FormMain#trackSelector_PreferredMinHeightChanged; splitContainer1.Panel2MinSize changed");
#endif
            }
        }

        public void trackSelector_PreviewKeyDown(Object sender, NKeyEventArgs e)
        {
            var e0 = new NKeyEventArgs(e.KeyData);
            processSpecialShortcutKey(e0, true);
        }

        public void trackSelector_RenderRequired(Object sender, int track)
        {
            List<int> list = new List<int>();
            list.Add(track);
            EditorManager.patchWorkToFreeze(this, list);
            /*int selected = EditorManager.Selected;
            Vector<Integer> t = new Vector<Integer>( Arrays.asList( PortUtil.convertIntArray( tracks ) ) );
            if ( t.contains( selected) ) {
                String file = fsys.combine( ApplicationGlobal.getTempWaveDir(), selected + ".wav" );
                if ( PortUtil.isFileExists( file ) ) {
                    Thread loadwave_thread = new Thread( new ParameterizedThreadStart( this.loadWave ) );
                    loadwave_thread.IsBackground = true;
                    loadwave_thread.Start( new Object[]{ file, selected - 1 } );
                }
            }*/
        }

        public void trackSelector_SelectedCurveChanged(Object sender, CurveType type)
        {
            refreshScreen();
        }

        public void trackSelector_SelectedTrackChanged(Object sender, int selected)
        {
            EditorManager.itemSelection.clearBezier();
            EditorManager.itemSelection.clearEvent();
            EditorManager.itemSelection.clearPoint();
            updateDrawObjectList();
            refreshScreen();
        }
        #endregion

        //BOOKMARK: cMenuTrackTab
        #region cMenuTrackTab

        public void cMenuTrackTab_Opening()
        {
#if DEBUG
            sout.println("FormMain#cMenuTrackTab_Opening");
#endif
            updateTrackMenuStatus();
        }

        public void updateTrackMenuStatus()
        {
            VsqFileEx vsq = MusicManager.getVsqFile();
            int selected = EditorManager.Selected;
            VsqTrack vsq_track = vsq.Track[selected];
            int tracks = vsq.Track.Count;
            cMenuTrackTabDelete.Enabled = tracks >= 3;
            menuTrackDelete.Enabled = tracks >= 3;
            cMenuTrackTabAdd.Enabled = tracks <= ApplicationGlobal.MAX_NUM_TRACK;
            menuTrackAdd.Enabled = tracks <= ApplicationGlobal.MAX_NUM_TRACK;
            cMenuTrackTabCopy.Enabled = tracks <= ApplicationGlobal.MAX_NUM_TRACK;
            menuTrackCopy.Enabled = tracks <= ApplicationGlobal.MAX_NUM_TRACK;

            bool on = vsq_track.isTrackOn();
            cMenuTrackTabTrackOn.Checked = on;
            menuTrackOn.Checked = on;

            if (tracks > 2) {
                cMenuTrackTabOverlay.Enabled = true;
                menuTrackOverlay.Enabled = true;
                cMenuTrackTabOverlay.Checked = EditorManager.TrackOverlay;
                menuTrackOverlay.Checked = EditorManager.TrackOverlay;
            } else {
                cMenuTrackTabOverlay.Enabled = false;
                menuTrackOverlay.Enabled = false;
                cMenuTrackTabOverlay.Checked = false;
                menuTrackOverlay.Checked = false;
            }
            cMenuTrackTabRenderCurrent.Enabled = !EditorManager.isPlaying();
            menuTrackRenderCurrent.Enabled = !EditorManager.isPlaying();
            cMenuTrackTabRenderAll.Enabled = !EditorManager.isPlaying();
            menuTrackRenderAll.Enabled = !EditorManager.isPlaying();

            var kind = VsqFileEx.getTrackRendererKind(vsq_track);
			model.RendererMenuHandlers.ForEach((handler) => handler.updateCheckedState(kind));
        }

        public void cMenuTrackTabOverlay_Click(Object sender, EventArgs e)
        {
            EditorManager.TrackOverlay = (!EditorManager.TrackOverlay);
            refreshScreen();
        }

        public void cMenuTrackTabRenderCurrent_Click(Object sender, EventArgs e)
        {
            List<int> tracks = new List<int>();
            tracks.Add(EditorManager.Selected);
			EditorManager.patchWorkToFreeze(this, tracks);
        }

        #endregion

        #region cPotisionIndicator
        public void cMenuPositionIndicatorStartMarker_Click(Object sender, EventArgs e)
        {
            int clock = mPositionIndicatorPopupShownClock;
            VsqFileEx vsq = MusicManager.getVsqFile();
            vsq.config.StartMarkerEnabled = true;
            vsq.config.StartMarker = clock;
            if (vsq.config.EndMarker < clock) {
                vsq.config.EndMarker = clock;
            }
            menuVisualStartMarker.Checked = true;
            setEdited(true);
            picturePositionIndicator.Refresh();
        }

        public void cMenuPositionIndicatorEndMarker_Click(Object sender, EventArgs e)
        {
            int clock = mPositionIndicatorPopupShownClock;
            VsqFileEx vsq = MusicManager.getVsqFile();
            vsq.config.EndMarkerEnabled = true;
            vsq.config.EndMarker = clock;
            if (vsq.config.StartMarker > clock) {
                vsq.config.StartMarker = clock;
            }
            menuVisualEndMarker.Checked = true;
            setEdited(true);
            picturePositionIndicator.Refresh();
        }
        #endregion
        #region buttonVZoom & buttonVMooz
        public void buttonVZoom_Click(Object sender, EventArgs e)
        {
            zoomY(1);
        }

        public void buttonVMooz_Click(Object sender, EventArgs e)
        {
            zoomY(-1);
        }
        #endregion

        #region pictureBox2
        public void pictureBox2_Paint(Object sender, PaintEventArgs e)
        {
            if (mGraphicsPictureBox2 == null) {
                mGraphicsPictureBox2 = new Graphics();
            }
            mGraphicsPictureBox2.NativeGraphics = e.Graphics.NativeGraphics;
            int width = pictureBox2.Width;
            int height = pictureBox2.Height;
            int unit_height = height / 4;
            mGraphicsPictureBox2.setColor(FormMainModel.ColorR214G214B214);
            mGraphicsPictureBox2.fillRect(0, 0, width, height);
            if (mPianoRollScaleYMouseStatus > 0) {
                mGraphicsPictureBox2.setColor(Cadencii.Gui.Colors.Gray);
                mGraphicsPictureBox2.fillRect(0, 0, width, unit_height);
            } else if (mPianoRollScaleYMouseStatus < 0) {
                mGraphicsPictureBox2.setColor(Cadencii.Gui.Colors.Gray);
                mGraphicsPictureBox2.fillRect(0, unit_height * 2, width, unit_height);
            }
            mGraphicsPictureBox2.setStroke(getStrokeDefault());
            mGraphicsPictureBox2.setColor(Cadencii.Gui.Colors.Gray);
            //mGraphicsPictureBox2.drawRect( 0, 0, width - 1, unit_height * 2 );
            mGraphicsPictureBox2.drawLine(0, unit_height, width, unit_height);
            mGraphicsPictureBox2.drawLine(0, unit_height * 2, width, unit_height * 2);
            mGraphicsPictureBox2.setStroke(getStroke2px());
            int cx = width / 2;
            int cy = unit_height / 2;
            mGraphicsPictureBox2.setColor((mPianoRollScaleYMouseStatus > 0) ? Cadencii.Gui.Colors.LightGray : Cadencii.Gui.Colors.Gray);
            mGraphicsPictureBox2.drawLine(cx - 4, cy, cx + 4, cy);
            mGraphicsPictureBox2.drawLine(cx, cy - 4, cx, cy + 4);
            cy += unit_height * 2;
            mGraphicsPictureBox2.setColor((mPianoRollScaleYMouseStatus < 0) ? Cadencii.Gui.Colors.LightGray : Cadencii.Gui.Colors.Gray);
            mGraphicsPictureBox2.drawLine(cx - 4, cy, cx + 4, cy);
        }

        public void pictureBox2_MouseDown(Object sender, NMouseEventArgs e)
        {
            // 拡大・縮小ボタンが押されたかどうか判定
            int height = pictureBox2.Height;
            int width = pictureBox2.Width;
            int height4 = height / 4;
            if (0 <= e.X && e.X < width) {
                int scaley = EditorManager.editorConfig.PianoRollScaleY;
                if (0 <= e.Y && e.Y < height4) {
                    if (scaley + 1 <= EditorConfig.MAX_PIANOROLL_SCALEY) {
                        zoomY(1);
                        mPianoRollScaleYMouseStatus = 1;
                    } else {
                        mPianoRollScaleYMouseStatus = 0;
                    }
                } else if (height4 * 2 <= e.Y && e.Y < height4 * 3) {
                    if (scaley - 1 >= EditorConfig.MIN_PIANOROLL_SCALEY) {
                        zoomY(-1);
                        mPianoRollScaleYMouseStatus = -1;
                    } else {
                        mPianoRollScaleYMouseStatus = 0;
                    }
                } else {
                    mPianoRollScaleYMouseStatus = 0;
                }
            } else {
                mPianoRollScaleYMouseStatus = 0;
            }
            refreshScreen();
        }

        public void pictureBox2_MouseUp(Object sender, NMouseEventArgs e)
        {
            mPianoRollScaleYMouseStatus = 0;
            pictureBox2.Invalidate();
        }
        #endregion

        //BOOKMARK: stripBtn
        #region stripBtn*
        public void stripBtnGrid_Click(Object sender, EventArgs e)
        {
            bool new_v = !EditorManager.isGridVisible();
            stripBtnGrid.Pushed = new_v;
            EditorManager.setGridVisible(new_v);
        }

        public void stripBtnArrow_Click(Object sender, EventArgs e)
        {
            EditorManager.SelectedTool = (EditTool.ARROW);
        }

        public void stripBtnPencil_Click(Object sender, EventArgs e)
        {
            EditorManager.SelectedTool = (EditTool.PENCIL);
        }

        public void stripBtnLine_Click(Object sender, EventArgs e)
        {
            EditorManager.SelectedTool = (EditTool.LINE);
        }

        public void stripBtnEraser_Click(Object sender, EventArgs e)
        {
            EditorManager.SelectedTool = (EditTool.ERASER);
        }

        public void stripBtnCurve_Click(Object sender, EventArgs e)
        {
            EditorManager.setCurveMode(!EditorManager.isCurveMode());
        }

        public void stripBtnPlay_Click(Object sender, EventArgs e)
        {
            EditorManager.setPlaying(!EditorManager.isPlaying(), this);
            focusPianoRoll();
        }

        public void stripBtnScroll_CheckedChanged(Object sender, EventArgs e)
        {
            bool pushed = stripBtnScroll.Pushed;
            EditorManager.mAutoScroll = pushed;
#if DEBUG
            sout.println("FormMain#stripBtnScroll_CheckedChanged; pushed=" + pushed);
#endif
            focusPianoRoll();
        }

        public void stripBtnLoop_CheckedChanged(Object sender, EventArgs e)
        {
            bool pushed = stripBtnLoop.Pushed;
            EditorManager.IsPreviewRepeatMode = pushed;
            focusPianoRoll();
        }

        public void stripBtnStepSequencer_CheckedChanged(Object sender, EventArgs e)
        {
            // EditorManager.mAddingEventがnullかどうかで処理が変わるのでnullにする
            EditorManager.mAddingEvent = null;
            // モードを切り替える
            controller.setStepSequencerEnabled(stripBtnStepSequencer.Checked);

            // MIDIの受信を開始
#if ENABLE_MIDI
            if (controller.isStepSequencerEnabled()) {
                mMidiIn.start();
            } else {
                mMidiIn.stop();
            }
#endif
        }

        public void stripBtnStop_Click(Object sender, EventArgs e)
        {
            EditorManager.setPlaying(false, this);
            timer.Stop();
            focusPianoRoll();
        }

        public void stripBtnMoveEnd_Click(Object sender, EventArgs e)
        {
            if (EditorManager.isPlaying()) {
                EditorManager.setPlaying(false, this);
            }
            EditorManager.setCurrentClock(MusicManager.getVsqFile().TotalClocks);
            ensureCursorVisible();
            refreshScreen();
        }

        public void stripBtnMoveTop_Click(Object sender, EventArgs e)
        {
            if (EditorManager.isPlaying()) {
                EditorManager.setPlaying(false, this);
            }
            EditorManager.setCurrentClock(0);
            ensureCursorVisible();
            refreshScreen();
        }

        public void stripBtnRewind_Click(Object sender, EventArgs e)
        {
            rewind();
        }

        public void stripBtnForward_Click(Object sender, EventArgs e)
        {
            forward();
        }
        #endregion

        //BOOKMARK: pictKeyLengthSplitter
        #region pictKeyLengthSplitter
        public void pictKeyLengthSplitter_MouseDown(Object sender, NMouseEventArgs e)
        {
            mKeyLengthSplitterMouseDowned = true;
			mKeyLengthSplitterInitialMouse = cadencii.core2.PortUtil.getMousePosition();
            mKeyLengthInitValue = EditorManager.keyWidth;
            mKeyLengthTrackSelectorRowsPerColumn = trackSelector.getRowsPerColumn();
            mKeyLengthSplitterDistance = splitContainer1.DividerLocation;
        }

        public void pictKeyLengthSplitter_MouseMove(Object sender, NMouseEventArgs e)
        {
            if (!mKeyLengthSplitterMouseDowned) {
                return;
            }
			int dx = cadencii.core2.PortUtil.getMousePosition().X - mKeyLengthSplitterInitialMouse.X;
            int draft = mKeyLengthInitValue + dx;
            if (draft < EditorConfig.MIN_KEY_WIDTH) {
                draft = EditorConfig.MIN_KEY_WIDTH;
			} else if (EditorConfig.MAX_KEY_WIDTH < draft) {
				draft = EditorConfig.MAX_KEY_WIDTH;
            }
            EditorManager.keyWidth = draft;
            int current = trackSelector.getRowsPerColumn();
            if (current >= mKeyLengthTrackSelectorRowsPerColumn) {
                int max_divider_location = splitContainer1.Height - splitContainer1.DividerSize - splitContainer1.Panel2MinSize;
                if (max_divider_location < mKeyLengthSplitterDistance) {
                    splitContainer1.DividerLocation = (max_divider_location);
                } else {
                    splitContainer1.DividerLocation = (mKeyLengthSplitterDistance);
                }
            }
            updateLayout();
            refreshScreen();
        }

        public void pictKeyLengthSplitter_MouseUp(Object sender, NMouseEventArgs e)
        {
            mKeyLengthSplitterMouseDowned = false;
        }
        #endregion

        #region toolBarMeasure
        void toolBarMeasure_MouseDown(Object sender, NMouseEventArgs e)
        {
            // マウス位置にあるボタンを捜す
            UiToolBarButton c = null;
            foreach (UiToolBarButton btn in toolBarMeasure.Buttons) {
                var rc = btn.Rectangle;
                if (Utility.isInRect(e.X, e.Y, rc.Left, rc.Top, rc.Width, rc.Height)) {
                    c = btn;
                    break;
                }
            }
            if (c == null) {
                return;
            }

            if (c == stripDDBtnQuantizeParent) {
                var rc = stripDDBtnQuantizeParent.Rectangle;
                stripDDBtnQuantize.Show(
                    toolBarMeasure,
                    new Cadencii.Gui.Point(rc.Left, rc.Bottom));
            }
        }

        void toolBarMeasure_ButtonClick(Object sender, ToolBarButtonClickEventArgs e)
        {
            if (e.Button == stripBtnStartMarker) {
				model.VisualMenu.RunStartMarkerCommand ();
            } else if (e.Button == stripBtnEndMarker) {
				model.VisualMenu.RunEndMarkerCommand ();
            }/* else if ( e.Button == stripDDBtnLengthParent ) {
                System.Drawing.Rectangle rc = stripDDBtnLengthParent.Rectangle;
                stripDDBtnLength.Show(
                    toolBarMeasure,
                    new System.Drawing.Point( rc.Left, rc.Bottom ) );
            } else if ( e.Button == stripDDBtnQuantizeParent ) {
                System.Drawing.Rectangle rc = stripDDBtnQuantizeParent.Rectangle;
                stripDDBtnQuantize.Show(
                    toolBarMeasure,
                    new System.Drawing.Point( rc.Left, rc.Bottom ) );
            }*/
        }
        #endregion

        void toolBarTool_ButtonClick(Object sender, ToolBarButtonClickEventArgs e)
        {
            if (e.Button == stripBtnPointer) {
                stripBtnArrow_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnPencil) {
                stripBtnPencil_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnLine) {
                stripBtnLine_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnEraser) {
                stripBtnEraser_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnGrid) {
                stripBtnGrid_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnCurve) {
                stripBtnCurve_Click(e.Button, new EventArgs());
            } else {
                handleStripPaletteTool_Click(e.Button, new EventArgs());
            }
        }

        void toolBarPosition_ButtonClick(Object sender, ToolBarButtonClickEventArgs e)
        {
            if (e.Button == stripBtnMoveTop) {
                stripBtnMoveTop_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnRewind) {
                stripBtnRewind_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnForward) {
                stripBtnForward_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnMoveEnd) {
                stripBtnMoveEnd_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnPlay) {
                stripBtnPlay_Click(e.Button, new EventArgs());
                //} else if ( e.Button == stripBtnStop ) {
                //stripBtnStop_Click( e.Button, new EventArgs() );
            } else if (e.Button == stripBtnScroll) {
                stripBtnScroll.Pushed = !stripBtnScroll.Pushed;
                stripBtnScroll_CheckedChanged(e.Button, new EventArgs());
            } else if (e.Button == stripBtnLoop) {
                stripBtnLoop.Pushed = !stripBtnLoop.Pushed;
                stripBtnLoop_CheckedChanged(e.Button, new EventArgs());
            }
        }

        void toolBarFile_ButtonClick(Object sender, ToolBarButtonClickEventArgs e)
		{
			if (e.Button == stripBtnFileNew) {
				model.FileMenu.RunFileNewCommand ();
			} else if (e.Button == stripBtnFileOpen) {
				model.FileMenu.RunFileOpenCommand ();
			} else if (e.Button == stripBtnFileSave) {
				model.FileMenu.RunFileSaveCommand ();
			} else if (e.Button == stripBtnCut) {
				model.Cut ();
			} else if (e.Button == stripBtnCopy) {
				model.Copy ();
			} else if (e.Button == stripBtnPaste) {
				model.Paste ();
			} else if (e.Button == stripBtnUndo) {
				model.EditMenu.RunEditUndoCommand ();
			} else if (e.Button == stripBtnRedo) {
				model.EditMenu.RunEditRedoCommand ();
			}
		}

        public void handleVibratoPresetSubelementClick(Object sender, EventArgs e)
        {
            if (sender == null) {
                return;
            }
            if (!(sender is UiToolStripMenuItem)) {
                return;
            }

            // イベントの送信元を特定
            UiToolStripMenuItem item = (UiToolStripMenuItem)sender;
            string text = item.Text;

            // メニューの表示文字列から，どの設定値についてのイベントかを探す
            VibratoHandle target = null;
            int size = EditorManager.editorConfig.AutoVibratoCustom.Count;
            for (int i = 0; i < size; i++) {
                VibratoHandle handle = EditorManager.editorConfig.AutoVibratoCustom[i];
                if (text.Equals(handle.getCaption())) {
                    target = handle;
                    break;
                }
            }

            // ターゲットが特定できなかったらbailout
            if (target == null) {
                return;
            }

            // 選択状態のアイテムを取得
            IEnumerable<SelectedEventEntry> itr = EditorManager.itemSelection.getEventIterator();
            if (itr.Count() == 0) {
                // アイテムがないのでbailout
                return;
            }
            VsqEvent ev = itr.First().original;
            if (ev.ID.VibratoHandle == null) {
                return;
            }

            // 設定値にコピー
            VibratoHandle h = ev.ID.VibratoHandle;
            target.setStartRate(h.getStartRate());
            target.setStartDepth(h.getStartDepth());
            if (h.getRateBP() == null) {
                target.setRateBP(null);
            } else {
                target.setRateBP((VibratoBPList)h.getRateBP().clone());
            }
            if (h.getDepthBP() == null) {
                target.setDepthBP(null);
            } else {
                target.setDepthBP((VibratoBPList)h.getDepthBP().clone());
            }
        }

        public void timer_Tick(Object sender, EventArgs e)
        {
            if (EditorManager.isGeneratorRunning()) {
                MonitorWaveReceiver monitor = MonitorWaveReceiver.getInstance();
                double play_time = 0.0;
                if (monitor != null) {
                    play_time = monitor.getPlayTime();
                }
                double now = play_time + EditorManager.mDirectPlayShift;
                int clock = (int)MusicManager.getVsqFile().getClockFromSec(now);
                if (mLastClock <= clock) {
                    mLastClock = clock;
                    EditorManager.setCurrentClock(clock);
                    if (EditorManager.mAutoScroll) {
                        ensureCursorVisible();
                    }
                }
            } else {
                EditorManager.setPlaying(false, this);
                int ending_clock = EditorManager.getPreviewEndingClock();
                EditorManager.setCurrentClock(ending_clock);
                if (EditorManager.mAutoScroll) {
                    ensureCursorVisible();
                }
                refreshScreen(true);
                if (EditorManager.IsPreviewRepeatMode) {
                    int dest_clock = 0;
                    VsqFileEx vsq = MusicManager.getVsqFile();
                    if (vsq.config.StartMarkerEnabled) {
                        dest_clock = vsq.config.StartMarker;
                    }
                    EditorManager.setCurrentClock(dest_clock);
                    EditorManager.setPlaying(true, this);
                }
            }
            refreshScreen();
        }

        public void bgWorkScreen_DoWork(Object sender, DoWorkEventArgs e)
        {
            try {
                this.Invoke(new EventHandler(this.refreshScreenCore));
            } catch (Exception ex) {
                serr.println("FormMain#bgWorkScreen_DoWork; ex=" + ex);
                Logger.write(typeof(FormMain) + ".bgWorkScreen_DoWork; ex=" + ex + "\n");
            }
        }

        public void toolStripEdit_Resize(Object sender, EventArgs e)
        {
            saveToolbarLocation();
        }

        public void toolStripPosition_Resize(Object sender, EventArgs e)
        {
            saveToolbarLocation();
        }

        public void toolStripMeasure_Resize(Object sender, EventArgs e)
        {
            saveToolbarLocation();
        }

        void toolStripFile_Resize(Object sender, EventArgs e)
        {
            saveToolbarLocation();
        }

        public void toolStripContainer_TopToolStripPanel_SizeChanged(Object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) {
                return;
            }
            Dimension minsize = getWindowMinimumSize();
            int wid = this.Width;
            int hei = this.Height;
            bool change_size_required = false;
            if (minsize.Width > wid) {
                wid = minsize.Width;
                change_size_required = true;
            }
            if (minsize.Height > hei) {
                hei = minsize.Height;
                change_size_required = true;
            }
            var min_size = getWindowMinimumSize();
            this.MinimumSize = new System.Drawing.Size(min_size.Width, min_size.Height);
            if (change_size_required) {
                this.Size = new System.Drawing.Size(wid, hei);
            }
        }

        public void handleStripPaletteTool_Click(Object sender, EventArgs e)
        {
            string id = "";  //選択されたツールのID
#if ENABLE_SCRIPT
            if (sender is UiToolBarButton) {
                UiToolBarButton tsb = (UiToolBarButton)sender;
                if (tsb.Tag != null && tsb.Tag is string) {
                    id = (string)tsb.Tag;
                    EditorManager.mSelectedPaletteTool = id;
                    EditorManager.SelectedTool = (EditTool.PALETTE_TOOL);
                    tsb.Pushed = true;
                }
            } else if (sender is ToolStripMenuItem) {
                ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
                if (tsmi.Tag != null && tsmi.Tag is string) {
                    id = (string)tsmi.Tag;
                    EditorManager.mSelectedPaletteTool = id;
                    EditorManager.SelectedTool = (EditTool.PALETTE_TOOL);
                    tsmi.Checked = true;
                }
            }
#endif

            int count = toolBarTool.Buttons.Count;
            for (int i = 0; i < count; i++) {
                Object item = toolBarTool.Buttons[i];
                if (item is UiToolBarButton) {
                    UiToolBarButton button = (UiToolBarButton)item;
                    if (button.Style == Cadencii.Gui.ToolBarButtonStyle.ToggleButton && button.Tag != null && button.Tag is string) {
                        if (((string)button.Tag).Equals(id)) {
                            button.Pushed = true;
                        } else {
                            button.Pushed = false;
                        }
                    }
                }
            }

            foreach (var item in cMenuPianoPaletteTool.DropDownItems) {
                if (item is PaletteToolMenuItem) {
                    PaletteToolMenuItem menu = (PaletteToolMenuItem)item;
                    string tagged_id = menu.getPaletteToolID();
                    menu.Checked = (id == tagged_id);
                }
            }

            //MenuElement[] sub_cmenu_track_selectro_palette_tool = cMenuTrackSelectorPaletteTool.getSubElements();
            //for ( int i = 0; i < sub_cmenu_track_selectro_palette_tool.Length; i++ ) {
            //MenuElement item = sub_cmenu_track_selectro_palette_tool[i];
            foreach (var item in cMenuTrackSelectorPaletteTool.DropDownItems) {
                if (item is PaletteToolMenuItem) {
                    PaletteToolMenuItem menu = (PaletteToolMenuItem)item;
                    string tagged_id = menu.getPaletteToolID();
                    menu.Checked = (id == tagged_id);
                }
            }
        }

        public void handleEditorConfig_QuantizeModeChanged(Object sender, EventArgs e)
        {
            applyQuantizeMode();
        }

        public void handleStripButton_Enter(Object sender, EventArgs e)
        {
            focusPianoRoll();
        }

#if ENABLE_MOUSE_ENTER_STATUS_LABEL
        /// <summary>
        /// メニューの説明をステータスバーに表示するための共通のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void handleMenuMouseEnter(Object sender, EventArgs e)
        {
            if (sender == null) {
                return;
            }

            bool notfound = false;
            string text = "";
            if (sender == menuEditUndo) {
                text = _("Undo.");
            } else if (sender == menuEditRedo) {
                text = _("Redo.");
            } else if (sender == menuEditCut) {
                text = _("Cut selected items.");
            } else if (sender == menuEditCopy) {
                text = _("Copy selected items.");
            } else if (sender == menuEditPaste) {
                text = _("Paste copied items to current song position.");
            } else if (sender == menuEditDelete) {
                text = _("Delete selected items.");
            } else if (sender == menuEditAutoNormalizeMode) {
                text = _("Avoid automaticaly polyphonic editing.");
            } else if (sender == menuEditSelectAll) {
                text = _("Select all items and control curves of current track.");
            } else if (sender == menuEditSelectAllEvents) {
                text = _("Select all items of current track.");
            } else if (sender == menuVisualControlTrack) {
                text = _("Show/Hide control curves.");
            } else if (sender == menuVisualEndMarker) {
                text = _("Enable/Disable end marker.");
            } else if (sender == menuVisualGridline) {
                text = _("Show/Hide grid line.");
            } else if (sender == menuVisualIconPalette) {
                text = _("Show/Hide icon palette");
            } else if (sender == menuVisualLyrics) {
                text = _("Show/Hide lyrics.");
            } else if (sender == menuVisualMixer) {
                text = _("Show/Hide mixer window.");
            } else if (sender == menuVisualNoteProperty) {
                text = _("Show/Hide expression lines.");
            } else if (sender == menuVisualOverview) {
                text = _("Show/Hide navigation view");
            } else if (sender == menuVisualPitchLine) {
                text = _("Show/Hide pitch bend lines.");
            } else if (sender == menuVisualPluginUi) {
                text = _("Open VSTi plugin window");
            } else if (sender == menuVisualProperty) {
                text = _("Show/Hide property window.");
            } else if (sender == menuVisualStartMarker) {
                text = _("Enable/Disable start marker.");
            } else if (sender == menuVisualWaveform) {
                text = _("Show/Hide waveform.");
            } else if (sender == menuFileNew) {
                text = _("Create new project.");
            } else if (sender == menuFileOpen) {
                text = _("Open Cadencii project.");
            } else if (sender == menuFileSave) {
                text = _("Save current project.");
            } else if (sender == menuFileSaveNamed) {
                text = _("Save current project with new name.");
            } else if (sender == menuFileOpenVsq) {
                text = _("Open VSQ / VOCALOID MIDI and create new project.");
            } else if (sender == menuFileOpenUst) {
                text = _("Open UTAU project and create new project.");
            } else if (sender == menuFileImport) {
                text = _("Import.");
            } else if (sender == menuFileImportMidi) {
                text = _("Import Standard MIDI.");
            } else if (sender == menuFileImportUst) {
                text = _("Import UTAU project");
            } else if (sender == menuFileImportVsq) {
                text = _("Import VSQ / VOCALOID MIDI.");
            } else if (sender == menuFileExport) {
                text = _("Export.");
            } else if (sender == menuFileExportParaWave) {
                text = _("Export all tracks to serial numbered WAVEs");
            } else if (sender == menuFileExportWave) {
                text = _("Export to WAVE file.");
            } else if (sender == menuFileExportMusicXml) {
                text = _("Export current track as Music XML");
            } else if (sender == menuFileExportMidi) {
                text = _("Export to Standard MIDI.");
            } else if (sender == menuFileExportUst) {
                text = _("Export current track as UTAU project file");
            } else if (sender == menuFileExportVsq) {
                text = _("Export to VSQ");
            } else if (sender == menuFileExportVsqx) {
                text = _("Export to VSQX");
            } else if (sender == menuFileExportVxt) {
                text = _("Export current track as Meta-text for vConnect");
            } else if (sender == menuFileRecent) {
                text = _("Recent projects.");
            } else if (sender == menuFileQuit) {
                text = _("Close this window.");
            } else if (sender == menuJobConnect) {
                text = _("Lengthen note end to neighboring note.");
            } else if (sender == menuJobLyric) {
                text = _("Import lyric.");
            } else if (sender == menuJobRewire) {
                text = _("Import tempo from ReWire host.") + _("(not implemented)");
            } else if (sender == menuJobReloadVsti) {
                text = _("Reload VSTi dll.") + _("(not implemented)");
            } else if (sender == menuJobNormalize) {
                text = _("Correct overlapped item.");
            } else if (sender == menuJobInsertBar) {
                text = _("Insert bar.");
            } else if (sender == menuJobDeleteBar) {
                text = _("Delete bar.");
            } else if (sender == menuJobRandomize) {
                text = _("Randomize items.");
            } else if (sender == menuLyricExpressionProperty) {
                text = _("Edit portamento/accent/decay of selected item");
            } else if (sender == menuLyricVibratoProperty) {
                text = _("Edit vibrato length and type of selected item");
            } else if (sender == menuLyricPhonemeTransformation) {
                text = _("Translate all phrase into phonetic symbol");
            } else if (sender == menuLyricDictionary) {
                text = _("Open configuration dialog for phonetic symnol dictionaries");
            } else if (sender == menuLyricCopyVibratoToPreset) {
                text = _("Copy vibrato config of selected item into vibrato preset");
            } else if (sender == menuScriptUpdate) {
                text = _("Read and compile all scripts and update the list of them");
            } else if (sender == menuSettingPreference) {
                text = _("Open configuration dialog for editor configs");
            } else if (sender == menuSettingPositionQuantize) {
                text = _("Change quantize resolution");
            } else if (sender == menuSettingGameControler) {
                text = _("Connect/Remove/Configure game controler");
            } else if (sender == menuSettingPaletteTool) {
                text = _("Configuration of palette tool");
            } else if (sender == menuSettingShortcut) {
                text = _("Open configuration dialog for shortcut key");
            } else if (sender == menuSettingVibratoPreset) {
                text = _("Open configuration dialog for vibrato preset");
            } else if (sender == menuSettingDefaultSingerStyle) {
                text = _("Edit default singer style");
            } else if (sender == menuSettingSequence) {
                text = _("Configuration of this sequence.");
            } else if (sender == menuTrackAdd) {
                text = _("Add new track.");
            } else if (sender == menuTrackBgm) {
                text = _("Add/Remove/Edit background music");
            } else if (sender == menuTrackOn) {
                text = _("Enable current track.");
            } else if (sender == menuTrackCopy) {
                text = _("Copy current track.");
            } else if (sender == menuTrackChangeName) {
                text = _("Change track name.");
            } else if (sender == menuTrackDelete) {
                text = _("Delete current track.");
            } else if (sender == menuTrackRenderCurrent) {
                text = _("Render current track.");
            } else if (sender == menuTrackRenderAll) {
                text = _("Render all tracks.");
            } else if (sender == menuTrackOverlay) {
                text = _("Show background items.");
            } else if (sender == menuTrackRenderer) {
                text = _("Select voice synthesis engine.");
            } else if (sender == menuTrackRendererAquesTone) {
                text = _("AquesTone");
            } else if (sender == menuTrackRendererUtau) {
                text = _("UTAU");
            } else if (sender == menuTrackRendererVCNT) {
                text = _("vConnect-STAND");
            } else if (sender == menuTrackRendererVOCALOID1) {
                text = _("VOCALOID1");
            } else if (sender == menuTrackRendererVOCALOID2) {
                text = _("VOCALOID2");
            } else if (sender == menuFileRecentClear) {
                text = _("Clear menu items");
            } else {
                notfound = true;
            }

#if DEBUG
            if (notfound && sender is ToolStripMenuItem) {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                Logger.write(typeof(FormMain) + ".handleMenuMouseEnter; cannot find message for " + item.Name + "\n");
            }
#endif
            statusLabel.Text = text;
        }
#endif

        public void handleSpaceKeyDown(Object sender, KeyEventArgs e)
        {
            if (((Keys) e.KeyCode & Keys.Space) == Keys.Space) {
                mSpacekeyDowned = true;
            }
        }

        public void handleSpaceKeyUp(Object sender, KeyEventArgs e)
        {
            if (((Keys) e.KeyCode & Keys.Space) == Keys.Space) {
                mSpacekeyDowned = false;
            }
        }
		
        void handleSpaceKeyDown(Object sender, NKeyEventArgs e)
        {
            if (((Keys) e.KeyCode & Keys.Space) == Keys.Space) {
                mSpacekeyDowned = true;
            }
        }

        void handleSpaceKeyUp(Object sender, NKeyEventArgs e)
        {
            if (((Keys) e.KeyCode & Keys.Space) == Keys.Space) {
                mSpacekeyDowned = false;
            }
        }

        public void handleBgmOffsetSeconds_Click(Object sender, EventArgs e)
        {
            if (!(sender is BgmMenuItem)) {
                return;
            }
            BgmMenuItem menu = (BgmMenuItem)sender;
            int index = menu.getBgmIndex();
            InputBox ib = null;
            try {
                ib = ApplicationUIHost.Create<InputBox>(_("Input Offset Seconds"));
                ib.Location = model.GetFormPreferedLocation(ib);
                ib.setResult(MusicManager.getBgm(index).readOffsetSeconds + "");
                var dr = DialogManager.showModalDialog(ib, this);
                if (dr != 1) {
                    return;
                }
                List<BgmFile> list = new List<BgmFile>();
                int count = MusicManager.getBgmCount();
                BgmFile item = null;
                for (int i = 0; i < count; i++) {
                    if (i == index) {
                        item = (BgmFile)MusicManager.getBgm(i).clone();
                        list.Add(item);
                    } else {
                        list.Add(MusicManager.getBgm(i));
                    }
                }
                double draft;
                try {
                    draft = double.Parse(ib.getResult());
                    item.readOffsetSeconds = draft;
                    menu.ToolTipText = draft + " " + _("seconds");
                } catch (Exception ex3) {
                    Logger.write(typeof(FormMain) + ".handleBgmOffsetSeconds_Click; ex=" + ex3 + "\n");
                }
                CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate(list);
                EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
                setEdited(true);
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".handleBgmOffsetSeconds_Click; ex=" + ex + "\n");
            } finally {
                if (ib != null) {
                    try {
                        ib.Dispose();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".handleBgmOffsetSeconds_Click; ex=" + ex2 + "\n");
                    }
                }
            }
        }

        public void handleBgmStartAfterPremeasure_CheckedChanged(Object sender, EventArgs e)
        {
            if (!(sender is BgmMenuItem)) {
                return;
            }
            BgmMenuItem menu = (BgmMenuItem)sender;
            int index = menu.getBgmIndex();
            List<BgmFile> list = new List<BgmFile>();
            int count = MusicManager.getBgmCount();
            for (int i = 0; i < count; i++) {
                if (i == index) {
                    BgmFile item = (BgmFile)MusicManager.getBgm(i).clone();
                    item.startAfterPremeasure = menu.Checked;
                    list.Add(item);
                } else {
                    list.Add(MusicManager.getBgm(i));
                }
            }
            CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate(list);
            EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
            setEdited(true);
        }

        public void handleBgmAdd_Click(Object sender, EventArgs e)
        {
            string dir = ApplicationGlobal.appConfig.getLastUsedPathIn("wav");
            openWaveDialog.SetSelectedFile(dir);
            var ret = DialogManager.showModalFileDialog(openWaveDialog, true, this);
            if (ret != Cadencii.Gui.DialogResult.OK) {
                return;
            }

            string file = openWaveDialog.FileName;
            ApplicationGlobal.appConfig.setLastUsedPathIn(file, ".wav");

            // 既に開かれていたらキャンセル
            int count = MusicManager.getBgmCount();
            bool found = false;
            for (int i = 0; i < count; i++) {
                BgmFile item = MusicManager.getBgm(i);
                if (file == item.file) {
                    found = true;
                    break;
                }
            }
            if (found) {
                DialogManager.showMessageBox(
                    PortUtil.formatMessage(_("file '{0}' is already registered as BGM."), file),
                    _("Error"),
                    cadencii.Dialog.MSGBOX_DEFAULT_OPTION,
                    cadencii.Dialog.MSGBOX_WARNING_MESSAGE);
                return;
            }

            // 登録
            EditorManager.addBgm(file);
            setEdited(true);
            updateBgmMenuState();
        }

        public void handleBgmRemove_Click(Object sender, EventArgs e)
        {
            if (!(sender is BgmMenuItem)) {
                return;
            }
            BgmMenuItem parent = (BgmMenuItem)sender;
            int index = parent.getBgmIndex();
            BgmFile bgm = MusicManager.getBgm(index);
            if (DialogManager.showMessageBox(PortUtil.formatMessage(_("remove '{0}'?"), bgm.file),
                                  "Cadencii",
                                  cadencii.Dialog.MSGBOX_YES_NO_OPTION,
                                  cadencii.Dialog.MSGBOX_QUESTION_MESSAGE) != Cadencii.Gui.DialogResult.Yes) {
                return;
            }
            EditorManager.removeBgm(index);
            setEdited(true);
            updateBgmMenuState();
        }

        public void handleSettingPaletteTool(Object sender, EventArgs e)
        {
#if ENABLE_SCRIPT
            if (!(sender is PaletteToolMenuItem)) {
                return;
            }
            PaletteToolMenuItem tsmi = (PaletteToolMenuItem)sender;
            string id = tsmi.getPaletteToolID();
            if (!PaletteToolServer.loadedTools.ContainsKey(id)) {
                return;
            }
            Object instance = PaletteToolServer.loadedTools[id];
            IPaletteTool ipt = (IPaletteTool)instance;
            if (ipt.openDialog() == Cadencii.Gui.DialogResult.OK) {
                XmlSerializer xsms = new XmlSerializer(instance.GetType(), true);
                string dir = Path.Combine(ApplicationGlobal.getApplicationDataPath(), "tool");
                if (!Directory.Exists(dir)) {
                    PortUtil.createDirectory(dir);
                }
                string cfg = id + ".config";
                string config = Path.Combine(dir, cfg);
                FileStream fs = null;
                try {
                    fs = new FileStream(config, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    xsms.serialize(fs, null);
                } catch (Exception ex) {
                    Logger.write(typeof(FormMain) + ".handleSettingPaletteTool; ex=" + ex + "\n");
                } finally {
                    if (fs != null) {
                        try {
                            fs.Close();
                        } catch (Exception ex2) {
                            Logger.write(typeof(FormMain) + ".handleSettingPaletteTool; ex=" + ex2 + "\n");
                        }
                    }
                }
            }
#endif
        }

#if ENABLE_MTC
        /// <summary>
        /// MTC用のMIDI-INデバイスからMIDIを受信します。
        /// </summary>
        /// <param name="now"></param>
        /// <param name="dataArray"></param>
        private void handleMtcMidiReceived( double now, byte[] dataArray ) {
            byte data = (byte)(dataArray[1] & 0x0f);
            byte type = (byte)((dataArray[1] >> 4) & 0x0f);
            if ( type == 0 ) {
                mtcFrameLsb = data;
            } else if ( type == 1 ) {
                mtcFrameMsb = data;
            } else if ( type == 2 ) {
                mtcSecLsb = data;
            } else if ( type == 3 ) {
                mtcSecMsb = data;
            } else if ( type == 4 ) {
                mtcMinLsb = data;
            } else if ( type == 5 ) {
                mtcMinMsb = data;
            } else if ( type == 6 ) {
                mtcHourLsb = data;
            } else if ( type == 7 ) {
                mtcHourMsb = (byte)(data & 1);
                int fpsType = (data & 6) >> 1;
                double fps = 30.0;
                if ( fpsType == 0 ) {
                    fps = 24.0;
                } else if ( fpsType == 1 ) {
                    fps = 25;
                } else if ( fpsType == 2 ) {
                    fps = 30000.0 / 1001.0;
                } else if ( fpsType == 3 ) {
                    fps = 30.0;
                }
                int hour = (mtcHourMsb << 4 | mtcHourLsb);
                int min = (mtcMinMsb << 4 | mtcMinLsb);
                int sec = (mtcSecMsb << 4 | mtcSecLsb);
                int frame = (mtcFrameMsb << 4 | mtcFrameLsb) + 2;
                double time = (hour * 60.0 + min) * 60.0 + sec + frame / fps;
                mtcLastReceived = now;
#if DEBUG
                int clock = (int)MusicManager.getVsqFile().getClockFromSec( time );
                EditorManager.setCurrentClock( clock );
#endif
                /*if ( !EditorManager.isPlaying() ) {
                    EditorManager.EditMode = EditMode.REALTIME_MTC );
                    EditorManager.setPlaying( true );
                    EventHandler handler = new EventHandler( EditorManager_PreviewStarted );
                    if ( handler != null ) {
                        this.Invoke( handler );
                        while ( VSTiProxy.getPlayTime() <= 0.0 ) {
                            System.Windows.Forms.Application.DoEvents();
                        }
                        EditorManager.setPlaying( true );
                    }
                }*/
#if DEBUG
                sout.println( "FormMain#handleMtcMidiReceived; time=" + time );
#endif
            }
        }
#endif

#if ENABLE_MIDI
        public void mMidiIn_MidiReceived(Object sender, javax.sound.midi.MidiMessage message)
        {
            byte[] data = message.getMessage();
#if DEBUG
            sout.println("FormMain#mMidiIn_MidiReceived; data.Length=" + data.Length);
#endif
            if (data.Length <= 2) {
                return;
            }
#if DEBUG
            sout.println("FormMain#mMidiIn_MidiReceived; EditorManager.isPlaying()=" + EditorManager.isPlaying());
#endif
            if (EditorManager.isPlaying()) {
                return;
            }
#if DEBUG
            sout.println("FormMain#mMidiIn_MidiReceived; isStepSequencerEnabeld()=" + controller.isStepSequencerEnabled());
#endif
            if (false == controller.isStepSequencerEnabled()) {
                return;
            }
            int code = data[0] & 0xf0;
            if (code != 0x80 && code != 0x90) {
                return;
            }
            if (code == 0x90 && data[2] == 0x00) {
                code = 0x80;//ベロシティ0のNoteOnはNoteOff
            }

            int note = (0xff & data[1]);

            int clock = EditorManager.getCurrentClock();
            int unit = EditorManager.getPositionQuantizeClock();
            if (unit > 1) {
                clock = FormMainModel.Quantize(clock, unit);
            }

#if DEBUG
            sout.println("FormMain#mMidiIn_Received; clock=" + clock + "; note=" + note);
#endif
            if (code == 0x80) {
                /*if ( EditorManager.mAddingEvent != null ) {
                    int len = clock - EditorManager.mAddingEvent.Clock;
                    if ( len <= 0 ) {
                        len = unit;
                    }
                    EditorManager.mAddingEvent.ID.Length = len;
                    int selected = EditorManager.Selected;
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventAdd( selected,
                                                                                                   EditorManager.mAddingEvent ) );
                    EditorManager.register( MusicManager.getVsqFile().executeCommand( run ) );
                    if ( !isEdited() ) {
                        setEdited( true );
                    }
                    updateDrawObjectList();
                }*/
            } else if (code == 0x90) {
                if (EditorManager.mAddingEvent != null) {
                    // mAddingEventがnullでない場合は打ち込みの試行中(未確定の音符がある)
                    // であるので，ノートだけが変わるようにする
                    clock = EditorManager.mAddingEvent.Clock;
                } else {
                    EditorManager.mAddingEvent = new VsqEvent();
                }
                EditorManager.mAddingEvent.Clock = clock;
                if (EditorManager.mAddingEvent.ID == null) {
                    EditorManager.mAddingEvent.ID = new VsqID();
                }
                EditorManager.mAddingEvent.ID.type = VsqIDType.Anote;
                EditorManager.mAddingEvent.ID.Dynamics = 64;
                EditorManager.mAddingEvent.ID.VibratoHandle = null;
                if (EditorManager.mAddingEvent.ID.LyricHandle == null) {
                    EditorManager.mAddingEvent.ID.LyricHandle = new LyricHandle("a", "a");
                }
                EditorManager.mAddingEvent.ID.LyricHandle.L0.Phrase = "a";
                EditorManager.mAddingEvent.ID.LyricHandle.L0.setPhoneticSymbol("a");
                EditorManager.mAddingEvent.ID.Note = note;

                // 音符の長さを計算
                int length = QuantizeModeUtil.getQuantizeClock(
                        EditorManager.editorConfig.getLengthQuantize(),
                        EditorManager.editorConfig.isLengthQuantizeTriplet());

                // 音符の長さを設定
	EditorManager.editLengthOfVsqEvent(
                    EditorManager.mAddingEvent,
                    length,
		EditorManager.vibratoLengthEditingRule);

                // 現在位置は，音符の末尾になる
                EditorManager.setCurrentClock(clock + length);

                // 画面を再描画
                if (this.InvokeRequired) {
                    DelegateRefreshScreen deleg = null;
                    try {
                        deleg = new DelegateRefreshScreen(refreshScreen);
                    } catch (Exception ex4) {
                        deleg = null;
                    }
                    if (deleg != null) {
                        this.Invoke(deleg, true);
                    }
                } else {
                    refreshScreen(true);
                }
                // 鍵盤音を鳴らす
                KeySoundPlayer.play(note);
            }
        }
#endif
        #endregion

        #region public static methods
        /// <summary>
        /// 文字列を、現在の言語設定に従って翻訳します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        /// <summary>
        /// スクリーンに対して、ウィンドウの最適な位置を取得する
        /// </summary>
        public static Point getAppropriateDialogLocation(int x, int y, int width, int height, int workingAreaX, int workingAreaY, int workingAreaWidth, int workingAreaHeight)
        {
            int top = y;
            if (top + height > workingAreaY + workingAreaHeight) {
                // ダイアログの下端が隠れる場合、位置をずらす
                top = workingAreaY + workingAreaHeight - height;
            }
            if (top < workingAreaY) {
                // ダイアログの上端が隠れる場合、位置をずらす
                top = workingAreaY;
            }
            int left = x;
            if (left + width > workingAreaX + workingAreaWidth) {
                left = workingAreaX + workingAreaWidth - width;
            }
            if (left < workingAreaX) {
                left = workingAreaX;
            }
            return new Point(left, top);
        }
        #endregion

        private void menuToolsCreateVConnectSTANDDb_Click(object sender, EventArgs e)
        {
            string creator = Path.Combine(System.Windows.Forms.Application.StartupPath, "vConnectStandDBConvert.exe");
            if (System.IO.File.Exists(creator)) {
                Process.Start(creator);
            }
        }

        private void menuHelpCheckForUpdates_Click(object sender, EventArgs args)
        {
            showUpdateInformationAsync(true);
        }
    }

}
