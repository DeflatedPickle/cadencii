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
using System.Media;

namespace cadencii
{
	public partial class FormMainModel
	{
		/// <summary>
		/// アンドゥ処理を行います
		/// </summary>
		public void Undo()
		{
			if (EditorManager.editHistory.hasUndoHistory()) {
				EditorManager.undo();
				form.menuEditRedo.Enabled = EditorManager.editHistory.hasRedoHistory();
				form.menuEditUndo.Enabled = EditorManager.editHistory.hasUndoHistory();
				form.cMenuPianoRedo.Enabled = EditorManager.editHistory.hasRedoHistory();
				form.cMenuPianoUndo.Enabled = EditorManager.editHistory.hasUndoHistory();
				form.cMenuTrackSelectorRedo.Enabled = EditorManager.editHistory.hasRedoHistory();
				form.cMenuTrackSelectorUndo.Enabled = EditorManager.editHistory.hasUndoHistory();
				EditorManager.MixerWindow.updateStatus();
				form.setEdited(true);
				form.updateDrawObjectList();

				#if ENABLE_PROPERTY
				if (EditorManager.propertyPanel != null) {
					EditorManager.propertyPanel.updateValue(EditorManager.Selected);
				}
				#endif
			}
		}

		/// <summary>
		/// リドゥ処理を行います
		/// </summary>
		public void Redo()
		{
			if (EditorManager.editHistory.hasRedoHistory()) {
				EditorManager.redo();
				form.menuEditRedo.Enabled = EditorManager.editHistory.hasRedoHistory();
				form.menuEditUndo.Enabled = EditorManager.editHistory.hasUndoHistory();
				form.cMenuPianoRedo.Enabled = EditorManager.editHistory.hasRedoHistory();
				form.cMenuPianoUndo.Enabled = EditorManager.editHistory.hasUndoHistory();
				form.cMenuTrackSelectorRedo.Enabled = EditorManager.editHistory.hasRedoHistory();
				form.cMenuTrackSelectorUndo.Enabled = EditorManager.editHistory.hasUndoHistory();
				EditorManager.MixerWindow.updateStatus();
				form.setEdited(true);
				form.updateDrawObjectList();

				#if ENABLE_PROPERTY
				if (EditorManager.propertyPanel != null) {
					EditorManager.propertyPanel.updateValue(EditorManager.Selected);
				}
				#endif
			}
		}

