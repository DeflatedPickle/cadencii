using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cadencii.Gui;
using System.Collections;
using System.ComponentModel;
using Cadencii.Gui.Toolkit;
using cadencii;

namespace Cadencii.Gui.Toolkit
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

		public void SetImagesKeyName (int i, string name)
		{
			impl.Images.SetKeyName (i, name);
		}

		public ColorDepth ColorDepth {
			set { impl.ColorDepth = (System.Windows.Forms.ColorDepth) value; }
			get { return (ColorDepth)impl.ColorDepth; }
		}

		public object ImageStream {
			get { return impl.ImageStream; }
			set { impl.ImageStream = (System.Windows.Forms.ImageListStreamer) value; }
		}

		public Cadencii.Gui.Dimension ImageSize {
			get { return impl.ImageSize.ToAwt (); }
			set { impl.ImageSize = value.ToWF (); }
		}

		public Cadencii.Gui.Color TransparentColor {
			get { return impl.TransparentColor.ToAwt (); }
			set { impl.TransparentColor = value.ToNative (); }
		}

		public ICollection<Cadencii.Gui.Image> Images {
			get { return new CastingList<Cadencii.Gui.Image,System.Drawing.Image> (impl.Images, ExtensionsWF.ToAwt, ExtensionsWF.ToWF); }
		}
	}
}
