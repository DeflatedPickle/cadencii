using System;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public partial class ToolStripContainerImpl : ToolStripContainerBase, UiToolStripContainer
	{
		class PanelWrapper : IControlContainer
		{
			PanelImpl panel;

			public PanelWrapper (PanelImpl panel)
			{
				this.panel = panel;
			}

			// ignore.
			public ToolStripRenderMode RenderMode { get; set; }

			public Size Size {
				get { return panel.Size.ToGui (); }
				set {
					panel.WidthRequest = value.Width;
					panel.HeightRequest = value.Height;
				}
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

		public ToolStripContainerImpl ()
		{
			content = new PanelImpl () { Name = "Content" };
			bottom = new PanelImpl () { Name = "Bottom" };
			base.PackStart (content);
			base.PackEnd (bottom);
		}

		PanelImpl content, bottom;

		IControlContainer UiToolStripContainer.ContentPanel {
			get { return new PanelWrapper (content); }
		}
		IControlContainer UiToolStripContainer.BottomToolStripPanel {
			get { return new PanelWrapper (bottom); }
		}
	}
}

