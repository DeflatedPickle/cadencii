/*
 * FormMainUi.cs
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
using System.Collections.Generic;


namespace cadencii
{

    /// <summary>
    /// メイン画面の実装クラスが持つべきメソッドを定義するインターフェース
    /// </summary>
    public interface FormMainUi
    {
		int calculateStartToDrawX ();

		bool isMouseMiddleButtonDowned (cadencii.java.awt.MouseButtons mouseButtons);

		UiContextMenuStrip cMenuTrackTab { get; set; }
		UiContextMenuStrip cMenuTrackSelector { get; set; }

		TrackSelector trackSelector { get; set; }

		UiHScrollBar hScroll { get; set; }

		PictPianoRoll pictPianoRoll { get; set; }

		IList<ValuePairOfStringArrayOfKeys> getDefaultShortcutKeys ();

		void refreshScreen ();

		void updateScriptShortcut ();

		void setEdited (bool b);

		void updateDrawObjectList ();

        /// <summary>
        /// ピアノロールの部品にフォーカスを持たせる
        /// </summary>
        [PureVirtualFunction]
        void focusPianoRoll();

	/// <summary>
        /// 指定したゲートタイムがピアノロール上で可視状態となるよう、横スクロールバーを移動させます。
        /// </summary>
        /// <param name="clock"></param>
	[PureVirtualFunction]
	void ensureVisible(int clock);
    }

}
