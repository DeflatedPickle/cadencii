using System;
using Cadencii.Gui;
using System.Collections.Generic;

namespace cadencii
{
	public partial class FormMainModel
	{
		public class TrackSelectorModel
		{
			readonly FormMainModel parent;

			public TrackSelectorModel (FormMainModel parent)
			{
				this.parent = parent;
			}

			bool mMouseDownedTrackSelector = false;
			ExtDragXMode mExtDragXTrackSelector = ExtDragXMode.NONE;

			//BOOKMARK: trackSelector
			#region trackSelector
			public void RunCommandExecuted()
			{
				parent.form.setEdited(true);
				parent.form.refreshScreen();
			}

			public void RunMouseClick (MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Right) {
					if (EditorManager.keyWidth < e.X && e.X < parent.form.TrackSelector.Width) {
						if (parent.form.TrackSelector.Height - TrackSelectorConsts.OFFSET_TRACK_TAB <= e.Y && e.Y <= parent.form.TrackSelector.Height) {
							parent.form.MenuTrackTab.Show(parent.form.TrackSelector, e.X, e.Y);
						} else {
							parent.form.MenuTrackSelector.Show(parent.form.TrackSelector, e.X, e.Y);
						}
					}
				}
			}

			public void RunMouseDoubleClick(MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Middle) {
					// ツールをポインター <--> 鉛筆に切り替える
					if (EditorManager.keyWidth < e.X &&
						e.Y < parent.form.TrackSelector.Height - TrackSelectorConsts.OFFSET_TRACK_TAB * 2) {
						if (EditorManager.SelectedTool == EditTool.ARROW) {
							EditorManager.SelectedTool = (EditTool.PENCIL);
						} else {
							EditorManager.SelectedTool = (EditTool.ARROW);
						}
					}
				}
			}

			public void RunMouseDown(MouseEventArgs e)
			{
				if (EditorManager.keyWidth < e.X) {
					mMouseDownedTrackSelector = true;
					if (parent.IsMouseMiddleButtonDown(e.Button)) {
						parent.form.mEditCurveMode = CurveEditMode.MIDDLE_DRAG;
						parent.form.mButtonInitial = new Point(e.X, e.Y);
						parent.form.mMiddleButtonHScroll = parent.form.hScroll.Value;
						parent.form.Cursor = Cursors.Hand;
					}
				}
			}

			public void RunMouseMove(MouseEventArgs e)
			{
				if (parent.form.mFormActivated && EditorManager.InputTextBox != null) {
					bool input_visible = !EditorManager.InputTextBox.IsDisposed && EditorManager.InputTextBox.Visible;
					#if ENABLE_PROPERTY
					bool prop_editing = EditorManager.propertyPanel.isEditing();
					#else
					bool prop_editing = false;
					#endif
					if (!input_visible && !prop_editing) {
						parent.form.TrackSelector.requestFocus();
					}
				}
				if (e.Button == MouseButtons.None) {
					if (!parent.timer.Enabled) {
						parent.form.refreshScreen(true);
					}
					return;
				}
				int parent_width = parent.form.TrackSelector.Width;
				if (parent.form.mEditCurveMode == CurveEditMode.MIDDLE_DRAG) {
					if (EditorManager.isPlaying()) {
						return;
					}

					int draft = parent.form.computeHScrollValueForMiddleDrag(e.X);
					if (parent.form.hScroll.Value != draft) {
						parent.form.hScroll.Value = draft;
					}
				} else {
					if (mMouseDownedTrackSelector) {
						if (mExtDragXTrackSelector == ExtDragXMode.NONE) {
							if (EditorManager.keyWidth > e.X) {
								mExtDragXTrackSelector = ExtDragXMode.LEFT;
							} else if (parent_width < e.X) {
								mExtDragXTrackSelector = ExtDragXMode.RIGHT;
							}
						} else {
							if (EditorManager.keyWidth <= e.X && e.X <= parent_width) {
								mExtDragXTrackSelector = ExtDragXMode.NONE;
							}
						}
					} else {
						mExtDragXTrackSelector = ExtDragXMode.NONE;
					}

					if (mExtDragXTrackSelector != ExtDragXMode.NONE) {
						double now = PortUtil.getCurrentTime();
						double dt = now - parent.form.mTimerDragLastIgnitted;
						parent.form.mTimerDragLastIgnitted = now;
						int px_move = EditorManager.editorConfig.MouseDragIncrement;
						if (px_move / dt > EditorManager.editorConfig.MouseDragMaximumRate) {
							px_move = (int)(dt * EditorManager.editorConfig.MouseDragMaximumRate);
						}
						px_move += 5;
						if (mExtDragXTrackSelector == ExtDragXMode.LEFT) {
							px_move *= -1;
						}
						double d_draft = parent.form.hScroll.Value + px_move * parent.form.controller.ScaleXInv;
						if (d_draft < 0.0) {
							d_draft = 0.0;
						}
						int draft = (int)d_draft;
						if (parent.form.hScroll.Maximum < draft) {
							parent.form.hScroll.Maximum = draft;
						}
						if (draft < parent.form.hScroll.Minimum) {
							draft = parent.form.hScroll.Minimum;
						}
						parent.form.hScroll.Value = draft;
					}
				}
				if (!parent.timer.Enabled) {
					parent.form.refreshScreen(true);
				}
			}

