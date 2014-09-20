/*
 * AppManager.cs
 * Copyright © 2009-2011 kbinani
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
//#define ENABLE_OBSOLUTE_COMMAND
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Threading;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.CSharp;
using cadencii.apputil;
using cadencii.java.awt;
using cadencii.java.io;
using cadencii.java.util;
using cadencii.media;
using cadencii.vsq;
using cadencii.xml;
using cadencii.utau;
using ApplicationGlobal = cadencii.core.ApplicationGlobal;
using Keys = cadencii.java.awt.Keys;
using DialogResult = cadencii.java.awt.DialogResult;

namespace cadencii
{


    public partial class AppManager
    {
        /// <summary>
        /// AttachedCurve用のシリアライザ
        /// </summary>
        public static XmlSerializer xmlSerializerListBezierCurves = new XmlSerializer(typeof(AttachedCurve));
        /// <summary>
        /// クリップボード管理クラスのインスタンス
        /// </summary>
        public static ClipboardModel clipboard = null;
        /// <summary>
        /// 選択アイテムの管理クラスのインスタンス
        /// </summary>
        public static ItemSelectionModel itemSelection = null;

        #region Static Readonly Fields
        /// <summary>
        /// トラックの背景部分の塗りつぶし色。16トラックそれぞれで異なる
        /// </summary>
        public static readonly Color[] HILIGHT = new Color[] {
            new Color( 181, 220, 16 ),
            new Color( 231, 244, 49 ),
            new Color( 252, 230, 29 ),
            new Color( 247, 171, 20 ),
            new Color( 249, 94, 17 ),
            new Color( 234, 27, 47 ),
            new Color( 175, 20, 80 ),
            new Color( 183, 24, 149 ),
            new Color( 105, 22, 158 ),
            new Color( 22, 36, 163 ),
            new Color( 37, 121, 204 ),
            new Color( 29, 179, 219 ),
            new Color( 24, 239, 239 ),
            new Color( 25, 206, 175 ),
            new Color( 23, 160, 134 ),
            new Color( 79, 181, 21 ) };
        /// <summary>
        /// トラックをレンダリングするためのボタンの背景色。16トラックそれぞれで異なる
        /// </summary>
        public static readonly Color[] RENDER = new Color[]{
            new Color( 19, 143, 52 ),
            new Color( 158, 154, 18 ),
            new Color( 160, 143, 23 ),
            new Color( 145, 98, 15 ),
            new Color( 142, 52, 12 ),
            new Color( 142, 19, 37 ),
            new Color( 96, 13, 47 ),
            new Color( 117, 17, 98 ),
            new Color( 62, 15, 99 ),
            new Color( 13, 23, 84 ),
            new Color( 25, 84, 132 ),
            new Color( 20, 119, 142 ),
            new Color( 19, 142, 139 ),
            new Color( 17, 122, 102 ),
            new Color( 13, 86, 72 ),
            new Color( 43, 91, 12 ) };
        /// <summary>
        /// スクリプトに前置されるusingのリスト
        /// </summary>
        public static readonly string[] usingS = new string[] { "using System;",
                                             "using System.IO;",
                                             "using cadencii.vsq;",
                                             "using cadencii;",
                                             "using cadencii.java.io;",
                                             "using cadencii.java.util;",
                                             "using cadencii.java.awt;",
                                             "using cadencii.media;",
                                             "using cadencii.apputil;",
                                             "using System.Windows.Forms;",
                                             "using System.Collections.Generic;",
                                             "using System.Drawing;",
                                             "using System.Text;",
                                             "using System.Xml.Serialization;" };
        /// <summary>
        /// ショートカットキーとして受付可能なキーのリスト
        /// </summary>
        public static readonly List<Keys> SHORTCUT_ACCEPTABLE = new List<Keys>(new Keys[] {
            Keys.A,
            Keys.B,
            Keys.Back,
            Keys.C,
            Keys.D,
            Keys.D0,
            Keys.D1,
            Keys.D2,
            Keys.D3,
            Keys.D4,
            Keys.D5,
            Keys.D6,
            Keys.D7,
            Keys.D8,
            Keys.D9,
            Keys.Down,
            Keys.E,
            Keys.F,
            Keys.F1,
            Keys.F2,
            Keys.F3,
            Keys.F4,
            Keys.F5,
            Keys.F6,
            Keys.F7,
            Keys.F8,
            Keys.F9,
            Keys.F10,
            Keys.F11,
            Keys.F12,
            Keys.F13,
            Keys.F14,
            Keys.F15,
            Keys.F16,
            Keys.F17,
            Keys.F18,
            Keys.F19,
            Keys.F20,
            Keys.F21,
            Keys.F22,
            Keys.F23,
            Keys.F24,
            Keys.G,
            Keys.H,
            Keys.I,
            Keys.J,
            Keys.K,
            Keys.L,
            Keys.Left,
            Keys.M,
            Keys.N,
            Keys.NumPad0,
            Keys.NumPad1,
            Keys.NumPad2,
            Keys.NumPad3,
            Keys.NumPad4,
            Keys.NumPad5,
            Keys.NumPad6,
            Keys.NumPad7,
            Keys.NumPad8,
            Keys.NumPad9,
            Keys.O,
            Keys.P,
            Keys.PageDown,
            Keys.PageUp,
            Keys.Q,
            Keys.R,
            Keys.Right,
            Keys.S,
            Keys.Space,
            Keys.T,
            Keys.U,
            Keys.Up,
            Keys.V,
            Keys.W,
            Keys.X,
            Keys.Y,
            Keys.Z,
            Keys.Delete,
            Keys.Home,
            Keys.End,
        });
        /// <summary>
        /// よく使うボーダー線の色
        /// </summary>
        public static readonly Color COLOR_BORDER = new Color(118, 123, 138);
        #endregion

        #region Private Static Fields
        private static System.Windows.Forms.Timer mAutoBackupTimer;

        private static int mCurrentClock = 0;
        private static bool mPlaying = false;
        private static bool mGridVisible = false;
        /// <summary>
        /// トラックのオーバーレイ表示
        /// </summary>
        private static bool mOverlay = true;

        /// <summary>
        /// Playingプロパティにロックをかけるためのオブジェクト
        /// </summary>
        private static Object mLockerPlayingProperty = new Object();
        #endregion

        #region 選択範囲の管理
        /// <summary>
        /// SelectedRegionが有効かどうかを表すフラグ
        /// </summary>
        private static bool mWholeSelectedIntervalEnabled = false;
        /// <summary>
        /// Ctrlキーを押しながらのマウスドラッグ操作による選択が行われた範囲(単位：クロック)
        /// </summary>
        public static SelectedRegion mWholeSelectedInterval = new SelectedRegion(0);
        /// <summary>
        /// コントロールカーブ上で現在選択されている範囲（x:クロック、y:各コントロールカーブの単位に同じ）。マウスが動いているときのみ使用
        /// </summary>
        public static Rectangle mCurveSelectingRectangle = new Rectangle();
        /// <summary>
        /// コントロールカーブ上で選択された範囲（単位：クロック）
        /// </summary>
        public static SelectedRegion mCurveSelectedInterval = new SelectedRegion(0);
        /// <summary>
        /// 選択範囲が有効かどうか。
        /// </summary>
        private static bool mCurveSelectedIntervalEnabled = false;
        /// <summary>
        /// 範囲選択モードで音符を動かしている最中の、選択範囲の開始位置（クロック）。マウスが動いているときのみ使用
        /// </summary>
        public static int mWholeSelectedIntervalStartForMoving = 0;
        #endregion

        /// <summary>
        /// 自動ノーマライズモードかどうかを表す値を取得、または設定します。
        /// </summary>
        public static bool mAutoNormalize = false;
        /// <summary>
        /// 再生時に自動スクロールするかどうか
        /// </summary>
        public static bool mAutoScroll = true;
        /// <summary>
        /// プレビュー再生が開始された時刻
        /// </summary>
        public static double mPreviewStartedTime;
        /// <summary>
        /// 現在選択中のパレットアイテムの名前
        /// </summary>
        public static string mSelectedPaletteTool = "";
        /// <summary>
        /// 画面に描かれるエントリのリスト．trackBar.Valueの変更やエントリの編集などのたびに更新される
        /// </summary>
        public static List<DrawObject>[] mDrawObjects = new List<DrawObject>[ApplicationGlobal.MAX_NUM_TRACK];
        /// <summary>
        /// 歌詞入力に使用するテキストボックス
        /// </summary>
        public static LyricTextBox mInputTextBox = null;
        public static int mAddingEventLength;
        /// <summary>
        /// 音符の追加操作における，追加中の音符
        /// </summary>
        public static VsqEvent mAddingEvent;
        /// <summary>
        /// AppManager.m_draw_objectsを描く際の，最初に検索されるインデクス．
        /// </summary>
        public static int[] mDrawStartIndex = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// 各トラックがUTAUモードかどうか．mDrawObjectsと同じタイミングで更新される
        /// </summary>
		public static bool[] mDrawIsUtau = new bool[ApplicationGlobal.MAX_NUM_TRACK];
        /// <summary>
        /// マウスが降りていて，かつ範囲選択をしているときに立つフラグ
        /// </summary>
        public static bool mIsPointerDowned = false;
        /// <summary>
        /// マウスが降りた仮想スクリーン上の座標(ピクセル)
        /// </summary>
        public static Point mMouseDownLocation = new Point();
        public static int mLastTrackSelectorHeight;
        /// <summary>
        /// 再生開始からの経過時刻がこの秒数以下の場合、再生を止めることが禁止される。
        /// </summary>
        public static double mForbidFlipPlayingThresholdSeconds = 0.2;
        /// <summary>
        /// ピアノロール画面に，コントロールカーブをオーバーレイしているモード
        /// </summary>
        public static bool mCurveOnPianoroll = false;
        /// <summary>
        /// 直接再生モード時の、再生開始した位置の曲頭からの秒数
        /// </summary>
        public static float mDirectPlayShift = 0.0f;
        /// <summary>
        /// プレビュー終了位置のクロック
        /// </summary>
        public static int mPreviewEndingClock = 0;

		/// <summary>
		/// ダイアログを表示中かどうか
		/// </summary>
//		private static bool mShowingDialog = false;
		#if ENABLE_PROPERTY
		/// <summary>
		/// プロパティウィンドウが分離した場合のプロパティウィンドウのインスタンス。
		/// メインウィンドウとプロパティウィンドウが分離している時、propertyPanelがpropertyWindowの子になる
		/// </summary>
		public static FormNotePropertyController propertyWindow;
		#endif
		/// <summary>
		/// アイコンパレット・ウィンドウのインスタンス
		/// </summary>
		public static FormIconPalette iconPalette = null;

		/// <summary>
		/// メインウィンドウにフォーカスを当てる要求があった時発生するイベント
		/// </summary>
		public static EventHandler MainWindowFocusRequired;

        #region 裏設定項目
        /// <summary>
        /// 再生中にWAVE波形の描画をスキップするかどうか（デフォルトはtrue）
        /// </summary>
        public static bool skipDrawingWaveformWhenPlaying = true;
        /// <summary>
        /// コントロールカーブに、音符の境界線を重ね描きするかどうか（デフォルトはtrue）
        /// </summary>
        public static bool drawItemBorderInControlCurveView = true;
        /// <summary>
        /// コントロールカーブに、データ点を表す四角を描くかどうか（デフォルトはtrue）
        /// </summary>
        public static bool drawCurveDotInControlCurveView = true;
        /// <summary>
        /// ピアノロール画面に、現在選択中の歌声合成エンジンの種類を描くかどうか
        /// </summary>
        public static bool drawOverSynthNameOnPianoroll = true;
        /// <summary>
        /// ピアノロール上で右クリックでコンテキストメニューを表示するかどうか
        /// </summary>
        public static bool showContextMenuWhenRightClickedOnPianoroll = true;
        #endregion // 裏設定項目

        /// <summary>
        /// メイン画面で、グリッド表示のOn/Offが切り替わった時発生するイベント
        /// </summary>
        public static event EventHandler GridVisibleChanged;

        /// <summary>
        /// プレビュー再生が開始された時発生するイベント
        /// </summary>
        public static event EventHandler PreviewStarted;

        /// <summary>
        /// プレビュー再生が終了した時発生するイベント
        /// </summary>
        public static event EventHandler PreviewAborted;

        /// <summary>
        /// BGMに何らかの変更があった時発生するイベント
        /// </summary>
        public static event EventHandler UpdateBgmStatusRequired;

        static AppManager()
        {
			for (int i = 0; i < ApplicationGlobal.MAX_NUM_TRACK; i++) {
                mDrawObjects[i] = new List<DrawObject>();
            }
        }

        /// <summary>
        /// プレビュー再生を開始します．
        /// 合成処理などが途中でキャンセルされた場合にtrue，それ以外の場合にfalseを返します
        /// </summary>
        private static bool previewStart(FormMain form)
        {
            int selected = EditorManager.Selected;
            RendererKind renderer = VsqFileEx.getTrackRendererKind(mVsq.Track[selected]);
            int clock = mCurrentClock;
            mDirectPlayShift = (float)mVsq.getSecFromClock(clock);
            // リアルタイム再生で無い場合
            string tmppath = ApplicationGlobal.getTempWaveDir();

            int track_count = mVsq.Track.Count;

            List<int> tracks = new List<int>();
            for (int track = 1; track < track_count; track++) {
                tracks.Add(track);
            }

            if (patchWorkToFreeze(form, tracks)) {
                // キャンセルされた
#if DEBUG
                sout.println("AppManager#previewStart; patchWorkToFreeze returns true");
#endif
                return true;
            }

            WaveSenderDriver driver = new WaveSenderDriver();
            List<Amplifier> waves = new List<Amplifier>();
            for (int i = 0; i < tracks.Count; i++) {
                int track = tracks[i];
                string file = Path.Combine(tmppath, track + ".wav");
                WaveReader wr = null;
                try {
                    wr = new WaveReader(file);
                    wr.setOffsetSeconds(mDirectPlayShift);
                    Amplifier a = new Amplifier();
                    FileWaveSender f = new FileWaveSender(wr);
                    a.setSender(f);
                    a.setAmplifierView(EditorManager.MixerWindow.getVolumeTracker(track));
                    waves.Add(a);
                    a.setRoot(driver);
                    f.setRoot(driver);
                } catch (Exception ex) {
                    Logger.write(typeof(AppManager) + ".previewStart; ex=" + ex + "\n");
                    serr.println("AppManager.previewStart; ex=" + ex);
                }
            }

            // clock以降に音符があるかどうかを調べる
            int count = 0;
            foreach (var ve in mVsq.Track[selected].getNoteEventIterator()) {
                if (ve.Clock >= clock) {
                    count++;
                    break;
                }
            }

            int bgm_count = MusicManager.getBgmCount();
            double pre_measure_sec = mVsq.getSecFromClock(mVsq.getPreMeasureClocks());
            for (int i = 0; i < bgm_count; i++) {
                BgmFile bgm = MusicManager.getBgm(i);
                WaveReader wr = null;
                try {
                    wr = new WaveReader(bgm.file);
                    double offset = bgm.readOffsetSeconds + mDirectPlayShift;
                    if (bgm.startAfterPremeasure) {
                        offset -= pre_measure_sec;
                    }
                    wr.setOffsetSeconds(offset);
#if DEBUG
                    sout.println("AppManager.previewStart; bgm.file=" + bgm.file + "; offset=" + offset);

#endif
                    Amplifier a = new Amplifier();
                    FileWaveSender f = new FileWaveSender(wr);
                    a.setSender(f);
                    a.setAmplifierView(EditorManager.MixerWindow.getVolumeTrackerBgm(i));
                    waves.Add(a);
                    a.setRoot(driver);
                    f.setRoot(driver);
                } catch (Exception ex) {
                    Logger.write(typeof(AppManager) + ".previewStart; ex=" + ex + "\n");
                    serr.println("AppManager.previewStart; ex=" + ex);
                }
            }

            // 最初のsenderをドライバにする
            driver.setSender(waves[0]);
            Mixer m = new Mixer();
            m.setRoot(driver);
            driver.setReceiver(m);
            EditorManager.stopGenerator();
            EditorManager.setGenerator(driver);
            Amplifier amp = new Amplifier();
            amp.setRoot(driver);
            amp.setAmplifierView(EditorManager.MixerWindow.getVolumeTrackerMaster());
            m.setReceiver(amp);
            MonitorWaveReceiver monitor = MonitorWaveReceiver.prepareInstance();
            monitor.setRoot(driver);
            amp.setReceiver(monitor);
            for (int i = 1; i < waves.Count; i++) {
                m.addSender(waves[i]);
            }

            int end_clock = mVsq.TotalClocks;
            if (mVsq.config.EndMarkerEnabled) {
                end_clock = mVsq.config.EndMarker;
            }
            mPreviewEndingClock = end_clock;
            double end_sec = mVsq.getSecFromClock(end_clock);
            int sample_rate = mVsq.config.SamplingRate;
            long samples = (long)((end_sec - mDirectPlayShift) * sample_rate);
            driver.init(mVsq, EditorManager.Selected, 0, end_clock, sample_rate);
#if DEBUG
            sout.println("AppManager.previewStart; calling runGenerator...");
#endif
            EditorManager.runGenerator(samples);
#if DEBUG
            sout.println("AppManager.previewStart; calling runGenerator... done");
#endif
            return false;
        }

        public static int getPreviewEndingClock()
        {
            return mPreviewEndingClock;
        }

        /// <summary>
        /// プレビュー再生を停止します
        /// </summary>
        private static void previewStop()
        {
            EditorManager.stopGenerator();
        }

        /// <summary>
        /// 指定したトラックのレンダリングが必要な部分を再レンダリングし，ツギハギすることでトラックのキャッシュを最新の状態にします．
        /// レンダリングが途中でキャンセルされた場合にtrue，そうでない場合にfalseを返します．
        /// </summary>
        /// <param name="tracks"></param>
        public static bool patchWorkToFreeze(FormMain main_window, List<int> tracks)
        {
            mVsq.updateTotalClocks();
            List<PatchWorkQueue> queue = EditorManager.patchWorkCreateQueue(tracks);
#if DEBUG
            sout.println("AppManager#patchWorkToFreeze; queue.size()=" + queue.Count);
#endif

			FormWorker fw = new FormWorker(() => new ProgressBarWithLabelUiImpl());
            fw.setupUi(new FormWorkerUiImpl(fw));
            fw.getUi().setTitle(_("Synthesize"));
            fw.getUi().setText(_("now synthesizing..."));

            double total = 0;
            SynthesizeWorker worker = new SynthesizeWorker(main_window);
            foreach (PatchWorkQueue q in queue) {
                // ジョブを追加
                double job_amount = q.getJobAmount();
                fw.addJob(worker, "processQueue", q.getMessage(), job_amount, q);
                total += job_amount;
            }

            // パッチワークをするジョブを追加
            fw.addJob(worker, "patchWork", _("patchwork"), total, new Object[] { queue, tracks });

            // ジョブを開始
            fw.startJob();

            // ダイアログを表示する
            var ret = DialogManager.showDialogTo (fw, main_window);

            return ret;
        }


        static string _(string id)
        {
            return Messaging.getMessage(id);
        }

#if !TREECOM
        /// <summary>
        /// アンドゥ処理を行います。
        /// </summary>
        public static void undo()
        {
            if (EditorManager.editHistory.hasUndoHistory()) {
                List<ValuePair<int, int>> before_ids = new List<ValuePair<int, int>>();
                foreach (var item in itemSelection.getEventIterator()) {
                    before_ids.Add(new ValuePair<int, int>(item.track, item.original.InternalID));
                }

                ICommand run_src = EditorManager.editHistory.getUndo();
                CadenciiCommand run = (CadenciiCommand)run_src;
                if (run.vsqCommand != null) {
                    if (run.vsqCommand.Type == VsqCommandType.TRACK_DELETE) {
                        int track = (int)run.vsqCommand.Args[0];
						if (track == EditorManager.Selected && track >= 2) {
							EditorManager.Selected = track - 1;
                        }
                    }
                }
                ICommand inv = mVsq.executeCommand(run);
                if (run.type == CadenciiCommandType.BGM_UPDATE) {
                    try {
                        if (UpdateBgmStatusRequired != null) {
                            UpdateBgmStatusRequired.Invoke(typeof(AppManager), new EventArgs());
                        }
                    } catch (Exception ex) {
                        Logger.write(typeof(AppManager) + ".undo; ex=" + ex + "\n");
                        serr.println(typeof(AppManager) + ".undo; ex=" + ex);
                    }
                }
                EditorManager.editHistory.registerAfterUndo(inv);

                cleanupDeadSelection(before_ids);
                itemSelection.updateSelectedEventInstance();
            }
        }

        /// <summary>
        /// リドゥ処理を行います。
        /// </summary>
        public static void redo()
        {
            if (EditorManager.editHistory.hasRedoHistory()) {
                List<ValuePair<int, int>> before_ids = new List<ValuePair<int, int>>();
                foreach (var item in itemSelection.getEventIterator()) {
                    before_ids.Add(new ValuePair<int, int>(item.track, item.original.InternalID));
                }

                ICommand run_src = EditorManager.editHistory.getRedo();
                CadenciiCommand run = (CadenciiCommand)run_src;
                if (run.vsqCommand != null) {
                    if (run.vsqCommand.Type == VsqCommandType.TRACK_DELETE) {
                        int track = (int)run.args[0];
                        if (track == EditorManager.Selected && track >= 2) {
							EditorManager.Selected = track - 1;
                        }
                    }
                }
                ICommand inv = mVsq.executeCommand(run);
                if (run.type == CadenciiCommandType.BGM_UPDATE) {
                    try {
                        if (UpdateBgmStatusRequired != null) {
                            UpdateBgmStatusRequired.Invoke(typeof(AppManager), new EventArgs());
                        }
                    } catch (Exception ex) {
                        Logger.write(typeof(AppManager) + ".redo; ex=" + ex + "\n");
                        serr.println(typeof(AppManager) + ".redo; ex=" + ex);
                    }
                }
                EditorManager.editHistory.registerAfterRedo(inv);

                cleanupDeadSelection(before_ids);
                itemSelection.updateSelectedEventInstance();
            }
        }

        /// <summary>
        /// 「選択されている」と登録されているオブジェクトのうち、Undo, Redoなどによって存在しなくなったものを登録解除する
        /// </summary>
        public static void cleanupDeadSelection(List<ValuePair<int, int>> before_ids)
        {
            int size = mVsq.Track.Count;
            foreach (var specif in before_ids) {
                bool found = false;
                int track = specif.getKey();
                int internal_id = specif.getValue();
                if (1 <= track && track < size) {
                    foreach (var item in mVsq.Track[track].getNoteEventIterator()) {
                        if (item.InternalID == internal_id) {
                            found = true;
                            break;
                        }
                    }
                }
                if (!found) {
                    AppManager.itemSelection.removeEvent(internal_id);
                }
            }
        }
#endif

        public static bool isOverlay()
        {
            return mOverlay;
        }

        public static void setOverlay(bool value)
        {
            mOverlay = value;
        }

        public static bool getRenderRequired(int track)
        {
            if (mVsq == null) {
                return false;
            }
            return mVsq.editorStatus.renderRequired[track - 1];
        }

        /// <summary>
        /// グリッドを表示するか否かを表す値を取得します
        /// </summary>
        public static bool isGridVisible()
        {
            return mGridVisible;
        }

        /// <summary>
        /// グリッドを表示するか否かを設定します
        /// </summary>
        /// <param name="value"></param>
        public static void setGridVisible(bool value)
        {
            if (value != mGridVisible) {
                mGridVisible = value;
                try {
                    if (GridVisibleChanged != null) {
                        GridVisibleChanged.Invoke(typeof(AppManager), new EventArgs());
                    }
                } catch (Exception ex) {
                    serr.println("AppManager#setGridVisible; ex=" + ex);
                    Logger.write(typeof(AppManager) + ".setGridVisible; ex=" + ex + "\n");
                }
            }
        }

        /// <summary>
        /// 現在プレビュー中かどうかを示す値を取得します
        /// </summary>
        public static bool isPlaying()
        {
            return mPlaying;
        }

        /// <summary>
        /// プレビュー再生中かどうかを設定します．このプロパティーを切り替えることで，再生の開始と停止を行います．
        /// </summary>
        /// <param name="value"></param>
        /// <param name="form"></param>
        public static void setPlaying(bool value, FormMain form)
        {
#if DEBUG
            sout.println("AppManager#setPlaying; value=" + value);
#endif
            lock (mLockerPlayingProperty) {
                bool previous = mPlaying;
                mPlaying = value;
                if (previous != mPlaying) {
                    if (mPlaying) {
                        try {
                            if (previewStart(form)) {
#if DEBUG
                                sout.println("AppManager#setPlaying; previewStart returns true");
#endif
                                mPlaying = false;
                                return;
                            }
                            if (PreviewStarted != null) {
                                PreviewStarted.Invoke(typeof(AppManager), new EventArgs());
                            }
                        } catch (Exception ex) {
                            serr.println("AppManager#setPlaying; ex=" + ex);
                            Logger.write(typeof(AppManager) + ".setPlaying; ex=" + ex + "\n");
                        }
                    } else if (!mPlaying) {
                        try {
                            previewStop();
#if DEBUG
                            sout.println("AppManager#setPlaying; raise previewAbortedEvent");
#endif
                            if (PreviewAborted != null) {
                                PreviewAborted.Invoke(typeof(AppManager), new EventArgs());
                            }
                        } catch (Exception ex) {
                            serr.println("AppManager#setPlaying; ex=" + ex);
                            Logger.write(typeof(AppManager) + ".setPlaying; ex=" + ex + "\n");
                        }
                    }
                }
            }
        }

		public static void saveTo (string file)
		{
			MusicManager.saveTo (file, (a,b,c,d) => DialogManager.showMessageBox(a,b,c,d), _, mFile => {
					EditorManager.editorConfig.pushRecentFiles(mFile);
				if (!mAutoBackupTimer.Enabled && EditorManager.editorConfig.AutoBackupIntervalMinutes > 0) {
					double millisec = EditorManager.editorConfig.AutoBackupIntervalMinutes * 60.0 * 1000.0;
                    int draft = (int)millisec;
                    if (millisec > int.MaxValue) {
                        draft = int.MaxValue;
                    }
                    mAutoBackupTimer.Interval = draft;
                    mAutoBackupTimer.Start();
                }
				});
		}

		static VsqFileEx mVsq { get { return MusicManager.getVsqFile (); } }

        /// <summary>
        /// 現在の演奏マーカーの位置を取得します。
        /// </summary>
        public static int getCurrentClock()
        {
            return mCurrentClock;
        }

        /// <summary>
        /// 現在の演奏マーカーの位置を設定します。
        /// </summary>
        public static void setCurrentClock(int value)
        {
            mCurrentClock = value;
        }
		public static bool readVsq (string file)
		{
			EditorManager.Selected = 1;
			return MusicManager.readVsq (file, hasTracks => {
				if (hasTracks) {
					EditorManager.Selected = 1;
				} else {
					EditorManager.Selected = -1;
				}
				try {
					if (UpdateBgmStatusRequired != null) {
						UpdateBgmStatusRequired.Invoke (typeof(AppManager), new EventArgs ());
					}
				} catch (Exception ex) {
					Logger.write (typeof(AppManager) + ".readVsq; ex=" + ex + "\n");
					serr.println (typeof(AppManager) + ".readVsq; ex=" + ex);
				}
			});
		}

		public static void setVsqFile (VsqFileEx vsq)
		{
			MusicManager.setVsqFile (vsq, preMeasureClocks => {
				mAutoBackupTimer.Stop ();
				setCurrentClock (preMeasureClocks);
				try {
					if (UpdateBgmStatusRequired != null) {
						UpdateBgmStatusRequired.Invoke (typeof(AppManager), new EventArgs ());
					}
				} catch (Exception ex) {
					Logger.write (typeof(AppManager) + ".setVsqFile; ex=" + ex + "\n");
					serr.println (typeof(AppManager) + ".setVsqFile; ex=" + ex);
				}
			});
		}

        public static void init()
        {
            EditorManager.loadConfig();
            clipboard = new ClipboardModel();
            itemSelection = new ItemSelectionModel();
            // UTAU歌手のアイコンを読み込み、起動画面に表示を要求する
			int c = ApplicationGlobal.appConfig.UtauSingers.Count;
            for (int i = 0; i < c; i++) {
				SingerConfig sc = ApplicationGlobal.appConfig.UtauSingers[i];
                if (sc == null) {
                    continue;
                }
                string dir = sc.VOICEIDSTR;
                SingerConfig sc_temp = new SingerConfig();
                string path_image = Utau.readUtauSingerConfig(dir, sc_temp);

#if DEBUG
                sout.println("AppManager#init; path_image=" + path_image);
#endif
                if (Cadencii.splash != null) {
                    try {
                        Cadencii.splash.addIconThreadSafe(path_image, sc.VOICENAME);
                    } catch (Exception ex) {
                        serr.println("AppManager#init; ex=" + ex);
                        Logger.write(typeof(AppManager) + ".init; ex=" + ex + "\n");
                    }
                }
            }

            VocaloSysUtil.init();

			EditorManager.editorConfig.check();
			EditorManager.keyWidth = EditorManager.editorConfig.KeyWidth;
            VSTiDllManager.init();
            // アイコンパレード, VOCALOID1
            SingerConfigSys singer_config_sys1 = VocaloSysUtil.getSingerConfigSys(SynthesizerType.VOCALOID1);
            if (singer_config_sys1 != null) {
                foreach (SingerConfig sc in singer_config_sys1.getInstalledSingers()) {
                    if (sc == null) {
                        continue;
                    }
                    string name = sc.VOICENAME.ToLower();
                    string path_image = Path.Combine(
                                            Path.Combine(
                                                PortUtil.getApplicationStartupPath(), "resources"),
                                            name + ".png");
#if DEBUG
                    sout.println("AppManager#init; path_image=" + path_image);
#endif
                    if (Cadencii.splash != null) {
                        try {
                            Cadencii.splash.addIconThreadSafe(path_image, sc.VOICENAME);
                        } catch (Exception ex) {
                            serr.println("AppManager#init; ex=" + ex);
                            Logger.write(typeof(AppManager) + ".init; ex=" + ex + "\n");
                        }
                    }
                }
            }

            // アイコンパレード、VOCALOID2
            SingerConfigSys singer_config_sys2 = VocaloSysUtil.getSingerConfigSys(SynthesizerType.VOCALOID2);
            if (singer_config_sys2 != null) {
                foreach (SingerConfig sc in singer_config_sys2.getInstalledSingers()) {
                    if (sc == null) {
                        continue;
                    }
                    string name = sc.VOICENAME.ToLower();
                    string path_image = Path.Combine(
                                            Path.Combine(
                                                PortUtil.getApplicationStartupPath(), "resources"),
                                            name + ".png");
#if DEBUG
                    sout.println("AppManager#init; path_image=" + path_image);
#endif
                    if (Cadencii.splash != null) {
                        try {
                            Cadencii.splash.addIconThreadSafe(path_image, sc.VOICENAME);
                        } catch (Exception ex) {
                            serr.println("AppManager#init; ex=" + ex);
                            Logger.write(typeof(AppManager) + ".init; ex=" + ex + "\n");
                        }
                    }
                }
            }

            PlaySound.init();
            // VOCALOID2の辞書を読み込み
            SymbolTable.loadSystemDictionaries();
            // 日本語辞書
            SymbolTable.loadDictionary(
                Path.Combine(Path.Combine(PortUtil.getApplicationStartupPath(), "resources"), "dict_ja.txt"),
                "DEFAULT_JP");
            // 英語辞書
            SymbolTable.loadDictionary(
                Path.Combine(Path.Combine(PortUtil.getApplicationStartupPath(), "resources"), "dict_en.txt"),
                "DEFAULT_EN");
            // 拡張辞書
            SymbolTable.loadAllDictionaries(Path.Combine(PortUtil.getApplicationStartupPath(), "udic"));
            //VSTiProxy.CurrentUser = "";

            // 辞書の設定を適用
            try {
                // 現在辞書リストに読込済みの辞書を列挙
                List<ValuePair<string, Boolean>> current = new List<ValuePair<string, Boolean>>();
                for (int i = 0; i < SymbolTable.getCount(); i++) {
                    current.Add(new ValuePair<string, Boolean>(SymbolTable.getSymbolTable(i).getName(), false));
                }
                // editorConfig.UserDictionariesの設定値をコピー
                List<ValuePair<string, Boolean>> config_data = new List<ValuePair<string, Boolean>>();
				int count = EditorManager.editorConfig.UserDictionaries.Count;
                for (int i = 0; i < count; i++) {
					string[] spl = PortUtil.splitString(EditorManager.editorConfig.UserDictionaries[i], new char[] { '\t' }, 2);
                    config_data.Add(new ValuePair<string, Boolean>(spl[0], (spl[1].Equals("T") ? true : false)));
#if DEBUG
                    CDebug.WriteLine("    " + spl[0] + "," + spl[1]);
#endif
                }
                // 辞書リストとeditorConfigの設定を比較する
                // currentの方には、editorConfigと共通するものについてのみsetValue(true)とする
                List<ValuePair<string, Boolean>> common = new List<ValuePair<string, Boolean>>();
                for (int i = 0; i < config_data.Count; i++) {
                    for (int j = 0; j < current.Count; j++) {
                        if (config_data[i].getKey().Equals(current[j].getKey())) {
                            // editorConfig.UserDictionariesにもKeyが含まれていたらtrue
                            current[j].setValue(true);
                            common.Add(new ValuePair<string, Boolean>(config_data[i].getKey(), config_data[i].getValue()));
                            break;
                        }
                    }
                }
                // editorConfig.UserDictionariesに登録されていないが、辞書リストには読み込まれている場合。
                // この場合は、デフォルトでENABLEとし、優先順位を最後尾とする。
                for (int i = 0; i < current.Count; i++) {
                    if (!current[i].getValue()) {
                        common.Add(new ValuePair<string, Boolean>(current[i].getKey(), true));
                    }
                }
                SymbolTable.changeOrder(common);
            } catch (Exception ex) {
                serr.println("AppManager#init; ex=" + ex);
            }

            EditorManager.reloadUtauVoiceDB();

            mAutoBackupTimer = new System.Windows.Forms.Timer();
            mAutoBackupTimer.Tick += new EventHandler(EditorManager.handleAutoBackupTimerTick);
        }
    }

}
