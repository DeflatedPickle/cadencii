using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public interface UiGridItem
	{
		PropertyDescriptor PropertyDescriptor { get; }
		UiGridItem Parent { get; }
		bool Expandable { get; }
		bool Expanded { get; set; }
		string Label { get; }
		IEnumerable<UiGridItem> GridItems { get; }
	}
}

