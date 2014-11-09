using System;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiUserControl : UiControl
	{
		BorderStyle BorderStyle { get; set; }
		void AddControl (UiControl control);
	}
	
}
