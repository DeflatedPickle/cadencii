

using System;
using System.Linq;
using Cadencii.Gui;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public partial class ButtonImpl : ButtonBase, UiButton
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class CheckBoxImpl : CheckBoxBase, UiCheckBox
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class ComboBoxImpl : ComboBoxBase, UiComboBox
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class ContainerControlImpl : ContainerControlBase, UiContainerControl
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class ContextMenuStripImpl : ContextMenuStripBase, UiContextMenuStrip
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class FlowLayoutPanelImpl : FlowLayoutPanelBase, UiFlowLayoutPanel
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class FormImpl : FormBase, UiForm
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class GroupBoxImpl : GroupBoxBase, UiGroupBox
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class HBoxImpl : HBoxBase, UiHBox
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class HScrollBarImpl : HScrollBarBase, UiHScrollBar
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class HTrackBarImpl : HTrackBarBase, UiHTrackBar
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class LabelImpl : LabelBase, UiLabel
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class LinkLabelImpl : LinkLabelBase, UiLinkLabel
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class ListBoxImpl : ListBoxBase, UiListBox
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class ListViewImpl : ListViewBase, UiListView
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class NumericUpDownImpl : NumericUpDownBase, UiNumericUpDown
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class PanelImpl : PanelBase, UiPanel
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class PictureBoxImpl : PictureBoxBase, UiPictureBox
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class ProgressBarImpl : ProgressBarBase, UiProgressBar
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class PropertyGridImpl : PropertyGridBase, UiPropertyGrid
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class RadioButtonImpl : RadioButtonBase, UiRadioButton
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class SplitContainerImpl : SplitContainerBase, UiSplitContainer
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class StatusStripImpl : StatusStripBase, UiStatusStrip
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class TabControlImpl : TabControlBase, UiTabControl
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class TabPageImpl : TabPageBase, UiTabPage
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class TextBoxImpl : TextBoxBase, UiTextBox
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class ToolBarImpl : ToolBarBase, UiToolBar
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class ToolStripImpl : ToolStripBase, UiToolStrip
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class ToolStripContainerImpl : ToolStripContainerBase, UiToolStripContainer
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class UserControlImpl : UserControlBase, UiUserControl
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class VBoxImpl : VBoxBase, UiVBox
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class VScrollBarImpl : VScrollBarBase, UiVScrollBar
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
	public partial class VTrackBarImpl : VTrackBarBase, UiVTrackBar
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyUp {
			add { this.KeyReleased += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<KeyEventArgs> UiControl.KeyDown {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Key)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 0)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				base.ButtonPressed += (o, e) => {
					if (e.MultiplePress == 2)
						value (o, new MouseEventArgs ((MouseButtons) e.Button, 2, (int) e.X, (int) e.Y, 0));
				};
			}
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseDown {
			add { base.ButtonPressed += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseUp {
			add { base.ButtonReleased += (o, e) => value (o, new MouseEventArgs ((MouseButtons) e.Button, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler<MouseEventArgs> MouseMove {
			add { MouseMoved += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 0)); }
			remove { throw new NotImplementedException (); }
		}

		// FIXME: "delta" is not given, so am passing some dummy value.
		public event EventHandler<MouseEventArgs> MouseWheel {
			add { MouseScrolled += (o, e) => value (o, new MouseEventArgs (MouseButtons.None, 0, (int) e.X, (int) e.Y, 8)); }
			remove { throw new NotImplementedException (); }
		}

		public event EventHandler SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public event EventHandler Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		public void Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		public void BringToFront ()
		{
			// FIXME: implement.
		}

		public void PerformLayout ()
		{
			// no way to support this.
		}

		public Point PointToClient (Point point)
		{
			var sb = Screen.Instance.getScreenBounds (this);
			return new Point (point.X - sb.X, point.Y - sb.Y);
		}

		public Point PointToScreen (Point point)
		{
			return base.ConvertToScreenCoordinates (point.ToWF ()).ToGui ();
		}

		public void SuspendLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout ()
		{
			// no need to support this.
		}

		public void ResumeLayout (bool performLayout)
		{
			// no need to support this.
		}

		public void Focus ()
		{
			this.SetFocus ();
		}

		public void Refresh ()
		{
			// FIXME: implement
		}

		public void Invalidate ()
		{
			// FIXME: implement
		}

		public void Invalidate (bool invalidateChildren)
		{
			// FIXME: implement
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		public new Cadencii.Gui.Cursor Cursor {
			get { return base.Cursor.ToGui (); }
			set { base.Cursor = value.ToNative (); }
		}

		// FIXME: replace any control that depends on this property with VBox/HBox.
		AnchorStyles UiControl.Anchor { get; set; }

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				Top = value.Top;
				Left = value.Left;
				base.WidthRequest = value.Width;
				base.HeightRequest = value.Height;
			}
		}

		public int Top {
			get { return (int) base.MarginTop; }
			set { base.MarginTop = value; }
		}

		public int Left {
			get { return (int) base.MarginLeft; }
			set { base.MarginLeft = value; }
		}

		public int Bottom {
			get { return (int) base.MarginBottom; }
		}

		public int Right {
			get { return (int) base.MarginRight; }
		}

		bool is_disposed;

		protected override void Dispose (bool disposing)
		{
			is_disposed = true;
			base.Dispose (disposing);
		}

		bool UiControl.IsDisposed {
			get { return is_disposed; }
		}

		object UiControl.Native {
			get { return this; }
		}

		Padding UiControl.Margin {
			get { return Margin.ToGui (); }
			set { Margin = value.ToWF (); }
		}

		int UiControl.TabIndex {
			get { return -1; }
			set {}
		}

		DockStyle UiControl.Dock {
			get { return this.ToDock (this.HorizontalPlacement, this.VerticalPlacement); }
			set {
				this.HorizontalPlacement = this.ToHorizontalPlacement (value);
				this.VerticalPlacement = this.ToVerticalPlacement (value);
			}
		}

		public int Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		public int Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		public Color BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		// FIXME: implement
		public Color ForeColor { get; set; }

		// FIXME: implement
		public Point Location { get; set; }

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		public bool Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return base.Font.ToGui (); }
			set { base.Font = value.ToWF (); }
		}
	}
}
