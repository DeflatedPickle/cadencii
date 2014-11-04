using System;
using cadencii.java.awt;
using cadencii.core;
using System.Collections.Generic;
using cadencii.vsq;
using System.IO;
using System.Threading;
using cadencii.apputil;
using cadencii.utau;
using cadencii.java.util;

namespace cadencii
{
	public partial class FormMainModel
	{
		public const string ApplicationName = "Cadencii";
		/// <summary>
		/// splitContainer*で使用するSplitterWidthプロパティの値
		/// </summary>
		public const int _SPL_SPLITTER_WIDTH = 4;

		static string _(string id)
		{
			return Messaging.getMessage(id);
		}

		readonly UiFormMain form;

		public FormMainModel (UiFormMain form)
		{
			this.form = form;
			FileMenu = new FileMenuModel (this);
			EditMenu = new EditMenuModel (this);
			VisualMenu = new VisualMenuModel (this);
			LyricMenu = new LyricMenuModel (this);
			SettingsMenu = new SettingsMenuModel (this);
		}
			
		public FileMenuModel FileMenu { get; private set; }
		public EditMenuModel EditMenu { get; private set; }
		public VisualMenuModel VisualMenu { get; private set; }
		public LyricMenuModel LyricMenu { get; private set; }
		public SettingsMenuModel SettingsMenu { get; private set; }

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

		public static int Quantize(int clock, int unit)
		{
			int odd = clock % unit;
			int new_clock = clock - odd;
			if (odd > unit / 2) {
				new_clock += unit;
			}
			return new_clock;
		}

