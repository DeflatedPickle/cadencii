using System;
using System.Linq;
using Cadencii.Gui;
using Keys = Cadencii.Gui.Toolkit.Keys;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using KeyEventHandler = System.Windows.Forms.KeyEventHandler;
using MouseButtons = System.Windows.Forms.MouseButtons;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;
using NKeyEventArgs = Cadencii.Gui.Toolkit.KeyEventArgs;
using NKeyPressEventArgs = Cadencii.Gui.Toolkit.KeyPressEventArgs;
using NKeyEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.KeyEventArgs>;
using NMouseButtons = Cadencii.Gui.Toolkit.MouseButtons;
using NMouseEventArgs = Cadencii.Gui.Toolkit.MouseEventArgs;
using NMouseEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.MouseEventArgs>;
using System.Collections.Generic;
using Cadencii.Gui.Toolkit;
using cadencii;

namespace Cadencii.Gui.Toolkit
{
	public partial class ToolStripContainerImpl : System.Windows.Forms.ToolStripContainer, UiToolStripContainer
	{
		void UiToolStripContainer.BottomToolStripPanel_SuspendLayout ()
		{
			BottomToolStripPanel.SuspendLayout ();
		}

		void UiToolStripContainer.ContentPanel_SuspendLayout ()
		{
			ContentPanel.SuspendLayout ();
		}

		void UiToolStripContainer.BottomToolStripPanel_ResumeLayout (bool b)
		{
			BottomToolStripPanel.ResumeLayout (b);
		}

		void UiToolStripContainer.BottomToolStripPanel_PerformLayout ()
		{
			BottomToolStripPanel.PerformLayout ();
		}

		void UiToolStripContainer.ContentPanel_ResumeLayout (bool b)
		{
			ContentPanel.ResumeLayout (b);
		}

		IList<UiControl> UiToolStripContainer.BottomToolStripPanel_Controls {
			get { return new CastingList<UiControl,System.Windows.Forms.Control> (BottomToolStripPanel.Controls, null, null); }
		}

		ToolStripRenderMode UiToolStripContainer.BottomToolStripPanel_RenderMode {
			get { return (ToolStripRenderMode)BottomToolStripPanel.RenderMode; }
			set { BottomToolStripPanel.RenderMode = (System.Windows.Forms.ToolStripRenderMode) value;}
		}

		IList<UiControl> UiToolStripContainer.ContentPanel_Controls {
			get { return new CastingList<UiControl,System.Windows.Forms.Control> (ContentPanel.Controls, null, null); }
		}

		Size UiToolStripContainer.ContentPanel_Size {
			get { return ContentPanel.Size.ToGui (); }
			set { ContentPanel.Size = value.ToWF (); }
		}
	}
}

