
using System;
using System.Collections.Generic;
using cadencii.java.awt;

namespace cadencii
{
	public interface UiImageList : UiComponent
	{
		Dimension ImageSize {
			get;
			set;
		}

		void SetImagesKeyName (int i, string diskpluspng);

		Color TransparentColor {
			get;
			set;
		}

		List<Image> Images { get; }
	}

}

