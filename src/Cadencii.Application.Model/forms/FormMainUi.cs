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
namespace cadencii
{

    /// <summary>
    /// メイン画面の実装クラスが持つべきメソッドを定義するインターフェース
    /// </summary>
    public interface FormMainUi
    {
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
