using System;
using cadencii.java.awt;
using cadencii.vsq;

namespace cadencii
{
	public interface UiControl
	{
		object Native { get; }
		Padding Margin { get; set; }
		string Name { get; set; }
		int TabIndex { get; set; }
		DockStyle Dock { get; set; }
		int Width { get; set; }
		int Height { get; set; }
	}
	
}
