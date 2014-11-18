using System;
using Cadencii.Media.Vsq;
using cadencii;

namespace Cadencii.Application.Models
{
	public partial class FormMainModel
	{
		public class PianoMenuModel
		{
			readonly FormMainModel parent;

			public PianoMenuModel (FormMainModel parent)
			{
				this.parent = parent;
			}
			//BOOKMARK: cMenuPiano
			#region cMenuPiano*

			/*
			public void RunPianoExpressionCommand ()
			{
				if (EditorManager.itemSelection.getEventCount() > 0) {
					VsqFileEx vsq = MusicManager.getVsqFile();
					int selected = EditorManager.Selected;
					SynthesizerType type = SynthesizerType.VOCALOID2;
					RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
					if (kind == RendererKind.VOCALOID1) {
						type = SynthesizerType.VOCALOID1;
					}
					VsqEvent original = EditorManager.itemSelection.getLastEvent().original;
					FormNoteExpressionConfig dlg = null;
					try {
						dlg = ApplicationUIHost.Create<FormNoteExpressionConfig>(type, original.ID.NoteHeadHandle);
						int id = EditorManager.itemSelection.getLastEvent().original.InternalID;
						dlg.PMBendDepth = (original.ID.PMBendDepth);
						dlg.PMBendLength = (original.ID.PMBendLength);
						dlg.PMbPortamentoUse = (original.ID.PMbPortamentoUse);
						dlg.DEMdecGainRate = (original.ID.DEMdecGainRate);
						dlg.DEMaccent = (original.ID.DEMaccent);
						var dr = DialogManager.showModalDialog(dlg, this);
						if (dr == 1) {
							VsqID copy = (VsqID)original.ID.clone();
							copy.PMBendDepth = dlg.PMBendDepth;
							copy.PMBendLength = dlg.PMBendLength;
							copy.PMbPortamentoUse = dlg.PMbPortamentoUse;
							copy.DEMdecGainRate = dlg.DEMdecGainRate;
							copy.DEMaccent = dlg.DEMaccent;
							copy.NoteHeadHandle = dlg.EditedNoteHeadHandle;
							CadenciiCommand run = new CadenciiCommand(
								VsqCommand.generateCommandEventChangeIDContaints(selected, id, copy));
							EditorManager.editHistory.register(vsq.executeCommand(run));
							parent.form.setEdited(true);
						}
					} catch (Exception ex) {
						Logger.write(GetType () + ".cMenuPianoExpression_Click; ex=" + ex + "\n");
					} finally {
						if (dlg != null) {
							try {
								dlg.Close();
							} catch (Exception ex2) {
								Logger.write(GetType () + ".cMenuPianoExpression_Click; ex=" + ex2 + "\n");
							}
						}
					}
				}
			}
			*/

			public void RunPianoPointerCommand()
			{
				EditorManager.SelectedTool = (EditTool.ARROW);
			}

			public void RunPianoPencilCommand()
			{
				EditorManager.SelectedTool = (EditTool.PENCIL);
			}

			public void RunPianoEraserCommand()
			{
				EditorManager.SelectedTool = (EditTool.ERASER);
			}

			public void RunPianoGridCommand()
			{
				bool new_v = !EditorManager.isGridVisible();
				parent.form.cMenuPianoGrid.Checked = new_v;
				EditorManager.setGridVisible(new_v);
			}

			public void RunPianoOpeningCommand()
			{
				parent.form.updateCopyAndPasteButtonStatus();
				parent.form.cMenuPianoImportLyric.Enabled = EditorManager.itemSelection.getLastEvent() != null;
			}

			public void RunPianoFixed01Command()
			{
				parent.form.mPencilMode.setMode(PencilModeEnum.L1);
				parent.form.updateCMenuPianoFixed();
			}

			public void RunPianoFixed02Command()
			{
				parent.form.mPencilMode.setMode(PencilModeEnum.L2);
				parent.form.updateCMenuPianoFixed();
			}

			public void RunPianoFixed04Command()
			{
				parent.form.mPencilMode.setMode(PencilModeEnum.L4);
				parent.form.updateCMenuPianoFixed();
			}

			public void RunPianoFixed08Command()
			{
				parent.form.mPencilMode.setMode(PencilModeEnum.L8);
				parent.form.updateCMenuPianoFixed();
			}

			public void RunPianoFixed16Command()
			{
				parent.form.mPencilMode.setMode(PencilModeEnum.L16);
				parent.form.updateCMenuPianoFixed();
			}

			public void RunPianoFixed32Command()
			{
				parent.form.mPencilMode.setMode(PencilModeEnum.L32);
				parent.form.updateCMenuPianoFixed();
			}

			public void RunPianoFixed64Command()
			{
				parent.form.mPencilMode.setMode(PencilModeEnum.L64);
				parent.form.updateCMenuPianoFixed();
			}

			public void RunPianoFixed128Command()
			{
				parent.form.mPencilMode.setMode(PencilModeEnum.L128);
				parent.form.updateCMenuPianoFixed();
			}

			public void RunPianoFixedOffCommand()
			{
				parent.form.mPencilMode.setMode(PencilModeEnum.Off);
				parent.form.updateCMenuPianoFixed();
			}

			public void RunPianoFixedTripletCommand()
			{
				parent.form.mPencilMode.setTriplet(!parent.form.mPencilMode.isTriplet());
				parent.form.updateCMenuPianoFixed();
			}

			public void RunPianoFixedDottedCommand()
			{
				parent.form.mPencilMode.setDot(!parent.form.mPencilMode.isDot());
				parent.form.updateCMenuPianoFixed();
			}

			public void RunPianoCurveCommand()
			{
				EditorManager.setCurveMode(!EditorManager.isCurveMode());
				parent.form.applySelectedTool();
			}
			#endregion

		}
	}
}
