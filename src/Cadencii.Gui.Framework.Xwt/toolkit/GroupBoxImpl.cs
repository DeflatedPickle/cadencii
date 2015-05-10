using System;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public partial class GroupBoxImpl
	{
		public GroupBoxImpl ()
		{
			this.Content = new PanelImpl ();
		}

		void IControlContainer.AddControl (UiControl control)
		{
			((IControlContainer) Content).AddControl (control);
		}

		void IControlContainer.RemoveControl (UiControl control)
		{
			((IControlContainer) Content).RemoveControl (control);
		}

		IEnumerable<UiControl> IControlContainer.GetControls ()
		{
			return ((IControlContainer) Content).GetControls ();
		}

		string UiGroupBox.Text {
			get { return this.Label; }
			set { this.Label = value; }
		}
	}
}

