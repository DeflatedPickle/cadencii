using System;
using Cadencii.Gui;
using Cadencii.Media.Vsq;

namespace cadencii
{

	public static class TrackSelectorConsts
	{
	#region constants and internal enums
        /// <summary>
        /// ベジエ曲線の色
        /// </summary>
		public static readonly Color COLOR_BEZIER_CURVE = Cadencii.Gui.Colors.Navy;
        /// <summary>
        /// ベジエ曲線の補助線の色
        /// </summary>
        public static readonly Color COLOR_BEZIER_AUXILIARY = Cadencii.Gui.Colors.Orange;
        /// <summary>
        /// ベジエ曲線の制御点の色
        /// </summary>
        public static readonly Color COLOR_BEZIER_DOT_NORMAL = new Color(237, 107, 158);
        /// <summary>
        /// ベジエ曲線の制御点の枠色
        /// </summary>
        public static readonly Color COLOR_BEZIER_DOT_NORMAL_DARK = new Color(153, 19, 70);
        /// <summary>
        /// ベジエ曲線のデータ点の色
        /// </summary>
        public static readonly Color COLOR_BEZIER_DOT_BASE = new Color(125, 198, 34);
        /// <summary>
        /// ベジエ曲線のデータ点の枠色
        /// </summary>
        public static readonly Color COLOR_BEZIER_DOT_BASE_DARK = new Color(62, 99, 17);

        /// <summary>
        /// データ点のハイライト色
        /// </summary>
		public static readonly Color COLOR_DOT_HILIGHT = Cadencii.Gui.Colors.Coral;
        public static readonly Color COLOR_A244R255G023B012 = new Color(255, 23, 12, 244);
        public static readonly Color COLOR_A144R255G255B255 = new Color(255, 255, 255, 144);
        public static readonly Color COLOR_A072R255G255B255 = new Color(255, 255, 255, 72);
        public static readonly Color COLOR_A127R008G166B172 = new Color(8, 166, 172, 127);
        public static readonly Color COLOR_A098R000G000B000 = new Color(0, 0, 0, 98);
        /// <summary>
        /// 歌手変更を表すボックスの枠線のハイライト色
        /// </summary>
        public static readonly Color COLOR_SINGERBOX_BORDER_HILIGHT = new Color(246, 251, 10);
        /// <summary>
        /// 歌手変更を表すボックスの枠線の色
        /// </summary>
        public static readonly Color COLOR_SINGERBOX_BORDER = new Color(182, 182, 182);
        /// <summary>
        /// ビブラートコントロールカーブの、ビブラート以外の部分を塗りつぶす時の色
        /// </summary>
        public static readonly Color COLOR_VIBRATO_SHADOW = new Color(0, 0, 0, 127);
        /// <summary>
        /// マウスの軌跡を描くときの塗りつぶし色
        /// </summary>
        public static readonly Color COLOR_MOUSE_TRACER = new Color(8, 166, 172, 127);
        /// <summary>
        /// ベロシティを画面に描くときの，棒グラフの幅(pixel)
        /// </summary>
        public const int VEL_BAR_WIDTH = 8;
        /// <summary>
        /// パフォーマンスカウンタ用バッファの容量
        /// </summary>
        public const int NUM_PCOUNTER = 50;
        /// <summary>
        /// コントロールの下辺から、TRACKタブまでのオフセット(px)
        /// </summary>
        public const int OFFSET_TRACK_TAB = 19;
        public const int FOOTER = 7;
        /// <summary>
        /// コントロールの上端と、グラフのY軸最大値位置との距離
        /// </summary>
        public const int HEADER = 8;
        public const int BUF_LEN = 512;
        /// <summary>
        /// 歌手変更イベントの表示矩形の幅
        /// </summary>
        public const int SINGER_ITEM_WIDTH = 66;
        /// <summary>
        /// RENDERボタンの幅(px)
        /// </summary>
        public const int PX_WIDTH_RENDER = 10;
        /// <summary>
        /// カーブ制御点の幅（実際は_DOT_WID * 2 + 1ピクセルで描画される）
        /// </summary>
        public const int DOT_WID = 3;
        /// <summary>
        /// カーブの種類を表す部分の，1個あたりの高さ（ピクセル，余白を含む）
        /// TrackSelectorの推奨表示高さは，HEIGHT_WITHOUT_CURVE + UNIT_HEIGHT_PER_CURVE * (カーブの個数)となる
        /// </summary>
        public const int UNIT_HEIGHT_PER_CURVE = 18;
        /// <summary>
        /// カーブの種類を除いた部分の高さ（ピクセル）．
        /// TrackSelectorの推奨表示高さは，HEIGHT_WITHOUT_CURVE + UNIT_HEIGHT_PER_CURVE * (カーブの個数)となる
        /// </summary>
        public const int HEIGHT_WITHOUT_CURVE = OFFSET_TRACK_TAB * 2 + UNIT_HEIGHT_PER_CURVE;
        /// <summary>
        /// トラックの名前表示部分の最大表示幅（ピクセル）
        /// </summary>
        public const int TRACK_SELECTOR_MAX_WIDTH = 80;
        /// <summary>
        /// 先行発音を表示する旗を描画する位置のy座標
        /// </summary>
        public const int OFFSET_PRE = 15;
        /// <summary>
        /// オーバーラップを表示する旗を描画する位置のy座標
        /// </summary>
        public const int OFFSET_OVL = 40;
        /// <summary>
        /// 旗の上下に追加するスペース(ピクセル)
        /// </summary>
        public const int FLAG_SPACE = 2;
        #endregion
	}
	
}
