using System;
using cadencii.java.awt;

namespace cadencii
{
	public interface UiUserControl : UiControl
	{
		BorderStyle BorderStyle { get; set; }
		void AddControl (UiControl control);
	}
	
}
