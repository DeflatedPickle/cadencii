using System;
using cadencii.vsq;
using System.Collections.Generic;
using cadencii.core;
using System.Text;
using Cadencii.Gui;
using Cadencii.Utilities;

namespace cadencii
{
	public partial class FormMainModel
	{
		public class JobMenuModel
		{
			readonly FormMainModel parent;

			public JobMenuModel (FormMainModel parent)
			{
				this.parent = parent;
			}
			//BOOKMARK: menuJob
			#region menuJob
			public void RunJobReloadVstiCommand()
			{
				//VSTiProxy.ReloadPlugin(); //todo: FormMain+menuJobReloadVsti_Click
			}

			public void RunJobDropDownOpening()
			{
				if (EditorManager.itemSelection.getEventCount() <= 1) {
					parent.form.menuJobConnect.Enabled = false;
				} else {
					// menuJobConnect(音符の結合)がEnableされるかどうかは、選択されている音符がピアノロール上で連続かどうかで決まる
					int[] list = new int[EditorManager.itemSelection.getEventCount()];
					for (int i = 0; i < MusicManager.getVsqFile().Track[EditorManager.Selected].getEventCount(); i++) {
						int count = -1;
						foreach (var item in EditorManager.itemSelection.getEventIterator()) {
							int key = item.original.InternalID;
							count++;
							if (key == MusicManager.getVsqFile().Track[EditorManager.Selected].getEvent(i).InternalID) {
								list[count] = i;
								break;
							}
						}
					}
					bool changed = true;
					while (changed) {
						changed = false;
						for (int i = 0; i < list.Length - 1; i++) {
							if (list[i] > list[i + 1]) {
								int b = list[i];
								list[i] = list[i + 1];
								list[i + 1] = b;
								changed = true;
							}
						}
					}
					bool continued = true;
					for (int i = 0; i < list.Length - 1; i++) {
						if (list[i] + 1 != list[i + 1]) {
							continued = false;
							break;
						}
					}
					parent.form.menuJobConnect.Enabled = continued;
				}

				parent.form.menuJobLyric.Enabled = EditorManager.itemSelection.getLastEvent() != null;
			}

			public void RunJobLyricCommand()
			{
				parent.ImportLyric();
			}

			public void RunJobConnectCommand()
			{
				int count = EditorManager.itemSelection.getEventCount();
				int[] clocks = new int[count];
				VsqID[] ids = new VsqID[count];
				int[] internalids = new int[count];
				int i = -1;
				foreach (var item in EditorManager.itemSelection.getEventIterator()) {
					i++;
					clocks[i] = item.original.Clock;
					ids[i] = (VsqID)item.original.ID.clone();
					internalids[i] = item.original.InternalID;
				}
				bool changed = true;
				while (changed) {
					changed = false;
					for (int j = 0; j < clocks.Length - 1; j++) {
						if (clocks[j] > clocks[j + 1]) {
							int b = clocks[j];
							clocks[j] = clocks[j + 1];
							clocks[j + 1] = b;
							VsqID a = ids[j];
							ids[j] = ids[j + 1];
							ids[j + 1] = a;
							changed = true;
							b = internalids[j];
							internalids[j] = internalids[j + 1];
							internalids[j + 1] = b;
						}
					}
				}

				for (int j = 0; j < ids.Length - 1; j++) {
					ids[j].setLength(clocks[j + 1] - clocks[j]);
				}
				CadenciiCommand run = new CadenciiCommand(
					VsqCommand.generateCommandEventChangeIDContaintsRange(EditorManager.Selected, internalids, ids));
				EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
				parent.form.setEdited(true);
				parent.form.Refresh();
			}

