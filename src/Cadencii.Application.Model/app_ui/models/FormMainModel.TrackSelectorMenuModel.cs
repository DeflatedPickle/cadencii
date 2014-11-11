using System;
using cadencii.core;
using Cadencii.Gui;
using Cadencii.Media.Vsq;

namespace cadencii
{
	public partial class FormMainModel
	{
		public class TrackSelectorMenuModel
		{
			readonly FormMainModel parent;

			public TrackSelectorMenuModel (FormMainModel parent)
			{
				this.parent = parent;
			}

			//BOOKMARK: cMenuTrackSelector
			#region cMenuTrackSelector
			public void RunOpening()
			{
				parent.form.updateCopyAndPasteButtonStatus();

				// 選択ツールの状態に合わせて表示を更新
				parent.form.cMenuTrackSelectorPointer.Checked = false;
				parent.form.cMenuTrackSelectorPencil.Checked = false;
				parent.form.cMenuTrackSelectorLine.Checked = false;
				parent.form.cMenuTrackSelectorEraser.Checked = false;
				EditTool tool = EditorManager.SelectedTool;
				if (tool == EditTool.ARROW) {
					parent.form.cMenuTrackSelectorPointer.Checked = true;
				} else if (tool == EditTool.PENCIL) {
					parent.form.cMenuTrackSelectorPencil.Checked = true;
				} else if (tool == EditTool.LINE) {
					parent.form.cMenuTrackSelectorLine.Checked = true;
				} else if (tool == EditTool.ERASER) {
					parent.form.cMenuTrackSelectorEraser.Checked = true;
				}
				parent.form.cMenuTrackSelectorCurve.Checked = EditorManager.isCurveMode();
			}

			public void RunPointerCommand()
			{
				EditorManager.SelectedTool = (EditTool.ARROW);
				parent.form.refreshScreen();
			}

			public void RunPencilCommand()
			{
				EditorManager.SelectedTool = (EditTool.PENCIL);
				parent.form.refreshScreen();
			}

			public void RunLineCommand()
			{
				EditorManager.SelectedTool = (EditTool.LINE);
			}

			public void RunEraserCommand()
			{
				EditorManager.SelectedTool = (EditTool.ERASER);
			}

			public void RunCurveCommand()
			{
				EditorManager.setCurveMode(!EditorManager.isCurveMode());
			}

			public void RunDeleteBezierCommand()
			{
				foreach (var sbp in EditorManager.itemSelection.getBezierIterator()) {
					int chain_id = sbp.chainID;
					int point_id = sbp.pointID;
					VsqFileEx vsq = MusicManager.getVsqFile();
					int selected = EditorManager.Selected;
					BezierChain chain = (BezierChain)vsq.AttachedCurves.get(selected - 1).getBezierChain(parent.form.TrackSelector.getSelectedCurve(), chain_id).clone();
					int index = -1;
					for (int i = 0; i < chain.points.Count; i++) {
						if (chain.points[i].getID() == point_id) {
							index = i;
							break;
						}
					}
					if (index >= 0) {
						chain.points.RemoveAt(index);
						if (chain.points.Count == 0) {
							CadenciiCommand run = VsqFileEx.generateCommandDeleteBezierChain(selected,
								parent.form.TrackSelector.getSelectedCurve(),
								chain_id,
								ApplicationGlobal.appConfig.getControlCurveResolutionValue());
							EditorManager.editHistory.register(vsq.executeCommand(run));
						} else {
							CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain(selected,
								parent.form.TrackSelector.getSelectedCurve(),
								chain_id,
								chain,
								ApplicationGlobal.appConfig.getControlCurveResolutionValue());
							EditorManager.editHistory.register(vsq.executeCommand(run));
						}
						parent.form.setEdited(true);
						parent.form.refreshScreen();
						break;
					}
				}
			}
			#endregion

		}
	}
}
