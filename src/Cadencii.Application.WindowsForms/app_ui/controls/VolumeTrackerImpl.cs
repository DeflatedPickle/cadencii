/*
 * VolumeTracker.cs
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
using System.Linq;
using cadencii;

using Cadencii.Gui;
using Cadencii.Media.Vsq;
using Cadencii.Application.Models;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
{

	public class VolumeTrackerImpl : UserControlImpl, VolumeTracker
    {
		double IAmplifierView.AmplifyL {
			get { return model.AmplifyL; }
		}

		double IAmplifierView.AmplifyR {
			get { return model.AmplifyR; }
		}

		VolumeTrackerModel model;

		public VolumeTrackerModel Model {
			get { return model; }
		}

		public VolumeTrackerImpl ()
		{
			model = new VolumeTrackerModel (this);
			InitializeComponent();
			registerEventHandlers();
			setResources();
			this.DoubleBuffered = true;
			model.Muted = false;
			model.Solo = false;
		}

        private void registerEventHandlers()
        {
            trackFeder.ValueChanged += model.trackFeder_ValueChanged;
			trackPanpot.ValueChanged += model.trackPanpot_ValueChanged;
			txtPanpot.KeyDown += model.txtPanpot_KeyDown;
			txtFeder.KeyDown += model.txtFeder_KeyDown;
			chkSolo.Click += model.chkSolo_Click;
			chkMute.Click += model.chkMute_Click;
			txtFeder.Enter += model.txtFeder_Enter;
			txtPanpot.Enter += model.txtPanpot_Enter;
        }

        private void setResources()
        {
        }

        #region UI Impl for C#

		public new bool DoubleBuffered {
			get { return base.DoubleBuffered; }
			set { base.DoubleBuffered = value; }
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

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "VolumeTracker.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		public UiTrackBar trackFeder { get; set; }
		public UiTrackBar trackPanpot { get; set; }
		public UiTextBox txtPanpot { get; set; }
		public UiLabel lblTitle { get; set; }
		public UiTextBox txtFeder { get; set; }
		public UiCheckBox chkMute { get; set; }
		public UiCheckBox chkSolo { get; set; }

        #endregion
    }
}
