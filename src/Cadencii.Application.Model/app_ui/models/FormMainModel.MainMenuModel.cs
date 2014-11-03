using System;
using cadencii.core;
using System.Collections.Generic;
using System.IO;


using cadencii.vsq;

namespace cadencii
{
	public partial class FormMainModel
	{

		public class MainMenuModel
		{
			FormMainModel parent;

			public MainMenuModel (FormMainModel parent)
			{
				this.parent = parent;
			}

			public void RunFileNewCommand ()
			{
				if (!parent.DirtyCheck()) {
					return;
				}
				EditorManager.Selected = (1);
				VsqFileEx vsq = new VsqFileEx(ApplicationGlobal.appConfig.DefaultSingerName, 1, 4, 4, 500000);

				RendererKind kind = ApplicationGlobal.appConfig.DefaultSynthesizer;
				string renderer = kind.getVersionString();
				List<VsqID> singers = MusicManager.getSingerListFromRendererKind(kind);
				vsq.Track[1].changeRenderer(renderer, singers);

				EditorManager.setVsqFile(vsq);
				parent.ClearExistingData();
				for (int i = 0; i < EditorManager.LastRenderedStatus.Length; i++) {
					EditorManager.LastRenderedStatus[i] = null;
				}
				parent.form.setEdited(false);
				EditorManager.MixerWindow.updateStatus();
				parent.ClearTempWave();

				// キャッシュディレクトリのパスを、デフォルトに戻す
				ApplicationGlobal.setTempWaveDir(Path.Combine(ApplicationGlobal.getCadenciiTempDir(), ApplicationGlobal.getID()));

				parent.form.updateDrawObjectList();
				parent.form.refreshScreen();
			}

			public void RunFileOpenCommand ()
			{
				if (!parent.DirtyCheck()) {
					return;
				}
				string dir = ApplicationGlobal.appConfig.getLastUsedPathIn("xvsq");
				parent.form.openXmlVsqDialog.SetSelectedFile(dir);
				var dialog_result = DialogManager.showModalFileDialog(parent.form.openXmlVsqDialog, true, this);
				if (dialog_result != cadencii.java.awt.DialogResult.OK) {
					return;
				}
				if (EditorManager.isPlaying()) {
					EditorManager.setPlaying(false, this);
				}
				string file = parent.form.openXmlVsqDialog.FileName;
				ApplicationGlobal.appConfig.setLastUsedPathIn(file, ".xvsq");
				if (parent.OpenVsqCor(file)) {
					DialogManager.showMessageBox(
						_("Invalid XVSQ file"),
						_("Error"),
						cadencii.Dialog.MSGBOX_DEFAULT_OPTION,
						cadencii.Dialog.MSGBOX_WARNING_MESSAGE);
					return;
				}
				parent.ClearExistingData();

				parent.form.setEdited(false);
				EditorManager.MixerWindow.updateStatus();
				parent.ClearTempWave();
				parent.form.updateDrawObjectList();
				parent.form.refreshScreen();
			}

			public void ShowRecentFileInMenuItem (RecentFileMenuItem item)
			{
				string filename = item.getFilePath();
				if (!parent.DirtyCheck()) {
					return;
				}
				parent.OpenVsqCor(filename);
				parent.ClearExistingData();
				EditorManager.MixerWindow.updateStatus();
				parent.ClearTempWave();
				parent.form.updateDrawObjectList();
				parent.form.refreshScreen();
			}

			public void UpdateStatusBarLabelByRecentFile (RecentFileMenuItem item)
			{
				parent.form.statusLabel.Text = item.ToolTipText;
			}
		}
	}
}

