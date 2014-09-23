using System;

namespace cadencii
{
	public interface PictOverview : UiPictureBox
	{
		void updateCachedImage ();

		void setMainForm (FormMainUi formMain);
	}
}

