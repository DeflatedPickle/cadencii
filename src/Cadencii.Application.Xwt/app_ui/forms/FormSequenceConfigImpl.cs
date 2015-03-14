/*
 * FormSequenceConfig.cs
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
using System;
using cadencii.apputil;
using Cadencii.Gui;
using Cadencii.Media;
using Cadencii.Media.Vsq;
using cadencii.core;
using Cadencii.Gui.Toolkit;
using cadencii;



namespace Cadencii.Application.Forms
{

	class FormSequenceConfigImpl : FormImpl, FormSequenceConfig
    {
        public FormSequenceConfigImpl()
        {
            InitializeComponent();
            applyLanguage();

            // wave channel
            comboChannel.Items.Clear();
            comboChannel.Items.Add(_("Monoral"));
            comboChannel.Items.Add(_("Stereo"));

            // sample rate
            comboSampleRate.Items.Clear();
            comboSampleRate.Items.Add("44100");
            comboSampleRate.Items.Add("48000");
            comboSampleRate.Items.Add("96000");
            comboSampleRate.SelectedIndex = 0;

            // pre-measure
            comboPreMeasure.Items.Clear();
			for (int i = ApplicationGlobal.MIN_PRE_MEASURE; i <= ApplicationGlobal.MAX_PRE_MEASURE; i++) {
                comboPreMeasure.Items.Add(i);
            }

            registerEventHandlers();
            setResources();
            GuiHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
        }

        #region public methods
        public void applyLanguage()
        {
            this.Text = _("Sequence config");
            btnCancel.Text = _("Cancel");
            btnOK.Text = _("OK");

            groupWaveFileOutput.Text = _("Wave File Output");
            lblChannel.Text = _("Channel");
            lblChannel.Mnemonic(Keys.C);
            labelSampleRate.Text = _("Sample rate");
            labelSampleRate.Mnemonic(Keys.S);
            radioMasterTrack.Text = _("Master Track");
            radioCurrentTrack.Text = _("Current Track");
            labelSampleRate.Text = _("Sample rate");

            int current_index = comboChannel.SelectedIndex;
            comboChannel.Items.Clear();
            comboChannel.Items.Add(_("Monoral"));
            comboChannel.Items.Add(_("Stereo"));
            comboChannel.SelectedIndex = current_index;

            groupSequence.Text = _("Sequence");
            labelPreMeasure.Text = _("Pre-measure");
        }

        /// <summary>
        /// プリメジャーの設定値を取得します
        /// </summary>
        /// <returns></returns>
        public int getPreMeasure()
        {
            int indx = comboPreMeasure.SelectedIndex;
            int ret = 1;
            if (indx >= 0) {
                ret = ApplicationGlobal.MIN_PRE_MEASURE + indx;
            } else {
                string s = comboPreMeasure.Text;
                try {
                    ret = int.Parse(s);
                } catch (Exception ex) {
                    ret = ApplicationGlobal.MIN_PRE_MEASURE;
                }
            }
            if (ret < ApplicationGlobal.MIN_PRE_MEASURE) {
                ret = ApplicationGlobal.MIN_PRE_MEASURE;
            }
            if (ApplicationGlobal.MAX_PRE_MEASURE < ret) {
                ret = ApplicationGlobal.MAX_PRE_MEASURE;
            }
            return ret;
        }

        /// <summary>
        /// プリメジャーの設定値を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setPreMeasure(int value)
        {
            int indx = value - ApplicationGlobal.MIN_PRE_MEASURE;
            if (indx < 0) {
                indx = 0;
            }
            if (comboPreMeasure.Items.Count <= indx) {
                indx = comboPreMeasure.Items.Count - 1;
            }
            comboPreMeasure.SelectedIndex = indx;
        }

        /// <summary>
        /// サンプリングレートの設定値を取得します
        /// </summary>
        /// <returns></returns>
        public int getSampleRate()
        {
            int index = comboSampleRate.SelectedIndex;
            string s = "44100";
            if (index >= 0) {
                s = (string)comboSampleRate.Items[index];
            } else {
                s = comboSampleRate.Text;
            }
            int ret = 44100;
            try {
                ret = int.Parse(s);
            } catch (Exception ex) {
                ret = 44100;
            }
            return ret;
        }

        /// <summary>
        /// サンプリングレートの設定値を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setSampleRate(int value)
        {
            comboSampleRate.SelectedIndex = 0;
            for (int i = 0; i < comboSampleRate.Items.Count; i++) {
                string s = (string)comboSampleRate.Items[i];
                int rate = 0;
                try {
                    rate = int.Parse(s);
                } catch (Exception ex) {
                    rate = 0;
                }
                if (rate == value) {
                    comboSampleRate.SelectedIndex = i;
                    break;
                }
            }
        }

        public bool isWaveFileOutputFromMasterTrack()
        {
            return radioMasterTrack.Checked;
        }

        public void setWaveFileOutputFromMasterTrack(bool value)
        {
            radioMasterTrack.Checked = value;
            radioCurrentTrack.Checked = !value;
        }

        public int getWaveFileOutputChannel()
        {
            if (comboChannel.SelectedIndex <= 0) {
                return 1;
            } else {
                return 2;
            }
        }

        public void setWaveFileOutputChannel(int value)
        {
            if (value == 1) {
                comboChannel.SelectedIndex = 0;
            } else {
                comboChannel.SelectedIndex = 1;
            }
        }
        #endregion

        #region event handlers
        public void btnOK_Click(Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region helper methods
        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void registerEventHandlers()
        {
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        private void setResources()
        {
        }
        #endregion

        #region ui implementation
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
			ApplicationUIHost.Instance.ApplyXml (this, "FormSequenceConfig.xml");
            this.ResumeLayout(false);

        }

        #endregion

		#pragma warning disable 0169,0649
        UiButton btnCancel;
        UiButton btnOK;
        UiLabel lblChannel;
        UiComboBox comboChannel;
        UiGroupBox groupWaveFileOutput;
        UiRadioButton radioCurrentTrack;
        UiRadioButton radioMasterTrack;
        UiLabel labelSampleRate;
        UiGroupBox groupSequence;
        UiLabel labelPreMeasure;
        UiComboBox comboPreMeasure;
        UiComboBox comboSampleRate;
		#pragma warning restore 0169,0649

        #endregion

    }

}
