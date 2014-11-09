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
using Cadencii.Gui;
using Cadencii.Gui.geom;
using System.Windows.Forms;
using System.Linq;

namespace Cadencii.Gui
{
	public partial class AwtHostWindowsForms : AwtHost
	{
		public AwtHostWindowsForms ()
		{
			Types [typeof(Image.ImageAdapter)] = typeof(ImageAdapterWF);
			Types [typeof(Graphics.GraphicsAdapter)] = typeof(GraphicsAdapterWF);
		}

		public override void InitializeCursors ()
		{
			Cursors.Default = System.Windows.Forms.Cursors.Default.ToAwt ();
			//Cursors.Hand = System.Windows.Forms.Cursors.Hand.ToAwt ();
			Cursors.VSplit = System.Windows.Forms.Cursors.VSplit.ToAwt ();
			Cursors.NoMoveHoriz = System.Windows.Forms.Cursors.NoMoveHoriz.ToAwt ();

			string _HAND = "AAACAAEAICAAABAAEADoAgAAFgAAACgAAAAgAAAAQAAAAAEABAAAAAAAgAIAAAAAAAAAAAAAAAAAAAAAAAAAA" +
				"AAAgAAAAACAAACAgAAAAACAAIAAgAAAgIAAwMDAAICAgAD/AAAAAP8AAP//AAAAAP8A/wD/AAD//wD///8AAAAAAAAAAAAAAAAAAAA" +
				"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
				"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/wAAAAAAAAAAAAAAAAAAD" +
				"//wAAAAAAAAAAAAAAAAAA//8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP8AAAAA/wAAAAAAAAAAA" +
				"A//AAAAAP/wAAAAAAAAAAAP/wAAAAD/8AAAAAAAAAAAAP8AAAAA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
				"AAAAAAAAA//8AAAAAAAAAAAAAAAAAAP//AAAAAAAAAAAAAAAAAAAP8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
				"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
				"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD////////////////////////////////////////////+f////" +
				"D////gf///4H////D///8/z//+H4f//B+D//wfg//+H4f//z/P///w////4H///+B////w////+f//////////////////////////" +
				"//////////////////w==";
			System.IO.MemoryStream ms = null;
			try {
				ms = new System.IO.MemoryStream(Convert.FromBase64String(_HAND));
				Cursors.Hand = new System.Windows.Forms.Cursor(ms).ToAwt ();
			} catch (Exception ex) {
				Logger.write(GetType () + ".InitializeCursors; ex=" + ex + "\n");
			} finally {
				if (ms != null) {
					try {
						ms.Close();
					} catch (Exception ex2) {
						Logger.write(GetType () + ".InitializeCursors; ex=" + ex2 + "\n");
					}
				}
			}
		}

		public override Cadencii.Gui.Keys DefaultModifierKeys ()
		{
			return (Cadencii.Gui.Keys) Control.ModifierKeys;
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
			if (obj is System.Windows.Forms.Control) {
				return ((System.Windows.Forms.Control)obj).Name;
			} else {
				return "";
			}
		}

		public override void ApplyFontRecurse (UiControl control, Font font)
		{
			ApplyFontRecurseW ((Control) control, font);
		}

		void ApplyFontRecurseW (Control c, Cadencii.Gui.Font font)
		{
			c.Font = (System.Drawing.Font) font.NativeFont;
			foreach (Control cc in c.Controls) {
				ApplyFontRecurseW (cc, font);
			}
		}

		public override int HorizontalScrollBarThumbWidth {
			get { return System.Windows.Forms.SystemInformation.HorizontalScrollBarThumbWidth; }
		}
		public override int VerticalScrollBarThumbHeight {
			get { return System.Windows.Forms.SystemInformation.VerticalScrollBarThumbHeight; }
		}

		public override Font SystemMenuFont {
			get { return System.Windows.Forms.SystemInformation.MenuFont.ToAwt (); }
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
			image = new System.Drawing.Bitmap (width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
		}

		System.Drawing.Image image;
		bool should_dispose;

		public override object NativeImage {
			get { return image; }
			set { image = (System.Drawing.Image)value; }
		}

		public override void Dispose ()
		{
			if (should_dispose && image != null)
				image.Dispose ();
		}

		public override int Width {
			get { return image.Width; }
		}

		public override int Height {
			get { return image.Height; }
		}

		public override void Save (System.IO.Stream stream)
		{
			image.Save (stream, System.Drawing.Imaging.ImageFormat.Png);
		}
	}


