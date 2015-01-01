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
	public partial class TextBoxImpl : System.Windows.Forms.TextBox, UiTextBox
	{
		BorderStyle UiTextBox.BorderStyle {
			get { return (BorderStyle)BorderStyle; }
			set { BorderStyle = (System.Windows.Forms.BorderStyle) value; }
		}

		HorizontalAlignment UiTextBox.TextAlign {
			get { return (HorizontalAlignment)TextAlign; }
			set { TextAlign = (System.Windows.Forms.HorizontalAlignment) value; }
		}

		string [] UiTextBox.Lines {
			get { return this.Lines; }
		}
	}
}