		public Point GetFormPreferedLocation(UiForm dlg)
		{
			return GetFormPreferedLocation(dlg.Width, dlg.Height);
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
						itm.Click += (o, e) => FileMenu.ShowRecentFileInMenuItem (itm);
						itm.MouseEnter += (o, e) => FileMenu.UpdateStatusBarLabelByRecentFile (itm);
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

		/// <summary>
		/// 指定した歌手とリサンプラーについて，設定値に登録されていないものだったら登録する．
		/// </summary>
		/// <param name="resampler_path"></param>
		/// <param name="singer_path"></param>
		private void CheckUnknownResamplerAndSinger(ByRef<string> resampler_path, ByRef<string> singer_path)
		{
			string utau = Utility.getExecutingUtau();
			string utau_dir = "";
			if (utau != "") {
				utau_dir = PortUtil.getDirectoryName(utau);
			}

			// 可能なら，VOICEの文字列を置換する
			string search = "%VOICE%";
			if (singer_path.value.StartsWith(search) && singer_path.value.Length > search.Length) {
				singer_path.value = singer_path.value.Substring(search.Length);
				singer_path.value = Path.Combine(Path.Combine(utau_dir, "voice"), singer_path.value);
			}

			// 歌手はknownかunknownか？
			// 歌手指定が知らない歌手だった場合に，ダイアログを出すかどうか
			bool check_unknown_singer = false;
			if (System.IO.File.Exists(Path.Combine(singer_path.value, "oto.ini"))) {
				// oto.iniが存在する場合
				// editorConfigに入っていない場合に，ダイアログを出す
				bool found = false;
				for (int i = 0; i < ApplicationGlobal.appConfig.UtauSingers.Count; i++) {
					SingerConfig sc = ApplicationGlobal.appConfig.UtauSingers[i];
					if (sc == null) {
						continue;
					}
					if (sc.VOICEIDSTR == singer_path.value) {
						found = true;
						break;
					}
				}
				check_unknown_singer = !found;
			}

			// リサンプラーが知っているやつかどうか
			bool check_unknwon_resampler = false;
			#if DEBUG
			sout.println("FormMain#checkUnknownResamplerAndSinger; resampler_path.value=" + resampler_path.value);
			#endif
			string resampler_dir = PortUtil.getDirectoryName(resampler_path.value);
			if (resampler_dir == "") {
				// ディレクトリが空欄なので，UTAUのデフォルトのリサンプラー指定である
				resampler_path.value = Path.Combine(utau_dir, resampler_path.value);
				resampler_dir = PortUtil.getDirectoryName(resampler_path.value);
			}
			if (resampler_dir != "" && System.IO.File.Exists(resampler_path.value)) {
				bool found = false;
				for (int i = 0; i < ApplicationGlobal.appConfig.getResamplerCount(); i++) {
					string resampler = ApplicationGlobal.appConfig.getResamplerAt(i);
					if (resampler == resampler_path.value) {
						found = true;
						break;
					}
				}
				check_unknwon_resampler = !found;
			}

			// unknownな歌手やリサンプラーが発見された場合.
			// 登録するかどうか問い合わせるダイアログを出す
			FormCheckUnknownSingerAndResampler dialog = null;
			try {
				if (check_unknown_singer || check_unknwon_resampler) {
					dialog = ApplicationUIHost.Create<FormCheckUnknownSingerAndResampler>(singer_path.value, check_unknown_singer, resampler_path.value, check_unknwon_resampler);
					dialog.Location = GetFormPreferedLocation(dialog.Width, dialog.Height);
					var dr = DialogManager.showModalDialog(dialog, this);
					if (dr != 1) {
						return;
					}

					// 登録する
					// リサンプラー
					if (dialog.isResamplerChecked()) {
						string path = dialog.getResamplerPath();
						if (System.IO.File.Exists(path)) {
							ApplicationGlobal.appConfig.addResampler(path);
						}
					}
					// 歌手
					if (dialog.isSingerChecked()) {
						string path = dialog.getSingerPath();
						if (Directory.Exists(path)) {
							SingerConfig sc = new SingerConfig();
							Utau.readUtauSingerConfig(path, sc);
							ApplicationGlobal.appConfig.UtauSingers.Add(sc);
						}
						EditorManager.reloadUtauVoiceDB();
					}
				}
			} catch (Exception ex) {
				Logger.write(GetType () + ".checkUnknownResamplerAndSinger; ex=" + ex + "\n");
			} finally {
				if (dialog != null) {
					try {
						dialog.Close();
					} catch (Exception ex2) {
					}
				}
			}
		}

		/// <summary>
		/// フォームをマウス位置に出す場合に推奨されるフォーム位置を計算します
		/// </summary>
		/// <param name="dlg"></param>
		/// <returns></returns>
		public Point GetFormPreferedLocation(int dialogWidth, int dialogHeight)
		{
			Point mouse = cadencii.core2.PortUtil.getMousePosition();
			Rectangle rcScreen = cadencii.core2.PortUtil.getWorkingArea(this);
			int top = mouse.Y - dialogHeight / 2;
			if (top + dialogHeight > rcScreen.Y + rcScreen.Height) {
				// ダイアログの下端が隠れる場合、位置をずらす
				top = rcScreen.Y + rcScreen.Height - dialogHeight;
			}
			if (top < rcScreen.Y) {
				// ダイアログの上端が隠れる場合、位置をずらす
				top = rcScreen.Y;
			}
			int left = mouse.X - dialogWidth / 2;
			if (left + dialogWidth > rcScreen.X + rcScreen.Width) {
				// ダイアログの右端が隠れる場合，位置をずらす
				left = rcScreen.X + rcScreen.Width - dialogWidth;
			}
			if (left < rcScreen.X) {
				// ダイアログの左端が隠れる場合，位置をずらす
				left = rcScreen.X;
			}
			return new Point(left, top);
		}

		/// <summary>
		/// VsqEvent, VsqBPList, BezierCurvesの全てのクロックを、tempoに格納されているテンポテーブルに
		/// 合致するようにシフトします．ただし，このメソッド内ではtargetのテンポテーブルは変更せず，クロック値だけが変更される．
		/// </summary>
		/// <param name="work"></param>
		/// <param name="tempo"></param>
		public static void ShiftClockToMatchWith(VsqFileEx target, VsqFile tempo, double shift_seconds)
		{
			// テンポをリプレースする場合。
			// まずクロック値を、リプレース後のモノに置き換え
			for (int track = 1; track < target.Track.Count; track++) {
				// ノート・歌手イベントをシフト
				for (Iterator<VsqEvent> itr = target.Track[track].getEventIterator(); itr.hasNext(); ) {
					VsqEvent item = itr.next();
					if (item.ID.type == VsqIDType.Singer && item.Clock == 0) {
						continue;
					}
					int clock = item.Clock;
					double sec_start = target.getSecFromClock(clock) + shift_seconds;
					double sec_end = target.getSecFromClock(clock + item.ID.getLength()) + shift_seconds;
					int clock_start = (int)tempo.getClockFromSec(sec_start);
					int clock_end = (int)tempo.getClockFromSec(sec_end);
					item.Clock = clock_start;
					item.ID.setLength(clock_end - clock_start);
					if (item.ID.VibratoHandle != null) {
						double sec_vib_start = target.getSecFromClock(clock + item.ID.VibratoDelay) + shift_seconds;
						int clock_vib_start = (int)tempo.getClockFromSec(sec_vib_start);
						item.ID.VibratoDelay = clock_vib_start - clock_start;
						item.ID.VibratoHandle.setLength(clock_end - clock_vib_start);
					}
				}

				// コントロールカーブをシフト
				for (int j = 0; j < BezierCurves.CURVE_USAGE.Length; j++) {
					CurveType ct = BezierCurves.CURVE_USAGE[j];
					VsqBPList item = target.Track[track].getCurve(ct.getName());
					if (item == null) {
						continue;
					}
					VsqBPList repl = new VsqBPList(item.getName(), item.getDefault(), item.getMinimum(), item.getMaximum());
					for (int i = 0; i < item.size(); i++) {
						int clock = item.getKeyClock(i);
						int value = item.getElement(i);
						double sec = target.getSecFromClock(clock) + shift_seconds;
						if (sec >= 0) {
							int clock_new = (int)tempo.getClockFromSec(sec);
							repl.add(clock_new, value);
						}
					}
					target.Track[track].setCurve(ct.getName(), repl);
				}

				// ベジエカーブをシフト
				for (int j = 0; j < BezierCurves.CURVE_USAGE.Length; j++) {
					CurveType ct = BezierCurves.CURVE_USAGE[j];
					List<BezierChain> list = target.AttachedCurves.get(track - 1).get(ct);
					if (list == null) {
						continue;
					}
					foreach (var chain in list) {
						foreach (var point in chain.points) {
							PointD bse = new PointD(tempo.getClockFromSec(target.getSecFromClock(point.getBase().getX()) + shift_seconds),
								point.getBase().getY());
							double rx = point.getBase().getX() + point.controlRight.getX();
							double new_rx = tempo.getClockFromSec(target.getSecFromClock(rx) + shift_seconds);
							PointD ctrl_r = new PointD(new_rx - bse.getX(), point.controlRight.getY());

							double lx = point.getBase().getX() + point.controlLeft.getX();
							double new_lx = tempo.getClockFromSec(target.getSecFromClock(lx) + shift_seconds);
							PointD ctrl_l = new PointD(new_lx - bse.getX(), point.controlLeft.getY());
							point.setBase(bse);
							point.controlLeft = ctrl_l;
							point.controlRight = ctrl_r;
						}
					}
				}
			}
		}

		/// <summary>
		/// 選択されている音符の表情を編集するためのダイアログを起動し、編集を行います。
		/// </summary>
		public void EditNoteExpressionProperty()
		{
			SelectedEventEntry item = EditorManager.itemSelection.getLastEvent();
			if (item == null) {
				return;
			}

			VsqEvent ev = item.original;
			SynthesizerType type = SynthesizerType.VOCALOID2;
			int selected = EditorManager.Selected;
			VsqFileEx vsq = MusicManager.getVsqFile();
			RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
			if (kind == RendererKind.VOCALOID1) {
				type = SynthesizerType.VOCALOID1;
			}
			FormNoteExpressionConfig dlg = null;
			try {
				dlg = ApplicationUIHost.Create<FormNoteExpressionConfig>(type, ev.ID.NoteHeadHandle);
				dlg.PMBendDepth = (ev.ID.PMBendDepth);
				dlg.PMBendLength = (ev.ID.PMBendLength);
				dlg.PMbPortamentoUse = (ev.ID.PMbPortamentoUse);
				dlg.DEMdecGainRate = (ev.ID.DEMdecGainRate);
				dlg.DEMaccent = (ev.ID.DEMaccent);

				dlg.Location = GetFormPreferedLocation(dlg.Width, dlg.Height);
				var dr = DialogManager.showModalDialog(dlg, this);
				if (dr == 1) {
					VsqEvent edited = (VsqEvent)ev.clone();
					edited.ID.PMBendDepth = dlg.PMBendDepth;
					edited.ID.PMBendLength = dlg.PMBendLength;
					edited.ID.PMbPortamentoUse = dlg.PMbPortamentoUse;
					edited.ID.DEMdecGainRate = dlg.DEMdecGainRate;
					edited.ID.DEMaccent = dlg.DEMaccent;
					edited.ID.NoteHeadHandle = dlg.EditedNoteHeadHandle;
					CadenciiCommand run = new CadenciiCommand(
						VsqCommand.generateCommandEventChangeIDContaints(selected, ev.InternalID, edited.ID));
					EditorManager.editHistory.register(vsq.executeCommand(run));
					form.setEdited(true);
					form.refreshScreen();
				}
			} catch (Exception ex) {
				Logger.write(GetType () + ".editNoteExpressionProperty; ex=" + ex + "\n");
			} finally {
				if (dlg != null) {
					try {
						dlg.Close();
					} catch (Exception ex2) {
						Logger.write(GetType () + ".editNoteExpressionProperty; ex=" + ex2 + "\n");
					}
				}
			}
		}

		/// <summary>
		/// 選択されている音符のビブラートを編集するためのダイアログを起動し、編集を行います。
		/// </summary>
		public void EditNoteVibratoProperty()
		{
			SelectedEventEntry item = EditorManager.itemSelection.getLastEvent();
			if (item == null) {
				return;
			}

			VsqEvent ev = item.original;
			int selected = EditorManager.Selected;
			VsqFileEx vsq = MusicManager.getVsqFile();
			RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
			SynthesizerType type = SynthesizerType.VOCALOID2;
			if (kind == RendererKind.VOCALOID1) {
				type = SynthesizerType.VOCALOID1;
			}
			FormVibratoConfig dlg = null;
			try {
				dlg = ApplicationUIHost.Create<FormVibratoConfig>(
					ev.ID.VibratoHandle,
					ev.ID.getLength(),
					ApplicationGlobal.appConfig.DefaultVibratoLength,
					type,
					ApplicationGlobal.appConfig.UseUserDefinedAutoVibratoType);
				dlg.Location = GetFormPreferedLocation(dlg);
				var dr = DialogManager.showModalDialog(dlg, this);
				if (dr == 1) {
					VsqEvent edited = (VsqEvent)ev.clone();
					if (dlg.getVibratoHandle() != null) {
						edited.ID.VibratoHandle = (VibratoHandle)dlg.getVibratoHandle().clone();
						//edited.ID.VibratoHandle.setStartDepth( ApplicationGlobal.appConfig.DefaultVibratoDepth );
						//edited.ID.VibratoHandle.setStartRate( ApplicationGlobal.appConfig.DefaultVibratoRate );
						edited.ID.VibratoDelay = ev.ID.getLength() - dlg.getVibratoHandle().getLength();
					} else {
						edited.ID.VibratoHandle = null;
					}
					CadenciiCommand run = new CadenciiCommand(
						VsqCommand.generateCommandEventChangeIDContaints(selected, ev.InternalID, edited.ID));
					EditorManager.editHistory.register(vsq.executeCommand(run));
					form.setEdited(true);
					form.refreshScreen();
				}
			} catch (Exception ex) {
				Logger.write(GetType () + ".editNoteVibratoProperty; ex=" + ex + "\n");
			} finally {
				if (dlg != null) {
					try {
						dlg.Close();
					} catch (Exception ex2) {
						Logger.write(GetType () + ".editNoteVibratoProperty; ex=" + ex2 + "\n");
					}
				}
			}
		}

		/// <summary>
		/// ミキサーダイアログの表示・非表示状態を更新します
		/// </summary>
		/// <param name="visible">表示状態にする場合true，そうでなければfalse</param>
		public void flipMixerDialogVisible(bool visible)
		{
			EditorManager.MixerWindow.Visible = visible;
			EditorManager.editorConfig.MixerVisible = visible;
			if (visible != form.menuVisualMixer.Checked) {
				form.menuVisualMixer.Checked = visible;
			}
		}

		/// <summary>
		/// アイコンパレットの表示・非表示状態を更新します
		/// </summary>
		public void flipIconPaletteVisible(bool visible)
		{
			EditorManager.iconPalette.Visible = visible;
			EditorManager.editorConfig.IconPaletteVisible = visible;
			if (visible != form.menuVisualIconPalette.Checked) {
				form.menuVisualIconPalette.Checked = visible;
			}
		}

		/// <summary>
		/// コントロールトラックの表示・非表示状態を更新します
		/// </summary>
		public void flipControlCurveVisible(bool visible)
		{
			form.TrackSelector.setCurveVisible(visible);
			if (visible) {
				form.splitContainer1.SplitterFixed = (false);
				form.splitContainer1.DividerSize = (FormMainModel._SPL_SPLITTER_WIDTH);
				form.splitContainer1.DividerLocation = (form.splitContainer1.Height - EditorManager.mLastTrackSelectorHeight - form.splitContainer1.DividerSize);
				form.splitContainer1.Panel2MinSize = (form.TrackSelector.getPreferredMinSize());
			} else {
				EditorManager.mLastTrackSelectorHeight = form.splitContainer1.Height - form.splitContainer1.DividerLocation - form.splitContainer1.DividerSize;
				form.splitContainer1.SplitterFixed = (true);
				form.splitContainer1.DividerSize = (0);
				int panel2height = TrackSelectorConsts.OFFSET_TRACK_TAB * 2;
				form.splitContainer1.DividerLocation = (form.splitContainer1.Height - panel2height - form.splitContainer1.DividerSize);
				form.splitContainer1.Panel2MinSize = (panel2height);
			}
			form.refreshScreen();
		}

	}
}

