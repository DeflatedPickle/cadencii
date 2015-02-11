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
	public partial class FormImpl : System.Windows.Forms.Form, UiForm
 	{
		// IControlContainer

		void IControlContainer.AddControl (UiControl control)
		{
			Controls.Add ((System.Windows.Forms.Control) control);
		}

		void IControlContainer.RemoveControl (UiControl control)
		{
			Controls.Remove ((System.Windows.Forms.Control) control);
		}

		// UiForm

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

		// needs to hide base.
		public new UiMenuStrip MainMenuStrip {
			get { return (UiMenuStrip) base.MainMenuStrip; }
			set {
				if (base.MainMenuStrip != null && base.MainMenuStrip != value.Native)
					Controls.Remove (base.MainMenuStrip);
				base.MainMenuStrip = (System.Windows.Forms.MenuStrip) value.Native;
				if (!Controls.Contains (base.MainMenuStrip))
					Controls.Add (base.MainMenuStrip);
			}
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

		event EventHandler UiForm.BoundsChanged {
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
	}
}