			public void RunJobInsertBarCommand()
			{
				int total_clock = MusicManager.getVsqFile().TotalClocks;
				int total_barcount = MusicManager.getVsqFile().getBarCountFromClock(total_clock) + 1;
				FormInsertBar dlg = null;
				try {
					dlg = ApplicationUIHost.Create<FormInsertBar>(total_barcount);
					int current_clock = EditorManager.getCurrentClock();
					int barcount = MusicManager.getVsqFile().getBarCountFromClock(current_clock);
					int draft = barcount - MusicManager.getVsqFile().getPreMeasure() + 1;
					if (draft <= 0) {
						draft = 1;
					}
					dlg.Position = (draft);

					dlg.Location = parent.GetFormPreferedLocation(dlg);
					var dr = DialogManager.ShowModalDialog(dlg, parent.form);
					if (dr == DialogResult.OK) {
						int pos = dlg.Position + MusicManager.getVsqFile().getPreMeasure() - 1;
						int length = dlg.Length;

						int clock_start = MusicManager.getVsqFile().getClockFromBarCount(pos);
						int clock_end = MusicManager.getVsqFile().getClockFromBarCount(pos + length);
						int dclock = clock_end - clock_start;
						VsqFileEx temp = (VsqFileEx)MusicManager.getVsqFile().clone();

						for (int track = 1; track < temp.Track.Count; track++) {
							BezierCurves newbc = new BezierCurves();
							foreach (CurveType ct in BezierCurves.CURVE_USAGE) {
								int index = ct.getIndex();
								if (index < 0) {
									continue;
								}

								List<BezierChain> list = new List<BezierChain>();
								foreach (var bc in temp.AttachedCurves.get(track - 1).get(ct)) {
									if (bc.size() < 2) {
										continue;
									}
									int chain_start = (int)bc.points[0].getBase().getX();
									int chain_end = (int)bc.points[bc.points.Count - 1].getBase().getX();

									if (clock_start <= chain_start) {
										for (int i = 0; i < bc.points.Count; i++) {
											PointD t = bc.points[i].getBase();
											bc.points[i].setBase(new PointD(t.getX() + dclock, t.getY()));
										}
										list.Add(bc);
									} else if (chain_start < clock_start && clock_start < chain_end) {
										BezierChain adding1 = bc.extractPartialBezier(chain_start, clock_start);
										BezierChain adding2 = bc.extractPartialBezier(clock_start, chain_end);
										for (int i = 0; i < adding2.points.Count; i++) {
											PointD t = adding2.points[i].getBase();
											adding2.points[i].setBase(new PointD(t.getX() + dclock, t.getY()));
										}
										adding1.points[adding1.points.Count - 1].setControlRightType(BezierControlType.None);
										adding2.points[0].setControlLeftType(BezierControlType.None);
										for (int i = 0; i < adding2.points.Count; i++) {
											adding1.points.Add(adding2.points[i]);
										}
										adding1.id = bc.id;
										list.Add(adding1);
									} else {
										list.Add((BezierChain)bc.clone());
									}
								}

								newbc.set(ct, list);
							}
							temp.AttachedCurves.set(track - 1, newbc);
						}

						for (int track = 1; track < MusicManager.getVsqFile().Track.Count; track++) {
							for (int i = 0; i < temp.Track[track].getEventCount(); i++) {
								if (temp.Track[track].getEvent(i).Clock >= clock_start) {
									temp.Track[track].getEvent(i).Clock += dclock;
								}
							}
							foreach (CurveType curve in BezierCurves.CURVE_USAGE) {
								if (curve.isScalar() || curve.isAttachNote()) {
									continue;
								}
								VsqBPList target = temp.Track[track].getCurve(curve.getName());
								VsqBPList src = MusicManager.getVsqFile().Track[track].getCurve(curve.getName());
								target.clear();
								foreach (var key in src.keyClockIterator()) {
									if (key >= clock_start) {
										target.add(key + dclock, src.getValue(key));
									} else {
										target.add(key, src.getValue(key));
									}
								}
							}
						}
						for (int i = 0; i < temp.TempoTable.Count; i++) {
							if (temp.TempoTable[i].Clock >= clock_start) {
								temp.TempoTable[i].Clock = temp.TempoTable[i].Clock + dclock;
							}
						}
						for (int i = 0; i < temp.TimesigTable.Count; i++) {
							if (temp.TimesigTable[i].Clock >= clock_start) {
								temp.TimesigTable[i].Clock = temp.TimesigTable[i].Clock + dclock;
							}
						}
						temp.updateTempoInfo();
						temp.updateTimesigInfo();

						CadenciiCommand run = VsqFileEx.generateCommandReplace(temp);
						EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
						parent.form.setEdited(true);
						parent.form.Refresh();
					}
				} catch (Exception ex) {
					Logger.write(GetType () + ".menuJobInsertBar_Click; ex=" + ex + "\n");
				} finally {
					if (dlg != null) {
						try {
							dlg.Close();
						} catch (Exception ex2) {
							Logger.write(GetType () + ".menuJobInsertBar_Click; ex=" + ex2 + "\n");
						}
					}
				}
			}

