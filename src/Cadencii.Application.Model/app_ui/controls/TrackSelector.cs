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

		UiFormMain getMainForm ();

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
