using System;
using Cadencii.Gui;
using System.Reflection;
using System.Linq;

namespace cadencii
{
	public partial class FormMainModel
	{
		public class ToolBarsModel
		{
			readonly FormMainModel parent;

			public ToolBarsModel (FormMainModel parent)
			{
				this.parent = parent;
			}

			bool field_references_initialized;

			void EnsureFieldReferences ()
			{
				if (!field_references_initialized) {
					var bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
					foreach (var fi in GetType ().GetFields ().Where (f => f.FieldType == typeof (UiToolBarButton) || f.FieldType == typeof (UiToolStripButton)))
						fi.SetValue (this, parent.form.GetType ().GetField (fi.Name, bf).GetValue (parent.form));
				}
				field_references_initialized = true;
			}

			UiToolBarButton stripBtnStartMarker, stripBtnEndMarker, stripBtnPointer, stripBtnPencil, stripBtnLine, stripBtnEraser, stripBtnGrid, stripBtnCurve;
			UiToolBarButton stripBtnMoveTop, stripBtnRewind, stripBtnForward, stripBtnMoveEnd, stripBtnPlay, stripBtnScroll, stripBtnLoop;
			UiToolBarButton stripBtnFileNew, stripBtnFileOpen, stripBtnFileSave, stripBtnCut, stripBtnCopy, stripBtnPaste, stripBtnUndo, stripBtnRedo;
			UiToolStripButton stripBtnStepSequencer;

			#region toolBarMeasure
			public void toolBarMeasure_MouseDown(MouseEventArgs e)
			{
				EnsureFieldReferences ();

				// マウス位置にあるボタンを捜す
				UiToolBarButton c = null;
				foreach (UiToolBarButton btn in parent.form.toolBarMeasure.Buttons) {
					var rc = btn.Rectangle;
					if (Utility.isInRect(e.X, e.Y, rc.Left, rc.Top, rc.Width, rc.Height)) {
						c = btn;
						break;
					}
				}
				if (c == null) {
					return;
				}

				if (c == parent.form.stripDDBtnQuantizeParent) {
					var rc = parent.form.stripDDBtnQuantizeParent.Rectangle;
					parent.form.stripDDBtnQuantize.Show(
						parent.form.toolBarMeasure,
						new Cadencii.Gui.Point(rc.Left, rc.Bottom));
				}
			}

			public void toolBarMeasure_ButtonClick(ToolBarButtonClickEventArgs e)
			{
				EnsureFieldReferences ();

				if (e.Button == stripBtnStartMarker) {
					parent.VisualMenu.RunStartMarkerCommand ();
				} else if (e.Button == stripBtnEndMarker) {
					parent.VisualMenu.RunEndMarkerCommand ();
				}/* else if ( e.Button == stripDDBtnLengthParent ) {
                System.Drawing.Rectangle rc = stripDDBtnLengthParent.Rectangle;
                stripDDBtnLength.Show(
                    toolBarMeasure,
                    new System.Drawing.Point( rc.Left, rc.Bottom ) );
            } else if ( e.Button == stripDDBtnQuantizeParent ) {
                System.Drawing.Rectangle rc = stripDDBtnQuantizeParent.Rectangle;
                stripDDBtnQuantize.Show(
                    toolBarMeasure,
                    new System.Drawing.Point( rc.Left, rc.Bottom ) );
            }*/
			}
			#endregion

			public void toolBarTool_ButtonClick(ToolBarButtonClickEventArgs e)
			{
				EnsureFieldReferences ();

				if (e.Button == stripBtnPointer) {
					stripBtnArrow_Click();
				} else if (e.Button == stripBtnPencil) {
					stripBtnPencil_Click();
				} else if (e.Button == stripBtnLine) {
					stripBtnLine_Click();
				} else if (e.Button == stripBtnEraser) {
					stripBtnEraser_Click();
				} else if (e.Button == stripBtnGrid) {
					stripBtnGrid_Click();
				} else if (e.Button == stripBtnCurve) {
					stripBtnCurve_Click();
				} else {
					parent.handleStripPaletteTool_Click (e.Button, null);
				}
			}

