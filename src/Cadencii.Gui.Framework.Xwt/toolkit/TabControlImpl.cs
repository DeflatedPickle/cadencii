using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cadencii.Gui.Toolkit
{
	public partial class TabControlImpl : TabControlBase, UiTabControl
	{
		TabPageCollection tabs;

		int UiTabControl.SelectedIndex {
			get { return base.CurrentTabIndex; }
			set { base.CurrentTabIndex = value; }
		}

		// ignore.
		bool UiTabControl.Multiline { get; set; }

		public new IList<UiTabPage> Tabs {
			get { return tabs ?? (tabs = new TabPageCollection (this)); }
		}

		class TabPageCollection : Collection<UiTabPage>
		{
			Xwt.Notebook nb;

			public TabPageCollection (Xwt.Notebook nb)
			{
				this.nb = nb;
			}

			protected override void InsertItem (int index, UiTabPage item)
			{
				var x = (TabPageImpl) item;
				base.InsertItem (index, item);
				x.OnAddedToTabControl (nb);
				//nb.Tabs.Add (x.NativeTab);
			}

			protected override void ClearItems ()
			{
				base.ClearItems ();
				nb.Tabs.Clear ();
			}

			protected override void RemoveItem (int index)
			{
				base.RemoveItem (index);
				nb.Tabs.RemoveAt (index);
			}

			protected override void SetItem (int index, UiTabPage item)
			{
				throw new NotSupportedException ();
			}
		}
	}
}
