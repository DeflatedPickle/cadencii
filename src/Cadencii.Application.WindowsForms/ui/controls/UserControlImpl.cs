using System;
using System.Windows.Forms;
using Keys = cadencii.java.awt.Keys;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using KeyEventHandler = System.Windows.Forms.KeyEventHandler;
using MouseButtons = System.Windows.Forms.MouseButtons;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;
using NMouseButtons = cadencii.java.awt.MouseButtons;
using NMouseEventArgs = cadencii.java.awt.MouseEventArgs;
using NMouseEventHandler = cadencii.java.awt.MouseEventHandler;

namespace cadencii
{
	public class UserControlImpl : UserControl, UiUserControl
	{
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

		cadencii.java.awt.BorderStyle UiUserControl.BorderStyle {
			get { return (cadencii.java.awt.BorderStyle)BorderStyle; }
			set { BorderStyle = (System.Windows.Forms.BorderStyle)value; }
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

		void UiUserControl.AddControl (UiControl c)
		{
			Controls.Add ((Control) c.Native);
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
			add { this.PreviewKeyDown += (object sender, PreviewKeyDownEventArgs e) => value (sender, new cadencii.java.awt.KeyEventArgs ((cadencii.java.awt.Keys) e.KeyData)); }
			remove { this.PreviewKeyDown -= (object sender, PreviewKeyDownEventArgs e) => value (sender, new cadencii.java.awt.KeyEventArgs ((cadencii.java.awt.Keys) e.KeyData)); }
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

