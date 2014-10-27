/*
 * WaveView.cs
 * Copyright © 2009-2011 kbinani
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
using System.Windows.Forms;
using cadencii;
using cadencii.java.awt;
using cadencii.media;
using cadencii.windows.forms;
 using ApplicationGlobal = cadencii.core.ApplicationGlobal;


namespace cadencii
{

    /// <summary>
    /// トラック16個分の波形描画コンテキストを保持し、それらの描画を行うコンポーネントです。
    /// </summary>
    public class WaveViewImpl : UserControlImpl, WaveView
    {
        /// <summary>
        /// 波形描画用のコンテキスト
        /// </summary>
        private WaveDrawContext[] mDrawer = new WaveDrawContext[ApplicationGlobal.MAX_NUM_TRACK];
        /// <summary>
        /// グラフィクスオブジェクトのキャッシュ
        /// </summary>
        private Graphics mGraphics = null;
        /// <summary>
        /// 縦軸方向のスケール
        /// </summary>
        private float mScale = MIN_SCALE;
        private const float MAX_SCALE = 10.0f;
        private const float MIN_SCALE = 1.0f;
        /// <summary>
        /// 左側のボタン部との境界線の色
        /// </summary>
        private Color mBorderColor = new Color(105, 105, 105);
        /// <summary>
        /// 縦軸のスケールを自動最大化するかどうか
        /// </summary>
        private bool mAutoMaximize = false;
        /// <summary>
        /// 幅2ピクセルのストローク
        /// </summary>
        private Stroke mStroke2px = null;
        /// <summary>
        /// デフォルトのストローク
        /// </summary>
        private Stroke mStrokeDefault = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WaveViewImpl()
            :
            base()
        {
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// 縦軸を自動最大化するかどうかを取得します
        /// </summary>
        /// <returns></returns>
        public bool isAutoMaximize()
        {
            return mAutoMaximize;
        }

        /// <summary>
        /// 縦軸を自動最大化するかどうかを設定します
        /// </summary>
        /// <param name="value"></param>
        public void setAutoMaximize(bool value)
        {
            mAutoMaximize = value;
        }

        /// <summary>
        /// コンポーネントの描画関数です
        /// </summary>
        /// <param name="g"></param>
        public void paint(Graphics g1)
        {
            int width = Width;
            int height = Height;
            Rectangle rc = new Rectangle(0, 0, width, height);

            Graphics g = (Graphics)g1;

            // 背景を塗りつぶす
            g.setStroke(getStrokeDefault());
            g.setColor(cadencii.java.awt.Colors.Gray);
            g.fillRect(rc.X, rc.Y, rc.Width, rc.Height);

            if (EditorManager.skipDrawingWaveformWhenPlaying && EditorManager.isPlaying()) {
                // 左側のボタン部との境界線
                g.setColor(mBorderColor);
                g.drawLine(0, 0, 0, height);

                g.setColor(cadencii.java.awt.Colors.Black);
				g.drawStringEx(
                    "(hidden for performance)",
					cadencii.core.EditorConfig.baseFont8,
                    rc,
					Graphics.STRING_ALIGN_CENTER,
					Graphics.STRING_ALIGN_CENTER);
                return;
            }

            // スケール線を描く
            int half_height = height / 2;
            g.setColor(cadencii.java.awt.Colors.Black);
            g.drawLine(0, half_height, width, half_height);

            // 描画コンテキストを用いて波形を描画
            int selected = EditorManager.Selected;
            WaveDrawContext context = mDrawer[selected - 1];

            if (context != null) {
                if (mAutoMaximize) {
                    context.draw(
                        g,
                        cadencii.java.awt.Colors.Black,
                        rc,
                        EditorManager.clockFromXCoord(EditorManager.keyWidth),
                        EditorManager.clockFromXCoord(EditorManager.keyWidth + width),
                        MusicManager.getVsqFile().TempoTable,
                        EditorManager.MainWindowController.getScaleX());
                } else {
                    context.draw(
                        g,
                        cadencii.java.awt.Colors.Black,
                        rc,
                        EditorManager.clockFromXCoord(EditorManager.keyWidth),
                        EditorManager.clockFromXCoord(EditorManager.keyWidth + width),
                        MusicManager.getVsqFile().TempoTable,
                        EditorManager.MainWindowController.getScaleX(),
                        mScale);
                }
            }

            // 左側のボタン部との境界線
            g.setColor(mBorderColor);
            g.drawLine(0, 0, 0, height);

            // ソングポジション
            int song_pos_x = EditorManager.xCoordFromClocks(EditorManager.getCurrentClock()) - EditorManager.keyWidth;
            if (0 < song_pos_x) {
				g.setColor(cadencii.java.awt.Colors.White);
                g.setStroke(getStroke2px());
                g.drawLine(song_pos_x, 0, song_pos_x, height);
            }
        }

        private Stroke getStrokeDefault()
        {
            if (mStrokeDefault == null) {
                mStrokeDefault = new Stroke();
            }
            return mStrokeDefault;
        }

        private Stroke getStroke2px()
        {
            if (mStroke2px == null) {
                mStroke2px = new Stroke(2.0f);
            }
            return mStroke2px;
        }

        /// <summary>
        /// 縦方向の描画倍率を取得します。
        /// </summary>
        /// <seealso cref="getScale"/>
        /// <returns></returns>
        public float scale()
        {
            return mScale;
        }

        /// <summary>
        /// 縦方向の描画倍率を取得します。
        /// </summary>
        /// <returns></returns>
        public float getScale()
        {
            return mScale;
        }

        /// <summary>
        /// 縦方向の描画倍率を設定します。
        /// </summary>
        /// <param name="value"></param>
        public void setScale(float value)
        {
            if (value < MIN_SCALE) {
                mScale = MIN_SCALE;
            } else if (MAX_SCALE < value) {
                mScale = MAX_SCALE;
            } else {
                mScale = value;
            }
        }

        /// <summary>
        /// 全ての波形描画コンテキストが保持しているデータをクリアします
        /// </summary>
        public void unloadAll()
        {
            for (int i = 0; i < mDrawer.Length; i++) {
                WaveDrawContext context = mDrawer[i];
                if (context == null) {
                    continue;
                }
                context.unload();
            }
        }

        /// <summary>
        /// 波形描画コンテキストに、指定したWAVEファイルの指定区間を再度読み込みます。
        /// </summary>
        /// <param name="index">読込を行わせる波形描画コンテキストのインデックス</param>
        /// <param name="file">読み込むWAVEファイルのパス</param>
        /// <param name="sec_from">読み込み区間の開始秒時</param>
        /// <param name="sec_to">読み込み区間の終了秒時</param>
        public void reloadPartial(int index, string file, double sec_from, double sec_to)
        {
            if (index < 0 || mDrawer.Length <= index) {
                return;
            }
            if (mDrawer[index] == null) {
                mDrawer[index] = new WaveDrawContext();
                mDrawer[index].load(file);
            } else {
                mDrawer[index].reloadPartial(file, sec_from, sec_to);
            }
        }

        /// <summary>
        /// 波形描画コンテキストに、指定したWAVEファイルを読み込みます。
        /// </summary>
        /// <param name="index">読込を行わせる波形描画コンテキストのインデックス</param>
        /// <param name="wave_path">読み込むWAVEファイルのパス</param>
        public void load(int index, string wave_path)
        {
            if (index < 0 || mDrawer.Length <= index) {
#if DEBUG
                sout.println("WaveView#load; index out of range");
#endif
                return;
            }
#if DEBUG
            sout.println("WaveView#load; index=" + index);
#endif
            if (mDrawer[index] == null) {
                mDrawer[index] = new WaveDrawContext();
            }
            mDrawer[index].load(wave_path);
        }

        /// <summary>
        /// オーバーライドされます。
        /// <seealso cref="M:System.Windows.Forms.Control.OnPaint"/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            if (mGraphics == null) {
                mGraphics = new Graphics();
            }
		mGraphics.NativeGraphics = e.Graphics;
            paint(mGraphics);
        }
    }

}
