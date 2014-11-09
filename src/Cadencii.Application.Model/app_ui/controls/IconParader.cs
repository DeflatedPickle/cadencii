using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public abstract class IconParaderController
	{
		public const int ICON_WIDTH = 48;
		public const int ICON_HEIGHT = 48;

		public delegate Image IconImageCreator (string path_image,string vOICENAME);

		static IconParaderController ()
		{
			createIconImage = ApplicationUIHost.Create<IconParaderController> ().DoCreateIconImage;
		}

		public static IconImageCreator createIconImage;

		public abstract IconImageCreator DoCreateIconImage { get; }
	}

	public interface IconParader : UiPictureBox
	{
		void setImage (Image img);
	}
}