		public void DeleteEvent()
		{
			#if DEBUG
			CDebug.WriteLine(
				"FormMain#deleteEvent(); EditorManager.InputTextBox.isEnabled()=" +
				EditorManager.InputTextBox.Enabled);
			#endif

			if (EditorManager.InputTextBox.Visible) {
				return;
			}
			#if ENABLE_PROPERTY
			if (EditorManager.propertyPanel.isEditing()) {
				return;
			}
			#endif

			int selected = EditorManager.Selected;
			VsqFileEx vsq = MusicManager.getVsqFile();
			VsqTrack vsq_track = vsq.Track[selected];

			if (EditorManager.itemSelection.getEventCount() > 0) {
				List<int> ids = new List<int>();
				bool contains_aicon = false;
				foreach (var ev in EditorManager.itemSelection.getEventIterator()) {
					ids.Add(ev.original.InternalID);
					if (ev.original.ID.type == VsqIDType.Aicon) {
						contains_aicon = true;
					}
				}
				VsqCommand run = VsqCommand.generateCommandEventDeleteRange(selected, ids);
				if (EditorManager.IsWholeSelectedIntervalEnabled) {
					VsqFileEx work = (VsqFileEx)vsq.clone();
					work.executeCommand(run);
					int stdx = form.controller.getStartToDrawX();
					int start_clock = EditorManager.mWholeSelectedInterval.getStart();
					int end_clock = EditorManager.mWholeSelectedInterval.getEnd();
					List<List<BPPair>> curves = new List<List<BPPair>>();
					List<CurveType> types = new List<CurveType>();
					VsqTrack work_vsq_track = work.Track[selected];
					foreach (CurveType vct in BezierCurves.CURVE_USAGE) {
						if (vct.isScalar() || vct.isAttachNote()) {
							continue;
						}
						VsqBPList work_curve = work_vsq_track.getCurve(vct.getName());
						List<BPPair> t = new List<BPPair>();
						t.Add(new BPPair(start_clock, work_curve.getValue(start_clock)));
						t.Add(new BPPair(end_clock, work_curve.getValue(end_clock)));
						curves.Add(t);
						types.Add(vct);
					}
					List<string> strs = new List<string>();
					for (int i = 0; i < types.Count; i++) {
						strs.Add(types[i].getName());
					}
					CadenciiCommand delete_curve = new CadenciiCommand(
						VsqCommand.generateCommandTrackCurveEditRange(selected, strs, curves));
					work.executeCommand(delete_curve);
					if (contains_aicon) {
						work.Track[selected].reflectDynamics();
					}
					CadenciiCommand run2 = new CadenciiCommand(VsqCommand.generateCommandReplace(work));
					EditorManager.editHistory.register(vsq.executeCommand(run2));
					form.setEdited(true);
				} else {
					CadenciiCommand run2 = null;
					if (contains_aicon) {
						VsqFileEx work = (VsqFileEx)vsq.clone();
						work.executeCommand(run);
						VsqTrack vsq_track_copied = work.Track[selected];
						vsq_track_copied.reflectDynamics();
						run2 = VsqFileEx.generateCommandTrackReplace(selected,
							vsq_track_copied,
							work.AttachedCurves.get(selected - 1));
					} else {
						run2 = new CadenciiCommand(run);
					}
					EditorManager.editHistory.register(vsq.executeCommand(run2));
					form.setEdited(true);
					EditorManager.itemSelection.clearEvent();
				}
				form.Refresh();
			} else if (EditorManager.itemSelection.getTempoCount() > 0) {
				List<int> clocks = new List<int>();
				foreach (var item in EditorManager.itemSelection.getTempoIterator()) {
					if (item.getKey() <= 0) {
						string msg = _("Cannot remove first symbol of track!");
						form.statusLabel.Text = msg;
						SystemSounds.Asterisk.Play();
						return;
					}
					clocks.Add(item.getKey());
				}
				int[] dum = new int[clocks.Count];
				for (int i = 0; i < dum.Length; i++) {
					dum[i] = -1;
				}
				CadenciiCommand run = new CadenciiCommand(
					VsqCommand.generateCommandUpdateTempoRange(PortUtil.convertIntArray(clocks.ToArray()),
						PortUtil.convertIntArray(clocks.ToArray()),
						dum));
				EditorManager.editHistory.register(vsq.executeCommand(run));
				form.setEdited(true);
				EditorManager.itemSelection.clearTempo();
				form.Refresh();
			} else if (EditorManager.itemSelection.getTimesigCount() > 0) {
				#if DEBUG
				CDebug.WriteLine("    Timesig");
				#endif
				int[] barcounts = new int[EditorManager.itemSelection.getTimesigCount()];
				int[] numerators = new int[EditorManager.itemSelection.getTimesigCount()];
				int[] denominators = new int[EditorManager.itemSelection.getTimesigCount()];
				int count = -1;
				foreach (var item in EditorManager.itemSelection.getTimesigIterator()) {
					int key = item.getKey();
					SelectedTimesigEntry value = item.getValue();
					count++;
					barcounts[count] = key;
					if (key <= 0) {
						string msg = "Cannot remove first symbol of track!";
						form.statusLabel.Text = _(msg);
						SystemSounds.Asterisk.Play();
						return;
					}
					numerators[count] = -1;
					denominators[count] = -1;
				}
				CadenciiCommand run = new CadenciiCommand(
					VsqCommand.generateCommandUpdateTimesigRange(barcounts, barcounts, numerators, denominators));
				EditorManager.editHistory.register(vsq.executeCommand(run));
				form.setEdited(true);
				EditorManager.itemSelection.clearTimesig();
				form.Refresh();
			}
			if (EditorManager.itemSelection.getPointIDCount() > 0) {
				#if DEBUG
				CDebug.WriteLine("    Curve");
				#endif
				string curve;
				if (!form.TrackSelector.getSelectedCurve().isAttachNote()) {
					curve = form.TrackSelector.getSelectedCurve().getName();
					VsqBPList src = vsq_track.getCurve(curve);
					VsqBPList list = (VsqBPList)src.clone();
					List<int> remove_clock_queue = new List<int>();
					int count = list.size();
					for (int i = 0; i < count; i++) {
						VsqBPPair item = list.getElementB(i);
						if (EditorManager.itemSelection.isPointContains(item.id)) {
							remove_clock_queue.Add(list.getKeyClock(i));
						}
					}
					count = remove_clock_queue.Count;
					for (int i = 0; i < count; i++) {
						list.remove(remove_clock_queue[i]);
					}
					CadenciiCommand run = new CadenciiCommand(
						VsqCommand.generateCommandTrackCurveReplace(selected,
							form.TrackSelector.getSelectedCurve().getName(),
							list));
					EditorManager.editHistory.register(vsq.executeCommand(run));
					form.setEdited(true);
				} else {
					//todo: FormMain+DeleteEvent; VibratoDepth, VibratoRateの場合
				}
				EditorManager.itemSelection.clearPoint();
				form.refreshScreen();
			}
		}

