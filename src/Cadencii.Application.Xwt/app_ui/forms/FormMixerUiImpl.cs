/*
 * FormMixer.cs
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
using System;
using System.Collections.Generic;
using cadencii.apputil;
using Cadencii.Gui;
using Cadencii.Media.Vsq;
using Cadencii.Utilities;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;
using cadencii;
using Cadencii.Application.Media;
using Cadencii.Media;

namespace Cadencii.Application.Forms
{

	public class FormMixerUiImpl : FormImpl, FormMixerUi
    {
        private List<VolumeTracker> m_tracker = null;
        private bool mPreviousAlwaysOnTop;

        public event FederChangedEventHandler FederChanged;

        public event PanpotChangedEventHandler PanpotChanged;

        public event SoloChangedEventHandler SoloChanged;

        public event MuteChangedEventHandler MuteChanged;

        public FormMixerUiImpl(FormMainImpl parent)
        {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            volumeMaster.Model.Feder = 0;
			volumeMaster.Model.Muted = false;
			volumeMaster.Model.Solo = true;
			volumeMaster.Model.Number = "Master";
			volumeMaster.Model.Panpot = 0;
			volumeMaster.Model.SoloButtonVisible = (false);
			volumeMaster.Model.Title = "";
            applyLanguage();
            this.TopMost = true;
			DoubleBuffered = true;
        }

        #region public methods
        /// <summary>
        /// AlwaysOnTopが強制的にfalseにされる直前の，AlwaysOnTop値を取得します．
        /// </summary>
        public bool getPreviousAlwaysOnTop()
        {
            return mPreviousAlwaysOnTop;
        }

        /// <summary>
        /// AlwaysOnTopが強制的にfalseにされる直前の，AlwaysOnTop値を設定しておきます．
        /// </summary>
        public void setPreviousAlwaysOnTop(bool value)
        {
            mPreviousAlwaysOnTop = value;
        }

        /// <summary>
        /// マスターボリュームのUIコントロールを取得します
        /// </summary>
        /// <returns></returns>
        public VolumeTracker getVolumeTrackerMaster()
        {
            return volumeMaster;
        }

        /// <summary>
        /// 指定したトラックのボリュームのUIコントロールを取得します
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public VolumeTracker getVolumeTracker(int track)
        {
            VsqFileEx vsq = MusicManager.getVsqFile();
            if (1 <= track && track < vsq.Track.Count &&
                 0 <= track - 1 && track - 1 < m_tracker.Count) {
                return m_tracker[track - 1];
            } else if (track == 0) {
                return volumeMaster;
            } else {
                return null;
            }
        }

        /// <summary>
        /// 指定したBGMのボリュームのUIコントロールを取得します
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VolumeTracker getVolumeTrackerBgm(int index)
        {
            VsqFileEx vsq = MusicManager.getVsqFile();
            int offset = vsq.Track.Count - 1;
            if (0 <= index + offset && index + offset < m_tracker.Count) {
                return m_tracker[index + offset];
            } else {
                return null;
            }
        }

        /// <summary>
        /// ソロ，ミュートのボタンのチェック状態を更新します
        /// </summary>
        private void updateSoloMute()
        {
            VsqFileEx vsq = MusicManager.getVsqFile();
            if (vsq == null) {
                return;
            }
            // マスター
            bool masterMuted = vsq.getMasterMute();
			volumeMaster.Model.Muted = masterMuted;

            // VSQのトラック
            bool soloSpecificationExists = false; // 1トラックでもソロ指定があればtrue
            for (int i = 1; i < vsq.Track.Count; i++) {
                if (vsq.getSolo(i)) {
                    soloSpecificationExists = true;
                    break;
                }
            }
            for (int track = 1; track < vsq.Track.Count; track++) {
                if (soloSpecificationExists) {
                    if (vsq.getSolo(track)) {
						m_tracker[track - 1].Model.Solo = (true);
						m_tracker[track - 1].Model.Muted = (masterMuted ? true : vsq.getMute(track));
                    } else {
						m_tracker[track - 1].Model.Solo = (false);
						m_tracker[track - 1].Model.Muted = (true);
                    }
                } else {
					m_tracker[track - 1].Model.Solo = (vsq.getSolo(track));
					m_tracker[track - 1].Model.Muted = (masterMuted ? true : vsq.getMute(track));
                }
            }

            // BGM
            int offset = vsq.Track.Count - 1;
            for (int i = 0; i < vsq.BgmFiles.Count; i++) {
				m_tracker[offset + i].Model.Muted = (masterMuted ? true : vsq.BgmFiles[i].mute == 1);
            }

            this.Refresh();
        }

        public void applyShortcut(Keys shortcut)
        {
            menuVisualReturn.ShortcutKeys = shortcut;
        }

        public void applyLanguage()
        {
            this.Text = _("Mixer");
        }

        /// <summary>
        /// 現在のシーケンスの状態に応じて，ミキサーウィンドウの状態を更新します
        /// </summary>
        public void updateStatus()
        {
            VsqFileEx vsq = MusicManager.getVsqFile();
            int num = vsq.Mixer.Slave.Count + MusicManager.getBgmCount();
            if (m_tracker == null) {
                m_tracker = new List<VolumeTracker>();
            }

            // イベントハンドラをいったん解除する
            unregisterEventHandlers();

            // trackerの総数が変化したかどうか
            bool num_changed = (m_tracker.Count != num);

            // trackerに過不足があれば数を調節
            if (m_tracker.Count < num) {
                int remain = num - m_tracker.Count;
                for (int i = 0; i < remain; i++) {
                    VolumeTracker item = new VolumeTrackerImpl();
                    item.BorderStyle = Cadencii.Gui.Toolkit.BorderStyle.FixedSingle;
                    item.Size = volumeMaster.Size;
                    m_tracker.Add(item);
                }
            } else if (m_tracker.Count > num) {
                int delete = m_tracker.Count - num;
                for (int i = 0; i < delete; i++) {
                    int indx = m_tracker.Count - 1;
                    VolumeTracker tr = m_tracker[indx];
                    m_tracker.RemoveAt(indx);
                    tr.Dispose();
                }
            }

            // 同時に表示できるVolumeTrackerの個数を計算
			int max = Cadencii.Gui.Toolkit.Screen.Instance.GetWorkingArea(this).Width;
            int bordersize = 4;// TODO: ここもともとは SystemInformation.FrameBorderSize;だった
            int max_client_width = max - 2 * bordersize;
            int max_num = (int)Math.Floor(max_client_width / (VolumeTrackerController.WIDTH + 1.0f));
            num++;

            int screen_num = num <= max_num ? num : max_num; //スクリーン上に表示するVolumeTrackerの個数

            // panelSlaves上に配置するVolumeTrackerの個数
            int num_vtracker_on_panel = vsq.Mixer.Slave.Count + MusicManager.getBgmCount();
            // panelSlaves上に一度に表示可能なVolumeTrackerの個数
            int panel_capacity = max_num - 1;

            if (panel_capacity >= num_vtracker_on_panel) {
                // volumeMaster以外の全てのVolumeTrackerを，画面上に同時表示可能
                hScroll.Minimum = 0;
                hScroll.Value = 0;
                hScroll.Maximum = 0;
                hScroll.LargeChange = 1;
				hScroll.Size = new Size((VolumeTrackerController.WIDTH + 1) * num_vtracker_on_panel, 15);
            } else {
                // num_vtracker_on_panel個のVolumeTrackerのうち，panel_capacity個しか，画面上に同時表示できない
                hScroll.Minimum = 0;
                hScroll.Value = 0;
				hScroll.Maximum = num_vtracker_on_panel * VolumeTrackerController.WIDTH;
				hScroll.LargeChange = panel_capacity * VolumeTrackerController.WIDTH;
				hScroll.Size = new Size((VolumeTrackerController.WIDTH + 1) * panel_capacity, 15);
            }
			hScroll.Location = new Point(0, VolumeTrackerController.HEIGHT);

            int j = -1;
            foreach (var vme in vsq.Mixer.Slave) {
                j++;
#if DEBUG
                Logger.StdOut("FormMixer#updateStatus; #" + j + "; feder=" + vme.Feder + "; panpot=" + vme.Panpot);
#endif
                VolumeTracker tracker = m_tracker[j];
				tracker.Model.Feder = (vme.Feder);
				tracker.Model.Panpot = (vme.Panpot);
				tracker.Model.Title = (vsq.Track[j + 1].getName());
				tracker.Model.Number = ((j + 1) + "");
				tracker.Location = new Cadencii.Gui.Point (j * (VolumeTrackerController.WIDTH + 1), 0);
				tracker.Model.SoloButtonVisible = (true);
				tracker.Model.Muted = ((vme.Mute == 1));
				tracker.Model.Solo = ((vme.Solo == 1));
				tracker.Model.Track = (j + 1);
				tracker.Model.SoloButtonVisible = (true);
                addToPanelSlaves(tracker, j);
            }
            int count = MusicManager.getBgmCount();
            for (int i = 0; i < count; i++) {
                j++;
                BgmFile item = MusicManager.getBgm(i);
                VolumeTracker tracker = m_tracker[j];
				tracker.Model.Feder = (item.feder);
				tracker.Model.Panpot = (item.panpot);
				tracker.Model.Title = (PortUtil.getFileName(item.file));
				tracker.Model.Number = ("");
				tracker.Location = new Cadencii.Gui.Point (j * (VolumeTrackerController.WIDTH + 1), 0);
				tracker.Model.SoloButtonVisible = (false);
				tracker.Model.Muted = ((item.mute == 1));
				tracker.Model.Solo = (false);
				tracker.Model.Track = (-i - 1);
				tracker.Model.SoloButtonVisible = (false);
                addToPanelSlaves(tracker, j);
            }
#if DEBUG
            Logger.StdOut("FormMixer#updateStatus; vsq.Mixer.MasterFeder=" + vsq.Mixer.MasterFeder);
#endif
			volumeMaster.Model.Feder = (vsq.Mixer.MasterFeder);
			volumeMaster.Model.Panpot = (vsq.Mixer.MasterPanpot);
			volumeMaster.Model.SoloButtonVisible = (false);

            updateSoloMute();

            // イベントハンドラを再登録
            reregisterEventHandlers();

            // ウィンドウのサイズを更新（必要なら）
            if (num_changed) {
				panelSlaves.Width = (VolumeTrackerController.WIDTH + 1) * (screen_num - 1);
				volumeMaster.Location = new Cadencii.Gui.Point((screen_num - 1) * (VolumeTrackerController.WIDTH + 1) + 3, 0);
                //this.MaximumSize = Size.Zero;
				this.MinimumSize = Cadencii.Gui.Size.Empty;
				this.ClientSize = new Size(screen_num * (VolumeTrackerController.WIDTH + 1) + 3, VolumeTrackerController.HEIGHT + hScroll.Height);
                this.MinimumSize = this.Size;
                //this.MaximumSize = this.Size;
                this.Invalidate();
            }
        }
        #endregion

        #region helper methods
        private void addToPanelSlaves(VolumeTracker item, int ix)
        {
            panelSlaves.AddControl(item);
        }

        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void unregisterEventHandlers()
        {
            int size = 0;
            if (m_tracker != null) {
                size = m_tracker.Count;
            }
            for (int i = 0; i < size; i++) {
                VolumeTracker item = m_tracker[i];
				item.Model.PanpotChanged -= new PanpotChangedEventHandler(FormMixer_PanpotChanged);
				item.Model.FederChanged -= new FederChangedEventHandler(FormMixer_FederChanged);
				item.Model.MuteButtonClick -= new EventHandler(FormMixer_MuteButtonClick);
				item.Model.SoloButtonClick -= new EventHandler(FormMixer_SoloButtonClick);
            }
			volumeMaster.Model.PanpotChanged -= new PanpotChangedEventHandler(volumeMaster_PanpotChanged);
			volumeMaster.Model.FederChanged -= new FederChangedEventHandler(volumeMaster_FederChanged);
			volumeMaster.Model.MuteButtonClick -= new EventHandler(volumeMaster_MuteButtonClick);
        }

        /// <summary>
        /// ボリューム用のイベントハンドラを再登録します
        /// </summary>
        private void reregisterEventHandlers()
        {
            int size = 0;
            if (m_tracker != null) {
                size = m_tracker.Count;
            }
            for (int i = 0; i < size; i++) {
                VolumeTracker item = m_tracker[i];
				item.Model.PanpotChanged += new PanpotChangedEventHandler(FormMixer_PanpotChanged);
				item.Model.FederChanged += new FederChangedEventHandler(FormMixer_FederChanged);
				item.Model.MuteButtonClick += new EventHandler(FormMixer_MuteButtonClick);
				item.Model.SoloButtonClick += new EventHandler(FormMixer_SoloButtonClick);
            }
			volumeMaster.Model.PanpotChanged += new PanpotChangedEventHandler(volumeMaster_PanpotChanged);
			volumeMaster.Model.FederChanged += new FederChangedEventHandler(volumeMaster_FederChanged);
			volumeMaster.Model.MuteButtonClick += new EventHandler(volumeMaster_MuteButtonClick);
        }

        private void registerEventHandlers()
        {
            menuVisualReturn.Click += new EventHandler(menuVisualReturn_Click);
            hScroll.ValueChanged += new EventHandler(veScrollBar_ValueChanged);
			this.AsGui ().FormClosing += (o,e) => FormMixer_FormClosing (e);
            this.Load += new EventHandler(FormMixer_Load);
            reregisterEventHandlers();
        }

        private void setResources()
        {
            this.Icon = cadencii.Properties.Resources.Icon1;
        }

        private void invokePanpotChangedEvent(int track, int panpot)
        {
            if (PanpotChanged != null) {
                PanpotChanged.Invoke(track, panpot);
            }
        }

        private void invokeFederChangedEvent(int track, int feder)
        {
            if (FederChanged != null) {
                FederChanged.Invoke(track, feder);
            }
        }

        private void invokeSoloChangedEvent(int track, bool solo)
        {
            if (SoloChanged != null) {
                SoloChanged.Invoke(track, solo);
            }
        }

        private void invokeMuteChangedEvent(int track, bool mute)
        {
            if (MuteChanged != null) {
                MuteChanged.Invoke(track, mute);
            }
        }
        #endregion

        #region event handlers
        public void FormMixer_Load(Object sender, EventArgs e)
        {
#if DEBUG
            Logger.StdOut("FormMixer#FormMixer_Load");
#endif
            this.TopMost = true;
        }

        public void FormMixer_PanpotChanged(int track, int panpot)
        {
            try {
                invokePanpotChangedEvent(track, panpot);
            } catch (Exception ex) {
                Logger.write(typeof(FormMixerUiImpl) + ".FormMixer_PanpotChanged; ex=" + ex + "\n");
                Logger.StdErr("FormMixer#FormMixer_PanpotChanged; ex=" + ex);
            }
        }

        public void FormMixer_FederChanged(int track, int feder)
        {
            try {
                invokeFederChangedEvent(track, feder);
            } catch (Exception ex) {
                Logger.write(typeof(FormMixerUiImpl) + ".FormMixer_FederChanged; ex=" + ex + "\n");
                Logger.StdErr("FormMixer#FormMixer_FederChanged; ex=" + ex);
            }
        }

        public void FormMixer_SoloButtonClick(Object sender, EventArgs e)
        {
            VolumeTracker parent = (VolumeTracker)sender;
			int track = parent.Model.Track;
            try {
				invokeSoloChangedEvent(track, parent.Model.Solo);
            } catch (Exception ex) {
                Logger.write(typeof(FormMixerUiImpl) + ".FormMixer_SoloButtonClick; ex=" + ex + "\n");
                Logger.StdErr("FormMixer#FormMixer_IsSoloChanged; ex=" + ex);
            }
            updateSoloMute();
        }

        public void FormMixer_MuteButtonClick(Object sender, EventArgs e)
        {
            VolumeTracker parent = (VolumeTracker)sender;
			int track = parent.Model.Track;
            try {
				invokeMuteChangedEvent(track, parent.Model.Muted);
            } catch (Exception ex) {
                Logger.write(typeof(FormMixerUiImpl) + ".FormMixer_MuteButtonClick; ex=" + ex + "\n");
                Logger.StdErr("FormMixer#FormMixer_IsMutedChanged; ex=" + ex);
            }
            updateSoloMute();
        }

        public void menuVisualReturn_Click(Object sender, EventArgs e)
        {
            this.Visible = false;
        }

		void FormMixer_FormClosing(Cadencii.Gui.Toolkit.FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }

        public void veScrollBar_ValueChanged(Object sender, EventArgs e)
        {
            int stdx = hScroll.Value;
            for (int i = 0; i < m_tracker.Count; i++) {
				m_tracker[i].Location = new Cadencii.Gui.Point (-stdx + (VolumeTrackerController.WIDTH + 1) * i, 0);
            }
            this.Invalidate();
        }

        public void volumeMaster_FederChanged(int track, int feder)
        {
            try {
                invokeFederChangedEvent(0, feder);
            } catch (Exception ex) {
                Logger.write(typeof(FormMixerUiImpl) + ".volumeMaster_FederChanged; ex=" + ex + "\n");
                Logger.StdErr("FormMixer#volumeMaster_FederChanged; ex=" + ex);
            }
        }

        public void volumeMaster_PanpotChanged(int track, int panpot)
        {
            try {
                invokePanpotChangedEvent(0, panpot);
            } catch (Exception ex) {
                Logger.write(typeof(FormMixerUiImpl) + ".volumeMaster_PanpotChanged; ex=" + ex + "\n");
                Logger.StdErr("FormMixer#volumeMaster_PanpotChanged; ex=" + ex);
            }
        }

        public void volumeMaster_MuteButtonClick(Object sender, EventArgs e)
        {
            try {
				invokeMuteChangedEvent(0, volumeMaster.Model.Muted);
            } catch (Exception ex) {
                Logger.write(typeof(FormMixerUiImpl) + ".volumeMaster_MuteButtonClick; ex=" + ex + "\n");
                Logger.StdErr("FormMixer#volumeMaster_IsMutedChanged; ex=" + ex);
            }
        }
        #endregion

        #region UI implementation
        #region UI Impl for C#
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "FormMixerUi.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		#pragma warning disable 0169,0649
        private UiMenuStrip menuMain;
        private UiToolStripMenuItem menuVisual;
        private UiToolStripMenuItem menuVisualReturn;
        private VolumeTracker volumeMaster;
        private UiPanel panelSlaves;
        private UiHScrollBar hScroll;
		#pragma warning restore 0169,0649
        #endregion
        #endregion

    }

}
