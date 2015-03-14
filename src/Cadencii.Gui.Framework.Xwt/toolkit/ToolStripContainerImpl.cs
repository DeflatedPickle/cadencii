using System;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public partial class ToolStripContainerImpl : ToolStripContainerBase, UiToolStripContainer
	{
		public ToolStripContainerImpl ()
		{
			content = new PanelImpl ();
			bottom = new PanelImpl ();
			base.PackStart (content);
			base.PackEnd (bottom);
		}

		PanelImpl content, bottom;

		class PanelWrapper : IControlContainer
		{
			PanelImpl panel;

			public PanelWrapper (PanelImpl panel)
			{
				this.panel = panel;
			}

			#region IControlContainer implementation
			void IControlContainer.AddControl (UiControl control)
			{
				panel.AddChild ((Xwt.Widget) control);
			}
			void IControlContainer.RemoveControl (UiControl control)
			{
				panel.RemoveChild ((Xwt.Widget) control);
			}

			IEnumerable<UiControl> IControlContainer.GetControls ()
			{
				foreach (UiControl c in panel.Children)
					yield return c;
			}
			#endregion
		}

		IControlContainer UiToolStripContainer.ContentPanel {
			get { return new PanelWrapper (content); }
		}
		IControlContainer UiToolStripContainer.BottomToolStripPanel {
			get { return new PanelWrapper (bottom); }
		}
	}
}

