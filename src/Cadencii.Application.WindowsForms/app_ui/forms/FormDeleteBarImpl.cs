/*
 * FormDeleteBar.cs
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



namespace Cadencii.Application.Forms
{
    class FormDeleteBarImpl : FormImpl, FormDeleteBar
    {
        public FormDeleteBarImpl(int max_barcount)
        {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            applyLanguage();
            numStart.Maximum = max_barcount;
            numEnd.Maximum = max_barcount;
            AwtHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
        }

        #region public methods
        public void applyLanguage()
        {
            this.Text = _("Delete Bars");
            lblStart.Text = _("Start");
            lblEnd.Text = _("End");
            btnOK.Text = _("OK");
            btnCancel.Text = _("Cancel");
        }

        public int Start {
			get { return (int)numStart.Value; }
			set { numStart.Value = value; }
        }

        public int End {
			get { return (int)numEnd.Value; }
			set { numEnd.Value = value; }
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
			this.AsAwt ().DialogResult = Cadencii.Gui.DialogResult.OK;
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
			this.AsAwt ().DialogResult = Cadencii.Gui.DialogResult.Cancel;
        }
        #endregion

        #region UI implementation
        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "FormDeleteBar.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#pragma warning disable 0649
        UiButton btnOK;
        UiButton btnCancel;
        UiLabel label4;
        UiLabel label3;
        UiLabel lblEnd;
        UiLabel lblStart;
        NumericUpDownEx numEnd;
        NumericUpDownEx numStart;
		#pragma warning restore 0649

        #endregion
    }

}
