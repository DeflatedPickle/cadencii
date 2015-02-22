using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cadencii.Gui.Toolkit
{
	public partial class StatusStripImpl : StatusStripBase, UiStatusStrip
	{
		UiToolStripItem UiStatusStrip.Content {
			get { return (UiToolStripItem) base.Children.FirstOrDefault (); }
			set {
				base.Clear ();
				base.PackStart ((Xwt.Widget) value);
			}
		}
	}
}