			public void toolBarPosition_ButtonClick(ToolBarButtonClickEventArgs e)
			{
				EnsureFieldReferences ();

				if (e.Button == stripBtnMoveTop) {
					MoveToTop ();
				} else if (e.Button == stripBtnRewind) {
					parent.Rewind ();
				} else if (e.Button == stripBtnForward) {
					parent.Forward ();
				} else if (e.Button == stripBtnMoveEnd) {
					MoveToEnd ();
				} else if (e.Button == stripBtnPlay) {
					stripBtnPlay_Click();
				} else if (e.Button == stripBtnScroll) {
					stripBtnScroll.Pushed = !stripBtnScroll.Pushed;
					stripBtnScroll_CheckedChanged();
				} else if (e.Button == stripBtnLoop) {
					stripBtnLoop.Pushed = !stripBtnLoop.Pushed;
					stripBtnLoop_CheckedChanged();
				}
			}

			public void toolBarFile_ButtonClick(ToolBarButtonClickEventArgs e)
			{
				EnsureFieldReferences ();

				if (e.Button == stripBtnFileNew) {
					parent.FileMenu.RunFileNewCommand ();
				} else if (e.Button == stripBtnFileOpen) {
					parent.FileMenu.RunFileOpenCommand ();
				} else if (e.Button == stripBtnFileSave) {
					parent.FileMenu.RunFileSaveCommand ();
				} else if (e.Button == stripBtnCut) {
					parent.Cut ();
				} else if (e.Button == stripBtnCopy) {
					parent.Copy ();
				} else if (e.Button == stripBtnPaste) {
					parent.Paste ();
				} else if (e.Button == stripBtnUndo) {
					parent.EditMenu.RunEditUndoCommand ();
				} else if (e.Button == stripBtnRedo) {
					parent.EditMenu.RunEditRedoCommand ();
				}
			}

			//BOOKMARK: stripBtn
			#region stripBtn*
			public void stripBtnGrid_Click()
			{
				bool new_v = !EditorManager.isGridVisible();
				stripBtnGrid.Pushed = new_v;
				EditorManager.setGridVisible(new_v);
			}

			public void stripBtnArrow_Click()
			{
				EditorManager.SelectedTool = (EditTool.ARROW);
			}

			public void stripBtnPencil_Click()
			{
				EditorManager.SelectedTool = (EditTool.PENCIL);
			}

			public void stripBtnLine_Click()
			{
				EditorManager.SelectedTool = (EditTool.LINE);
			}

			public void stripBtnEraser_Click()
			{
				EditorManager.SelectedTool = (EditTool.ERASER);
			}

			public void stripBtnCurve_Click()
			{
				EditorManager.setCurveMode(!EditorManager.isCurveMode());
			}

			public void stripBtnPlay_Click()
			{
				EditorManager.setPlaying(!EditorManager.isPlaying(), parent.form);
				parent.form.pictPianoRoll.Focus();
			}

			public void stripBtnScroll_CheckedChanged()
			{
				bool pushed = stripBtnScroll.Pushed;
				EditorManager.mAutoScroll = pushed;
				#if DEBUG
				sout.println("FormMain#stripBtnScroll_CheckedChanged; pushed=" + pushed);
				#endif
				parent.form.pictPianoRoll.Focus();
			}

			public void stripBtnLoop_CheckedChanged()
			{
				bool pushed = stripBtnLoop.Pushed;
				EditorManager.IsPreviewRepeatMode = pushed;
				parent.form.pictPianoRoll.Focus();
			}

			public void stripBtnStepSequencer_CheckedChanged()
			{
				EnsureFieldReferences ();

				// EditorManager.mAddingEventがnullかどうかで処理が変わるのでnullにする
				EditorManager.mAddingEvent = null;
				// モードを切り替える
				parent.form.controller.setStepSequencerEnabled(stripBtnStepSequencer.Checked);

				// MIDIの受信を開始
				#if ENABLE_MIDI
				if (parent.form.controller.isStepSequencerEnabled()) {
					parent.mMidiIn.start();
				} else {
					parent.mMidiIn.stop();
				}
				#endif
			}

			public void stripBtnStop_Click()
			{
				parent.Stop ();
			}

			void MoveToEnd()
			{
				if (EditorManager.isPlaying()) {
					EditorManager.setPlaying(false, parent.form);
				}
				EditorManager.setCurrentClock(MusicManager.getVsqFile().TotalClocks);
				parent.EnsurePlayerCursorVisible();
				parent.form.refreshScreen();
			}

			void MoveToTop()
			{
				if (EditorManager.isPlaying()) {
					EditorManager.setPlaying(false, parent.form);
				}
				EditorManager.setCurrentClock(0);
				parent.EnsurePlayerCursorVisible();
				parent.form.refreshScreen();
			}
			#endregion

		}
	}
}
