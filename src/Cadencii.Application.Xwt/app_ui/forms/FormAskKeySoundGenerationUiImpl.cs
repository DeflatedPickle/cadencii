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
            GuiHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
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

        #region helper methods
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
            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "FormAskKeySoundGenerationUi.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#pragma warning disable 0169,0649
        UiButton btnNo;
        UiCheckBox chkAlwaysPerformThisCheck;
        UiLabel lblMessage;
        UiButton btnYes;
		#pragma warning restore 0169,0649

        #endregion
    }


}
