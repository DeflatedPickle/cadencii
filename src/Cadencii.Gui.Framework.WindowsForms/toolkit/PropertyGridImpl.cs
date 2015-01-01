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
	public partial class PropertyGridImpl : System.Windows.Forms.PropertyGrid, UiPropertyGrid
	{
		class GridItemImpl : UiGridItem 
		{
			public GridItemImpl (System.Windows.Forms.GridItem item)
			{
                if (item == null)
                    throw new ArgumentNullException("item");
				this.item = item;
			}

			System.Windows.Forms.GridItem item;

			#region UiGridItem implementation
			System.ComponentModel.PropertyDescriptor UiGridItem.PropertyDescriptor {
				get { return item.PropertyDescriptor; }
			}
			UiGridItem UiGridItem.Parent {
				get { return item.Parent == null ? null : new GridItemImpl (item.Parent); }
			}
			bool UiGridItem.Expandable {
				get { return item.Expandable; }
			}
			bool UiGridItem.Expanded {
				get { return item.Expanded; }
				set { item.Expanded = value; }
			}
			string UiGridItem.Label {
				get { return item.Label; }
			}
			IEnumerable<UiGridItem> UiGridItem.GridItems {
				get { return item.GridItems.Cast<System.Windows.Forms.GridItem> ().Select (i => new GridItemImpl (i)); }
			}
			#endregion
		}

		// UiPropertyGrid

		UiGridItem UiPropertyGrid.SelectedGridItem {
			get { return SelectedGridItem == null ? null : new GridItemImpl (SelectedGridItem); }
		}

		PropertySort UiPropertyGrid.PropertySort {
			get { return (PropertySort) PropertySort; }
			set { PropertySort = (System.Windows.Forms.PropertySort) value; }
		}

		event EventHandler UiPropertyGrid.SelectedGridItemChanged {
			add { SelectedGridItemChanged += (sender, e) => value (sender, e); }
			remove { throw new NotImplementedException (); }
		}

		event EventHandler UiPropertyGrid.PropertyValueChanged {
			add { PropertyValueChanged += (sender, e) => value (sender, e); }
			remove { throw new NotImplementedException (); }
		}
	}
}

