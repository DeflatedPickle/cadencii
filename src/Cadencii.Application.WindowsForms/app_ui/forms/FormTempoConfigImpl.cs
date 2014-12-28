/*
 * FormTempoConfig.cs
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
using cadencii.apputil;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;
using cadencii;


namespace Cadencii.Application.Forms
{

    class FormTempoConfigImpl : FormImpl, FormTempoConfig
    {
        public FormTempoConfigImpl(int bar_count, int beat, int beat_max, int clock, int clock_max, float tempo, int pre_measure)
        {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            applyLanguage();
            numBar.Minimum = -pre_measure + 1;
            numBar.Maximum = 100000;
            numBar.Value = bar_count;

            numBeat.Minimum = 1;
            numBeat.Maximum = beat_max;
            numBeat.Value = beat;
            numClock.Minimum = 0;
            numClock.Maximum = clock_max;
            numClock.Value = clock;
            numTempo.Value = (decimal)tempo;
            GuiHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
        }

        #region public methods
        public void applyLanguage()
        {
            this.Text = _("Global Tempo");
            groupPosition.Text = _("Position");
            lblBar.Text = _("Measure");
            lblBar.Mnemonic(Keys.M);
            lblBeat.Text = _("Beat");
            lblBeat.Mnemonic(Keys.B);
            lblClock.Text = _("Clock");
            lblClock.Mnemonic(Keys.L);
            groupTempo.Text = _("Tempo");
            btnOK.Text = _("OK");
            btnCancel.Text = _("Cancel");
        }

        public int getBeatCount()
        {
            return (int)numBeat.Value;
        }

        public int getClock()
        {
            return (int)numClock.Value;
        }

        public float getTempo()
        {
            return (float)numTempo.Value;
        }
        #endregion

        #region event handlers
        public void btnOK_Click(Object sender, EventArgs e)
        {
			this.AsGui ().DialogResult = Cadencii.Gui.DialogResult.OK;
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
			this.AsGui ().DialogResult = Cadencii.Gui.DialogResult.Cancel;
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
        protected void Dispose(bool disposing)
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
			ApplicationUIHost.Instance.ApplyXml (this, "FormTempoConfig.xml");
            this.ResumeLayout(false);

        }

        #endregion

        UiGroupBox groupPosition;
        UiLabel lblClock;
        UiLabel lblBeat;
        UiLabel lblBar;
        UiGroupBox groupTempo;
        UiButton btnOK;
        UiButton btnCancel;
        NumericUpDownEx numBar;
        NumericUpDownEx numClock;
        NumericUpDownEx numBeat;
        UiLabel lblTempoRange;
        NumericUpDownEx numTempo;
        #endregion

    }

}
