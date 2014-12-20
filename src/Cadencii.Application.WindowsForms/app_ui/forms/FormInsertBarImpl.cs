/*
 * FormInsertBar.cs
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



namespace Cadencii.Application.Forms
{

    public class FormInsertBarImpl : FormImpl, FormInsertBar
    {
        public FormInsertBarImpl(int max_position)
        {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            applyLanguage();
            numPosition.Maximum = max_position;
            AwtHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
        }

        #region public methods
        public void applyLanguage()
        {
            this.Text = _("Insert Bars");
            string th_prefix = _("_PREFIX_TH_");
            if (th_prefix.Equals("_PREFIX_TH_")) {
                lblPositionPrefix.Text = "";
            } else {
                lblPositionPrefix.Text = th_prefix;
            }
            lblPosition.Text = _("Position");
            lblLength.Text = _("Length");
            lblThBar.Text = _("th bar");
            lblBar.Text = _("bar");
            btnOK.Text = _("OK");
            btnCancel.Text = _("Cancel");
            lblPositionPrefix.Left = numPosition.Left - lblPositionPrefix.Width;
        }

        public int Length {
			get {
				return (int)numLength.Value;
			}

			set {
				numLength.Value = value;
			}
		}

        public int Position {
			get {
				return (int)numPosition.Value;
			}

			set {
				numPosition.Value = value;
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
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        private void setResources()
        {
        }
        #endregion

        #region event handlers
        public void btnOK_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        #endregion

        #region UI implementation
		#pragma warning disable 0169,0649
        private System.ComponentModel.IContainer components = null;
        NumericUpDownEx numPosition;
        NumericUpDownEx numLength;
        UiLabel lblPosition;
        UiLabel lblLength;
        UiLabel lblThBar;
        UiLabel lblBar;
        UiButton btnCancel;
        UiButton btnOK;
        UiLabel lblPositionPrefix;
		#pragma warning restore 0169,0649

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
			ApplicationUIHost.Instance.ApplyXml (this, "FormInsertBar.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

    }

}
