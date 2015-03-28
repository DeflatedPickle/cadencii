using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class ToolStripStatusLabelImpl : Xwt.Label, UiToolStripStatusLabel
	{
		// FIXME: implement.
		public Image Image { get; set; }

		public event EventHandler MouseEnter {
			add { base.MouseEntered += value; }
			remove { base.MouseEntered -= value; }
		}

		public event EventHandler Click {
			add { base.LinkClicked += (o, e) => new Xwt.LinkEventArgs (new Uri ("urn:dummy")); }
			remove { throw new NotImplementedException (); }
		}

		public void PerformClick ()
		{
			// FIXME: implement.
			throw new NotImplementedException ();
		}

		// ignore.
		Cadencii.Gui.Font UiToolStripItem.Font { get; set; }

		public bool Enabled {
			get { return base.Sensitive; }
			set { base.Sensitive = value; }
		}

		public int Height {
			get { return (int) base.HeightRequest; }
		}

		public string ToolTipText {
			get { return base.TooltipText; }
			set { base.TooltipText = value; }
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

