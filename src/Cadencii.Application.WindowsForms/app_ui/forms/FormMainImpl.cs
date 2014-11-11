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
using System.Linq;
using System.IO;
using System.Net;
using System.ComponentModel;
using cadencii.apputil;
using Cadencii.Gui;
using cadencii.java.util;
using Cadencii.Media.Vsq;
using Cadencii.Xml;
using cadencii.utau;
using cadencii.ui;

using Consts = cadencii.FormMainModel.Consts;
using cadencii.core;
using Cadencii.Utilities;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
    /// <summary>
    /// エディタのメイン画面クラス
    /// </summary>
	public partial class FormMainImpl : FormImpl, FormMain, PropertyWindowListener
    {
		FormMainModel model;

		FormMainModel PropertyWindowListener.Model {
			get { return model; }
		}

		FormMainModel FormMain.Model {
			get { return model; }
		}

		UiContextMenuStrip FormMain.MenuTrackTab {
			get { return cMenuTrackTab; }
			set { cMenuTrackTab = value; }
		}

		UiContextMenuStrip FormMain.MenuTrackSelector {
			get { return cMenuTrackSelector; }
			set { cMenuTrackSelector = value; }
		}

		TrackSelector FormMain.TrackSelector {
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
        /// refreshScreenが呼ばれている最中かどうか
        /// </summary>
        private bool mIsRefreshing = false;

        #region fields
        string appId;

		public VersionInfo mVersionInfo { get; set; }
        /// <summary>
        /// ボタンがDownされた位置。(座標はpictureBox基準)
        /// </summary>
		public Point mButtonInitial { get; set; }
        /// <summary>
        /// 真ん中ボタンがダウンされたときのvscrollのvalue値
        /// </summary>
		public int mMiddleButtonVScroll { get; set; }
        /// <summary>
        /// 真ん中ボタンがダウンされたときのhscrollのvalue値
        /// </summary>
		public int mMiddleButtonHScroll { get; set; }
		public bool mEdited { get; set; }
        /// <summary>
        /// 最後にメイン画面が更新された時刻(秒単位)
        /// </summary>
        private double mLastScreenRefreshedSec;
        /// <summary>
        /// カーブエディタの編集モード
        /// </summary>
		public CurveEditMode mEditCurveMode { get; set; }
        /// <summary>
        /// ピアノロールの画面外へのドラッグ時、前回自動スクロール操作を行った時刻
        /// </summary>
		public double mTimerDragLastIgnitted { get; set; }
        /// <summary>
        /// マウスが降りているかどうかを表すフラグ．EditorManager.isPointerDownedとは別なので注意
        /// </summary>
		public bool mMouseDowned { get; set; }
        /// <summary>
        /// 鉛筆のモード
        /// </summary>
		public PencilMode mPencilMode { get; set; }
        /// <summary>
        /// このフォームがアクティブ化されているかどうか
        /// </summary>
		public bool mFormActivated { get; set; }
#if ENABLE_MTC
        public MidiInDevice m_midi_in_mtc = null;
#endif
		public FormMidiImExport mDialogMidiImportAndExport { get; set; }
        SortedDictionary<EditTool, Cadencii.Gui.Cursor> mCursor = new SortedDictionary<EditTool, Cadencii.Gui.Cursor>();
		public Preference mDialogPreference { get; set; }
#if ENABLE_PROPERTY
        PropertyPanelContainer mPropertyPanelContainer;
#endif
#if ENABLE_SCRIPT
        List<UiToolBarButton> mPaletteTools = new List<UiToolBarButton>();
#endif

		public UiOpenFileDialog openXmlVsqDialog { get; set; }
		public UiSaveFileDialog saveXmlVsqDialog { get; set; }
		public UiOpenFileDialog openUstDialog { get; set; }
		public UiOpenFileDialog openMidiDialog { get; set; }
		public UiSaveFileDialog saveMidiDialog { get; set; }
		public UiOpenFileDialog openWaveDialog { get; set; }
		public System.ComponentModel.BackgroundWorker bgWorkScreen { get; set; }
        /// <summary>
        /// 特殊な取り扱いが必要なショートカットのキー列と、対応するメニューアイテムを保存しておくリスト。
        /// </summary>
        List<SpecialShortcutHolder> mSpecialShortcutHolders = new List<SpecialShortcutHolder>();
        /// <summary>
        /// 歌詞流し込み用のダイアログ
        /// </summary>
		public FormImportLyric mDialogImportLyric { get; set; }
        /// <summary>
        /// pictureBox2の描画ループで使うグラフィックス
        /// </summary>
		public Graphics mGraphicsPictureBox2 { get;set; }
#if MONITOR_FPS
        /// <summary>
        /// パフォーマンスカウンタ
        /// </summary>
        double[] mFpsDrawTime = new double[128];
        int mFpsDrawTimeIndex = 0;
        /// <summary>
        /// パフォーマンスカウンタから算出される画面の更新速度
        /// </summary>
		public float mFps { get; set; }
        double[] mFpsDrawTime2 = new double[128];
		public float mFps2 { get; set; }
#endif
        #endregion

        #region constructor
        public FormMainImpl(string file)
        {
			EditorManager.MainWindow = this;
			model = new FormMainModel (this);

		this.appId = Handle.ToString ("X32");
			mButtonInitial = new Point();
			mEditCurveMode = CurveEditMode.NONE;
			mPencilMode = new PencilMode();
			mFormActivated = true;

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
			EditorConfig.baseFont10Bold = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.BOLD, EditorConfig.FONT_SIZE10);
			EditorConfig.baseFont8 = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.PLAIN, EditorConfig.FONT_SIZE8);
			EditorConfig.baseFont10 = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.PLAIN, EditorConfig.FONT_SIZE10);
			EditorConfig.baseFont9 = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.PLAIN, EditorConfig.FONT_SIZE9);
			EditorConfig.baseFont50Bold = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.BOLD, EditorConfig.FONT_SIZE50);

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

			// call this after "components" is set.
			model.InitializeTimer (components);

            panelOverview.setMainForm(this);
            pictPianoRoll.setMainForm(this);
            bgWorkScreen = new System.ComponentModel.BackgroundWorker();

			this.panelWaveformZoom.AddControl (this.waveView);
			this.waveView.Anchor = ((Cadencii.Gui.Toolkit.AnchorStyles)((((Cadencii.Gui.Toolkit.AnchorStyles.Top | Cadencii.Gui.Toolkit.AnchorStyles.Bottom)
				| Cadencii.Gui.Toolkit.AnchorStyles.Left)
                        | Cadencii.Gui.Toolkit.AnchorStyles.Right)));
            this.waveView.BackColor = new Cadencii.Gui.Color(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.waveView.Location = new Cadencii.Gui.Point(66, 0);
            this.waveView.Margin = new Cadencii.Gui.Toolkit.Padding(0);
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
            this.Icon = Properties.Resources.Icon1;

#if !ENABLE_SCRIPT
            menuSettingPaletteTool.setVisible( false );
            menuScript.setVisible( false );
#endif
            trackSelector.updateVisibleCurves();
            trackSelector.setBackground(new Color(108, 108, 108));
            trackSelector.setCurveVisible(true);
            trackSelector.setSelectedCurve(CurveType.VEL);
            trackSelector.setLocation(new Point(0, 242));
            trackSelector.Margin = new Cadencii.Gui.Toolkit.Padding(0);
            trackSelector.Name = "trackSelector";
            trackSelector.setSize(446, 250);
            trackSelector.TabIndex = 0;

            splitContainer1.Panel2MinSize = trackSelector.getPreferredMinSize();
            var minimum_size = getWindowMinimumSize();
			this.AsAwt ().MinimumSize = new Dimension(minimum_size.Width, minimum_size.Height);
            stripBtnScroll.Pushed = EditorManager.mAutoScroll;

            applySelectedTool();
            applyQuantizeMode();

            // Palette Toolの読み込み
#if ENABLE_SCRIPT
            updatePaletteTool();
#endif

            splitContainer1.Panel1.BorderStyle = Cadencii.Gui.Toolkit.BorderStyle.None;
            splitContainer1.Panel2.BorderStyle = Cadencii.Gui.Toolkit.BorderStyle.None;
            splitContainer1.BackColor = new Cadencii.Gui.Color(212, 212, 212);
            splitContainer2.Panel1.AddControl(panel1);
            panel1.Dock = Cadencii.Gui.Toolkit.DockStyle.Fill;
            splitContainer2.Panel2.AddControl(panelWaveformZoom);
            splitContainer2.Panel2.BorderStyle = BorderStyle.FixedSingle;
            splitContainer2.Panel2.BorderColor = new Cadencii.Gui.Color(112, 112, 112);
            splitContainer1.Panel1.AddControl(splitContainer2);
            panelWaveformZoom.Dock = Cadencii.Gui.Toolkit.DockStyle.Fill;
			splitContainer2.Dock = Cadencii.Gui.Toolkit.DockStyle.Fill;
            splitContainer1.Panel2.AddControl(trackSelector);
            trackSelector.Dock = Cadencii.Gui.Toolkit.DockStyle.Fill;
			splitContainer1.Dock = Cadencii.Gui.Toolkit.DockStyle.Fill;
            splitContainer1.Panel2MinSize = trackSelector.getPreferredMinSize();
            splitContainerProperty.FixedPanel = Cadencii.Gui.Toolkit.FixedPanel.Panel1;

#if ENABLE_PROPERTY
            splitContainerProperty.Panel1.AddControl(mPropertyPanelContainer);
            mPropertyPanelContainer.Dock = Cadencii.Gui.Toolkit.DockStyle.Fill;
#else
            splitContainerProperty.setDividerLocation( 0 );
            splitContainerProperty.setEnabled( false );
            menuVisualProperty.setVisible( false );
#endif

            splitContainerProperty.Panel2.AddControl(splitContainer1);
            splitContainerProperty.Dock = Cadencii.Gui.Toolkit.DockStyle.Fill;

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

			picturePositionIndicator.MouseWheel += (o,e) => model.PositionIndicator.RunMouseWheel (e);

			menuVisualOverview.CheckedChanged += (o, e) => model.VisualMenu.RunVisualOverviewCheckedChanged ();

            hScroll.Maximum = MusicManager.getVsqFile().TotalClocks + 240;
            hScroll.LargeChange = 240 * 4;

            vScroll.Maximum = (int)(model.ScaleY * 100 * 128);
            vScroll.LargeChange = 24 * 4;
            hScroll.SmallChange = 240;
            vScroll.SmallChange = 24;

            trackSelector.setCurveVisible(true);

            // inputTextBoxの初期化
            EditorManager.InputTextBox = new LyricTextBoxImpl();
            EditorManager.InputTextBox.Visible = false;
            EditorManager.InputTextBox.BorderStyle = Cadencii.Gui.Toolkit.BorderStyle.None;
            EditorManager.InputTextBox.Width = 80;
            EditorManager.InputTextBox.AcceptsReturn = true;
            EditorManager.InputTextBox.BackColor = Colors.White;
			EditorManager.InputTextBox.Font = new Font (EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.PLAIN, EditorConfig.FONT_SIZE9);
            EditorManager.InputTextBox.Enabled = false;
            EditorManager.InputTextBox.KeyPress += model.InputTextBox.mInputTextBox_KeyPress;
            EditorManager.InputTextBox.Parent = pictPianoRoll;
            panel1.AddControl(EditorManager.InputTextBox);

#if DEBUG
            menuHelpDebug.Visible = true;
#endif // DEBUG

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
                            Logger.write(typeof(FormMainImpl) + ".ctor; ex=" + ex + "\n");
                            Logger.StdErr("FormMain#.ctor; ex=" + ex);
                        }
                    }
                }
            }

            trackBar.Value = EditorManager.editorConfig.DefaultXScale;
            EditorManager.setCurrentClock(0);
            setEdited(false);

			EditorManager.PreviewStarted += (o, e) => model.StartPreview ();
			EditorManager.PreviewAborted += (o, e) => model.AbortPreview ();
			EditorManager.GridVisibleChanged += model.EditorManagerCommands.EditorManager_GridVisibleChanged;
			EditorManager.itemSelection.SelectedEventChanged += model.EditorManagerCommands.ItemSelectionModel_SelectedEventChanged;
			EditorManager.SelectedToolChanged += model.EditorManagerCommands.EditorManager_SelectedToolChanged;
			EditorManager.UpdateBgmStatusRequired += model.EditorManagerCommands.EditorManager_UpdateBgmStatusRequired;
			EditorManager.MainWindowFocusRequired = model.EditorManagerCommands.EditorManager_MainWindowFocusRequired;
			EditorManager.EditedStateChanged += model.EditorManagerCommands.EditorManager_EditedStateChanged;
			EditorManager.WaveViewReloadRequired += model.EditorManagerCommands.EditorManager_WaveViewRealoadRequired;
			AppConfig.QuantizeModeChanged += (o, e) => applyQuantizeMode ();