	class CursorAdapterWF : Cursor.CursorAdapter
	{
		private int m_type = Cursor.DEFAULT_CURSOR;
		System.Windows.Forms.Cursor cursor = System.Windows.Forms.Cursors.Default;

		public CursorAdapterWF (int type)
		{
			m_type = type;
			if (m_type == Cursor.CROSSHAIR_CURSOR) {
				cursor = System.Windows.Forms.Cursors.Cross;
			} else if (m_type == Cursor.HAND_CURSOR) {
				cursor = System.Windows.Forms.Cursors.Hand;
			} else if (m_type == Cursor.TEXT_CURSOR) {
				cursor = System.Windows.Forms.Cursors.IBeam;
			} else if (m_type == Cursor.E_RESIZE_CURSOR) {
				cursor = System.Windows.Forms.Cursors.PanEast;
			} else if (m_type == Cursor.NE_RESIZE_CURSOR) {
				cursor = System.Windows.Forms.Cursors.PanNE;
			} else if (m_type == Cursor.N_RESIZE_CURSOR) {
				cursor = System.Windows.Forms.Cursors.PanNorth;
			} else if (m_type == Cursor.NW_RESIZE_CURSOR) {
				cursor = System.Windows.Forms.Cursors.PanNW;
			} else if (m_type == Cursor.SE_RESIZE_CURSOR) {
				cursor = System.Windows.Forms.Cursors.PanSE;
			} else if (m_type == Cursor.S_RESIZE_CURSOR) {
				cursor = System.Windows.Forms.Cursors.PanSouth;
			} else if (m_type == Cursor.SW_RESIZE_CURSOR) {
				cursor = System.Windows.Forms.Cursors.PanSW;
			} else if (m_type == Cursor.W_RESIZE_CURSOR) {
				cursor = System.Windows.Forms.Cursors.PanWest;
			} else if (m_type == Cursor.MOVE_CURSOR) {
				cursor = System.Windows.Forms.Cursors.SizeAll;
			}
		}

		public override void Dispose ()
		{
			// do nothing
		}

		public override object NativeCursor {
			get { return cursor; }
			set {
				cursor = (System.Windows.Forms.Cursor)value;
				if (cursor == System.Windows.Forms.Cursors.Cross)
					m_type = Cursor.CROSSHAIR_CURSOR;
				if (cursor == System.Windows.Forms.Cursors.Hand)
					m_type = Cursor.HAND_CURSOR;
				if (cursor == System.Windows.Forms.Cursors.IBeam)
					m_type = Cursor.TEXT_CURSOR;
				if (cursor == System.Windows.Forms.Cursors.PanEast)
					m_type = Cursor.E_RESIZE_CURSOR;
				if (cursor == System.Windows.Forms.Cursors.PanNE)
					m_type = Cursor.NE_RESIZE_CURSOR;
				if (cursor == System.Windows.Forms.Cursors.PanNorth)
					m_type = Cursor.N_RESIZE_CURSOR;
				if (cursor == System.Windows.Forms.Cursors.PanNW)
					m_type = Cursor.NW_RESIZE_CURSOR;
				if (cursor == System.Windows.Forms.Cursors.PanSE)
					m_type = Cursor.SE_RESIZE_CURSOR;
				if (cursor == System.Windows.Forms.Cursors.PanSouth)
					m_type = Cursor.S_RESIZE_CURSOR;
				if (cursor == System.Windows.Forms.Cursors.PanSW)
					m_type = Cursor.SW_RESIZE_CURSOR;
				if (cursor == System.Windows.Forms.Cursors.PanWest)
					m_type = Cursor.W_RESIZE_CURSOR;
				if (cursor == System.Windows.Forms.Cursors.SizeAll)
					m_type = Cursor.MOVE_CURSOR;
			}
		}