		public void Paste()
		{
			int clock = EditorManager.getCurrentClock();
			int unit = EditorManager.getPositionQuantizeClock();
			clock = Quantize(clock, unit);

			VsqCommand add_event = null; // VsqEventを追加するコマンド

			ClipboardEntry ce = EditorManager.clipboard.getCopiedItems();
			int copy_started_clock = ce.copyStartedClock;
			List<VsqEvent> copied_events = ce.events;
			#if DEBUG
			sout.println("FormMain#pasteEvent; copy_started_clock=" + copy_started_clock);
			sout.println("FormMain#pasteEvent; copied_events.size()=" + copied_events.Count);
			#endif
			if (copied_events.Count != 0) {
				// VsqEventのペーストを行うコマンドを発行
				int dclock = clock - copy_started_clock;
				if (clock >= MusicManager.getVsqFile().getPreMeasureClocks()) {
					List<VsqEvent> paste = new List<VsqEvent>();
					int count = copied_events.Count;
					for (int i = 0; i < count; i++) {
						VsqEvent item = (VsqEvent)copied_events[i].clone();
						item.Clock = copied_events[i].Clock + dclock;
						paste.Add(item);
					}
					add_event = VsqCommand.generateCommandEventAddRange(
						EditorManager.Selected, paste.ToArray());
				}
			}
			List<TempoTableEntry> copied_tempo = ce.tempo;
			if (copied_tempo.Count != 0) {
				// テンポ変更の貼付けを実行
				int dclock = clock - copy_started_clock;
				int count = copied_tempo.Count;
				int[] clocks = new int[count];
				int[] tempos = new int[count];
				for (int i = 0; i < count; i++) {
					TempoTableEntry item = copied_tempo[i];
					clocks[i] = item.Clock + dclock;
					tempos[i] = item.Tempo;
				}
				CadenciiCommand run = new CadenciiCommand(
					VsqCommand.generateCommandUpdateTempoRange(clocks, clocks, tempos));
				EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
				form.setEdited(true);
				form.refreshScreen();
				return;
			}
			List<TimeSigTableEntry> copied_timesig = ce.timesig;
			if (copied_timesig.Count > 0) {
				// 拍子変更の貼付けを実行
				int bar_count = MusicManager.getVsqFile().getBarCountFromClock(clock);
				int min_barcount = copied_timesig[0].BarCount;
				foreach (var tste in copied_timesig) {
					min_barcount = Math.Min(min_barcount, tste.BarCount);
				}
				int dbarcount = bar_count - min_barcount;
				int count = copied_timesig.Count;
				int[] barcounts = new int[count];
				int[] numerators = new int[count];
				int[] denominators = new int[count];
				for (int i = 0; i < count; i++) {
					TimeSigTableEntry item = copied_timesig[i];
					barcounts[i] = item.BarCount + dbarcount;
					numerators[i] = item.Numerator;
					denominators[i] = item.Denominator;
				}
				CadenciiCommand run = new CadenciiCommand(
					VsqCommand.generateCommandUpdateTimesigRange(
						barcounts, barcounts, numerators, denominators));
				EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
				form.setEdited(true);
				form.refreshScreen();
				return;
			}

			// BPPairの貼付け
			VsqCommand edit_bpcurve = null; // BPListを変更するコマンド
			SortedDictionary<CurveType, VsqBPList> copied_curve = ce.points;
			#if DEBUG
			sout.println("FormMain#pasteEvent; copied_curve.size()=" + copied_curve.Count);
			#endif
			if (copied_curve.Count > 0) {
				int dclock = clock - copy_started_clock;

				SortedDictionary<string, VsqBPList> work = new SortedDictionary<string, VsqBPList>();
				foreach (var curve in copied_curve.Keys) {
					VsqBPList list = copied_curve[curve];
					#if DEBUG
					CDebug.WriteLine("FormMain#pasteEvent; curve=" + curve);
					#endif
					if (curve.isScalar()) {
						continue;
					}
					if (list.size() <= 0) {
						continue;
					}
					if (curve.isAttachNote()) {
						//todo: FormMain+PasteEvent; VibratoRate, VibratoDepthカーブのペースト処理
					} else {
						VsqBPList target = (VsqBPList)MusicManager.getVsqFile().Track[EditorManager.Selected].getCurve(curve.getName()).clone();
						int count = list.size();
						#if DEBUG
						sout.println("FormMain#pasteEvent; list.getCount()=" + count);
						#endif
						int min = list.getKeyClock(0) + dclock;
						int max = list.getKeyClock(count - 1) + dclock;
						int valueAtEnd = target.getValue(max);
						for (int i = 0; i < target.size(); i++) {
							int cl = target.getKeyClock(i);
							if (min <= cl && cl <= max) {
								target.removeElementAt(i);
								i--;
							}
						}
						int lastClock = min;
						for (int i = 0; i < count - 1; i++) {
							lastClock = list.getKeyClock(i) + dclock;
							target.add(lastClock, list.getElementA(i));
						}
						// 最後のやつ
						if (lastClock < max - 1) {
							target.add(max - 1, list.getElementA(count - 1));
						}
						target.add(max, valueAtEnd);
						if (copied_curve.Count == 1) {
							work[form.TrackSelector.getSelectedCurve().getName()] = target;
						} else {
							work[curve.getName()] = target;
						}
					}
				}
				#if DEBUG
				sout.println("FormMain#pasteEvent; work.size()=" + work.Count);
				#endif
				if (work.Count > 0) {
					string[] curves = new string[work.Count];
					VsqBPList[] bplists = new VsqBPList[work.Count];
					int count = -1;
					foreach (var s in work.Keys) {
						count++;
						curves[count] = s;
						bplists[count] = work[s];
					}
					edit_bpcurve = VsqCommand.generateCommandTrackCurveReplaceRange(EditorManager.Selected, curves, bplists);
				}
				EditorManager.itemSelection.clearPoint();
			}

			// ベジエ曲線の貼付け
			CadenciiCommand edit_bezier = null;
			SortedDictionary<CurveType, List<BezierChain>> copied_bezier = ce.beziers;
			#if DEBUG
			sout.println("FormMain#pasteEvent; copied_bezier.size()=" + copied_bezier.Count);
			#endif
			if (copied_bezier.Count > 0) {
				int dclock = clock - copy_started_clock;
				BezierCurves attached_curve = (BezierCurves)MusicManager.getVsqFile().AttachedCurves.get(EditorManager.Selected - 1).clone();
				SortedDictionary<CurveType, List<BezierChain>> command_arg = new SortedDictionary<CurveType, List<BezierChain>>();
				foreach (var curve in copied_bezier.Keys) {
					if (curve.isScalar()) {
						continue;
					}
					foreach (var bc in copied_bezier[curve]) {
						BezierChain bc_copy = (BezierChain)bc.clone();
						foreach (var bp in bc_copy.points) {
							bp.setBase(new PointD(bp.getBase().getX() + dclock, bp.getBase().getY()));
						}
						attached_curve.mergeBezierChain(curve, bc_copy);
					}
					List<BezierChain> arg = new List<BezierChain>();
					foreach (var bc in attached_curve.get(curve)) {
						arg.Add(bc);
					}
					command_arg[curve] = arg;
				}
				edit_bezier = VsqFileEx.generateCommandReplaceAttachedCurveRange(EditorManager.Selected, command_arg);
			}

			int commands = 0;
			commands += (add_event != null) ? 1 : 0;
			commands += (edit_bpcurve != null) ? 1 : 0;
			commands += (edit_bezier != null) ? 1 : 0;

			#if DEBUG
			CDebug.WriteLine("FormMain#pasteEvent; commands=" + commands);
			CDebug.WriteLine("FormMain#pasteEvent; (add_event != null)=" + (add_event != null));
			CDebug.WriteLine("FormMain#pasteEvent; (edit_bpcurve != null)=" + (edit_bpcurve != null));
			CDebug.WriteLine("FormMain#pasteEvent; (edit_bezier != null)=" + (edit_bezier != null));
			#endif
			if (commands == 1) {
				if (add_event != null) {
					CadenciiCommand run = new CadenciiCommand(add_event);
					EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
				} else if (edit_bpcurve != null) {
					CadenciiCommand run = new CadenciiCommand(edit_bpcurve);
					EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
				} else if (edit_bezier != null) {
					EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(edit_bezier));
				}
				MusicManager.getVsqFile().updateTotalClocks();
				form.setEdited(true);
				form.refreshScreen();
			} else if (commands > 1) {
				VsqFileEx work = (VsqFileEx)MusicManager.getVsqFile().clone();
				if (add_event != null) {
					work.executeCommand(add_event);
				}
				if (edit_bezier != null) {
					work.executeCommand(edit_bezier);
				}
				if (edit_bpcurve != null) {
					// edit_bpcurveのVsqCommandTypeはTrackEditCurveRangeしかありえない
					work.executeCommand(edit_bpcurve);
				}
				CadenciiCommand run = VsqFileEx.generateCommandReplace(work);
				EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
				MusicManager.getVsqFile().updateTotalClocks();
				form.setEdited(true);
				form.refreshScreen();
			}
		}

