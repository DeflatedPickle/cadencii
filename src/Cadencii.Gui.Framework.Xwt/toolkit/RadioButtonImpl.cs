using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class RadioButtonImpl : RadioButtonBase, UiRadioButton
	{
		event EventHandler UiRadioButton.CheckedChanged {
			add { base.ActiveChanged += value; }
			remove { base.ActiveChanged -= value; }
		}

		bool UiRadioButton.Checked {
			get { return base.Active; }
			set { base.Active = value; }
		}

		string IHasText.Text {
			get { return base.Label; }
			set { base.Label = value; }
		}
	}
}

