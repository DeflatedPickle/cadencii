using System;
using cadencii.java.awt;

namespace cadencii
{
	static class ColorExtensionsWF
	{
		public static System.Drawing.Color ToNative (this Color c)
		{
			return System.Drawing.Color.FromArgb (c.getRed (), c.getGreen (), c.getBlue ());
		}
	}
}

