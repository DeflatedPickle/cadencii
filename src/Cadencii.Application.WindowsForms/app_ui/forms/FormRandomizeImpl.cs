/*
 * FormRandomize.cs
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
using System;
using Cadencii.Media.Vsq;
using cadencii.apputil;

using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;
using Cadencii.Application.Media;



namespace Cadencii.Application.Forms
{

    public class FormRandomizeImpl : FormImpl, FormRandomize
    {
        private static bool lastPositionRandomizeEnabled = true;
        private static int lastPositionRandomizeValue = 3;
        private static bool lastPitRandomizeEnabled = true;
        private static int lastResolution = 5;
        private static int lastPitRandomizeValue = 3;
        private static int lastPitRandomizePattern = 1;
        private static int lastStartBar = 1;
        private static int lastStartBeat = 1;
        private static int lastEndBar = 2;
        private static int lastEndBeat = 1;
        /// <summary>
        /// trueなら、numStartBar, numStartBeat, numEndBar, numEndBeatの値が変更されたときに、イベントハンドラを起動しない
        /// </summary>
        private bool lockRequired = false;

        public FormRandomizeImpl()
        {
            InitializeComponent();
            registerEventHandlers();
            applyLanguage();

            comboShiftValue.Items.Clear();
            string[] shift_items = new string[]{
                "1(small)",
                "2",
                "3(medium)",
                "4",
                "5(large)"};
            for (int i = 0; i < shift_items.Length; i++) {
                comboShiftValue.Items.Add(shift_items[i]);
            }

            comboPitPattern.Items.Clear();
            string[] pit_pat_items = new string[]{
                "Pattern 1",
                "Pattern 2",
                "Pattern 3"};
            for (int i = 0; i < pit_pat_items.Length; i++) {
                comboPitPattern.Items.Add(pit_pat_items[i]);
            }

            comboPitValue.Items.Clear();
            string[] pit_value_items = new string[]{
                "1(small)",
                "2",
                "3(medium)",
                "4",
                "5(large)"};
            for (int i = 0; i < pit_value_items.Length; i++) {
                comboPitValue.Items.Add(pit_value_items[i]);
            }

            chkShift.Checked = lastPositionRandomizeEnabled;
            comboShiftValue.SelectedIndex = lastPositionRandomizeValue - 1;
            chkPit.Checked = lastPitRandomizeEnabled;
            numResolution.Value = lastResolution;
            comboPitPattern.SelectedIndex = lastPitRandomizePattern - 1;
            comboPitValue.SelectedIndex = lastPitRandomizeValue - 1;
            lockRequired = true;
            numStartBar.Value = lastStartBar;
            numStartBeat.Value = lastStartBeat;
            numEndBar.Value = lastEndBar;
            numEndBeat.Value = lastEndBeat;
            lockRequired = false;
            AwtHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
        }

        #region helper methods
        /// <summary>
        /// numStartBar, numStartBeat, numEndBar, numEndBeatの値の範囲の妥当性をチェックする
        /// </summary>
        private void validateNumRange()
        {
            int startBar = getStartBar();
            int startBeat = getStartBeat();
            int endBar = getEndBar();
            int endBeat = getEndBeat();
            VsqFileEx vsq = MusicManager.getVsqFile();
            if (vsq == null) {
                return;
            }

            int preMeasure = vsq.getPreMeasure();
            startBar += (preMeasure - 1); // 曲頭からの小節数は、表示上の小節数と(preMeasure - 1)だけずれているので。
            endBar += (preMeasure - 1);
            startBeat--;
            endBeat--;

            int startBarClock = vsq.getClockFromBarCount(startBar); // startBar小節開始位置のゲートタイム
            Timesig startTimesig = vsq.getTimesigAt(startBarClock);    // startBar小節開始位置の拍子
            int startClock = startBarClock + startBeat * 480 * 4 / startTimesig.denominator;  // 第startBar小節の第startBeat拍開始位置のゲートタイム

            int endBarClock = vsq.getClockFromBarCount(endBar);
            Timesig endTimesig = vsq.getTimesigAt(endBarClock);
            int endClock = endBarClock + endBeat * 480 * 4 / endTimesig.denominator;

            if (endClock <= startClock) {
                // 選択範囲が0以下の場合、値を強制的に変更する
                // ここでは、一拍分を選択するように変更
                endClock = startClock + 480 * 4 / startTimesig.denominator;
                endBar = vsq.getBarCountFromClock(endClock);
                int remain = endClock - vsq.getClockFromBarCount(endBar);
                endTimesig = vsq.getTimesigAt(endClock);
                endBeat = remain / (480 * 4 / endTimesig.denominator);
            }

            // numStartBarの最大値・最小値を決定
            int startBarMax = endBar - 1;
            if (startBeat < endBeat) {
                startBarMax = endBar;
            }
            int startBarMin = 1;

            // numStartBeatの最大値・最小値を決定
            int startBeatMax = startTimesig.numerator;
            if (startBar == endBar) {
                startBeatMax = endBeat - 1;
            }
            int startBeatMin = 1;

            // numEndBarの最大値・最小値を決定
            int endBarMax = int.MaxValue;
            int endBarMin = startBar + 1;
            if (startBeat < endBeat) {
                endBarMin = startBar;
            }

            // numEndBeatの最大値・最小値の決定
            int endBeatMax = endTimesig.numerator;
            int endBeatMin = 1;
            if (startBar == endBar) {
                endBeatMin = startBeat + 1;
            }

            lockRequired = true;
            numStartBar.Maximum = startBarMax;
            numStartBar.Minimum = startBarMin;
            numStartBeat.Maximum = startBeatMax;
            numStartBeat.Minimum = startBeatMin;
            numEndBar.Maximum = endBarMax;
            numEndBar.Minimum = endBarMin;
            numEndBeat.Maximum = endBeatMax;
            numEndBeat.Minimum = endBeatMin;
            lockRequired = false;
        }

        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void registerEventHandlers()
        {
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            numStartBar.ValueChanged += new EventHandler(numCommon_ValueChanged);
            numStartBeat.ValueChanged += new EventHandler(numCommon_ValueChanged);
            numEndBar.ValueChanged += new EventHandler(numCommon_ValueChanged);
            numEndBeat.ValueChanged += new EventHandler(numCommon_ValueChanged);
            chkShift.CheckedChanged += new EventHandler(chkShift_CheckedChanged);
            chkPit.CheckedChanged += new EventHandler(chkPit_CheckedChanged);
        }
        #endregion

        #region event handlers
        public void chkShift_CheckedChanged(Object sender, EventArgs e)
        {
            bool v = chkShift.Checked;
            comboShiftValue.Enabled = v;
        }

        public void chkPit_CheckedChanged(Object sender, EventArgs e)
        {
            bool v = chkPit.Checked;
            numResolution.Enabled = v;
            comboPitPattern.Enabled = v;
            comboPitValue.Enabled = v;
        }

        public void numCommon_ValueChanged(Object sender, EventArgs e)
        {
            if (lockRequired) {
                return;
            }
            validateNumRange();
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
			this.AsAwt ().DialogResult = Cadencii.Gui.DialogResult.Cancel;
        }

        public void btnOK_Click(Object sender, EventArgs e)
        {
            lastPositionRandomizeEnabled = isPositionRandomizeEnabled();
            lastPositionRandomizeValue = getPositionRandomizeValue();
            lastPitRandomizeEnabled = isPitRandomizeEnabled();
            lastPitRandomizePattern = getPitRandomizePattern();
            lastPitRandomizeValue = getPitRandomizeValue();
            lastResolution = getResolution();
            lastStartBar = getStartBar();
            lastStartBeat = getStartBeat();
            lastEndBar = getEndBar();
            lastEndBeat = getEndBeat();
			this.AsAwt ().DialogResult = Cadencii.Gui.DialogResult.OK;
        }
        #endregion

        #region public methods
        public int getResolution()
        {
            return (int)numResolution.Value;
        }

        public int getStartBar()
        {
            return (int)numStartBar.Value;
        }

        public int getStartBeat()
        {
            return (int)numStartBeat.Value;
        }

        public int getEndBar()
        {
            return (int)numEndBar.Value;
        }

        public int getEndBeat()
        {
            return (int)numEndBeat.Value;
        }

        public bool isPositionRandomizeEnabled()
        {
            return chkShift.Checked;
        }

        public int getPositionRandomizeValue()
        {
            int draft = comboShiftValue.SelectedIndex + 1;
            if (draft <= 0) {
                draft = 1;
            }
            return draft;
        }

        public bool isPitRandomizeEnabled()
        {
            return chkPit.Checked;
        }

        public int getPitRandomizeValue()
        {
            int draft = comboPitValue.SelectedIndex + 1;
            if (draft <= 0) {
                draft = 1;
            }
            return draft;
        }

        public int getPitRandomizePattern()
        {
            int draft = comboPitPattern.SelectedIndex + 1;
            if (draft <= 0) {
                draft = 1;
            }
            return draft;
        }

        public void applyLanguage()
        {
            lblStart.Text = _("Start");
            lblStartBar.Text = _("bar");
            lblStartBeat.Text = _("beat");
            lblEnd.Text = _("End");
            lblEndBar.Text = _("bar");
            lblEndBeat.Text = _("beat");

            chkShift.Text = _("Note Shift");
            lblShiftValue.Text = _("Value");

            chkPit.Text = _("Pitch Fluctuation");
            lblResolution.Text = _("Resolution");
            lblPitPattern.Text = _("Pattern");
            lblPitValue.Text = _("Value");

            btnOK.Text = _("OK");
            btnCancel.Text = _("Cancel");

            this.Text = _("Randomize");
        }
        #endregion

        #region UI implementation
        #region UI impl for C#
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
			ApplicationUIHost.Instance.ApplyXml (this, "FormRandomize.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

		#pragma warning disable 0649
        NumericUpDownEx numStartBar;
        UiLabel lblStart;
        UiLabel lblStartBar;
        NumericUpDownEx numStartBeat;
        UiLabel lblStartBeat;
        UiLabel bLabel1;
        UiLabel lblEndBeat;
        NumericUpDownEx numEndBeat;
        UiLabel lblEndBar;
        UiLabel lblEnd;
        NumericUpDownEx numEndBar;
        UiCheckBox chkShift;
        UiLabel lblShiftValue;
        UiComboBox comboShiftValue;
        UiComboBox comboPitPattern;
        UiLabel lblPitPattern;
        UiCheckBox chkPit;
        UiComboBox comboPitValue;
        UiLabel lblPitValue;
        UiLabel lblResolution;
        NumericUpDownEx numResolution;
        UiButton btnCancel;
        UiButton btnOK;
		#pragma warning restore 0169,0649

        #endregion
        #endregion

    }

}
