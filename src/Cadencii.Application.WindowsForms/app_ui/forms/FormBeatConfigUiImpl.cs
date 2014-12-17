/*
 * FormBeatConfigUiImpl.cs
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

    public class FormBeatConfigUiImpl : FormImpl, FormBeatConfigUi
    {
        private FormBeatConfigUiListener mListener;

        public FormBeatConfigUiImpl(FormBeatConfigUiListener listener)
        {
            mListener = listener;
            InitializeComponent();
			RegisterEventHandlers ();
        }


        #region FormBeatConfigUiインターフェースの実装

        public int getWidth()
        {
            return this.Width;
        }

        public int getHeight()
        {
            return this.Height;
        }

        public void setLocation(int x, int y)
        {
            this.Location = new System.Drawing.Point(x, y);
        }

        public void setDialogResult(bool value)
        {
            if (value) {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            } else {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
        }

        public int getSelectedIndexDenominatorCombobox()
        {
            return comboDenominator.SelectedIndex;
        }

        public bool isCheckedEndCheckbox()
        {
            return chkEnd.Checked;
        }

        public void setTextBar2Label(string value)
        {
            lblBar2.Text = value;
        }

        public void setTextBar1Label(string value)
        {
            lblBar1.Text = value;
        }

        public void setTextStartLabel(string value)
        {
            lblStart.Text = value;
        }

        public void setTextOkButton(string value)
        {
            btnOK.Text = value;
        }

        public void setTextCancelButton(string value)
        {
            btnCancel.Text = value;
        }

        public void setTextBeatGroup(string value)
        {
            groupBeat.Text = value;
        }

        public int getValueEndNum()
        {
            return (int)numEnd.Value;
        }

        public bool isEnabledEndCheckbox()
        {
            return chkEnd.Enabled;
        }

        public void setTextEndCheckbox(string value)
        {
            chkEnd.Text = value;
        }

        public int getValueNumeratorNum()
        {
            return (int)numNumerator.Value;
        }

        public void setFont(string fontName, float fontSize)
        {
            AwtHost.Current.ApplyFontRecurse(this, new Cadencii.Gui.Font(new System.Drawing.Font(fontName, fontSize)));
        }

        public void setTextPositionGroup(string value)
        {
            groupPosition.Text = value;
        }

        public int getMaximumStartNum()
        {
            return (int)numStart.Maximum;
        }

        public int getMinimumStartNum()
        {
            return (int)numStart.Minimum;
        }

        public void setValueStartNum(int value)
        {
            numStart.Value = new decimal(value);
        }

        public int getValueStartNum()
        {
            return (int)numStart.Value;
        }

        public int getMinimumNumeratorNum()
        {
            return (int)numNumerator.Minimum;
        }

        public int getMaximumNumeratorNum()
        {
            return (int)numNumerator.Maximum;
        }

        public void setValueNumeratorNum(int value)
        {
            numNumerator.Value = new decimal(value);
        }

        public void setSelectedIndexDenominatorCombobox(int value)
        {
            comboDenominator.SelectedIndex = value;
        }

        public void addItemDenominatorCombobox(string value)
        {
            comboDenominator.Items.Add(value);
        }

        public void setMinimumStartNum(int value)
        {
            numStart.Minimum = value;
        }

        public void setMaximumStartNum(int value)
        {
            numStart.Maximum = value;
        }

        public void setMinimumEndNum(int value)
        {
            numEnd.Minimum = value;
        }

        public void setMaximumEndNum(int value)
        {
            numEnd.Maximum = value;
        }

        public void setValueEndNum(int value)
        {
            numEnd.Value = new decimal(value);
        }

        public int getMinimumEndNum()
        {
            return (int)numEnd.Minimum;
        }

        public int getMaximumEndNum()
        {
            return (int)numEnd.Maximum;
        }

        public void setTitle(string value)
        {
            this.Text = value;
        }

        public void removeAllItemsDenominatorCombobox()
        {
            comboDenominator.Items.Clear();
        }

        public void setEnabledEndCheckbox(bool value)
        {
            chkEnd.Enabled = value;
        }

        public void setEnabledStartNum(bool value)
        {
            numStart.Enabled = value;
        }

        public void setEnabledEndNum(bool value)
        {
            numEnd.Enabled = value;
        }

        #endregion



        #region イベントハンドラーの実装

        public void chkEnd_CheckedChanged(Object sender, EventArgs e)
        {
            mListener.checkboxEndCheckedChangedSlot();
        }

        public void btnOK_Click(Object sender, EventArgs e)
        {
            mListener.buttonOkClickedSlot();
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
            mListener.buttonCancelClickedSlot();
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
            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "FormBeatConfigUi.xml");
            this.ResumeLayout(false);
	}

		void RegisterEventHandlers ()
		{
			chkEnd.CheckedChanged += new System.EventHandler (chkEnd_CheckedChanged);
			btnOK.Click += new System.EventHandler (btnOK_Click);
			btnCancel.Click += new System.EventHandler (btnCancel_Click);
		}

		#pragma warning disable 0649
        UiGroupBox groupPosition;
        UiGroupBox groupBeat;
        UiButton btnOK;
        UiButton btnCancel;
        NumericUpDownEx numStart;
        UiCheckBox chkEnd;
        UiLabel lblStart;
        UiLabel lblBar2;
        UiLabel lblBar1;
        NumericUpDownEx numEnd;
        UiLabel label1;
        NumericUpDownEx numNumerator;
        UiLabel label2;
        UiComboBox comboDenominator;
		#pragma warning restore 0649

        #endregion
    }

}
