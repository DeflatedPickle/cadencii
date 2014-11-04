using System;

namespace cadencii
{
	public partial class FormMainModel
	{
		public class EditMenuModel
		{
			readonly FormMainModel parent;

			public EditMenuModel (FormMainModel parent)
			{
				this.parent = parent;
			}
			public void RunEditDeleteCommand()
			{
				parent.DeleteEvent();
			}

			public void RunEditUndoCommand()
			{
				#if DEBUG
				CDebug.WriteLine("menuEditUndo_Click");
				#endif
				parent.Undo();
				parent.form.refreshScreen();
			}


			public void RunEditRedoCommand()
			{
				#if DEBUG
				CDebug.WriteLine("menuEditRedo_Click");
				#endif
				parent.Redo();
				parent.form.refreshScreen();
			}

			public void RunEditSelectAllEventsCommand()
			{
				parent.SelectAllEvent();
			}

			public void RunEditSelectAllCommand()
			{
				parent.SelectAll();
			}

			public void RunEditAutoNormalizeModeCommand()
			{
				EditorManager.mAutoNormalize = !EditorManager.mAutoNormalize;
				parent.form.menuEditAutoNormalizeMode.Checked = EditorManager.mAutoNormalize;
			}
		}
	}
}
