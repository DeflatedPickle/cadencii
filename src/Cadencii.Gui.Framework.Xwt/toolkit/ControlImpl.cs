

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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
		}
	}
	public partial class MenuStripImpl : MenuStripBase, UiMenuStrip
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
		}
	}
	public partial class TrackBarImpl : TrackBarBase, UiTrackBar
	{
		event EventHandler<KeyEventArgs> UiControl.PreviewKeyDown {
			add { this.PreviewTextInput += (sender, e) => value (sender, new KeyEventArgs ((Keys) e.Text.FirstOrDefault ())); }
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
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

		// FIXME: apparently Xwt has different concept of KeyPress and KeyDown than Windows key events.
		event EventHandler<KeyPressEventArgs> UiControl.KeyPress {
			add { this.KeyPressed += (sender, e) => value (sender, new KeyPressEventArgs ((char) e.Key)); }
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

		event EventHandler<MouseEventArgs> UiControl.MouseClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDoubleClick {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseDown {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseUp {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler<MouseEventArgs> UiControl.MouseMove {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: MouseScrolled maybe?
		event EventHandler<MouseEventArgs> UiControl.MouseWheel {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		event EventHandler UiControl.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiControl.Enter {
			add { this.MouseEntered += value; }
			remove { this.MouseEntered -= value; }
		}

		event EventHandler UiControl.Leave {
			add { this.MouseExited += value; }
			remove { this.MouseExited -= value; }
		}

		event EventHandler UiControl.SizeChanged {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.Resize {
			add { this.BoundsChanged += value; }
			remove { this.BoundsChanged -= value; }
		}

		event EventHandler UiControl.ImeModeChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		void UiControl.Invoke (Delegate e)
		{
			Xwt.Application.Invoke ((Action) e);
		}

		void UiControl.BringToFront ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.PerformLayout ()
		{
			// no way to support this.
		}

		Point UiControl.PointToClient (Point point)
		{
			throw new NotImplementedException ();
		}

		Point UiControl.PointToScreen (Point point)
		{
			throw new NotImplementedException ();
		}

		void UiControl.SuspendLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout ()
		{
			// no way to support this.
		}

		void UiControl.ResumeLayout (bool performLayout)
		{
			// no way to support this.
		}

		void UiControl.Focus ()
		{
			this.SetFocus ();
		}

		void UiControl.Refresh ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate ()
		{
			throw new NotImplementedException ();
		}

		void UiControl.Invalidate (bool invalidateChildren)
		{
			throw new NotImplementedException ();
		}

		bool UiControl.Focused {
			get { return this.HasFocus; }
		}

		UiControl UiControl.Parent {
			get { return (UiControl) this.Parent; }
		}

		Cadencii.Gui.Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { this.Cursor = value.ToNative (); }
		}

		System.Collections.Generic.IList<UiControl> UiControl.Controls {
			get { return this.Surface.Children.Cast<UiControl> (); }
		}

		AnchorStyles UiControl.Anchor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		bool UiControl.TabStop {
			// not really sure if there is any more appropriate property...
			get { return this.CanGetFocus; }
			set { this.CanGetFocus = value; }
		}

		Rectangle UiControl.Bounds {
			get { return this.ScreenBounds.ToGui (); }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Top {
			get { return (int) this.ScreenBounds.Top; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Left {
			get { return (int) this.ScreenBounds.Left; }
			set {
				throw new NotImplementedException ();
			}
		}

		int UiControl.Bottom {
			get { return (int) this.ScreenBounds.Bottom; }
		}

		int UiControl.Right {
			get { return (int) this.ScreenBounds.Right; }
		}

		ImeMode UiControl.ImeMode {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
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

		int UiControl.Width {
			get { return (int) this.WidthRequest; }
			set { this.WidthRequest = value; }
		}

		int UiControl.Height {
			get { return (int) this.HeightRequest; }
			set { this.HeightRequest = value; }
		}

		Color UiControl.BackColor {
			get { return this.BackgroundColor.ToGui (); }
			set { this.BackgroundColor = value.ToNative (); }
		}

		Color UiControl.ForeColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Point UiControl.Location {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		Cadencii.Gui.Size UiControl.Size {
			get { return this.Size.ToGui (); }
			set {
				this.WidthRequest = value.Width;
				this.HeightRequest = value.Height;
			}
		}

		bool UiControl.Enabled {
			get { return this.Sensitive; }
			set { this.Sensitive = value; }
		}

		Cadencii.Gui.Font UiControl.Font {
			get { return this.Font.ToGui (); }
			set { this.Font = value.ToWF (); }
		}
	}
}