		public override int Type {
			get { return m_type; }
		}
	}

	class GraphicsAdapterWF : Graphics.GraphicsAdapter
	{
		System.Drawing.Graphics nativeGraphics;
		Color color = Cadencii.Gui.Colors.Black;
		Stroke stroke = new Stroke ();
		System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush (Cadencii.Gui.Colors.Black.ToNative ());
		System.Drawing.Font m_font = new System.Drawing.Font ("Arial", 10);
		bool should_dispose;

		public GraphicsAdapterWF ()
		{
		}

		public GraphicsAdapterWF (Image image)
		{
			should_dispose = true;
			nativeGraphics = System.Drawing.Graphics.FromImage ((System.Drawing.Image)image.NativeImage);
		}

		public GraphicsAdapterWF (Graphics other)
		{
			var a = (GraphicsAdapterWF) other.Adapter;
			nativeGraphics = a.nativeGraphics;
			m_font = a.m_font;
		}

		public override object NativeGraphics { get { return nativeGraphics; } set { nativeGraphics = (System.Drawing.Graphics)value; } }

		public override object NativeBrush { get { return brush; } set { brush = (System.Drawing.SolidBrush)value; } }

		public override void Dispose ()
		{
			if (should_dispose && nativeGraphics != null)
				nativeGraphics.Dispose ();
		}

		public override void clearRect (int x, int y, int width, int height)
		{
			nativeGraphics.FillRectangle (System.Drawing.Brushes.White, x, y, width, height);
		}

		public override void drawLine (int x1, int y1, int x2, int y2)
		{
			nativeGraphics.DrawLine ((System.Drawing.Pen)stroke.NativePen, x1, y1, x2, y2);
		}

		public override void drawRect (int x, int y, int width, int height)
		{
			nativeGraphics.DrawRectangle ((System.Drawing.Pen)stroke.NativePen, x, y, width, height);
		}

		public override void drawRect (Stroke pen, Rectangle rect)
		{
			nativeGraphics.DrawRectangle ((System.Drawing.Pen) pen.NativePen, rect.X, rect.Y, rect.Width, rect.Height);
		}

		public override void fillRect (int x, int y, int width, int height)
		{
			nativeGraphics.FillRectangle (brush, x, y, width, height);
		}

		public override void fillRect (Color fillcolor, int x, int y, int width, int height)
		{
			nativeGraphics.FillRectangle (new System.Drawing.SolidBrush (fillcolor.ToNative ()), x, y, width, height);
		}

		public override void fillEllipse (Color mDotColor, int i, int i2, int mDotWidth, int mDotWidth2)
		{
			nativeGraphics.FillEllipse (new System.Drawing.SolidBrush (mDotColor.ToNative ()), i, i2, mDotWidth, mDotWidth2);
		}

		public override void drawOval (int x, int y, int width, int height)
		{
			nativeGraphics.DrawEllipse ((System.Drawing.Pen)stroke.NativePen, x, y, width, height);
		}

		public override void fillOval (int x, int y, int width, int height)
		{
			nativeGraphics.FillEllipse (brush, x, y, width, height);
		}

		public override void drawBezier (float x1, float y1,
		                                 float ctrlx1, float ctrly1,
		                                 float ctrlx2, float ctrly2,
		                                 float x2, float y2)
		{
			Stroke stroke = getStroke ();
			System.Drawing.Pen pen = null;
			if (stroke is Stroke) {
				pen = (System.Drawing.Pen)((Stroke)stroke).NativePen;
			} else {
				pen = new System.Drawing.Pen (Cadencii.Gui.Colors.Black.ToNative ());
			}
			((System.Drawing.Graphics)NativeGraphics).DrawBezier (pen, new System.Drawing.PointF (x1, y1),
				new System.Drawing.PointF (ctrlx1, ctrly1),
				new System.Drawing.PointF (ctrlx2, ctrly2),
				new System.Drawing.PointF (x2, y2));
		}

		public override void setColor (Color c)
		{
			color = c;
			((System.Drawing.Pen)stroke.NativePen).Color = c.ToNative ();
			brush.Color = c.ToNative ();
		}

		public override Color getColor ()
		{
			return color;
		}

