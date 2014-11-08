/*
 * FormMainController.cs
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

namespace cadencii
{


    /// <summary>
    /// メイン画面のコントローラ
    /// </summary>
    public class FormMainController : ControllerBase
    {
        private UiFormMain ui;

        public FormMainController()
        {
			ScaleX = 0.1f;
        }

        /// <summary>
        /// MIDIステップ入力モードがONかどうかを取得します
        /// </summary>
        /// <returns></returns>
		public bool IsStepSequencerEnabled { get; set; }

        public void setupUi(UiFormMain ui)
        {
            this.ui = ui;
        }
				
        /// <summary>
        /// ピアノロールの，Y方向のスケールを取得します(pixel/cent)
        /// </summary>
        /// <returns></returns>
		public float ScaleY {
			get {
				if (EditorManager.editorConfig.PianoRollScaleY < EditorConfig.MIN_PIANOROLL_SCALEY) {
					EditorManager.editorConfig.PianoRollScaleY = EditorConfig.MIN_PIANOROLL_SCALEY;
				} else if (EditorConfig.MAX_PIANOROLL_SCALEY < EditorManager.editorConfig.PianoRollScaleY) {
					EditorManager.editorConfig.PianoRollScaleY = EditorConfig.MAX_PIANOROLL_SCALEY;
				}
				if (EditorManager.editorConfig.PianoRollScaleY == 0) {
					return EditorManager.editorConfig.PxTrackHeight / 100.0f;
				} else if (EditorManager.editorConfig.PianoRollScaleY > 0) {
					return (2 * EditorManager.editorConfig.PianoRollScaleY + 5) * EditorManager.editorConfig.PxTrackHeight / 5 / 100.0f;
				} else {
					return (EditorManager.editorConfig.PianoRollScaleY + 8) * EditorManager.editorConfig.PxTrackHeight / 8 / 100.0f;
				}
			}
		}

		/// <summary>
		/// ピアノロールの，X方向のスケールを取得します(pixel/clock)
		/// </summary>
		/// <returns></returns>
		public float ScaleX { get; set; }

		/// <summary>
		/// ピアノロールの，X方向のスケールの逆数を取得します(clock/pixel)
		/// </summary>
		/// <returns></returns>
		public float ScaleXInv {
			get { return 1.0f / ScaleX; }
		}

        /// <summary>
        /// ピアノロール画面の，ビューポートと仮想スクリーンとの横方向のオフセットを取得します
        /// </summary>
        /// <returns></returns>
		public int StartToDrawX { get; set; }

        /// <summary>
        /// ピアノロール画面の，ビューポートと仮想スクリーンとの縦方向のオフセットを取得します
        /// </summary>
        /// <returns></returns>
		public int StartToDrawY { get; set; }
    }

}
