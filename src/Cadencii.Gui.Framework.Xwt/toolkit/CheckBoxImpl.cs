using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class CheckBoxImpl
	{
		event EventHandler UiCheckBox.Click {
			add { this.Clicked += value; }
			remove { this.Clicked -= value; }
		}

		event EventHandler UiCheckBox.CheckedChanged {
			add { this.Toggled += value; }
			remove { this.Toggled -= value; }
		}

		// should be almost ignorable
		public bool UseVisualStyleBackColor { get; set; }

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

