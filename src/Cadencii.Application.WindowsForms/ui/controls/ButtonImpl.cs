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
	public class ButtonImpl : Button, UiControl
	{
		static MouseEventArgs ToWF (NMouseEventArgs e)
		{
			return new MouseEventArgs ((MouseButtons) e.Button, e.Clicks, e.X, e.Y, e.Delta);
		}

		static NMouseEventArgs ToAwt (MouseEventArgs e)
		{
			return new NMouseEventArgs ((NMouseButtons) e.Button, e.Clicks, e.X, e.Y, e.Delta);
		}
		
		cadencii.java.awt.Color UiControl.BackColor {
			get { return BackColor.ToAwt (); }
			set { BackColor = value.ToNative (); }
		}

		cadencii.java.awt.Point UiControl.Location {
			get { return new cadencii.java.awt.Point (Location.X, Location.Y); }
			set { Location = new System.Drawing.Point (value.X, value.Y); }
		}

		cadencii.java.awt.Size UiControl.Size {
			get { return new cadencii.java.awt.Size (Size.Width, Size.Height); }
			set { this.Size = new System.Drawing.Size (value.Width, value.Height); }
		}

		object UiControl.Native {
			get { return this; }
		}

		cadencii.java.awt.Padding UiControl.Margin {
			get { return new cadencii.java.awt.Padding (Margin.All); }
			set { Margin = new System.Windows.Forms.Padding (value.All); }
		}

		cadencii.java.awt.DockStyle UiControl.Dock {
			get { return (cadencii.java.awt.DockStyle)Dock; }
			set { Dock = (System.Windows.Forms.DockStyle)value; }
		}

		event cadencii.java.awt.KeyEventHandler UiControl.PreviewKeyDown {
			add { this.PreviewKeyDown += (object sender, PreviewKeyDownEventArgs e) => value (sender, new cadencii.java.awt.KeyEventArgs ((cadencii.java.awt.Keys) e.KeyData)); }
			remove { this.PreviewKeyDown -= (object sender, PreviewKeyDownEventArgs e) => value (sender, new cadencii.java.awt.KeyEventArgs ((cadencii.java.awt.Keys) e.KeyData)); }
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
			add { this.MouseClick += (object sender, MouseEventArgs e) => value (sender, ToAwt (e)); }
			remove { this.MouseClick -= (object sender, MouseEventArgs e) => value (sender, ToAwt (e)); }
		}

		event cadencii.java.awt.MouseEventHandler UiControl.MouseDoubleClick {
			add { this.MouseDoubleClick += (object sender, MouseEventArgs e) => value (sender, ToAwt (e)); }
			remove { this.MouseDoubleClick -= (object sender, MouseEventArgs e) => value (sender, ToAwt (e)); }
		}

		event cadencii.java.awt.MouseEventHandler UiControl.MouseDown {
			add { this.MouseDown += (object sender, MouseEventArgs e) => value (sender, ToAwt (e)); }
			remove { this.MouseDown -= (object sender, MouseEventArgs e) => value (sender, ToAwt (e)); }
		}

		event cadencii.java.awt.MouseEventHandler UiControl.MouseUp {
			add { this.MouseUp += (object sender, MouseEventArgs e) => value (sender, ToAwt (e)); }
			remove { this.MouseUp -= (object sender, MouseEventArgs e) => value (sender, ToAwt (e)); }
		}

		event cadencii.java.awt.MouseEventHandler UiControl.MouseMove {
			add { this.MouseMove += (object sender, MouseEventArgs e) => value (sender, ToAwt (e)); }
			remove { this.MouseMove -= (object sender, MouseEventArgs e) => value (sender, ToAwt (e)); }
		}
		event cadencii.java.awt.MouseEventHandler UiControl.MouseWheel {
			add { this.MouseWheel += (object sender, MouseEventArgs e) => value (sender, ToAwt (e)); }
			remove { this.MouseWheel -= (object sender, MouseEventArgs e) => value (sender, ToAwt (e)); }
		}
	}
}

