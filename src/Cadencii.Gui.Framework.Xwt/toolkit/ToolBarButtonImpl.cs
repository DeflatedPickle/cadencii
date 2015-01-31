using System;

namespace Cadencii.Gui.Toolkit
{
	public class ToolBarButtonImpl : Xwt.Button, UiToolBarButton
	{
		bool UiToolBarButton.Enabled {
			get { return base.Sensitive; }
			set { base.Sensitive = value; }
		}
		// FIXME: implement
		bool UiToolBarButton.Pushed {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		// FIXME: implement
		string UiToolBarButton.ImageKey {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		int UiToolBarButton.ImageIndex {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		string UiToolBarButton.Text {
			get { return base.Label; }
			set { base.Label = value; }
		}
		string UiToolBarButton.ToolTipText {
			get { return base.TooltipText; }
			set { base.TooltipText = value; }
		}
		Rectangle UiToolBarButton.Rectangle {
			get { return base.ScreenBounds.ToGui (); }
		}
	}

	public class ToolBarToggleButtonImpl : Xwt.ToggleButton, UiToolBarToggleButton
	{
		bool UiToolBarButton.Enabled {
			get { return base.Sensitive; }
			set { base.Sensitive = value; }
		}
		bool UiToolBarButton.Pushed {
			get { return base.Active; }
			set { base.Active = value; }
		}
		// FIXME: implement
		string UiToolBarButton.ImageKey {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		int UiToolBarButton.ImageIndex {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		string UiToolBarButton.Text {
			get { return base.Label; }
			set { base.Label = value; }
		}
		string UiToolBarButton.ToolTipText {
			get { return base.TooltipText; }
			set { base.TooltipText = value; }
		}
		Rectangle UiToolBarButton.Rectangle {
			get { return base.ScreenBounds.ToGui (); }
		}
	}

	// dunno what's good enough to implement it, so far just make it invisible.
	public class ToolBarSeparatorButtonImpl : ToolBarButtonImpl, UiToolBarSeparatorButton
	{
		public ToolBarSeparatorButtonImpl ()
		{
		}
	}
}
