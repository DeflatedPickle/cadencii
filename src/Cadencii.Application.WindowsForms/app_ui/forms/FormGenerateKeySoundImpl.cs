/*
 * FormGenerateKeySound.cs
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
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using cadencii.java.util;
using Cadencii.Media;
using Cadencii.Media.Vsq;

using cadencii.core;
using Cadencii.Utilities;
using cadencii;
using Cadencii.Gui.Toolkit;
using Cadencii.Gui;


namespace Cadencii.Application.Forms
{

    public class FormGenerateKeySoundImpl : FormImpl
    {
        private delegate void updateTitleDelegate(string title);

        const int _SAMPLE_RATE = 44100;

        private FolderBrowserDialog folderBrowser;
        private System.ComponentModel.BackgroundWorker bgWork;
        private SingerConfig[] m_singer_config1;
        private SingerConfig[] m_singer_config2;
        private SingerConfig[] m_singer_config_utau;
        private bool m_cancel_required = false;
        /// <summary>
        /// 処理が終わったら自動でフォームを閉じるかどうか。デフォルトではfalse（閉じない）
        /// </summary>
        private bool m_close_when_finished = false;

        public FormGenerateKeySoundImpl(bool close_when_finished)
        {
            InitializeComponent();
            bgWork = new System.ComponentModel.BackgroundWorker();
            bgWork.WorkerReportsProgress = true;
            bgWork.WorkerSupportsCancellation = true;
            folderBrowser = new FolderBrowserDialog();

            m_close_when_finished = close_when_finished;
            m_singer_config1 = VocaloSysUtil.getSingerConfigs(SynthesizerType.VOCALOID1);
            m_singer_config2 = VocaloSysUtil.getSingerConfigs(SynthesizerType.VOCALOID2);
            m_singer_config_utau = ApplicationGlobal.appConfig.UtauSingers.ToArray();
            if (m_singer_config1.Length > 0) {
                comboSingingSynthSystem.Items.Add("VOCALOID1");
            }
            if (m_singer_config2.Length > 0) {
                comboSingingSynthSystem.Items.Add("VOCALOID2");
            }

            // 取りあえず最初に登録されているresamplerを使うってことで
			string resampler = ApplicationGlobal.appConfig.getResamplerAt(0);
            if (m_singer_config_utau.Length > 0 &&
                 ApplicationGlobal.appConfig.PathWavtool != null && File.Exists(ApplicationGlobal.appConfig.PathWavtool) &&
                 resampler != null && File.Exists(resampler)) {
                comboSingingSynthSystem.Items.Add("UTAU");
            }
            if (comboSingingSynthSystem.Items.Count > 0) {
                comboSingingSynthSystem.SelectedIndex = 0;
            }
            updateSinger();
			txtDir.Text = ApplicationGlobal.getKeySoundPath();

            registerEventHandlers();
        }

        #region helper methods
        private void registerEventHandlers()
        {

			btnExecute.Click += new EventHandler (btnExecute_Click);
			btnCancel.Click += new EventHandler (btnCancel_Click);
			comboSingingSynthSystem.SelectedIndexChanged += new EventHandler (comboSingingSynthSystem_SelectedIndexChanged);
			btnBrowse.Click += new EventHandler(btnBrowse_Click);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Program_FormClosed);
            bgWork.DoWork += new DoWorkEventHandler(bgWork_DoWork);
            bgWork.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWork_RunWorkerCompleted);
            bgWork.ProgressChanged += new ProgressChangedEventHandler(bgWork_ProgressChanged);
        }

        private void updateSinger()
        {
            if (comboSingingSynthSystem.SelectedIndex < 0) {
                return;
            }
            string singer = (string)comboSingingSynthSystem.SelectedItem;
            SingerConfig[] list = null;
            if (singer.Equals("VOCALOID1")) {
                list = m_singer_config1;
            } else if (singer.Equals("VOCALOID2")) {
                list = m_singer_config2;
            } else if (singer.Equals("UTAU")) {
                list = m_singer_config_utau;
            }
            comboSinger.Items.Clear();
            if (list == null) {
                return;
            }
            for (int i = 0; i < list.Length; i++) {
                comboSinger.Items.Add(list[i].VOICENAME);
            }
            if (comboSinger.Items.Count > 0) {
                comboSinger.SelectedIndex = 0;
            }
        }

        private void updateTitle(string title)
        {
            this.Text = title;
        }

        private void updateEnabled(bool enabled)
        {
            comboSinger.Enabled = enabled;
            comboSingingSynthSystem.Enabled = enabled;
            txtDir.ReadOnly = !enabled;
            btnBrowse.Enabled = enabled;
            btnExecute.Enabled = enabled;
            chkIgnoreExistingWavs.Enabled = enabled;
            if (enabled) {
                btnCancel.Text = "Close";
            } else {
                btnCancel.Text = "Cancel";
            }
        }
        #endregion

        #region event handlers
        public void comboSingingSynthSystem_SelectedIndexChanged(Object sender, EventArgs e)
        {
            updateSinger();
        }

        public void btnBrowse_Click(Object sender, EventArgs e)
        {
            folderBrowser.SelectedPath = txtDir.Text;
            if (folderBrowser.ShowDialog(this) != DialogResult.OK) {
                return;
            }
            txtDir.Text = folderBrowser.SelectedPath;
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
            if (bgWork.IsBusy) {
                m_cancel_required = true;
                while (m_cancel_required) {
					AwtHost.Current.ApplicationDoEvents();
                }
            } else {
                this.Close();
            }
        }

        public void btnExecute_Click(Object sender, EventArgs e)
        {
            PrepareStartArgument arg = new PrepareStartArgument();
            arg.singer = (string)comboSinger.SelectedItem;
            arg.amplitude = 1.0;
            arg.directory = txtDir.Text;
            arg.replace = chkIgnoreExistingWavs.Checked;
            updateEnabled(false);
            bgWork.RunWorkerAsync(arg);
        }

        public void bgWork_DoWork(Object sender, DoWorkEventArgs e)
        {
#if DEBUG
            Logger.StdOut("FormGenerateKeySound#bgWork_DoWork");
#endif
            PrepareStartArgument arg = (PrepareStartArgument)e.Argument;
            string singer = arg.singer;
            double amp = arg.amplitude;
            string dir = arg.directory;
            bool replace = arg.replace;
            // 音源を準備
            if (!Directory.Exists(dir)) {
                PortUtil.createDirectory(dir);
            }

            for (int i = 0; i < 127; i++) {
                string path = Path.Combine(dir, i + ".wav");
                Logger.StdOut("writing \"" + path + "\" ...");
                if (replace || (!replace && !File.Exists(path))) {
                    try {
                        FormGenerateKeySoundStatic.GenerateSinglePhone(i, singer, path, amp);
                        if (File.Exists(path)) {
                            try {
                                Wave wv = new Wave(path);
                                wv.trimSilence();
                                wv.monoralize();
                                wv.write(path);
                            } catch (Exception ex0) {
                                Logger.StdErr("FormGenerateKeySound#bgWork_DoWork; ex0=" + ex0);
                                Logger.write(typeof(FormGenerateKeySound) + ".bgWork_DoWork; ex=" + ex0 + "\n");
                            }
                        }
                    } catch (Exception ex) {
                        Logger.write(typeof(FormGenerateKeySound) + ".bgWork_DoWork; ex=" + ex + "\n");
                        Logger.StdErr("FormGenerateKeySound#bgWork_DoWork; ex=" + ex);
                    }
                }
                Logger.StdOut(" done");
                if (m_cancel_required) {
                    m_cancel_required = false;
                    break;
                }
                bgWork.ReportProgress((int)(i / 127.0 * 100.0), null);
            }
            m_cancel_required = false;
        }

        private void bgWork_ProgressChanged(Object sender, ProgressChangedEventArgs e)
        {
            string title = "Progress: " + e.ProgressPercentage + "%";
            this.Invoke(new updateTitleDelegate(this.updateTitle), new Object[] { title });
        }

        public void Program_FormClosed(Object sender, FormClosedEventArgs e)
        {
            VSTiDllManager.terminate();
        }

        public void bgWork_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            updateEnabled(true);
            if (m_close_when_finished) {
                Close();
            }
        }
        #endregion

        #region UI implementation
        private void InitializeComponent()
        {
			ApplicationUIHost.Instance.ApplyXml (this, "FormGenerateKeySound.xml");
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox comboSingingSynthSystem;
        private Label lblSingingSynthSystem;
        private Label lblSinger;
        private System.Windows.Forms.ComboBox comboSinger;
        private CheckBox chkIgnoreExistingWavs;
        private TextBox txtDir;
        private System.Windows.Forms.Button btnBrowse;
        private Label lblDir;

        #endregion

    }

}