			public void RunJobChangePreMeasureCommand()
			{
				InputBox dialog = null;
				try {
					dialog = ApplicationUIHost.Create<InputBox>(_("input pre-measure"));
					int old_pre_measure = MusicManager.getVsqFile().getPreMeasure();
					dialog.setResult(old_pre_measure + "");
					dialog.Location = parent.GetFormPreferedLocation(dialog);
					var ret = DialogManager.ShowModalDialog(dialog, parent.form);
					if (ret == DialogResult.OK) {
						string str_result = dialog.getResult();
						int result = old_pre_measure;
						try {
							result = int.Parse(str_result);
						} catch (Exception ex) {
							result = old_pre_measure;
						}
						if (result < ApplicationGlobal.MIN_PRE_MEASURE) {
							result = ApplicationGlobal.MIN_PRE_MEASURE;
						}
						if (result > ApplicationGlobal.MAX_PRE_MEASURE) {
							result = ApplicationGlobal.MAX_PRE_MEASURE;
						}
						if (old_pre_measure != result) {
							CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandChangePreMeasure(result));
							EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
							MusicManager.getVsqFile().updateTotalClocks();
							parent.form.refreshScreen(true);
							parent.form.setEdited(true);
						}
					}
				} catch (Exception ex) {
					return;
				} finally {
					if (dialog != null) {
						try {
							dialog.Close();
						} catch (Exception ex2) {
						}
					}
				}
			}

			public void RunJobDeleteBarCommand()
			{
				int total_clock = MusicManager.getVsqFile().TotalClocks;
				int total_barcount = MusicManager.getVsqFile().getBarCountFromClock(total_clock) + 1;
				int clock = EditorManager.getCurrentClock();
				int barcount = MusicManager.getVsqFile().getBarCountFromClock(clock);
				FormDeleteBar dlg = null;
				try {
					dlg = ApplicationUIHost.Create<FormDeleteBar>(total_barcount);
					int draft = barcount - MusicManager.getVsqFile().getPreMeasure() + 1;
					if (draft <= 0) {
						draft = 1;
					}
					dlg.Start = (draft);
					dlg.End = (draft + 1);

					dlg.Location = parent.GetFormPreferedLocation(dlg);
					var dr = DialogManager.ShowModalDialog(dlg, parent.form);
					if (dr == DialogResult.OK) {
						VsqFileEx temp = (VsqFileEx)MusicManager.getVsqFile().clone();
						int start = dlg.Start + MusicManager.getVsqFile().getPreMeasure() - 1;
						int end = dlg.End + MusicManager.getVsqFile().getPreMeasure() - 1;
						#if DEBUG
						CDebug.WriteLine("FormMain+menuJobDeleteBar_Click");
						CDebug.WriteLine("    start,end=" + start + "," + end);
						#endif
						int clock_start = temp.getClockFromBarCount(start);
						int clock_end = temp.getClockFromBarCount(end);
						int dclock = clock_end - clock_start;
						for (int track = 1; track < temp.Track.Count; track++) {
							BezierCurves newbc = new BezierCurves();
							for (int j = 0; j < BezierCurves.CURVE_USAGE.Length; j++) {
								CurveType ct = BezierCurves.CURVE_USAGE[j];
								int index = ct.getIndex();
								if (index < 0) {
									continue;
								}

								List<BezierChain> list = new List<BezierChain>();
								foreach (var bc in temp.AttachedCurves.get(track - 1).get(ct)) {
									if (bc.size() < 2) {
										continue;
									}
									int chain_start = (int)bc.points[0].getBase().getX();
									int chain_end = (int)bc.points[bc.points.Count - 1].getBase().getX();

									if (clock_start < chain_start && chain_start < clock_end && clock_end < chain_end) {
										BezierChain adding = bc.extractPartialBezier(clock_end, chain_end);
										adding.id = bc.id;
										for (int i = 0; i < adding.points.Count; i++) {
											PointD t = adding.points[i].getBase();
											adding.points[i].setBase(new PointD(t.getX() - dclock, t.getY()));
										}
										list.Add(adding);
									} else if (chain_start < clock_start && clock_end < chain_end) {
										BezierChain adding1 = bc.extractPartialBezier(chain_start, clock_start);
										adding1.id = bc.id;
										adding1.points[adding1.points.Count - 1].setControlRightType(BezierControlType.None);
										BezierChain adding2 = bc.extractPartialBezier(clock_end, chain_end);
										adding2.points[0].setControlLeftType(BezierControlType.None);
										PointD t = adding2.points[0].getBase();
										adding2.points[0].setBase(new PointD(t.getX() - dclock, t.getY()));
										adding1.points.Add(adding2.points[0]);
										for (int i = 1; i < adding2.points.Count; i++) {
											t = adding2.points[i].getBase();
											adding2.points[i].setBase(new PointD(t.getX() - dclock, t.getY()));
											adding1.points.Add(adding2.points[i]);
										}
										list.Add(adding1);
									} else if (chain_start < clock_start && clock_start < chain_end && chain_end < clock_end) {
										BezierChain adding = bc.extractPartialBezier(chain_start, clock_start);
										adding.id = bc.id;
										list.Add(adding);
									} else if (clock_end <= chain_start || chain_end <= clock_start) {
										if (clock_end <= chain_start) {
											for (int i = 0; i < bc.points.Count; i++) {
												PointD t = bc.points[i].getBase();
												bc.points[i].setBase(new PointD(t.getX() - dclock, t.getY()));
											}
										}
										list.Add((BezierChain)bc.clone());
									}
								}

								newbc.set(ct, list);
							}
							temp.AttachedCurves.set(track - 1, newbc);
						}

						temp.removePart(clock_start, clock_end);
						CadenciiCommand run = VsqFileEx.generateCommandReplace(temp);
						EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
						parent.form.setEdited(true);
						parent.form.Refresh();
					}
				} catch (Exception ex) {
					Logger.write(GetType () + ".menuJobDeleteBar_Click; ex=" + ex + "\n");
				} finally {
					if (dlg != null) {
						try {
							dlg.Close();
						} catch (Exception ex2) {
							Logger.write(GetType () + ".menuJobDeleteBar_Click; ex=" + ex2 + "\n");
						}
					}
				}
			}

