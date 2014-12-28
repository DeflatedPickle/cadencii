using System;
using System.Linq;
using Cadencii.Gui;
using Keys = Cadencii.Gui.Toolkit.Keys;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using KeyEventHandler = System.Windows.Forms.KeyEventHandler;
using MouseButtons = System.Windows.Forms.MouseButtons;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;
using NKeyEventArgs = Cadencii.Gui.Toolkit.KeyEventArgs;
using NKeyPressEventArgs = Cadencii.Gui.Toolkit.KeyPressEventArgs;
using NKeyEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.KeyEventArgs>;
using NMouseButtons = Cadencii.Gui.Toolkit.MouseButtons;
using NMouseEventArgs = Cadencii.Gui.Toolkit.MouseEventArgs;
using NMouseEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.MouseEventArgs>;
using System.Collections.Generic;
using Cadencii.Gui.Toolkit;
using cadencii;
using System.ComponentModel;

namespace Cadencii.Gui.Toolkit
{
	public class FormImpl : System.Windows.Forms.Form, UiForm
 	{
		FormBorderStyle UiForm.FormBorderStyle {
			get { return (FormBorderStyle) FormBorderStyle; }
			set { FormBorderStyle = (System.Windows.Forms.FormBorderStyle) value; }
		}

		Size UiForm.ClientSize {
			get { return ClientSize.ToGui (); }
			set { ClientSize = value.ToWF (); }
		}

		DialogResult UiForm.DialogResult {
			get { return (DialogResult) DialogResult; }
			set { DialogResult = (System.Windows.Forms.DialogResult) value; }
		}

		public virtual IContainer Components {
			get { return null; }
		}

		// it hides protected Control.DoubleBuffered.
		public new bool DoubleBuffered { 
			get { return base.DoubleBuffered; }
			set { base.DoubleBuffered = value; }
		}

		public bool UserPaint {
			get { return this.GetStyle (System.Windows.Forms.ControlStyles.UserPaint); }
			set { this.SetStyle (System.Windows.Forms.ControlStyles.UserPaint, value); }
		}

		public bool AllPaintingInWmPaint {
			get { return this.GetStyle (System.Windows.Forms.ControlStyles.AllPaintingInWmPaint); }
			set { this.SetStyle (System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, value); }
		}

		AutoScaleMode UiForm.AutoScaleMode {
			get { return (AutoScaleMode)AutoScaleMode;  }
			set { AutoScaleMode = (System.Windows.Forms.AutoScaleMode)value; }
		}

		UiMenuStrip UiForm.MainMenuStrip {
			get { return (UiMenuStrip)MainMenuStrip; }
			set { MainMenuStrip = (System.Windows.Forms.MenuStrip) value.Native; }
		}

