/*
 * FormMixer.cs
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
using System.Linq;
using cadencii.apputil;
using cadencii.java.awt;
using cadencii.java.util;
using cadencii.vsq;
using cadencii.java.awt;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace cadencii
{
	public class FormImpl : System.Windows.Forms.Form, UiForm
 	{
		event EventHandler UiForm.LocationChanged {
			add { this.LocationChanged += (object sender, EventArgs e) => value (sender, e); }
			remove { this.LocationChanged -= (object sender, EventArgs e) => value (sender, e); }
		}

		cadencii.java.awt.Point UiForm.Location {
			get { return Location.ToAwt (); }
			set { Location = value.ToWF (); }
		}

		event EventHandler UiForm.FormClosing {
			add { this.FormClosing += (object sender, System.Windows.Forms.FormClosingEventArgs e) => value (sender, e); }
			remove { this.FormClosing -= (object sender, System.Windows.Forms.FormClosingEventArgs e) => value (sender, e); }
		}

		int UiForm.showDialog (object parent_form)
		{
			return ShowDialog ((System.Windows.Forms.IWin32Window) parent_form) == System.Windows.Forms.DialogResult.OK ? 1 : 0;
		}

		// UiControl
		object UiControl.Native {
			get { return this; }
		}

		bool UiControl.IsDisposed {
			get { return IsDisposed; }
		}

		cadencii.java.awt.AnchorStyles UiControl.Anchor {
			get { return (cadencii.java.awt.AnchorStyles)Anchor; }
			set { Anchor = (System.Windows.Forms.AnchorStyles) value; }
		}

		cadencii.java.awt.Rectangle UiControl.Bounds {
			get { return Bounds.ToAwt (); }
			set { this.Bounds = value.ToWF (); }
		}

		cadencii.java.awt.ImeMode UiControl.ImeMode {
			get { return (cadencii.java.awt.ImeMode)ImeMode; }
			set { ImeMode = (System.Windows.Forms.ImeMode) value; }
		}
		
		cadencii.java.awt.Color UiControl.BackColor {
			get { return BackColor.ToAwt (); }
			set { BackColor = value.ToNative (); }
		}

		cadencii.java.awt.Font UiControl.Font {
			get { return Font.ToAwt (); }
			set { Font = value.ToWF (); }
		}

		cadencii.java.awt.Color UiControl.ForeColor {
			get { return ForeColor.ToAwt (); }
			set { ForeColor = value.ToNative (); }
		}

		cadencii.java.awt.Point UiControl.Location {
			get { return Location.ToAwt (); }
			set { Location = value.ToWF (); }
		}

		cadencii.java.awt.Dimension UiControl.Size {
			get { return new cadencii.java.awt.Dimension (Size.Width, Size.Height); }
			set { this.Size = new System.Drawing.Size (value.width, value.height); }
		}

		cadencii.java.awt.Padding UiControl.Margin {
			get { return new cadencii.java.awt.Padding (Margin.All); }
			set { Margin = new System.Windows.Forms.Padding (value.All); }
		}

		cadencii.java.awt.DockStyle UiControl.Dock {
			get { return (cadencii.java.awt.DockStyle)Dock; }
			set { Dock = (System.Windows.Forms.DockStyle)value; }
		}

		void UiControl.Focus ()
		{
			Focus ();
		}

		cadencii.java.awt.Point UiControl.PointToClient (cadencii.java.awt.Point point)
		{
			return PointToClient (point.ToWF ()).ToAwt ();
		}

		cadencii.java.awt.Point UiControl.PointToScreen (cadencii.java.awt.Point point)
		{
			return PointToScreen (point.ToWF ()).ToAwt ();
		}

		event EventHandler UiControl.Resize {
			add { this.Resize += value; }
			remove { this.Resize -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add { this.ImeModeChanged += value; }
			remove { this.ImeModeChanged -= value; }
		}

		event cadencii.java.awt.KeyEventHandler UiControl.PreviewKeyDown {
			add { this.PreviewKeyDown += (object sender, System.Windows.Forms.PreviewKeyDownEventArgs e) => value (sender, new cadencii.java.awt.KeyEventArgs ((cadencii.java.awt.Keys) e.KeyData)); }
			remove { this.PreviewKeyDown -= (object sender, System.Windows.Forms.PreviewKeyDownEventArgs e) => value (sender, new cadencii.java.awt.KeyEventArgs ((cadencii.java.awt.Keys) e.KeyData)); }
		}

		event EventHandler<cadencii.java.awt.KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPress += (object sender, System.Windows.Forms.KeyPressEventArgs e) => value (sender, new cadencii.java.awt.KeyPressEventArgs (e.KeyChar) { Handled = e.Handled}); }
			remove { this.KeyPress -= (object sender, System.Windows.Forms.KeyPressEventArgs e) => value (sender, new cadencii.java.awt.KeyPressEventArgs (e.KeyChar) { Handled = e.Handled}); }
		}

		event cadencii.java.awt.KeyEventHandler UiControl.KeyUp {
			add { this.KeyUp += (object sender, KeyEventArgs e) => value (sender, new cadencii.java.awt.KeyEventArgs ((cadencii.java.awt.Keys) e.KeyData)); }
			remove { this.KeyUp -= (object sender, KeyEventArgs e) => value (sender, new cadencii.java.awt.KeyEventArgs ((cadencii.java.awt.Keys) e.KeyData)); }
		}

		event cadencii.java.awt.KeyEventHandler UiControl.KeyDown {
			add { this.KeyDown += (object sender, KeyEventArgs e) => value (sender, new cadencii.java.awt.KeyEventArgs ((cadencii.java.awt.Keys) e.KeyData)); }
			remove { this.KeyDown -= (object sender, KeyEventArgs e) => value (sender, new cadencii.java.awt.KeyEventArgs ((cadencii.java.awt.Keys) e.KeyData)); }
		}

		event cadencii.java.awt.MouseEventHandler UiControl.MouseClick {
			add { this.MouseClick += (object sender, MouseEventArgs e) => value (sender, e.ToAwt ()); }
			remove { this.MouseClick -= (object sender, MouseEventArgs e) => value (sender, e.ToAwt ()); }
		}

		event cadencii.java.awt.MouseEventHandler UiControl.MouseDoubleClick {
			add { this.MouseDoubleClick += (object sender, MouseEventArgs e) => value (sender, e.ToAwt ()); }
			remove { this.MouseDoubleClick -= (object sender, MouseEventArgs e) => value (sender, e.ToAwt ()); }
		}

		event cadencii.java.awt.MouseEventHandler UiControl.MouseDown {
			add { this.MouseDown += (object sender, MouseEventArgs e) => value (sender, e.ToAwt ()); }
			remove { this.MouseDown -= (object sender, MouseEventArgs e) => value (sender, e.ToAwt ()); }
		}

		event cadencii.java.awt.MouseEventHandler UiControl.MouseUp {
			add { this.MouseUp += (object sender, MouseEventArgs e) => value (sender, e.ToAwt ()); }
			remove { this.MouseUp -= (object sender, MouseEventArgs e) => value (sender, e.ToAwt ()); }
		}

		event cadencii.java.awt.MouseEventHandler UiControl.MouseMove {
			add { this.MouseMove += (object sender, MouseEventArgs e) => value (sender, e.ToAwt ()); }
			remove { this.MouseMove -= (object sender, MouseEventArgs e) => value (sender, e.ToAwt ()); }
		}
		event cadencii.java.awt.MouseEventHandler UiControl.MouseWheel {
			add { this.MouseWheel += (object sender, MouseEventArgs e) => value (sender, e.ToAwt ()); }
			remove { this.MouseWheel -= (object sender, MouseEventArgs e) => value (sender, e.ToAwt ()); }
		}
	}
}

