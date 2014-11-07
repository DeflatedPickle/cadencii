using System;
using Cadencii.Gui;
using System.Collections.Generic;

namespace cadencii
{
	/// <summary>
	/// カーブエディタ画面の編集モード
	/// </summary>
	public enum CurveEditMode
	{
		/// <summary>
		/// 何もしていない
		/// </summary>
		NONE,
		/// <summary>
		/// 鉛筆ツールで編集するモード
		/// </summary>
		EDIT,
		/// <summary>
		/// ラインツールで編集するモード
		/// </summary>
		LINE,
		/// <summary>
		/// 鉛筆ツールでVELを編集するモード
		/// </summary>
		EDIT_VEL,
		/// <summary>
		/// ラインツールでVELを編集するモード
		/// </summary>
		LINE_VEL,
		/// <summary>
		/// 真ん中ボタンでドラッグ中
		/// </summary>
		MIDDLE_DRAG,
	}
	
}
