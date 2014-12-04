using System;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Forms;

namespace Cadencii.Application.Controls
{
	public interface PictOverview : UiPictureBox
	{
		void updateCachedImage ();

		void setMainForm (FormMain formMain);

		event EventHandler MouseLeave;
	}
}