		/// <summary>
		/// アイテムのコピーを行います
		/// </summary>
		public void Copy()
		{
			#if DEBUG
			CDebug.WriteLine("FormMain#copyEvent");
			#endif
			int min = int.MaxValue; // コピーされたアイテムの中で、最小の開始クロック

			if (EditorManager.IsWholeSelectedIntervalEnabled) {
				#if DEBUG
				sout.println("FormMain#copyEvent; selected with CTRL key");
				#endif
				int stdx = form.controller.getStartToDrawX();
				int start_clock = EditorManager.mWholeSelectedInterval.getStart();
				int end_clock = EditorManager.mWholeSelectedInterval.getEnd();
				ClipboardEntry ce = new ClipboardEntry();
				ce.copyStartedClock = start_clock;
				ce.points = new SortedDictionary<CurveType, VsqBPList>();
				ce.beziers = new SortedDictionary<CurveType, List<BezierChain>>();
				for (int i = 0; i < BezierCurves.CURVE_USAGE.Length; i++) {
					CurveType vct = BezierCurves.CURVE_USAGE[i];
					VsqBPList list = MusicManager.getVsqFile().Track[EditorManager.Selected].getCurve(vct.getName());
					if (list == null) {
						continue;
					}
					List<BezierChain> tmp_bezier = new List<BezierChain>();
					CopyCurveCor(EditorManager.Selected,
						vct,
						start_clock,
						end_clock,
						tmp_bezier);
					VsqBPList tmp_bplist = new VsqBPList(list.getName(), list.getDefault(), list.getMinimum(), list.getMaximum());
					int c = list.size();
					for (int j = 0; j < c; j++) {
						int clock = list.getKeyClock(j);
						if (start_clock <= clock && clock <= end_clock) {
							tmp_bplist.add(clock, list.getElement(j));
						} else if (end_clock < clock) {
							break;
						}
					}
					ce.beziers[vct] = tmp_bezier;
					ce.points[vct] = tmp_bplist;
				}

				if (EditorManager.itemSelection.getEventCount() > 0) {
					List<VsqEvent> list = new List<VsqEvent>();
					foreach (var item in EditorManager.itemSelection.getEventIterator()) {
						if (item.original.ID.type == VsqIDType.Anote) {
							min = Math.Min(item.original.Clock, min);
							list.Add((VsqEvent)item.original.clone());
						}
					}
					ce.events = list;
				}
				EditorManager.clipboard.setClipboard(ce);
			} else if (EditorManager.itemSelection.getEventCount() > 0) {
				List<VsqEvent> list = new List<VsqEvent>();
				foreach (var item in EditorManager.itemSelection.getEventIterator()) {
					min = Math.Min(item.original.Clock, min);
					list.Add((VsqEvent)item.original.clone());
				}
				EditorManager.clipboard.setCopiedEvent(list, min);
			} else if (EditorManager.itemSelection.getTempoCount() > 0) {
				List<TempoTableEntry> list = new List<TempoTableEntry>();
				foreach (var item in EditorManager.itemSelection.getTempoIterator()) {
					int key = item.getKey();
					SelectedTempoEntry value = item.getValue();
					min = Math.Min(value.original.Clock, min);
					list.Add((TempoTableEntry)value.original.clone());
				}
				EditorManager.clipboard.setCopiedTempo(list, min);
			} else if (EditorManager.itemSelection.getTimesigCount() > 0) {
				List<TimeSigTableEntry> list = new List<TimeSigTableEntry>();
				foreach (var item in EditorManager.itemSelection.getTimesigIterator()) {
					int key = item.getKey();
					SelectedTimesigEntry value = item.getValue();
					min = Math.Min(value.original.Clock, min);
					list.Add((TimeSigTableEntry)value.original.clone());
				}
				EditorManager.clipboard.setCopiedTimesig(list, min);
			} else if (EditorManager.itemSelection.getPointIDCount() > 0) {
				ClipboardEntry ce = new ClipboardEntry();
				ce.points = new SortedDictionary<CurveType, VsqBPList>();
				ce.beziers = new SortedDictionary<CurveType, List<BezierChain>>();

				ValuePair<int, int> t = form.TrackSelector.getSelectedRegion();
				int start = t.getKey();
				int end = t.getValue();
				ce.copyStartedClock = start;
				List<BezierChain> tmp_bezier = new List<BezierChain>();
				CopyCurveCor(EditorManager.Selected,
					form.TrackSelector.getSelectedCurve(),
					start,
					end,
					tmp_bezier);
				if (tmp_bezier.Count > 0) {
					// ベジエ曲線が1個以上コピーされた場合
					// 範囲内のデータ点を追加する
					ce.beziers[form.TrackSelector.getSelectedCurve()] = tmp_bezier;
					CurveType curve = form.TrackSelector.getSelectedCurve();
					VsqBPList list = MusicManager.getVsqFile().Track[EditorManager.Selected].getCurve(curve.getName());
					if (list != null) {
						VsqBPList tmp_bplist = new VsqBPList(list.getName(), list.getDefault(), list.getMinimum(), list.getMaximum());
						int c = list.size();
						for (int i = 0; i < c; i++) {
							int clock = list.getKeyClock(i);
							if (start <= clock && clock <= end) {
								tmp_bplist.add(clock, list.getElement(i));
							} else if (end < clock) {
								break;
							}
						}
						ce.points[curve] = tmp_bplist;
					}
				} else {
					// ベジエ曲線がコピーされなかった場合
					// EditorManager.selectedPointIDIteratorの中身のみを選択
					CurveType curve = form.TrackSelector.getSelectedCurve();
					VsqBPList list = MusicManager.getVsqFile().Track[EditorManager.Selected].getCurve(curve.getName());
					if (list != null) {
						VsqBPList tmp_bplist = new VsqBPList(curve.getName(), curve.getDefault(), curve.getMinimum(), curve.getMaximum());
						foreach (var id in EditorManager.itemSelection.getPointIDIterator()) {
							VsqBPPairSearchContext cxt = list.findElement(id);
							if (cxt.index >= 0) {
								tmp_bplist.add(cxt.clock, cxt.point.value);
							}
						}
						if (tmp_bplist.size() > 0) {
							ce.copyStartedClock = tmp_bplist.getKeyClock(0);
							ce.points[curve] = tmp_bplist;
						}
					}
				}
				EditorManager.clipboard.setClipboard(ce);
			}
		}

