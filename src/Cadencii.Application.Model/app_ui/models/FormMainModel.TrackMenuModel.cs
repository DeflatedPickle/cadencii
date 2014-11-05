using System;
using System.Collections.Generic;
using cadencii.vsq;

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
				sout.println("FormMain#menuTrack_DropDownOpening");
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
		}
	}
}
