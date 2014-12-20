/*
 * FormGameControlerConfig.cs
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
using System.Collections.Generic;
using cadencii.apputil;
using cadencii;
using cadencii.java.util;

using Cadencii.Gui;
using Cadencii.Platform.Windows;
using Cadencii.Gui.Toolkit;



namespace Cadencii.Application.Forms
{

    public class FormGameControllerConfigImpl : FormImpl, FormGameControllerConfig
    {
        private List<int> m_list = new List<int>();
        private List<int> m_povs = new List<int>();
        private int index;
        private Timer timer;

        public FormGameControllerConfigImpl()
        {
            InitializeComponent();

			timer = ApplicationUIHost.Create<Timer> (this.components);
            registerEventHandlers();
            setResources();
            for (int i = 0; i < 10; i++) {
                m_list.Add(-1);
            }
            for (int i = 0; i < 4; i++) {
                m_povs.Add(int.MinValue);
            }
            applyLanguage();
			int num_dev = EditorManager.GameControllerManager.GetNumberOfJoyPads ();
            if (num_dev > 0) {
				pictButton.Image = Resources.btn1;
                progressCount.Maximum = 8;
                progressCount.Minimum = 0;
                progressCount.Value = 0;
                index = 1;
                btnSkip.Enabled = true;
                btnReset.Enabled = true;
                timer.Start();
            } else {
                btnSkip.Enabled = false;
                btnReset.Enabled = false;
            }
            AwtHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
        }

        #region public methods
        public void applyLanguage()
        {
			int num_dev = EditorManager.GameControllerManager.GetNumberOfJoyPads ();
            if (num_dev > 0) {
                lblMessage.Text = _("Push buttons in turn as shown below");
            } else {
                lblMessage.Text = _("Game controler is not available");
            }
            this.Text = _("Game Controler Configuration");
            btnOK.Text = _("OK");
            btnCancel.Text = _("Cancel");
            btnReset.Text = _("Reset And Exit");
            btnSkip.Text = _("Skip");
        }

        public int getRectangle()
        {
            return m_list[0];
        }

        public int getTriangle()
        {
            return m_list[1];
        }

        public int getCircle()
        {
            return m_list[2];
        }

        public int getCross()
        {
            return m_list[3];
        }

        public int getL1()
        {
            return m_list[4];
        }

        public int getL2()
        {
            return m_list[5];
        }

        public int getR1()
        {
            return m_list[6];
        }

        public int getR2()
        {
            return m_list[7];
        }

        public int getSelect()
        {
            return m_list[8];
        }

        public int getStart()
        {
            return m_list[9];
        }

        public int getPovDown()
        {
            return m_povs[0];
        }

        public int getPovLeft()
        {
            return m_povs[1];
        }

        public int getPovUp()
        {
            return m_povs[2];
        }

        public int getPovRight()
        {
            return m_povs[3];
        }
        #endregion

        #region helper methods
        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void registerEventHandlers()
        {
            timer.Tick += new EventHandler(timer_Tick);
            btnSkip.Click += new EventHandler(btnSkip_Click);
            btnReset.Click += new EventHandler(btnReset_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        private void setResources()
        {
        }
        #endregion

        #region event handlers
        public void timer_Tick(Object sender, EventArgs e)
        {
            //int num_btn = vstidrv.JoyGetNumButtons( 0 );
            byte[] btn;
            int pov;
			var stat = EditorManager.GameControllerManager.GetJoyPadStatus ();
			btn = stat.Buttons;
			pov = stat.Pov;

#if DEBUG
            CDebug.WriteLine("FormGameControlerConfig+timer_Tick");
            CDebug.WriteLine("    pov=" + pov);
#endif
            bool added = false;
            if (index <= 4) {
                if (pov >= 0 && !m_povs.Contains(pov)) {
                    m_povs[index - 1] = pov;
                    added = true;
                }
            } else {
                for (int i = 0; i < btn.Length; i++) {
                    if (btn[i] > 0x0 && !m_list.Contains(i)) {
                        m_list[index - 5] = i;
                        added = true;
                        break;
                    }
                }
            }
            if (added) {
                if (index <= 8) {
                    progressCount.Value = index;
                } else if (index <= 12) {
                    progressCount.Value = index - 8;
                } else {
                    progressCount.Value = index - 12;
                }

                if (index == 8) {
                    pictButton.Image = Resources.btn2;
                    progressCount.Value = 0;
                    progressCount.Maximum = 4;
                } else if (index == 12) {
                    pictButton.Image = Resources.btn3;
                    progressCount.Value = 0;
                    progressCount.Maximum = 2;
                }
                if (index == 14) {
                    btnSkip.Enabled = false;
                    btnOK.Enabled = true;
                    timer.Stop();
                }
                index++;
            }
        }

        public void btnSkip_Click(Object sender, EventArgs e)
        {
            if (index <= 4) {
                m_povs[index - 1] = int.MinValue;
            } else {
                m_list[index - 5] = -1;
            }
            if (index <= 8) {
                progressCount.Value = index;
            } else if (index <= 12) {
                progressCount.Value = index - 8;
            } else {
                progressCount.Value = index - 12;
            }

            if (index == 8) {
                pictButton.Image = Resources.btn2;
                progressCount.Value = 0;
                progressCount.Maximum = 4;
            } else if (index == 12) {
                pictButton.Image = Resources.btn3;
                progressCount.Value = 0;
                progressCount.Maximum = 2;
            }
            if (index == 14) {
                btnSkip.Enabled = false;
                btnOK.Enabled = true;
                timer.Stop();
            }
            index++;
        }

        public void btnReset_Click(Object sender, EventArgs e)
        {
            m_list[0] = 3; // □
            m_list[1] = 0; // △
            m_list[2] = 1; // ○
            m_list[3] = 2; // ×
            m_list[4] = 4; // L1
            m_list[5] = 6; // L2
            m_list[6] = 5; // R1
            m_list[7] = 7; // R2
            m_list[8] = 8; // SELECT
            m_list[9] = 9; // START
            m_povs[0] = 18000; // down
            m_povs[1] = 27000; // left
            m_povs[2] = 0; // up
            m_povs[3] = 9000; // right
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        public void btnOK_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
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
			this.components = new System.ComponentModel.Container();
            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "FormGameControllerConfig.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#pragma warning disable 0169,0649
        UiLabel lblMessage;
        UiPictureBox pictButton;
        UiProgressBar progressCount;
        UiButton btnSkip;
        UiButton btnOK;
        UiButton btnCancel;
        UiButton btnReset;
		#pragma warning restore 0169,0649

        #endregion

    }

}
