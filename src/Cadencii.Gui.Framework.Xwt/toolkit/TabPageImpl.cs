using System;
using System.Linq;

namespace Cadencii.Gui.Toolkit
{
	public partial class TabPageImpl : TabPageBase, UiTabPage
	{
		public TabPageImpl ()
		{
			this.ExpandHorizontal = true;
			this.ExpandVertical = true;
		}

		public Xwt.NotebookTab NativeTab { get; set; }

		internal void OnAddedToTabControl (Xwt.Notebook parent)
		{
			parent.Add (this, text ?? "(dummy)");
			NativeTab = parent.Tabs.Last ();
		}

		// ignore.
		bool UiTabPage.AutoScroll { get; set; }

		// ignore?
		Padding UiTabPage.Padding { get; set; }

		string text;

		string Cadencii.Gui.IHasText.Text {
			get { return text; }
			set {
				text = value;
				if (NativeTab != null)
					NativeTab.Label = value;
			}
		}
	}
}

