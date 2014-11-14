/*
 * FormCurvePointEdit.cs
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
using System.Collections.Generic;
using cadencii.apputil;
using Cadencii.Media.Vsq;
using cadencii;

using cadencii.java.util;
using Cadencii.Utilities;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;
using Cadencii.Application.Media;

namespace Cadencii.Application.Forms
{

    public class FormCurvePointEditImpl : FormImpl, FormCurvePointEdit
    {
        private long m_editing_id = -1;
        private CurveType m_curve;
        private bool m_changed = false;
        private FormMain mMainWindow = null;

        public FormCurvePointEditImpl(FormMain main_window, long editing_id, CurveType curve)
        {
            InitializeComponent();
            mMainWindow = main_window;
            registerEventHandlers();
            setResources();
            applyLanguage();
            m_editing_id = editing_id;
            m_curve = curve;

            VsqBPPairSearchContext context = MusicManager.getVsqFile().Track[EditorManager.Selected].getCurve(m_curve.getName()).findElement(m_editing_id);
            txtDataPointClock.Text = context.clock + "";
            txtDataPointValue.Text = context.point.value + "";
            txtDataPointValue.SelectAll();

            btnUndo.Enabled = EditorManager.editHistory.hasUndoHistory();
            btnRedo.Enabled = EditorManager.editHistory.hasRedoHistory();
        }

        #region public methods
        public void applyLanguage()
        {
            this.Text = _("Edit Value");
            lblDataPointClock.Text = _("Clock");
            lblDataPointValue.Text = _("Value");
            btnApply.Text = _("Apply");
            btnExit.Text = _("Exit");
        }
        #endregion

        #region helper methods
        private string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void applyValue(bool mode_clock)
        {
            if (!m_changed) {
                return;
            }
            int value = m_curve.getDefault();
            try {
                value = int.Parse(txtDataPointValue.Text);
            } catch (Exception ex) {
                Logger.write(typeof(FormCurvePointEdit) + ".applyValue; ex=" + ex + "\n");
                return;
            }
            if (value < m_curve.getMinimum()) {
                value = m_curve.getMinimum();
            } else if (m_curve.getMaximum() < value) {
                value = m_curve.getMaximum();
            }

            int clock = 0;
            try {
                clock = int.Parse(txtDataPointClock.Text);
            } catch (Exception ex) {
                Logger.write(typeof(FormCurvePointEdit) + ".applyValue; ex=" + ex + "\n");
                return;
            }

            int selected = EditorManager.Selected;
            VsqTrack vsq_track = MusicManager.getVsqFile().Track[selected];
            VsqBPList src = vsq_track.getCurve(m_curve.getName());
            VsqBPList list = (VsqBPList)src.clone();

            VsqBPPairSearchContext context = list.findElement(m_editing_id);
            list.move(context.clock, clock, value);
            CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandTrackCurveReplace(selected,
                                                                                                    m_curve.getName(),
                                                                                                    list));
            EditedZone zone = new EditedZone();
            Utility.compareList(zone, new VsqBPListComparisonContext(list, src));
            List<EditedZoneUnit> zoneUnits = new List<EditedZoneUnit>();
            foreach (var item in zone.iterator()) {
                zoneUnits.Add(item);
            }
            EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));

            txtDataPointClock.Text = clock + "";
            txtDataPointValue.Text = value + "";

            if (mMainWindow != null) {
                mMainWindow.setEdited(true);
                mMainWindow.Model.EnsureClockVisibleOnPianoRoll(clock);
                mMainWindow.refreshScreen();
            }

            if (mode_clock) {
                txtDataPointClock.SelectAll();
            } else {
                txtDataPointValue.SelectAll();
            }

            btnUndo.Enabled = EditorManager.editHistory.hasUndoHistory();
            btnRedo.Enabled = EditorManager.editHistory.hasRedoHistory();
            m_changed = false;
        }


        private void setResources()
        {
        }

        private void registerEventHandlers()
        {
            btnForward.Click += new EventHandler(commonButton_Click);
            btnBackward.Click += new EventHandler(commonButton_Click);
            btnBackward2.Click += new EventHandler(commonButton_Click);
            btnForward2.Click += new EventHandler(commonButton_Click);
            btnApply.Click += new EventHandler(btnApply_Click);
            txtDataPointClock.TextChanged += new EventHandler(commonTextBox_TextChanged);
            txtDataPointClock.KeyUp += commonTextBox_KeyUp;
            txtDataPointValue.TextChanged += new EventHandler(commonTextBox_TextChanged);
			txtDataPointValue.KeyUp += commonTextBox_KeyUp;
            btnBackward3.Click += new EventHandler(commonButton_Click);
            btnForward3.Click += new EventHandler(commonButton_Click);
            btnUndo.Click += new EventHandler(handleUndoRedo_Click);
            btnRedo.Click += new EventHandler(handleUndoRedo_Click);
            btnExit.Click += new EventHandler(btnExit_Click);
        }
        #endregion

        #region event handlers
        public void commonTextBox_KeyUp(Object sender, Cadencii.Gui.Toolkit.KeyEventArgs e)
        {
			if ((e.KeyCode & Cadencii.Gui.Toolkit.Keys.Enter) != Cadencii.Gui.Toolkit.Keys.Enter) {
                return;
            }
            applyValue((sender == txtDataPointClock));
        }

        public void commonButton_Click(Object sender, EventArgs e)
        {
            VsqBPList list = MusicManager.getVsqFile().Track[EditorManager.Selected].getCurve(m_curve.getName());
            VsqBPPairSearchContext search = list.findElement(m_editing_id);
            int index = search.index;
            if (sender == btnForward) {
                index++;
            } else if (sender == btnBackward) {
                index--;
            } else if (sender == btnBackward2) {
                index -= 5;
            } else if (sender == btnForward2) {
                index += 5;
            } else if (sender == btnForward3) {
                index += 10;
            } else if (sender == btnBackward3) {
                index -= 10;
            }

            if (index < 0) {
                index = 0;
            }

            if (list.size() <= index) {
                index = list.size() - 1;
            }

            VsqBPPair bp = list.getElementB(index);
            m_editing_id = bp.id;
            int clock = list.getKeyClock(index);
            txtDataPointClock.TextChanged -= commonTextBox_TextChanged;
            txtDataPointValue.TextChanged -= commonTextBox_TextChanged;
            txtDataPointClock.Text = clock + "";
            txtDataPointValue.Text = bp.value + "";
            txtDataPointClock.TextChanged += commonTextBox_TextChanged;
            txtDataPointValue.TextChanged += commonTextBox_TextChanged;

            txtDataPointValue.Focus();
            txtDataPointValue.SelectAll();

            EditorManager.itemSelection.clearPoint();
            EditorManager.itemSelection.addPoint(m_curve, bp.id);
            if (mMainWindow != null) {
                mMainWindow.Model.EnsureClockVisibleOnPianoRoll(clock);
                mMainWindow.refreshScreen();
            }
        }

        public void btnApply_Click(Object sender, EventArgs e)
        {
            applyValue(true);
        }

        public void commonTextBox_TextChanged(Object sender, EventArgs e)
        {
            m_changed = true;
        }

        public void handleUndoRedo_Click(Object sender, EventArgs e)
        {
            if (sender == btnUndo) {
                EditorManager.undo();
            } else if (sender == btnRedo) {
                EditorManager.redo();
            } else {
                return;
            }
            VsqFileEx vsq = MusicManager.getVsqFile();
            bool exists = false;
            if (vsq != null) {
                exists = vsq.Track[EditorManager.Selected].getCurve(m_curve.getName()).findElement(m_editing_id).index >= 0;
            }
#if DEBUG
            Logger.StdOut("FormCurvePointEdit#handleUndoRedo_Click; exists=" + exists);
#endif
            txtDataPointClock.Enabled = exists;
            txtDataPointValue.Enabled = exists;
            btnApply.Enabled = exists;
            btnBackward.Enabled = exists;
            btnBackward2.Enabled = exists;
            btnBackward3.Enabled = exists;
            btnForward.Enabled = exists;
            btnForward2.Enabled = exists;
            btnForward3.Enabled = exists;

            if (exists) {
                EditorManager.itemSelection.clearPoint();
                EditorManager.itemSelection.addPoint(m_curve, m_editing_id);
            }

            if (mMainWindow != null) {
                mMainWindow.updateDrawObjectList();
                mMainWindow.refreshScreen();
            }
            btnUndo.Enabled = EditorManager.editHistory.hasUndoHistory();
            btnRedo.Enabled = EditorManager.editHistory.hasRedoHistory();
        }

        public void btnExit_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        #endregion

        #region UI implementation
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

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "FormCurvePointEdit.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Button btnBackward;
        private Label lblDataPointValue;
        private NumberTextBox txtDataPointClock;
        private Label lblDataPointClock;
        private NumberTextBox txtDataPointValue;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnBackward2;
        private System.Windows.Forms.Button btnForward2;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnBackward3;
        private System.Windows.Forms.Button btnForward3;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnRedo;

        #endregion

    }

}
