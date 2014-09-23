using System;
using cadencii.java.awt;
using MouseButtons = System.Windows.Forms.MouseButtons;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;
using NMouseButtons = cadencii.java.awt.MouseButtons;
using NMouseEventArgs = cadencii.java.awt.MouseEventArgs;
using NMouseEventHandler = cadencii.java.awt.MouseEventHandler;

namespace cadencii
{
	// This file is directly added to Cadencii.Appication.WindowsForms.csproj because 
	// the shared project somehow causes resgen compilation error.
	static class ExtensionsWF
	{
		public static Color ToAwt (this System.Drawing.Color c)
		{
			return new Color (c.R, c.G, c.B, c.A);
		}

		public static System.Drawing.Color ToNative (this Color c)
		{
			return System.Drawing.Color.FromArgb (c.R, c.G, c.B);
		}

		public static cadencii.java.awt.Point ToAwt (this System.Drawing.Point point)
		{
			return new cadencii.java.awt.Point (point.X, point.Y);
		}

		public static System.Drawing.Point ToWF (this cadencii.java.awt.Point point)
		{
			return new System.Drawing.Point (point.X, point.Y);
		}
		
		public static cadencii.java.awt.Dimension ToAwt (this System.Drawing.Size size)
		{
			return new cadencii.java.awt.Dimension (size.Width, size.Height);
		}

		public static System.Drawing.Size ToWF (this Dimension size)
		{
			return new System.Drawing.Size (size.width, size.height);
		}
		
		public static Rectangle ToAwt (this System.Drawing.Rectangle rect)
		{
			return new Rectangle (rect.X, rect.Y, rect.Width, rect.Height);
		}

		public static System.Drawing.Rectangle ToWF (this Rectangle rect)
		{
			return new System.Drawing.Rectangle (rect.x, rect.y, rect.width, rect.height);
		}

		public static Image ToAwt (this System.Drawing.Image image)
		{
			return new Image () { NativeImage = image };
		}

		public static System.Drawing.Image ToWF (this Image image)
		{
			return (System.Drawing.Image) image.NativeImage;
		}

		public static MouseEventArgs ToWF (this NMouseEventArgs e)
		{
			return new MouseEventArgs ((MouseButtons) e.Button, e.Clicks, e.X, e.Y, e.Delta);
		}

		public static NMouseEventArgs ToAwt (this MouseEventArgs e)
		{
			return new NMouseEventArgs ((NMouseButtons) e.Button, e.Clicks, e.X, e.Y, e.Delta);
		}

		public static Font ToAwt (this System.Drawing.Font f)
		{
			return AwtHost.Current.New<Font> (f);
		}

		public static System.Drawing.Font ToWF (this Font f)
		{
			return (System.Drawing.Font) f.NativeFont;
		}
	}
}

