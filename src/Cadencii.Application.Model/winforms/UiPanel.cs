using System;
using cadencii.java.awt;

namespace cadencii
{
	public interface UiPanel : UiControl
	{
		BorderStyle BorderStyle { get; set; }
	}
}

