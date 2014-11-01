using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using cadencii.java.awt;
using System.Collections;

namespace cadencii
{
	// cannot derive from System.Windows.Forms.ImageList, so it delegates to impl.
	public class ImageListImpl : UiImageList
	{
		readonly System.Windows.Forms.ImageList impl;

		public ImageListImpl (System.ComponentModel.IContainer components)
		{
			impl = new System.Windows.Forms.ImageList (components);
		}

		#region UiImageList implementation

		void UiImageList.SetImagesKeyName (int i, string name)
		{
			impl.Images.SetKeyName (i, name);
		}

		cadencii.java.awt.ColorDepth UiImageList.ColorDepth {
			set { impl.ColorDepth = (System.Windows.Forms.ColorDepth) value; }
			get { return (cadencii.java.awt.ColorDepth)impl.ColorDepth; }
		}

		object UiImageList.ImageStream {
			get { return impl.ImageStream; }
			set { impl.ImageStream = (System.Windows.Forms.ImageListStreamer) value; }
		}

		cadencii.java.awt.Dimension UiImageList.ImageSize {
			get { return impl.ImageSize.ToAwt (); }
			set { impl.ImageSize = value.ToWF (); }
		}

		cadencii.java.awt.Color UiImageList.TransparentColor {
			get { return impl.TransparentColor.ToAwt (); }
			set { impl.TransparentColor = value.ToNative (); }
		}

		ICollection<cadencii.java.awt.Image> UiImageList.Images {
			get { return new CastingList<cadencii.java.awt.Image,System.Drawing.Image> (impl.Images, ExtensionsWF.ToAwt, ExtensionsWF.ToWF); }
		}

		#endregion
	}
}
