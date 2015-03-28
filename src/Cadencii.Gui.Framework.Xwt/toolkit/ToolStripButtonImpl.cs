using System;

namespace Cadencii.Gui.Toolkit
{
	public class ToolStripButtonImpl : Xwt.ToggleButton, UiToolStripButton
	{
		event EventHandler UiToolStripButton.CheckedChanged {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}

		// FIXME: implement
		Color UiToolStripButton.ImageTransparentColor { get; set; }

		public bool CheckOnClick { get; set; }

		bool UiToolStripButton.Checked {
			get { return base.Active; }
			set { base.Active = value; }
		}
		Cadencii.Gui.Image UiToolStripButton.Image {
			get { return base.Image.ToGui (); }
			set { base.Image = value.ToWF (); }
		}
		event EventHandler UiToolStripItem.MouseEnter {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}
		event EventHandler UiToolStripItem.Click {
			add {
				throw new NotImplementedException ();
			}
			remove {
				throw new NotImplementedException ();
			}
		}
		void UiToolStripItem.PerformClick ()
		{
			throw new NotImplementedException ();
		}
		Cadencii.Gui.Font UiToolStripItem.Font {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		bool UiToolStripItem.Enabled {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		int UiToolStripItem.Height {
			get {
				throw new NotImplementedException ();
			}
		}
		string UiToolStripItem.Text {
			get { return base.Label; }
			set { base.Label = value; }
		}
		string UiToolStripItem.ToolTipText {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		Cadencii.Gui.Size UiToolStripItem.Size {
			get { return base.Size.ToGui (); }
			set {
				base.WidthRequest = value.Width; 
				base.HeightRequest = value.Height;
			}
		}
	}
}