		public void Cut()
		{
			// まずコピー
			Copy();

			int track = EditorManager.Selected;

			// 選択されたノートイベントがあれば、まず、削除を行うコマンドを発行
			VsqCommand delete_event = null;
			bool other_command_executed = false;
			if (EditorManager.itemSelection.getEventCount() > 0) {
				List<int> ids = new List<int>();
				foreach (var item in EditorManager.itemSelection.getEventIterator()) {
					ids.Add(item.original.InternalID);
				}
				delete_event = VsqCommand.generateCommandEventDeleteRange(EditorManager.Selected, ids);
			}

			// Ctrlキーを押しながらドラッグしたか、そうでないかで分岐
			if (EditorManager.IsWholeSelectedIntervalEnabled || EditorManager.itemSelection.getPointIDCount() > 0) {
				int stdx = form.controller.getStartToDrawX();
				int start_clock, end_clock;
				if (EditorManager.IsWholeSelectedIntervalEnabled) {
					start_clock = EditorManager.mWholeSelectedInterval.getStart();
					end_clock = EditorManager.mWholeSelectedInterval.getEnd();
				} else {
					start_clock = form.TrackSelector.getSelectedRegion().getKey();
					end_clock = form.TrackSelector.getSelectedRegion().getValue();
				}

				// クローンを作成
				VsqFileEx work = (VsqFileEx)MusicManager.getVsqFile().clone();
				if (delete_event != null) {
					// 選択されたノートイベントがあれば、クローンに対して削除を実行
					work.executeCommand(delete_event);
				}

				// BPListに削除処理を施す
				for (int i = 0; i < BezierCurves.CURVE_USAGE.Length; i++) {
					CurveType curve = BezierCurves.CURVE_USAGE[i];
					VsqBPList list = work.Track[track].getCurve(curve.getName());
					if (list == null) {
						continue;
					}
					int c = list.size();
					List<long> delete = new List<long>();
					if (EditorManager.IsWholeSelectedIntervalEnabled) {
						// 一括選択モード
						for (int j = 0; j < c; j++) {
							int clock = list.getKeyClock(j);
							if (start_clock <= clock && clock <= end_clock) {
								delete.Add(list.getElementB(j).id);
							} else if (end_clock < clock) {
								break;
							}
						}
					} else {
						// 普通の範囲選択
						foreach (var id in EditorManager.itemSelection.getPointIDIterator()) {
							delete.Add(id);
						}
					}
					VsqCommand tmp = VsqCommand.generateCommandTrackCurveEdit2(track, curve.getName(), delete, new SortedDictionary<int, VsqBPPair>());
					work.executeCommand(tmp);
				}

				// ベジエ曲線に削除処理を施す
				List<CurveType> target_curve = new List<CurveType>();
				if (EditorManager.IsWholeSelectedIntervalEnabled) {
					// ctrlによる全選択モード
					for (int i = 0; i < BezierCurves.CURVE_USAGE.Length; i++) {
						CurveType ct = BezierCurves.CURVE_USAGE[i];
						if (ct.isScalar() || ct.isAttachNote()) {
							continue;
						}
						target_curve.Add(ct);
					}
				} else {
					// 普通の選択モード
					target_curve.Add(form.TrackSelector.getSelectedCurve());
				}
				work.AttachedCurves.get(EditorManager.Selected - 1).deleteBeziers(target_curve, start_clock, end_clock);

				// コマンドを発行し、実行
				CadenciiCommand run = VsqFileEx.generateCommandReplace(work);
				EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
				form.setEdited(true);

				other_command_executed = true;
			} else if (EditorManager.itemSelection.getTempoCount() > 0) {
				// テンポ変更のカット
				int count = -1;
				int[] dum = new int[EditorManager.itemSelection.getTempoCount()];
				int[] clocks = new int[EditorManager.itemSelection.getTempoCount()];
				foreach (var item in EditorManager.itemSelection.getTempoIterator()) {
					int key = item.getKey();
					SelectedTempoEntry value = item.getValue();
					count++;
					dum[count] = -1;
					clocks[count] = value.original.Clock;
				}
				CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandUpdateTempoRange(clocks, clocks, dum));
				EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
				form.setEdited(true);
				other_command_executed = true;
			} else if (EditorManager.itemSelection.getTimesigCount() > 0) {
				// 拍子変更のカット
				int[] barcounts = new int[EditorManager.itemSelection.getTimesigCount()];
				int[] numerators = new int[EditorManager.itemSelection.getTimesigCount()];
				int[] denominators = new int[EditorManager.itemSelection.getTimesigCount()];
				int count = -1;
				foreach (var item in EditorManager.itemSelection.getTimesigIterator()) {
					int key = item.getKey();
					SelectedTimesigEntry value = item.getValue();
					count++;
					barcounts[count] = value.original.BarCount;
					numerators[count] = -1;
					denominators[count] = -1;
				}
				CadenciiCommand run = new CadenciiCommand(
					VsqCommand.generateCommandUpdateTimesigRange(barcounts, barcounts, numerators, denominators));
				EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
				form.setEdited(true);
				other_command_executed = true;
			}

