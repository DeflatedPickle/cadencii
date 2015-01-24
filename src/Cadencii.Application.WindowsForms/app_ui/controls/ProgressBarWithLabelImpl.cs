/*
 * ProgressBarWithLabelUi.cs
 * Copyright © 2011 kbinani
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;

namespace Cadencii.Application.Forms
{

    public class ProgressBarWithLabelUiImpl : UserControlImpl, ProgressBarWithLabel
    {
        private UiProgressBar progressBar1;
        private UiLabel label1;
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ProgressBarWithLabelUiImpl()
        {
            InitializeComponent();
        }

        public string Text {
			get { return label1.Text; }
			set { label1.Text = value; }
		}

		public int Progress {
			set {
				if (value < progressBar1.Minimum)
					value = progressBar1.Minimum;
				if (progressBar1.Maximum < value)
					value = progressBar1.Maximum;
				progressBar1.Value = value;
			}

			get {
				return progressBar1.Value;
			}
		}

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

        #region コンポーネント デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar1 = ApplicationUIHost.Create<UiProgressBar> ();
			this.label1 = ApplicationUIHost.Create<UiLabel> ();
            this.SuspendLayout();
            //
            // progressBar1
            //
            this.progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.progressBar1.Location = new Cadencii.Gui.Point(16, 11);
            this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new Size (290, 10);
            this.progressBar1.TabIndex = 0;
            //
            // label1
            //
            this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.label1.Location = new Cadencii.Gui.Point (14, 24);
            this.label1.Name = "label1";
			this.label1.Size = new  Cadencii.Gui.Size(292, 12);
            this.label1.TabIndex = 1;
            //
            // ProgressBarWithLabelUi
            //
			var control = (UiUserControl) this;
			control.AutoScaleDimensions = new Cadencii.Gui.Size(6, 12);
			control.AutoScaleMode = Cadencii.Gui.Toolkit.AutoScaleMode.Font;
			control.AddControl(this.label1);
			control.AddControl(this.progressBar1);
			control.Name = "ProgressBarWithLabelUi";
			control.Size = new  Cadencii.Gui.Size(322, 45);
			control.ResumeLayout(false);
        }

        #endregion
    }

}
