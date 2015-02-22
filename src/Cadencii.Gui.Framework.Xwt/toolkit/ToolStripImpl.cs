using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cadencii.Gui.Toolkit
{
	public partial class ToolStripImpl : ToolStripBase, UiToolStrip
	{
		// ignorable.
		ToolStripRenderMode UiToolStrip.RenderMode { get; set; }


		ItemCollection items;

		public IList<UiToolStripItem> Items {
			get { return items ?? (items = new ItemCollection (this)); }
		}

		class ItemCollection : Collection<UiToolStripItem>
		{
			Xwt.HBox lv;

			public ItemCollection (Xwt.HBox lv)
			{
				this.lv = lv;
			}

			protected override void InsertItem (int index, UiToolStripItem item)
			{
				if (index != lv.Children.Count ())
					throw new NotSupportedException ();
				base.InsertItem (index, item);
				lv.PackStart ((Xwt.Widget) item);
			}

			protected override void ClearItems ()
			{
				base.ClearItems ();
				lv.Clear ();
			}

			protected override void RemoveItem (int index)
			{
				throw new NotSupportedException ();
			}

			protected override void SetItem (int index, UiToolStripItem item)
			{
				throw new NotSupportedException ();
			}
		}
	}
}
