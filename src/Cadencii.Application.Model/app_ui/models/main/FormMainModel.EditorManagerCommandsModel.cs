using System;
using System.Linq;
using Cadencii.Gui.Toolkit;
using cadencii;
using Cadencii.Application.Controls;
using System.Reflection;

namespace Cadencii.Application.Models
{
	public partial class FormMainModel
	{
		public class EditorManagerCommandsModel
		{
			readonly FormMainModel parent;

			public EditorManagerCommandsModel (FormMainModel parent)
			{
				this.parent = parent;
			}

			public void InitializeControls ()
			{
				var bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
				foreach (var fi in GetType ().GetFields (bf).Where (f => f.FieldType == typeof (UiMenuItem) || f.FieldType == typeof (UiToolStripButton))) {
					var spi = parent.form.GetType ().GetProperty (fi.Name, bf);
					var sfi = spi == null ? parent.form.GetType ().GetField (fi.Name, bf) : null;
					fi.SetValue (this, spi != null ? spi.GetValue (parent.form, null) : sfi.GetValue (parent.form));
				}
			}

			//BOOKMARK: EditorManager
			#region EditorManager
			public void EditorManager_EditedStateChanged(Object sender, bool edited)
			{
				parent.form.setEdited(edited);
			}

			public void EditorManager_GridVisibleChanged(Object sender, EventArgs e)
			{
				parent.form.menuVisualGridline.Checked = EditorManager.isGridVisible();
				parent.form.stripBtnGrid.Pushed = EditorManager.isGridVisible();
				parent.form.cMenuPianoGrid.Checked = EditorManager.isGridVisible();
			}

			public void EditorManager_MainWindowFocusRequired(Object sender, EventArgs e)
			{
				parent.form.Focus();
			}

			public void EditorManager_SelectedToolChanged(Object sender, EventArgs e)
			{
				parent.form.applySelectedTool();
			}

			BgmMenuItem menuEditCut, menuEditPaste, menuEditDelete, menuLyricVibratoProperty, menuLyricExpressionProperty;
			BgmMenuItem cMenuPianoCut, cMenuPianoCopy, cMenuPianoDelete, cMenuPianoExpressionProperty;
			UiToolBarButton stripBtnCut, stripBtnCopy;

			public void ItemSelectionModel_SelectedEventChanged(Object sender, bool selected_event_is_null)
			{
				menuEditCut.Enabled = !selected_event_is_null;
				menuEditPaste.Enabled = !selected_event_is_null;
				menuEditDelete.Enabled = !selected_event_is_null;
				cMenuPianoCut.Enabled = !selected_event_is_null;
				cMenuPianoCopy.Enabled = !selected_event_is_null;
				cMenuPianoDelete.Enabled = !selected_event_is_null;
				cMenuPianoExpressionProperty.Enabled = !selected_event_is_null;
				menuLyricVibratoProperty.Enabled = !selected_event_is_null;
				menuLyricExpressionProperty.Enabled = !selected_event_is_null;
				stripBtnCut.Enabled = !selected_event_is_null;
				stripBtnCopy.Enabled = !selected_event_is_null;
			}

			public void EditorManager_UpdateBgmStatusRequired(Object sender, EventArgs e)
			{
				parent.form.updateBgmMenuState();
			}

			public void EditorManager_WaveViewRealoadRequired(Object sender, WaveViewRealoadRequiredEventArgs arg)
			{
				int track = arg.track;
				string file = arg.file;
				double sec_start = arg.secStart;
				double sec_end = arg.secEnd;
				if (sec_start <= sec_end) {
					parent.form.waveView.Model.reloadPartial(track - 1, file, sec_start, sec_end);
				} else {
					parent.form.waveView.Model.load(track - 1, file);
				}
			}
			#endregion

		}
	}
}