#if ENABLE_PROPERTY
			mPropertyPanelContainer.StateChangeRequired += (o, state) => model.OtherItems.mPropertyPanelContainer_StateChangeRequired (state);
#endif

            model.UpdateRecentFileMenu();

            // C3が画面中央に来るように調整
            int draft_start_to_draw_y = 68 * (int)(100 * model.ScaleY) - pictPianoRoll.Height / 2;
            int draft_vscroll_value = (int)((draft_start_to_draw_y * (double)vScroll.Maximum) / (128 * (int)(100 * model.ScaleY) - vScroll.Height));
            try {
                vScroll.Value = draft_vscroll_value;
            } catch (Exception ex) {
                Logger.write(typeof(FormMainImpl) + ".FormMain_Load; ex=" + ex + "\n");
            }

            // x=97がプリメジャークロックになるように調整
            int cp = MusicManager.getVsqFile().getPreMeasureClocks();
            int draft_hscroll_value = (int)(cp - 24.0 * model.ScaleXInv);
            try {
                hScroll.Value = draft_hscroll_value;
            } catch (Exception ex) {
                Logger.write(typeof(FormMainImpl) + ".FormMain_Load; ex=" + ex + "\n");
            }

            //s_pen_dashed_171_171_171.DashPattern = new float[] { 3, 3 };
            //s_pen_dashed_209_204_172.DashPattern = new float[] { 3, 3 };

            menuVisualNoteProperty.Checked = EditorManager.editorConfig.ShowExpLine;
            menuVisualLyrics.Checked = EditorManager.editorConfig.ShowLyric;
            menuVisualMixer.Checked = EditorManager.editorConfig.MixerVisible;
			menuVisualPitchLine.Checked = ApplicationGlobal.appConfig.ViewAtcualPitch;

            updateMenuFonts();

			EditorManager.MixerWindow.FederChanged += (track, feder) => model.OtherItems.mixerWindow_FederChanged (track, feder);
			EditorManager.MixerWindow.PanpotChanged += (track, panpot) => model.OtherItems.mixerWindow_FederChanged (track, panpot);
			EditorManager.MixerWindow.MuteChanged += (track, mute) => model.OtherItems.mixerWindow_MuteChanged (track, mute);
			EditorManager.MixerWindow.SoloChanged += (track, solo) => model.OtherItems.mixerWindow_SoloChanged (track, solo);
            EditorManager.MixerWindow.updateStatus();
            if (EditorManager.editorConfig.MixerVisible) {
                EditorManager.MixerWindow.Visible = true;
            }
			EditorManager.MixerWindow.FormClosing += (o, e) => model.OtherItems.mixerWindow_FormClosing ();

            Point p1 = EditorManager.editorConfig.FormIconPaletteLocation.toPoint();
			if (!Screen.Instance.IsPointInScreens (p1)) {
				Rectangle workingArea = Screen.Instance.GetWorkingArea(this);
                p1 = new Point(workingArea.X, workingArea.Y);
            }
            EditorManager.iconPalette.Location = new Point(p1.X, p1.Y);
            if (EditorManager.editorConfig.IconPaletteVisible) {
                EditorManager.iconPalette.Visible = true;
            }
			EditorManager.iconPalette.FormClosing += (o, e) => model.OtherItems.iconPalette_FormClosing ();
			EditorManager.iconPalette.LocationChanged += (o, e) => model.OtherItems.iconPalette_LocationChanged ();


