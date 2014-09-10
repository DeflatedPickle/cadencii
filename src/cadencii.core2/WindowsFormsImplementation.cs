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
using cadencii.java.awt;
using cadencii.java.awt.geom;
using System.Collections.Generic;
using System.Linq;

namespace cadencii.java.awt
{
	class UIHostWindowsForms : AwtHost
	{
		public UIHostWindowsForms ()
		{
			Types [typeof(Image.ImageAdapter)] = typeof(ImageAdapterWF);
		}
	}

	abstract class ImageAdapterWF : Image.ImageAdapter
	{
		public ImageAdapterWF ()
		{
		}

		public ImageAdapterWF (int width, int height)
		{
			image = new System.Drawing.Bitmap (width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
		}

		System.Drawing.Image image;

		public override object NativeImage {
			get { return image; }
			set { image = (System.Drawing.Image)value; }
		}

		public override void Dispose ()
		{
			if (image != null)
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

	public class GraphicsAdapterWF : Graphics.GraphicsAdapter
	{
		System.Drawing.Graphics nativeGraphics;
		Color color = Color.black;
		Stroke stroke = new Stroke ();
		System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush (System.Drawing.Color.Black);
		System.Drawing.Font m_font = new System.Drawing.Font ("Arial", 10);

		public GraphicsAdapterWF ()
		{
		}

		public GraphicsAdapterWF (Image image)
		{
			NativeGraphics = System.Drawing.Graphics.FromImage ((System.Drawing.Image)image.NativeImage);
		}

		public GraphicsAdapterWF (Graphics other)
		{
			NativeGraphics = (System.Drawing.Graphics)other.NativeGraphics;
		}

		public override object NativeGraphics { get { return nativeGraphics; } set { nativeGraphics = (System.Drawing.Graphics)value; } }

		public override object NativeBrush { get { return brush; } set { brush = (System.Drawing.SolidBrush)value; } }

		public override void Dispose ()
		{
			if (nativeGraphics != null)
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

		public override void fillRect (int x, int y, int width, int height)
		{
			nativeGraphics.FillRectangle (brush, x, y, width, height);
		}

		public override void drawOval (int x, int y, int width, int height)
		{
			nativeGraphics.DrawEllipse ((System.Drawing.Pen)stroke.NativePen, x, y, width, height);
		}

		public override void fillOval (int x, int y, int width, int height)
		{
			nativeGraphics.FillEllipse (brush, x, y, width, height);
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
			m_font = (System.Drawing.Font)font.NativeFont;
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

		public override void fillPolygon (Polygon p)
		{
			fillPolygon (p.xpoints, p.ypoints, p.npoints);
		}

		public override void fillPolygon (int[] xPoints, int[] yPoints, int nPoints)
		{
			System.Drawing.Point[] points = new System.Drawing.Point[nPoints];
			for (int i = 0; i < nPoints; i++) {
				points [i] = new System.Drawing.Point (xPoints [i], yPoints [i]);
			}
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
				((System.Drawing.Graphics)NativeGraphics).FillRectangle (brush, rc.x, rc.y, rc.width, rc.height);
			} else {
				serr.println (
					"fixme; org.kbinani.java.awt.Graphics#fill; type of argument s is not supported for '" +
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
				nativeGraphics.Clip = new System.Drawing.Region (new System.Drawing.Rectangle (rc.x, rc.y, rc.width, rc.height));
			} else {
				serr.println (
					"fixme: org.kbinani.java.awt.Graphics#setClip; argument type of clip is not supported for '" +
					clip.GetType () + "'.");
			}
		}

		public override void clipRect (int x, int y, int width, int height)
		{
			nativeGraphics.Clip = new System.Drawing.Region (new System.Drawing.Rectangle (x, y, width, height));
		}

		public override void drawImage (cadencii.java.awt.Image img, int x, int y, object obs)
		{
			if (img == null) {
				return;
			}
			nativeGraphics.DrawImage ((System.Drawing.Image)img.NativeImage, new System.Drawing.Point (x, y));
		}

		public override void translate (int tx, int ty)
		{
			((System.Drawing.Graphics)NativeGraphics).TranslateTransform (tx, ty);
		}

		public override void translate (double tx, double ty)
		{
			((System.Drawing.Graphics)NativeGraphics).TranslateTransform ((float)tx, (float)ty);
		}
	}

	class StrokeAdapterWF : Stroke.StrokeAdapter
	{
		System.Drawing.Pen pen;

		public override object NativePen { get { return pen; } set { pen = (System.Drawing.Pen)value; } }

		public StrokeAdapterWF ()
		{
			pen = new System.Drawing.Pen (System.Drawing.Color.Black);
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
			pen = new System.Drawing.Pen (System.Drawing.Color.Black, width);
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
	}

	public class FontAdapterWF : Font.FontAdapter
    {

        System.Drawing.Font font;

		public FontAdapterWF(object value)
        {
			font = (System.Drawing.Font) value;
        }

		public FontAdapterWF(string name, int style, int size)
        {
            System.Drawing.FontStyle fstyle = System.Drawing.FontStyle.Regular;
            if (style >= Font.BOLD) {
                fstyle = fstyle | System.Drawing.FontStyle.Bold;
            }
            if (style >= Font.ITALIC) {
                fstyle = fstyle | System.Drawing.FontStyle.Italic;
            }
            font = new System.Drawing.Font(name, size, fstyle);
        }

		public override object NativeFont {
			get;
			set;
		}

		public override void Dispose ()
		{
			if (font != null) font.Dispose ();
		}

		public override string getName()
        {
            return font.Name;
        }

		public override int getSize()
        {
            return (int)font.SizeInPoints;
        }

		public override float getSize2D()
        {
            return font.SizeInPoints;
        }
    }
        

}
