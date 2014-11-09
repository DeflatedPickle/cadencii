using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cadencii.Gui;
using System.Collections;
using System.ComponentModel;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	// cannot derive from System.Windows.Forms.ImageList, so it delegates to impl.
	public class ImageListImpl : UiImageList
	{
		readonly System.Windows.Forms.ImageList impl;

		public ImageListImpl (System.Windows.Forms.ImageList impl)
		{
			this.impl = impl;
		}

		public ImageListImpl (IContainer components)
		{
			this.impl = new System.Windows.Forms.ImageList (components);
		}

		public System.Windows.Forms.ImageList Native {
			get { return impl; }
		}

		#region UiImageList implementation

		void UiImageList.SetImagesKeyName (int i, string name)
		{
			impl.Images.SetKeyName (i, name);
		}

		ColorDepth UiImageList.ColorDepth {
			set { impl.ColorDepth = (System.Windows.Forms.ColorDepth) value; }
			get { return (ColorDepth)impl.ColorDepth; }
		}

		object UiImageList.ImageStream {
			get { return impl.ImageStream; }
			set { impl.ImageStream = (System.Windows.Forms.ImageListStreamer) value; }
		}

		Cadencii.Gui.Dimension UiImageList.ImageSize {
			get { return impl.ImageSize.ToAwt (); }
			set { impl.ImageSize = value.ToWF (); }
		}

		Cadencii.Gui.Color UiImageList.TransparentColor {
			get { return impl.TransparentColor.ToAwt (); }
			set { impl.TransparentColor = value.ToNative (); }
		}

		ICollection<Cadencii.Gui.Image> UiImageList.Images {
			get { return new CastingList<Cadencii.Gui.Image,System.Drawing.Image> (impl.Images, ExtensionsWF.ToAwt, ExtensionsWF.ToWF); }
		}

		#endregion
	}
}
