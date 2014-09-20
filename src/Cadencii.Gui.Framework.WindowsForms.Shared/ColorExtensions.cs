using System;
using cadencii.java.awt;

namespace cadencii
{
	static class ColorExtensionsWF
	{
		public static Color ToAwt (this System.Drawing.Color c)
		{
			return new Color (c.R, c.G, c.B, c.A);
		}
		public static System.Drawing.Color ToNative (this Color c)
		{
			return System.Drawing.Color.FromArgb (c.getRed (), c.getGreen (), c.getBlue ());
		}
	}
}

