using System;
using cadencii.core;
using System.Collections.Generic;
using System.IO;


using cadencii.vsq;
using cadencii.vsq.io;
using System.Text;
using System.Linq;
using cadencii.java.util;
using Cadencii.Gui;
using Cadencii.Utilities;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public partial class FormMainModel
	{
		public class FileMenuModel
		{
			readonly FormMainModel parent;

			public FileMenuModel (FormMainModel parent)
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
				var dialog_result = DialogManager.ShowModalFileDialog(parent.form.openXmlVsqDialog, true, parent.form);
				if (dialog_result != Cadencii.Gui.DialogResult.OK) {
					return;
				}
				if (EditorManager.isPlaying()) {
					EditorManager.setPlaying(false, parent.form);
				}
				string file = parent.form.openXmlVsqDialog.FileName;
				ApplicationGlobal.appConfig.setLastUsedPathIn(file, ".xvsq");
				if (parent.OpenVsqCor(file)) {
					DialogManager.ShowMessageBox(
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

			public void RunFileSaveCommand ()
			{
				for (int track = 1; track < MusicManager.getVsqFile().Track.Count; track++) {
					if (MusicManager.getVsqFile().Track[track].getEventCount() == 0) {
						DialogManager.ShowMessageBox(
							PortUtil.formatMessage(
								_("Invalid note data.\nTrack {0} : {1}\n\n-> Piano roll : Blank sequence."),
								track,
								MusicManager.getVsqFile().Track[track].getName()),
							FormMainModel.Consts.ApplicationName,
							cadencii.Dialog.MSGBOX_DEFAULT_OPTION,
							cadencii.Dialog.MSGBOX_WARNING_MESSAGE);
						return;
					}
				}
				string file = MusicManager.getFileName();
				if (file.Equals("")) {
					string last_file = ApplicationGlobal.appConfig.getLastUsedPathOut("xvsq");
					if (!last_file.Equals("")) {
						string dir = PortUtil.getDirectoryName(last_file);
						parent.form.saveXmlVsqDialog.SetSelectedFile(dir);
					}
					var dr = DialogManager.ShowModalFileDialog(parent.form.saveXmlVsqDialog, false, parent.form);
					if (dr == Cadencii.Gui.DialogResult.OK) {
						file = parent.form.saveXmlVsqDialog.FileName;
						ApplicationGlobal.appConfig.setLastUsedPathOut(file, ".xvsq");
					}
				}
				if (file != "") {
					EditorManager.saveTo(file);
					parent.UpdateRecentFileMenu();
					parent.form.setEdited(false);
				}
			}

			public void RunFileSaveNamedCommand ()
			{
				for (int track = 1; track < MusicManager.getVsqFile().Track.Count; track++) {
					if (MusicManager.getVsqFile().Track[track].getEventCount() == 0) {
						DialogManager.ShowMessageBox(
							PortUtil.formatMessage(
								_("Invalid note data.\nTrack {0} : {1}\n\n-> Piano roll : Blank sequence."), track, MusicManager.getVsqFile().Track[track].getName()
							),
							FormMainModel.Consts.ApplicationName,
							cadencii.Dialog.MSGBOX_DEFAULT_OPTION,
							cadencii.Dialog.MSGBOX_WARNING_MESSAGE);
						return;
					}
				}

				string dir = ApplicationGlobal.appConfig.getLastUsedPathOut("xvsq");
				parent.form.saveXmlVsqDialog.SetSelectedFile(dir);
				var dr = DialogManager.ShowModalFileDialog(parent.form.saveXmlVsqDialog, false, parent.form);
				if (dr == Cadencii.Gui.DialogResult.OK) {
					string file = parent.form.saveXmlVsqDialog.FileName;
					ApplicationGlobal.appConfig.setLastUsedPathOut(file, ".xvsq");
					EditorManager.saveTo(file);
					parent.UpdateRecentFileMenu();
					parent.form.setEdited(false);
				}
			}

			public void RunFileOpenVsqCommand()
			{
				if (!parent.DirtyCheck()) {
					return;
				}

				string[] filters = parent.form.openMidiDialog.Filter.Split('|');
				int filter_index = -1;
				string filter = "";
				foreach (string f in filters) {
					++filter_index;
					if (f.EndsWith(EditorManager.editorConfig.LastUsedExtension)) {
						break;
					}
				}

				parent.form.openMidiDialog.FilterIndex = filter_index;
				string dir = ApplicationGlobal.appConfig.getLastUsedPathIn(filter);
				parent.form.openMidiDialog.SetSelectedFile(dir);
				var dialog_result = DialogManager.ShowModalFileDialog(parent.form.openMidiDialog, true, parent.form);
				string ext = ".vsq";
				if (dialog_result == Cadencii.Gui.DialogResult.OK) {
					#if DEBUG
					CDebug.WriteLine("openMidiDialog.Filter=" + parent.form.openMidiDialog.Filter);
					#endif
					string selected_filter = parent.form.openMidiDialog.SelectedFilter();
					if (selected_filter.EndsWith(".mid")) {
						EditorManager.editorConfig.LastUsedExtension = ".mid";
						ext = ".mid";
					} else if (selected_filter.EndsWith(".vsq")) {
						EditorManager.editorConfig.LastUsedExtension = ".vsq";
						ext = ".vsq";
					} else if (selected_filter.EndsWith(".vsqx")) {
						EditorManager.editorConfig.LastUsedExtension = ".vsqx";
						ext = ".vsqx";
					}
				} else {
					return;
				}
				try {
					string filename = parent.form.openMidiDialog.FileName;
					string actualReadFile = filename;
					bool isVsqx = filename.EndsWith(".vsqx");
					if (isVsqx) {
						actualReadFile = PortUtil.createTempFile();
						VsqFile temporarySequence = VsqxReader.readFromVsqx(filename);
						temporarySequence.write(actualReadFile);
					}
					VsqFileEx vsq = new VsqFileEx(actualReadFile, "Shift_JIS");
					if (isVsqx) {
						PortUtil.deleteFile(actualReadFile);
					}
					ApplicationGlobal.appConfig.setLastUsedPathIn(filename, ext);
					EditorManager.setVsqFile(vsq);
				} catch (Exception ex) {
					Logger.write(GetType () + ".menuFileOpenVsq_Click; ex=" + ex + "\n");
					#if DEBUG
					Logger.StdOut("FormMain#menuFileOpenVsq_Click; ex=" + ex);
					#endif
					DialogManager.ShowMessageBox(
						_("Invalid VSQ/VOCALOID MIDI file"),
						_("Error"),
						cadencii.Dialog.MSGBOX_DEFAULT_OPTION,
						cadencii.Dialog.MSGBOX_WARNING_MESSAGE);
					return;
				}
				EditorManager.Selected = (1);
				parent.ClearExistingData();
				parent.form.setEdited(true);
				EditorManager.MixerWindow.updateStatus();
				parent.ClearTempWave();
				parent.form.updateDrawObjectList();
				parent.form.refreshScreen();
			}

			public void RunFileOpenUstCommand()
			{
				if (!parent.DirtyCheck()) {
					return;
				}

				string dir = ApplicationGlobal.appConfig.getLastUsedPathIn("ust");
				parent.form.openUstDialog.SetSelectedFile(dir);
				var dialog_result = DialogManager.ShowModalFileDialog(parent.form.openUstDialog, true, parent.form);

				if (dialog_result != Cadencii.Gui.DialogResult.OK) {
					return;
				}

				try {
					string filename = parent.form.openUstDialog.FileName;
					ApplicationGlobal.appConfig.setLastUsedPathIn(filename, ".ust");

					// ust読み込み
					UstFile ust = new UstFile(filename);

					// vsqに変換
					VsqFileEx vsq = new VsqFileEx(ust);
					vsq.insertBlank(0, vsq.getPreMeasureClocks());

					// すべてのトラックの合成器指定をUTAUにする
					for (int i = 1; i < vsq.Track.Count; i++) {
						VsqTrack vsq_track = vsq.Track[i];
						VsqFileEx.setTrackRendererKind(vsq_track, RendererKind.UTAU);
					}

					// unknownな歌手やresamplerを何とかする
					ByRef<string> ref_resampler = new ByRef<string>(ust.getResampler());
					ByRef<string> ref_singer = new ByRef<string>(ust.getVoiceDir());
					parent.CheckUnknownResamplerAndSinger(ref_resampler, ref_singer);

					// 歌手変更を何とかする
					int program = 0;
					for (int i = 0; i < ApplicationGlobal.appConfig.UtauSingers.Count; i++) {
						SingerConfig sc = ApplicationGlobal.appConfig.UtauSingers[i];
						if (sc == null) {
							continue;
						}
						if (sc.VOICEIDSTR == ref_singer.value) {
							program = i;
							break;
						}
					}
					// 歌手変更のテンプレートを作成
					VsqID singer_id = Utility.getSingerID(RendererKind.UTAU, program, 0);
					if (singer_id == null) {
						singer_id = new VsqID();
						singer_id.type = VsqIDType.Singer;
						singer_id.IconHandle = new IconHandle();
						singer_id.IconHandle.Program = program;
						singer_id.IconHandle.IconID = "$0401" + PortUtil.toHexString(0, 4);
					}
					// トラックの歌手変更イベントをすべて置き換える
					for (int i = 1; i < vsq.Track.Count; i++) {
						VsqTrack vsq_track = vsq.Track[i];
						int c = vsq_track.getEventCount();
						for (int j = 0; j < c; j++) {
							VsqEvent itemj = vsq_track.getEvent(j);
							if (itemj.ID.type == VsqIDType.Singer) {
								itemj.ID = (VsqID)singer_id.clone();
							}
						}
					}

					// resamplerUsedを更新(可能なら)
					for (int j = 1; j < vsq.Track.Count; j++) {
						VsqTrack vsq_track = vsq.Track[j];
						for (int i = 0; i < ApplicationGlobal.appConfig.getResamplerCount(); i++) {
							string resampler = ApplicationGlobal.appConfig.getResamplerAt(i);
							if (resampler == ref_resampler.value) {
								VsqFileEx.setTrackResamplerUsed(vsq_track, i);
								break;
							}
						}
					}

					parent.ClearExistingData();
					EditorManager.setVsqFile(vsq);
					parent.form.setEdited(true);
					EditorManager.MixerWindow.updateStatus();
					parent.ClearTempWave();
					parent.form.updateDrawObjectList();
					parent.form.refreshScreen();

				} catch (Exception ex) {
					Logger.write(GetType () + ".menuFileOpenUst_Click; ex=" + ex + "\n");
					#if DEBUG
					Logger.StdOut("FormMain#menuFileOpenUst_Click; ex=" + ex);
					#endif
				}
			}

			public void RunFileImportUstCommand()
			{
				UiOpenFileDialog dialog = null;
				try {
					// 読み込むファイルを選ぶ
					string dir = ApplicationGlobal.appConfig.getLastUsedPathIn ("ust");
					dialog = ApplicationUIHost.Create<UiOpenFileDialog> ();
					dialog.SetSelectedFile (dir);
					var dialog_result = DialogManager.ShowModalFileDialog (dialog, true, parent.form);
					if (dialog_result != Cadencii.Gui.DialogResult.OK) {
						return;
					}
					string file = dialog.FileName;
					ApplicationGlobal.appConfig.setLastUsedPathIn (file, ".ust");

					// ustを読み込む
					UstFile ust = new UstFile (file);

					// vsqに変換
					VsqFile vsq = new VsqFile (ust);
					vsq.insertBlank (0, vsq.getPreMeasureClocks ());

					// RendererKindをUTAUに指定
					for (int i = 1; i < vsq.Track.Count; i++) {
						VsqTrack vsq_track = vsq.Track [i];
						VsqFileEx.setTrackRendererKind (vsq_track, RendererKind.UTAU);
					}

					// unknownな歌手とresamplerを何とかする
					ByRef<string> ref_resampler = new ByRef<string> (ust.getResampler ());
					ByRef<string> ref_singer = new ByRef<string> (ust.getVoiceDir ());
					parent.CheckUnknownResamplerAndSinger (ref_resampler, ref_singer);

					// 歌手変更を何とかする
					int program = 0;
					for (int i = 0; i < ApplicationGlobal.appConfig.UtauSingers.Count; i++) {
						SingerConfig sc = ApplicationGlobal.appConfig.UtauSingers [i];
						if (sc == null) {
							continue;
						}
						if (sc.VOICEIDSTR == ref_singer.value) {
							program = i;
							break;
						}
					}
					// 歌手変更のテンプレートを作成
					VsqID singer_id = Utility.getSingerID (RendererKind.UTAU, program, 0);
					if (singer_id == null) {
						singer_id = new VsqID ();
						singer_id.type = VsqIDType.Singer;
						singer_id.IconHandle = new IconHandle ();
						singer_id.IconHandle.Program = program;
						singer_id.IconHandle.IconID = "$0401" + PortUtil.toHexString (0, 4);
					}
					// トラックの歌手変更イベントをすべて置き換える
					for (int i = 1; i < vsq.Track.Count; i++) {
						VsqTrack vsq_track = vsq.Track [i];
						int c = vsq_track.getEventCount ();
						for (int j = 0; j < c; j++) {
							VsqEvent itemj = vsq_track.getEvent (j);
							if (itemj.ID.type == VsqIDType.Singer) {
								itemj.ID = (VsqID)singer_id.clone ();
							}
						}
					}

					// resamplerUsedを更新(可能なら)
					for (int j = 1; j < vsq.Track.Count; j++) {
						VsqTrack vsq_track = vsq.Track [j];
						for (int i = 0; i < ApplicationGlobal.appConfig.getResamplerCount (); i++) {
							string resampler = ApplicationGlobal.appConfig.getResamplerAt (i);
							if (resampler == ref_resampler.value) {
								VsqFileEx.setTrackResamplerUsed (vsq_track, i);
								break;
							}
						}
					}

					// 読込先のvsqと，インポートするvsqではテンポテーブルがずれているので，
					// 読み込んだ方のvsqの内容を，現在のvsqと合致するように編集する
					VsqFileEx dst = (VsqFileEx)MusicManager.getVsqFile ().clone ();
					vsq.adjustClockToMatchWith (dst.TempoTable);

					// トラック数の上限になるまで挿入を実行
					int size = vsq.Track.Count;
					for (int i = 1; i < size; i++) {
						if (dst.Track.Count + 1 >= VsqFile.MAX_TRACKS + 1) {
							// トラック数の上限
							break;
						}
						dst.Track.Add (vsq.Track [i]);
						dst.AttachedCurves.add (new BezierCurves ());
						dst.Mixer.Slave.Add (new VsqMixerEntry ());
					}

					// コマンドを発行して実行
					CadenciiCommand run = VsqFileEx.generateCommandReplace (dst);
					EditorManager.editHistory.register (MusicManager.getVsqFile ().executeCommand (run));
					EditorManager.MixerWindow.updateStatus ();
					parent.form.setEdited (true);
					parent.form.refreshScreen (true);
				} catch (Exception ex) {
					Logger.write (GetType () + ".menuFileImportUst_Click; ex=" + ex + "\t");
				} finally {
					if (dialog != null) {
						try {
							dialog.Dispose ();
						} catch (Exception ex) {
						}
					}
				}
			}

			public void RunFileImportMidiCommand()
			{
				if (parent.form.mDialogMidiImportAndExport == null) {
					parent.form.mDialogMidiImportAndExport = ApplicationUIHost.Create<FormMidiImExport>();
				}
				var dlg = parent.form.mDialogMidiImportAndExport;
				dlg.listTrack.ClearItems();
				dlg.Mode = (FormMidiMode.IMPORT);

				string dir = ApplicationGlobal.appConfig.getLastUsedPathIn("mid");
				parent.form.openMidiDialog.SetSelectedFile(dir);
				var dialog_result = DialogManager.ShowModalFileDialog(parent.form.openMidiDialog, true, parent.form);

				if (dialog_result != Cadencii.Gui.DialogResult.OK) {
					return;
				}
				dlg.Location = parent.GetFormPreferedLocation(dlg.Width, dlg.Height);
				MidiFile mf = null;
				try {
					string filename = parent.form.openMidiDialog.FileName;
					ApplicationGlobal.appConfig.setLastUsedPathIn(filename, ".mid");
					mf = new MidiFile(filename);
				} catch (Exception ex) {
					Logger.write(GetType () + ".menuFileImportMidi_Click; ex=" + ex + "\n");
					DialogManager.ShowMessageBox(
						_("Invalid MIDI file."),
						_("Error"),
						cadencii.Dialog.MSGBOX_DEFAULT_OPTION,
						cadencii.Dialog.MSGBOX_WARNING_MESSAGE);
					return;
				}
				if (mf == null) {
					DialogManager.ShowMessageBox(
						_("Invalid MIDI file."),
						_("Error"),
						cadencii.Dialog.MSGBOX_DEFAULT_OPTION,
						cadencii.Dialog.MSGBOX_WARNING_MESSAGE);
					return;
				}
				int count = mf.getTrackCount();

				Func<int[], string> get_string_from_metatext = (buffer) => {
					var encoding_candidates = new List<Encoding>();
					encoding_candidates.Add(Encoding.GetEncoding("Shift_JIS"));
					encoding_candidates.Add(Encoding.Default);
					encoding_candidates.Add(Encoding.UTF8);
					encoding_candidates.AddRange(Encoding.GetEncodings().Select((encoding) => encoding.GetEncoding()));
					foreach (var encoding in encoding_candidates) {
						try {
							return encoding.GetString(buffer.Select((b) => (byte)(0xFF & b)).ToArray(), 0, buffer.Length);
						} catch {
							continue;
						}
					}
					return string.Empty;
				};

				for (int i = 0; i < count; i++) {
					int notes = 0;
					List<MidiEvent> events = mf.getMidiEventList(i);
					int events_count = events.Count;

					// トラック名を取得
					string track_name =
						events
							.Where((item) => item.firstByte == 0xff && item.data.Length >= 2 && item.data[0] == 0x03)
							.Select((item) => {
								int[] d = item.data.Skip(1).ToArray();
								return get_string_from_metatext(d);
							})
							.FirstOrDefault();

					// イベント数を数える
					for (int j = 0; j < events_count; j++) {
						MidiEvent item = events[j];
						if ((item.firstByte & 0xf0) == 0x90 && item.data.Length > 1 && item.data[1] > 0x00) {
							notes++;
						}
					}
					dlg.listTrack.AddRow(
						new string[] { i + "", track_name, notes + "" }, true);
				}

				var dr = DialogManager.ShowModalDialog(dlg, parent.form);
				if (dr != DialogResult.OK) {
					return;
				}

				bool secondBasis = dlg.isSecondBasis();
				int offsetClocks = dlg.getOffsetClocks();
				double offsetSeconds = dlg.getOffsetSeconds();
				bool importFromPremeasure = dlg.isPreMeasure();

				// インポートするしないにかかわらずテンポと拍子を取得
				VsqFileEx tempo = new VsqFileEx("Miku", 2, 4, 4, 500000); //テンポリスト用のVsqFile。テンポの部分のみ使用
				tempo.executeCommand(VsqCommand.generateCommandChangePreMeasure(0));
				bool tempo_added = false;
				bool timesig_added = false;
				tempo.TempoTable.Clear();
				tempo.TimesigTable.Clear();
				int mf_getTrackCount = mf.getTrackCount();
				for (int i = 0; i < mf_getTrackCount; i++) {
					List<MidiEvent> events = mf.getMidiEventList(i);
					bool t_tempo_added = false;   //第iトラックからテンポをインポートしたかどうか
					bool t_timesig_added = false; //第iトラックから拍子をインポートしたかどうか
					int last_timesig_clock = 0; // 最後に拍子変更を検出したゲートタイム
					int last_num = 4; // 最後に検出した拍子変更の分子
					int last_den = 4; // 最後に検出した拍子変更の分母
					int last_barcount = 0;
					int events_Count = events.Count;
					for (int j = 0; j < events_Count; j++) {
						MidiEvent itemj = events[j];
						if (!tempo_added && itemj.firstByte == 0xff && itemj.data.Length >= 4 && itemj.data[0] == 0x51) {
							bool contains_same_clock = false;
							int size = tempo.TempoTable.Count;
							// 同時刻のテンポ変更は、最初以外無視する
							for (int k = 0; k < size; k++) {
								if (tempo.TempoTable[k].Clock == itemj.clock) {
									contains_same_clock = true;
									break;
								}
							}
							if (!contains_same_clock) {
								int vtempo = itemj.data[1] << 16 | itemj.data[2] << 8 | itemj.data[3];
								tempo.TempoTable.Add(new TempoTableEntry((int)itemj.clock, vtempo, 0.0));
								t_tempo_added = true;
							}
						}
						if (!timesig_added && itemj.firstByte == 0xff && itemj.data.Length >= 5 && itemj.data[0] == 0x58) {
							int num = itemj.data[1];
							int den = 1;
							for (int k = 0; k < itemj.data[2]; k++) {
								den = den * 2;
							}
							int clock_per_bar = last_num * 480 * 4 / last_den;
							int barcount_at_itemj = last_barcount + ((int)itemj.clock - last_timesig_clock) / clock_per_bar;
							// 同時刻の拍子変更は、最初以外無視する
							int size = tempo.TimesigTable.Count;
							bool contains_same_clock = false;
							for (int k = 0; k < size; k++) {
								if (tempo.TimesigTable[k].Clock == itemj.clock) {
									contains_same_clock = true;
									break;
								}
							}
							if (!contains_same_clock) {
								tempo.TimesigTable.Add(new TimeSigTableEntry((int)itemj.clock, num, den, barcount_at_itemj));
								last_timesig_clock = (int)itemj.clock;
								last_den = den;
								last_num = num;
								last_barcount = barcount_at_itemj;
								t_timesig_added = true;
							}
						}
					}
					if (t_tempo_added) {
						tempo_added = true;
					}
					if (t_timesig_added) {
						timesig_added = true;
					}
					if (timesig_added && tempo_added) {
						// 両方ともインポート済みならexit。2個以上のトラックから、重複してテンポや拍子をインポートするのはNG（たぶん）
						break;
					}
				}
				bool contains_zero = false;
				int c = tempo.TempoTable.Count;
				for (int i = 0; i < c; i++) {
					if (tempo.TempoTable[i].Clock == 0) {
						contains_zero = true;
						break;
					}
				}
				if (!contains_zero) {
					tempo.TempoTable.Add(new TempoTableEntry(0, 500000, 0.0));
				}
				contains_zero = false;
				// =>
				// Thanks, げっぺータロー.
				// BEFORE:
				// c = tempo.TempoTable.size();
				// AFTER:
				c = tempo.TimesigTable.Count;
				// <=
				for (int i = 0; i < c; i++) {
					if (tempo.TimesigTable[i].Clock == 0) {
						contains_zero = true;
						break;
					}
				}
				if (!contains_zero) {
					tempo.TimesigTable.Add(new TimeSigTableEntry(0, 4, 4, 0));
				}
				VsqFileEx work = (VsqFileEx)MusicManager.getVsqFile().clone(); //後でReplaceコマンドを発行するための作業用
				int preMeasureClocks = work.getPreMeasureClocks();
				double sec_at_premeasure = work.getSecFromClock(preMeasureClocks);
				if (!dlg.isPreMeasure()) {
					sec_at_premeasure = 0.0;
				}
				VsqFileEx copy_src = (VsqFileEx)tempo.clone();
				if (sec_at_premeasure != 0.0) {
					int t = work.TempoTable[0].Tempo;
					VsqFileEx.shift(copy_src, sec_at_premeasure, t);
				}
				tempo.updateTempoInfo();
				tempo.updateTimesigInfo();

				// tempoをインポート
				bool import_tempo = dlg.isTempo();
				if (import_tempo) {
					#if DEBUG
					Logger.StdOut("FormMain#menuFileImportMidi_Click; sec_at_premeasure=" + sec_at_premeasure);
					#endif
					// 最初に、workにある全てのイベント・コントロールカーブ・ベジエ曲線をtempoのテンポテーブルに合うように、シフトする
					//ShiftClockToMatchWith( work, copy_src, work.getSecFromClock( work.getPreMeasureClocks() ) );
					//ShiftClockToMatchWith( work, copy_src, copy_src.getSecFromClock( copy_src.getPreMeasureClocks() ) );
					if (secondBasis) {
						FormMainModel.ShiftClockToMatchWith(work, copy_src, sec_at_premeasure);
					}

					work.TempoTable.Clear();
					List<TempoTableEntry> list = copy_src.TempoTable;
					int list_count = list.Count;
					for (int i = 0; i < list_count; i++) {
						TempoTableEntry item = list[i];
						work.TempoTable.Add(new TempoTableEntry(item.Clock, item.Tempo, item.Time));
					}
					work.updateTempoInfo();
				}

				// timesig
				if (dlg.isTimesig()) {
					work.TimesigTable.Clear();
					List<TimeSigTableEntry> list = tempo.TimesigTable;
					int list_count = list.Count;
					for (int i = 0; i < list_count; i++) {
						TimeSigTableEntry item = list[i];
						work.TimesigTable.Add(
							new TimeSigTableEntry(
								item.Clock,
								item.Numerator,
								item.Denominator,
								item.BarCount));
					}
					work.TimesigTable.Sort();
					work.updateTimesigInfo();
				}

				for (int i = 0; i < dlg.listTrack.ItemCount; i++) {
					if (!dlg.listTrack.GetItem(i).Checked) {
						continue;
					}
					if (work.Track.Count + 1 > ApplicationGlobal.MAX_NUM_TRACK) {
						break;
					}
					VsqTrack work_track = new VsqTrack(dlg.listTrack.GetItem(i).GetSubItem(1).Text, "Miku");

					// デフォルトの音声合成システムに切り替え
					RendererKind kind = ApplicationGlobal.appConfig.DefaultSynthesizer;
					string renderer = kind.getVersionString();
					List<VsqID> singers = MusicManager.getSingerListFromRendererKind(kind);
					work_track.changeRenderer(renderer, singers);

					List<MidiEvent> events = mf.getMidiEventList(i);
					events.Sort();
					int events_count = events.Count;

					// note
					if (dlg.isNotes()) {
						int[] onclock_each_note = new int[128];
						int[] velocity_each_note = new int[128];
						for (int j = 0; j < 128; j++) {
							onclock_each_note[j] = -1;
							velocity_each_note[j] = 64;
						}
						int last_note = -1;
						for (int j = 0; j < events_count; j++) {
							MidiEvent itemj = events[j];
							int not_closed_note = -1;
							if ((itemj.firstByte & 0xf0) == 0x90 && itemj.data.Length >= 2 && itemj.data[1] > 0) {
								for (int m = 0; m < 128; m++) {
									if (onclock_each_note[m] >= 0) {
										not_closed_note = m;
										break;
									}
								}
							}
							#if DEBUG
							Logger.StdOut("FormMain#menuFileImprotMidi_Click; not_closed_note=" + not_closed_note);
							#endif
							if (((itemj.firstByte & 0xf0) == 0x90 && itemj.data.Length >= 2 && itemj.data[1] == 0) ||
								((itemj.firstByte & 0xf0) == 0x80 && itemj.data.Length >= 2) ||
								not_closed_note >= 0) {
								int clock_off = (int)itemj.clock;
								int note = (int)itemj.data[0];
								if (not_closed_note >= 0) {
									note = not_closed_note;
								}
								if (onclock_each_note[note] >= 0) {
									int add_clock_on = onclock_each_note[note];
									int add_clock_off = clock_off;
									if (secondBasis) {
										double time_clock_on = tempo.getSecFromClock(onclock_each_note[note]) + sec_at_premeasure + offsetSeconds;
										double time_clock_off = tempo.getSecFromClock(clock_off) + sec_at_premeasure + offsetSeconds;
										add_clock_on = (int)work.getClockFromSec(time_clock_on);
										add_clock_off = (int)work.getClockFromSec(time_clock_off);
									} else {
										add_clock_on += (importFromPremeasure ? preMeasureClocks : 0) + offsetClocks;
										add_clock_off += (importFromPremeasure ? preMeasureClocks : 0) + offsetClocks;
									}
									if (add_clock_on < 0) {
										add_clock_on = 0;
									}
									if (add_clock_off < 0) {
										continue;
									}
									VsqID vid = new VsqID(0);
									vid.type = VsqIDType.Anote;
									vid.setLength(add_clock_off - add_clock_on);
									#if DEBUG
									Logger.StdOut("FormMain#menuFileImportMidi_Click; vid.Length=" + vid.getLength());
									#endif
									string phrase = "a";
									if (dlg.isLyric()) {
										for (int k = 0; k < events_count; k++) {
											MidiEvent itemk = events[k];
											if (onclock_each_note[note] <= (int)itemk.clock && (int)itemk.clock <= clock_off) {
												if (itemk.firstByte == 0xff && itemk.data.Length >= 2 && itemk.data[0] == 0x05) {
													int[] d = new int[itemk.data.Length - 1];
													for (int m = 1; m < itemk.data.Length; m++) {
														d[m - 1] = 0xff & itemk.data[m];
													}
													phrase = get_string_from_metatext(d);
													break;
												}
											}
										}
									}
									vid.LyricHandle = new LyricHandle(phrase, "a");
									vid.Note = note;
									vid.Dynamics = velocity_each_note[note];
									// デフォルとの歌唱スタイルを適用する
									EditorManager.editorConfig.applyDefaultSingerStyle(vid);

									// ビブラート
									if (EditorManager.editorConfig.EnableAutoVibrato) {
										int note_length = vid.getLength();
										// 音符位置での拍子を調べる
										Timesig timesig = work.getTimesigAt(add_clock_on);

										// ビブラートを自動追加するかどうかを決める閾値
										int threshold = EditorManager.editorConfig.AutoVibratoThresholdLength;
										if (note_length >= threshold) {
											int vibrato_clocks = 0;
											DefaultVibratoLengthEnum vib_length = ApplicationGlobal.appConfig.DefaultVibratoLength;
											if (vib_length == DefaultVibratoLengthEnum.L100) {
												vibrato_clocks = note_length;
											} else if (vib_length == DefaultVibratoLengthEnum.L50) {
												vibrato_clocks = note_length / 2;
											} else if (vib_length == DefaultVibratoLengthEnum.L66) {
												vibrato_clocks = note_length * 2 / 3;
											} else if (vib_length == DefaultVibratoLengthEnum.L75) {
												vibrato_clocks = note_length * 3 / 4;
											}
											// とりあえずVOCALOID2のデフォルトビブラートの設定を使用
											vid.VibratoHandle = EditorManager.editorConfig.createAutoVibrato(SynthesizerType.VOCALOID2, vibrato_clocks);
											vid.VibratoDelay = note_length - vibrato_clocks;
										}
									}

									VsqEvent ve = new VsqEvent(add_clock_on, vid);
									work_track.addEvent(ve);
									onclock_each_note[note] = -1;
								}
							}
							if ((itemj.firstByte & 0xf0) == 0x90 && itemj.data.Length >= 2 && itemj.data[1] > 0) {
								int note = itemj.data[0];
								onclock_each_note[note] = (int)itemj.clock;
								int vel = itemj.data[1];
								velocity_each_note[note] = vel;
								last_note = note;
							}
						}

						int track = work.Track.Count;
						CadenciiCommand run_add =
							VsqFileEx.generateCommandAddTrack(
								work_track,
								new VsqMixerEntry(0, 0, 0, 0),
								track,
								new BezierCurves());
						work.executeCommand(run_add);
					}
				}

				CadenciiCommand lastrun = VsqFileEx.generateCommandReplace(work);
				EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(lastrun));
				parent.form.setEdited(true);
				parent.form.refreshScreen();
			}

			public void RunFileImportVsqCommand()
			{
				string dir = ApplicationGlobal.appConfig.getLastUsedPathIn(EditorManager.editorConfig.LastUsedExtension);
				parent.form.openMidiDialog.SetSelectedFile(dir);
				var dialog_result = DialogManager.ShowModalFileDialog(parent.form.openMidiDialog, true, parent.form);

				if (dialog_result != Cadencii.Gui.DialogResult.OK) {
					return;
				}
				VsqFileEx vsq = null;
				string filename = parent.form.openMidiDialog.FileName;
				ApplicationGlobal.appConfig.setLastUsedPathIn(filename, ".vsq");
				try {
					vsq = new VsqFileEx(filename, "Shift_JIS");
				} catch (Exception ex) {
					Logger.write(GetType () + ".menuFileImportVsq_Click; ex=" + ex + "\n");
					DialogManager.ShowMessageBox(_("Invalid VSQ/VOCALOID MIDI file"), _("Error"), cadencii.Dialog.MSGBOX_DEFAULT_OPTION, cadencii.Dialog.MSGBOX_WARNING_MESSAGE);
					return;
				}
				if (parent.form.mDialogMidiImportAndExport == null) {
					parent.form.mDialogMidiImportAndExport = ApplicationUIHost.Create<FormMidiImExport>();
				}
				var dlg = parent.form.mDialogMidiImportAndExport;
				dlg.listTrack.ClearItems();
				for (int track = 1; track < vsq.Track.Count; track++) {
					dlg.listTrack.AddRow(new string[] {
						track + "",
						vsq.Track[ track ].getName(),
						vsq.Track[ track ].getEventCount() + "" }, true);
				}
				dlg.Mode = (FormMidiMode.IMPORT_VSQ);
				dlg.setTempo(false);
				dlg.setTimesig(false);
				dlg.Location = parent.GetFormPreferedLocation(dlg.Width, dlg.Height);
				var dr = DialogManager.ShowModalDialog(dlg, parent.form);
				if (dr != DialogResult.OK) {
					return;
				}

				List<int> add_track = new List<int>();
				for (int i = 0; i < dlg.listTrack.ItemCount; i++) {
					if (dlg.listTrack.GetItem(i).Checked) {
						add_track.Add(i + 1);
					}
				}
				if (add_track.Count <= 0) {
					return;
				}

				VsqFileEx replace = (VsqFileEx)MusicManager.getVsqFile().clone();
				double premeasure_sec_replace = replace.getSecFromClock(replace.getPreMeasureClocks());
				double premeasure_sec_vsq = vsq.getSecFromClock(vsq.getPreMeasureClocks());

				if (dlg.isTempo()) {
					FormMainModel.ShiftClockToMatchWith(replace, vsq, premeasure_sec_replace - premeasure_sec_vsq);
					// テンポテーブルを置き換え
					replace.TempoTable.Clear();
					for (int i = 0; i < vsq.TempoTable.Count; i++) {
						replace.TempoTable.Add((TempoTableEntry)vsq.TempoTable[i].clone());
					}
					replace.updateTempoInfo();
					replace.updateTotalClocks();
				}

				if (dlg.isTimesig()) {
					// 拍子をリプレースする場合
					replace.TimesigTable.Clear();
					for (int i = 0; i < vsq.TimesigTable.Count; i++) {
						replace.TimesigTable.Add((TimeSigTableEntry)vsq.TimesigTable[i].clone());
					}
					replace.updateTimesigInfo();
				}

				foreach (var track in add_track) {
					if (replace.Track.Count + 1 >= ApplicationGlobal.MAX_NUM_TRACK) {
						break;
					}
					if (!dlg.isTempo()) {
						// テンポをリプレースしない場合。インポートするトラックのクロックを調節する
						for (Iterator<VsqEvent> itr2 = vsq.Track[track].getEventIterator(); itr2.hasNext(); ) {
							VsqEvent item = itr2.next();
							if (item.ID.type == VsqIDType.Singer && item.Clock == 0) {
								continue;
							}
							int clock = item.Clock;
							double sec_start = vsq.getSecFromClock(clock) - premeasure_sec_vsq + premeasure_sec_replace;
							double sec_end = vsq.getSecFromClock(clock + item.ID.getLength()) - premeasure_sec_vsq + premeasure_sec_replace;
							int clock_start = (int)replace.getClockFromSec(sec_start);
							int clock_end = (int)replace.getClockFromSec(sec_end);
							item.Clock = clock_start;
							item.ID.setLength(clock_end - clock_start);
							if (item.ID.VibratoHandle != null) {
								double sec_vib_start = vsq.getSecFromClock(clock + item.ID.VibratoDelay) - premeasure_sec_vsq + premeasure_sec_replace;
								int clock_vib_start = (int)replace.getClockFromSec(sec_vib_start);
								item.ID.VibratoDelay = clock_vib_start - clock_start;
								item.ID.VibratoHandle.setLength(clock_end - clock_vib_start);
							}
						}

						// コントロールカーブをシフト
						foreach (CurveType ct in BezierCurves.CURVE_USAGE) {
							VsqBPList item = vsq.Track[track].getCurve(ct.getName());
							if (item == null) {
								continue;
							}
							VsqBPList repl = new VsqBPList(item.getName(), item.getDefault(), item.getMinimum(), item.getMaximum());
							for (int i = 0; i < item.size(); i++) {
								int clock = item.getKeyClock(i);
								int value = item.getElement(i);
								double sec = vsq.getSecFromClock(clock) - premeasure_sec_vsq + premeasure_sec_replace;
								if (sec >= premeasure_sec_replace) {
									int clock_new = (int)replace.getClockFromSec(sec);
									repl.add(clock_new, value);
								}
							}
							vsq.Track[track].setCurve(ct.getName(), repl);
						}

						// ベジエカーブをシフト
						foreach (CurveType ct in BezierCurves.CURVE_USAGE) {
							List<BezierChain> list = vsq.AttachedCurves.get(track - 1).get(ct);
							if (list == null) {
								continue;
							}
							foreach (var chain in list) {
								foreach (var point in chain.points) {
									PointD bse = new PointD(replace.getClockFromSec(vsq.getSecFromClock(point.getBase().getX()) - premeasure_sec_vsq + premeasure_sec_replace),
										point.getBase().getY());
									PointD ctrl_r = new PointD(replace.getClockFromSec(vsq.getSecFromClock(point.controlLeft.getX()) - premeasure_sec_vsq + premeasure_sec_replace),
										point.controlLeft.getY());
									PointD ctrl_l = new PointD(replace.getClockFromSec(vsq.getSecFromClock(point.controlRight.getX()) - premeasure_sec_vsq + premeasure_sec_replace),
										point.controlRight.getY());
									point.setBase(bse);
									point.controlLeft = ctrl_l;
									point.controlRight = ctrl_r;
								}
							}
						}
					}
					replace.Mixer.Slave.Add(new VsqMixerEntry());
					replace.Track.Add(vsq.Track[track]);
					replace.AttachedCurves.add(vsq.AttachedCurves.get(track - 1));
				}

				// コマンドを発行し、実行
				CadenciiCommand run = VsqFileEx.generateCommandReplace(replace);
				EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
				parent.form.setEdited(true);
			}

			public void RunFileExportWaveCommand()
			{
				var dialog_result = Cadencii.Gui.DialogResult.Cancel;
				string filename = "";
				UiSaveFileDialog sfd = null;
				try {
					string last_path = ApplicationGlobal.appConfig.getLastUsedPathOut("wav");
					#if DEBUG
					Logger.StdOut("FormMain#menuFileExportWave_Click; last_path=" + last_path);
					#endif
					sfd = ApplicationUIHost.Create<UiSaveFileDialog> ();
					sfd.SetSelectedFile(last_path);
					sfd.Title = _("Wave Export");
					sfd.Filter = string.Join("|", new[] { _("Wave File(*.wav)|*.wav"), _("All Files(*.*)|*.*") });
					dialog_result = DialogManager.ShowModalFileDialog(sfd, false, parent.form);
					if (dialog_result != Cadencii.Gui.DialogResult.OK) {
						return;
					}
					filename = sfd.FileName;
					ApplicationGlobal.appConfig.setLastUsedPathOut(filename, ".wav");
				} catch (Exception ex) {
					Logger.write(GetType () + ".menuFileExportWave_Click; ex=" + ex + "\n");
				} finally {
					if (sfd != null) {
						try {
							sfd.Dispose();
						} catch (Exception ex2) {
							Logger.write(GetType () + ".menuFileExportWave_Click; ex=" + ex2 + "\n");
						}
					}
				}

				VsqFileEx vsq = MusicManager.getVsqFile();
				int clockStart = vsq.config.StartMarkerEnabled ? vsq.config.StartMarker : 0;
				int clockEnd = vsq.config.EndMarkerEnabled ? vsq.config.EndMarker : vsq.TotalClocks + 240;
				if (clockStart > clockEnd) {
					DialogManager.ShowMessageBox(
						_("invalid rendering region; start>=end"),
						_("Error"),
						Cadencii.Gui.AwtHost.OK_OPTION,
						cadencii.Dialog.MSGBOX_INFORMATION_MESSAGE);
					return;
				}
				List<int> other_tracks = new List<int>();
				int selected = EditorManager.Selected;
				for (int i = 1; i < vsq.Track.Count; i++) {
					if (i != selected) {
						other_tracks.Add(i);
					}
				}
				List<PatchWorkQueue> queue =
					EditorManager.patchWorkCreateQueue(other_tracks);
				PatchWorkQueue q = new PatchWorkQueue();
				q.track = selected;
				q.clockStart = clockStart;
				q.clockEnd = clockEnd;
				q.file = filename;
				q.renderAll = true;
				q.vsq = vsq;
				// 末尾に追加
				queue.Add(q);
				double started = PortUtil.getCurrentTime();

				FormWorker fs = null;
				try {
					fs = new FormWorker();
					fs.setupUi(ApplicationUIHost.Create<FormWorkerUi> (fs));
					fs.getUi().setTitle(_("Synthesize"));
					fs.getUi().setText(_("now synthesizing..."));

					SynthesizeWorker worker = new SynthesizeWorker(parent.form);

					foreach (PatchWorkQueue qb in queue) {
						fs.addJob(worker, "processQueue", qb.getMessage(), qb.getJobAmount(), qb);
					}

					fs.startJob();
					DialogManager.ShowModalDialog(fs.getUi(), parent.form);
				} catch (Exception ex) {
					Logger.write(GetType () + ".menuFileExportWave_Click; ex=" + ex + "\n");
				} finally {
					if (fs != null) {
						try {
							fs.getUi().close();
						} catch (Exception ex2) {
							Logger.write(GetType () + ".menuFileExportWave_Click; ex=" + ex2 + "\n");
						}
					}
				}
			}

			public void RunFileExportParaWaveCommand()
			{
				// 出力するディレクトリを選択
				string dir = "";
				UiFolderBrowserDialog file_dialog = null;
				try {
					file_dialog = ApplicationUIHost.Create<UiFolderBrowserDialog> ();
					string initial_dir = ApplicationGlobal.appConfig.getLastUsedPathOut("wav");
					file_dialog.Description = _("Choose destination directory");
					file_dialog.SelectedPath = initial_dir;
					var ret = DialogManager.ShowModalFolderDialog(file_dialog, parent.form);
					if (ret != Cadencii.Gui.DialogResult.OK) {
						return;
					}
					dir = file_dialog.SelectedPath;
					// 1.wavはダミー
					initial_dir = Path.Combine(dir, "1.wav");
					ApplicationGlobal.appConfig.setLastUsedPathOut(initial_dir, ".wav");
				} catch (Exception ex) {
				} finally {
					if (file_dialog != null) {
						try {
							file_dialog.Dispose();
						} catch (Exception ex2) {
						}
					}
				}

				// 全部レンダリング済みの状態にするためのキュー
				VsqFileEx vsq = MusicManager.getVsqFile();
				List<int> tracks = new List<int>();
				int size = vsq.Track.Count;
				for (int i = 1; i < size; i++) {
					tracks.Add(i);
				}
				List<PatchWorkQueue> queue = EditorManager.patchWorkCreateQueue(tracks);

				// 全トラックをファイルに出力するためのキュー
				int clockStart = vsq.config.StartMarkerEnabled ? vsq.config.StartMarker : 0;
				int clockEnd = vsq.config.EndMarkerEnabled ? vsq.config.EndMarker : vsq.TotalClocks + 240;
				if (clockStart > clockEnd) {
					DialogManager.ShowMessageBox(
						_("invalid rendering region; start>=end"),
						_("Error"),
						Cadencii.Gui.AwtHost.OK_OPTION,
						cadencii.Dialog.MSGBOX_INFORMATION_MESSAGE);
					return;
				}
				for (int i = 1; i < size; i++) {
					PatchWorkQueue q = new PatchWorkQueue();
					q.track = i;
					q.clockStart = clockStart;
					q.clockEnd = clockEnd;
					q.file = Path.Combine(dir, i + ".wav");
					q.renderAll = true;
					q.vsq = vsq;
					queue.Add(q);
				}

				// 合成ダイアログを出す
				FormWorker fw = null;
				try {
					fw = new FormWorker();
					fw.setupUi(ApplicationUIHost.Create<FormWorkerUi>(fw));
					fw.getUi().setTitle(_("Synthesize"));
					fw.getUi().setText(_("now synthesizing..."));

					SynthesizeWorker worker = new SynthesizeWorker(parent.form);

					for (int i = 0; i < queue.Count; i++) {
						PatchWorkQueue q = queue[i];
						fw.addJob(worker, "processQueue", q.getMessage(), q.getJobAmount(), q);
					}

					fw.startJob();
					DialogManager.ShowModalDialog(fw.getUi(), parent.form);
				} catch (Exception ex) {
					Logger.write(GetType () + ".menuFileExportParaWave; ex=" + ex + "\n");
				} finally {
					if (fw != null) {
						try {
							fw.getUi().close();
						} catch (Exception ex2) {
						}
					}
				}
			}

			public void RunFileExportMidiCommand()
			{
				if (parent.form.mDialogMidiImportAndExport == null) {
					parent.form.mDialogMidiImportAndExport = ApplicationUIHost.Create<FormMidiImExport>();
				}
				var dlg = parent.form.mDialogMidiImportAndExport;
				dlg.listTrack.ClearItems();
				VsqFileEx vsq = (VsqFileEx)MusicManager.getVsqFile().clone();

				for (int i = 0; i < vsq.Track.Count; i++) {
					VsqTrack track = vsq.Track[i];
					int notes = 0;
					foreach (var obj in track.getNoteEventIterator()) {
						notes++;
					}
					dlg.listTrack.AddRow(new string[] { i + "", track.getName(), notes + "" }, true);
				}
				dlg.Mode = (FormMidiMode.EXPORT);
				dlg.Location = parent.GetFormPreferedLocation(dlg.Width, dlg.Height);
				var dr = DialogManager.ShowModalDialog(dlg, parent.form);
				if (dr == DialogResult.OK) {
					if (!dlg.isPreMeasure()) {
						vsq.removePart(0, vsq.getPreMeasureClocks());
					}
					int track_count = 0;
					for (int i = 0; i < dlg.listTrack.ItemCount; i++) {
						if (dlg.listTrack.GetItem(i).Checked) {
							track_count++;
						}
					}
					if (track_count == 0) {
						return;
					}

					string dir = ApplicationGlobal.appConfig.getLastUsedPathOut("mid");
					parent.form.saveMidiDialog.SetSelectedFile(dir);
					var dialog_result = DialogManager.ShowModalFileDialog(parent.form.saveMidiDialog, false, parent.form);

					if (dialog_result == Cadencii.Gui.DialogResult.OK) {
						FileStream fs = null;
						string filename = parent.form.saveMidiDialog.FileName;
						ApplicationGlobal.appConfig.setLastUsedPathOut(filename, ".mid");
						try {
							fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
							// ヘッダー
							fs.Write(new byte[] { 0x4d, 0x54, 0x68, 0x64 }, 0, 4);
							//データ長
							fs.WriteByte((byte)0x00);
							fs.WriteByte((byte)0x00);
							fs.WriteByte((byte)0x00);
							fs.WriteByte((byte)0x06);
							//フォーマット
							fs.WriteByte((byte)0x00);
							fs.WriteByte((byte)0x01);
							//トラック数
							VsqFile.writeUnsignedShort(fs, track_count);
							//時間単位
							fs.WriteByte((byte)0x01);
							fs.WriteByte((byte)0xe0);
							int count = -1;
							for (int i = 0; i < dlg.listTrack.ItemCount; i++) {
								if (!dlg.listTrack.GetItem (i).Checked) {
									continue;
								}
								VsqTrack track = vsq.Track[i];
								count++;
								fs.Write(new byte[] { 0x4d, 0x54, 0x72, 0x6b }, 0, 4);
								//データ長。とりあえず0を入れておく
								fs.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }, 0, 4);
								long first_position = fs.Position;
								//トラック名
								VsqFile.writeFlexibleLengthUnsignedLong(fs, 0);//デルタタイム
								fs.WriteByte((byte)0xff);//ステータスタイプ
								fs.WriteByte((byte)0x03);//イベントタイプSequence/Track Name
								byte[] track_name = PortUtil.getEncodedByte("Shift_JIS", track.getName());
								fs.WriteByte((byte)track_name.Length);
								fs.Write(track_name, 0, track_name.Length);

								List<MidiEvent> events = new List<MidiEvent>();

								// tempo
								bool print_tempo = dlg.isTempo();
								if (print_tempo && count == 0) {
									List<MidiEvent> tempo_events = vsq.generateTempoChange();
									for (int j = 0; j < tempo_events.Count; j++) {
										events.Add(tempo_events[j]);
									}
								}

								// timesig
								if (dlg.isTimesig() && count == 0) {
									List<MidiEvent> timesig_events = vsq.generateTimeSig();
									for (int j = 0; j < timesig_events.Count; j++) {
										events.Add(timesig_events[j]);
									}
								}

								// Notes
								if (dlg.isNotes()) {
									foreach (var ve in track.getNoteEventIterator()) {
										int clock_on = ve.Clock;
										int clock_off = ve.Clock + ve.ID.getLength();
										if (!print_tempo) {
											// テンポを出力しない場合、テンポを500000（120）と見なしてクロックを再計算
											double time_on = vsq.getSecFromClock(clock_on);
											double time_off = vsq.getSecFromClock(clock_off);
											clock_on = (int)(960.0 * time_on);
											clock_off = (int)(960.0 * time_off);
										}
										MidiEvent noteon = new MidiEvent();
										noteon.clock = clock_on;
										noteon.firstByte = 0x90;
										noteon.data = new int[2];
										noteon.data[0] = ve.ID.Note;
										noteon.data[1] = ve.ID.Dynamics;
										events.Add(noteon);
										MidiEvent noteoff = new MidiEvent();
										noteoff.clock = clock_off;
										noteoff.firstByte = 0x80;
										noteoff.data = new int[2];
										noteoff.data[0] = ve.ID.Note;
										noteoff.data[1] = 0x7f;
										events.Add(noteoff);
									}
								}

								// lyric
								if (dlg.isLyric()) {
									foreach (var ve in track.getNoteEventIterator()) {
										int clock_on = ve.Clock;
										if (!print_tempo) {
											double time_on = vsq.getSecFromClock(clock_on);
											clock_on = (int)(960.0 * time_on);
										}
										MidiEvent add = new MidiEvent();
										add.clock = clock_on;
										add.firstByte = 0xff;
										byte[] lyric = PortUtil.getEncodedByte("Shift_JIS", ve.ID.LyricHandle.L0.Phrase);
										add.data = new int[lyric.Length + 1];
										add.data[0] = 0x05;
										for (int j = 0; j < lyric.Length; j++) {
											add.data[j + 1] = lyric[j];
										}
										events.Add(add);
									}
								}

								// vocaloid metatext
								List<MidiEvent> meta;
								if (dlg.isVocaloidMetatext() && i > 0) {
									meta = vsq.generateMetaTextEvent(i, "Shift_JIS");
								} else {
									meta = new List<MidiEvent>();
								}

								// vocaloid nrpn
								List<MidiEvent> vocaloid_nrpn_midievent;
								if (dlg.isVocaloidNrpn() && i > 0) {
									VsqNrpn[] vsqnrpn = VsqFileEx.generateNRPN((VsqFile)vsq, i, ApplicationGlobal.appConfig.PreSendTime);
									NrpnData[] nrpn = VsqNrpn.convert(vsqnrpn);

									vocaloid_nrpn_midievent = new List<MidiEvent>();
									for (int j = 0; j < nrpn.Length; j++) {
										MidiEvent me = new MidiEvent();
										me.clock = nrpn[j].getClock();
										me.firstByte = 0xb0;
										me.data = new int[2];
										me.data[0] = nrpn[j].getParameter();
										me.data[1] = nrpn[j].Value;
										vocaloid_nrpn_midievent.Add(me);
									}
								} else {
									vocaloid_nrpn_midievent = new List<MidiEvent>();
								}
								#if DEBUG
								Logger.StdOut("menuFileExportMidi_Click");
								Logger.StdOut("    vocaloid_nrpn_midievent.size()=" + vocaloid_nrpn_midievent.Count);
								#endif

								// midi eventを出力
								events.Sort();
								long last_clock = 0;
								int events_count = events.Count;
								if (events_count > 0) {
									for (int j = 0; j < events_count; j++) {
										if (events[j].clock > 0 && meta.Count > 0) {
											for (int k = 0; k < meta.Count; k++) {
												VsqFile.writeFlexibleLengthUnsignedLong(fs, 0);
												meta[k].writeData(fs);
											}
											meta.Clear();
											last_clock = 0;
										}
										long clock = events[j].clock;
										while (vocaloid_nrpn_midievent.Count > 0 && vocaloid_nrpn_midievent[0].clock < clock) {
											VsqFile.writeFlexibleLengthUnsignedLong(fs, (long)(vocaloid_nrpn_midievent[0].clock - last_clock));
											last_clock = vocaloid_nrpn_midievent[0].clock;
											vocaloid_nrpn_midievent[0].writeData(fs);
											vocaloid_nrpn_midievent.RemoveAt(0);
										}
										VsqFile.writeFlexibleLengthUnsignedLong(fs, (long)(events[j].clock - last_clock));
										events[j].writeData(fs);
										last_clock = events[j].clock;
									}
								} else {
									int c = vocaloid_nrpn_midievent.Count;
									for (int k = 0; k < meta.Count; k++) {
										VsqFile.writeFlexibleLengthUnsignedLong(fs, 0);
										meta[k].writeData(fs);
									}
									meta.Clear();
									last_clock = 0;
									for (int j = 0; j < c; j++) {
										MidiEvent item = vocaloid_nrpn_midievent[j];
										long clock = item.clock;
										VsqFile.writeFlexibleLengthUnsignedLong(fs, (long)(clock - last_clock));
										item.writeData(fs);
										last_clock = clock;
									}
								}

								// トラックエンドを記入し、
								VsqFile.writeFlexibleLengthUnsignedLong(fs, (long)0);
								fs.WriteByte((byte)0xff);
								fs.WriteByte((byte)0x2f);
								fs.WriteByte((byte)0x00);
								// チャンクの先頭に戻ってチャンクのサイズを記入
								long pos = fs.Position;
								fs.Seek(first_position - 4, SeekOrigin.Begin);
								VsqFile.writeUnsignedInt(fs, pos - first_position);
								// ファイルを元の位置にseek
								fs.Seek(pos, SeekOrigin.Begin);
							}
						} catch (Exception ex) {
							Logger.write(GetType () + ".menuFileExportMidi_Click; ex=" + ex + "\n");
						} finally {
							if (fs != null) {
								try {
									fs.Close();
								} catch (Exception ex2) {
									Logger.write(GetType () + ".menuFileExportMidi_Click; ex=" + ex2 + "\n");
								}
							}
						}
					}
				}
			}

			public void RunFileExportMusicXmlCommand()
			{
				UiSaveFileDialog dialog = null;
				try {
					VsqFileEx vsq = MusicManager.getVsqFile();
					if (vsq == null) {
						return;
					}
					string first = ApplicationGlobal.appConfig.getLastUsedPathOut("xml");
					dialog = ApplicationUIHost.Create<UiSaveFileDialog> ();
					dialog.SetSelectedFile(first);
					dialog.Filter = string.Join("|", new[] { _("MusicXML(*.xml)|*.xml"), _("All Files(*.*)|*.*") });
					var result = DialogManager.ShowModalFileDialog(dialog, false, parent.form);
					if (result != Cadencii.Gui.DialogResult.OK) {
						return;
					}
					string file = dialog.FileName;
					var writer = new MusicXmlWriter();
					writer.write(vsq, file);
					ApplicationGlobal.appConfig.setLastUsedPathOut(file, ".xml");
				} catch (Exception ex) {
					Logger.write(GetType () + ".menuFileExportMusicXml_Click; ex=" + ex + "\n");
					Logger.StdErr("FormMain#menuFileExportMusicXml_Click; ex=" + ex);
				} finally {
					if (dialog != null) {
						try {
							dialog.Dispose();
						} catch (Exception ex2) {
							Logger.write(GetType () + ".menuFileExportMusicXml_Click; ex=" + ex2 + "\n");
							Logger.StdErr("FormMain#menuFileExportMusicXml_Click; ex2=" + ex2);
						}
					}
				}
			}

			public void RunFileExportUstCommand()
			{
				VsqFileEx vsq = (VsqFileEx)MusicManager.getVsqFile().clone();

				// どのトラックを出力するか決める
				int selected = EditorManager.Selected;

				// 出力先のファイル名を選ぶ
				UiSaveFileDialog dialog = null;
				var dialog_result = Cadencii.Gui.DialogResult.Cancel;
				string file_name = "";
				try {
					string last_path = ApplicationGlobal.appConfig.getLastUsedPathOut("ust");
					dialog = ApplicationUIHost.Create<UiSaveFileDialog> ();
					dialog.SetSelectedFile(last_path);
					dialog.Title = _("Export UTAU (*.ust)");
					dialog.Filter = string.Join("|", new[] { _("UTAU Script Format(*.ust)|*.ust"), _("All Files(*.*)|*.*") });
					dialog_result = DialogManager.ShowModalFileDialog(dialog, false, parent.form);
					if (dialog_result != Cadencii.Gui.DialogResult.OK) {
						return;
					}
					file_name = dialog.FileName;
					ApplicationGlobal.appConfig.setLastUsedPathOut(file_name, ".ust");
				} catch (Exception ex) {
					Logger.write(GetType () + ".menuFileExportUst_Click; ex=" + ex + "\n");
				} finally {
					if (dialog != null) {
						try {
							dialog.Dispose();
						} catch (Exception ex2) {
							Logger.write(GetType () + ".menuFileExportUst_Click; ex=" + ex2 + "\n");
						}
					}
				}

				// 出力処理
				vsq.removePart(0, vsq.getPreMeasureClocks());
				UstFile ust = new UstFile(vsq, selected);
				// voice dirを設定
				VsqTrack vsq_track = vsq.Track[selected];
				VsqEvent singer = vsq_track.getSingerEventAt(0);
				string voice_dir = "";
				if (singer != null) {
					int program = singer.ID.IconHandle.Program;
					int size = ApplicationGlobal.appConfig.UtauSingers.Count;
					if (0 <= program && program < size) {
						SingerConfig cfg = ApplicationGlobal.appConfig.UtauSingers[program];
						voice_dir = cfg.VOICEIDSTR;
					}
				}
				ust.setVoiceDir(voice_dir);
				ust.setWavTool(ApplicationGlobal.appConfig.PathWavtool);
				int resampler_index = VsqFileEx.getTrackResamplerUsed(vsq_track);
				if (0 <= resampler_index && resampler_index < ApplicationGlobal.appConfig.getResamplerCount()) {
					ust.setResampler(
						ApplicationGlobal.appConfig.getResamplerAt(resampler_index));
				}
				ust.write(file_name);
			}

			public void RunFileExportVsqCommand()
			{
				VsqFileEx vsq = MusicManager.getVsqFile();

				// 出力先のファイル名を選ぶ
				UiSaveFileDialog dialog = null;
				var dialog_result = Cadencii.Gui.DialogResult.Cancel;
				string file_name = "";
				try {
					string last_path = ApplicationGlobal.appConfig.getLastUsedPathOut("vsq");
					dialog = ApplicationUIHost.Create<UiSaveFileDialog>();
					dialog.SetSelectedFile(last_path);
					dialog.Title = _("Export VSQ (*.vsq)");
					dialog.Filter = string.Join("|", new[] { _("VSQ Format(*.vsq)|*.vsq"), _("All Files(*.*)|*.*") });
					dialog_result = DialogManager.ShowModalFileDialog(dialog, false, parent.form);
					if (dialog_result != Cadencii.Gui.DialogResult.OK) {
						return;
					}
					file_name = dialog.FileName;
					ApplicationGlobal.appConfig.setLastUsedPathOut(file_name, ".vsq");
				} catch (Exception ex) {
					Logger.write(GetType () + ".menuFileExportVsq_Click; ex=" + ex + "\n");
				} finally {
					if (dialog != null) {
						try {
							dialog.Dispose();
						} catch (Exception ex2) {
							Logger.write(GetType () + ".menuFileExportVsq_Click; ex=" + ex2 + "\n");
						}
					}
				}

				// 出力処理
				VsqFile tvsq = (VsqFile)vsq;
				tvsq.write(file_name, ApplicationGlobal.appConfig.PreSendTime, "Shift_JIS");
			}

			public void RunFileExportVsqxCommand()
			{
				VsqFileEx sequence = MusicManager.getVsqFile();
				using (var dialog = ApplicationUIHost.Create<UiSaveFileDialog> ()) {
					dialog.Title = _("Export VSQX (*.vsqx)");
					dialog.Filter = _("VSQX Format(*.vsqx)|*.vsqx") + "|" + _("All Files(*.*)|*.*");
					if (dialog.ShowDialog() == DialogResult.OK) {
						var file_path = dialog.FileName;
						var writer = new VsqxWriter();
						writer.write(sequence, file_path);
					}
				}
			}

			public void RunFileExportVxtCommand()
			{
				// UTAUの歌手が登録されていない場合は警告を表示
				if (ApplicationGlobal.appConfig.UtauSingers.Count <= 0) {
					var dr = DialogManager.ShowMessageBox(
						_("UTAU singer not registered yet.\nContinue ?"),
						_("Info"),
						cadencii.Dialog.MSGBOX_YES_NO_OPTION,
						cadencii.Dialog.MSGBOX_INFORMATION_MESSAGE);
					if (dr != Cadencii.Gui.DialogResult.Yes) {
						return;
					}
				}

				VsqFileEx vsq = MusicManager.getVsqFile();

				// 出力先のファイル名を選ぶ
				UiSaveFileDialog dialog = null;
				var dialog_result = Cadencii.Gui.DialogResult.Cancel;
				string file_name = "";
				try {
					string last_path = ApplicationGlobal.appConfig.getLastUsedPathOut("txt");
					dialog = ApplicationUIHost.Create<UiSaveFileDialog> ();
					dialog.SetSelectedFile(last_path);
					dialog.Title = _("Metatext for vConnect");
					dialog.Filter = string.Join("|", new[] { _("Text File(*.txt)|*.txt"), _("All Files(*.*)|*.*") });
					dialog_result = DialogManager.ShowModalFileDialog(dialog, false, parent.form);
					if (dialog_result != Cadencii.Gui.DialogResult.OK) {
						return;
					}
					file_name = dialog.FileName;
					ApplicationGlobal.appConfig.setLastUsedPathOut(file_name, ".txt");
				} catch (Exception ex) {
					Logger.write(GetType () + ".menuFileExportVxt_Click; ex=" + ex + "\n");
				} finally {
					if (dialog != null) {
						try {
							dialog.Dispose();
						} catch (Exception ex2) {
							Logger.write(GetType () + ".menuFileExportVxt_Click; ex=" + ex2 + "\n");
						}
					}
				}

				// 出力処理
				int selected = EditorManager.Selected;
				VsqTrack vsq_track = vsq.Track[selected];
				StreamWriter bw = null;
				try {
					bw = new StreamWriter(file_name, false, new UTF8Encoding(false));
					string oto_ini = ApplicationGlobal.appConfig.UtauSingers[0].VOICEIDSTR;
					// 先頭に登録されている歌手変更を検出
					VsqEvent singer = null;
					int c = vsq_track.getEventCount();
					for (int i = 0; i < c; i++) {
						VsqEvent itemi = vsq_track.getEvent(i);
						if (itemi.ID.type == VsqIDType.Singer) {
							singer = itemi;
							break;
						}
					}
					// 歌手のプログラムチェンジから，歌手の原音設定へのパスを取得する
					if (singer != null) {
						int indx = singer.ID.IconHandle.Program;
						if (0 <= indx && indx < ApplicationGlobal.appConfig.UtauSingers.Count) {
							oto_ini = ApplicationGlobal.appConfig.UtauSingers[indx].VOICEIDSTR;
						}
					}

					// oto.iniで終わってる？
					if (!oto_ini.EndsWith("oto.ini")) {
						oto_ini = Path.Combine(oto_ini, "oto.ini");
					}

					// 出力
					VConnectWaveGenerator.prepareMetaText(
						bw, vsq_track, oto_ini, vsq.TotalClocks, false);
				} catch (Exception ex) {
					Logger.write(GetType () + ".menuFileExportVxt_Click; ex=" + ex + "\n");
					Logger.StdErr(GetType () + ".menuFileExportVxt_Click; ex=" + ex);
				} finally {
					if (bw != null) {
						try {
							bw.Close();
						} catch (Exception ex2) {
						}
					}
				}
			}

			public void RunFileRecentClearCommand()
			{
				if (EditorManager.editorConfig.RecentFiles != null) {
					EditorManager.editorConfig.RecentFiles.Clear();
				}
				parent.UpdateRecentFileMenu();
			}

			public void RunFileQuitCommand()
			{
				parent.form.Close();
			}

			public void RunExportDropDownCommand()
			{
				parent.form.menuFileExportWave.Enabled = (MusicManager.getVsqFile().Track[EditorManager.Selected].getEventCount() > 0);
			}

			public void RunEditDropDownCommand ()
			{
				parent.form.updateCopyAndPasteButtonStatus();
			}
		}
	}
}
