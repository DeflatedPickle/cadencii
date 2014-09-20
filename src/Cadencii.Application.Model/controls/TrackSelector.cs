using System;
using cadencii.java.awt;
using cadencii.vsq;

namespace cadencii
{
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

		void Refresh ();
        
		event KeyEventHandler PreviewKeyDown;
		event KeyEventHandler KeyUp;
		event KeyEventHandler KeyDown;
		event MouseEventHandler MouseClick;
		event MouseEventHandler MouseDoubleClick;
		event MouseEventHandler MouseDown;
		event MouseEventHandler MouseUp;
		event MouseEventHandler MouseMove;
		event MouseEventHandler MouseWheel;

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
