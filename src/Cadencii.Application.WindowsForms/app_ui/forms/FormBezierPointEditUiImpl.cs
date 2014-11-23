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
using Cadencii.Utilities;


namespace Cadencii.Application.Forms
{
    public class FormBezierPointEditUiImpl : FormImpl, FormBezierPointEditUi
    {
		private FormBezierPointEditModel model;

		public FormBezierPointEditUiImpl(TrackSelector parent,
			CurveType curve_type,
			int selected_chain_id,
			int selected_point_id)
        {
			this.model = new FormBezierPointEditModel (parent, curve_type, selected_chain_id, selected_point_id);
			applyLanguage ();
            InitializeComponent();
			model.Initialize (this);
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

		// FIXME: this should rather be implemented as in XML UI.
		public void applyLanguage()
		{
			this.Text = _("Edit Bezier Data Point");

			this.groupDataPoint.Text = (_("Data Poin"));
			this.lblDataPointClock.Text = _("Clock");
			this.lblDataPointValue.Text = _("Value");

			this.groupLeft.Text = (_("Left Control Point"));
			this.lblLeftClock.Text = (_("Clock"));
			this.lblLeftValue.Text = (_("Value"));

			this.groupRight.Text = (_("Right Control Point"));
			this.lblRightClock.Text = (_("Clock"));
			this.lblRightValue.Text = (_("Value"));

			this.chkEnableSmooth.Text = _("Smooth");
		}

        #endregion


        #region helper methods

        private static string _(string message)
        {
            return Messaging.getMessage(message);
        }

        private void registerEventHandlers()
        {
			btnOK.Click += (o,e) => model.buttonOkClick ();
			btnCancel.Click += (o,e) => model.buttonCancelClick ();
			chkEnableSmooth.CheckedChanged += (o,e) => model.checkboxEnableSmoothCheckedChanged ();
			btnLeft.MouseMove += (o,e) => model.buttonsMouseMove ();
			btnLeft.MouseDown += (o,e) => model.buttonLeftMouseDown ();
			btnLeft.MouseUp += (o,e) => model.buttonsMouseUp();
			btnDataPoint.MouseMove += (o,e) => model.buttonsMouseMove();
			btnDataPoint.MouseDown += (o,e) => model.buttonCenterMouseDown ();
			btnDataPoint.MouseUp += (o,e) => model.buttonsMouseUp();
			btnRight.MouseMove += (o,e) => model.buttonsMouseMove();
			btnRight.MouseDown += (o,e) => model.buttonLeftMouseDown ();
			btnRight.MouseUp += (o,e) => model.buttonsMouseUp();
			btnBackward.Click += (o,e) => model.buttonBackwardClick();
			btnForward.Click += (o,e) => model.buttonForwardClick();
        }

        private void setResources()
        {
			this.btnLeft.Image = cadencii.Properties.Resources.target__pencil.ToAwt ();
			this.btnDataPoint.Image = cadencii.Properties.Resources.target__pencil.ToAwt ();
			this.btnRight.Image = cadencii.Properties.Resources.target__pencil.ToAwt ();
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

        private UiButton btnCancel;
        private UiButton btnOK;
		public UiCheckBox chkEnableSmooth { get; set; }
		public UiLabel lblLeftValue { get; set; }
		public UiLabel lblLeftClock { get; set; }
		public NumberTextBox txtLeftValue { get; set; }
		public NumberTextBox txtLeftClock { get; set; }
		public UiGroupBox groupDataPoint { get; set; }
		public UiGroupBox groupLeft { get; set; }
		public UiGroupBox groupRight { get; set; }
		public UiLabel lblDataPointValue { get; set; }
		public NumberTextBox txtDataPointClock { get; set; }
		public UiLabel lblDataPointClock { get; set; }
		public NumberTextBox txtDataPointValue { get; set; }
		public UiLabel lblRightValue { get; set; }
		public NumberTextBox txtRightClock { get; set; }
		public UiLabel lblRightClock { get; set; }
		public NumberTextBox txtRightValue { get; set; }
        private UiButton btnDataPoint;
		public UiButton btnLeft { get; set; }
		public UiButton btnRight { get; set; }
        private UiButton btnBackward;
        private UiButton btnForward;

    }

}
