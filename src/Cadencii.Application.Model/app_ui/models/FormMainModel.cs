using System;
using cadencii.java.awt;
using cadencii.core;
using System.Collections.Generic;
using cadencii.vsq;
using System.IO;
using System.Threading;
using cadencii.apputil;

namespace cadencii
{
	public partial class FormMainModel
	{
		static string _(string id)
		{
			return Messaging.getMessage(id);
		}

		readonly UiFormMain form;

		public FormMainModel (UiFormMain form)
		{
			this.form = form;
			MainMenu = new MainMenuModel (this);
		}
			
		public MainMenuModel MainMenu { get; private set; }

		public static void TrackSelector_MouseClick (UiFormMain window, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) {
				if (EditorManager.keyWidth < e.X && e.X < window.TrackSelector.Width) {
					if (window.TrackSelector.Height - TrackSelectorConsts.OFFSET_TRACK_TAB <= e.Y && e.Y <= window.TrackSelector.Height) {
						window.MenuTrackTab.Show (window.TrackSelector, e.X, e.Y);
					} else {
						window.MenuTrackSelector.Show (window.TrackSelector, e.X, e.Y);
					}
				}
			}
		}

		/// <summary>
		/// 現在の編集データを全て破棄する。DirtyCheckは行われない。
		/// </summary>
		public void ClearExistingData()
		{
			EditorManager.editHistory.clear();
			EditorManager.itemSelection.clearBezier();
			EditorManager.itemSelection.clearEvent();
			EditorManager.itemSelection.clearTempo();
			EditorManager.itemSelection.clearTimesig();
			if (EditorManager.isPlaying()) {
				EditorManager.setPlaying(false, this);
			}
			form.waveView.unloadAll();
		}

		public void ClearTempWave()
		{
			string tmppath = Path.Combine(ApplicationGlobal.getCadenciiTempDir(), ApplicationGlobal.getID());
			if (!Directory.Exists(tmppath)) {
				return;
			}

			// 今回このPCが起動されるよりも以前に，Cadenciiが残したデータを削除する
			//TODO: システムカウンタは約49日でリセットされてしまい，厳密には実装できないようなので，保留．

			// このFormMainのインスタンスが使用したデータを消去する
			for (int i = 1; i <= ApplicationGlobal.MAX_NUM_TRACK; i++) {
				string file = Path.Combine(tmppath, i + ".wav");
				if (System.IO.File.Exists(file)) {
					for (int error = 0; error < 100; error++) {
						try {
							PortUtil.deleteFile(file);
							break;
						} catch (Exception ex) {
							Logger.write(GetType () + ".clearTempWave; ex=" + ex + "\n");
							#if DEBUG
							cadencii.core2.debug.push_log("FormMain+ClearTempWave()");
							cadencii.core2.debug.push_log("    ex=" + ex.ToString());
							cadencii.core2.debug.push_log("    error_count=" + error);
							#endif

							Thread.Sleep(100);
						}
					}
				}
			}
			string whd = Path.Combine(tmppath, UtauWaveGenerator.FILEBASE + ".whd");
			if (System.IO.File.Exists(whd)) {
				try {
					PortUtil.deleteFile(whd);
				} catch (Exception ex) {
					Logger.write(GetType () + ".clearTempWave; ex=" + ex + "\n");
				}
			}
			string dat = Path.Combine(tmppath, UtauWaveGenerator.FILEBASE + ".dat");
			if (System.IO.File.Exists(dat)) {
				try {
					PortUtil.deleteFile(dat);
				} catch (Exception ex) {
					Logger.write(GetType () + ".clearTempWave; ex=" + ex + "\n");
				}
			}
		}

		/// <summary>
		/// 保存されていない編集内容があるかどうかチェックし、必要なら確認ダイアログを出す。
		/// </summary>
		/// <returns>保存されていない保存内容などない場合、または、保存する必要がある場合で（保存しなくてよいと指定された場合または保存が行われた場合）にtrueを返す</returns>
		public bool DirtyCheck()
		{
			if (form.mEdited) {
				string file = MusicManager.getFileName();
				if (file == "") {
					file = "Untitled";
				} else {
					file = PortUtil.getFileName(file);
				}
				var dr = DialogManager.showMessageBox(_("Save this sequence?"),
					_("Affirmation"),
					cadencii.Dialog.MSGBOX_YES_NO_CANCEL_OPTION,
					cadencii.Dialog.MSGBOX_QUESTION_MESSAGE);
				if (dr == cadencii.java.awt.DialogResult.Yes) {
					if (MusicManager.getFileName() == "") {
						var dr2 = DialogManager.showModalFileDialog(form.saveXmlVsqDialog, false, this);
						if (dr2 == cadencii.java.awt.DialogResult.OK) {
							string sf = form.saveXmlVsqDialog.FileName;
							EditorManager.saveTo(sf);
							return true;
						} else {
							return false;
						}
					} else {
						EditorManager.saveTo(MusicManager.getFileName());
						return true;
					}
				} else if (dr == cadencii.java.awt.DialogResult.No) {
					return true;
				} else {
					return false;
				}
			} else {
				return true;
			}
		}
	}
}

