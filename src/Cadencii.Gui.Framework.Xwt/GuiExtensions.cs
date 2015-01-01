using System;
using Cadencii.Gui;
using MouseButtons = Xwt.PointerButton;
using MouseEventArgs = Xwt.ButtonEventArgs;
using MouseEventHandler = System.EventHandler<Xwt.ButtonEventArgs>;
using NMouseButtons = Cadencii.Gui.Toolkit.MouseButtons;
using NMouseEventArgs = Cadencii.Gui.Toolkit.MouseEventArgs;
using Cadencii.Gui.Toolkit;
using System.Linq;

namespace Cadencii.Gui
{

	// This file is directly added to Cadencii.Appication.WindowsForms.csproj because
	// the shared project somehow causes resgen compilation error.
	static class ExtensionsXwt
	{
		public static Color ToGui (this Xwt.Drawing.Color c)
		{
			return new Color (c.Red, c.Green, c.Blue, c.Alpha);
		}

		public static Xwt.Drawing.Color ToNative (this Color c)
		{
			return Xwt.Drawing.Color.FromBytes (c.R, c.G, c.B);
		}

		public static Cursor ToGui (this Xwt.CursorType c)
		{
			return new Cursor (0) { NativeCursor = c };
		}

		public static Xwt.CursorType ToNative (this Cursor c)
		{
			return (Xwt.CursorType) c.NativeCursor;
		}

		public static Point ToGui (this Xwt.Point point)
		{
			return new Point (point.X, point.Y);
		}

		public static Xwt.Point ToWF (this Point point)
		{
			return new Xwt.Point (point.X, point.Y);
		}

		public static Size ToGui (this Xwt.Size size)
		{
			return new Size (size.Width, size.Height);
		}

		public static Xwt.Size ToWF (this Size size)
		{
			return new Xwt.Size (size.Width, size.Height);
		}

		public static Rectangle ToGui (this Xwt.Rectangle rect)
		{
			return new Rectangle (rect.X, rect.Y, rect.Width, rect.Height);
		}

		public static Xwt.Rectangle ToWF (this Rectangle rect)
		{
			return new Xwt.Rectangle (rect.X, rect.Y, rect.Width, rect.Height);
		}

		public static Padding ToGui (this Padding padding)
		{
			return padding;
		}

		public static Padding ToWF (this Padding padding)
		{
			return padding;
		}

		public static Image ToGui (this Xwt.Drawing.Image image)
		{
			return image == null ? null : new Image () { NativeImage = image };
		}

		public static Xwt.Drawing.Image ToWF (this Image image)
		{
			return image == null ? null : (Xwt.Drawing.Image)image.NativeImage;
		}

		public static MouseEventArgs ToWF (this NMouseEventArgs e)
		{
			return new MouseEventArgs ((MouseButtons)e.Button, e.Clicks, e.X, e.Y);
		}

		public static NMouseEventArgs ToGui (this MouseEventArgs e)
		{
			// There is no wheel delta value in Xwt. Actually neither in WPF mouse event, so use fixed value.
			return new NMouseEventArgs ((NMouseButtons)e.Button, e.MultiplePress, e.X, e.Y, 120);
		}

		public static Font ToGui (this Xwt.Drawing.Font f)
		{
			return new Font (f);
		}

		public static Xwt.Drawing.Font ToWF (this Font f)
		{
			return (Xwt.Drawing.Font)f.NativeFont;
		}

		public static Graphics ToGui (this Xwt.Drawing.ImageBuilder g)
		{
			return new Graphics () { NativeGraphics = g };
		}

		public static Xwt.Drawing.ImageBuilder ToWF (this Graphics g)
		{
			return (Xwt.Drawing.ImageBuilder) g.NativeGraphics;
		}

		/*
		public static PaintEventArgs ToGui (this System.Windows.Forms.PaintEventArgs e)
		{
			return new PaintEventArgs () { Graphics = e.Graphics.ToGui (), ClipRectangle = e.ClipRectangle.ToGui () };
		}

		public static System.Windows.Forms.PaintEventArgs ToWF (this PaintEventArgs e)
		{
			return new System.Windows.Forms.PaintEventArgs (e.Graphics.ToWF (), e.ClipRectangle.ToWF ());
		}

		public static ToolBarButtonClickEventArgs ToGui (this System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			return new ToolBarButtonClickEventArgs () { Button = (UiToolBarButton)e.Button };
		}

		public static Xwt.Widget Mnemonic (this Xwt.Widget control, Keys value)
		{
			control.Text = GetMnemonicString (control.Text, value);
			return control;
		}

		public static System.Windows.Forms.ToolStripItem Mnemonic (this System.Windows.Forms.ToolStripItem item, Keys value)
		{
			item.Text = GetMnemonicString (item.Text, value);
			return item;
		}
		*/

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

		public static string SelectedFilter(this Xwt.FileDialog dialog)
		{
			return dialog.ActiveFilter != null ? dialog.ActiveFilter.Patterns.First () : null;
		}

		public static Xwt.FileDialog SetSelectedFile(this Xwt.FileDialog dialog, string file_path)
		{
			string file_name = string.Empty;
			string initial_directory = System.IO.Directory.Exists(file_path) ? file_path : (System.IO.File.Exists(file_path) ? System.IO.Path.GetDirectoryName(file_path) : file_path);

			dialog.FileName = file_name;
			dialog.CurrentFolder = initial_directory;
			return dialog;
		}

		/*
		public static DrawToolTipEventArgs ToGui (this System.Windows.Forms.DrawToolTipEventArgs e)
		{
			return new DrawToolTipEventArgs (e.Bounds.ToGui (), e.DrawBackground, e.DrawBorder, f => e.DrawText ((System.Windows.Forms.TextFormatFlags) f));
		}

		public static FormClosingEventArgs ToGui (this System.Windows.Forms.FormClosingEventArgs e)
		{
			return new FormClosingEventArgs () { Cancel = e.Cancel };
		}

		public static DragEventArgs ToGui (this System.Windows.Forms.DragEventArgs e)
		{
			return new DragEventArgs (new DataObjectWF (e.Data), e.KeyState, e.X, e.Y, (DragDropEffects) e.AllowedEffect, (DragDropEffects) e.Effect);
		}
		*/
	}
}