#if ENABLE_SCRIPT
            model.UpdateScriptShortcut();
            // RunOnceという名前のスクリプトがあれば，そいつを実行
            foreach (var id in ScriptServer.getScriptIdIterator()) {
                if (PortUtil.getFileNameWithoutExtension(id).ToLower() == "runonce") {
                    ScriptServer.invokeScript(id, MusicManager.getVsqFile(), (x1,x2,x3,x4) => DialogManager.ShowMessageBox (x1, x2, x3, x4));
                    break;
                }
            }
#endif

            model.ClearTempWave();
            updateVibratoPresetMenu();
            mPencilMode.setMode(PencilModeEnum.Off);
            updateCMenuPianoFixed();
            model.LoadGameController();
#if ENABLE_MIDI
            model.reloadMidiIn();
#endif
			menuVisualWaveform.Checked = ApplicationGlobal.appConfig.ViewWaveform;

            updateRendererMenu();

            // ウィンドウの位置・サイズを再現
            if (EditorManager.editorConfig.WindowMaximized) {
				this.AsAwt ().WindowState = FormWindowState.Maximized;
            } else {
				this.AsAwt ().WindowState = FormWindowState.Normal;
            }
            Rectangle bounds = EditorManager.editorConfig.WindowRect;
            this.Bounds = new System.Drawing.Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            // ウィンドウ位置・サイズの設定値が、使えるディスプレイのどれにも被っていない場合
			Rectangle rc2 = Screen.Instance.getScreenBounds(this);
            if (bounds.X < rc2.X ||
                 rc2.X + rc2.Width < bounds.X + bounds.Width ||
                 bounds.Y < rc2.Y ||
                 rc2.Y + rc2.Height < bounds.Y + bounds.Height) {
                bounds.X = rc2.X;
                bounds.Y = rc2.Y;
                this.Bounds = new System.Drawing.Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                EditorManager.editorConfig.WindowRect = bounds;
            }
			this.LocationChanged += (o,e) => model.FormMain.RunLocationChanged ();

            updateScrollRangeHorizontal();
            updateScrollRangeVertical();

            // プロパティウィンドウの位置を復元
			Rectangle rc1 = Screen.Instance.getScreenBounds(this);
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
			EditorManager.propertyPanel.CommandExecuteRequired += (o, c) => model.OtherItems.propertyPanel_CommandExecuteRequired (c);
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
            foreach (System.Reflection.FieldInfo fi in typeof(AppConfig).GetFields()) {
                if (fi.IsPublic && !fi.IsStatic) {
                    list.Add(new ValuePair<string, string>(fi.Name, fi.FieldType.ToString()));
                }
            }

            foreach (System.Reflection.PropertyInfo pi in typeof(AppConfig).GetProperties()) {
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
                Logger.write(typeof(FormMainImpl) + ".ctor; ex=" + ex + "\n");
            } finally {
                if (sw != null) {
                    try {
                        sw.Close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMainImpl) + ".ctor; ex=" + ex2 + "\n");
                    }
                }
            }
