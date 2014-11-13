using System;
using Cadencii.Gui.Toolkit;
using cadencii;

namespace Cadencii.Application.Controls
{
	public interface PropertyPanelContainer : UiControl
	{
		void addComponent (UiControl propertyPanel);
		event StateChangeRequiredEventHandler StateChangeRequired;
	}
}

