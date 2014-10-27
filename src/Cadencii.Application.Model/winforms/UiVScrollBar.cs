using System;

namespace cadencii
{
	public interface UiVScrollBar : UiControl
	{
		int LargeChange { get; set; }
		int SmallChange { get; set; }
		int Maximum { get; set; }
		int Minimum { get; set; }
		int Value { get; set; }
		event EventHandler ValueChanged;
	}
}

