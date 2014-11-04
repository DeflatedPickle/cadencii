using System;
using cadencii.core;
using System.Collections.Generic;
using System.IO;


using cadencii.vsq;
using cadencii.vsq.io;
using System.Text;
using System.Linq;
using cadencii.java.util;
using cadencii.java.awt;

namespace cadencii
{
	public partial class FormMainModel
	{
		public class LyricMenuModel
		{
			readonly FormMainModel parent;

			public LyricMenuModel (FormMainModel parent)
			{
				this.parent = parent;
			}
			#region menuLyric*
			public void RunLyricDropDownOpening()
			{
				parent.form.menuLyricCopyVibratoToPreset.Enabled = false;

				int num = EditorManager.itemSelection.getEventCount();
				if (num <= 0) {
					return;
				}
				SelectedEventEntry item = EditorManager.itemSelection.getEventIterator().First();
				if (item.original.ID.type != VsqIDType.Anote) {
					return;
				}
				if (item.original.ID.VibratoHandle == null) {
					return;
				}

				parent.form.menuLyricCopyVibratoToPreset.Enabled = true;
			}

			public void RunLyricExpressionPropertyCommand()
			{
				parent.EditNoteExpressionProperty();
			}

			public void RunLyricPhonemeTransformationCommand()
			{
				List<int> internal_ids = new List<int>();
				List<VsqID> ids = new List<VsqID>();
				VsqFileEx vsq = MusicManager.getVsqFile();
				if (vsq == null) {
					return;
				}
				int selected = EditorManager.Selected;
				VsqTrack vsq_track = vsq.Track[selected];
				foreach (var item in vsq_track.getNoteEventIterator()) {
					VsqID id = item.ID;
					if (id.LyricHandle.L0.PhoneticSymbolProtected) {
						continue;
					}
					string phrase = id.LyricHandle.L0.Phrase;
					string symbolOld = id.LyricHandle.L0.getPhoneticSymbol();
					string symbolResult = symbolOld;
					SymbolTableEntry entry = SymbolTable.attatch(phrase);
					if (entry == null) {
						continue;
					}
					symbolResult = entry.getSymbol();
					if (symbolResult.Equals(symbolOld)) {
						continue;
					}
					VsqID idNew = (VsqID)id.clone();
					idNew.LyricHandle.L0.setPhoneticSymbol(symbolResult);
					ids.Add(idNew);
					internal_ids.Add(item.InternalID);
				}
				if (ids.Count <= 0) {
					return;
				}
				CadenciiCommand run = new CadenciiCommand(
					VsqCommand.generateCommandEventChangeIDContaintsRange(
						selected,
						PortUtil.convertIntArray(internal_ids.ToArray()),
						ids.ToArray()));
				EditorManager.editHistory.register(vsq.executeCommand(run));
				parent.form.setEdited(true);
			}

			/// <summary>
			/// 現在表示しているトラックの，選択状態の音符イベントについて，それぞれのイベントの
			/// 時刻でのUTAU歌手に応じて，UTAUの各種パラメータを原音設定のものにリセットします
			/// </summary>
			/// <param name="sender"></param>
			/// <param name="e"></param>
			public void RunLyricApplyUtauParametersCommand()
			{
				// 選択されているトラックの番号
				int selected = EditorManager.Selected;
				// シーケンス
				VsqFileEx vsq = MusicManager.getVsqFile();
				VsqTrack vsq_track = vsq.Track[selected];

				// 選択状態にあるイベントを取り出す
				List<VsqEvent> replace = new List<VsqEvent>();
				foreach (var sel_item in EditorManager.itemSelection.getEventIterator()) {
					VsqEvent item = sel_item.original;
					if (item.ID.type != VsqIDType.Anote) {
						continue;
					}
					VsqEvent edit = (VsqEvent)item.clone();
					// UTAUのパラメータを適用
					MusicManager.applyUtauParameter(vsq_track, edit);
					// 合成したとき，意味のある変更が行われたか？
					if (edit.UstEvent.equalsForSynth(item.UstEvent)) {
						continue;
					}
					// 意味のある変更があったので，リストに登録
					replace.Add(edit);
				}

				// コマンドを発行
				CadenciiCommand run = new CadenciiCommand(
					VsqCommand.generateCommandEventReplaceRange(selected, replace.ToArray()));
				// コマンドを実行
				EditorManager.editHistory.register(vsq.executeCommand(run));
				parent.form.setEdited(true);
			}

			public void RunLyricDictionaryCommand()
			{
				FormWordDictionaryController dlg = null;
				try {
					dlg = new FormWordDictionaryController(c => ApplicationUIHost.Create<FormWordDictionaryUi> (c));
					var p =parent.GetFormPreferedLocation(dlg.getWidth(), dlg.getHeight());
					dlg.setLocation(p.X, p.Y);
					int dr = DialogManager.showModalDialog(dlg.getUi(), this);
					if (dr == 1) {
						List<ValuePair<string, Boolean>> result = dlg.getResult();
						SymbolTable.changeOrder(result);
					}
				} catch (Exception ex) {
					Logger.write(GetType () + ".menuLyricDictionary_Click; ex=" + ex + "\n");
					serr.println("FormMain#menuLyricDictionary_Click; ex=" + ex);
				} finally {
					if (dlg != null) {
						try {
							dlg.close();
						} catch (Exception ex2) {
							Logger.write(GetType () + ".menuLyricDictionary_Click; ex=" + ex2 + "\n");
						}
					}
				}
			}

			public void RunLyricVibratoPropertyCommand()
			{
				parent.EditNoteVibratoProperty();
			}
			#endregion
		}
	}
}
