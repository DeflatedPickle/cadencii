using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public partial class FormMainModel
	{
		public class WaveViewModel
		{
			readonly FormMainModel parent;

			public WaveViewModel (FormMainModel parent)
			{
				this.parent = parent;
			}

			//BOOKMARK: waveView
			#region waveView
			public void RunMouseDoubleClick(MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Middle) {
					// ツールをポインター <--> 鉛筆に切り替える
					if (e.Y < parent.form.TrackSelector.Height - TrackSelectorConsts.OFFSET_TRACK_TAB * 2) {
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
				#if DEBUG
				Logger.StdOut("waveView_MouseDown; isMiddleButtonDowned=" + parent.IsMouseMiddleButtonDown(e.Button));
				#endif
				if (parent.IsMouseMiddleButtonDown(e.Button)) {
					parent.form.mEditCurveMode = CurveEditMode.MIDDLE_DRAG;
					parent.form.mButtonInitial = new Point(e.X, e.Y);
					parent.form.mMiddleButtonHScroll = parent.form.hScroll.Value;
					parent.form.Cursor = Cursors.Hand;
				}
			}

			public void RunMouseUp(MouseEventArgs e)
			{
				if (parent.form.mEditCurveMode == CurveEditMode.MIDDLE_DRAG) {
					parent.form.mEditCurveMode = CurveEditMode.NONE;
					parent.form.Cursor = Cursors.Default;
				}
			}

			public void RunMouseMove(MouseEventArgs e)
			{
				if (parent.form.mEditCurveMode == CurveEditMode.MIDDLE_DRAG) {
					int draft = parent.form.computeHScrollValueForMiddleDrag(e.X);
					if (parent.form.hScroll.Value != draft) {
						parent.form.hScroll.Value = draft;
					}
				}
			}
			#endregion

		}
	}
}