#endif
        }
        #endregion

        #region helper methods
        /// <summary>
        /// renderer_menu_handler_ を初期化する
        /// </summary>
		public void initializeRendererMenuHandler(FormMainModel model)
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


		public void updateContextMenuPiano (Point mouseAt)
		{
			ByRef<Rectangle> out_id_rect = new ByRef<Rectangle>();
			VsqEvent item = getItemAtClickedPosition(mouseAt, out_id_rect);
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

			int clock = EditorManager.clockFromXCoord(mouseAt.X);
			cMenuPianoPaste.Enabled = ((EditorManager.clipboard.getCopiedItems().events.Count != 0) && (clock >= MusicManager.getVsqFile().getPreMeasureClocks()));
			refreshScreen();
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
						"", null, (o,e) => model.OtherItems.handleVibratoPresetSubelementClick ((UiToolStripMenuItem) o));
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
        /// 現在追加しようとしている音符の内容(EditorManager.mAddingEvent)をfixします
        /// </summary>
        /// <returns></returns>
        public void fixAddingEvent()
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
                model.EnsureNoteVisibleOnPianoRoll(note_max);
            } else if (delta_note < 0) {
                note_min -= 2;
                if (note_min < 0) {
                    note_min = 0;
                }
				model.EnsureNoteVisibleOnPianoRoll(note_min);
            }

            // 音符が見えるようにする。時間方向
            if (delta_clock > 0) {
				model.EnsureClockVisibleOnPianoRoll(clock_max);
            } else if (delta_clock < 0) {
				model.EnsureClockVisibleOnPianoRoll(clock_min);
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
        public VsqEvent getItemAtClickedPosition(Point mouse_position, ByRef<Rectangle> rect)
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
                int start_to_draw_x = model.StartToDrawX;
                int start_to_draw_y = model.StartToDrawY;
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
        public int computeVScrollValueForMiddleDrag(int mouse_y)
        {
            int dy = mouse_y - mButtonInitial.Y;
            int max = vScroll.Maximum - vScroll.LargeChange;
            int min = vScroll.Minimum;
            double new_vscroll_value = (double)mMiddleButtonVScroll - dy * max / (128.0 * (int)(100.0 * model.ScaleY) - (double)pictPianoRoll.Height);
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
        public int computeHScrollValueForMiddleDrag(int mouse_x)
        {
            int dx = mouse_x - mButtonInitial.X;
            int max = hScroll.Maximum - hScroll.LargeChange;
            int min = hScroll.Minimum;
            double new_hscroll_value = (double)mMiddleButtonHScroll - (double)dx * model.ScaleXInv;
            int value = (int)new_hscroll_value;
            if (value < min) {
                value = min;
            } else if (max < value) {
                value = max;
            }
            return value;
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
            return (int)(value * model.ScaleY);
        }

        /// <summary>
        /// Downloads update information xml, and deserialize it.
        /// </summary>
        /// <returns></returns>
        private updater.UpdateInfo downloadUpdateInfo()
        {
            var xml_contents = "";
            try {
                var url = Consts.RECENT_UPDATE_INFO_URL;
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
        public void showUpdateInformationAsync(bool is_explicit_update_check)
        {
			#if true//SUPPORT_UPDATE_FORM
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
			var form = ApplicationUIHost.Create<UpdateCheckForm>();
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
			System.Windows.Forms.MessageBox.Show(_("Cadencii is up to date"),
                                        _("Info"),
			System.Windows.Forms.MessageBoxButtons.OK,
			System.Windows.Forms.MessageBoxIcon.Information);
                    }
                } else if (is_explicit_update_check) {
			System.Windows.Forms.MessageBox.Show(_("Can't get update information. Please retry after few minutes."),
                                    _("Error"),
			System.Windows.Forms.MessageBoxButtons.OK,
			System.Windows.Forms.MessageBoxIcon.Information);
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
			#else
			System.Windows.Forms.MessageBox.Show ("Automatic Update is not supported in this buid");
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
                    refreshScreenCore();

                    mIsRefreshing = false;
                }
            }
#endif
        }

        public void refreshScreen()
        {
            refreshScreen(false);
        }

        public void refreshScreenCore()
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

        public int calculateStartToDrawX()
        {
            return (int)(hScroll.Value * model.ScaleX);
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
					menu_remove.Click += (o,e) => model.OtherItems.handleBgmRemove_Click ((BgmMenuItem) o);
                    menu.DropDownItems.Add(menu_remove);

					BgmMenuItem menu_start_after_premeasure = ApplicationUIHost.Create<BgmMenuItem>(i);
                    menu_start_after_premeasure.Text = _("Start After Premeasure");
                    menu_start_after_premeasure.Name = "menu_start_after_premeasure" + i;
                    menu_start_after_premeasure.CheckOnClick = true;
                    menu_start_after_premeasure.Checked = item.startAfterPremeasure;
					menu_start_after_premeasure.CheckedChanged += (o,e) => model.OtherItems.handleBgmStartAfterPremeasure_CheckedChanged((BgmMenuItem) o);
                    menu.DropDownItems.Add(menu_start_after_premeasure);

					BgmMenuItem menu_offset_second = ApplicationUIHost.Create<BgmMenuItem>(i);
                    menu_offset_second.Text = _("Set Offset Seconds");
                    menu_offset_second.ToolTipText = item.readOffsetSeconds + " " + _("seconds");
					menu_offset_second.Click += (o,e) => model.OtherItems.handleBgmOffsetSeconds_Click((BgmMenuItem) o);
                    menu.DropDownItems.Add(menu_offset_second);

                    menuTrackBgm.DropDownItems.Add(menu);
                }
                menuTrackBgm.DropDownItems.Add(new ToolStripSeparatorImpl());
            }
            var menu_add = new ToolStripMenuItemImpl();
            menu_add.Text = _("Add");
			menu_add.Click += (o, e) => model.OtherItems.handleBgmAdd_Click ();
            menuTrackBgm.DropDownItems.Add(menu_add);
        }


