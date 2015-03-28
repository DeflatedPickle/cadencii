using System;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public partial class ImageListImpl  : UiImageList
	{
		public ImageListImpl (System.ComponentModel.IContainer components)
		{
		}

		Dictionary<int,string> keynames = new Dictionary<int, string> ();

		#region UiImageList implementation
		void UiImageList.SetImagesKeyName (int i, string name)
		{
			keynames [i] = name;
		}
		ColorDepth UiImageList.ColorDepth { get; set; }
		object UiImageList.ImageStream { get; set; }
		Size UiImageList.ImageSize { get; set; }
		Color UiImageList.TransparentColor { get; set; }
		System.Collections.Generic.ICollection<Image> UiImageList.Images {
			get {
				throw new NotImplementedException ();
			}
		}
		#endregion
	}
}

