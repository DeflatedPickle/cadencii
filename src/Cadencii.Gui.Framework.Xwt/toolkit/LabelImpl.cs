using System;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public partial class LabelImpl : LabelBase, UiLabel
	{
		// ignore
		ContentAlignment UiLabel.TextAlign { get; set; }
	}
}

