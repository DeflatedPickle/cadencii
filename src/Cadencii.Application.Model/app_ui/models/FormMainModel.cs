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

		/// <summary>
		/// editorConfigのRecentFilesを元に，menuFileRecentのドロップダウンアイテムを更新します
		/// </summary>
		public void UpdateRecentFileMenu()
		{
			int added = 0;
			form.menuFileRecent.DropDownItems.Clear();
			if (EditorManager.editorConfig.RecentFiles != null) {
				for (int i = 0; i < EditorManager.editorConfig.RecentFiles.Count; i++) {
					string item = EditorManager.editorConfig.RecentFiles[i];
					if (item == null) {
						continue;
					}
					if (item != "") {
						string short_name = PortUtil.getFileName(item);
						bool available = System.IO.File.Exists(item);
						var itm = ApplicationUIHost.Create<RecentFileMenuItem> (item);
						itm.Text = short_name;
						string tooltip = "";
						if (!available) {
							tooltip = _("[file not found]") + " ";
						}
						tooltip += item;
						itm.ToolTipText = tooltip;
						itm.Enabled = available;
						itm.Click += (o, e) => MainMenu.ShowRecentFileInMenuItem (itm);
						itm.MouseEnter += (o, e) => MainMenu.UpdateStatusBarLabelByRecentFile (itm);
						form.menuFileRecent.DropDownItems.Add(itm);
						added++;
					}
				}
			} else {
				EditorManager.editorConfig.pushRecentFiles("");
			}
			form.menuFileRecent.DropDownItems.Add(ApplicationUIHost.Create<UiToolStripSeparator> ());
			form.menuFileRecent.DropDownItems.Add(form.menuFileRecentClear);
			form.menuFileRecent.Enabled = true;
		}

		/// <summary>
		/// xvsqファイルを開きます
		/// </summary>
		/// <returns>ファイルを開くのに成功した場合trueを，それ以外はfalseを返します</returns>
		public bool OpenVsqCor(string file)
		{
			if (EditorManager.readVsq(file)) {
				return true;
			}
			if (MusicManager.getVsqFile().Track.Count >= 2) {
				form.updateScrollRangeHorizontal();
			}
			EditorManager.editorConfig.pushRecentFiles(file);
			UpdateRecentFileMenu();
			form.setEdited(false);
			EditorManager.editHistory.clear();
			EditorManager.MixerWindow.updateStatus();

			// キャッシュwaveなどの処理
			if (ApplicationGlobal.appConfig.UseProjectCache) {
				#region キャッシュディレクトリの処理
				VsqFileEx vsq = MusicManager.getVsqFile();
				string cacheDir = vsq.cacheDir; // xvsqに保存されていたキャッシュのディレクトリ
				string dir = PortUtil.getDirectoryName(file);
				string name = PortUtil.getFileNameWithoutExtension(file);
				string estimatedCacheDir = Path.Combine(dir, name + ".cadencii"); // ファイル名から推測されるキャッシュディレクトリ
				if (cacheDir == null) {
					cacheDir = "";
				}
				if (cacheDir != "" &&
					Directory.Exists(cacheDir) &&
					estimatedCacheDir != "" &&
					cacheDir != estimatedCacheDir) {
					// ファイル名から推測されるキャッシュディレクトリ名と
					// xvsqに指定されているキャッシュディレクトリと異なる場合
					// cacheDirの必要な部分をestimatedCacheDirに移す

					// estimatedCacheDirが存在しない場合、新しく作る
					#if DEBUG
					sout.println("FormMain#openVsqCor;fsys.isDirectoryExists( estimatedCacheDir )=" + Directory.Exists(estimatedCacheDir));
					#endif
					if (!Directory.Exists(estimatedCacheDir)) {
						try {
							PortUtil.createDirectory(estimatedCacheDir);
						} catch (Exception ex) {
							Logger.write(GetType () + ".openVsqCor; ex=" + ex + "\n");
							serr.println("FormMain#openVsqCor; ex=" + ex);
							DialogManager.showMessageBox(PortUtil.formatMessage(_("cannot create cache directory: '{0}'"), estimatedCacheDir),
								_("Info."),
								cadencii.java.awt.AwtHost.OK_OPTION,
								cadencii.Dialog.MSGBOX_INFORMATION_MESSAGE);
							return true;
						}
					}

					// ファイルを移す
					for (int i = 1; i < vsq.Track.Count; i++) {
						string wavFrom = Path.Combine(cacheDir, i + ".wav");
						string xmlFrom = Path.Combine(cacheDir, i + ".xml");

						string wavTo = Path.Combine(estimatedCacheDir, i + ".wav");
						string xmlTo = Path.Combine(estimatedCacheDir, i + ".xml");
						if (System.IO.File.Exists(wavFrom)) {
							try {
								PortUtil.moveFile(wavFrom, wavTo);
							} catch (Exception ex) {
								Logger.write(GetType () + ".openVsqCor; ex=" + ex + "\n");
								serr.println("FormMain#openVsqCor; ex=" + ex);
							}
						}
						if (System.IO.File.Exists(xmlFrom)) {
							try {
								PortUtil.moveFile(xmlFrom, xmlTo);
							} catch (Exception ex) {
								Logger.write(GetType () + ".openVsqCor; ex=" + ex + "\n");
								serr.println("FormMain#openVsqCor; ex=" + ex);
							}
						}
					}
				}
				cacheDir = estimatedCacheDir;

				// キャッシュが無かったら作成
				if (!Directory.Exists(cacheDir)) {
					try {
						PortUtil.createDirectory(cacheDir);
					} catch (Exception ex) {
						Logger.write(GetType () + ".openVsqCor; ex=" + ex + "\n");
						serr.println("FormMain#openVsqCor; ex=" + ex);
						DialogManager.showMessageBox(PortUtil.formatMessage(_("cannot create cache directory: '{0}'"), estimatedCacheDir),
							_("Info."),
							cadencii.java.awt.AwtHost.OK_OPTION,
							cadencii.Dialog.MSGBOX_INFORMATION_MESSAGE);
						return true;
					}
				}

				// RenderedStatusを読み込む
				for (int i = 1; i < vsq.Track.Count; i++) {
					EditorManager.deserializeRenderingStatus(cacheDir, i);
				}

				// キャッシュ内のwavを、waveViewに読み込む
				form.waveView.unloadAll();
				for (int i = 1; i < vsq.Track.Count; i++) {
					string wav = Path.Combine(cacheDir, i + ".wav");
					#if DEBUG
					sout.println("FormMain#openVsqCor; wav=" + wav + "; isExists=" + System.IO.File.Exists(wav));
					#endif
					if (!System.IO.File.Exists(wav)) {
						continue;
					}
					form.waveView.load(i - 1, wav);
				}

				// 一時ディレクトリを、cachedirに変更
				ApplicationGlobal.setTempWaveDir(cacheDir);
				#endregion
			}
			return false;
		}
	}
}

