using System;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface PictOverview : UiPictureBox
	{
		void updateCachedImage ();

		void setMainForm (FormMain formMain);
	}
}

