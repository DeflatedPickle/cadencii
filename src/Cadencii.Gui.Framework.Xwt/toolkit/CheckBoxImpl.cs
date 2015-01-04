using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class CheckBoxImpl
	{
		event EventHandler UiCheckBox.CheckedChanged {
			add { this.Toggled += value; }
			remove { this.Toggled -= value; }
		}

		bool UiCheckBox.Checked {
			get { return this.State == Xwt.CheckBoxState.On; }
			set { this.State = value ? Xwt.CheckBoxState.On : Xwt.CheckBoxState.Off; } 
		}

		string IHasText.Text {
			get { return this.Label; }
			set { this.Label = value; }
		}
	}
}

