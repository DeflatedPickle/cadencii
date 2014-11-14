/*
 * FormBezierPointEditUiImpl.cs
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
using System.Windows.Forms;
using cadencii.apputil;
using cadencii;
using Cadencii.Gui;
using cadencii.java.util;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;


namespace Cadencii.Application.Forms
{
    public class FormBezierPointEditUiImpl : FormImpl, FormBezierPointEditUi
    {
        private FormBezierPointEditUiListener listener;

        public FormBezierPointEditUiImpl(FormBezierPointEditUiListener listener)
        {
            this.listener = listener;
            InitializeComponent();
            registerEventHandlers();
            setResources();
            AwtHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
        }


        #region UiBaseインターフェースの実装

        public int showDialog(object obj)
        {
            System.Windows.Forms.DialogResult ret;
            if (obj == null || (obj != null && !(obj is System.Windows.Forms.Form))) {
                ret = base.ShowDialog();
            } else {
                System.Windows.Forms.Form form = (System.Windows.Forms.Form)obj;
                ret = base.ShowDialog(form);
            }
            if (ret == System.Windows.Forms.DialogResult.OK || ret == System.Windows.Forms.DialogResult.Yes) {
                return 1;
            } else {
                return 0;
            }
        }

        #endregion


        #region FormBezierPointEditUiインターフェースの実装

        public void setDataPointValueText(string value)
        {
            txtDataPointValue.Text = value;
        }

        public void setDataPointClockText(string value)
        {
            txtDataPointClock.Text = value;
        }

        public void setRightValueText(string value)
        {
            txtRightValue.Text = value;
        }

        public void setRightClockText(string value)
        {
            txtRightClock.Text = value;
        }

        public void setLeftValueText(string value)
        {
            txtLeftValue.Text = value;
        }

        public void setLeftClockText(string value)
        {
            txtLeftClock.Text = value;
        }

        public void setLeftClockEnabled(bool value)
        {
            txtLeftClock.Enabled = value;
        }

        public bool isEnableSmoothSelected()
        {
            return chkEnableSmooth.Checked;
        }

        public void setEnableSmoothSelected(bool value)
        {
            chkEnableSmooth.Checked = value;
        }

        public void setRightButtonEnabled(bool value)
        {
            btnRight.Enabled = value;
        }

        public void setRightValueEnabled(bool value)
        {
            txtRightValue.Enabled = value;
        }

        public void setRightClockEnabled(bool value)
        {
            txtRightClock.Enabled = value;
        }

        public void setLeftButtonEnabled(bool value)
        {
            btnLeft.Enabled = value;
        }

        public void setLeftValueEnabled(bool value)
        {
            txtLeftValue.Enabled = value;
        }

        public string getRightValueText()
        {
            return txtRightValue.Text;
        }

        public string getRightClockText()
        {
            return txtRightClock.Text;
        }

        public string getLeftValueText()
        {
            return txtLeftValue.Text;
        }

        public string getLeftClockText()
        {
            return txtLeftClock.Text;
        }

        public string getDataPointValueText()
        {
            return txtDataPointValue.Text;
        }

        public string getDataPointClockText()
        {
            return txtDataPointClock.Text;
        }

        public void setCheckboxEnableSmoothText(string value)
        {
            chkEnableSmooth.Text = value;
        }

        public void setLabelRightValueText(string value)
        {
            lblRightValue.Text = value;
        }

        public void setGroupRightTitle(string value)
        {
            groupRight.Text = value;
        }

        public void setLabelRightClockText(string value)
        {
            lblRightClock.Text = value;
        }

        public void setLabelLeftValueText(string value)
        {
            lblLeftValue.Text = value;
        }

        public void setLabelLeftClockText(string value)
        {
            lblLeftClock.Text = value;
        }

        public void setGroupLeftTitle(string value)
        {
            groupLeft.Text = value;
        }

        public void setLabelDataPointValueText(string value)
        {
            lblDataPointValue.Text = value;
        }

        public void setLabelDataPointClockText(string value)
        {
            lblDataPointClock.Text = value;
        }

        public void setGroupDataPointTitle(string value)
        {
            groupDataPoint.Text = value;
        }

        public void setDialogResult(bool result)
        {
            if (result) {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            } else {
		this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
        }

        public void setOpacity(double opacity)
        {
            this.Opacity = opacity;
        }

        public void close()
        {
            this.Close();
        }

        public void setTitle(string value)
        {
            this.Text = value;
        }

        #endregion


        #region helper methods

        private static string _(string message)
        {
            return Messaging.getMessage(message);
        }

        private void registerEventHandlers()
        {
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            chkEnableSmooth.CheckedChanged += new EventHandler(chkEnableSmooth_CheckedChanged);
            btnLeft.MouseMove += new MouseEventHandler(common_MouseMove);
            btnLeft.MouseDown += new MouseEventHandler(handleOperationButtonMouseDown);
            btnLeft.MouseUp += new MouseEventHandler(common_MouseUp);
            btnDataPoint.MouseMove += new MouseEventHandler(common_MouseMove);
            btnDataPoint.MouseDown += new MouseEventHandler(handleOperationButtonMouseDown);
            btnDataPoint.MouseUp += new MouseEventHandler(common_MouseUp);
            btnRight.MouseMove += new MouseEventHandler(common_MouseMove);
            btnRight.MouseDown += new MouseEventHandler(handleOperationButtonMouseDown);
            btnRight.MouseUp += new MouseEventHandler(common_MouseUp);
            btnBackward.Click += new EventHandler(btnBackward_Click);
            btnForward.Click += new EventHandler(btnForward_Click);
        }

        private void setResources()
        {
			this.btnLeft.Image = cadencii.Properties.Resources.target__pencil;
			this.btnDataPoint.Image = cadencii.Properties.Resources.target__pencil;
			this.btnRight.Image = cadencii.Properties.Resources.target__pencil;
        }
        #endregion


        #region event handlers

        public void btnOK_Click(object sender, EventArgs e)
        {
            this.listener.buttonOkClick();
        }

        public void chkEnableSmooth_CheckedChanged(object sender, EventArgs e)
        {
            this.listener.checkboxEnableSmoothCheckedChanged();
        }

        public void handleOperationButtonMouseDown(object sender, MouseEventArgs e)
        {
            if (sender == btnLeft) {
                this.listener.buttonLeftMouseDown();
            } else if (sender == btnRight) {
                this.listener.buttonRightMouseDown();
            } else {
                this.listener.buttonCenterMouseDown();
            }
        }

        public void common_MouseUp(object sender, MouseEventArgs e)
        {
            this.listener.buttonsMouseUp();
        }

        public void common_MouseMove(object sender, MouseEventArgs e)
        {
            this.listener.buttonsMouseMove();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            this.listener.buttonForwardClick();
        }

        private void btnBackward_Click(object sender, EventArgs e)
        {
            this.listener.buttonBackwardClick();
        }

        public void btnCancel_Click(object sender, EventArgs e)
        {
            this.listener.buttonCancelClick();
        }

        #endregion

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
            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "FormBezierPointEditUi.xml");
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Button btnCancel;
        private Button btnOK;
        private CheckBox chkEnableSmooth;
        private Label lblLeftValue;
        private Label lblLeftClock;
        private NumberTextBox txtLeftValue;
        private NumberTextBox txtLeftClock;
        private GroupBox groupLeft;
        private GroupBox groupDataPoint;
        private Label lblDataPointValue;
        private NumberTextBox txtDataPointClock;
        private Label lblDataPointClock;
        private NumberTextBox txtDataPointValue;
        private GroupBox groupRight;
        private Label lblRightValue;
        private NumberTextBox txtRightClock;
        private Label lblRightClock;
        private NumberTextBox txtRightValue;
        private Button btnDataPoint;
        private Button btnLeft;
        private Button btnRight;
        private Button btnBackward;
        private Button btnForward;

    }

}
