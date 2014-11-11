using System;
using System.Collections.Generic;
using Cadencii.Media.Vsq;
using System.Linq;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public partial class FormMainModel
	{
		public class TrackMenuModel
		{
			readonly FormMainModel parent;

			public TrackMenuModel (FormMainModel parent)
			{
				this.parent = parent;
			}

			public void RunTrackOnCommand()
			{
				int selected = EditorManager.Selected;
				VsqTrack vsq_track = MusicManager.getVsqFile().Track[selected];
				bool old_status = vsq_track.isTrackOn();
				bool new_status = !old_status;
				int last_play_mode = vsq_track.getCommon().LastPlayMode;
				CadenciiCommand run = new CadenciiCommand(
					VsqCommand.generateCommandTrackChangePlayMode(
						selected,
						new_status ? last_play_mode : PlayMode.Off,
						last_play_mode));
				EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
				parent.form.menuTrackOn.Checked = new_status;
				parent.form.cMenuTrackTabTrackOn.Checked = new_status;
				parent.form.setEdited(true);
				parent.form.refreshScreen();
			}

			public void RunTrackRenderAllCommand()
			{
				List<int> list = new List<int>();
				int c = MusicManager.getVsqFile().Track.Count;
				for (int i = 1; i < c; i++) {
					if (EditorManager.getRenderRequired(i)) {
						list.Add(i);
					}
				}
				if (list.Count <= 0) {
					return;
				}
				EditorManager.patchWorkToFreeze(parent.form, list);
			}

			//BOOKMARK: menuTrack
			#region menuTrack*
			public void RunTrackDropDownOpening()
			{
				#if DEBUG
				Logger.StdOut("FormMain#menuTrack_DropDownOpening");
				#endif
				parent.form.updateTrackMenuStatus();
			}

			public void RunTrackOverlayCommand()
			{
				EditorManager.TrackOverlay = (!EditorManager.TrackOverlay);
				parent.form.refreshScreen();
			}

			public void RunTrackRenderCurrentCommand()
			{
				List<int> tracks = new List<int>();
				tracks.Add(EditorManager.Selected);
				EditorManager.patchWorkToFreeze(parent.form, tracks);
			}

			public void RunTrackRendererDropDownOpening()
			{
				parent.form.updateRendererMenu();
			}
			#endregion

			public void RunChangeRendererCommand(RendererKind kind, int resampler_index)
			{
				VsqFileEx vsq = MusicManager.getVsqFile ();
				int selected = EditorManager.Selected;
				VsqTrack vsq_track = vsq.Track [selected];
				RendererKind old = VsqFileEx.getTrackRendererKind (vsq_track);
				int old_resampler_index = VsqFileEx.getTrackResamplerUsed (vsq_track);
				bool changed = (old != kind);
				if (!changed && kind == RendererKind.UTAU) {
					changed = (old_resampler_index != resampler_index);
				}

				if (!changed) {
					return;
				}

				var track_copy = (VsqTrack)vsq_track.clone ();
				List<VsqID> singers = MusicManager.getSingerListFromRendererKind (kind);
				string renderer = kind.getVersionString ();
				if (singers == null) {
					Logger.StdErr ("FormMain#changeRendererCor; singers is null");
					return;
				}

				track_copy.changeRenderer (renderer, singers);
				VsqFileEx.setTrackRendererKind (track_copy, kind);
				if (kind == RendererKind.UTAU) {
					VsqFileEx.setTrackResamplerUsed (track_copy, resampler_index);
				}
				CadenciiCommand run = VsqFileEx.generateCommandTrackReplace (selected,
					                      track_copy,
					                      vsq.AttachedCurves.get (selected - 1));
				EditorManager.editHistory.register (vsq.executeCommand (run));

				parent.RendererMenuHandlers.ForEach ((handler) => handler.updateCheckedState (kind));
				var utau = parent.RendererMenuHandlers.FirstOrDefault (h => h.RenderKind == RendererKind.UTAU);
				for (int i = 0; i < utau.ContextMenuItem.DropDownItems.Count; i++) {
					((UiToolStripMenuItem)utau.ContextMenuItem.DropDownItems [i]).Checked = (i == resampler_index);
				}
				for (int i = 0; i < utau.TrackMenuItem.DropDownItems.Count; i++) {
					((UiToolStripMenuItem)utau.TrackMenuItem.DropDownItems [i]).Checked = (i == resampler_index);
				}
				parent.form.setEdited (true);
				parent.form.refreshScreen ();
			}

			//BOOKMARK: cMenuTrackTab
			#region cMenuTrackTab

			public void RunTrackTabOpening()
			{
				#if DEBUG
				Logger.StdOut(GetType () + ".cMenuTrackTab_Opening");
				#endif
				parent.form.updateTrackMenuStatus();
			}

			public void RunTrackTabOverlayCommand()
			{
				EditorManager.TrackOverlay = (!EditorManager.TrackOverlay);
				parent.form.refreshScreen();
			}

			public void RunTrackTabRenderCurrentCommand ()
			{
				List<int> tracks = new List<int>();
				tracks.Add(EditorManager.Selected);
				EditorManager.patchWorkToFreeze(parent.form, tracks);
			}

			#endregion
		}
	}
}
