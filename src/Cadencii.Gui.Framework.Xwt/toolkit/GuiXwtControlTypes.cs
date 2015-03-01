using System;
using System.Linq;
using Cadencii.Gui;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public partial class ButtonBase : Xwt.Button
	{
	}

	public partial class CheckBoxBase : Xwt.CheckBox
	{
	}

	public partial class ComboBoxBase : Xwt.ComboBox
	{
	}

	public partial class ContainerControlBase : AbsoluteLayoutBase
	{
	}

	public partial class ContextMenuStripBase : Xwt.Widget
	{
	}

	public partial class FlowLayoutPanelBase : Xwt.Frame
	{
	}

	public partial class FormBase : AbsoluteLayoutBase
	{
	}

	public partial class GroupBoxBase : Xwt.Frame
	{
	}

	public partial class HScrollBarBase : Xwt.HScrollbar
	{
	}

	public partial class LabelBase : Xwt.Label
	{
	}

	public partial class LinkLabelBase : Xwt.LinkLabel
	{
	}

	public partial class ListBoxBase : Xwt.ListBox
	{
	}

	public partial class ListViewBase : Xwt.ListView
	{
	}

	public partial class MenuStripBase : Xwt.Widget
	{
	}

	public partial class NumericUpDownBase : Xwt.Spinner
	{
	}

	public partial class PanelBase : AbsoluteLayoutBase
	{
	}

	// content will be ImageView
	public partial class PictureBoxBase : Xwt.Frame
	{
	}

	public partial class ProgressBarBase : Xwt.ProgressBar
	{
	}

	public partial class PropertyGridBase : Xwt.VBox
	{
	}

	public partial class RadioButtonBase : Xwt.RadioButton
	{
	}

	public partial class SplitContainerBase : Xwt.VPaned
	{
	}

	public partial class StatusStripBase : Xwt.HBox
	{
	}

	public partial class TabControlBase : Xwt.Notebook
	{
	}

	public partial class TabPageBase : AbsoluteLayoutBase
	{
	}

	public partial class TextBoxBase : Xwt.TextEntry
	{
	}

	public partial class ToolBarBase : Xwt.SegmentedButton
	{
	}

	public partial class ToolStripBase : Xwt.HBox
	{
	}

	public partial class ToolStripContainerBase : Xwt.VBox
	{
	}

	public partial class HTrackBarBase : Xwt.HSlider
	{
	}

	public partial class VTrackBarBase : Xwt.VSlider
	{
	}

	public partial class UserControlBase : AbsoluteLayoutBase
	{
	}

	public partial class VScrollBarBase : Xwt.VScrollbar
	{
	}

	public abstract class AbsoluteLayoutBase : Xwt.Canvas, IControlContainer
	{
		#region IControlContainer implementation
		public void AddControl (UiControl control)
		{
			base.AddChild ((Xwt.Widget)control, control.Left, control.Top);
		}

		public void RemoveControl (UiControl control)
		{
			base.RemoveChild ((Xwt.Widget)control);
		}
		#endregion
	}
}
