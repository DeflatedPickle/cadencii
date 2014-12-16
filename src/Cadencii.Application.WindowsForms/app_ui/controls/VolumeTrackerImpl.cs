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
		int logical_feder;
        private string m_number = "0";
        private string m_title = "";
        private int mTrack = 0;

        #region Constants
        private static readonly int[,] _KEY = {
            {55, 26}, 
            {51, 27},
            {47, 28},
            {42, 30},
            {38, 31},
            {35, 33},
            {31, 34},
            {28, 36},
            {24, 37},
            {21, 39},
            {18, 40},
            {15, 42},
            {12, 43},
            {10, 45},
            {7, 46},
            {5, 48},
            {2, 49},
            {0, 51},
            {-2, 52},
            {-5, 54},
            {-7, 55},
            {-10, 57},
            {-12, 58},
            {-15, 60},
            {-18, 61},
            {-21, 63},
            {-24, 64},
            {-28, 66},
            {-31, 67},
            {-35, 69},
            {-38, 70},
            {-42, 72},
            {-47, 73},
            {-51, 75},
            {-55, 76},
            {-60, 78},
            {-65, 79},
            {-70, 81},
            {-76, 82},
            {-81, 84},
            {-87, 85},
            {-93, 87},
            {-100, 88},
            {-107, 89},
            {-114, 91},
            {-121, 92},
            {-129, 94},
            {-137, 95},
            {-145, 97},
            {-154, 98},
            {-163, 100},
            {-173, 101},
            {-183, 103},
            {-193, 104},
            {-204, 106},
            {-215, 107},
            {-227, 109},
            {-240, 110},
            {-253, 112},
            {-266, 113},
            {-280, 115},
            {-295, 116},
            {-311, 118},
            {-327, 119},
            {-344, 121},
            {-362, 122},
            {-380, 124},
            {-399, 125},
            {-420, 127},
            {-441, 128},
            {-463, 130},
            {-486, 131},
            {-510, 133},
            {-535, 134},
            {-561, 136},
            {-589, 137},
            {-617, 139},
            {-647, 140},
            {-678, 142},
            {-711, 143},
            {-745, 145},
            {-781, 146},
            {-818, 148},
            {-857, 149},
            {-898, 151},
        };
        #endregion

        public event FederChangedEventHandler FederChanged;

        public event PanpotChangedEventHandler PanpotChanged;

        public event EventHandler MuteButtonClick;

        public event EventHandler SoloButtonClick;

        public VolumeTrackerImpl()
        {
            InitializeComponent();
            registerEventHandlers();
            setResources();
			this.DoubleBuffered = true;
            Muted = false;
            Solo = false;
        }

		public int Track {
			get { return mTrack; }
			set { mTrack = value; }
		}
		public double AmplifyL {
			get {
				double ret = 0.0;
				if (!Muted) {
					ret = VocaloSysUtil.getAmplifyCoeffFromFeder (Feder) * VocaloSysUtil.getAmplifyCoeffFromPanLeft (Panpot);
				}
				return ret;
			}
		}
		public double AmplifyR {
			get {
				double ret = 0.0;
				if (!Muted) {
					ret = VocaloSysUtil.getAmplifyCoeffFromFeder (Feder) * VocaloSysUtil.getAmplifyCoeffFromPanRight (Panpot);
				}
				return ret;
			}
		}

		public string Title {
			get { return m_title; }
			set {
				m_title = value;
	            updateTitle();
			}
		}

        private void updateTitle()
        {
            if (m_number == "") {
                lblTitle.Text = m_title;
            } else if (m_title == "") {
                lblTitle.Text = m_number;
            } else {
                lblTitle.Text = m_number + " " + m_title;
            }
        }

		public string Number {
			get { return m_number; }
			set {
				m_number = value;
				updateTitle ();
			}
		}

		public bool Muted {
			get { return chkMute.Checked; }
			set {
				bool old = chkMute.Checked;
				chkMute.Checked = value;
				chkMute.BackColor = value ? Colors.DimGray : Colors.White;
			}
		}

		public bool Solo {
			get { return chkSolo.Checked; }
			set {
				bool old = chkSolo.Checked;
				chkSolo.Checked = value;
				chkSolo.BackColor = value ? Colors.DarkCyan : Colors.White;
			}
		}

		public int Panpot {
			get { return trackPanpot.Value; }
			set { trackPanpot.Value = Math.Min (trackPanpot.Maximum, Math.Max (trackPanpot.Minimum, value)); }
		}

		public bool SoloButtonVisible {
			get { return chkSolo.Visible; }
			set { chkSolo.Visible = value; }
		}

		public int Feder {
			get { return logical_feder; }
			set {
				int old = logical_feder;
				logical_feder = value;
				if (old != logical_feder) {
					try {
						if (FederChanged != null) {
							FederChanged.Invoke (mTrack, logical_feder);
						}
					} catch (Exception ex) {
						Logger.StdErr ("VolumeTracker#setFeder; ex=" + ex);
					}
				}
				int v = 177 - FederToYCoord (logical_feder);
				trackFeder.Value = v;
			}
		}

        private static int YCoordToFeder(int y)
        {
            int feder = _KEY[0, 0];
            int min_diff = Math.Abs(_KEY[0, 1] - y);
            int index = 0;
            int len = _KEY.GetUpperBound(0) + 1;
            for (int i = 1; i < len; i++) {
                int diff = Math.Abs(_KEY[i, 1] - y);
                if (diff < min_diff) {
                    index = i;
                    min_diff = diff;
                    feder = _KEY[i, 0];
                }
            }
            return feder;
        }

        private static int FederToYCoord (int feder)
        {
            int y = _KEY[0, 1];
            int min_diff = Math.Abs(_KEY[0, 0] - feder);
            int index = 0;
            int len = _KEY.GetUpperBound(0) + 1;
            for (int i = 1; i < len; i++) {
                int diff = Math.Abs(_KEY[i, 0] - feder);
                if (diff < min_diff) {
                    index = i;
                    min_diff = diff;
                    y = _KEY[i, 1];
                }
            }
            return y;
        }

        #region event handlers
        private void txtPanpot_Enter(Object sender, EventArgs e)
        {
            txtPanpot.SelectAll();
        }

        private void txtFeder_Enter(Object sender, EventArgs e)
        {
            txtFeder.SelectAll();
        }

        public void VolumeTracker_Resize(Object sender, EventArgs e)
        {
			this.Width = VolumeTrackerController.WIDTH;
			this.Height = VolumeTrackerController.HEIGHT;
        }

        public void trackFeder_ValueChanged(Object sender, EventArgs e)
        {
			logical_feder = YCoordToFeder (151 - (trackFeder.Value - 26));
			txtFeder.Text = (logical_feder / 10.0) + "";
            try {
                if (FederChanged != null) {
					FederChanged.Invoke(mTrack, logical_feder);
                }
            } catch (Exception ex) {
                Logger.StdErr("VolumeTracker#trackFeder_ValueChanged; ex=" + ex);
            }
        }

        public void trackPanpot_ValueChanged(Object sender, EventArgs e)
        {
            var mPanpot = trackPanpot.Value;
            txtPanpot.Text = mPanpot + "";
            try {
                if (PanpotChanged != null) {
                    PanpotChanged.Invoke(mTrack, mPanpot);
                }
            } catch (Exception ex) {
                Logger.StdErr("VolumeTracker#trackPanpot_ValueChanged; ex=" + ex);
            }
        }

        public void txtFeder_KeyDown(Object sender, KeyEventArgs e)
        {
            if (((Keys) e.KeyCode & Keys.Enter) != Keys.Enter) {
                return;
            }
            try {
                int feder = (int)((float)double.Parse(txtFeder.Text) * 10.0f);
                if (55 < feder) {
                    feder = 55;
                }
                if (feder < -898) {
                    feder = -898;
                }
                Feder = feder;
                txtFeder.Text = Feder / 10.0f + "";
                txtFeder.Focus();
                txtFeder.SelectAll();
            } catch (Exception ex) {
                Logger.StdErr("VolumeTracker#txtFeder_KeyDown; ex=" + ex);
            }
        }

        public void txtPanpot_KeyDown(Object sender, KeyEventArgs e)
        {
            if (((Keys) e.KeyCode & Keys.Enter) != Keys.Enter) {
                return;
            }
            try {
                int panpot = int.Parse(txtPanpot.Text);
                if (panpot < -64) {
                    panpot = -64;
                }
                if (64 < panpot) {
                    panpot = 64;
                }
                Panpot = panpot;
                txtPanpot.Text = Panpot + "";
                txtPanpot.Focus();
                txtPanpot.SelectAll();
            } catch (Exception ex) {
                Logger.StdErr("VolumeTracker#txtPanpot_KeyDown; ex=" + ex);
            }
        }

        public void chkSolo_Click(Object sender, EventArgs e)
        {
            try {
                if (SoloButtonClick != null) {
                    SoloButtonClick.Invoke(this, e);
                }
            } catch (Exception ex) {
                Logger.StdErr("VolumeTracker#chkSolo_Click; ex=" + ex);
            }
        }

        public void chkMute_Click(Object sender, EventArgs e)
        {
            Muted = chkMute.Checked;
            try {
                if (MuteButtonClick != null) {
                    MuteButtonClick.Invoke(this, e);
                }
            } catch (Exception ex) {
                Logger.StdErr("VolumeTracker#chkMute_Click; ex=" + ex);
            }
        }
        #endregion

        private void registerEventHandlers()
        {
            trackFeder.ValueChanged += trackFeder_ValueChanged;
            trackPanpot.ValueChanged += trackPanpot_ValueChanged;
            txtPanpot.KeyDown += txtPanpot_KeyDown;
            txtFeder.KeyDown += txtFeder_KeyDown;
            chkSolo.Click += chkSolo_Click;
            chkMute.Click += chkMute_Click;
            txtFeder.Enter += txtFeder_Enter;
            txtPanpot.Enter += txtPanpot_Enter;
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

        private UiTrackBar trackFeder;
        private UiTrackBar trackPanpot;
        private UiTextBox txtPanpot;
        private UiLabel lblTitle;
        private UiTextBox txtFeder;
        private UiCheckBox chkMute;
        private UiCheckBox chkSolo;

        #endregion
    }
}
