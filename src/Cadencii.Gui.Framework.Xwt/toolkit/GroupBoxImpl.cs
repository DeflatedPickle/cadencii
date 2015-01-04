using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class GroupBoxImpl
	{
		string UiGroupBox.Text {
			get { return this.Label; }
			set { this.Label = value; }
		}
	}
}

