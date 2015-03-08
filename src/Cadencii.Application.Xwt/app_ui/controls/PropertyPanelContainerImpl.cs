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
using Cadencii.Application.Models;

namespace Cadencii.Application.Controls
{

    public class PropertyPanelContainerImpl : UserControlImpl, PropertyPanelContainer
    {
        public const int _TITLE_HEIGHT = 29;
        public event StateChangeRequiredEventHandler StateChangeRequired;

		PropertyPanelContainerModel model;

        public PropertyPanelContainerImpl()
        {
			model = new PropertyPanelContainerModel (this);
			InitializeComponent();
			registerEventHandlers();
			setResources(btnClose, btnWindow);
        }

		/// <summary> 
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "PropertyPanelContainer.xml");
			ResumeLayout(false);
		}

		private void registerEventHandlers()
		{
			this.panelMain.SizeChanged += new EventHandler(this.panelMain_SizeChanged);
			this.btnClose.Click += new EventHandler(model.btnClose_Click);
			this.btnWindow.Click += new EventHandler(model.btnWindow_Click);
			this.panelTitle.MouseDoubleClick += model.panelTitle_MouseDoubleClick;
		}

		public void addComponent(UiControl c)
		{
			panelMain.AddControl(c);
			c.Dock = DockStyle.Fill;
		}

		public void setResources(UiButton btnClose, UiButton btnWindow)
		{
			btnClose.Image = cadencii.Properties.Resources.cross_small.ToGui ();
			btnWindow.Image = cadencii.Properties.Resources.chevron_small_collapse.ToGui ();
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

		/// <summary>
		/// javaは自動レイアウトなのでいらない
		/// </summary>
		private void panelMain_SizeChanged(Object sender, EventArgs e)
		{
			panelTitle.Left = 0;
			panelTitle.Top = 0;
			panelTitle.Height = _TITLE_HEIGHT;
			panelTitle.Width = ((UiControl) this).Width;

			panelMain.Top = _TITLE_HEIGHT;
			panelMain.Left = 0;
			panelMain.Width = ((UiControl) this).Width;
			panelMain.Height = ((UiControl) this).Height - _TITLE_HEIGHT;
		}

		private UiPanel panelMain;
		private UiButton btnClose;
		private UiButton btnWindow;
		private UiPanel panelTitle;
    }

}
#endif