			public void RunJobNormalizeCommand()
			{
				VsqFile work = (VsqFile)MusicManager.getVsqFile().clone();
				int track = EditorManager.Selected;
				bool changed = true;
				bool total_changed = false;

				// 最初、開始時刻が同じになっている奴を削除
				while (changed) {
					changed = false;
					for (int i = 0; i < work.Track[track].getEventCount() - 1; i++) {
						int clock = work.Track[track].getEvent(i).Clock;
						int id = work.Track[track].getEvent(i).InternalID;
						for (int j = i + 1; j < work.Track[track].getEventCount(); j++) {
							if (clock == work.Track[track].getEvent(j).Clock) {
								if (id < work.Track[track].getEvent(j).InternalID) { //内部IDが小さい＝より高年齢（音符追加時刻が古い）
									work.Track[track].removeEvent(i);
								} else {
									work.Track[track].removeEvent(j);
								}
								changed = true;
								total_changed = true;
								break;
							}
						}
						if (changed) {
							break;
						}
					}
				}

				changed = true;
				while (changed) {
					changed = false;
					for (int i = 0; i < work.Track[track].getEventCount() - 1; i++) {
						int start_clock = work.Track[track].getEvent(i).Clock;
						int end_clock = work.Track[track].getEvent(i).ID.getLength() + start_clock;
						for (int j = i + 1; j < work.Track[track].getEventCount(); j++) {
							int this_start_clock = work.Track[track].getEvent(j).Clock;
							if (this_start_clock < end_clock) {
								work.Track[track].getEvent(i).ID.setLength(this_start_clock - start_clock);
								changed = true;
								total_changed = true;
							}
						}
					}
				}

				if (total_changed) {
					CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandReplace(work));
					EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
					parent.form.setEdited(true);
					parent.form.refreshScreen();
				}
			}

