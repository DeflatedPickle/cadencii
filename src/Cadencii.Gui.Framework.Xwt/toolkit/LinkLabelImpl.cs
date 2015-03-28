using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class LinkLabelImpl
	{
		event EventHandler UiLinkLabel.LinkClicked {
			add { base.LinkClicked += (o, e) => value (o, e); }
			remove { throw new NotImplementedException (); }
		}

		// ignore
		ContentAlignment UiLabel.TextAlign { get; set; }
	}
}

