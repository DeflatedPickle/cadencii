/*
 * FormNoteExpressionConfig.cs
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
using System.Drawing;
using cadencii.apputil;
using Cadencii.Media.Vsq;

using cadencii.java.util;
using Keys = Cadencii.Gui.Toolkit.Keys;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;
using cadencii;
using Cadencii.Application.Models;

namespace Cadencii.Application.Forms
{
    public class FormNoteExpressionConfigImpl : FormImpl, FormNoteExpressionConfig
    {
        bool m_apply_current_track = false;
        NoteHeadHandle m_note_head_handle = null;

        public FormNoteExpressionConfigImpl(SynthesizerType type, NoteHeadHandle note_head_handle)
        {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            AwtHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
            applyLanguage();

            if (note_head_handle != null) {
                m_note_head_handle = (NoteHeadHandle)note_head_handle.clone();
            }

            if (type == SynthesizerType.VOCALOID1) {
                flowLayoutPanel.Controls.Remove(groupDynamicsControl);
                flowLayoutPanel.Controls.Remove(panelVocaloid2Template);
                flowLayoutPanel.Controls.Remove(groupPitchControl);
            } else {
                flowLayoutPanel.Controls.Remove(groupAttack);
            }

            //comboAttackTemplateを更新
            NoteHeadHandle empty = new NoteHeadHandle();
            comboAttackTemplate.Items.Clear();
            empty.IconID = "$01010000";
            empty.setCaption("[Non Attack]");
            comboAttackTemplate.Items.Add(empty);
            comboAttackTemplate.SelectedItem = empty;
            string icon_id = "";
            if (m_note_head_handle != null) {
                icon_id = m_note_head_handle.IconID;
                txtDuration.Text = m_note_head_handle.getDuration() + "";
                txtDepth.Text = m_note_head_handle.getDepth() + "";
            } else {
                txtDuration.Enabled = false;
                txtDepth.Enabled = false;
                trackDuration.Enabled = false;
                trackDepth.Enabled = false;
            }
            foreach (var item in VocaloSysUtil.attackConfigIterator(SynthesizerType.VOCALOID1)) {
                comboAttackTemplate.Items.Add(item);
                if (item.IconID.Equals(icon_id)) {
                    comboAttackTemplate.SelectedItem = comboAttackTemplate.Items[comboAttackTemplate.Items.Count - 1];
                }
            }
            comboAttackTemplate.SelectedIndexChanged += new EventHandler(comboAttackTemplate_SelectedIndexChanged);

            comboTemplate.Items.Clear();
            string[] strs = new string[]{
                "[Select a template]",
                "normal",
                "accent",
                "strong accent",
                "legato",
                "slow legate",
            };
            for (int i = 0; i < strs.Length; i++) {
                comboTemplate.Items.Add(strs[i]);
            }

            Size current_size = this.ClientSize;
            this.ClientSize = new Size(current_size.Width, flowLayoutPanel.ClientSize.Height + flowLayoutPanel.Top * 2);
			this.AsAwt ().FormBorderStyle = Cadencii.Gui.Toolkit.FormBorderStyle.FixedDialog;
        }

        #region public methods
        public NoteHeadHandle EditedNoteHeadHandle {
			get {
				return m_note_head_handle;
			}
		}

        public void applyLanguage()
        {
            lblTemplate.Text = _("Template");
            lblTemplate.Mnemonic(Keys.T);
            groupPitchControl.Text = _("Pitch Control");
            lblBendDepth.Text = _("Bend Depth");
            lblBendDepth.Mnemonic(Keys.B);
            lblBendLength.Text = _("Bend Length");
            lblBendLength.Mnemonic(Keys.L);
            chkUpPortamento.Text = _("Add portamento in rising movement");
            chkUpPortamento.Mnemonic(Keys.R);
            chkDownPortamento.Text = _("Add portamento in falling movement");
            chkDownPortamento.Mnemonic(Keys.F);

            groupAttack.Text = _("Attack Control (VOCALOID1)");
            groupDynamicsControl.Text = _("Dynamics Control (VOCALOID2)");
            lblDecay.Text = _("Decay");
            lblDecay.Mnemonic(Keys.D);
            lblAccent.Text = _("Accent");
            lblAccent.Mnemonic(Keys.A);

            btnOK.Text = _("OK");
            btnCancel.Text = _("Cancel");

            lblTemplate.Left = comboTemplate.Left - lblTemplate.Width;
            this.Text = _("Expression control property");
        }

        public int PMBendDepth {
			get {
				return trackBendDepth.Value;
			}

			set {
				trackBendDepth.Value = value;
				txtBendDepth.Text = value + "";
			}
		}

        public int PMBendLength {
			get {
				return trackBendLength.Value;
			}

			set {
				trackBendLength.Value = value;
				txtBendLength.Text = value + "";
			}
		}

        public int PMbPortamentoUse {
			get {
				int ret = 0;
				if (chkUpPortamento.Checked) {
					ret += 1;
				}
				if (chkDownPortamento.Checked) {
					ret += 2;
				}
				return ret;
			}

			set {
				if (value % 2 == 1) {
					chkUpPortamento.Checked = true;
				} else {
					chkUpPortamento.Checked = false;
				}
				if (value >= 2) {
					chkDownPortamento.Checked = true;
				} else {
					chkDownPortamento.Checked = false;
				}
			}
		}

        public int DEMdecGainRate {
			get {
				return trackDecay.Value;
			}

			set {
				trackDecay.Value = value;
				txtDecay.Text = value + "";
			}
		}

        public int DEMaccent {
			get {
				return trackAccent.Value;
			}

			set {
				trackAccent.Value = value;
				txtAccent.Text = value + "";
			}
		}

        public bool ApplyCurrentTrack {
			get {
				return m_apply_current_track;
			}
		}
        #endregion

        #region helper methods
        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void registerEventHandlers()
        {
            txtBendLength.TextChanged += new EventHandler(txtBendLength_TextChanged);
            txtBendDepth.TextChanged += new EventHandler(txtBendDepth_TextChanged);
            trackBendLength.ValueChanged += new EventHandler(trackBendLength_Scroll);
            trackBendDepth.ValueChanged += new EventHandler(trackBendDepth_Scroll);
            txtAccent.TextChanged += new EventHandler(txtAccent_TextChanged);
            txtDecay.TextChanged += new EventHandler(txtDecay_TextChanged);
            trackAccent.ValueChanged += new EventHandler(trackAccent_Scroll);
            trackDecay.ValueChanged += new EventHandler(trackDecay_Scroll);
            btnOK.Click += new EventHandler(btnOK_Click);
            comboTemplate.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
            txtDepth.TextChanged += new EventHandler(txtDepth_TextChanged);
            txtDuration.TextChanged += new EventHandler(txtDuration_TextChanged);
            trackDepth.ValueChanged += new EventHandler(trackDepth_Scroll);
            trackDuration.ValueChanged += new EventHandler(trackDuration_Scroll);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        private void setResources()
        {
        }
        #endregion

        #region event handlers
        public void comboAttackTemplate_SelectedIndexChanged(Object sender, EventArgs e)
        {
            int index = comboAttackTemplate.SelectedIndex;
            if (index < 0) {
                return;
            }
            if (index == 0) {
                m_note_head_handle = null;
                txtDuration.Enabled = false;
                trackDuration.Enabled = false;
                txtDepth.Enabled = false;
                trackDepth.Enabled = false;
                return;
            }
            txtDuration.Enabled = true;
            trackDuration.Enabled = true;
            txtDepth.Enabled = true;
            trackDepth.Enabled = true;
            NoteHeadHandle aconfig = (NoteHeadHandle)comboAttackTemplate.SelectedItem;
            if (m_note_head_handle == null) {
                txtDuration.Text = aconfig.getDuration() + "";
                txtDepth.Text = aconfig.getDepth() + "";
            }
            m_note_head_handle = (NoteHeadHandle)aconfig.clone();
            m_note_head_handle.setDuration(trackDuration.Value);
            m_note_head_handle.setDepth(trackDepth.Value);
        }

        public void trackBendDepth_Scroll(Object sender, EventArgs e)
        {
            string s = trackBendDepth.Value + "";
            if (s != txtBendDepth.Text) {
                txtBendDepth.Text = s;
            }
        }

        public void txtBendDepth_TextChanged(Object sender, EventArgs e)
        {
            try {
                int draft = int.Parse(txtBendDepth.Text);
                if (draft < trackBendDepth.Minimum) {
                    draft = trackBendDepth.Minimum;
                    txtBendDepth.Text = draft + "";
                } else if (trackBendDepth.Maximum < draft) {
                    draft = trackBendDepth.Maximum;
                    txtBendDepth.Text = draft + "";
                }
                if (draft != trackBendDepth.Value) {
                    trackBendDepth.Value = draft;
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormNoteExpressionConfig) + ".txtBendDepth_TextChanged; ex=" + ex + "\n");
            }
        }

        public void trackBendLength_Scroll(Object sender, EventArgs e)
        {
            string s = trackBendLength.Value + "";
            if (s != txtBendLength.Text) {
                txtBendLength.Text = s;
            }
        }

        public void txtBendLength_TextChanged(Object sender, EventArgs e)
        {
            try {
                int draft = int.Parse(txtBendLength.Text);
                if (draft < trackBendLength.Minimum) {
                    draft = trackBendLength.Minimum;
                    txtBendLength.Text = draft + "";
                } else if (trackBendLength.Maximum < draft) {
                    draft = trackBendLength.Maximum;
                    txtBendLength.Text = draft + "";
                }
                if (draft != trackBendLength.Value) {
                    trackBendLength.Value = draft;
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormNoteExpressionConfig) + ".txtBendLength_TextChanged; ex=" + ex + "\n");
            }
        }

        public void trackDecay_Scroll(Object sender, EventArgs e)
        {
            string s = trackDecay.Value + "";
            if (s != txtDecay.Text) {
                txtDecay.Text = s;
            }
        }

        public void txtDecay_TextChanged(Object sender, EventArgs e)
        {
            try {
                int draft = int.Parse(txtDecay.Text);
                if (draft < trackDecay.Minimum) {
                    draft = trackDecay.Minimum;
                    txtDecay.Text = draft + "";
                } else if (trackDecay.Maximum < draft) {
                    draft = trackDecay.Maximum;
                    txtDecay.Text = draft + "";
                }
                if (draft != trackDecay.Value) {
                    trackDecay.Value = draft;
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormNoteExpressionConfig) + ".txtDecay_TextChanged; ex=" + ex + "\n");
            }
        }

        public void trackAccent_Scroll(Object sender, EventArgs e)
        {
            string s = trackAccent.Value + "";
            if (s != txtAccent.Text) {
                txtAccent.Text = s;
            }
        }

        public void txtAccent_TextChanged(Object sender, EventArgs e)
        {
            try {
                int draft = int.Parse(txtAccent.Text);
                if (draft < trackAccent.Minimum) {
                    draft = trackAccent.Minimum;
                    txtAccent.Text = draft + "";
                } else if (trackAccent.Maximum < draft) {
                    draft = trackAccent.Maximum;
                    txtAccent.Text = draft + "";
                }
                if (draft != trackAccent.Value) {
                    trackAccent.Value = draft;
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormNoteExpressionConfig) + ".txtAccent_TextChanged; ex=" + ex + "\n");
            }
        }

        public void btnOK_Click(Object sender, EventArgs e)
        {
			this.AsAwt ().DialogResult = Cadencii.Gui.DialogResult.OK;
        }

        public void comboBox1_SelectedIndexChanged(Object sender, EventArgs e)
        {
            int index = comboTemplate.SelectedIndex - 1;
            if (index < 0 || 4 < index) {
                return;
            }
            int[] pm_bend_depth = new int[] { 8, 8, 8, 20, 20 };
            int[] pm_bend_length = new int[] { 0, 0, 0, 0, 0 };
            int[] pmb_portamento_use = new int[] { 0, 0, 0, 3, 3 };
            int[] dem_dec_gain_rate = new int[] { 50, 50, 70, 50, 50 };
            int[] dem_accent = new int[] { 50, 68, 80, 42, 25 };
            PMBendDepth = (pm_bend_depth[index]);
            PMBendLength = (pm_bend_length[index]);
            PMbPortamentoUse = (pmb_portamento_use[index]);
            DEMdecGainRate = (dem_dec_gain_rate[index]);
            DEMaccent = (dem_accent[index]);
        }

        public void btnApply_Click(Object sender, EventArgs e)
        {
            if (DialogManager.ShowMessageBox(_("Would you like to change singer style for all events?"),
				FormMainModel.Consts.ApplicationName,
				Cadencii.Gui.Toolkit.MessageBoxButtons.YesNo,
				Cadencii.Gui.Toolkit.MessageBoxIcon.Warning) == Cadencii.Gui.DialogResult.Yes) {
                m_apply_current_track = true;
				this.AsAwt ().DialogResult = Cadencii.Gui.DialogResult.OK;
            }
        }

        public void trackDuration_Scroll(Object sender, EventArgs e)
        {
            string s = trackDuration.Value + "";
            if (s != txtDuration.Text) {
                txtDuration.Text = s;
            }
            if (m_note_head_handle != null) {
                m_note_head_handle.setDuration(trackDuration.Value);
            }
        }

        public void trackDepth_Scroll(Object sender, EventArgs e)
        {
            string s = trackDepth.Value + "";
            if (s != txtDepth.Text) {
                txtDepth.Text = s;
            }
            if (m_note_head_handle != null) {
                m_note_head_handle.setDepth(trackDepth.Value);
            }
        }

        public void txtDuration_TextChanged(Object sender, EventArgs e)
        {
            try {
                int draft = int.Parse(txtDuration.Text);
                if (draft < trackDuration.Minimum) {
                    draft = trackDuration.Minimum;
                    txtDuration.Text = draft + "";
                } else if (trackDuration.Maximum < draft) {
                    draft = trackDuration.Maximum;
                    txtDuration.Text = draft + "";
                }
                if (draft != trackDuration.Value) {
                    trackDuration.Value = draft;
                }
                if (m_note_head_handle != null) {
                    m_note_head_handle.setDuration(draft);
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormNoteExpressionConfig) + ".txtDuration_TextChanged; ex=" + ex + "\n");
            }
        }

        public void txtDepth_TextChanged(Object sender, EventArgs e)
        {
            try {
                int draft = int.Parse(txtDepth.Text);
                if (draft < trackDepth.Minimum) {
                    draft = trackDepth.Minimum;
                    txtDepth.Text = draft + "";
                } else if (trackDepth.Maximum < draft) {
                    draft = trackDepth.Maximum;
                    txtDepth.Text = draft + "";
                }
                if (draft != trackDepth.Value) {
                    trackDepth.Value = draft;
                }
                if (m_note_head_handle != null) {
                    m_note_head_handle.setDepth(trackDepth.Value);
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormNoteExpressionConfig) + ".txtDepth_TextChanged; ex=" + ex + "\n");
            }
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
			this.AsAwt ().DialogResult = Cadencii.Gui.DialogResult.Cancel;
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
			ApplicationUIHost.Instance.ApplyXml (this, "FormNoteExpressionConfig.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		#pragma warning disable 0649
        UiGroupBox groupPitchControl;
        UiGroupBox groupDynamicsControl;
        UiLabel lblBendDepth;
        UiLabel lblTemplate;
        UiLabel lblBendLength;
        UiCheckBox chkDownPortamento;
        UiCheckBox chkUpPortamento;
        UiTrackBar trackBendDepth;
        UiTrackBar trackBendLength;
        UiTrackBar trackAccent;
        UiTrackBar trackDecay;
        UiLabel lblAccent;
        UiLabel lblDecay;
        NumberTextBox txtBendLength;
        NumberTextBox txtBendDepth;
        NumberTextBox txtAccent;
        NumberTextBox txtDecay;
        UiLabel label5;
        UiLabel label4;
        UiLabel label7;
        UiLabel label6;
        UiButton btnCancel;
        UiButton btnOK;
        UiComboBox comboTemplate;
        UiGroupBox groupAttack;
        NumberTextBox txtDepth;
        NumberTextBox txtDuration;
        UiTrackBar trackDepth;
        UiTrackBar trackDuration;
        UiLabel lblDepth;
        UiLabel lblDuration;
        UiFlowLayoutPanel flowLayoutPanel;
        UiPanel panelButtons;
        UiPanel panelVocaloid2Template;
        UiComboBox comboAttackTemplate;
        UiLabel lblAttackTemplate;
		#pragma warning restore 0649
        #endregion
        #endregion

    }

}
