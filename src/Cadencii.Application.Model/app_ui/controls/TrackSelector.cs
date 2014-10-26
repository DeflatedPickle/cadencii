using System;
using cadencii.java.awt;
using cadencii.vsq;

namespace cadencii
{
        public enum MouseDownMode
        {
            NONE,
            CURVE_EDIT,
            TRACK_LIST,
            SINGER_LIST,
            /// <summary>
            /// マウス長押しによるVELの編集。マウスがDownされ、MouseHoverが発生するのを待っている状態
            /// </summary>
            VEL_WAIT_HOVER,
            /// <summary>
            /// マウス長押しによるVELの編集。MouseHoverが発生し、編集している状態
            /// </summary>
            VEL_EDIT,
            /// <summary>
            /// ベジエカーブのデータ点または制御点を移動させているモード
            /// </summary>
            BEZIER_MODE,
            /// <summary>
            /// ベジエカーブのデータ点の範囲選択をするモード
            /// </summary>
            BEZIER_SELECT,
            /// <summary>
            /// ベジエカーブのデータ点を新規に追加し、マウスドラッグにより制御点の位置を変えているモード
            /// </summary>
            BEZIER_ADD_NEW,
            /// <summary>
            /// 既存のベジエカーブのデータ点を追加し、マウスドラッグにより制御点の位置を変えているモード
            /// </summary>
            BEZIER_EDIT,
            /// <summary>
            ///  エンベロープのデータ点を移動させているモード
            /// </summary>
            ENVELOPE_MOVE,
            /// <summary>
            /// 先行発音を移動させているモード
            /// </summary>
            PRE_UTTERANCE_MOVE,
            /// <summary>
            /// オーバーラップを移動させているモード
            /// </summary>
            OVERLAP_MOVE,
            /// <summary>
            /// データ点を移動しているモード
            /// </summary>
            POINT_MOVE,
        }

	public class TrackSelectorConsts
	{
	#region constants and internal enums
        /// <summary>
        /// ベジエ曲線の色
        /// </summary>
		private static readonly Color COLOR_BEZIER_CURVE = cadencii.java.awt.Colors.Navy;
        /// <summary>
        /// ベジエ曲線の補助線の色
        /// </summary>
        private static readonly Color COLOR_BEZIER_AUXILIARY = cadencii.java.awt.Colors.Orange;
        /// <summary>
        /// ベジエ曲線の制御点の色
        /// </summary>
        private static readonly Color COLOR_BEZIER_DOT_NORMAL = new Color(237, 107, 158);
        /// <summary>
        /// ベジエ曲線の制御点の枠色
        /// </summary>
        private static readonly Color COLOR_BEZIER_DOT_NORMAL_DARK = new Color(153, 19, 70);
        /// <summary>
        /// ベジエ曲線のデータ点の色
        /// </summary>
        private static readonly Color COLOR_BEZIER_DOT_BASE = new Color(125, 198, 34);
        /// <summary>
        /// ベジエ曲線のデータ点の枠色
        /// </summary>
        private static readonly Color COLOR_BEZIER_DOT_BASE_DARK = new Color(62, 99, 17);

        /// <summary>
        /// データ点のハイライト色
        /// </summary>
		private static readonly Color COLOR_DOT_HILIGHT = cadencii.java.awt.Colors.Coral;
        private static readonly Color COLOR_A244R255G023B012 = new Color(255, 23, 12, 244);
        private static readonly Color COLOR_A144R255G255B255 = new Color(255, 255, 255, 144);
        private static readonly Color COLOR_A072R255G255B255 = new Color(255, 255, 255, 72);
        private static readonly Color COLOR_A127R008G166B172 = new Color(8, 166, 172, 127);
        private static readonly Color COLOR_A098R000G000B000 = new Color(0, 0, 0, 98);
        /// <summary>
        /// 歌手変更を表すボックスの枠線のハイライト色
        /// </summary>
        private static readonly Color COLOR_SINGERBOX_BORDER_HILIGHT = new Color(246, 251, 10);
        /// <summary>
        /// 歌手変更を表すボックスの枠線の色
        /// </summary>
        private static readonly Color COLOR_SINGERBOX_BORDER = new Color(182, 182, 182);
        /// <summary>
        /// ビブラートコントロールカーブの、ビブラート以外の部分を塗りつぶす時の色
        /// </summary>
        private static readonly Color COLOR_VIBRATO_SHADOW = new Color(0, 0, 0, 127);
        /// <summary>
        /// マウスの軌跡を描くときの塗りつぶし色
        /// </summary>
        private static readonly Color COLOR_MOUSE_TRACER = new Color(8, 166, 172, 127);
        /// <summary>
        /// ベロシティを画面に描くときの，棒グラフの幅(pixel)
        /// </summary>
        public const int VEL_BAR_WIDTH = 8;
        /// <summary>
        /// パフォーマンスカウンタ用バッファの容量
        /// </summary>
        const int NUM_PCOUNTER = 50;
        /// <summary>
        /// コントロールの下辺から、TRACKタブまでのオフセット(px)
        /// </summary>
        public const int OFFSET_TRACK_TAB = 19;
        const int FOOTER = 7;
        /// <summary>
        /// コントロールの上端と、グラフのY軸最大値位置との距離
        /// </summary>
        public const int HEADER = 8;
        const int BUF_LEN = 512;
        /// <summary>
        /// 歌手変更イベントの表示矩形の幅
        /// </summary>
        const int SINGER_ITEM_WIDTH = 66;
        /// <summary>
        /// RENDERボタンの幅(px)
        /// </summary>
        const int PX_WIDTH_RENDER = 10;
        /// <summary>
        /// カーブ制御点の幅（実際は_DOT_WID * 2 + 1ピクセルで描画される）
        /// </summary>
        const int DOT_WID = 3;
        /// <summary>
        /// カーブの種類を表す部分の，1個あたりの高さ（ピクセル，余白を含む）
        /// TrackSelectorの推奨表示高さは，HEIGHT_WITHOUT_CURVE + UNIT_HEIGHT_PER_CURVE * (カーブの個数)となる
        /// </summary>
        const int UNIT_HEIGHT_PER_CURVE = 18;
        /// <summary>
        /// カーブの種類を除いた部分の高さ（ピクセル）．
        /// TrackSelectorの推奨表示高さは，HEIGHT_WITHOUT_CURVE + UNIT_HEIGHT_PER_CURVE * (カーブの個数)となる
        /// </summary>
        const int HEIGHT_WITHOUT_CURVE = OFFSET_TRACK_TAB * 2 + UNIT_HEIGHT_PER_CURVE;
        /// <summary>
        /// トラックの名前表示部分の最大表示幅（ピクセル）
        /// </summary>
        const int TRACK_SELECTOR_MAX_WIDTH = 80;
        /// <summary>
        /// 先行発音を表示する旗を描画する位置のy座標
        /// </summary>
        const int OFFSET_PRE = 15;
        /// <summary>
        /// オーバーラップを表示する旗を描画する位置のy座標
        /// </summary>
        const int OFFSET_OVL = 40;
        /// <summary>
        /// 旗の上下に追加するスペース(ピクセル)
        /// </summary>
        const int FLAG_SPACE = 2;
        #endregion
	}
	public interface TrackSelector : UiControl
	{
		void updateVisibleCurves ();

