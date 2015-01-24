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

namespace Cadencii.Gui.Toolkit
{
	public partial class ButtonImpl : System.Windows.Forms.Button, UiButton
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class CheckBoxImpl : System.Windows.Forms.CheckBox, UiCheckBox
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class ComboBoxImpl : System.Windows.Forms.ComboBox, UiComboBox
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class ContainerControlImpl : System.Windows.Forms.ContainerControl, UiContainerControl
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class ContextMenuStripImpl : System.Windows.Forms.ContextMenuStrip, UiContextMenuStrip
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class FlowLayoutPanelImpl : System.Windows.Forms.FlowLayoutPanel, UiFlowLayoutPanel
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class FormImpl : System.Windows.Forms.Form, UiForm
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class GroupBoxImpl : System.Windows.Forms.GroupBox, UiGroupBox
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class HScrollBarImpl : System.Windows.Forms.HScrollBar, UiHScrollBar
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class LabelImpl : System.Windows.Forms.Label, UiLabel
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class LinkLabelImpl : System.Windows.Forms.LinkLabel, UiLinkLabel
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class ListBoxImpl : System.Windows.Forms.ListBox, UiListBox
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class ListViewImpl : System.Windows.Forms.ListView, UiListView
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class MenuStripImpl : System.Windows.Forms.MenuStrip, UiMenuStrip
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class NumericUpDownImpl : System.Windows.Forms.NumericUpDown, UiNumericUpDown
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class PanelImpl : System.Windows.Forms.Panel, UiPanel
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class PictureBoxImpl : System.Windows.Forms.PictureBox, UiPictureBox
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class ProgressBarImpl : System.Windows.Forms.ProgressBar, UiProgressBar
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class PropertyGridImpl : System.Windows.Forms.PropertyGrid, UiPropertyGrid
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class RadioButtonImpl : System.Windows.Forms.RadioButton, UiRadioButton
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class SplitContainerImpl : System.Windows.Forms.SplitContainer, UiSplitContainer
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class StatusStripImpl : System.Windows.Forms.StatusStrip, UiStatusStrip
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class TabControlImpl : System.Windows.Forms.TabControl, UiTabControl
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class TabPageImpl : System.Windows.Forms.TabPage, UiTabPage
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class TextBoxImpl : System.Windows.Forms.TextBox, UiTextBox
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class ToolBarImpl : System.Windows.Forms.ToolBar, UiToolBar
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class ToolStripImpl : System.Windows.Forms.ToolStrip, UiToolStrip
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class ToolStripContainerImpl : System.Windows.Forms.ToolStripContainer, UiToolStripContainer
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class TrackBarImpl : System.Windows.Forms.TrackBar, UiTrackBar
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class UserControlImpl : System.Windows.Forms.UserControl, UiUserControl
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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
	public partial class VScrollBarImpl : System.Windows.Forms.VScrollBar, UiVScrollBar
	{
		// UiControl

		void UiControl.Invoke (Delegate d)
		{
			Invoke (d);
		}

		UiControl UiControl.Parent {
			get { return Parent as UiControl; }
		}

		Cursor UiControl.Cursor {
			get { return this.Cursor.ToGui (); }
			set { Cursor = value.ToNative (); }
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

