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
			return new Color ((int) c.Red * 256, (int) c.Green * 256, (int) c.Blue * 256, (int) c.Alpha * 256);
		}

		public static Xwt.Drawing.Color ToNative (this Color c)
		{
			return Xwt.Drawing.Color.FromBytes ((byte) c.R, (byte) c.G, (byte) c.B);
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
			return new Point ((int) point.X, (int) point.Y);
		}

		public static Xwt.Point ToWF (this Point point)
		{
			return new Xwt.Point (point.X, point.Y);
		}

		public static Size ToGui (this Xwt.Size size)
		{
			return new Size ((int) size.Width, (int) size.Height);
		}

		public static Xwt.Size ToWF (this Size size)
		{
			return new Xwt.Size (size.Width, size.Height);
		}

		public static Rectangle ToGui (this Xwt.Rectangle rect)
		{
			return new Rectangle ((int) rect.X, (int) rect.Y, (int) rect.Width, (int) rect.Height);
		}

		public static Xwt.Rectangle ToWF (this Rectangle rect)
		{
			return new Xwt.Rectangle (rect.X, rect.Y, rect.Width, rect.Height);
		}

		public static Padding ToGui (this Xwt.WidgetSpacing padding)
		{
			return new Padding ((int) padding.Left, (int) padding.Top, (int) padding.Right, (int) padding.Bottom);
		}

		public static Xwt.WidgetSpacing ToWF (this Padding padding)
		{
			return new Xwt.WidgetSpacing (padding.Left, padding.Top, padding.Right, padding.Bottom);
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
			return new MouseEventArgs () { Button = (MouseButtons)e.Button, MultiplePress = e.Clicks, X = e.X, Y = e.Y };
		}

		public static NMouseEventArgs ToGui (this MouseEventArgs e)
		{
			// There is no wheel delta value in Xwt. Actually neither in WPF mouse event, so use fixed value.
			return new NMouseEventArgs ((NMouseButtons)e.Button, e.MultiplePress, (int) e.X, (int) e.Y, 120);
		}

		public static Font ToGui (this Xwt.Drawing.Font f)
		{
			return new Font (f);
		}

		public static Xwt.Drawing.Font ToWF (this Font f)
		{
			return (Xwt.Drawing.Font)f.NativeFont;
		}

		public static Graphics ToGui (this Xwt.Drawing.Context g)
		{
			return new Graphics () { NativeGraphics = g };
		}

		public static Xwt.Drawing.Context ToWF (this Graphics g)
		{
			return (Xwt.Drawing.Context) g.NativeGraphics;
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

			dialog.InitialFileName = file_name;
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
		*/

		static DragDropEffects ToGui (this Xwt.DragDropAction src)
		{
			DragDropEffects eff = default (DragDropEffects);
			if ((src & Xwt.DragDropAction.Copy) != 0)
				eff |= DragDropEffects.Copy;
			if ((src & Xwt.DragDropAction.Link) != 0)
				eff |= DragDropEffects.Link;
			if ((src & Xwt.DragDropAction.Move) != 0)
				eff |= DragDropEffects.Move;
			return eff;
		}

		public static DragEventArgs ToGui (this Xwt.DragEventArgs e)
		{
			return new DragEventArgs (new DataObjectXwt (e.Data), 0, (int) e.Position.X, (int) e.Position.Y, e.Action.ToGui (), e.Action.ToGui ());
		}

		public static DragEventArgs ToGui (this Xwt.DragOverEventArgs e)
		{
			return new DragEventArgs (new DataObjectXwt (e.Data), 0, (int) e.Position.X, (int) e.Position.Y, e.AllowedAction.ToGui (), e.Action.ToGui ());
		}

		public static DockStyle ToDock (this UiControl control, Xwt.WidgetPlacement h, Xwt.WidgetPlacement v)
		{
			DockStyle ret = DockStyle.None;
			bool both = false;

			switch (h) {
			case Xwt.WidgetPlacement.Fill:
				both = true;
				goto case Xwt.WidgetPlacement.Start;
			case Xwt.WidgetPlacement.Start:
				ret |= DockStyle.Left;
				if (both)
					goto case Xwt.WidgetPlacement.End;
				break;
			case Xwt.WidgetPlacement.End:
				ret |= DockStyle.Right;
				break;
			}

			both = false;
			switch (v) {
			case Xwt.WidgetPlacement.Fill:
				both = true;
				goto case Xwt.WidgetPlacement.Start;
			case Xwt.WidgetPlacement.Start:
				ret |= DockStyle.Top;
				if (both)
					goto case Xwt.WidgetPlacement.End;
				break;
			case Xwt.WidgetPlacement.End:
				ret |= DockStyle.Bottom;
				break;
			}

			return ret;
		}

		public static Xwt.WidgetPlacement ToHorizontalPlacement (this UiControl control, DockStyle d)
		{
			bool l = (d & DockStyle.Left) != 0;
			bool r = (d & DockStyle.Right) != 0;
			return l && r ? Xwt.WidgetPlacement.Fill : l ? Xwt.WidgetPlacement.Start : r ? Xwt.WidgetPlacement.End : Xwt.WidgetPlacement.Center;
		}

		public static Xwt.WidgetPlacement ToVerticalPlacement (this UiControl control, DockStyle d)
		{
			bool t = (d & DockStyle.Top) != 0;
			bool b = (d & DockStyle.Bottom) != 0;
			return t && b ? Xwt.WidgetPlacement.Fill : t ? Xwt.WidgetPlacement.Start : b ? Xwt.WidgetPlacement.End : Xwt.WidgetPlacement.Center;
		}
	}
}

