using System;
using Cadencii.Application.Controls;
using cadencii;
using Cadencii.Media.Vsq;
using Cadencii.Gui.Toolkit;
using Cadencii.Gui;

namespace Cadencii.Application.Models
{
	public class VolumeTrackerModel
	{
		VolumeTracker control;

		public VolumeTrackerModel (VolumeTracker control)
		{
			this.control = control;
		}

		public event FederChangedEventHandler FederChanged;

		public event PanpotChangedEventHandler PanpotChanged;

		public event EventHandler MuteButtonClick;

		public event EventHandler SoloButtonClick;

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

		int logical_feder;
		private string m_number = "0";
		private string m_title = "";
		private int mTrack = 0;

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
				control.lblTitle.Text = m_title;
			} else if (m_title == "") {
				control.lblTitle.Text = m_number;
			} else {
				control.lblTitle.Text = m_number + " " + m_title;
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
			get { return control.chkMute.Checked; }
			set {
				bool old = control.chkMute.Checked;
				control.chkMute.Checked = value;
				control.chkMute.BackColor = value ? Colors.DimGray : Colors.White;
			}
		}

		public bool Solo {
			get { return control.chkSolo.Checked; }
			set {
				bool old = control.chkSolo.Checked;
				control.chkSolo.Checked = value;
				control.chkSolo.BackColor = value ? Colors.DarkCyan : Colors.White;
			}
		}

		public int Panpot {
			get { return control.trackPanpot.Value; }
			set { control.trackPanpot.Value = Math.Min (control.trackPanpot.Maximum, Math.Max (control.trackPanpot.Minimum, value)); }
		}

		public bool SoloButtonVisible {
			get { return control.chkSolo.Visible; }
			set { control.chkSolo.Visible = value; }
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
				control.trackFeder.Value = v;
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
		public void txtPanpot_Enter(Object sender, EventArgs e)
		{
			control.txtPanpot.SelectAll();
		}

		public void txtFeder_Enter(Object sender, EventArgs e)
		{
			control.txtFeder.SelectAll();
		}

		public void VolumeTracker_Resize(Object sender, EventArgs e)
		{
			control.Size = new Dimension (VolumeTrackerController.WIDTH, VolumeTrackerController.HEIGHT);
		}

		public void trackFeder_ValueChanged(Object sender, EventArgs e)
		{
			logical_feder = YCoordToFeder (151 - (control.trackFeder.Value - 26));
			control.txtFeder.Text = (logical_feder / 10.0) + "";
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
			var mPanpot = control.trackPanpot.Value;
			control.txtPanpot.Text = mPanpot + "";
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
				int feder = (int)((float)double.Parse(control.txtFeder.Text) * 10.0f);
				if (55 < feder) {
					feder = 55;
				}
				if (feder < -898) {
					feder = -898;
				}
				Feder = feder;
				control.txtFeder.Text = Feder / 10.0f + "";
				control.txtFeder.Focus();
				control.txtFeder.SelectAll();
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
				int panpot = int.Parse(control.txtPanpot.Text);
				if (panpot < -64) {
					panpot = -64;
				}
				if (64 < panpot) {
					panpot = 64;
				}
				Panpot = panpot;
				control.txtPanpot.Text = Panpot + "";
				control.txtPanpot.Focus();
				control.txtPanpot.SelectAll();
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
			Muted = control.chkMute.Checked;
			try {
				if (MuteButtonClick != null) {
					MuteButtonClick.Invoke(this, e);
				}
			} catch (Exception ex) {
				Logger.StdErr("VolumeTracker#chkMute_Click; ex=" + ex);
			}
		}
		#endregion
	}
}

