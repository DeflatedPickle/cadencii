using System;
using cadencii.java.awt;
using cadencii.vsq;

namespace cadencii
{
	public interface UiUserControl : UiControl
	{
		BorderStyle BorderStyle { get; set; }
		void AddControl (UiControl control);
	}
	
}
