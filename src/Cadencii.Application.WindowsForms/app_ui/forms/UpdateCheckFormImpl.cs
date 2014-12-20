/*
 * UpdateCheckForm.cs
 * Copyright © 2013 kbinani
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
using System.Linq;
using System.Collections.Generic;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Forms
{
    class UpdateCheckFormImpl : Cadencii.Gui.Toolkit.FormImpl, UpdateCheckForm
    {
		#pragma warning disable 0169,0649
        UiLabel label1;
        UiLinkLabel linkLabel1;
        UiCheckBox checkBox1;
        UiButton button1;
		#pragma warning restore 0169,0649

        public event Action okButtonClicked;
        public event Action downloadLinkClicked;

        public UpdateCheckFormImpl()
        {
            InitializeComponent();
        }

        public void setOkButtonText(string text)
        {
            button1.Text = text;
        }

        public void setDownloadUrl(string url)
        {
            linkLabel1.Text = url;
        }

        public void setMessage(string text)
        {
            label1.Text = text;
        }

        public bool isAutomaticallyCheckForUpdates()
        {
            return checkBox1.Checked;
        }

        public void setAutomaticallyCheckForUpdates(bool value)
        {
            checkBox1.Checked = value;
        }

        public void setAutomaticallyCheckForUpdatesMessage(string message)
        {
            checkBox1.Text = message;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "UpdateCheckForm.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

			button1.Click += new System.EventHandler (button1_Click);
			linkLabel1.LinkClicked += linkLabel1_LinkClicked;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (okButtonClicked != null) {
                okButtonClicked.Invoke();
            }
        }

        private void linkLabel1_LinkClicked(object sender, EventArgs e)
        {
            if (downloadLinkClicked != null) {
                downloadLinkClicked.Invoke();
            }
        }
    }
}
