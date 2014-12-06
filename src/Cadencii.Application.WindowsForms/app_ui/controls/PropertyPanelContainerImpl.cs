#if ENABLE_PROPERTY
/*
 * PropertyPanelContainer.cs
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
using Cadencii.Gui;

using Cadencii.Gui.Toolkit;
using cadencii;

namespace Cadencii.Application.Controls
{

    public class PropertyPanelContainerImpl : UserControlImpl, PropertyPanelContainer
    {
        public const int _TITLE_HEIGHT = 29;
        public event StateChangeRequiredEventHandler StateChangeRequired;

        public PropertyPanelContainerImpl()
        {
            InitializeComponent();
            registerEventHandlers();
            setResources();
        }

        public void addComponent(UiControl c)
        {
            panelMain.Controls.Add(c);
            c.Dock = DockStyle.Fill;
        }

        public void panelTitle_MouseDoubleClick(Object sender, MouseEventArgs e)
        {
            handleRestoreWindow();
        }

        public void btnClose_Click(Object sender, EventArgs e)
        {
            handleClose();
        }

        public void btnWindow_Click(Object sender, EventArgs e)
        {
            handleRestoreWindow();
        }

        private void handleClose()
        {
            invokeStateChangeRequiredEvent(PanelState.Hidden);
        }

        private void handleRestoreWindow()
        {
            invokeStateChangeRequiredEvent(PanelState.Window);
        }

        private void invokeStateChangeRequiredEvent(PanelState state)
        {
            if (StateChangeRequired != null) {
                StateChangeRequired(this, state);
            }
        }

        /// <summary>
        /// javaは自動レイアウトなのでいらない
        /// </summary>
        private void panelMain_SizeChanged(Object sender, EventArgs e)
        {
            panelTitle.Left = 0;
            panelTitle.Top = 0;
            panelTitle.Height = _TITLE_HEIGHT;
            panelTitle.Width = this.Width;

            panelMain.Top = _TITLE_HEIGHT;
            panelMain.Left = 0;
            panelMain.Width = this.Width;
            panelMain.Height = this.Height - _TITLE_HEIGHT;
        }

        private void registerEventHandlers()
        {
            this.panelMain.SizeChanged += new EventHandler(panelMain_SizeChanged);
            this.btnClose.Click += new EventHandler(btnClose_Click);
            this.btnWindow.Click += new EventHandler(btnWindow_Click);
            this.panelTitle.MouseDoubleClick += panelTitle_MouseDoubleClick;
        }

        private void setResources()
        {
			this.btnClose.Image = cadencii.Properties.Resources.cross_small.ToAwt ();
			this.btnWindow.Image = cadencii.Properties.Resources.chevron_small_collapse.ToAwt ();
        }

        #region ui impl for C#
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
			ApplicationUIHost.Instance.ApplyXml (this, "PropertyPanelContainer.xml");
            this.ResumeLayout(false);
        }

        #endregion

        private UiPanel panelMain;
        private UiButton btnClose;
        private UiButton btnWindow;
        private UiPanel panelTitle;
        #endregion
    }

}
#endif