			public void RunMouseUp(MouseEventArgs e)
			{
				mMouseDownedTrackSelector = false;
				if (parent.form.mEditCurveMode == CurveEditMode.MIDDLE_DRAG) {
					parent.form.mEditCurveMode = CurveEditMode.NONE;
					parent.form.Cursor = Cursors.Default;
				}
			}

			public void RunMouseWheel(MouseEventArgs e)
			{
				#if DEBUG
				sout.println("FormMain#trackSelector_MouseWheel");
				#endif
				if ((AwtHost.ModifierKeys & Keys.Shift) == Keys.Shift) {
					double new_val = (double)parent.form.vScroll.Value - e.Delta;
					int max = parent.form.vScroll.Maximum - parent.form.vScroll.Minimum;
					int min = parent.form.vScroll.Minimum;
					if (new_val > max) {
						parent.form.vScroll.Value = max;
					} else if (new_val < min) {
						parent.form.vScroll.Value = min;
					} else {
						parent.form.vScroll.Value = (int)new_val;
					}
				} else {
					parent.form.hScroll.Value = parent.form.computeScrollValueFromWheelDelta(e.Delta);
				}
				parent.form.refreshScreen();
			}

			public void RunPreferredMinHeightChanged()
			{
				if (parent.form.menuVisualControlTrack.Checked) {
					parent.form.splitContainer1.Panel2MinSize = (parent.form.TrackSelector.getPreferredMinSize());
					#if DEBUG
					sout.println("FormMain#trackSelector_PreferredMinHeightChanged; splitContainer1.Panel2MinSize changed");
					#endif
				}
			}

			public void RunPreviewKeyDown(KeyEventArgs e)
			{
				var e0 = new KeyEventArgs(e.KeyData);
				parent.form.processSpecialShortcutKey(e0, true);
			}

			public void RunRenderRequired(int track)
			{
				List<int> list = new List<int>();
				list.Add(track);
				EditorManager.patchWorkToFreeze(parent.form, list);
				/*int selected = EditorManager.Selected;
            Vector<Integer> t = new Vector<Integer>( Arrays.asList( PortUtil.convertIntArray( tracks ) ) );
            if ( t.contains( selected) ) {
                String file = fsys.combine( ApplicationGlobal.getTempWaveDir(), selected + ".wav" );
                if ( PortUtil.isFileExists( file ) ) {
                    Thread loadwave_thread = new Thread( new ParameterizedThreadStart( this.loadWave ) );
                    loadwave_thread.IsBackground = true;
                    loadwave_thread.Start( new Object[]{ file, selected - 1 } );
                }
            }*/
			}

			public void RunSelectedCurveChanged(CurveType type)
			{
				parent.form.refreshScreen();
			}

			public void RunSelectedTrackChanged(int selected)
			{
				EditorManager.itemSelection.clearBezier();
				EditorManager.itemSelection.clearEvent();
				EditorManager.itemSelection.clearPoint();
				parent.form.updateDrawObjectList();
				parent.form.refreshScreen();
			}
			#endregion

					}
	}
}