		public override void setFont (Font font)
		{
			var f = (System.Drawing.Font)font.NativeFont;
			if (f == null)
				throw new ArgumentException ("Native font is null", "font");
			m_font = f;
		}

		public override void setStroke (Stroke stroke)
		{
			if (stroke is Stroke) {
				Stroke bstroke = (Stroke)stroke;
				this.stroke.NativePen = (System.Drawing.Pen)bstroke.NativePen;
				((System.Drawing.Pen)this.stroke.NativePen).Color = getColor ().ToNative ();
			}
		}

		public override Stroke getStroke ()
		{
			return stroke;
		}

		public override void drawString (string str, float x, float y)
		{
			nativeGraphics.DrawString (str, m_font, brush, x, y);
		}

		public override void drawPolygon (Polygon p)
		{
			drawPolygon (p.xpoints, p.ypoints, p.npoints);
		}

		public override void drawPolygon (int[] xPoints, int[] yPoints, int nPoints)
		{
			System.Drawing.Point[] points = new System.Drawing.Point[nPoints];
			for (int i = 0; i < nPoints; i++) {
				points [i] = new System.Drawing.Point (xPoints [i], yPoints [i]);
			}
			nativeGraphics.DrawPolygon ((System.Drawing.Pen)stroke.NativePen, points);
		}

		public override void drawPolyline (int[] xPoints, int[] yPoints, int nPoints)
		{
			System.Drawing.Point[] points = new System.Drawing.Point[nPoints];
			for (int i = 0; i < nPoints; i++) {
				points [i] = new System.Drawing.Point (xPoints [i], yPoints [i]);
			}
			nativeGraphics.DrawLines ((System.Drawing.Pen)stroke.NativePen, points);
		}

		public override void DrawLines (Point[] points)
		{
			nativeGraphics.DrawLines ((System.Drawing.Pen)stroke.NativePen, points.Select (p => new System.Drawing.Point (p.X, p.Y)).ToArray ());
		}

		public override void drawLines (int mLineWidth, Color mLineColor, Point[] mPoints)
		{
			var pen = new System.Drawing.Pen (mLineColor.ToNative ()) { Width = mLineWidth };
			nativeGraphics.DrawLines (pen, mPoints.Select (p => new System.Drawing.Point (p.X, p.Y)).ToArray ());
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
			System.Drawing.Point[] points = new System.Drawing.Point[nPoints];
			for (int i = 0; i < nPoints; i++) {
				points [i] = new System.Drawing.Point (xPoints [i], yPoints [i]);
			}
			if (fillColor != null)
				nativeGraphics.FillPolygon (new System.Drawing.SolidBrush (((Color)fillColor).ToNative ()), points);
			else
				nativeGraphics.FillPolygon (brush, points);
		}

		public override void fill (Shape s)
		{
			if (s == null) {
				return;
			}

			if (s is Area) {
				Area a = (Area)s;
				if (a.NativeRegion != null) {
					((System.Drawing.Graphics)NativeGraphics).FillRegion (brush, (System.Drawing.Region)a.NativeRegion);
				}
			} else if (s is Rectangle) {
				Rectangle rc = (Rectangle)s;
				((System.Drawing.Graphics)NativeGraphics).FillRectangle (brush, rc.X, rc.Y, rc.Width, rc.Height);
			} else {
				Logger.StdErr (
					"fixme; org.kbinani.Cadencii.Gui.Graphics#fill; type of argument s is not supported for '" +
					s.GetType () + "'.");
			}
		}

		public override Shape getClip ()
		{
			Area ret = new Area ();
			ret.NativeRegion = nativeGraphics.Clip;
			return ret;
		}

		public override void setClip (int x, int y, int width, int height)
		{
			nativeGraphics.SetClip (new System.Drawing.Rectangle (x, y, width, height));
		}

