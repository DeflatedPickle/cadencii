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
			window = new Xwt.Window ();
			window.Content = this;
		}

		Xwt.Window window;
		bool load_registered, load_invoked;
		event EventHandler UiFormLoad;
		MenuStripImpl main_menu;

		// UiForm

		// FIXME: implement
		UiButton UiForm.AcceptButton { get; set; }
		UiButton UiForm.CancelButton { get; set; }

		// ignore.
		bool UiForm.AllowDrop { get; set; }

		// ignore.
		FormStartPosition UiForm.StartPosition { get; set; }

		bool UiForm.ShowInTaskbar {
			get { return base.ParentWindow.ShowInTaskbar; }
			set { base.ParentWindow.ShowInTaskbar = value; }
		}

		// ignore. Just don't set icon if you don't want to show any.
		bool UiForm.ShowIcon { get; set; }

		// FIXME: how to implement?
		bool UiForm.MaximizeBox { get; set; }

		// FIXME: how to implement?
		bool UiForm.MinimizeBox { get; set; }

		// FIXME: how to implement?
		bool UiForm.KeyPreview { get; set; }

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

		public event EventHandler Load {
			add { RegisterLoad (value); }
			remove { UnregisterLoad (value); }
		}

		public event EventHandler Activated {
			add { window.Shown += value; }
			remove { window.Shown -= value; }
		}

		public event EventHandler Deactivate {
			add { window.Hidden += value; }
			remove { window.Hidden -= value; }
		}

		public event EventHandler<DragEventArgs> DragEnter {
			add { base.DragStarted += (o, e) => value (o, e.ToGui ()); }
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

		public event EventHandler<FormClosingEventArgs> FormClosing {
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

		public event EventHandler FormClosed {
			add { window.Closed += value; }
			remove { window.Closed -= value; }
		}

		// this needs to be explicitly overriden to show *window*, not content Widget.
		void UiForm.Show ()
		{
			window.Show ();
		}

		public void HideWindow ()
		{
			window.Hide ();
		}

		public void Close ()
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

		// igore.
		public Cadencii.Gui.Size AutoScaleDimensions { get; set; }

		// FIXME: no effect now
		public DialogResult DialogResult { get; set; }

		public System.ComponentModel.IContainer Components {
			get { return null; }
		}

		// FIXME: no effect. Xwt ckaims it is double buffered by default (which seems sometimes false though).
		public bool DoubleBuffered { get; set; }

		// FIXME: no effect
		public bool UserPaint { get; set; }

		// FIXME: no effect
		public bool AllPaintingInWmPaint { get; set; }

		// FIXME: no effect
		public AutoScaleMode AutoScaleMode { get; set; }

		public UiMenuStrip MainMenuStrip {
			get { return main_menu; }
			set {
				main_menu = (MenuStripImpl) value;
				window.MainMenu = (Xwt.Menu) ((UiMenuStrip) main_menu).Native;
			}
		}

		public FormWindowState WindowState {
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

		public Cadencii.Gui.Size MinimumSize {
			get { return new Size ((int) base.MinWidth, (int) base.MinHeight); }
			set {
				if (value.Width >= 0)
					base.MinWidth = value.Width;
				if (value.Height >= 0)
					base.MinHeight = value.Height;
			}
		}

		// FIXME: maybe better be implemented, but we can live without it.
		public bool TopMost { get; set; }

		public string Text {
			get { return window.Title; }
			set { window.Title = value; }
		}
	}
}

