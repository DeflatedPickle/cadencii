using System;
using cadencii.java.awt;

namespace cadencii.java.awt
{
	public static class GraphicsUtil
	{
		#region Graphics extension
        public static void drawBezier(Graphics g, float x1, float y1,
                           float ctrlx1, float ctrly1,
                           float ctrlx2, float ctrly2,
                           float x2, float y2)
        {
            Stroke stroke = g.getStroke();
            System.Drawing.Pen pen = null;
            if (stroke is Stroke) {
				pen = (System.Drawing.Pen) ((Stroke)stroke).NativePen;
            } else {
                pen = new System.Drawing.Pen(System.Drawing.Color.Black);
            }
            ((System.Drawing.Graphics) g.NativeGraphics).DrawBezier(pen, new System.Drawing.PointF(x1, y1),
                                              new System.Drawing.PointF(ctrlx1, ctrly1),
                                              new System.Drawing.PointF(ctrlx2, ctrly2),
                                              new System.Drawing.PointF(x2, y2));
        }

        public const int STRING_ALIGN_FAR = 1;
        public const int STRING_ALIGN_NEAR = -1;
        public const int STRING_ALIGN_CENTER = 0;
        private static System.Drawing.StringFormat mStringFormat = new System.Drawing.StringFormat();
        public static void drawStringEx(Graphics g1, string s, Font font, Rectangle rect, int align, int valign)
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
			((System.Drawing.Graphics) g1.NativeGraphics).DrawString(s, (System.Drawing.Font) font.NativeFont, (System.Drawing.Brush) g1.NativeBrush, new System.Drawing.RectangleF(rect.x, rect.y, rect.width, rect.height), mStringFormat);
        }
        #endregion
		
	}
}

