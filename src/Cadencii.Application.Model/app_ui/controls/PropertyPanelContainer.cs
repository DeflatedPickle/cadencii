using System;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface PropertyPanelContainer : UiControl
	{
		void addComponent (UiControl propertyPanel);
		event StateChangeRequiredEventHandler StateChangeRequired;
	}
}

