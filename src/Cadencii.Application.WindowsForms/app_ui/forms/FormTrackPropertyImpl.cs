/*
 * FormTrackProperty.cs
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
using System.Windows.Forms;
using cadencii.apputil;
using cadencii;

using Cadencii.Gui;



namespace cadencii
{

    public class FormTrackPropertyImpl : FormImpl, FormTrackProperty
    {
        private int m_master_tuning;

        public FormTrackPropertyImpl(int master_tuning_in_cent)
        {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            applyLanguage();
            m_master_tuning = master_tuning_in_cent;
            txtMasterTuning.Text = master_tuning_in_cent + "";
            AwtHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
        }

        #region public methods
        public void applyLanguage()
        {
            lblMasterTuning.Text = _("Master Tuning in Cent");
            this.Text = _("Track Property");
            btnOK.Text = _("OK");
            btnCancel.Text = _("Cancel");
        }

        public int getMasterTuningInCent()
        {
            return m_master_tuning;
        }
        #endregion

        #region helper methods
        private string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void registerEventHandlers()
        {
            txtMasterTuning.TextChanged += new EventHandler(txtMasterTuning_TextChanged);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        private void setResources()
        {
        }
        #endregion

        #region event handlers
        public void txtMasterTuning_TextChanged(Object sender, EventArgs e)
        {
            int v = m_master_tuning;
            try {
                v = int.Parse(txtMasterTuning.Text);
                m_master_tuning = v;
            } catch (Exception ex) {
            }
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        public void btnOK_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
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
			ApplicationUIHost.Instance.ApplyXml (this, "FormTrackProperty.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnOK;
        private Button btnCancel;
        private Label lblMasterTuning;
        private TextBox txtMasterTuning;
        #endregion
        #endregion

    }

}
