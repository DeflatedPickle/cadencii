/*
 * FormAskKeySoundGenerationUiImpl.cs
 * Copyright © 2010-2011 kbinani
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
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;



namespace Cadencii.Application.Forms
{

    public class FormAskKeySoundGenerationUiImpl : FormImpl, FormAskKeySoundGenerationUi
    {
        private FormAskKeySoundGenerationUiListener mListener;

        public FormAskKeySoundGenerationUiImpl(FormAskKeySoundGenerationUiListener controller)
        {
            InitializeComponent();
            mListener = controller;
            registerEventHandlers();
            AwtHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
        }


        #region FormAskKeySoundGenerationUiインターフェースの実装

        /// <summary>
        /// メッセージの文字列を設定します．
        /// </summary>
        /// <param name="value">設定する文字列．</param>
        public void setMessageLabelText(string value)
        {
            lblMessage.Text = value;
        }

        public void setAlwaysPerformThisCheckCheckboxText(string value)
        {
            chkAlwaysPerformThisCheck.Text = value;
        }

        public void setYesButtonText(string value)
        {
            btnYes.Text = value;
        }

        public void setNoButtonText(string value)
        {
            btnNo.Text = value;
        }

        public void setAlwaysPerformThisCheck(bool value)
        {
            chkAlwaysPerformThisCheck.Checked = value;
        }

        public bool isAlwaysPerformThisCheck()
        {
            return chkAlwaysPerformThisCheck.Checked;
        }

        #endregion



        #region UiBaseインターフェースの実装

        public int showDialog(Object parent_form)
        {
			System.Windows.Forms.DialogResult ret;
            if (parent_form == null || (parent_form != null && !(parent_form is Form))) {
                ret = base.ShowDialog();
            } else {
                Form form = (Form)parent_form;
                ret = base.ShowDialog(form);
            }
            if (ret == DialogResult.OK || ret == DialogResult.Yes) {
                return 1;
            } else {
                return 0;
            }
        }

        #endregion



        #region public methods
        /// <summary>
        /// フォームを閉じます．
        /// valueがtrueのときダイアログの結果をCancelに，それ以外の場合はOKとなるようにします．
        /// </summary>
        public void close(bool value)
        {
            if (value) {
                this.DialogResult = DialogResult.Cancel;
            } else {
                this.DialogResult = DialogResult.OK;
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
            btnYes.Click += new EventHandler(btnYes_Click);
            btnNo.Click += new EventHandler(btnNo_Click);
        }

        #endregion



        #region event handlers

        private void btnYes_Click(Object sender, EventArgs e)
        {
            mListener.buttonOkClickedSlot();
        }

        private void btnNo_Click(Object sender, EventArgs e)
        {
            mListener.buttonCancelClickedSlot();
        }
        #endregion



        #region UI implementation

        private void InitializeComponent()
        {
            this.btnNo = new Button();
            this.btnYes = new Button();
            this.chkAlwaysPerformThisCheck = new CheckBox();
            this.lblMessage = new Label();
            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "FormAskKeySoundGenerationUi.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Button btnNo;
        private CheckBox chkAlwaysPerformThisCheck;
        private Label lblMessage;
        private Button btnYes;

        #endregion
    }


}