			// 冒頭で作成した音符イベント削除以外に、コマンドが実行されなかった場合
			if (delete_event != null && !other_command_executed) {
				CadenciiCommand run = new CadenciiCommand(delete_event);
				EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
				form.setEdited(true);
			}

			form.refreshScreen();
		}

		public void SelectAll()
		{

			EditorManager.itemSelection.clearEvent();
			EditorManager.itemSelection.clearTempo();
			EditorManager.itemSelection.clearTimesig();
			EditorManager.itemSelection.clearPoint();
			int min = int.MaxValue;
			int max = int.MinValue;
			int premeasure = MusicManager.getVsqFile().getPreMeasureClocks();
			List<int> add_required = new List<int>();
			for (Iterator<VsqEvent> itr = MusicManager.getVsqFile().Track[EditorManager.Selected].getEventIterator(); itr.hasNext(); ) {
				VsqEvent ve = itr.next();
				if (premeasure <= ve.Clock) {
					add_required.Add(ve.InternalID);
					min = Math.Min(min, ve.Clock);
					max = Math.Max(max, ve.Clock + ve.ID.getLength());
				}
			}
			if (add_required.Count > 0) {
				EditorManager.itemSelection.addEventAll(add_required);
			}
			foreach (CurveType vct in BezierCurves.CURVE_USAGE) {
				if (vct.isScalar() || vct.isAttachNote()) {
					continue;
				}
				VsqBPList target = MusicManager.getVsqFile().Track[EditorManager.Selected].getCurve(vct.getName());
				if (target == null) {
					continue;
				}
				int count = target.size();
				if (count >= 1) {
					//int[] keys = target.getKeys();
					int max_key = target.getKeyClock(count - 1);
					max = Math.Max(max, target.getValue(max_key));
					for (int i = 0; i < count; i++) {
						int key = target.getKeyClock(i);
						if (premeasure <= key) {
							min = Math.Min(min, key);
							break;
						}
					}
				}
			}
			if (min < premeasure) {
				min = premeasure;
			}
			if (min < max) {
				//int stdx = EditorManager.startToDrawX;
				//min = xCoordFromClocks( min ) + stdx;
				//max = xCoordFromClocks( max ) + stdx;
				EditorManager.mWholeSelectedInterval = new SelectedRegion(min);
				EditorManager.mWholeSelectedInterval.setEnd(max);
				EditorManager.IsWholeSelectedIntervalEnabled = true;
			}
		}

		public void SelectAllEvent()
		{
			EditorManager.itemSelection.clearTempo();
			EditorManager.itemSelection.clearTimesig();
			EditorManager.itemSelection.clearEvent();
			EditorManager.itemSelection.clearPoint();
			int selected = EditorManager.Selected;
			VsqFileEx vsq = MusicManager.getVsqFile();
			VsqTrack vsq_track = vsq.Track[selected];
			int premeasureclock = vsq.getPreMeasureClocks();
			List<int> add_required = new List<int>();
			for (Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
				VsqEvent ev = itr.next();
				if (ev.ID.type == VsqIDType.Anote && ev.Clock >= premeasureclock) {
					add_required.Add(ev.InternalID);
				}
			}
			if (add_required.Count > 0) {
				EditorManager.itemSelection.addEventAll(add_required);
			}
			form.refreshScreen();
		}

		void CopyCurveCor(
			int track,
			CurveType curve_type,
			int start,
			int end,
			List<BezierChain> copied_chain
		)
		{
			foreach (var bc in MusicManager.getVsqFile().AttachedCurves.get(track - 1).get(curve_type)) {
				int len = bc.points.Count;
				if (len < 2) {
					continue;
				}
				int chain_start = (int)bc.points[0].getBase().getX();
				int chain_end = (int)bc.points[len - 1].getBase().getX();
				BezierChain add = null;
				if (start < chain_start && chain_start < end && end < chain_end) {
					// (1) chain_start ~ end をコピー
					try {
						add = bc.extractPartialBezier(chain_start, end);
					} catch (Exception ex) {
						Logger.write(GetType () + ".copyCurveCor; ex=" + ex + "\n");
						add = null;
					}
				} else if (chain_start <= start && end <= chain_end) {
					// (2) start ~ endをコピー
					try {
						add = bc.extractPartialBezier(start, end);
					} catch (Exception ex) {
						Logger.write(GetType () + ".copyCurveCor; ex=" + ex + "\n");
						add = null;
					}
				} else if (chain_start < start && start < chain_end && chain_end <= end) {
					// (3) start ~ chain_endをコピー
					try {
						add = bc.extractPartialBezier(start, chain_end);
					} catch (Exception ex) {
						Logger.write(GetType () + ".copyCurveCor; ex=" + ex + "\n");
						add = null;
					}
				} else if (start <= chain_start && chain_end <= end) {
					// (4) 全部コピーでOK
					add = (BezierChain)bc.clone();
				}
				if (add != null) {
					copied_chain.Add(add);
				}
			}
		}
	}
}

