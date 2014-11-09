using System;
using Cadencii.Gui;
using cadencii.vsq;
using Cadencii.Utilities;
using Cadencii.Gui.Toolkit;

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

	public interface TrackSelector : UiControl
	{
		void updateVisibleCurves ();

		void setBounds (int x, int y, int width, int height);

		void setBounds (Cadencii.Gui.Rectangle rc);

		Cadencii.Gui.Cursor getCursor ();

		void setCursor (Cadencii.Gui.Cursor value);

		Object getParent ();

		Cadencii.Gui.Point getLocationOnScreen ();

		Cadencii.Gui.Point getLocation ();

		void setLocation (int x, int y);

		void setLocation (Cadencii.Gui.Point p);

		Cadencii.Gui.Rectangle getBounds ();

		Cadencii.Gui.Dimension getSize ();

		void setSize (int width, int height);

		void setSize (Cadencii.Gui.Dimension d);

		void setBackground (Cadencii.Gui.Color color);

		Cadencii.Gui.Color getBackground ();

		void setForeground (Cadencii.Gui.Color color);

		Cadencii.Gui.Color getForeground ();

		bool isEnabled ();

		void setEnabled (bool value);

		void requestFocus ();

		bool isFocusOwner ();

		void setPreferredSize (Cadencii.Gui.Dimension size);

		Cadencii.Gui.Font getFont ();

		void setFont (Cadencii.Gui.Font font);

		Cadencii.Gui.Point pointToScreen (Cadencii.Gui.Point point_on_client);

		Cadencii.Gui.Point pointToClient (Cadencii.Gui.Point point_on_screen);

		Object getTag ();

		void setTag (Object value);

		void applyLanguage ();

		void applyFont (Cadencii.Gui.Font font);

		int getRowsPerColumn ();

		int getPreferredMinSize ();

		FormMain getMainForm ();

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
