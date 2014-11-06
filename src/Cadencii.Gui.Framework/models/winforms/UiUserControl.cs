using System;
using Cadencii.Gui;

namespace cadencii
{
	public interface UiUserControl : UiControl
	{
		BorderStyle BorderStyle { get; set; }
		void AddControl (UiControl control);
	}
	
}
