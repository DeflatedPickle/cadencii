/*
 * FormSingerStyleConfig.cs
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
using cadencii;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;
using Cadencii.Application.Models;

namespace Cadencii.Application.Forms
{

    class FormSingerStyleConfigImpl : FormImpl, FormSingerStyleConfig
    {
        bool m_apply_current_track = false;

        public FormSingerStyleConfigImpl()
        {
            InitializeComponent();

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

            registerEventHandlers();
            setResources();
            AwtHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
            applyLanguage();
        }

        #region public methods
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

            groupDynamicsControl.Text = _("Dynamics Control");
            lblDecay.Text = _("Decay");
			lblDecay.Mnemonic(Keys.D);
            lblAccent.Text = _("Accent");
			lblAccent.Mnemonic(Keys.A);

            btnOK.Text = _("OK");
            btnCancel.Text = _("Cancel");
            btnApply.Text = _("Apply to current track");
			btnApply.Mnemonic(Keys.C);

            lblTemplate.Left = comboTemplate.Left - lblTemplate.Width;
            this.Text = _("Default Singer Style");
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
            btnApply.Click += new EventHandler(btnApply_Click);
            comboTemplate.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        private void setResources()
        {
        }
        #endregion

        #region event handlers
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
                //txtBendDepth.Text = trackBendDepth.Value + "";
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
                //txtBendLength.Text = trackBendLength.Value + "";
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
                //txtDecay.Text = trackDecay.Value + "";
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
                //txtAccent.Text = trackAccent.Value + "";
            }
        }

        public void btnOK_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
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
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
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
			ApplicationUIHost.Instance.ApplyXml (this, "FormSingerStyleConfig.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		#pragma warning disable 0169,0649
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
        UiButton btnApply;
        UiComboBox comboTemplate;
		#pragma warning restore 0169,0649
        #endregion
        #endregion

    }

}