		public override void setClip (Shape clip)
		{
			if (clip == null) {
				nativeGraphics.Clip = new System.Drawing.Region ();
			} else if (clip is Area) {
				nativeGraphics.Clip = (System.Drawing.Region)((Area)clip).NativeRegion;
			} else if (clip is Rectangle) {
				Rectangle rc = (Rectangle)clip;
				nativeGraphics.Clip = new System.Drawing.Region (new System.Drawing.Rectangle (rc.X, rc.Y, rc.Width, rc.Height));
			} else {
				Logger.StdErr (
					"fixme: org.kbinani.Cadencii.Gui.Graphics#setClip; argument type of clip is not supported for '" +
					clip.GetType () + "'.");
			}
		}

		public override void clipRect (int x, int y, int width, int height)
		{
			nativeGraphics.Clip = new System.Drawing.Region (new System.Drawing.Rectangle (x, y, width, height));
		}

		public override void drawImage (Cadencii.Gui.Image img, int x, int y, object obs)
		{
			if (img == null) {
				return;
			}
			nativeGraphics.DrawImage ((System.Drawing.Image)img.NativeImage, new System.Drawing.Point (x, y));
		}

		public override void translate (int tx, int ty)
		{
			nativeGraphics.TranslateTransform (tx, ty);
		}

		public override void translate (double tx, double ty)
		{
			nativeGraphics.TranslateTransform ((float)tx, (float)ty);
		}

		System.Drawing.StringFormat mStringFormat = new System.Drawing.StringFormat ();

		public override void drawStringEx (string s, Font font, Rectangle rect, int align, int valign)
		{
			if (align > 0) {
				mStringFormat.Alignment = System.Drawing.StringAlignment.Far;
			} else if (align < 0) {
				mStringFormat.Alignment = System.Drawing.StringAlignment.Near;
			} else {
				mStringFormat.Alignment = System.Drawing.StringAlignment.Center;
			}
			if (valign > 0) {
				mStringFormat.LineAlignment = System.Drawing.StringAlignment.Far;
			} else if (valign < 0) {
				mStringFormat.LineAlignment = System.Drawing.StringAlignment.Near;
			} else {
				mStringFormat.LineAlignment = System.Drawing.StringAlignment.Center;
			}
			nativeGraphics.DrawString (s, (System.Drawing.Font)font.NativeFont, brush, new System.Drawing.RectangleF (rect.X, rect.Y, rect.Width, rect.Height), mStringFormat);
		}

		public override Dimension measureString (string s, Font font)
		{
			return nativeGraphics.MeasureString (s, font.ToWF ()).ToSize ().ToAwt ();
		}
	}

	class StrokeAdapterWF : Stroke.StrokeAdapter
	{
		System.Drawing.Pen pen;
		bool should_dispose;

		public override object NativePen { get { return pen; } set { pen = (System.Drawing.Pen)value; } }

		public StrokeAdapterWF ()
		{
			should_dispose = true;
			pen = new System.Drawing.Pen (Cadencii.Gui.Colors.Black.ToNative ());
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
		{
			pen = new System.Drawing.Pen (Cadencii.Gui.Colors.Black.ToNative (), width);
			System.Drawing.Drawing2D.LineCap linecap = System.Drawing.Drawing2D.LineCap.Flat;
			if (cap == 1) {
				linecap = System.Drawing.Drawing2D.LineCap.Round;
			} else if (cap == 2) {
				linecap = System.Drawing.Drawing2D.LineCap.Square;
			}
			pen.StartCap = linecap;
			pen.EndCap = linecap;
			System.Drawing.Drawing2D.LineJoin linejoin = System.Drawing.Drawing2D.LineJoin.Miter;
			if (join == 1) {
				linejoin = System.Drawing.Drawing2D.LineJoin.Round;
			} else if (join == 2) {
				linejoin = System.Drawing.Drawing2D.LineJoin.Bevel;
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
			if (should_dispose && pen != null)
				pen.Dispose ();
		}
	}

	class FontAdapterWF : Font.FontAdapter
	{

		System.Drawing.Font font;
		bool should_dispose;

		public FontAdapterWF (object value)
		{
			if (value == null)
				throw new ArgumentNullException ("value");
			font = (System.Drawing.Font)value;
		}

		public FontAdapterWF (string name, int style, int size)
		{
			System.Drawing.FontStyle fstyle = System.Drawing.FontStyle.Regular;
			if (style >= Font.BOLD) {
				fstyle = fstyle | System.Drawing.FontStyle.Bold;
			}
			if (style >= Font.ITALIC) {
				fstyle = fstyle | System.Drawing.FontStyle.Italic;
			}
			should_dispose = true;
			font = new System.Drawing.Font (name, size, fstyle);
		}

		public override object NativeFont {
			get { return font; }
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				font = (System.Drawing.Font) value;
			}
		}

		public override void Dispose ()
		{
			if (should_dispose && font != null)
				font.Dispose ();
		}

		public override string getName ()
		{
			return font.Name;
		}

		public override int getSize ()
		{
			return (int)font.SizeInPoints;
		}

		public override float getSize2D ()
		{
			return font.SizeInPoints;
		}
	}