		void setBounds (int x, int y, int width, int height);

		void setBounds (cadencii.java.awt.Rectangle rc);

		cadencii.java.awt.Cursor getCursor ();

		void setCursor (cadencii.java.awt.Cursor value);

		Object getParent ();

		cadencii.java.awt.Point getLocationOnScreen ();

		cadencii.java.awt.Point getLocation ();

		void setLocation (int x, int y);

		void setLocation (cadencii.java.awt.Point p);

		cadencii.java.awt.Rectangle getBounds ();

		cadencii.java.awt.Dimension getSize ();

		void setSize (int width, int height);

		void setSize (cadencii.java.awt.Dimension d);

		void setBackground (cadencii.java.awt.Color color);

		cadencii.java.awt.Color getBackground ();

		void setForeground (cadencii.java.awt.Color color);

		cadencii.java.awt.Color getForeground ();

		bool isEnabled ();

		void setEnabled (bool value);

		void requestFocus ();

		bool isFocusOwner ();

		void setPreferredSize (cadencii.java.awt.Dimension size);

		cadencii.java.awt.Font getFont ();

		void setFont (cadencii.java.awt.Font font);

		java.awt.Point pointToScreen (java.awt.Point point_on_client);

		java.awt.Point pointToClient (java.awt.Point point_on_screen);

		Object getTag ();

		void setTag (Object value);

		void applyLanguage ();

		void applyFont (java.awt.Font font);

		int getRowsPerColumn ();

		int getPreferredMinSize ();

		FormMainUi getMainForm ();

		ValuePair<int, int> getSelectedRegion ();

		CurveType getSelectedCurve ();

		void setSelectedCurve (CurveType value);

		int valueFromYCoord (int y);

		int valueFromYCoord (int y, int max, int min);

		int yCoordFromValue (int value);

		int yCoordFromValue (int value, int max, int min);

		bool isCurveVisible ();

		void setCurveVisible (bool value);

		float getScaleY ();

		Rectangle getRectFromCurveType (CurveType curve);

		void paint (Graphics graphics);

		void doInvalidate ();

		void drawVEL (Graphics g, VsqTrack track, Color color, bool is_front, CurveType type);

		void SelectPreviousCurve();
		void SelectNextCurve();
		void onMouseDown(Object sender, MouseEventArgs e);
		void onMouseUp(Object sender, MouseEventArgs e);
		void TrackSelector_MouseHover(Object sender, EventArgs e);
		void prepareSingerMenu(RendererKind renderer);
		BezierPoint HandleMouseMoveForBezierMove(int clock, int value, int value_raw, BezierPickedSide picked);
		BezierPoint HandleMouseMoveForBezierMove(MouseEventArgs e, BezierPickedSide picked);
		void setEditingPointID(int id);

		/// <summary>
		/// 最前面に表示するカーブの種類が変更されたとき発生するイベント．
		/// </summary>
		event SelectedCurveChangedEventHandler SelectedCurveChanged;
		/// <summary>
		/// 表示するトラック番号が変更されたとき発生するイベント．
		/// </summary>
		event SelectedTrackChangedEventHandler SelectedTrackChanged;
		/// <summary>
		/// VSQの編集コマンドが発行されたとき発生するイベント．
		/// </summary>
		event EventHandler CommandExecuted;
		/// <summary>
		/// トラックの歌声合成が要求されたとき発生するイベント．
		/// </summary>
		event RenderRequiredEventHandler RenderRequired;
		/// <summary>
		/// このコントロールの推奨最少表示高さが変わったとき発生するイベント．
		/// </summary>
		event EventHandler PreferredMinHeightChanged;
	}
}
