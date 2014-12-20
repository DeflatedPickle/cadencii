/*
 * FormRealtimeConfig.cs
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
using cadencii;

using cadencii.apputil;
using Cadencii.Gui;
using Cadencii.Utilities;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;

namespace Cadencii.Application.Forms
{

    public class FormRealtimeConfigImpl : FormImpl, FormRealtimeConfig
    {
        private bool m_game_ctrl_enabled = false;
        private double m_last_event_processed;
        Timer timer;

        public FormRealtimeConfigImpl()
        {
            InitializeComponent();
			timer = ApplicationUIHost.Create<Timer> (this.components);
            timer.Interval = 10;
            registerEventHandlers();
            setResources();
            AwtHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
        }

        #region public methods
        public float getSpeed()
        {
            return (float)numSpeed.Value;
        }
        #endregion

        #region event handlers
        public void FormRealtimeConfig_Load(Object sender, EventArgs e)
        {
			int num_joydev = EditorManager.GameControllerManager.GetNumberOfJoyPads ();
            m_game_ctrl_enabled = (num_joydev > 0);
            if (m_game_ctrl_enabled) {
                timer.Start();
            }
        }

        public void timer_Tick(Object sender, EventArgs e)
        {
            try {
                double now = PortUtil.getCurrentTime();
                double dt_ms = (now - m_last_event_processed) * 1000.0;
                //JoystickState state = m_game_ctrl.CurrentJoystickState;
				int len = EditorManager.GameControllerManager.GetNumberOfButtons(0);
				var stat = EditorManager.GameControllerManager.GetJoyPadStatus ();
				byte[] buttons = stat.Buttons;
				int pov0 = stat.Pov;
                bool btn_x = (buttons[EditorManager.editorConfig.GameControlerCross] > 0x00);
                bool btn_o = (buttons[EditorManager.editorConfig.GameControlerCircle] > 0x00);
                bool btn_tr = (buttons[EditorManager.editorConfig.GameControlerTriangle] > 0x00);
                bool btn_re = (buttons[EditorManager.editorConfig.GameControlerRectangle] > 0x00);
                bool pov_r = pov0 == 9000;  //(4500 <= pov0 && pov0 <= 13500);
                bool pov_l = pov0 == 27000; //(22500 <= pov[0] && pov[0] <= 31500);
                bool pov_u = pov0 == 0;     //(31500 <= pov[0] || (0 <= pov[0] && pov[0] <= 4500));
                bool pov_d = pov0 == 18000; //(13500 <= pov[0] && pov[0] <= 22500);
                bool L1 = (buttons[EditorManager.editorConfig.GameControlL1] > 0x00);
                bool R1 = (buttons[EditorManager.editorConfig.GameControlR1] > 0x00);
                bool L2 = (buttons[EditorManager.editorConfig.GameControlL2] > 0x00);
                bool R2 = (buttons[EditorManager.editorConfig.GameControlR2] > 0x00);
                bool SELECT = (buttons[EditorManager.editorConfig.GameControlSelect] > 0x00);
                if (dt_ms > EditorManager.editorConfig.GameControlerMinimumEventInterval) {
                    if (btnStart.Focused) {
                        if (btn_o) {
                            timer.Stop();
                            btnStart_Click(this, new EventArgs());
                            m_last_event_processed = now;
                        } else if (pov_r) {
                            btnCancel.Focus();
                            m_last_event_processed = now;
                        } else if (pov_d) {
                            numSpeed.Focus();
                            m_last_event_processed = now;
                        }
                    } else if (btnCancel.Focused) {
                        if (btn_o) {
                            timer.Stop();
							this.AsAwt ().DialogResult = Cadencii.Gui.DialogResult.Cancel;
                            Close();
                        } else if (pov_l) {
                            btnStart.Focus();
                            m_last_event_processed = now;
                        } else if (pov_d || pov_r) {
                            numSpeed.Focus();
                            m_last_event_processed = now;
                        }
                    } else if (numSpeed.Focused) {
                        if (R1) {
                            if (numSpeed.Value + numSpeed.Increment <= numSpeed.Maximum) {
                                numSpeed.Value = numSpeed.Value + numSpeed.Increment;
                                m_last_event_processed = now;
                            }
                        } else if (L1) {
                            if (numSpeed.Value - numSpeed.Increment >= numSpeed.Minimum) {
                                numSpeed.Value = numSpeed.Value - numSpeed.Increment;
                                m_last_event_processed = now;
                            }
                        } else if (pov_l) {
                            btnCancel.Focus();
                            m_last_event_processed = now;
                        } else if (pov_u) {
                            btnStart.Focus();
                            m_last_event_processed = now;
                        }
                    }
                }
            } catch (Exception ex) {
            }
        }

        public void btnStart_Click(Object sender, EventArgs e)
        {
			this.AsAwt ().DialogResult = Cadencii.Gui.DialogResult.OK;
            Close();
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
			this.AsAwt ().DialogResult = Cadencii.Gui.DialogResult.Cancel;
        }
        #endregion

        #region helper methods
        private void registerEventHandlers()
        {
            this.Load += new EventHandler(FormRealtimeConfig_Load);
            timer.Tick += new EventHandler(timer_Tick);
            btnStart.Click += new EventHandler(btnStart_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        private void setResources()
        {
        }
        #endregion

        #region UI implementation
        #region UI Impl for C#
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
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "FormRealtimeConfig.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		#pragma warning disable 0169,0649
        UiButton btnStart;
        UiButton btnCancel;
        UiLabel lblRealTimeInput;
        UiLabel lblSpeed;
        NumericUpDownEx numSpeed;
		#pragma warning restore 0169,0649
        #endregion
        #endregion
    }

}
