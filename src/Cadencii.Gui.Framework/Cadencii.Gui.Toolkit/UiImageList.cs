
using System;
using System.Collections.Generic;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiImageList : UiComponent
	{
		ColorDepth ColorDepth {
			get;
			set;
		}

		object ImageStream {
			get;
			set;
		}

		Size ImageSize {
			get;
			set;
		}

		void SetImagesKeyName (int i, string name);

		Color TransparentColor {
			get;
			set;
		}

		ICollection<Image> Images { get; }
	}

}

