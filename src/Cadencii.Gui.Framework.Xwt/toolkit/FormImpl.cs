using System;
using Cadencii.Gui;
using System.Linq;
using System.Threading;

namespace Cadencii.Gui.Toolkit
{
	public partial class FormImpl
	{
		public FormImpl ()
		{
			this.ExpandHorizontal = true;
			this.ExpandVertical = true;
			window = new Xwt.Dialog ();
			window.Content = this;
		}

		Xwt.Dialog window;
		bool load_registered, load_invoked;
		event EventHandler UiFormLoad;

		// UiForm

		// FIXME: how to implement?
		bool UiForm.InvokeRequired {
			get { return true; }
		}

		void RegisterLoad (EventHandler loaded)
		{
			if (!load_registered)
				window.Shown += OnLoad;
			load_registered = true;
			UiFormLoad += loaded;
		}
			
		void UnregisterLoad (EventHandler loaded)
		{
			UiFormLoad -= loaded;
		}

		void OnLoad (object o, EventArgs e)
		{
			if (load_invoked)
				return;
			load_invoked = true;
			UiFormLoad (o, e);
		}

		event EventHandler UiForm.Load {
			add { RegisterLoad (value); }
			remove { UnregisterLoad (value); }
		}

		event EventHandler UiForm.Activated {
			add { window.Shown += value; }
			remove { window.Shown -= value; }
		}

		event EventHandler UiForm.Deactivate {
			add { window.Hidden += value; }
			remove { window.Hidden -= value; }
		}

		event EventHandler<DragEventArgs> UiForm.DragEnter {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<DragEventArgs> UiForm.DragDrop {
			add { this.DragDrop += (o, e) => value (o, e.ToGui ()); }
			remove { throw new NotImplementedException (); }
		}

		event EventHandler<DragEventArgs> UiForm.DragOver {
			add { this.DragOver += (o, e) => value (o, e.ToGui ()); }
			remove { throw new NotImplementedException (); }
		}

		event EventHandler UiForm.DragLeave {
			add { this.DragLeave += value; }
			remove { this.DragLeave -= value; }
		}

		event EventHandler<FormClosingEventArgs> UiForm.FormClosing {
			add {
				window.CloseRequested += (o, e) => {
					var xe = new FormClosingEventArgs ();
					value (o, xe);
					e.AllowClose = !xe.Cancel;
				};
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiForm.FormClosed {
			add { window.Closed += value; }
			remove { window.Closed -= value; }
		}

		void UiForm.Close ()
		{
			window.Close ();
		}

		public UiForm AsGui ()
		{
			return this;
		}

		public object Invoke (Delegate d, params object[] args)
		{
			object ret = null;
			SynchronizationContext.Current.Send (o => ret = d.DynamicInvoke (args), null);
			return ret;
		}

		public DialogResult ShowDialog ()
		{
			throw new NotImplementedException ();
		}

		public DialogResult ShowDialog (object parentForm)
		{
			throw new NotImplementedException ();
		}

		// FIXME: no effect now
		public FormBorderStyle FormBorderStyle { get; set; }

		// FIXME: is it correct?
		public Cadencii.Gui.Size ClientSize {
			get { return window.Size.ToGui (); }
			set { window.Size = value.ToWF (); }
		}

		// FIXME: no effect now
		public DialogResult DialogResult { get; set; }

		public System.ComponentModel.IContainer Components {
			get { return null; }
		}

		// FIXME: no effect. Xwt ckaims it is double buffered by default (which seems sometimes false though).
		bool UiForm.DoubleBuffered { get; set; }

		// FIXME: no effect
		bool UiForm.UserPaint { get; set; }

		// FIXME: no effect
		bool UiForm.AllPaintingInWmPaint { get; set; }

		// FIXME: no effect
		AutoScaleMode UiForm.AutoScaleMode { get; set; }

		UiMenuStrip UiForm.MainMenuStrip {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		FormWindowState UiForm.WindowState {
			get { return window.FullScreen ? FormWindowState.Maximized : window.ScreenBounds.IsEmpty ? FormWindowState.Minimized : FormWindowState.Normal; }
			set {
				switch (value) {
				case FormWindowState.Maximized:
					window.FullScreen = true;
					window.Show ();
					break;
				case FormWindowState.Minimized:
					window.FullScreen = false;
					window.Hide ();
					break;
				default:
					window.FullScreen = false;
					window.Show ();
					break;
				}
			}
		}

		Cadencii.Gui.Size UiForm.MinimumSize {
			get { return new Size ((int) base.MinWidth, (int) base.MinHeight); }
			set {
				base.MinWidth = value.Width;
				base.MinHeight = value.Height;
			}
		}

		// FIXME: maybe better be implemented, but we can live without it.
		bool UiForm.TopMost { get; set; }

		string UiForm.Text {
			get { return window.Title; }
			set { window.Title = value; }
		}
	}
}

