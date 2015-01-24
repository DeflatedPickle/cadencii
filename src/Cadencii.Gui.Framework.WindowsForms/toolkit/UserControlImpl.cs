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
	public partial class UserControlImpl : System.Windows.Forms.UserControl, UiUserControl
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

		// UiUserControl

		public event EventHandler<PaintEventArgs> Paint {
			add { base.Paint += (object o, System.Windows.Forms.PaintEventArgs e) => value (o, e.ToGui ()); }
			remove { throw new NotImplementedException (); }
		}

		// implementation should either invoke this base method or dispatch to S.W.F.OnPaint().
		public virtual void OnPaint (PaintEventArgs e)
		{
			// could be overriden.
			base.OnPaint (e.ToWF ());
		}

		protected override void OnPaint (System.Windows.Forms.PaintEventArgs e)
		{
			this.OnPaint (e.ToGui ());
		}

		public bool DoubleBuffer {
			get { return this.GetStyle (System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer); }
			set { this.SetStyle (System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, value); }
		}

		public bool UserPaint {
			get { return this.GetStyle (System.Windows.Forms.ControlStyles.UserPaint); }
			set { this.SetStyle (System.Windows.Forms.ControlStyles.UserPaint, value); }
		}

		public bool AllPaintingInWmPaint {
			get { return this.GetStyle (System.Windows.Forms.ControlStyles.AllPaintingInWmPaint); }
			set { this.SetStyle (System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, value); }
		}

		BorderStyle UiUserControl.BorderStyle {
			get { return (BorderStyle)BorderStyle; }
			set { BorderStyle = (System.Windows.Forms.BorderStyle)value; }
		}

		AutoScaleMode UiUserControl.AutoScaleMode {
			get { return (AutoScaleMode)AutoScaleMode; }
			set { AutoScaleMode = (System.Windows.Forms.AutoScaleMode)value; }
		}

		bool UiUserControl.DoubleBuffered {
			get { return DoubleBuffered; }
			set { DoubleBuffered = true; }
		}

		Size UiUserControl.AutoScaleDimensions {
			get { return new Size ((int) AutoScaleDimensions.Width, (int) AutoScaleDimensions.Height); }
			set { AutoScaleDimensions = new System.Drawing.SizeF (value.Width, value.Height); }
		}
	}
}

