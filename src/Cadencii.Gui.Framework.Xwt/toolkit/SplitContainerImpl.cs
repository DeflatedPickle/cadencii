using System;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	class SplitContainerPanelWrapper : IControlContainer
	{
		Xwt.Panel panel;

		public SplitContainerPanelWrapper (Xwt.Panel panel)
		{
			this.panel = panel;
		}

		// ignore.
		public ToolStripRenderMode RenderMode { get; set; }

		// FIXME: implement.
		public Size Size { get; set; }

		// FIXME: implement.
		public Point Location { get; set; }

		// implement?
		public AnchorStyles Anchor { get; set; }

		// ignore
		public BorderStyle BorderStyle { get; set; }

		public string Name { get; set; }

		public int TabIndex { get; set; }

		#region IControlContainer implementation
		void IControlContainer.AddControl (UiControl control)
		{
			((IControlContainer) panel.Content).AddControl (control);
		}
		void IControlContainer.RemoveControl (UiControl control)
		{
			((IControlContainer) panel.Content).RemoveControl (control);
		}

		IEnumerable<UiControl> IControlContainer.GetControls ()
		{
			foreach (var c in ((IControlContainer) panel.Content).GetControls ())
				yield return c;
		}
		#endregion
	}

	public partial class HSplitContainerImpl
	{
		IControlContainer panel1, panel2;

		public HSplitContainerImpl ()
		{
			Panel1.Content = new PanelImpl ();
			Panel2.Content = new PanelImpl ();
		}

		// ignore.
		public string FixedPanel { get; set; }

		IControlContainer UiSplitContainer.Panel1 {
			get { return panel1 ?? (panel1 = new SplitContainerPanelWrapper (Panel1)); }
		}

		IControlContainer UiSplitContainer.Panel2 {
			get { return panel2 ?? (panel2 = new SplitContainerPanelWrapper (Panel2)); }
		}

		// FIXME: implement
		public Size MinimumSize { get; set; }

		// FIXME: implement
		public int Panel1MinSize { get; set; }

		// FIXME: implement
		public int Panel2MinSize { get; set; }

		// ignore
		public int SplitterWidth { get; set; }
	}

	public partial class VSplitContainerImpl
	{
		IControlContainer panel1, panel2;

		public VSplitContainerImpl ()
		{
			Panel1.Content = new PanelImpl ();
			Panel2.Content = new PanelImpl ();
		}

		// ignore.
		public string FixedPanel { get; set; }

		IControlContainer UiSplitContainer.Panel1 {
			get { return panel1 ?? (panel1 = new SplitContainerPanelWrapper (Panel1)); }
		}

		IControlContainer UiSplitContainer.Panel2 {
			get { return panel2 ?? (panel2 = new SplitContainerPanelWrapper (Panel2)); }
		}

		// FIXME: implement
		public Size MinimumSize { get; set; }

		// FIXME: implement
		public int Panel1MinSize { get; set; }

		// FIXME: implement
		public int Panel2MinSize { get; set; }

		// ignore
		public int SplitterWidth { get; set; }
	}
}