	class AreaAdapterWF : Area.AreaAdapter
	{
		System.Drawing.Region region;

		public AreaAdapterWF ()
		{
			region = new System.Drawing.Region ();
			region.MakeEmpty ();
		}

		public AreaAdapterWF (Shape s)
		{
			if (s == null) {
				region = new System.Drawing.Region ();
			} else if (s is Area) {
				AreaAdapterWF a = (AreaAdapterWF)((Area)s).Adapter;
				if (a.region == null) {
					region = new System.Drawing.Region ();
				} else {
					region = (System.Drawing.Region)a.region.Clone ();
				}
			} else if (s is Rectangle) {
				Rectangle rc = (Rectangle)s;
				region = new System.Drawing.Region (new System.Drawing.Rectangle (rc.X, rc.Y, rc.Width, rc.Height));
			} else {
				Logger.StdErr (
					"fixme: org.kbinani.Cadencii.Gui.Area#.ctor(org.kbinani.Cadencii.Gui.Shape); type of argument s is not supported for '" +
					s.GetType () + "'.");
				region = new System.Drawing.Region ();
			}
		}

		public override object NativeRegion {
			get { return region; }
			set { region = (System.Drawing.Region)value; }
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
				region = new System.Drawing.Region ();
			}
			region.Union (a.region);
		}

		public override void subtract (Area rhs)
		{
			if (rhs == null) {
				return;
			}
			AreaAdapterWF a = (AreaAdapterWF)rhs.Adapter;
			if (a.region == null) {
				return;
			}
			if (region == null) {
				region = new System.Drawing.Region ();
			}
			region.Exclude (a.region);
		}

		public override Area clone ()
		{
			Area ret = new Area ();
			if (region == null) {
				ret.NativeRegion = new System.Drawing.Region ();
			} else {
				ret.NativeRegion = (System.Drawing.Region)region.Clone ();
			}
			return ret;
		}
	}

	class ScreenAdapterWF : Screen.ScreenAdapter
	{
		public override Rectangle getScreenBounds (object nativeControl)
		{
			var w = (System.Windows.Forms.Control)nativeControl;
			System.Drawing.Rectangle rc = System.Windows.Forms.Screen.GetWorkingArea (w);
			return new Rectangle (rc.X, rc.Y, rc.Width, rc.Height);
		}

		
		public override void setMousePosition (Point p)
		{
			System.Windows.Forms.Cursor.Position = new System.Drawing.Point (p.X, p.Y);
		}

		public override Point getMousePosition ()
		{
			System.Drawing.Point p = System.Windows.Forms.Control.MousePosition;
			return new Point (p.X, p.Y);
		}

		/// <summary>
		/// 指定した点が，コンピュータの画面のいずれかに含まれているかどうかを調べます
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public override bool isPointInScreens (Point p)
		{
			foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens) {
				System.Drawing.Rectangle rc = screen.WorkingArea;
				if (rc.X <= p.X && p.X <= rc.X + rc.Width) {
					if (rc.Y <= p.Y && p.Y <= rc.Y + rc.Height) {
						return true;
					}
				}
			}
			return false;
		}

		public override Rectangle getWorkingArea (object nativeWindow)
		{
			var w = (System.Windows.Forms.Form)nativeWindow;
			System.Drawing.Rectangle r = System.Windows.Forms.Screen.GetWorkingArea (w);
			return new Rectangle (r.X, r.Y, r.Width, r.Height);
		}
	}
}
