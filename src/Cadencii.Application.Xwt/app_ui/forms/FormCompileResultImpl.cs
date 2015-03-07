/*
 * FormCompileResult.cs
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
using cadencii;

using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using Xwt;



namespace Cadencii.Application.Forms
{

    public class FormCompileResultImpl : FormImpl, FormCompileResult
    {
        public FormCompileResultImpl(string message, string errors)
        {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            applyLanguage();
            label1.Text = message;
            textBox1.Text = errors;
            GuiHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
        }

        #region public methods
        public void applyLanguage()
        {
            textBox1.Text = _("Script Compilation Result");
        }
        #endregion

        #region helper methods
        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void setResources()
        {
        }

        private void registerEventHandlers()
        {
            btnOK.Clicked += new EventHandler(btnOK_Click);
        }
        #endregion

        #region event handlers
        public void btnOK_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
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
			ApplicationUIHost.Instance.ApplyXml (this, "FormCompileResult.xml");
        }

		#pragma warning disable 0169,0649
        private Label label1;
		private TextEntry textBox1;
        private Button btnOK;
		#pragma warning restore 0169,0649

        #endregion
    }

}
