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

namespace cadencii.java.awt
{
	public class Graphics : IDisposable
	{
		GraphicsAdapter a;

		public Graphics ()
		{
			a = AwtHost.Current.New<GraphicsAdapter> ();
		}

		public Graphics (Image image)
		{
			a = AwtHost.Current.New<GraphicsAdapter> (image);
		}

		public Graphics (Graphics other)
		{
			a = AwtHost.Current.New<GraphicsAdapter> (other);
		}

		public GraphicsAdapter Adapter { get { return a; } }

		public object NativeGraphics { get { return a.NativeGraphics; } set { a.NativeGraphics = value; } }

		public object NativeBrush { get { return a.NativeBrush; } set { a.NativeBrush = value; } }

	        public void Dispose ()
	        {
	        	a.Dispose ();
	        }

	        public void clearRect(int x, int y, int width, int height)
	        {
	        	a.clearRect (x, y, width, height);
	        }

		public void drawLine (int x1, int y1, int x2, int y2)
		{
			a.drawLine (x1, y1, x2, y2);
		}

		public void drawRect(int x, int y, int width, int height)
		{
			a.drawRect (x, y, width, height);
		}

		public void fillRect(int x, int y, int width, int height)
		{
			a.fillRect (x, y, width, height);
		}

		public void fillEllipse (Color mDotColor, int i, int i2, int mDotWidth, int mDotWidth2)
		{
			a.fillEllipse (mDotColor, i, i2, mDotWidth, mDotWidth2);
		}

		public void drawOval(int x, int y, int width, int height)
		{
			a.drawOval (x, y, width, height);
		}

		public void fillOval(int x, int y, int width, int height)
		{
			a.fillOval (x, y, width, height);
		}

		public void setColor(Color c)
		{
			a.setColor (c);
		}

		public Color getColor()
		{
			return a.getColor ();
		}

		public void setFont(Font font)
		{
			a.setFont (font);
		}

		public void drawString(string str, float x, float y)
		{
			a.drawString (str, x, y);
		}

		public void drawPolygon(Polygon p)
		{
			a.drawPolygon (p);
		}

		public void drawPolygon(int[] xPoints, int[] yPoints, int nPoints)
		{
			a.drawPolygon (xPoints, yPoints, nPoints);
		}

		public void drawPolyline(int[] xPoints, int[] yPoints, int nPoints)
		{
			a.drawPolyline (xPoints, yPoints, nPoints);
		}

		public void drawBezier (float x1, float y1,
		                              float ctrlx1, float ctrly1,
		                              float ctrlx2, float ctrly2,
		                              float x2, float y2)
		{
			a.drawBezier (x1, y1, ctrlx1, ctrly1, ctrlx2, ctrly2, x2, y2);
		}
	
		public void fillPolygon(Polygon p)
		{
			a.fillPolygon (p);
		}

		public void fillPolygon(Color fillColor, Polygon p)
		{
			a.fillPolygon (fillColor, p);
		}

		public void fillPolygon(int[] xPoints, int[] yPoints, int nPoints)
		{
			a.fillPolygon (xPoints, yPoints, nPoints);
		}

		public void fill(Shape s)
		{
			a.fill (s);
		}

		public void fillRectangle (Color mDotColor, int i, int i2, int mDotWidth, int mDotWidth2)
		{
			a.fillRect (mDotColor, i, i2, mDotWidth, mDotWidth2);
		}

		public Shape getClip()
		{
			return a.getClip ();
		}

		public void setClip(int x, int y, int width, int height)
		{
			a.setClip (x, y, width, height);
		}

		public void setClip(Shape clip)
		{
			a.setClip (clip);
		}

		public void clipRect(int x, int y, int width, int height)
		{
			a.clipRect (x, y, width, height);
		}

		public void setStroke(Stroke stroke)
		{
			a.setStroke (stroke);
		}

		public Stroke getStroke()
		{
			return a.getStroke ();
		}

		public void translate(int tx, int ty)
		{
			a.translate (tx, ty);
		}

		public void translate(double tx, double ty)
		{
			a.translate (tx, ty);
		}

		public void drawImage(cadencii.java.awt.Image img, int x, int y, object obs)
		{
			a.drawImage (img, x, y, obs);
		}

		public void DrawLines (Point[] points)
		{
			a.DrawLines (points);
		}

		public void drawLines (int mLineWidth, Color mLineColor, Point[] mPoints)
		{
			a.drawLines (mLineWidth, mLineColor, mPoints);
		}

		public void drawStringEx (string s, Font font, Rectangle rect, int align, int valign)
		{
			a.drawStringEx (s, font, rect, align, valign);
		}

		public const int STRING_ALIGN_FAR = 1;
		public const int STRING_ALIGN_NEAR = -1;
		public const int STRING_ALIGN_CENTER = 0;

		public abstract class GraphicsAdapter : IDisposable
		{
			public abstract object NativeGraphics { get; set; }

			public abstract object NativeBrush { get; set; }

			public abstract void Dispose ();

			public abstract void clearRect(int x, int y, int width, int height);

			public abstract void drawLine(int x1, int y1, int x2, int y2);

			public abstract void drawRect(int x, int y, int width, int height);

			public abstract void drawBezier(float x1, float y1, float ctrlx1, float ctrly1, float ctrlx2, float ctrly2, float x2, float y2);

			public abstract void fillRect(int x, int y, int width, int height);

			public abstract void fillRect(Color fillColor, int x, int y, int width, int height);

			public abstract void fillEllipse (Color mDotColor, int i, int i2, int mDotWidth, int mDotWidth2);

			public abstract void drawOval(int x, int y, int width, int height);

			public abstract void fillOval(int x, int y, int width, int height);

			public abstract void fill(Shape s);

			public abstract void setColor(Color c);

			public abstract Color getColor();

			public abstract void setFont(Font font);

			public abstract void setStroke (Stroke stroke);

			public abstract Stroke getStroke ();

			public abstract void drawString(string str, float x, float y);

			public abstract void drawPolygon(Polygon p);

			public abstract void drawPolygon(int[] xPoints, int[] yPoints, int nPoints);

			public abstract void drawPolyline(int[] xPoints, int[] yPoints, int nPoints);

			public abstract void fillPolygon(Polygon p);

			public abstract void fillPolygon(Color fillColor, Polygon p);

			public abstract void fillPolygon(int[] xPoints, int[] yPoints, int nPoints);

			public abstract Shape getClip();

			public abstract void setClip(int x, int y, int width, int height);

			public abstract void setClip(Shape clip);

			public abstract void clipRect(int x, int y, int width, int height);

			public abstract void drawImage(cadencii.java.awt.Image img, int x, int y, object obs);
			
			public abstract void DrawLines (Point[] points);

			public abstract void drawLines (int mLineWidth, Color mLineColor, Point[] mPoints);

			public abstract void translate(int tx, int ty);

			public abstract void translate(double tx, double ty);

			public abstract void drawStringEx(string s, Font font, Rectangle rect, int align, int valign);
		}
	}

}
