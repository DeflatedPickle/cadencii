/*
 * InputBox.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.windows.forms.
 *
 * cadencii.windows.forms is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.windows.forms is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml;


namespace cadencii
{

    public class InputBoxImpl : FormImpl, InputBox
    {
        public InputBoxImpl(string message)
        {
            InitializeComponent();
            registerEventHandlers();
            lblMessage.Text = message;
        }

		public string Result {
			get { return txtInput.Text; }
			set { txtInput.Text = value; }
		}

        public void btnCancel_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        public void btnOk_Click(Object sender, EventArgs e)
        {
#if DEBUG
            Logger.StdOut("InputBox#btnOk_Click");
#endif
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void registerEventHandlers()
        {
            btnOk.Click += new EventHandler(btnOk_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

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
			ApplicationUIHost.Instance.ApplyXml (this, "InputBox.xml");
			this.ResumeLayout(false);
			this.PerformLayout();
        }
        #endregion

        private Label lblMessage;
        private Button btnCancel;
        private TextBox txtInput;
        private Button btnOk;
    }

}
