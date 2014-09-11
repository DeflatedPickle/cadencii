/*
 * FormPluginUi.cs
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
using cadencii.java.awt;


#if ENABLE_AQUESTONE
using System;
using System.Windows.Forms;
using cadencii.windows.forms;

namespace cadencii
{

    public class FormPluginUi : Form
    {
        private System.ComponentModel.IContainer components;
        public IntPtr childWnd = IntPtr.Zero;
        private double lastDrawn = 0.0;

        public FormPluginUi()
        {
            this.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);
            this.SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(FormPluginUi_FormClosing);
            // FIXME: bring this back
            //this.Icon = Properties.Resources._switch;
        }

		public bool IsOpened {
			get;
			set;
		}

		internal Dimension WindowRect = new Dimension(373, 158);

        public void FormPluginUi_FormClosing(Object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormPluginUi
            // 
            this.ClientSize = new System.Drawing.Size(334, 164);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPluginUi";
            this.ResumeLayout(false);

        }

        public void invalidateUi()
        {
            double now = PortUtil.getCurrentTime();

            if (now - lastDrawn > 0.04) {
                if (childWnd != IntPtr.Zero) {
                    bool ret = false;
                    try {
                        ret = win32.InvalidateRect(childWnd, IntPtr.Zero, false);
                    } catch (Exception ex) {
                        serr.println("FormPluginUi#invalidateUi; ex=" + ex);
                        ret = false;
                    }
                    lastDrawn = now;
                }
            }
        }
		
        internal void UpdatePluginUiRect()
        {
                try {
                    win32.EnumChildWindows(Handle, EnumChildProc, 0);
                } catch (Exception ex) {
                    serr.println("vstidrv#updatePluginUiRect; ex=" + ex);
                }
        }

        private bool EnumChildProc(IntPtr hwnd, int lParam)
        {
            RECT rc = new RECT();
            try {
                win32.GetWindowRect(hwnd, ref rc);
            } catch (Exception ex) {
                serr.println("vstidrv#enumChildProc; ex=" + ex);
            }
            childWnd = hwnd;
            WindowRect = new Dimension(rc.right - rc.left, rc.bottom - rc.top);
            return false; //最初のやつだけ検出できればおｋなので
        }
    }

}
#endif
