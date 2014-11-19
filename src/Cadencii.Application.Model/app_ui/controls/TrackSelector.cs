using System;
using Cadencii.Gui;
using Cadencii.Media.Vsq;
using Cadencii.Utilities;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Forms;
using cadencii;

namespace Cadencii.Application.Controls
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

	public interface TrackSelector : UiControl
	{
		void updateVisibleCurves ();

		Cadencii.Gui.Cursor Cursor { get; set; }

		Object Parent { get; }

		Point LocationOnScreen { get; }

		Point Location { get; set; }

		Cadencii.Gui.Rectangle Bounds { get; set; }

		Cadencii.Gui.Dimension Size { get; set; }

		Cadencii.Gui.Color Background { get; set; }

		Cadencii.Gui.Color Foreground { get; set; }

		bool Enabled { get; set; }

		void RequestFocus ();

		bool IsFocusOwner ();

		void setPreferredSize (Cadencii.Gui.Dimension size);

		Cadencii.Gui.Font Font { get; set; }

		Cadencii.Gui.Point PointToScreen (Cadencii.Gui.Point point_on_client);

		Cadencii.Gui.Point PointToClient (Cadencii.Gui.Point point_on_screen);

		Object Tag { get; set; }

		void applyLanguage ();

		void applyFont (Cadencii.Gui.Font font);

		int RowsPerColumn { get; }

		int PreferredMinSize { get; }

		FormMain MainForm { get; }

		ValuePair<int, int> getSelectedRegion ();

		CurveType SelectedCurve { get; set; }

		int ValueFromYCoord (int y);

		int ValueFromYCoord (int y, int max, int min);

		int YCoordFromValue (int value);

		int YCoordFromValue (int value, int max, int min);

		bool CurveVisible { get; set; }

		float ScaleY { get; }

		Rectangle getRectFromCurveType (CurveType curve);

		void paint (Graphics graphics);

		void doInvalidate ();

		void drawVEL (Graphics g, VsqTrack track, Color color, bool is_front, CurveType type);

		void SelectPreviousCurve();
		void SelectNextCurve();
		// FIXME: these are ugly and should be eliminated.
		void OnMouseDown(Object sender, MouseEventArgs e);
		void OnMouseUp(Object sender, MouseEventArgs e);
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
