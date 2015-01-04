using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class ComboBoxImpl : UiComboBox
	{
		event System.EventHandler Cadencii.Gui.Toolkit.UiComboBox.SelectedIndexChanged {
			add { this.SelectionChanged += value; }
			remove { this.SelectionChanged -= value; }
		}

		System.Collections.IList Cadencii.Gui.Toolkit.UiComboBox.Items {
			get { return this.Items; }
		}

		string Cadencii.Gui.IHasText.Text {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

	}
}

