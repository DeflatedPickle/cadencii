/*
 * FormMidiImExport.cs
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
using cadencii.apputil;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using Cadencii.Media.Vsq;
using Cadencii.Application.Controls;
using cadencii;

namespace Cadencii.Application.Forms
{
	public class FormMidiImExportImpl : FormImpl, FormMidiImExport
    {
        private FormMidiMode m_mode;
        private VsqFileEx m_vsq;
        private static int columnWidthTrack;
        private static int columnWidthName;
        private static int columnWidthNotes;

        public FormMidiImExportImpl()
        {
            InitializeComponent();
            applyLanguage();
            Mode = FormMidiMode.EXPORT;
            AwtHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());

		/* It should not be done like this...
            System.Drawing.Point p = btnCheckAll.Location;
            btnUncheckAll.Location = new System.Drawing.Point(p.X + btnCheckAll.Width + 6, p.Y);
		*/

            registerEventHandlers();
            setResources();
        }

		UiListView FormMidiImExport.ListTrack {
			get { return listTrack; }
		}

        #region public methods
        public void applyLanguage()
        {
            if (m_mode == FormMidiMode.EXPORT) {
                this.Text = _("Midi Export");
            } else if (m_mode == FormMidiMode.IMPORT) {
                this.Text = _("Midi Import");
            } else {
                this.Text = _("VSQ/Vocaloid Midi Import");
            }
            groupMode.Text = _("Import Basis");
            radioGateTime.Text = _("gate-time");
            radioPlayTime.Text = _("play-time");
            listTrack.SetColumnHeaders(new string[] { _("Track"), _("Name"), _("Notes") });
            btnCheckAll.Text = _("Check All");
            btnUncheckAll.Text = _("Uncheck All");
            groupCommonOption.Text = _("Option");
            btnOK.Text = _("OK");
            btnCancel.Text = _("Cancel");
            chkTempo.Text = _("Tempo");
            chkBeat.Text = _("Beat");
            chkNote.Text = _("Note");
            chkLyric.Text = _("Lyrics");
            chkExportVocaloidNrpn.Text = _("vocaloid NRPN");
            lblOffset.Text = _("offset");
            if (radioGateTime.Checked) {
                lblOffsetUnit.Text = _("clocks");
            } else {
                lblOffsetUnit.Text = _("seconds");
            }
        }

        public double getOffsetSeconds()
        {
            double v = 0.0;
            try {
                v = double.Parse(txtOffset.Text);
            } catch (Exception ex) {
                Logger.write(typeof(FormMidiImExport) + ".getOffsetSeconds; ex=" + ex + "\n");
                Logger.StdErr("FormMidiImExport#getOffsetClocks; ex=" + ex);
            }
            return v;
        }

        public int getOffsetClocks()
        {
            int v = 0;
            try {
                v = int.Parse(txtOffset.Text);
            } catch (Exception ex) {
                Logger.write(typeof(FormMidiImExport) + ".getOffsetClocks; ex=" + ex + "\n");
                Logger.StdErr("FormMidiImExport#getOffsetClocks; ex=" + ex);
            }
            return v;
        }

        public bool isSecondBasis()
        {
            return radioPlayTime.Checked;
        }

		public FormMidiMode Mode {
			get { return m_mode; }

			set {
				m_mode = value;
				chkExportVocaloidNrpn.Enabled = (m_mode == FormMidiMode.EXPORT);
				chkLyric.Enabled = (m_mode != FormMidiMode.IMPORT_VSQ);
				chkNote.Enabled = (m_mode != FormMidiMode.IMPORT_VSQ);
				chkPreMeasure.Enabled = (m_mode != FormMidiMode.IMPORT_VSQ);
				if (m_mode == FormMidiMode.EXPORT) {
					this.Text = _ ("Midi Export");
					chkPreMeasure.Text = _ ("Export pre-measure part");
					if (chkExportVocaloidNrpn.Checked) {
						chkPreMeasure.Enabled = false;
						EditorManager.editorConfig.MidiImExportConfigExport.LastPremeasureCheckStatus = chkPreMeasure.Checked;
						chkPreMeasure.Checked = true;
					} else {
						chkPreMeasure.Checked = EditorManager.editorConfig.MidiImExportConfigExport.LastPremeasureCheckStatus;
					}
					if (chkNote.Checked) {
						chkMetaText.Enabled = false;
						EditorManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus = chkMetaText.Checked;
						chkMetaText.Checked = false;
					} else {
						chkMetaText.Checked = EditorManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus;
					}
					groupMode.Enabled = false;
				} else if (m_mode == FormMidiMode.IMPORT) {
					this.Text = _ ("Midi Import");
					chkPreMeasure.Text = _ ("Inserting start at pre-measure");
					chkMetaText.Enabled = false;
					EditorManager.editorConfig.MidiImExportConfigImport.LastMetatextCheckStatus = chkMetaText.Checked;
					chkMetaText.Checked = false;
					groupMode.Enabled = true;
				} else {
					this.Text = _ ("VSQ/Vocaloid Midi Import");
					chkPreMeasure.Text = _ ("Inserting start at pre-measure");
					chkPreMeasure.Checked = false;
					EditorManager.editorConfig.MidiImExportConfigImportVsq.LastMetatextCheckStatus = chkMetaText.Checked;
					chkMetaText.Checked = true;
					groupMode.Enabled = false;
				}
			}
		}

        public bool isVocaloidMetatext()
        {
            if (chkNote.Checked) {
                return false;
            } else {
                return chkMetaText.Checked;
            }
        }

        public bool isVocaloidNrpn()
        {
            return chkExportVocaloidNrpn.Checked;
        }

        public bool isTempo()
        {
            return chkTempo.Checked;
        }

        public void setTempo(bool value)
        {
            chkTempo.Checked = value;
        }

        public bool isTimesig()
        {
            return chkBeat.Checked;
        }

        public void setTimesig(bool value)
        {
            chkBeat.Checked = value;
        }

        public bool isNotes()
        {
            return chkNote.Checked;
        }

        public bool isLyric()
        {
            return chkLyric.Checked;
        }

        public bool isPreMeasure()
        {
            return chkPreMeasure.Checked;
        }
        #endregion

        #region helper methods
        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void registerEventHandlers()
        {
            btnCheckAll.Click += new EventHandler(btnCheckAll_Click);
            btnUncheckAll.Click += new EventHandler(btnUnckeckAll_Click);
            chkNote.CheckedChanged += new EventHandler(chkNote_CheckedChanged);
            chkMetaText.Click += new EventHandler(chkMetaText_Click);
            chkExportVocaloidNrpn.CheckedChanged += new EventHandler(chkExportVocaloidNrpn_CheckedChanged);
            chkExportVocaloidNrpn.CheckedChanged += new EventHandler(chkExportVocaloidNrpn_CheckedChanged);
			this.AsAwt ().FormClosing += (o, e) => FormMidiImExport_FormClosing ();
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            radioGateTime.CheckedChanged += new EventHandler(radioGateTime_CheckedChanged);
            radioPlayTime.CheckedChanged += new EventHandler(radioPlayTime_CheckedChanged);
        }

        private void setResources()
        {
        }
        #endregion

        #region event handlers
        public void btnCheckAll_Click(Object sender, EventArgs e)
        {
			var listTrack = (UiListView) this.listTrack;
            for (int i = 0; i < listTrack.ItemCount; i++) {
                listTrack.GetItem(i).Checked = true;
            }
        }

        public void btnUnckeckAll_Click(Object sender, EventArgs e)
        {
			var listTrack = (UiListView) this.listTrack;
            for (int i = 0; i < listTrack.ItemCount; i++) {
                listTrack.GetItem(i).Checked = false;
            }
        }

        public void chkExportVocaloidNrpn_CheckedChanged(Object sender, EventArgs e)
        {
            if (m_mode == FormMidiMode.EXPORT) {
                if (chkExportVocaloidNrpn.Checked) {
                    chkPreMeasure.Enabled = false;
                    EditorManager.editorConfig.MidiImExportConfigExport.LastPremeasureCheckStatus = chkPreMeasure.Checked;
                    chkPreMeasure.Checked = true;
                } else {
                    chkPreMeasure.Enabled = true;
                    chkPreMeasure.Checked = EditorManager.editorConfig.MidiImExportConfigExport.LastPremeasureCheckStatus;
                }
            }
        }

        public void chkNote_CheckedChanged(Object sender, EventArgs e)
        {
            if (m_mode == FormMidiMode.EXPORT) {
                if (chkNote.Checked) {
                    chkMetaText.Enabled = false;
                    EditorManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus = chkMetaText.Checked;
                    chkMetaText.Checked = false;
                } else {
                    chkMetaText.Enabled = true;
                    chkMetaText.Checked = EditorManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus;
                }
            }
        }

        public void chkMetaText_Click(Object sender, EventArgs e)
        {
            if (m_mode == FormMidiMode.EXPORT) {
                EditorManager.editorConfig.MidiImExportConfigExport.LastMetatextCheckStatus = chkMetaText.Checked;
            }
        }

        public void FormMidiImExport_FormClosing()
        {
			var listTrack = (UiListView) this.listTrack;
            columnWidthTrack = listTrack.Columns [0].Width;
            columnWidthName = listTrack.Columns [1].Width;
            columnWidthNotes = listTrack.Columns [2].Width;
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        public void btnOK_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public void radioGateTime_CheckedChanged(Object sender, EventArgs e)
        {
            if (radioGateTime.Checked) {
                lblOffsetUnit.Text = _("clocks");
                txtOffset.Type = NumberTextBoxValueType.Integer;
            }
        }

        public void radioPlayTime_CheckedChanged(Object sender, EventArgs e)
        {
            if (radioPlayTime.Checked) {
                lblOffsetUnit.Text = _("seconds");
                txtOffset.Type = NumberTextBoxValueType.Double;
            }
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
			ApplicationUIHost.Instance.ApplyXml (this, "FormMidiImExport.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#pragma warning disable 0169,0649
        UiButton btnCancel;
        UiButton btnOK;
        UiButton btnCheckAll;
        UiButton btnUncheckAll;
        UiCheckBox chkBeat;
        UiCheckBox chkTempo;
        UiCheckBox chkNote;
        UiCheckBox chkLyric;
        UiGroupBox groupCommonOption;
        UiCheckBox chkExportVocaloidNrpn;
		UiListView listTrack { get ; set; }
        UiCheckBox chkPreMeasure;
        UiCheckBox chkMetaText;
        UiGroupBox groupMode;
        UiRadioButton radioPlayTime;
        UiRadioButton radioGateTime;
        UiLabel lblOffset;
        NumberTextBox txtOffset;
        UiLabel lblOffsetUnit;
		#pragma warning restore 0169,0649

        #endregion

    }

}
