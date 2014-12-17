/*
 * WaveformZoomController.cs
 * Copyright © 2011 kbinani
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
using Cadencii.Gui;
using cadencii.core;
using Cadencii.Application.Forms;
using Cadencii.Application.Controls;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Models
{

    public class WaveformZoomModel
    {
        /// <summary>
        /// 波形表示部の拡大ボタン上でマウスが下りた状態かどうか
        /// </summary>
        private bool mWaveViewButtonZoomMouseDowned = false;
        /// <summary>
        /// 波形表示部のAutoMaximizeボタン上でマウスが下りた状態かどうか
        /// </summary>
        private bool mWaveViewButtonAutoMaximizeMouseDowned = false;
        /// <summary>
        /// 波形表示部の縦軸の拡大率を自動最大化するかどうか
        /// </summary>
        private bool mWaveViewAutoMaximize = false;
        /// <summary>
        /// 波形表示部分のズーム時に，マウスが下りた瞬間のY座標
        /// </summary>
        private int mWaveViewMouseDownedLocationY;
        /// <summary>
        /// 波形表示部の拡大ボタン上でマウスが下りた瞬間の，波形表示部の縦軸拡大率．
        /// </summary>
        private float mWaveViewInitScale;

        private WaveformZoom control = null;

        /// <summary>
        /// Wave表示部等のボタンと他のコンポーネントの間のスペース
        /// </summary>
        const int SPACE = 4;

		public WaveformZoomModel(WaveformZoom control)
        {
			this.control = control;

			control.MouseMove += (sender, e) => receiveMouseMoveSignal (e.X, e.Y);
			control.MouseDown += (sender, e) => receiveMouseDownSignal (e.X, e.Y);
			control.MouseUp += (sender, e) => receiveMouseUpSignal (e.X, e.Y);
			control.Paint += (o, e) => receivePaintSignal (e.Graphics);
        }

		WaveView WaveView {
			get { return control.WaveView; }
		}

		FormMain Main {
			get {
				for (UiControl c = control.Parent; c != null; c = c.Parent)
					if (c is FormMain)
						return c as FormMain;
				return null;
			}
		}

        public void refreshScreen()
        {
            Main.refreshScreen();
        }

        public void receivePaintSignal(Graphics g)
        {
            int key_width = EditorManager.keyWidth;
            int width = key_width - 1;
			int height = control.Height - 1;

            // 背景を塗る
			g.setColor(Cadencii.Gui.Colors.DarkGray);
            g.fillRect(0, 0, width, height);

            // AutoMaximizeのチェックボックスを描く
			g.setColor(mWaveViewButtonAutoMaximizeMouseDowned ? Cadencii.Gui.Colors.Gray : Cadencii.Gui.Colors.LightGray);
            g.fillRect(SPACE, SPACE, 16, 16);
			g.setColor(Cadencii.Gui.Colors.Gray);
            g.drawRect(SPACE, SPACE, 16, 16);
            if (mWaveViewAutoMaximize) {
				g.setColor(Cadencii.Gui.Colors.Gray);
                g.fillRect(SPACE + 3, SPACE + 3, 11, 11);
            }
            g.setColor(Cadencii.Gui.Colors.Black);
            g.setFont(EditorConfig.baseFont8);
            g.drawString(
                "Auto Maximize",
                SPACE + 16 + SPACE,
                SPACE + AppConfig.baseFont8Height / 2 - AppConfig.baseFont8OffsetHeight + 1);

            // ズーム用ボタンを描く
            int zoom_button_y = SPACE + 16 + SPACE;
            int zoom_button_height = height - SPACE - zoom_button_y;
            Rectangle rc = getButtonBoundsWaveViewZoom();
            if (!mWaveViewAutoMaximize) {
				g.setColor(mWaveViewButtonZoomMouseDowned ? Cadencii.Gui.Colors.Gray : Cadencii.Gui.Colors.LightGray);
                g.fillRect(rc.X, rc.Y, rc.Width, rc.Height);
            }
			g.setColor(Cadencii.Gui.Colors.Gray);
            g.drawRect(rc.X, rc.Y, rc.Width, rc.Height);
			g.setColor(mWaveViewAutoMaximize ? Cadencii.Gui.Colors.Gray : Cadencii.Gui.Colors.Black);
            rc.Y = rc.Y + 1;
			g.drawStringEx(
				(mWaveViewButtonZoomMouseDowned ? "↑Move Mouse↓" : "Zoom"), EditorConfig.baseFont9,
				rc, Graphics.STRING_ALIGN_CENTER, Graphics.STRING_ALIGN_CENTER);
        }

        public void receiveMouseDownSignal(int x, int y)
        {
            Point p = new Point(x, y);

            int width = EditorManager.keyWidth - 1;
			int height = control.Height;

            // AutoMaximizeボタン
            Rectangle rc = new Rectangle(SPACE, SPACE, width - SPACE - SPACE, 16);
            if (Utility.isInRect(p, rc)) {
                mWaveViewButtonAutoMaximizeMouseDowned = true;
                mWaveViewButtonZoomMouseDowned = false;

                control.Refresh();
                return;
            }

            if (!mWaveViewAutoMaximize) {
                // Zoomボタン
                rc = getButtonBoundsWaveViewZoom();
                if (Utility.isInRect(p, rc)) {
                    mWaveViewMouseDownedLocationY = p.Y;
                    mWaveViewButtonZoomMouseDowned = true;
                    mWaveViewButtonAutoMaximizeMouseDowned = false;
					mWaveViewInitScale = WaveView.Model.Scale;

                    control.Refresh();
                    return;
                }
            }

            mWaveViewButtonAutoMaximizeMouseDowned = false;
            mWaveViewButtonZoomMouseDowned = false;
            control.Refresh();
        }

        public void receiveMouseMoveSignal(int x, int y)
        {
            if (!mWaveViewButtonZoomMouseDowned) {
                return;
            }

			int height = control.Height;
            int delta = mWaveViewMouseDownedLocationY - y;
            float scale = mWaveViewInitScale + delta * 3.0f / height * mWaveViewInitScale;
			WaveView.Model.Scale = scale;

            Main.refreshScreen();
        }

        public void receiveMouseUpSignal(int x, int y)
        {
            int width = EditorManager.keyWidth - 1;
			int height = control.Height;

            // AutoMaximizeボタン
            if (Utility.isInRect(x, y, SPACE, SPACE, width - SPACE - SPACE, 16)) {
                if (mWaveViewButtonAutoMaximizeMouseDowned) {
                    mWaveViewAutoMaximize = !mWaveViewAutoMaximize;
					WaveView.Model.setAutoMaximize(mWaveViewAutoMaximize);
                }
            }

            mWaveViewButtonAutoMaximizeMouseDowned = false;
            mWaveViewButtonZoomMouseDowned = false;
            control.Refresh();
        }

        /// <summary>
        /// 波形表示部のズームボタンの形を取得します
        /// </summary>
        /// <returns></returns>
        private Rectangle getButtonBoundsWaveViewZoom()
        {
            int width = EditorManager.keyWidth - 1;
            int height = control.Height - 1;

            int y = SPACE + 16 + SPACE;
            return new Rectangle(SPACE, y, width - SPACE - SPACE, height - SPACE - y);
        }
    }

}
