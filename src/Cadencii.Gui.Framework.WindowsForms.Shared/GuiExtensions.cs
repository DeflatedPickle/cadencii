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

		public static Cursor ToAwt (this System.Windows.Forms.Cursor c)
		{
			return new Cursor (0) { NativeCursor = c };
		}

		public static System.Windows.Forms.Cursor ToNative (this Cursor c)
		{
			return (System.Windows.Forms.Cursor)c.NativeCursor;
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
			return new System.Drawing.Size (size.Width, size.Height);
		}

		public static Rectangle ToAwt (this System.Drawing.Rectangle rect)
		{
			return new Rectangle (rect.X, rect.Y, rect.Width, rect.Height);
		}

		public static System.Drawing.Rectangle ToWF (this Rectangle rect)
		{
			return new System.Drawing.Rectangle (rect.X, rect.Y, rect.Width, rect.Height);
		}

		public static Image ToAwt (this System.Drawing.Image image)
		{
			return image == null ? null : new Image () { NativeImage = image };
		}

		public static System.Drawing.Image ToWF (this Image image)
		{
			return image == null ? null : (System.Drawing.Image)image.NativeImage;
		}

		public static MouseEventArgs ToWF (this NMouseEventArgs e)
		{
			return new MouseEventArgs ((MouseButtons)e.Button, e.Clicks, e.X, e.Y, e.Delta);
		}

		public static NMouseEventArgs ToAwt (this MouseEventArgs e)
		{
			return new NMouseEventArgs ((NMouseButtons)e.Button, e.Clicks, e.X, e.Y, e.Delta);
		}

		public static Font ToAwt (this System.Drawing.Font f)
		{
			return AwtHost.Current.New<Font> (f);
		}

		public static System.Drawing.Font ToWF (this Font f)
		{
			return (System.Drawing.Font)f.NativeFont;
		}

		public static Graphics ToAwt (this System.Drawing.Graphics g)
		{
			return new Graphics () { NativeGraphics = g };
		}

		public static System.Drawing.Graphics ToWF (this Graphics g)
		{
			return (System.Drawing.Graphics)g.NativeGraphics;
		}

		public static cadencii.PaintEventArgs ToAwt (this System.Windows.Forms.PaintEventArgs e)
		{
			return new PaintEventArgs () { Graphics = e.Graphics.ToAwt () };
		}

		public static cadencii.ToolBarButtonClickEventArgs ToAwt (this System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			return new ToolBarButtonClickEventArgs () { Button = (UiToolBarButton)e.Button };
		}

		public static System.Windows.Forms.Control Mnemonic (this System.Windows.Forms.Control control, Keys value)
		{
			control.Text = GetMnemonicString (control.Text, value);
			return control;
		}

		public static System.Windows.Forms.ToolStripItem Mnemonic (this System.Windows.Forms.ToolStripItem item, Keys value)
		{
			item.Text = GetMnemonicString (item.Text, value);
			return item;
		}

		private static string GetMnemonicString (string text, Keys keys)
		{
			int value = (int)keys;
			if (value == 0) {
				return text;
			}
			if ((value < 48 || 57 < value) && (value < 65 || 90 < value)) {
				return text;
			}

			if (text.Length >= 2) {
				char lastc = text [0];
				int index = -1; // 第index文字目が、ニーモニック
				for (int i = 1; i < text.Length; i++) {
					char c = text [i];
					if (lastc == '&' && c != '&') {
						index = i;
					}
					lastc = c;
				}

				if (index >= 0) {
					string newtext = text.Substring (0, index) + new string ((char)value, 1) + ((index + 1 < text.Length) ? text.Substring (index + 1) : "");
					return newtext;
				}
			}
			return text + "(&" + new string ((char)value, 1) + ")";
		}

		public static string SelectedFilter(this System.Windows.Forms.FileDialog dialog)
		{
			string[] filters = dialog.Filter.Split('|');
			int index = dialog.FilterIndex;
			if (0 <= index && index < filters.Length) {
				return filters[index];
			} else {
				return string.Empty;
			}
		}

		public static System.Windows.Forms.FileDialog SetSelectedFile(this System.Windows.Forms.FileDialog dialog, string file_path)
		{
			string file_name = string.Empty;
			string initial_directory = System.IO.Directory.Exists(file_path) ? file_path : (System.IO.File.Exists(file_path) ? System.IO.Path.GetDirectoryName(file_path) : file_path);

			dialog.FileName = file_name;
			dialog.InitialDirectory = initial_directory;
			return dialog;
		}
	}
}