#if ENABLE_PROPERTY
        public void updatePropertyPanelState(PanelState state)
        {
#if DEBUG
            Logger.StdOut("FormMain#updatePropertyPanelState; state=" + state);
#endif
            if (state == PanelState.Docked) {
                mPropertyPanelContainer.addComponent(EditorManager.propertyPanel);
                menuVisualProperty.Checked = true;
                EditorManager.editorConfig.PropertyWindowStatus.State = PanelState.Docked;
                splitContainerProperty.Panel1Hidden = (false);
                splitContainerProperty.SplitterFixed = (false);
				splitContainerProperty.DividerSize = (Consts._SPL_SPLITTER_WIDTH);
				splitContainerProperty.Panel1MinSize = Consts._PROPERTY_DOCK_MIN_WIDTH;
                int w = EditorManager.editorConfig.PropertyWindowStatus.DockWidth;
				if (w < Consts._PROPERTY_DOCK_MIN_WIDTH) {
					w = Consts._PROPERTY_DOCK_MIN_WIDTH;
                }
                splitContainerProperty.DividerLocation = (w);
#if DEBUG
                Logger.StdOut("FormMain#updatePropertyPanelState; state=Docked; w=" + w);
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
                Point appropriateLocation = FormMainModel.GetAppropriateDialogLocation(
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


        public void updateLayout()
        {
            int width = panel1.Width;
            int height = panel1.Height;

            if (EditorManager.editorConfig.OverviewEnabled) {
				panelOverview.Height = Consts._OVERVIEW_HEIGHT;
            } else {
                panelOverview.Height = 0;
            }
            panelOverview.Width = width;
            int key_width = EditorManager.keyWidth;

            /*btnMooz.setBounds( 3, 12, 23, 23 );
            btnZoom.setBounds( 26, 12, 23, 23 );*/

            picturePositionIndicator.Width = width;
			picturePositionIndicator.Height = Consts._PICT_POSITION_INDICATOR_HEIGHT;

            hScroll.Top = 0;
            hScroll.Left = key_width;
			hScroll.Width = width - key_width - Consts._SCROLL_WIDTH - trackBar.Width;
			hScroll.Height = Consts._SCROLL_WIDTH;

			vScroll.Width = Consts._SCROLL_WIDTH;
			vScroll.Height = height - Consts._PICT_POSITION_INDICATOR_HEIGHT - Consts._SCROLL_WIDTH * 4 - panelOverview.Height;

			pictPianoRoll.Width = width - Consts._SCROLL_WIDTH;
			pictPianoRoll.Height = height - Consts._PICT_POSITION_INDICATOR_HEIGHT - Consts._SCROLL_WIDTH - panelOverview.Height;

			pictureBox3.Width = key_width - Consts._SCROLL_WIDTH;
			pictKeyLengthSplitter.Width = Consts._SCROLL_WIDTH;
			pictureBox3.Height = Consts._SCROLL_WIDTH;
			pictureBox2.Height = Consts._SCROLL_WIDTH * 4;
			trackBar.Height = Consts._SCROLL_WIDTH;

            panelOverview.Top = 0;
            panelOverview.Left = 0;

            picturePositionIndicator.Top = panelOverview.Height;
            picturePositionIndicator.Left = 0;

			pictPianoRoll.Top = Consts._PICT_POSITION_INDICATOR_HEIGHT + panelOverview.Height;
            pictPianoRoll.Left = 0;

			vScroll.Top = Consts._PICT_POSITION_INDICATOR_HEIGHT + panelOverview.Height;
			vScroll.Left = width - Consts._SCROLL_WIDTH;

			pictureBox3.Top = height - Consts._SCROLL_WIDTH;
            pictureBox3.Left = 0;
			pictKeyLengthSplitter.Top = height - Consts._SCROLL_WIDTH;
            pictKeyLengthSplitter.Left = pictureBox3.Width;

			hScroll.Top = height - Consts._SCROLL_WIDTH;
            hScroll.Left = pictureBox3.Width + pictKeyLengthSplitter.Width;

			trackBar.Top = height - Consts._SCROLL_WIDTH;
			trackBar.Left = width - Consts._SCROLL_WIDTH - trackBar.Width;

			pictureBox2.Top = height - Consts._SCROLL_WIDTH * 4;
			pictureBox2.Left = width - Consts._SCROLL_WIDTH;

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
            int boxheight = (int)(vibrato.Depth * 2 / 100.0 * (int)(100.0 * model.ScaleY));
            int px_shift = (int)(vibrato.Shift / 100.0 * vibrato.Depth / 100.0 * (int)(100.0 * model.ScaleY));

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
                float px_track_height = (int)(model.ScaleY * 100.0f);
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
                Logger.write(typeof(FormMainImpl) + ".drawUtauVibato; ex=" + oex + "\n");
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
                tsb.Style = Cadencii.Gui.Toolkit.ToolBarButtonStyle.ToggleButton;
                if (icon != null) {
                    imageListTool.Images.Add(icon);
                    tsb.ImageIndex = imageListTool.Images.Count - 1;
                }
                tsb.Text = name;
                tsb.ToolTipText = desc;
                tsb.Tag = id;
                if (first) {
                    UiToolBarButton sep = new ToolBarButtonImpl();
                    sep.Style = Cadencii.Gui.Toolkit.ToolBarButtonStyle.Separator;
                    toolBarTool.Buttons.Add(sep);
                    first = false;
                }
                mPaletteTools.Add(tsb);
                toolBarTool.Buttons.Add(tsb);

                // cMenuTrackSelector
				PaletteToolMenuItem tsmi = ApplicationUIHost.Create<PaletteToolMenuItem> (id);
                tsmi.Text = name;
                tsmi.ToolTipText = desc;
				tsmi.Click += (o, e) => model.RunStripPaletteToolSelected (tsmi.Tag as string, () => tsmi.Checked = true);
                cMenuTrackSelectorPaletteTool.DropDownItems.Add(tsmi);

                // cMenuPiano
				PaletteToolMenuItem tsmi2 = ApplicationUIHost.Create<PaletteToolMenuItem> (id);
                tsmi2.Text = name;
                tsmi2.ToolTipText = desc;
				tsmi2.Click += (o, e) => model.RunStripPaletteToolSelected (tsmi2.Tag as string, () => tsmi2.Checked = true);
                cMenuPianoPaletteTool.DropDownItems.Add(tsmi2);

                // menuSettingPaletteTool
                if (ipt.hasDialog()) {
					PaletteToolMenuItem tsmi3 = ApplicationUIHost.Create<PaletteToolMenuItem> (id);
                    tsmi3.Text = name;
					tsmi3.Click += (o,e) => model.OtherItems.handleSettingPaletteTool ((PaletteToolMenuItem) o);
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
				Consts._SCROLL_WIDTH + Consts._PICT_POSITION_INDICATOR_HEIGHT + pictPianoRoll.MinimumSize.Height +
                                  rebar.Height +
                                  menuStripMain.Height + statusStrip.Height +
                                  (current.Height - client.Height) +
                                  20);
        }
        /// <summary>
        /// 特殊なショートカットキーを処理します。
        /// </summary>
        /// <param name="e"></param>
        /// <param name="onPreviewKeyDown">PreviewKeyDownイベントから送信されてきた場合、true（送る側が設定する）</param>
        public void processSpecialShortcutKey(KeyEventArgs e, bool onPreviewKeyDown)
        {
#if DEBUG
            Logger.StdOut("FormMain#processSpecialShortcutKey");
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
                            Logger.StdOut("FormMain#processSpecialShortcutKey; perform click: name=" + holder.menu.Name);
#endif
                            holder.menu.PerformClick();
                        } catch (Exception ex) {
                            Logger.write(typeof(FormMainImpl) + ".processSpecialShortcutKey; ex=" + ex + "\n");
                            Logger.StdErr("FormMain#processSpecialShortcutKey; ex=" + ex);
                        }
                        if ((Keys) e.KeyCode == Keys.Tab) {
                            pictPianoRoll.Focus();
                        }
                        return;
                    }
                }
            }

            if ((Keys) e.Modifiers != Keys.None) {
#if DEBUG
                Logger.StdOut("FormMain#processSpecialShortcutKey; bailout with (modifier != VK_UNDEFINED)");
#endif
                return;
            }

            EditMode edit_mode = EditorManager.EditMode;

            if ((Keys) e.KeyCode == Keys.Return) {
                // MIDIステップ入力のときの処理
                if (model.IsStepSequencerEnabled) {
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
                    model.Forward();
                }
            } else if ((Keys) e.KeyCode == Keys.Subtract || (Keys) e.KeyCode == Keys.OemMinus || (Keys) e.KeyCode == Keys.Left) {
                if (onPreviewKeyDown) {
                    model.Rewind();
                }
            } else if ((Keys) e.KeyCode == Keys.Escape) {
                // ステップ入力中の場合，入力中の音符をクリアする
                VsqEvent item = EditorManager.mAddingEvent;
                if (model.IsStepSequencerEnabled && item != null) {
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
							model.EnsurePlayerCursorVisible();
                            refreshScreen();
                        }
                    }
                }
            }
            if (!onPreviewKeyDown && flipPlaying) {
				model.FlipPlaying ();
            }
            if ((Keys) e.KeyCode == Keys.Tab) {
                pictPianoRoll.Focus();
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
            float scalex = model.ScaleX;
            int key_width = EditorManager.keyWidth;
            int pict_piano_roll_width = pwidth - key_width;
            int large_change = (int)(pict_piano_roll_width / scalex);
            int maximum = (int)(l + large_change);

            int thumb_width = AwtHost.Current.HorizontalScrollBarThumbWidth;
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

            float scaley = model.ScaleY;

            int maximum = (int)(128 * (int)(100 * scaley) / scaley);
            int large_change = (int)(pheight / scaley);

			int thumb_height = AwtHost.Current.VerticalScrollBarThumbHeight;
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
				var menu = model.SearchMenuItemFromName(menuStripMain, key, parent);
                if (menu != null) {
                    string menu_name = "";
                    if (menu is UiToolStripMenuItem) {
                        menu_name = ((UiToolStripMenuItem)menu).Name;
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
							if (dict.ContainsKey(AwtHost.Current.GetComponentName(dd_run))) {
								applyMenuItemShortcut(dict, tsmi, AwtHost.Current.GetComponentName(tsi));
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
                    if (!(item is UiToolStripMenuItem)) {
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
                Logger.write(typeof(FormMainImpl) + ".applyMenuItemShortcut; ex=" + ex + "\n");
            }
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
        /// このコンポーネントの表示言語を、現在の言語設定に従って更新します。
        /// </summary>
        public void applyLanguage()
        {
            openXmlVsqDialog.Filter = string.Empty;
            try {
                openXmlVsqDialog.Filter = string.Join("|", new[] { _("XML-VSQ Format(*.xvsq)|*.xvsq"), _("All Files(*.*)|*.*") });
            } catch (Exception ex) {
                Logger.write(typeof(FormMainImpl) + ".applyLanguage; ex=" + ex + "\n");
                openXmlVsqDialog.Filter = string.Join("|", new[] { "XML-VSQ Format(*.xvsq)|*.xvsq", "All Files(*.*)|*.*" });
            }

            saveXmlVsqDialog.Filter = string.Empty;
            try {
                saveXmlVsqDialog.Filter = string.Join("|", new[] { _("XML-VSQ Format(*.xvsq)|*.xvsq"), _("All Files(*.*)|*.*") });
            } catch (Exception ex) {
                Logger.write(typeof(FormMainImpl) + ".applyLanguage; ex=" + ex + "\n");
                saveXmlVsqDialog.Filter = string.Join("|", new[] { "XML-VSQ Format(*.xvsq)|*.xvsq", "All Files(*.*)|*.*" });
            }

            openUstDialog.Filter = string.Empty;
            try {
                openUstDialog.Filter = string.Join("|", new[] { _("UTAU Script Format(*.ust)|*.ust"), _("All Files(*.*)|*.*") });
            } catch (Exception ex) {
                Logger.write(typeof(FormMainImpl) + ".applyLanguage; ex=" + ex + "\n");
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
                Logger.write(typeof(FormMainImpl) + ".applyLanguage; ex=" + ex + "\n");
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
                Logger.write(typeof(FormMainImpl) + ".applyLanguage; ex=" + ex + "\n");
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
                Logger.write(typeof(FormMainImpl) + ".applyLanguage; ex=" + ex + "\n");
                openWaveDialog.Filter = string.Join("|", new[] {
                    "Wave File(*.wav)|*.wav",
                    "All Files(*.*)|*.*" });
            }

            stripLblGameCtrlMode.ToolTipText = _("Game controler");

			this.Invoke(new Action(model.UpdateGameControlerStatus));

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
                    if (tsb.Style == Cadencii.Gui.Toolkit.ToolBarButtonStyle.ToggleButton && tsb.Tag != null && tsb.Tag is string) {
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
            double new_val = (double)hScroll.Value - delta * EditorManager.editorConfig.WheelOrder / (5.0 * model.ScaleX);
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
                    if (tsb.Style == Cadencii.Gui.Toolkit.ToolBarButtonStyle.ToggleButton && tag != null && tag is string) {
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
                int yoffset = (int)(127 * (int)(100 * model.ScaleY));
                float scalex = model.ScaleX;
                Font SMALL_FONT = null;
                try {
                    SMALL_FONT = new Font(EditorManager.editorConfig.ScreenFontName, Cadencii.Gui.Font.PLAIN, EditorConfig.FONT_SIZE8);
                    int track_height = (int)(100 * model.ScaleY);
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
                            string title = Utility.trimString(lyric_jp + " [" + lyric_en + "]", SMALL_FONT, lyric_width);
                            int accent = item.ID.DEMaccent;
                            int px_vibrato_start = x + lyric_width;
                            int px_vibrato_end = x;
                            int px_vibrato_delay = lyric_width * 2;
                            int vib_delay = length;
                            if (item.ID.VibratoHandle != null) {
                                vib_delay = item.ID.VibratoDelay;
                                double rate = (double)vib_delay / (double)length;
                                px_vibrato_delay = Consts._PX_ACCENT_HEADER + (int)((lyric_width - Consts._PX_ACCENT_HEADER) * rate);
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
                                width = AppConfig.DYNAFF_ITEM_WIDTH;
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
                            int y = -note * (int)(100 * model.ScaleY) + yoffset;
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
                    Logger.write(typeof(FormMainImpl) + ".updateDrawObjectList; ex=" + ex + "\n");
                    Logger.StdErr("FormMain#updateDrawObjectList; ex=" + ex);
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
			string title = file + " - " + Consts.ApplicationName;
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

        public void updateMenuFonts()
        {
            if (EditorManager.editorConfig.BaseFontName == "") {
                return;
            }
            Font font = EditorManager.editorConfig.getBaseFont();
            AwtHost.Current.ApplyFontRecurse((UiForm) this, font);
#if !JAVA_MAC
            Utility.applyContextMenuFontRecurse(cMenuPiano, font);
            Utility.applyContextMenuFontRecurse(cMenuTrackSelector, font);
            if (EditorManager.MixerWindow != null) {
                AwtHost.Current.ApplyFontRecurse(EditorManager.MixerWindow, font);
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
            AwtHost.Current.ApplyFontRecurse(toolBarFile, font);
            AwtHost.Current.ApplyFontRecurse(toolBarMeasure, font);
            AwtHost.Current.ApplyFontRecurse(toolBarPosition, font);
            AwtHost.Current.ApplyFontRecurse(toolBarTool, font);
            if (mDialogPreference != null) {
                AwtHost.Current.ApplyFontRecurse(mDialogPreference, font);
            }

			EditorConfig.baseFont10Bold = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.BOLD, EditorConfig.FONT_SIZE10);
			EditorConfig.baseFont8 = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.PLAIN, EditorConfig.FONT_SIZE8);
			EditorConfig.baseFont10 = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.PLAIN, EditorConfig.FONT_SIZE10);
			EditorConfig.baseFont9 = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.PLAIN, EditorConfig.FONT_SIZE9);
			EditorConfig.baseFont50Bold = new Font(EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.BOLD, EditorConfig.FONT_SIZE50);
			AppConfig.baseFont10OffsetHeight = Utility.getStringDrawOffset(EditorConfig.baseFont10);
			AppConfig.baseFont8OffsetHeight = Utility.getStringDrawOffset(EditorConfig.baseFont8);
			AppConfig.baseFont9OffsetHeight = Utility.getStringDrawOffset(EditorConfig.baseFont9);
			AppConfig.baseFont50OffsetHeight = Utility.getStringDrawOffset(EditorConfig.baseFont50Bold);
			AppConfig.baseFont8Height = Utility.measureString(Utility.PANGRAM, EditorConfig.baseFont8).Height;
			AppConfig.baseFont9Height = Utility.measureString(Utility.PANGRAM, EditorConfig.baseFont9).Height;
			AppConfig.baseFont10Height = Utility.measureString(Utility.PANGRAM, EditorConfig.baseFont10).Height;
			AppConfig.baseFont50Height = Utility.measureString(Utility.PANGRAM, EditorConfig.baseFont50Bold).Height;
        }

        /// <summary>
        /// イベントハンドラを登録します。
        /// </summary>
        public void registerEventHandlers()
        {
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

			var mainForm = this.AsAwt ();
			mainForm.Load += (o,e) => model.FormMain.RunLoad ();
			mainForm.Deactivate += (o,e) => model.FormMain.RunDeactivate ();
			mainForm.Activated += (o,e) => model.FormMain.RunActivated ();
			mainForm.FormClosed += (o,e) => model.FormMain.FormMain_FormClosed ();
			mainForm.FormClosing += (o,e ) => model.FormMain.RunFormClosing (e);
			mainForm.PreviewKeyDown += (o, e) => model.FormMain.RunPreviewKeyDown (e);
			mainForm.SizeChanged += (o,e) => model.FormMain.RunSizeChanged ();
			//mainForm.Resize += new EventHandler( handleVScrollResize );

			menuFileNew.Click += (o, e) => model.FileMenu.RunFileNewCommand ();
			menuFileOpen.Click += (o, e) => model.FileMenu.RunFileOpenCommand ();
			menuFileSave.Click += (o, e) => model.FileMenu.RunFileSaveCommand ();
			menuFileSaveNamed.Click += (o, e) => model.FileMenu.RunFileSaveNamedCommand ();
			menuFileOpenVsq.Click += (o, e) => model.FileMenu.RunFileOpenVsqCommand ();
			menuFileOpenUst.Click += (o, e) => model.FileMenu.RunFileOpenUstCommand ();
			menuFileImportMidi.Click += (o, e) => model.FileMenu.RunFileImportMidiCommand ();
			menuFileImportUst.Click += (o, e) => model.FileMenu.RunFileImportUstCommand ();
			menuFileImportVsq.Click += (o, e) => model.FileMenu.RunFileImportVsqCommand ();
			menuFileExport.DropDownOpening += (o, e) => model.FileMenu.RunExportDropDownCommand ();
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
			menuEdit.DropDownOpening += (o, e) => model.FileMenu.RunEditDropDownCommand ();
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
			menuSettingGameControlerLoad.Click += (o, e) => model.LoadGameController ();
			menuSettingGameControlerRemove.Click += (o, e) => model.RemoveGameControler ();
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
			cMenuTrackTab.Opening += (o, e) => model.TrackMenu.RunTrackTabOpening ();
			cMenuTrackTabTrackOn.Click += (o, e) => model.TrackMenu.RunTrackOnCommand ();
			cMenuTrackTabAdd.Click += (o, e) => model.AddTrack ();
			cMenuTrackTabCopy.Click += (o, e) => model.CopyTrack ();
			cMenuTrackTabChangeName.Click += (o, e) => model.ChangeTrackName ();
			cMenuTrackTabDelete.Click += (o, e) => model.DeleteTrack ();
			cMenuTrackTabRenderCurrent.Click += (o, e) => model.TrackMenu.RunTrackTabRenderCurrentCommand ();
			cMenuTrackTabRenderAll.Click += (o, e) => model.TrackMenu.RunTrackRenderAllCommand ();
			cMenuTrackTabOverlay.Click += (o, e) => model.TrackMenu.RunTrackTabOverlayCommand ();
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
			cMenuPositionIndicatorEndMarker.Click += (o,e) => model.PositionIndicator.RunEndMarkerCommand ();
			cMenuPositionIndicatorStartMarker.Click += (o,e) => model.PositionIndicator.RunStartMarkerCommand ();
			trackBar.ValueChanged += (o,e) => model.OtherItems.trackBar_ValueChanged ();
			trackBar.Enter += (o,e) => model.OtherItems.trackBar_Enter ();
			bgWorkScreen.DoWork += (o, e) => model.OtherItems.bgWorkScreen_DoWork ();
			pictKeyLengthSplitter.MouseMove += (o,e) => model.OtherItems.pictKeyLengthSplitter_MouseMove ();
			pictKeyLengthSplitter.MouseDown += (o,e) => model.OtherItems.pictKeyLengthSplitter_MouseDown ();
			pictKeyLengthSplitter.MouseUp += (o,e) => model.OtherItems.pictKeyLengthSplitter_MouseUp ();
			panelOverview.KeyUp += (o,e) => model.HandleSpaceKeyUp (e);
			panelOverview.KeyDown += (o,e) => model.HandleSpaceKeyDown (e);
			vScroll.ValueChanged += (o,e) => model.OtherItems.vScroll_ValueChanged ();
			pictPianoRoll.Resize += (o, e) => model.PianoRoll.RunPianoRollResize ();
			vScroll.Enter += (o,e) => model.OtherItems.vScroll_Enter();
			hScroll.ValueChanged += (o,e) => model.OtherItems.hScroll_ValueChanged();
			hScroll.Resize += (o,e) => model.OtherItems.hScroll_Resize();
			hScroll.Enter += (o,e) => model.OtherItems.hScroll_Enter();
			picturePositionIndicator.PreviewKeyDown += (o,e) => model.PositionIndicator.RunPreviewKeyDown (e);
			picturePositionIndicator.MouseMove += (o,e) => model.PositionIndicator.RunMouseMove (e);
			picturePositionIndicator.MouseClick += (o,e) => model.PositionIndicator.RunMouseClick (e);
			picturePositionIndicator.MouseDoubleClick += (o,e) => model.PositionIndicator.RunMouseDoubleClick (e);
			picturePositionIndicator.MouseDown += (o,e) => model.PositionIndicator.RunMouseDown (e);
			picturePositionIndicator.MouseUp += (o,e) => model.PositionIndicator.RunMouseUp (e);
			picturePositionIndicator.Paint += (o,e) => model.PositionIndicator.RunPaint (e);
			pictPianoRoll.PreviewKeyDown += (o, e) => model.PianoRoll.RunPianoRollPreviewKeyDown2 (e);
			pictPianoRoll.KeyUp += (o,e) => model.HandleSpaceKeyUp (e);
			pictPianoRoll.KeyUp += (o, e) => model.PianoRoll.RunPianoRollKeyUp (e);
			pictPianoRoll.MouseMove += (o, e) => model.PianoRoll.RunPianoRollMouseMove (e);
			pictPianoRoll.MouseDoubleClick += (o, e) => model.PianoRoll.RunPianoRollMouseDoubleClick (e);
			pictPianoRoll.MouseClick += (o, e) => model.PianoRoll.RunPianoRollMouseClick (e);
			pictPianoRoll.MouseDown += (o, e) => model.PianoRoll.RunPianoRollMouseDown (e);
			pictPianoRoll.MouseUp += (o, e) => model.PianoRoll.RunPianoRollMouseUp (e);
			pictPianoRoll.KeyDown += (o,e) => model.HandleSpaceKeyDown (e);
			pictPianoRoll.MouseWheel += (o, e) => model.PianoRoll.RunPianoRollMouseWheelCommand (e);
			trackSelector.MouseClick += (o, e) => model.TrackSelector.RunMouseClick (e);
			trackSelector.MouseUp += (o, e) => model.TrackSelector.RunMouseUp (e);
			trackSelector.MouseDown += (o, e) => model.TrackSelector.RunMouseDown (e);
			trackSelector.MouseMove += (o, e) => model.TrackSelector.RunMouseMove (e);
			trackSelector.KeyDown += (o,e) => model.HandleSpaceKeyDown (e);
			trackSelector.KeyUp += (o,e) => model.HandleSpaceKeyUp (e);
			trackSelector.PreviewKeyDown += (o, e) => model.TrackSelector.RunPreviewKeyDown (e);
			trackSelector.SelectedTrackChanged += (o, trk) => model.TrackSelector.RunSelectedTrackChanged (trk);
			trackSelector.SelectedCurveChanged += (o, type) => model.TrackSelector.RunSelectedCurveChanged (type);
			trackSelector.RenderRequired += (o, trk) => model.TrackSelector.RunRenderRequired (trk);
			trackSelector.PreferredMinHeightChanged += (o, e) => model.TrackSelector.RunPreferredMinHeightChanged ();
			trackSelector.MouseDoubleClick += (o, e) => model.TrackSelector.RunMouseDoubleClick (e);
			trackSelector.MouseWheel += (o, e) => model.TrackSelector.RunMouseWheel (e);
			trackSelector.CommandExecuted += (o, e) => model.TrackSelector.RunCommandExecuted ();
			waveView.MouseDoubleClick += (o, e) => model.WaveView.RunMouseDoubleClick (e);
			waveView.MouseDown += (o, e) => model.WaveView.RunMouseDown (e);
			waveView.MouseUp += (o, e) => model.WaveView.RunMouseUp (e);
			waveView.MouseMove += (o, e) => model.WaveView.RunMouseMove (e);
			this.AsAwt ().DragEnter += (o,e) => model.FormMain.DragEnter(e);
			this.AsAwt ().DragDrop += (o,e) => model.FormMain.DragDrop (e);
			this.AsAwt ().DragOver += (o,e) => model.FormMain.DragOver (e);
			this.AsAwt ().DragLeave += (o,e) => model.FormMain.DragLeave ();

			pictureBox2.MouseDown += (o,e) => model.OtherItems.pictureBox2_MouseDown(e);
			pictureBox2.MouseUp += (o,e) => model.OtherItems.pictureBox2_MouseUp ();
			pictureBox2.Paint += (o,e) => model.OtherItems.pictureBox2_Paint (e);
			toolBarTool.ButtonClick += (o,e) => model.ToolBars.ToolButtonClick (e);
			rebar.SizeChanged += (o,e) => model.OtherItems.toolStripContainer_TopToolStripPanel_SizeChanged ();
			stripDDBtnQuantize04.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p4);
			stripDDBtnQuantize08.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p8);
			stripDDBtnQuantize16.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p16);
			stripDDBtnQuantize32.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p32);
			stripDDBtnQuantize64.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p64);
			stripDDBtnQuantize128.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.p128);
			stripDDBtnQuantizeOff.Click += (o, e) => model.HandlePositionQuantize (QuantizeMode.off);
			stripDDBtnQuantizeTriplet.Click += (o, e) => model.HandlePositionQuantizeTriplet ();
			toolBarFile.ButtonClick += (o, e) => model.ToolBars.FileButtonClick (e);
			toolBarPosition.ButtonClick += (o, e) => model.ToolBars.PositionButtonClick (e);
			toolBarMeasure.ButtonClick += (o, e) => model.ToolBars.MeasureButtonClick (e);
			toolBarMeasure.MouseDown += (o, e) => model.ToolBars.MeasureMouseDown (e);
			stripBtnStepSequencer.CheckedChanged += (o, e) => model.ToolBars.StepSequencerCheckedChanged ();
			panelOverview.Enter += (o,e) => model.OtherItems.panelOverview_Enter();
        }

        #endregion // public methods


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
        #endregion
    }

}
