using System;
using cadencii.core;
using System.Collections.Generic;
using System.IO;


using cadencii.vsq;
using cadencii.vsq.io;
using System.Text;
using System.Linq;

namespace cadencii
{
	public partial class FormMainModel
	{
		public class MainMenuModel
		{
			readonly FormMainModel parent;

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

			public void RunFileSaveCommand ()
			{
				for (int track = 1; track < MusicManager.getVsqFile().Track.Count; track++) {
					if (MusicManager.getVsqFile().Track[track].getEventCount() == 0) {
						DialogManager.showMessageBox(
							PortUtil.formatMessage(
								_("Invalid note data.\nTrack {0} : {1}\n\n-> Piano roll : Blank sequence."),
								track,
								MusicManager.getVsqFile().Track[track].getName()),
							FormMainModel.ApplicationName,
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
					var dr = DialogManager.showModalFileDialog(parent.form.saveXmlVsqDialog, false, this);
					if (dr == cadencii.java.awt.DialogResult.OK) {
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
						DialogManager.showMessageBox(
							PortUtil.formatMessage(
								_("Invalid note data.\nTrack {0} : {1}\n\n-> Piano roll : Blank sequence."), track, MusicManager.getVsqFile().Track[track].getName()
							),
							FormMainModel.ApplicationName,
							cadencii.Dialog.MSGBOX_DEFAULT_OPTION,
							cadencii.Dialog.MSGBOX_WARNING_MESSAGE);
						return;
					}
				}

				string dir = ApplicationGlobal.appConfig.getLastUsedPathOut("xvsq");
				parent.form.saveXmlVsqDialog.SetSelectedFile(dir);
				var dr = DialogManager.showModalFileDialog(parent.form.saveXmlVsqDialog, false, this);
				if (dr == cadencii.java.awt.DialogResult.OK) {
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
				var dialog_result = DialogManager.showModalFileDialog(parent.form.openMidiDialog, true, this);
				string ext = ".vsq";
				if (dialog_result == cadencii.java.awt.DialogResult.OK) {
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
					sout.println("FormMain#menuFileOpenVsq_Click; ex=" + ex);
					#endif
					DialogManager.showMessageBox(
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
				var dialog_result = DialogManager.showModalFileDialog(parent.form.openUstDialog, true, this);

				if (dialog_result != cadencii.java.awt.DialogResult.OK) {
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
					sout.println("FormMain#menuFileOpenUst_Click; ex=" + ex);
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
					var dialog_result = DialogManager.showModalFileDialog (dialog, true, this);
					if (dialog_result != cadencii.java.awt.DialogResult.OK) {
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
				var dialog_result = DialogManager.showModalFileDialog(parent.form.openMidiDialog, true, this);

				if (dialog_result != cadencii.java.awt.DialogResult.OK) {
					return;
				}
				dlg.Location = parent.getFormPreferedLocation(dlg.Width, dlg.Height);
				MidiFile mf = null;
				try {
					string filename = parent.form.openMidiDialog.FileName;
					ApplicationGlobal.appConfig.setLastUsedPathIn(filename, ".mid");
					mf = new MidiFile(filename);
				} catch (Exception ex) {
					Logger.write(GetType () + ".menuFileImportMidi_Click; ex=" + ex + "\n");
					DialogManager.showMessageBox(
						_("Invalid MIDI file."),
						_("Error"),
						cadencii.Dialog.MSGBOX_DEFAULT_OPTION,
						cadencii.Dialog.MSGBOX_WARNING_MESSAGE);
					return;
				}
				if (mf == null) {
					DialogManager.showMessageBox(
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

				var dr = DialogManager.showModalDialog(dlg, this);
				if (dr != 1) {
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
					sout.println("FormMain#menuFileImportMidi_Click; sec_at_premeasure=" + sec_at_premeasure);
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
							sout.println("FormMain#menuFileImprotMidi_Click; not_closed_note=" + not_closed_note);
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
									sout.println("FormMain#menuFileImportMidi_Click; vid.Length=" + vid.getLength());
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
		}
	}
}

