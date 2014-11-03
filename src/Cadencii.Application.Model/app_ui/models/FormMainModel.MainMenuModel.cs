using System;
using cadencii.core;
using System.Collections.Generic;
using System.IO;


using cadencii.vsq;
using cadencii.vsq.io;

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
		}
	}
}