		event EventHandler<DragEventArgs> UiForm.DragEnter {
			add { DragEnter += (sender, e) => value (sender, e.ToGui ()); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<DragEventArgs> UiForm.DragDrop {
			add { DragDrop += (sender, e) => value (sender, e.ToGui ()); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<DragEventArgs> UiForm.DragOver {
			add { DragOver += (sender, e) => value (sender, e.ToGui ()); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiForm.DragLeave {
			add { DragLeave += value; }
			remove { DragLeave -= value; }
		}

		FormWindowState UiForm.WindowState {
			get { return (FormWindowState)WindowState; }
			set { WindowState = (System.Windows.Forms.FormWindowState)value; }
		}

		public UiForm AsGui ()
		{
			return this;
		}

		Size UiForm.MinimumSize {
			get { return MinimumSize.ToGui (); }
			set { MinimumSize = value.ToWF (); }
		}

		Cadencii.Gui.DialogResult UiForm.ShowDialog ()
		{
			return (Cadencii.Gui.DialogResult) ShowDialog ();
		}

		Cadencii.Gui.DialogResult UiForm.ShowDialog (object owner)
		{
			return (Cadencii.Gui.DialogResult) ShowDialog (owner as System.Windows.Forms.IWin32Window);
		}

		event EventHandler UiForm.LocationChanged {
			add { this.LocationChanged += (sender, e) => value (sender, e); }
			remove { this.LocationChanged -= (sender, e) => value (sender, e); }
		}

		event EventHandler<FormClosingEventArgs> UiForm.FormClosing {
			add { this.FormClosing += (sender, e) => value (sender, e.ToGui ()); }
			remove { throw new NotImplementedException (); }
		}

		event EventHandler UiForm.FormClosed {
			add { this.FormClosed += (sender, e) => value (sender, e); }
			remove { this.FormClosed -= (sender, e) => value (sender, e); }
		}

		// UiControl

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
		}

		IList<UiControl> UiControl.Controls {
			get { return new CastingList<UiControl, System.Windows.Forms.Control> (Controls, null, null); }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.SizeChanged += value; }
			remove { this.SizeChanged -= value; }
		}

		object UiControl.Native {
			get { return this; }
		}

		bool UiControl.IsDisposed {
			get { return IsDisposed; }
		}

		AnchorStyles UiControl.Anchor {
			get { return (AnchorStyles)Anchor; }
			set { Anchor = (System.Windows.Forms.AnchorStyles) value; }
		}

		Rectangle UiControl.Bounds {
			get { return Bounds.ToGui (); }
			set { this.Bounds = value.ToWF (); }
		}

		ImeMode UiControl.ImeMode {
			get { return (ImeMode)ImeMode; }
			set { ImeMode = (System.Windows.Forms.ImeMode) value; }
		}

		Font UiControl.Font {
			get { return Font.ToGui (); }
			set { Font = value.ToWF (); }
		}

		Color UiControl.ForeColor {
			get { return ForeColor.ToGui (); }
			set { ForeColor = value.ToNative (); }
		}

		Color UiControl.BackColor {
			get { return BackColor.ToGui (); }
			set { BackColor = value.ToNative (); }
		}

		Point UiControl.Location {
			get { return Location.ToGui (); }
			set { Location = value.ToWF (); }
		}

		Size UiControl.Size {
			get { return new Size (Size.Width, Size.Height); }
			set { this.Size = new System.Drawing.Size (value.Width, value.Height); }
		}

		Padding UiControl.Margin {
			get { return new Padding (Margin.All); }
			set { Margin = new System.Windows.Forms.Padding (value.All); }
		}

		DockStyle UiControl.Dock {
			get { return (DockStyle)Dock; }
			set { Dock = (System.Windows.Forms.DockStyle)value; }
		}

		void UiControl.Focus ()
		{
			Focus ();
		}

		void UiControl.Invalidate ()
		{
			Invalidate ();
		}

		Point UiControl.PointToClient (Point point)
		{
			return PointToClient (point.ToWF ()).ToGui ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			return PointToScreen (point.ToWF ()).ToGui ();
		}

		event EventHandler UiControl.Enter {
			add { Enter += value; }
			remove { Enter -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.Resize += value; }
			remove { this.Resize -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add { this.ImeModeChanged += value; }
			remove { this.ImeModeChanged -= value; }
		}

		event NKeyEventHandler UiControl.PreviewKeyDown {
			add { this.PreviewKeyDown += (sender, e) => value (sender, new NKeyEventArgs ((Keys) e.KeyData)); }
			remove { this.PreviewKeyDown -= (sender, e) => value (sender, new NKeyEventArgs ((Keys) e.KeyData)); }
		}

		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPress += (sender,  e) => value (sender, new NKeyPressEventArgs (e.KeyChar) { Handled = e.Handled}); }
			remove { this.KeyPress -= (sender,  e) => value (sender, new NKeyPressEventArgs (e.KeyChar) { Handled = e.Handled}); }
		}

		event NKeyEventHandler UiControl.KeyUp {
			add { this.KeyUp += (sender, e) => value (sender, new NKeyEventArgs ((Keys) e.KeyData)); }
			remove { this.KeyUp -= (sender, e) => value (sender, new NKeyEventArgs ((Keys) e.KeyData)); }
		}

		event NKeyEventHandler UiControl.KeyDown {
			add { this.KeyDown += (sender, e) => value (sender, new NKeyEventArgs ((Keys) e.KeyData)); }
			remove { this.KeyDown -= (sender, e) => value (sender, new NKeyEventArgs ((Keys) e.KeyData)); }
		}

		event NMouseEventHandler UiControl.MouseClick {
			add { this.MouseClick += (sender, e) => value (sender, e.ToGui ()); }
			remove { this.MouseClick -= (sender, e) => value (sender, e.ToGui ()); }
		}

		event NMouseEventHandler UiControl.MouseDoubleClick {
			add { this.MouseDoubleClick += (sender, e) => value (sender, e.ToGui ()); }
			remove { this.MouseDoubleClick -= (sender, e) => value (sender, e.ToGui ()); }
		}

		event NMouseEventHandler UiControl.MouseDown {
			add { this.MouseDown += (sender, e) => value (sender, e.ToGui ()); }
			remove { this.MouseDown -= (sender, e) => value (sender, e.ToGui ()); }
		}

		event NMouseEventHandler UiControl.MouseUp {
			add { this.MouseUp += (sender, e) => value (sender, e.ToGui ()); }
			remove { this.MouseUp -= (sender, e) => value (sender, e.ToGui ()); }
		}

		event NMouseEventHandler UiControl.MouseMove {
			add { this.MouseMove += (sender, e) => value (sender, e.ToGui ()); }
			remove { this.MouseMove -= (sender, e) => value (sender, e.ToGui ()); }
		}
		event NMouseEventHandler UiControl.MouseWheel {
			add { this.MouseWheel += (sender, e) => value (sender, e.ToGui ()); }
			remove { this.MouseWheel -= (sender, e) => value (sender, e.ToGui ()); }
		}
	}
}

