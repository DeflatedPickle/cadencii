using System;

namespace cadencii
{
	public interface PropertyPanelContainer : UiControl
	{
		void addComponent (UiControl propertyPanel);
		event StateChangeRequiredEventHandler StateChangeRequired;
	}
}

