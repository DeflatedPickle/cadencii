/*
 * BSplitContainer.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.apputil.
 *
 * cadencii.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.ComponentModel;
using cadencii;

using AnchorStyles = Cadencii.Gui.Toolkit.AnchorStyles;
using UiControl = Cadencii.Gui.Toolkit.UiControl;
using Orientation = Cadencii.Gui.Toolkit.Orientation;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;

namespace Cadencii.Application.Models
{
	[Serializable]
	public class BSplitContainerModel
	{
		private Cadencii.Gui.Toolkit.Orientation m_orientation = Cadencii.Gui.Toolkit.Orientation.Horizontal;
		private int m_splitter_distance = 50;
		private int m_panel1_min = 25;
		private int m_panel2_min = 25;
		private int m_splitter_width = 4;
		private bool m_splitter_moving = false;
		private int m_splitter_distance_draft = 50;
		private bool m_splitter_fixed = false;
		private FixedPanel m_fixed_panel;
		private int m_panel2_distance = 1;
		private double m_distance_rate = 0.5;

		BSplitContainer control;

		public BSplitContainerModel(BSplitContainer control)
		{
			this.control = control;
		}

		public int Width {
			get { return control.Width; }
		}
		public int Height {
			get { return control.Height; }
		}

		public void Initialize()
		{
			Splitter.MouseMove += control.SplitterMouseMove;
			Splitter.MouseDown += m_lbl_splitter_MouseDown;
			Splitter.MouseUp += m_lbl_splitter_MouseUp;
			Panel2.BorderStyleChanged += m_panel2_BorderStyleChanged;
			Panel2.SizeChanged += m_panel2_SizeChanged;
			Panel1.BorderStyleChanged += m_panel1_BorderStyleChanged;
			Panel1.SizeChanged += m_panel1_SizeChanged;
			control.Paint += (o, e) => this.SplitContainerEx_Paint (o, new Cadencii.Gui.Toolkit.PaintEventArgs () { Graphics = new Cadencii.Gui.Graphics () { NativeGraphics = e.Graphics } });

			if (m_orientation == Cadencii.Gui.Toolkit.Orientation.Horizontal) {
				Splitter.Cursor = Cadencii.Gui.Cursors.VSplit;
			} else {
				Splitter.Cursor = Cadencii.Gui.Cursors.HSplit;
			}
			if (m_orientation == Cadencii.Gui.Toolkit.Orientation.Horizontal) {
				m_panel2_distance = this.Width - m_splitter_distance;
			} else {
				m_panel2_distance = this.Height - m_splitter_distance;
			}
			m_distance_rate = m_splitter_distance / (double)(m_splitter_distance + m_panel2_distance);
		}

		public void setPanel2Hidden(bool value)
		{
			if (value) {
				if (m_orientation == Cadencii.Gui.Toolkit.Orientation.Horizontal) {
					setDividerLocation(Width);
				} else {
					setDividerLocation(Height);
				}
				SplitterFixed = true;
			} else {
				SplitterFixed = false;
			}
		}

		public void setPanel1Hidden(bool value)
		{
			if (value) {
				setDividerLocation(0);
				SplitterFixed = true;
			} else {
				SplitterFixed = false;
			}
		}

		public FixedPanel FixedPanel
		{
			get
			{
				return m_fixed_panel;
			}
			set
			{
				var old = m_fixed_panel;
				m_fixed_panel = value;
				if (m_fixed_panel != Cadencii.Gui.Toolkit.FixedPanel.None && m_fixed_panel != old) {
					if (m_fixed_panel == Cadencii.Gui.Toolkit.FixedPanel.Panel1) {
						Panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
						if (m_orientation == Orientation.Vertical) {
							Panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
						} else {
							Panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
						}
					} else {
						Panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
						if (m_orientation == Orientation.Vertical) {
							Panel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
						} else {
							Panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
						}
					}
				}
			}
		}

		public bool SplitterFixed
		{
			get
			{
				return m_splitter_fixed;
			}
			set
			{
				m_splitter_fixed = value;
				if (m_splitter_fixed) {
					Splitter.Cursor = Cursors.Default;
				} else {
					if (m_orientation == Orientation.Horizontal) {
						Splitter.Cursor = Cursors.VSplit;
					} else {
						Splitter.Cursor = Cursors.HSplit;
					}
				}
			}
		}

		private void SplitContainerEx_Paint(object sender, Cadencii.Gui.Toolkit.PaintEventArgs e)
		{
			bool panel1_visible = true;
			if (Orientation == Cadencii.Gui.Toolkit.Orientation.Horizontal) {
				if (Panel1.Width == 0) {
					panel1_visible = false;
				}
			} else {
				if (Panel1.Height == 0) {
					panel1_visible = false;
				}
			}
			if (Panel1.BorderStyle == Cadencii.Gui.Toolkit.BorderStyle.FixedSingle && panel1_visible) {
				e.Graphics.drawRect(Panel1.BorderColor,
					new Cadencii.Gui.Rectangle(Panel1.Left - 1, Panel1.Top - 1, Panel1.Width + 1, Panel1.Height + 1));
			}

			bool panel2_visible = true;
			if (Orientation == Orientation.Horizontal) {
				if (Panel2.Width == 0) {
					panel2_visible = false;
				}
			} else {
				if (Panel2.Height == 0) {
					panel2_visible = false;
				}
			}
			if (Panel2.BorderStyle == Cadencii.Gui.Toolkit.BorderStyle.FixedSingle && panel2_visible) {
				e.Graphics.drawRect(Panel2.BorderColor,
					new Cadencii.Gui.Rectangle(Panel2.Left - 1, Panel2.Top - 1, Panel2.Width + 1, Panel2.Height + 1));
			}
		}

		private void m_panel2_BorderStyleChanged(object sender, EventArgs e)
		{
			UpdateLayout(m_splitter_distance, m_splitter_width, m_panel1_min, m_panel2_min, false);
		}

		private void m_panel1_BorderStyleChanged(object sender, EventArgs e)
		{
			UpdateLayout(m_splitter_distance, m_splitter_width, m_panel1_min, m_panel2_min, false);
		}

		private void m_panel2_SizeChanged(object sender, EventArgs e)
		{
			Panel2.Invalidate(true);
		}

		private void m_panel1_SizeChanged(object sender, EventArgs e)
		{
			Panel1.Invalidate(true);
		}

		public int getPanel1MinSize()
		{
			return m_panel1_min;
		}

		public void setPanel1MinSize(int value)
		{
			int min_splitter_distance = value;
			if (m_splitter_distance < min_splitter_distance && min_splitter_distance > 0) {
				m_splitter_distance = min_splitter_distance;
			}
			UpdateLayout(m_splitter_distance, m_splitter_width, value, m_panel2_min, false);
		}

		public int Panel1MinSize
		{
			get
			{
				return getPanel1MinSize();
			}
			set
			{
				setPanel1MinSize(value);
			}
		}

		public int getPanel2MinSize()
		{
			return m_panel2_min;
		}

		public void setPanel2MinSize(int value)
		{
			int max_splitter_distance = (m_orientation == Orientation.Horizontal) ?
				this.Width - m_splitter_width - value :
				this.Height - m_splitter_width - value;
			if (m_splitter_distance > max_splitter_distance && max_splitter_distance > 0) {
				m_splitter_distance = max_splitter_distance;
			}
			UpdateLayout(m_splitter_distance, m_splitter_width, m_panel1_min, value, false);
		}

		public int Panel2MinSize
		{
			get
			{
				return getPanel2MinSize();
			}
			set
			{
				setPanel2MinSize(value);
			}
		}

		public int SplitterWidth
		{
			get
			{
				return m_splitter_width;
			}
			set
			{
				if (value < 0) {
					value = 0;
				}
				UpdateLayout(m_splitter_distance, value, m_panel1_min, m_panel2_min, false);
			}
		}

		public int getDividerSize()
		{
			return this.SplitterWidth;
		}

		public void setDividerSize(int value)
		{
			this.SplitterWidth = value;
		}

		private bool UpdateLayout(int splitter_distance, int splitter_width, int panel1_min, int panel2_min, bool check_only)
		{
			Cadencii.Gui.Point mouse = control.PointToClient(Screen.Instance.GetScreenMousePosition ());
			int pad1 = (Panel1.BorderStyle == Cadencii.Gui.Toolkit.BorderStyle.FixedSingle) ? 1 : 0;
			int pad2 = (Panel2.BorderStyle == Cadencii.Gui.Toolkit.BorderStyle.FixedSingle) ? 1 : 0;
			if (m_orientation == Orientation.Horizontal) {
				int p1 = splitter_distance;
				if (p1 < 0) {
					p1 = 0;
				} else if (this.Width < p1 + splitter_width) {
					p1 = this.Width - splitter_width;
				}
				int p2 = this.Width - p1 - splitter_width;
				if (check_only) {
					if (p1 < panel1_min || p2 < panel2_min) {
						return false;
					}
				} else {
					if (p1 < panel1_min) {
						p1 = panel1_min;
					}
					p2 = this.Width - p1 - splitter_width;
					if (p2 < panel2_min) {
						p2 = panel2_min;
						//return false;
					}
				}
				if (!check_only) {
					Panel1.Left = pad1;
					Panel1.Top = pad1;
					Panel1.Width = (p1 - 2 * pad1 >= 0) ? (p1 - 2 * pad1) : 0;
					Panel1.Height = (this.Height - 2 * pad1 >= 0) ? (this.Height - 2 * pad1) : 0;

					Panel2.Left = p1 + splitter_width + pad2;
					Panel2.Top = pad2;
					Panel2.Width = (p2 - 2 * pad2 >= 0) ? (p2 - 2 * pad2) : 0;
					Panel2.Height = (this.Height - 2 * pad2 >= 0) ? (this.Height - 2 * pad2) : 0;

					m_splitter_distance = p1;
					m_panel2_distance = this.Width - m_splitter_distance;
					m_distance_rate = m_splitter_distance / (double)(m_splitter_distance + m_panel2_distance);

					m_splitter_width = splitter_width;
					m_panel1_min = panel1_min;
					m_panel2_min = panel2_min;

					Splitter.Left = p1;
					Splitter.Top = 0;
					Splitter.Width = splitter_width;
					Splitter.Height = this.Height;
				}
			} else {
				int p1 = splitter_distance;
				if (p1 < 0) {
					p1 = 0;
				} else if (this.Height < p1 + splitter_width) {
					p1 = this.Height - splitter_width;
				}
				int p2 = this.Height - p1 - splitter_width;
				if (check_only) {
					if (p1 < panel1_min || p2 < panel2_min) {
						return false;
					}
				} else {
					if (p1 < panel1_min) {
						p1 = panel1_min;
					}
					p2 = this.Height - p1 - splitter_width;
					if (p2 < panel2_min) {
						p2 = panel2_min;
						//return false;
					}
				}
				if (!check_only) {
					Panel1.Left = pad1;
					Panel1.Top = pad1;
					Panel1.Width = (this.Width - 2 * pad1 >= 0) ? (this.Width - 2 * pad1) : 0;
					Panel1.Height = (p1 - 2 * pad1 >= 0) ? (p1 - 2 * pad1) : 0;

					Panel2.Left = pad2;
					Panel2.Top = p1 + splitter_width + pad2;
					Panel2.Width = (this.Width - 2 * pad2 >= 0) ? (this.Width - 2 * pad2) : 0;
					Panel2.Height = (p2 - 2 * pad2 >= 0) ? (p2 - 2 * pad2) : 0;

					m_splitter_distance = p1;
					m_panel2_distance = this.Height - m_splitter_distance;
					m_distance_rate = m_splitter_distance / (double)(m_splitter_distance + m_panel2_distance);

					m_splitter_width = splitter_width;
					m_panel1_min = panel1_min;
					m_panel2_min = panel2_min;

					Splitter.Left = 0;
					Splitter.Top = p1;
					Splitter.Width = this.Width;
					Splitter.Height = splitter_width;
				}
			}
			return true;
		}

		public int SplitterDistance
		{
			get
			{
				return getDividerLocation();
			}
			set
			{
				setDividerLocation(value);
			}
		}

		public int getDividerLocation()
		{
			return m_splitter_distance;
		}

		public void setDividerLocation(int value)
		{
			UpdateLayout(value, m_splitter_width, m_panel1_min, m_panel2_min, false);
			if (m_orientation == Orientation.Horizontal) {
				m_panel2_distance = this.Width - m_splitter_distance;
			} else {
				m_panel2_distance = this.Height - m_splitter_distance;
			}
		}

		public Cadencii.Gui.Toolkit.Orientation Orientation
		{
			get
			{
				return m_orientation;
			}
			set
			{
				if (m_orientation != value) {
					m_orientation = value;
					UpdateLayout(m_splitter_distance, m_splitter_width, m_panel1_min, m_panel2_min, false);
					if (m_orientation == Orientation.Horizontal) {
						Splitter.Cursor = Cursors.VSplit;
					} else {
						Splitter.Cursor = Cursors.HSplit;
					}
				}
			}
		}

		public BSplitterPanel Panel1 {
			get { return control.Panel1; } 
		}

		public BSplitterPanel Panel2 {
			get { return control.Panel2; }
		}

		public Cadencii.Gui.Toolkit.UiPictureBox Splitter {
			get { return control.Splitter; }
		}

		public void ProcessSizeChanged ()
		{
			if (Width <= 0 || Height <= 0) {
				return;
			}
			if (m_fixed_panel == FixedPanel.Panel2) {
				if (m_orientation == Orientation.Horizontal) {
					m_splitter_distance = this.Width - m_panel2_distance;
				} else {
					m_splitter_distance = this.Height - m_panel2_distance;
				}
			} else if (m_fixed_panel == FixedPanel.None) {
				#if DEBUG
				//Console.WriteLine( "    m_distance_rate=" + m_distance_rate );
				#endif
				if (m_orientation == Orientation.Horizontal) {
					m_splitter_distance = (int)(this.Width * m_distance_rate);
				} else {
					m_splitter_distance = (int)(this.Height * m_distance_rate);
				}
			}
			UpdateLayout(m_splitter_distance, m_splitter_width, m_panel1_min, m_panel2_min, false);
		}

		private void m_lbl_splitter_MouseDown(object sender, MouseEventArgs e)
		{
			if (!m_splitter_fixed) {
				m_splitter_moving = true;
				m_splitter_distance_draft = m_splitter_distance;
				control.Cursor = (m_orientation == Orientation.Horizontal) ? Cursors.VSplit : Cursors.HSplit;
				Splitter.BackColor = SystemColors.ControlDark;
				Splitter.BringToFront();
			}
		}

		private void m_lbl_splitter_MouseUp(object sender, MouseEventArgs e)
		{
			if (m_splitter_moving) {
				m_splitter_moving = false;
				UpdateLayout(m_splitter_distance_draft, m_splitter_width, m_panel1_min, m_panel2_min, false);
				control.Cursor = Cursors.Default;
				Splitter.BackColor = SystemColors.Control;
			}
		}

		public void ProcessSplitterMouseMove ()
		{
			if (m_splitter_fixed) {
				return;
			}
			var mouse_local = ((UiControl) this).PointToClient(Screen.Instance.GetScreenMousePosition ());
			if (m_splitter_moving) {
				int new_distance = m_splitter_distance;
				if (m_orientation == Orientation.Horizontal) {
					new_distance = mouse_local.X;
				} else {
					new_distance = mouse_local.Y;
				}
				if (UpdateLayout(new_distance, m_splitter_width, m_panel1_min, m_panel2_min, true)) {
					m_splitter_distance_draft = new_distance;
					if (m_orientation == Orientation.Horizontal) {
						Splitter.Left = m_splitter_distance_draft;
					} else {
						Splitter.Top = m_splitter_distance_draft;
					}
				}
			}
		}
	}

}
