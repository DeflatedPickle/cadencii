/*
 * FormBezierPointEditController.cs
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
using System.Collections.Generic;
using cadencii.apputil;
using cadencii;
using Cadencii.Gui;
using NMouseButtons = Cadencii.Gui.Toolkit.MouseButtons;
using NMouseEventArgs = Cadencii.Gui.Toolkit.MouseEventArgs;
using NMouseEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.MouseEventArgs>;
using Cadencii.Utilities;
using Cadencii.Gui.Toolkit;
using Cadencii.Media.Vsq;
using Cadencii.Application.Controls;
using Cadencii.Application.Media;

namespace Cadencii.Application.Forms
{

	public class FormBezierPointEditModel
	{
		BezierPoint m_point;
		int m_min;
		int m_max;
		/// <summary>
		/// 移動ボタンでデータ点または制御点を動かすためにマウスを強制的に動かす直前の，スクリーン上のマウス位置
		/// </summary>
		Point m_last_mouse_global_location;
		TrackSelector m_parent;
		bool m_btn_datapoint_downed = false;
		double m_min_opacity = 0.4;
		CurveType m_curve_type;
		int m_track;
		int m_chain_id = -1;
		int m_point_id = -1;
		BezierPickedSide m_picked_side = BezierPickedSide.BASE;
		/// <summary>
		/// 移動ボタンでデータ点または制御点を動かすためにマウスを強制的に動かした直後の，スクリーン上のマウス位置
		/// </summary>
		Point mScreenMouseDownLocation;
		FormBezierPointEditUi form;

		public FormBezierPointEditModel (TrackSelector parent,
		                                 CurveType curve_type,
		                                 int selected_chain_id,
		                                 int selected_point_id)
		{
			m_parent = parent;
			m_curve_type = curve_type;
			m_track = EditorManager.Selected;
			m_chain_id = selected_chain_id;
			m_point_id = selected_point_id;
		}

		public void Initialize (FormBezierPointEditUi form)
		{
			this.form = form;
			bool found = false;
			VsqFileEx vsq = MusicManager.getVsqFile ();
			BezierCurves attached = vsq.AttachedCurves.get (m_track - 1);
			List<BezierChain> chains = attached.get (m_curve_type);
			for (int i = 0; i < chains.Count; i++) {
				if (chains [i].id == m_chain_id) {
					found = true;
					break;
				}
			}
			if (!found) {
				return;
			}
			bool smooth = false;
			foreach (var bp in attached.getBezierChain(m_curve_type, m_chain_id).points) {
				if (bp.getID () == m_point_id) {
					m_point = bp;
					smooth =
                        (bp.getControlLeftType () != BezierControlType.None) ||
					(bp.getControlRightType () != BezierControlType.None);
					break;
				}
			}
			updateStatus ();
		}


		#region FormBezierPointEditorUiListener の実装

		public void buttonOkClick ()
		{
			try {
				int x, y;
				x = int.Parse (this.form.txtDataPointClock.Text);
				y = int.Parse (this.form.txtDataPointValue.Text);
				if (y < this.m_min || this.m_max < y) {
					DialogManager.ShowMessageBox (
						_ ("Invalid value"),
						_ ("Error"),
						MessageBoxButtons.OK,
						MessageBoxIcon.Error
					);
					return;
				}
				if (form.chkEnableSmooth.Checked) {
					x = int.Parse (this.form.txtLeftClock.Text);
					y = int.Parse (this.form.txtLeftValue.Text);
					x = int.Parse (this.form.txtRightClock.Text);
					y = int.Parse (this.form.txtRightValue.Text);
				}
				this.form.DialogResult = DialogResult.OK;
			} catch (Exception ex) {
				DialogManager.ShowMessageBox (
					_ ("Integer format error"),
					_ ("Error"),
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
				this.form.DialogResult = DialogResult.Cancel;
				Logger.write (typeof (FormBezierPointEditModel) + ".btnOK_Click; ex=" + ex + "\n");
			}
		}

		public void buttonCancelClick ()
		{
			this.form.DialogResult = DialogResult.Cancel;
		}

		public void buttonBackwardClick ()
		{
			this.handleMoveButtonClick (true);
		}

		public void buttonForwardClick ()
		{
			this.handleMoveButtonClick (false);
		}

		public void checkboxEnableSmoothCheckedChanged ()
		{
			bool value = this.form.chkEnableSmooth.Checked;
			this.form.txtLeftClock.Enabled = value;
			this.form.txtLeftValue.Enabled = value;
			this.form.btnLeft.Enabled = value;
			this.form.txtRightClock.Enabled = value;
			this.form.txtRightValue.Enabled  = value;
			this.form.btnRight.Enabled = value;

			bool old =
				(m_point.getControlLeftType () != BezierControlType.None) ||
				(m_point.getControlRightType () != BezierControlType.None);
			if (value) {
				m_point.setControlLeftType (BezierControlType.Normal);
				m_point.setControlRightType (BezierControlType.Normal);
			} else {
				m_point.setControlLeftType (BezierControlType.None);
				m_point.setControlRightType (BezierControlType.None);
			}
			this.form.txtLeftClock.Text = (((int) (m_point.getBase ().getX () + m_point.controlLeft.getX ())) + "");
			this.form.txtLeftValue.Text = (((int) (m_point.getBase ().getY () + m_point.controlLeft.getY ())) + "");
			this.form.txtRightClock.Text = (((int) (m_point.getBase ().getX () + m_point.controlRight.getX ())) + "");
			this.form.txtRightValue.Text = (((int) (m_point.getBase ().getY () + m_point.controlRight.getY ())) + "");
			m_parent.doInvalidate ();
		}

		public void buttonLeftMouseDown ()
		{
			this.handleMouseDown (BezierPickedSide.LEFT);
		}

		public void buttonRightMouseDown ()
		{
			this.handleMouseDown (BezierPickedSide.RIGHT);
		}

		public void buttonCenterMouseDown ()
		{
			this.handleMouseDown (BezierPickedSide.BASE);
		}

		public void buttonsMouseUp ()
		{
			m_btn_datapoint_downed = false;

			this.form.Opacity = 1.0;

			Point loc_on_screen = Screen.Instance.GetScreenMousePosition ();
			Point loc_trackselector = m_parent.LocationOnScreen;
			Point loc_on_trackselector = new Point (loc_on_screen.X - loc_trackselector.X, loc_on_screen.Y - loc_trackselector.Y);
			var event_arg = new NMouseEventArgs (NMouseButtons.Left, 0, loc_on_trackselector.X, loc_on_trackselector.Y, 0);
			m_parent.OnMouseUp (this, event_arg);
			Screen.Instance.SetScreenMousePosition (m_last_mouse_global_location);
			m_parent.doInvalidate ();
		}

		public void buttonsMouseMove ()
		{
			if (m_btn_datapoint_downed) {
				Point loc_on_screen = Screen.Instance.GetScreenMousePosition ();

				if (loc_on_screen.X == mScreenMouseDownLocation.X &&
				    loc_on_screen.Y == mScreenMouseDownLocation.Y) {
					// マウスが動いていないようならbailout
					return;
				}

				Point loc_trackselector = m_parent.LocationOnScreen;
				Point loc_on_trackselector =
					new Point (loc_on_screen.X - loc_trackselector.X, loc_on_screen.Y - loc_trackselector.Y);
				var event_arg =
					new NMouseEventArgs (NMouseButtons.Left, 0, loc_on_trackselector.X, loc_on_trackselector.Y, 0);
				BezierPoint ret = m_parent.HandleMouseMoveForBezierMove (event_arg, m_picked_side);

				this.form.txtDataPointClock.Text = (((int) ret.getBase ().getX ()) + "");
				this.form.txtDataPointValue.Text = (((int) ret.getBase ().getY ()) + "");
				this.form.txtLeftClock.Text = (((int) ret.getControlLeft ().getX ()) + "");
				this.form.txtLeftValue.Text = (((int) ret.getControlLeft ().getY ()) + "");
				this.form.txtRightClock.Text = (((int) ret.getControlRight ().getX ()) + "");
				this.form.txtRightValue.Text = (((int) ret.getControlRight ().getY ()) + "");

				m_parent.doInvalidate ();
			}
		}

		#endregion

		#region helper methods

		void handleMouseDown (BezierPickedSide side)
		{
			this.form.Opacity = m_min_opacity;

			m_last_mouse_global_location = Screen.Instance.GetScreenMousePosition ();
			PointD pd = m_point.getPosition (side);
			Point loc_on_trackselector = new Point (
				                             EditorManager.xCoordFromClocks ((int) pd.getX ()),
				                             m_parent.YCoordFromValue ((int) pd.getY ()));
			Point loc_topleft = m_parent.LocationOnScreen;
			mScreenMouseDownLocation = new Point (
				loc_topleft.X + loc_on_trackselector.X,
				loc_topleft.Y + loc_on_trackselector.Y);
			Screen.Instance.SetScreenMousePosition (mScreenMouseDownLocation);
			var event_arg = new NMouseEventArgs (
				                NMouseButtons.Left, 0,
				                loc_on_trackselector.X, loc_on_trackselector.Y, 0);
			m_parent.OnMouseDown (this, event_arg);
			m_picked_side = side;
			m_btn_datapoint_downed = true;
		}

		void updateStatus ()
		{
			this.form.txtDataPointClock.Text = (m_point.getBase ().getX () + "");
			this.form.txtDataPointValue.Text = (m_point.getBase ().getY () + "");
			this.form.txtLeftClock.Text = (((int) (m_point.getBase ().getX () + m_point.controlLeft.getX ())) + "");
			this.form.txtLeftValue.Text = (((int) (m_point.getBase ().getY () + m_point.controlLeft.getY ())) + "");
			this.form.txtRightClock.Text = (((int) (m_point.getBase ().getX () + m_point.controlRight.getX ())) + "");
			this.form.txtRightValue.Text = (((int) (m_point.getBase ().getY () + m_point.controlRight.getY ())) + "");
			bool smooth =
				(m_point.getControlLeftType () != BezierControlType.None) ||
				(m_point.getControlRightType () != BezierControlType.None);
			this.form.chkEnableSmooth.Checked = smooth;
			this.form.btnLeft.Enabled = smooth;
			this.form.btnRight.Enabled = smooth;
			m_min = m_curve_type.getMinimum ();
			m_max = m_curve_type.getMaximum ();
		}

		static string _ (string message)
		{
			return Messaging.getMessage (message);
		}

		void handleMoveButtonClick (bool backward)
		{
			// イベントの送り主によって動作を変える
			int delta = 1;
			if (backward) {
				delta = -1;
			}

			// 選択中のデータ点を検索し，次に選択するデータ点を決める
			BezierChain target = MusicManager.getVsqFile ().AttachedCurves.get (m_track - 1).getBezierChain (m_curve_type, m_chain_id);
			int index = -2;
			int size = target.size ();
			for (int i = 0; i < size; i++) {
				if (target.points [i].getID () == m_point_id) {
					index = i + delta;
					break;
				}
			}

			// 次に選択するデータ点のインデックスが有効範囲なら，選択を実行
			if (0 <= index && index < size) {
				// 選択を実行
				m_point_id = target.points [index].getID ();
				m_point = target.points [index];
				updateStatus ();
				m_parent.setEditingPointID (m_point_id);
				m_parent.doInvalidate ();

				// スクリーン上でデータ点が見えるようにする
				var main = m_parent.MainForm;
				if (main != null) {
					main.Model.EnsureClockVisibleOnPianoRoll ((int) m_point.getBase ().getX ());
				}
			}
		}

		#endregion
	}

}
