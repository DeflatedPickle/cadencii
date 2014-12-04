using System;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Forms;
using Cadencii.Application.Drawing;

namespace Cadencii.Application.Controls
{
	public interface PictOverview : UiPictureBox, IImageCachedComponentDrawer
	{
		void InvokeOnUiThread (Action action);

		void updateCachedImage ();

		void setMainForm (FormMain formMain);

		event EventHandler MouseLeave;
	}
}

