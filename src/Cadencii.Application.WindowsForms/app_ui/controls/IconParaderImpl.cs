/*
 * IconParader.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Linq;
using Cadencii.Gui;
using cadencii.windows.forms;



namespace cadencii
{

    /// <summary>
    /// 起動時のスプラッシュウィンドウに表示されるアイコンパレードの、1個のアイコンを表現します
    /// </summary>
    public class IconParaderImpl : PictureBoxImpl, IconParader
    {
        const int RADIUS = 6; // 角の丸み
        const int DIAMETER = 2 * RADIUS;
	const int ICON_WIDTH = IconParaderController.ICON_WIDTH;
	const int ICON_HEIGHT = IconParaderController.ICON_HEIGHT;

        private System.Drawing.Drawing2D.GraphicsPath graphicsPath = null;
        private System.Drawing.Region region = null;
        private System.Drawing.Region invRegion = null;
        private System.Drawing.SolidBrush brush = null;

        public IconParaderImpl()
        {
            var d = new System.Drawing.Size(ICON_WIDTH, ICON_HEIGHT);
            this.Size = d;
            this.MaximumSize = d;
            this.MinimumSize = d;
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        }

        public void setImage(Image img)
        {
            Image bmp = new Image(ICON_WIDTH, ICON_HEIGHT);
            Graphics g = null;
            try {
                g = new Graphics(bmp);
                ((System.Drawing.Graphics) g.NativeGraphics).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                if (img != null) {
                    int img_width = img.Width;
                    int img_height = img.Height;
                    double a = img_height / (double)img_width;
                    double aspecto = ICON_HEIGHT / (double)ICON_WIDTH;

                    int x = 0;
                    int y = 0;
                    int w = ICON_WIDTH;
                    int h = ICON_HEIGHT;
                    if (a >= aspecto) {
                        // アイコンより縦長
                        double act_width = ICON_WIDTH / a;
                        x = (int)((ICON_WIDTH - act_width) / 2.0);
                        w = (int)act_width;
                    } else {
                        // アイコンより横長
                        double act_height = ICON_HEIGHT * a;
                        y = (int)((ICON_HEIGHT - act_height) / 2.0);
                        h = (int)act_height;
                    }
                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(x, y, w, h);
                    System.Drawing.Rectangle srcRect = new System.Drawing.Rectangle(0, 0, img_width, img_height);
                    ((System.Drawing.Graphics) g.NativeGraphics).DrawImage((System.Drawing.Image) img.NativeImage, destRect, srcRect, System.Drawing.GraphicsUnit.Pixel);
                }
				((System.Drawing.Graphics) g.NativeGraphics).FillRegion(getBrush(), getInvRegion());
				((System.Drawing.Graphics) g.NativeGraphics).DrawPath(System.Drawing.Pens.DarkGray, getGraphicsPath());
            } catch (Exception ex) {
                Logger.write(typeof(IconParader) + ".setImage; ex=" + ex + "\n");
            } finally {
                if (g != null) {
			g.Dispose();
                }
            }
            base.Image = (System.Drawing.Image) bmp.NativeImage;
        }

        /// <summary>
        /// アイコンの4隅を塗りつぶすためのブラシを取得します
        /// </summary>
        /// <returns></returns>
        private System.Drawing.SolidBrush getBrush()
        {
            if (brush == null) {
                brush = new System.Drawing.SolidBrush(base.BackColor);
            } else {
                if (brush.Color != base.BackColor) {
                    brush.Color = base.BackColor;
                }
            }
            return brush;
        }

        /// <summary>
        /// 角の丸い枠線を表すGraphicsPathを取得します
        /// </summary>
        /// <returns></returns>
        private System.Drawing.Drawing2D.GraphicsPath getGraphicsPath()
        {
            if (graphicsPath == null) {
                graphicsPath = new System.Drawing.Drawing2D.GraphicsPath();
                graphicsPath.StartFigure();
                int w = base.Width - 1;
                int h = base.Height - 1;
                // 上の横線
                graphicsPath.AddLine(RADIUS, 0, w - RADIUS, 0);
                // 右上の角
                graphicsPath.AddArc(w - DIAMETER, 0, DIAMETER, DIAMETER, 270, 90);
                // 右の縦線
                graphicsPath.AddLine(w, RADIUS, w, h - RADIUS);
                // 右下の角
                graphicsPath.AddArc(w - DIAMETER, h - DIAMETER, DIAMETER, DIAMETER, 0, 90);
                // 下の横線
                graphicsPath.AddLine(w - RADIUS, h, RADIUS, h);
                // 左下の角
                graphicsPath.AddArc(0, h - DIAMETER, DIAMETER, DIAMETER, 90, 90);
                // 左の縦線
                graphicsPath.AddLine(0, h - RADIUS, 0, RADIUS);
                // 左上の角
                graphicsPath.AddArc(0, 0, DIAMETER, DIAMETER, 180, 90);
                graphicsPath.CloseFigure();
            }
            return graphicsPath;
        }

        /// <summary>
        /// 角の丸いアイコンの画像領域を取得します
        /// </summary>
        /// <returns></returns>
        private System.Drawing.Region getRegion()
        {
            if (region == null) {
                region = new System.Drawing.Region(getGraphicsPath());
            }
            return region;
        }

        /// <summary>
        /// アイコンの画像領域以外の領域(4隅)を取得します
        /// </summary>
        /// <returns></returns>
        private System.Drawing.Region getInvRegion()
        {
            if (invRegion == null) {
                invRegion = new System.Drawing.Region();
                invRegion.Exclude(getGraphicsPath());
            }
            return invRegion;
        }
    }

}