			public void RunJobRandomizeCommand()
			{
				FormRandomize dlg = null;
				try {
					dlg = ApplicationUIHost.Create<FormRandomize>();
					dlg.Location = parent.GetFormPreferedLocation(dlg);
					var dr = DialogManager.ShowModalDialog(dlg, parent.form);
					if (dr == DialogResult.OK) {
						VsqFileEx vsq = MusicManager.getVsqFile();
						int preMeasure = vsq.getPreMeasure();
						int startBar = dlg.getStartBar() + (preMeasure - 1);
						int startBeat = dlg.getStartBeat() - 1;
						int endBar = dlg.getEndBar() + (preMeasure - 1);
						int endBeat = dlg.getEndBeat();
						int startBarClock = vsq.getClockFromBarCount(startBar);
						int endBarClock = vsq.getClockFromBarCount(endBar);
						Timesig startTimesig = vsq.getTimesigAt(startBarClock);
						Timesig endTimesig = vsq.getTimesigAt(endBarClock);
						int startClock = startBarClock + startBeat * 480 * 4 / startTimesig.denominator;
						int endClock = endBarClock + endBeat * 480 * 4 / endTimesig.denominator;

						int selected = EditorManager.Selected;
						VsqTrack vsq_track = vsq.Track[selected];
						VsqTrack work = (VsqTrack)vsq_track.clone();
						Random r = new Random();

						// 音符位置のシフト
						#region 音符位置のシフト
						if (dlg.isPositionRandomizeEnabled()) {
							int[] sigmaPreset = new int[] { 10, 20, 30, 60, 120 };
							int sigma = sigmaPreset[dlg.getPositionRandomizeValue() - 1]; // 標準偏差

							int count = work.getEventCount(); // イベントの個数
							int lastItemIndex = -1;  // 直前に処理した音符イベントのインデクス
							int thisItemIndex = -1;  // 処理中の音符イベントのインデクス
							int nextItemIndex = -1;  // 処理中の音符イベントの次の音符イベントのインデクス
							double sqrt2 = Math.Sqrt(2.0);
							int clockPreMeasure = vsq.getPreMeasureClocks(); // プリメジャーいちでのゲートタイム

							while (true) {
								// nextItemIndexを決定
								if (nextItemIndex != -2) {
									int start = nextItemIndex + 1;
									nextItemIndex = -2;  // -2は、トラックの最後まで走査し終わった、という意味
									for (int i = start; i < count; i++) {
										if (work.getEvent(i).ID.type == VsqIDType.Anote) {
											nextItemIndex = i;
											break;
										}
									}
								}

								if (thisItemIndex >= 0) {
									// ここにメインの処理
									VsqEvent lastItem = lastItemIndex >= 0 ? work.getEvent(lastItemIndex) : null;
									VsqEvent thisItem = work.getEvent(thisItemIndex);
									VsqEvent nextItem = nextItemIndex >= 0 ? work.getEvent(nextItemIndex) : null;
									int lastItemClock = lastItem == null ? 0 : lastItem.Clock;
									int lastItemLength = lastItem == null ? 0 : lastItem.ID.getLength();

									int clock = thisItem.Clock;
									int length = thisItem.ID.getLength();
									if (startClock <= thisItem.Clock && thisItem.Clock + thisItem.ID.getLength() <= endClock) {
										int draftClock = 0;
										int draftLength = 0;
										int draftLastItemLength = lastItemLength;
										// 音符のめり込み等のチェックをクリアするまで、draft(Clock|Length|LastItemLength)をトライ＆エラーで決定する
										while (true) {
											int x = 3 * sigma;
											while (Math.Abs(x) > 2 * sigma) {
												double d = r.NextDouble();
												double y = (d - 0.5) * 2.0;
												x = (int)(sigma * sqrt2 * math.erfinv(y));
											}
											draftClock = clock + x;
											draftLength = clock + length - draftClock;
											if (lastItem != null) {
												if (clock == lastItemClock + lastItemLength) {
													// 音符が連続していた場合
													draftLastItemLength = lastItem.ID.getLength() + x;
												}
											}
											// 音符がめり込んだりしてないかどうかをチェック
											if (draftClock < clockPreMeasure) {
												continue;
											}
											if (lastItem != null) {
												if (clock != lastItemClock + lastItemLength) {
													// 音符が連続していなかった場合に、直前の音符にめり込んでいないかどうか
													if (draftClock + draftLength < lastItem.Clock + lastItem.ID.getLength()) {
														continue;
													}
												}
											}
											// チェックにクリアしたのでループを抜ける
											break;
										}
										// draft*の値を適用
										thisItem.Clock = draftClock;
										thisItem.ID.setLength(draftLength);
										if (lastItem != null) {
											lastItem.ID.setLength(draftLastItemLength);
										}
									} else if (endClock < thisItem.Clock) {
										break;
									}
								}

								// インデクスを移す
								lastItemIndex = thisItemIndex;
								thisItemIndex = nextItemIndex;

								if (lastItemIndex == -2 && thisItemIndex == -2 && nextItemIndex == -2) {
									// トラックの最後まで走査し終わったので抜ける
									break;
								}
							}
						}
						#endregion

						// ピッチベンドのランダマイズ
						#region ピッチベンドのランダマイズ
						if (dlg.isPitRandomizeEnabled()) {
							int pattern = dlg.getPitRandomizePattern();
							int value = dlg.getPitRandomizeValue();
							double order = 1.0 / Math.Pow(2.0, 5.0 - value);
							int[] patternPreset = pattern == 1 ? Utility.getRandomizePitPattern1() : pattern == 2 ? Utility.getRandomizePitPattern2() : Utility.getRandomizePitPattern3();
							int resolution = dlg.getResolution();
							VsqBPList pit = work.getCurve("pit");
							VsqBPList pbs = work.getCurve("pbs");
							int pbsAtStart = pbs.getValue(startClock);
							int pbsAtEnd = pbs.getValue(endClock);

							// startClockからendClock - 1までのpit, pbsをクリアする
							int count = pit.size();
							for (int i = count - 1; i >= 0; i--) {
								int keyClock = pit.getKeyClock(i);
								if (startClock <= keyClock && keyClock < endClock) {
									pit.removeElementAt(i);
								}
							}
							count = pbs.size();
							for (int i = count - 1; i >= 0; i--) {
								int keyClock = pbs.getKeyClock(i);
								if (startClock <= keyClock && keyClock < endClock) {
									pbs.removeElementAt(i);
								}
							}

							// pbsをデフォルト値にする
							if (pbsAtStart != 2) {
								pbs.add(startClock, 2);
							}
							if (pbsAtEnd != 2) {
								pbs.add(endClock, pbsAtEnd);
							}

							StringBuilder sb = new StringBuilder();
							count = pit.size();
							bool first = true;
							for (int i = 0; i < count; i++) {
								int clock = pit.getKeyClock(i);
								if (clock < startClock) {
									int v = pit.getElementA(i);
									sb.Append((first ? "" : ",") + (clock + "=" + v));
									first = false;
								} else if (clock <= endClock) {
									break;
								}
							}
							double d = r.NextDouble();
							int start = (int)(d * (patternPreset.Length - 1));
							for (int clock = startClock; clock < endClock; clock += resolution) {
								int copyIndex = start + (clock - startClock);
								int odd = copyIndex / patternPreset.Length;
								copyIndex = copyIndex - patternPreset.Length * odd;
								int v = (int)(patternPreset[copyIndex] * order);
								sb.Append((first ? "" : ",") + (clock + "=" + v));
								first = false;
								//pit.add( clock, v );
							}
							for (int i = 0; i < count; i++) {
								int clock = pit.getKeyClock(i);
								if (endClock <= clock) {
									int v = pit.getElementA(i);
									sb.Append((first ? "" : ",") + (clock + "=" + v));
									first = false;
								}
							}
							pit.setData(sb.ToString());
						}
						#endregion

						CadenciiCommand run = VsqFileEx.generateCommandTrackReplace(selected, work, vsq.AttachedCurves.get(selected - 1));
						EditorManager.editHistory.register(vsq.executeCommand(run));
						parent.form.setEdited(true);
					}
				} catch (Exception ex) {
					Logger.write(GetType () + ".menuJobRandomize_Click; ex=" + ex + "\n");
					Logger.StdErr("FormMain#menuJobRandomize_Click; ex=" + ex);
				} finally {
					if (dlg != null) {
						try {
							dlg.Close();
						} catch (Exception ex2) {
							Logger.write(GetType () + ".menuJobRandomize_Click; ex=" + ex2 + "\n");
							Logger.StdErr("FormMain#menuJobRandomize; ex2=" + ex2);
						}
					}
				}
			}

			#endregion

		}
	}
}
