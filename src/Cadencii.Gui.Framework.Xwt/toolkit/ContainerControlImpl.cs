using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class ContainerControlImpl
	{
		void IDisposable.Dispose ()
		{
			Dispose (false);
		}

		string UiControl.Name { get; set; }

		bool UiControl.Visible {
			get { return this.Visible; }
			set { this.Visible = true; }
		}

	}
}

