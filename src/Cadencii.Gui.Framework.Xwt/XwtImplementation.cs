/*
 * awt.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.core.
 *
 * cadencii.core is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.core is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using cadencii;
using Cadencii.Gui.Toolkit;
using System.Linq;
using Cadencii.Utilities;

namespace Cadencii.Gui
{
	public partial class GuiHostXwt : GuiHost
	{
		public GuiHostXwt ()
		{
			Types [typeof(Image.ImageAdapter)] = typeof(ImageAdapterWF);
			Types [typeof(Graphics.GraphicsAdapter)] = typeof(GraphicsAdapterWF);
		}

		public override void InitializeSystemColors ()
		{
			SystemColors.ActiveBorder = Colors.Black;// System.Drawing.SystemColors.ActiveBorder.ToGui ();
			SystemColors.Control = Colors.White; // System.Drawing.SystemColors.Control.ToGui ();
			SystemColors.ControlDark = Colors.Black; // System.Drawing.SystemColors.ControlDark.ToGui ();
			SystemColors.ControlText = Colors.Black; // System.Drawing.SystemColors.ControlText.ToGui ();
			SystemColors.Window = Colors.Blue; // System.Drawing.SystemColors.Window.ToGui ();
			SystemColors.WindowText = Colors.White; // System.Drawing.SystemColors.WindowText.ToGui ();
		}

		public override void InitializeCursors ()
		{
			Cursors.Default = Xwt.CursorType.Arrow.ToGui ();
			Cursors.Hand = Xwt.CursorType.Hand.ToGui ();
			Cursors.HSplit = Xwt.CursorType.ResizeLeftRight.ToGui ();
			Cursors.VSplit = Xwt.CursorType.ResizeUpDown.ToGui ();
			Cursors.NoMoveHoriz = Xwt.CursorType.Arrow.ToGui (); // nothing appopriate...
		}

		public override void ApplicationDoEvents ()
		{
			// no good GUI toolkits should have this.
		}

		public override Cadencii.Gui.Toolkit.Keys DefaultModifierKeys ()
		{
			Keys ret = Keys.None;
			if ((Xwt.Keyboard.CurrentModifiers & Xwt.ModifierKeys.Alt) != 0)
				ret |= Keys.Alt;
			if ((Xwt.Keyboard.CurrentModifiers & Xwt.ModifierKeys.Control) != 0)
				ret |= Keys.Control;
			if ((Xwt.Keyboard.CurrentModifiers & Xwt.ModifierKeys.Shift) != 0)
				ret |= Keys.Shift;
			// Command key is not cross platform. Do not expect that.
			return ret;
		}

		public override int PlatformMouseHoverTime {
			get { throw new NotImplementedException (); }
		}

		/// <summary>
		/// java:コンポーネントのnameプロパティを返します。C#:コントロールのNameプロパティを返します。
		/// objがnullだったり、型がComponent/Controlでない場合は空文字を返します。
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override string GetComponentName (Object obj)
		{
			if (obj == null) {
				return "";
			}
			if (obj is Xwt.Widget) {
				return ((Xwt.Widget) obj).Name;
			} else {
				return "";
			}
		}

		public override void ApplyFontRecurse (UiControl control, Font font)
		{
			// no way to retrieve "Controls". Also not likely needed.
		}

		/*
		// that's what mono Windows.Forms does...
		public override int HorizontalScrollBarThumbWidth {
			get { return 16; } // System.Windows.Forms.SystemInformation.HorizontalScrollBarThumbWidth; }
		}
		public override int VerticalScrollBarThumbHeight {
			get { return 16; } // System.Windows.Forms.SystemInformation.VerticalScrollBarThumbHeight; }
		}
		public override int VerticalScrollBarWidth {
			get { return 16; } // System.Windows.Forms.SystemInformation.VerticalScrollBarWidth; }
		}
		*/

		public override Font SystemMenuFont {
			get { return Xwt.Drawing.Font.SystemFont.ToGui (); }
		}
	}

	class ImageAdapterWF : Image.ImageAdapter
	{
		public ImageAdapterWF ()
		{
		}

		public ImageAdapterWF (int width, int height)
		{
			should_dispose = true;
			image = new Xwt.Drawing.ImageBuilder (width, height).ToBitmap ();
		}

		Xwt.Drawing.Image image;
		bool should_dispose;

		public override object NativeImage {
			get { return image; }
			set { image = (Xwt.Drawing.Image) value; }
		}

		public override void Dispose ()
		{
			if (should_dispose && image != null)
				image.Dispose ();
		}

		public override int Width {
			get { return (int) image.Width; }
		}

		public override int Height {
			get { return (int) image.Height; }
		}

		public override void Save (System.IO.Stream stream)
		{
			image.Save (stream, Xwt.Drawing.ImageFileType.Png);
		}
	}

	class CursorAdapterWF : Cursor.CursorAdapter
	{
		private int m_type = Cursor.DEFAULT_CURSOR;
		Xwt.CursorType cursor = Xwt.CursorType.Arrow;

		public CursorAdapterWF (int type)
		{
			m_type = type;
			if (m_type == Cursor.CROSSHAIR_CURSOR) {
				cursor = Xwt.CursorType.Crosshair;
			} else if (m_type == Cursor.HAND_CURSOR) {
				cursor = Xwt.CursorType.Hand;
			} else if (m_type == Cursor.TEXT_CURSOR) {
				cursor = Xwt.CursorType.IBeam;
			} else if (m_type == Cursor.E_RESIZE_CURSOR) {
				cursor = Xwt.CursorType.ResizeRight;
			} else if (m_type == Cursor.NE_RESIZE_CURSOR) {
				cursor = Xwt.CursorType.Move; // no corresponding cursor in Xwt
			} else if (m_type == Cursor.N_RESIZE_CURSOR) {
				cursor = Xwt.CursorType.ResizeUp;
			} else if (m_type == Cursor.NW_RESIZE_CURSOR) {
				cursor = Xwt.CursorType.Move; // no corresponding cursor in Xwt
			} else if (m_type == Cursor.SE_RESIZE_CURSOR) {
				cursor = Xwt.CursorType.Move; // no corresponding cursor in Xwt
			} else if (m_type == Cursor.S_RESIZE_CURSOR) {
				cursor = Xwt.CursorType.ResizeDown;
			} else if (m_type == Cursor.SW_RESIZE_CURSOR) {
				cursor = Xwt.CursorType.Move; // no corresponding cursor in Xwt
			} else if (m_type == Cursor.W_RESIZE_CURSOR) {
				cursor = Xwt.CursorType.ResizeLeft;
			} else if (m_type == Cursor.MOVE_CURSOR) {
				cursor = Xwt.CursorType.Move;
			}
		}

		public override void Dispose ()
		{
			// do nothing
		}

		public override object NativeCursor {
			get { return cursor; }
			set {
				cursor = (Xwt.CursorType)value;
				if (cursor == Xwt.CursorType.Crosshair)
					m_type = Cursor.CROSSHAIR_CURSOR;
				if (cursor == Xwt.CursorType.Hand)
					m_type = Cursor.HAND_CURSOR;
				if (cursor == Xwt.CursorType.IBeam)
					m_type = Cursor.TEXT_CURSOR;
				if (cursor == Xwt.CursorType.ResizeRight)
					m_type = Cursor.E_RESIZE_CURSOR;
				//if (cursor == Xwt.CursorType.PanNE)
				//	m_type = Cursor.NE_RESIZE_CURSOR;
				if (cursor == Xwt.CursorType.ResizeUp)
					m_type = Cursor.N_RESIZE_CURSOR;
				//if (cursor == Xwt.CursorType.PanNW)
				//	m_type = Cursor.NW_RESIZE_CURSOR;
				//if (cursor == Xwt.CursorType.PanSE)
				//	m_type = Cursor.SE_RESIZE_CURSOR;
				if (cursor == Xwt.CursorType.ResizeDown)
					m_type = Cursor.S_RESIZE_CURSOR;
				//if (cursor == Xwt.CursorType.PanSW)
				//	m_type = Cursor.SW_RESIZE_CURSOR;
				if (cursor == Xwt.CursorType.ResizeLeft)
					m_type = Cursor.W_RESIZE_CURSOR;
				if (cursor == Xwt.CursorType.Move)
					m_type = Cursor.MOVE_CURSOR;
			}
		}

		public override int Type {
			get { return m_type; }
		}
	}

	class GraphicsAdapterWF : Graphics.GraphicsAdapter
	{
		Xwt.Drawing.ImageBuilder builder;
		StrokeXwt stroke = new StrokeXwt () { Color = Cadencii.Gui.Colors.Black.ToNative () };
		Xwt.Drawing.Font m_font;
		bool should_dispose;

		public GraphicsAdapterWF ()
		{
			m_font = Xwt.Drawing.Font.SystemFont.WithSize (10);
		}

		public GraphicsAdapterWF (Image image)
		{
			should_dispose = true;
			builder = new Xwt.Drawing.ImageBuilder (image.Width, image.Height);
			builder.Context.DrawImage (image.ToWF (), Point.Empty.ToWF ());
		}

		public GraphicsAdapterWF (Graphics other)
		{
			var a = (GraphicsAdapterWF) other.Adapter;
			builder = a.builder;
			m_font = a.m_font;
		}

		public override object NativeGraphics { get { return builder; } set { builder = (Xwt.Drawing.ImageBuilder) value; } }

		// FIXME: implement
		public override object NativeBrush { get; set; }

		// FIXME: implement
		public override SmoothingMode SmoothingMode { get; set; }

		public override void Dispose ()
		{
			if (should_dispose && builder != null)
				builder.Dispose ();
		}

		public override void clearRect (int x, int y, int width, int height)
		{
			builder.Context.SetColor (Colors.White.ToNative ());
			builder.Context.Rectangle (x, y, width, height);
			builder.Context.Fill ();
		}

		public override void drawLine (int x1, int y1, int x2, int y2)
		{
			builder.Context.SetColor (stroke.Color);
			builder.Context.MoveTo (x1, y1);
			builder.Context.LineTo (x2, y2);
			builder.Context.Stroke ();
		}

		public override void drawRect (int x, int y, int width, int height)
		{
			builder.Context.SetColor (stroke.Color);
			builder.Context.Rectangle (x, y, width, height);
			builder.Context.Stroke ();
		}

		public override void drawRect (Color pen, Rectangle rect)
		{
			setColor (pen);
			drawRect (rect.X, rect.Y, rect.Width, rect.Height);
		}

		public override void fillRect (int x, int y, int width, int height)
		{
			builder.Context.Rectangle (x, y, width, height);
			builder.Context.Fill ();
		}

		public override void fillRect (Color fillcolor, int x, int y, int width, int height)
		{
			setColor (fillcolor);
			fillRect (x, y, width, height);
		}

		public override void fillEllipse (Color mDotColor, int i, int i2, int mDotWidth, int mDotWidth2)
		{
			throw new NotImplementedException ();
		}

		public override void drawOval (int x, int y, int width, int height)
		{
			throw new NotImplementedException ();
		}

		public override void fillOval (int x, int y, int width, int height)
		{
			throw new NotImplementedException ();
		}

		public override void drawBezier (float x1, float y1,
			float ctrlx1, float ctrly1,
			float ctrlx2, float ctrly2,
			float x2, float y2)
		{
			builder.Context.MoveTo (x1, y1);
			builder.Context.RelCurveTo (ctrlx1, ctrly1, ctrlx2, ctrly2, x2, y2);
			builder.Context.Stroke ();
		}

		public override void setColor (Color c)
		{
			stroke.Color = c.ToNative ();
			builder.Context.SetColor (c.ToNative ());
		}

		public override Color getColor ()
		{
			return stroke.Color.ToGui ();
		}

		public override void setFont (Font font)
		{
			var f = (Xwt.Drawing.Font) font.NativeFont;
			if (f == null)
				throw new ArgumentException ("Native font is null", "font");
			m_font = f;
		}

		public override void setStroke (Stroke stroke)
		{
			this.stroke = (StrokeXwt) stroke.NativePen;
		}

		public override Stroke getStroke ()
		{
			return new Stroke () { NativePen = stroke };
		}

		public override void drawString (string str, float x, float y)
		{
			builder.Context.DrawTextLayout (new Xwt.Drawing.TextLayout () { Font = m_font, Text = str }, x, y);
		}

		public override void drawPolygon (Polygon p)
		{
			drawPolygon (p.xpoints, p.ypoints, p.npoints);
		}

		public override void drawPolygon (int[] xPoints, int[] yPoints, int nPoints)
		{
			DrawPolySomething (xPoints, yPoints, nPoints, true, false);
		}

		public override void drawPolyline (int[] xPoints, int[] yPoints, int nPoints)
		{
			DrawPolySomething (xPoints, yPoints, nPoints, false, false);
		}

		void DrawPolySomething (int [] xPoints, int [] yPoints, int nPoints, bool close, bool fill)
		{
			var path = new Xwt.Drawing.DrawingPath ();
			if (nPoints > 0)
				path.MoveTo (xPoints [0], yPoints [0]);
			for (int i = 1; i < nPoints; i++)
				path.RelLineTo (xPoints [i], yPoints [i]);
			if (close)
				path.ClosePath ();
			builder.Context.AppendPath (path);
			if (fill)
				builder.Context.Fill ();
			else
				builder.Context.Stroke ();
		}

		public override void DrawLines (Point[] points)
		{
			var path = new Xwt.Drawing.DrawingPath ();
			if (points.Length > 0)
				path.MoveTo (points [0].ToWF ());
			for (int i = 1; i < points.Length; i++)
				path.LineTo (points [i].ToWF ());
			builder.Context.AppendPath (path);
			builder.Context.Stroke ();
		}

		public override void drawLines (int mLineWidth, Color mLineColor, Point[] mPoints)
		{
			builder.Context.SetLineWidth (mLineWidth);
			builder.Context.SetColor (mLineColor.ToNative ());
			DrawLines (mPoints);
		}

		public override void fillPolygon (Polygon p)
		{
			fillPolygon (null, p.xpoints, p.ypoints, p.npoints);
		}

		public override void fillPolygon (Color fillColor, Polygon p)
		{
			fillPolygon (fillColor, p.xpoints, p.ypoints, p.npoints);
		}

		public override void fillPolygon (int[] xPoints, int[] yPoints, int nPoints)
		{
			fillPolygon (null, xPoints, yPoints, nPoints);
		}

		void fillPolygon (Color? fillColor, int[] xPoints, int[] yPoints, int nPoints)
		{
			if (fillColor != null)
				builder.Context.SetColor (fillColor.Value.ToNative ());
			DrawPolySomething (xPoints, yPoints, nPoints, true, true);
		}

		public override void fill (Shape s)
		{
			if (s == null) {
				return;
			}

			if (s is Area) {
				Area a = (Area)s;
				if (a.NativeRegion != null) {
					// FIXME: brush
					builder.Context.AppendPath ((Xwt.Drawing.DrawingPath)a.NativeRegion);
					builder.Context.Fill ();
				}
			} else if (s is Rectangle) {
				// FIXME: brush
				Rectangle rc = (Rectangle)s;
				builder.Context.Rectangle (rc.ToWF ());
				builder.Context.Fill ();
			}
			else
				throw new NotImplementedException ();
		}

		public override Shape getClip ()
		{
			Area ret = new Area ();
			ret.NativeRegion = builder.Context.CopyPath ();
			return ret;
		}

		public override void setClip (int x, int y, int width, int height)
		{
			builder.Context.NewPath ();
			builder.Context.Rectangle (x, y, width, height);
			builder.Context.Clip ();
		}

		public override void setClip (Shape clip)
		{
			if (clip == null) {
				builder.Context.NewPath ();
			} else if (clip is Area) {
				builder.Context.NewPath ();
				builder.Context.AppendPath ((Xwt.Drawing.DrawingPath)((Area)clip).NativeRegion);
				builder.Context.Clip ();
			} else if (clip is Rectangle) {
				Rectangle rc = (Rectangle)clip;
				setClip (rc.X, rc.Y, rc.Width, rc.Height);
			}
			else
				throw new NotImplementedException ();
		}

		public override void clipRect (int x, int y, int width, int height)
		{
			setClip (new Rectangle (x, y, width, height));
		}

		public override void drawImage (Cadencii.Gui.Image img, int x, int y, object obs)
		{
			if (img == null) {
				return;
			}
			builder.Context.DrawImage ((Xwt.Drawing.Image) img.NativeImage, new Xwt.Point (x, y));
		}

		public override void translate (int tx, int ty)
		{
			builder.Context.Translate (tx, ty);
		}

		public override void translate (double tx, double ty)
		{
			builder.Context.Translate (tx, ty);
		}

		public override void drawStringEx (string s, Font font, Rectangle rect, int align, int valign)
		{
			// FIXME: align and valign cannot be applied...
			var text = new Xwt.Drawing.TextLayout () {
				Font = (Xwt.Drawing.Font)font.NativeFont,
				Text = s
			};
			builder.Context.Save ();
			builder.Context.Rectangle (rect.ToWF ());
			builder.Context.Clip ();
			builder.Context.DrawTextLayout (text, rect.X, rect.Y);
			builder.Context.Restore ();
		}

		public override Size measureString (string s, Font font)
		{
			return new Xwt.Drawing.TextLayout () { Text = s, Font = font.ToWF () }.GetSize ().ToGui ();
		}
	}

	class StrokeAdapterWF : Stroke.StrokeAdapter
	{
		StrokeXwt pen;

		public override object NativePen { get { return pen; } set { pen = (StrokeXwt) value; } }

		public StrokeAdapterWF ()
		{
			pen = new StrokeXwt () { Color = Cadencii.Gui.Colors.Black.ToNative () };
		}

		public StrokeAdapterWF (float width)
			: this (width, 0, 0, 10.0f)
		{
		}

		public StrokeAdapterWF (float width, int cap, int join)
			: this (width, cap, join, 10.0f)
		{
		}

		public StrokeAdapterWF (float width, int cap, int join, float miterlimit)
			: this ()
		{
			pen.LineWidth = width;
			StrokeXwt.LineCapXwt linecap = StrokeXwt.LineCapXwt.Flat;
			if (cap == 1) {
				linecap = StrokeXwt.LineCapXwt.Round;
			} else if (cap == 2) {
				linecap = StrokeXwt.LineCapXwt.Square;
			}
			pen.LineCap = linecap;
			StrokeXwt.LineJoinXwt linejoin = StrokeXwt.LineJoinXwt.Miter;
			if (join == 1) {
				linejoin = StrokeXwt.LineJoinXwt.Round;
			} else if (join == 2) {
				linejoin = StrokeXwt.LineJoinXwt.Bevel;
			}
			pen.LineJoin = linejoin;
			pen.MiterLimit = miterlimit;
		}

		public StrokeAdapterWF (float width, int cap, int join, float miterlimit, float[] dash, float dash_phase)
			: this (width, cap, join, miterlimit)
		{
			pen.DashPattern = dash;
			pen.DashOffset = dash_phase;
		}

		public void Dispose ()
		{
		}
	}

	class FontAdapterWF : Font.FontAdapter
	{
		Xwt.Drawing.Font font;

		public FontAdapterWF (object value)
		{
			if (value == null)
				throw new ArgumentNullException ("value");
			font = (Xwt.Drawing.Font)value;
		}

		public FontAdapterWF (string name, int style, int size)
		{
			Xwt.Drawing.FontStyle fstyle = Xwt.Drawing.FontStyle.Normal;
			Xwt.Drawing.FontWeight fweight = Xwt.Drawing.FontWeight.Normal;
			if (style >= Font.BOLD) {
				fweight = Xwt.Drawing.FontWeight.Bold;
			}
			if (style >= Font.ITALIC) {
				fstyle = Xwt.Drawing.FontStyle.Italic;
			}
			font = Xwt.Drawing.Font.FromName (name).WithSize (size).WithWeight (fweight).WithStyle (fstyle);
		}

		public override void Dispose ()
		{
		}

		public override object NativeFont {
			get { return font; }
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				font = (Xwt.Drawing.Font) value;
			}
		}

		public override string getName ()
		{
			return font.Family;
		}

		public override int getSize ()
		{
			return (int) font.Size;
		}

		public override float getSize2D ()
		{
			return (float) font.Size;
		}
	}

	class AreaAdapterWF : Area.AreaAdapter
	{
		Xwt.Drawing.DrawingPath region;

		public AreaAdapterWF ()
		{
			region = new Xwt.Drawing.DrawingPath ();
		}

		public AreaAdapterWF (Shape s)
		{
			if (s == null) {
				region = new Xwt.Drawing.DrawingPath ();
			} else if (s is Area) {
				AreaAdapterWF a = (AreaAdapterWF)((Area)s).Adapter;
				if (a.region == null) {
					region = new Xwt.Drawing.DrawingPath ();
				} else {
					region = a.region.CopyPath ();
				}
			} else if (s is Rectangle) {
				Rectangle rc = (Rectangle)s;
				region = new Xwt.Drawing.DrawingPath ();
				region.Rectangle (new Xwt.Rectangle (rc.X, rc.Y, rc.Width, rc.Height));
			}
			else
				throw new NotImplementedException ();
		}

		public override object NativeRegion {
			get { return region; }
			set { region = (Xwt.Drawing.DrawingPath) value; }
		}

		public override void add (Area rhs)
		{
			if (rhs == null) {
				return;
			}
			AreaAdapterWF a = (AreaAdapterWF)rhs.Adapter;
			if (a.region == null) {
				return;
			}
			if (region == null) {
				region = new Xwt.Drawing.DrawingPath ();
			}
			region.AppendPath (a.region);
		}

		public override void subtract (Area rhs)
		{
			// FIXME: no way to subtract
			throw new NotImplementedException ();
			/*
			if (rhs == null) {
				return;
			}
			AreaAdapterWF a = (AreaAdapterWF)rhs.Adapter;
			if (a.region == null) {
				return;
			}
			if (region == null) {
				region = new Xwt.Drawing.DrawingPath ();
			}
			region.Exclude (a.region);
			*/
		}

		public override Area clone ()
		{
			Area ret = new Area ();
			if (region == null) {
				ret.NativeRegion = new Xwt.Drawing.DrawingPath ();
			} else {
				ret.NativeRegion = region.CopyPath ();
			}
			return ret;
		}
	}

	class ScreenAdapterWF : Cadencii.Gui.Toolkit.Screen.ScreenAdapter
	{
		public override Rectangle getScreenBounds (object nativeControl)
		{
			var w = (Xwt.Widget) nativeControl;
			Xwt.Rectangle rc = w.ScreenBounds;
			return new Rectangle ((int) rc.X, (int) rc.Y, (int) rc.Width, (int) rc.Height);
		}

		public override void setMousePosition (Point p)
		{
			// FIXME: cannot do anything
			//var w = (Xwt.Widget) nativeControl;
			//System.Windows.Forms.Cursor.Position = new Xwt.Point (p.X, p.Y);
		}

		public override Point getMousePosition ()
		{
			return Xwt.Desktop.MouseLocation.ToGui ();
		}

		public override bool isPointInScreens (Point p)
		{
			return Xwt.Desktop.GetScreenAtLocation (p.ToWF ()) != null;
		}

		public override Rectangle getWorkingArea (object nativeWindow)
		{
			var w = (Xwt.Widget) nativeWindow;
			return w.ScreenBounds.ToGui ();
		}
	}
}
