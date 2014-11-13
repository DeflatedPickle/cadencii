using System;
using System.IO;
using Cadencii.Utilities;
using cadencii;
using Cadencii.Application.Media;
using Cadencii.Application.Forms;

namespace Cadencii.Application.Models
{
	public partial class FormMainModel
	{
		public class ScriptMenuModel
		{
			readonly FormMainModel parent;

			public ScriptMenuModel (FormMainModel parent)
			{
				this.parent = parent;
			}

			//BOOKMARK: menuScript
			#region menuScript
			public void RunScriptUpdateCommand ()
			{
				#if ENABLE_SCRIPT
				parent.UpdateScriptShortcut();
				parent.form.applyShortcut();
				#endif
			}
			#endregion

			#if ENABLE_SCRIPT
			public void RunScriptMenuItemCommand(string id)
			{
			#if DEBUG
				Logger.StdOut("FormMain#handleScriptMenuItem_Click");
			#endif
				try {
					string dir = EditorManager.getScriptPath();
			#if DEBUG
					Logger.StdOut("FormMain#handleScriptMenuItem_Click; id=" + id);
			#endif
					string script_file = Path.Combine(dir, id);
					if (ScriptServer.getTimestamp(id) != PortUtil.getFileLastModified(script_file)) {
						ScriptServer.reload(id);
					}
					if (ScriptServer.isAvailable(id)) {
						if (ScriptServer.invokeScript(id, MusicManager.getVsqFile(), (p1,p2,p3,p4) => DialogManager.ShowMessageBox (p1, p2, p3, p4))) {
							parent.form.setEdited(true);
							parent.form.updateDrawObjectList();
							int selected = EditorManager.Selected;
			#if DEBUG
							Logger.StdOut("FormMain#handleScriptMenuItem_Click; ScriptServer.invokeScript has returned TRUE");
			#endif
							EditorManager.itemSelection.updateSelectedEventInstance();
							EditorManager.propertyPanel.updateValue(selected);
							parent.form.refreshScreen();
						}
					} else {
						FormCompileResult dlg = null;
						try {
							dlg = ApplicationUIHost.Create<FormCompileResult>(_("Failed loading script."), ScriptServer.getCompileMessage(id));
							DialogManager.ShowModalDialog(dlg, parent.form);
						} catch (Exception ex) {
							Logger.write(GetType () + ".handleScriptMenuItem_Click; ex=" + ex + "\n");
						} finally {
							if (dlg != null) {
								try {
									dlg.Close();
								} catch (Exception ex2) {
									Logger.write(GetType () + ".handleScriptMenuItem_Click; ex=" + ex2 + "\n");
								}
							}
						}
					}
				} catch (Exception ex3) {
					Logger.write(GetType () + ".handleScriptMenuItem_Click; ex=" + ex3 + "\n");
			#if DEBUG
					Logger.StdOut("EditorManager#dd_run_Click; ex3=" + ex3);
			#endif
				}
			}
			#endif
		}
	}
}
