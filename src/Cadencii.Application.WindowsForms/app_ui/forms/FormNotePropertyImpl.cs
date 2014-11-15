/*
 * FormNotePropertyUiImpl.cs
 * Copyright © 2012 kbinani
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
using System.Drawing;
using cadencii.apputil;
using Keys = Cadencii.Gui.Toolkit.Keys;
using Cadencii.Gui;
using Rectangle = System.Drawing.Rectangle;
using Screen = System.Windows.Forms.Screen;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Forms
{
    public class FormNotePropertyImpl : FormImpl, UiFormNoteProperty
    {
        private FormNotePropertyListener listener;
        protected System.Windows.Forms.FormWindowState lastWindowState = System.Windows.Forms.FormWindowState.Normal;

        public FormNotePropertyImpl(FormNotePropertyListener listener)
        {
            this.listener = listener;
            InitializeComponent();
            AwtHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
        }


        #region FormNotePropertyUiの実装

        public void addComponent(object c)
        {
            if (c == null) {
                return;
            }
            if (!(c is Control)) {
                return;
            }
            Control control = (Control)c;
            this.Controls.Add(control);
            control.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        public bool isWindowMinimized()
        {
            return this.WindowState == System.Windows.Forms.FormWindowState.Minimized;
        }

        public void deiconfyWindow()
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
        }

        public void setTitle(string title)
        {
            this.Text = title;
        }

        public void close()
        {
            this.Close();
        }

        public void setMenuCloseAccelerator(Keys value)
        {
            this.menuClose.ShortcutKeys = (System.Windows.Forms.Keys) value;
        }

        public void setAlwaysOnTop(bool alwaysOnTop)
        {
            this.TopMost = alwaysOnTop;
        }

        public bool isAlwaysOnTop()
        {
            return this.TopMost;
        }

        public void setBounds(int x, int y, int width, int height)
        {
            this.Bounds = new System.Drawing.Rectangle(x, y, width, height);
        }

        public int getX()
        {
            return this.Location.X;
        }

        public int getY()
        {
            return this.Location.Y;
        }

        public int getWidth()
        {
            return this.Width;
        }

        public int getHeight()
        {
            return this.Height;
        }

        public void setVisible(bool visible)
        {
            this.Visible = visible;
        }

        public bool isVisible()
        {
            return this.Visible;
        }

        public int getWorkingAreaX()
        {
            Rectangle r = Screen.GetWorkingArea(this);
            return r.X;
        }

        public int getWorkingAreaY()
        {
            Rectangle r = Screen.GetWorkingArea(this);
            return r.Y;
        }

        public int getWorkingAreaWidth()
        {
            Rectangle r = Screen.GetWorkingArea(this);
            return r.Width;
        }

        public int getWorkingAreaHeight()
        {
            Rectangle r = Screen.GetWorkingArea(this);
            return r.Height;
        }

        public void hideWindow()
        {
            this.Visible = false;
        }

        #endregion


        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.lastWindowState != this.WindowState) {
                this.listener.windowStateChanged();
            }
            this.lastWindowState = this.WindowState;
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

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "FormNoteProperty.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

			Load += new System.EventHandler (FormNotePropertyUiImpl_Load);
			SizeChanged += new System.EventHandler (FormNotePropertyUiImpl_SizeChanged);
			FormClosing += new System.Windows.Forms.FormClosingEventHandler (FormNotePropertyUiImpl_FormClosing);
			LocationChanged += new System.EventHandler (FormNotePropertyUiImpl_LocationChanged);
			menuClose.Click += new System.EventHandler (menuClose_Click);

        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem menuWindow;
        private ToolStripMenuItem menuClose;

        private void FormNotePropertyUiImpl_Load(object sender, System.EventArgs e)
        {
            this.listener.onLoad();
        }

        private void menuClose_Click(object sender, System.EventArgs e)
        {
            this.listener.menuCloseClick();
        }

        private void FormNotePropertyUiImpl_SizeChanged(object sender, System.EventArgs e)
        {
            this.listener.locationOrSizeChanged();
        }

        private void FormNotePropertyUiImpl_LocationChanged(object sender, System.EventArgs e)
        {
            this.listener.locationOrSizeChanged();
        }

        private void FormNotePropertyUiImpl_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (e.CloseReason != System.Windows.Forms.CloseReason.UserClosing) {
                return;
            }
            e.Cancel = true;
            this.listener.formClosing();
        }
    }
}
