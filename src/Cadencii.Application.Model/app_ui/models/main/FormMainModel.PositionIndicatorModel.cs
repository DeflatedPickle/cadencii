using System;
using Cadencii.Gui;
using System.Media;
using Cadencii.Media.Vsq;
using cadencii.core;
using Cadencii.Utilities;
using Cadencii.Gui.Toolkit;
using cadencii;
using Cadencii.Application.Forms;
using Cadencii.Application.Media;
using Cadencii.Media;

namespace Cadencii.Application.Models
{

	public partial class FormMainModel
	{
		public class PositionIndicatorModel
		{
			readonly FormMainModel parent;

			public PositionIndicatorModel (FormMainModel parent)
			{
				this.parent = parent;
			}

			/// <summary>
			/// PositionIndicatorのマウスモード
			/// </summary>
			PositionIndicatorMouseDownMode mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.NONE;

			int mTempoDraggingDeltaClock = 0;
			int mTimesigDraggingDeltaClock = 0;
			/// <summary>
			/// PositionIndicatorに表示しているポップアップのクロック位置
			/// </summary>
			private int mPositionIndicatorPopupShownClock;

			//BOOKMARK: picturePositionIndicator
			#region picturePositionIndicator
			public void RunMouseWheel(MouseEventArgs e)
			{
				#if DEBUG
				Logger.StdOut("FormMain#picturePositionIndicator_MouseWheel");
				#endif
				parent.form.hScroll.Value = parent.form.computeScrollValueFromWheelDelta(e.Delta);
			}

			public void RunMouseClick(MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Right && 0 < e.Y && e.Y <= 18 && EditorManager.keyWidth < e.X) {
					// クリックされた位置でのクロックを保存
					int clock = EditorManager.clockFromXCoord(e.X);
					int unit = EditorManager.getPositionQuantizeClock();
					clock = FormMainModel.Quantize(clock, unit);
					if (clock < 0) {
						clock = 0;
					}
					mPositionIndicatorPopupShownClock = clock;
					parent.form.cMenuPositionIndicator.Show(parent.form.picturePositionIndicator, e.X, e.Y);
				}
			}

			public void RunMouseDoubleClick(MouseEventArgs e)
			{
				if (e.X < EditorManager.keyWidth || parent.form.Width - 3 < e.X) {
					return;
				}
				if (e.Button == MouseButtons.Left) {
					VsqFileEx vsq = MusicManager.getVsqFile();
					if (18 < e.Y && e.Y <= 32) {
						#region テンポの変更
						#if DEBUG
						CDebug.WriteLine("TempoChange");
						#endif
						EditorManager.itemSelection.clearEvent();
						EditorManager.itemSelection.clearTimesig();

						if (EditorManager.itemSelection.getTempoCount() > 0) {
							#region テンポ変更があった場合
							int index = -1;
							int clock = EditorManager.itemSelection.getLastTempoClock();
							for (int i = 0; i < vsq.TempoTable.Count; i++) {
								if (clock == vsq.TempoTable[i].Clock) {
									index = i;
									break;
								}
							}
							if (index >= 0) {
								if (EditorManager.SelectedTool == EditTool.ERASER) {
									#region ツールがEraser
									if (vsq.TempoTable[index].Clock == 0) {
										string msg = _("Cannot remove first symbol of track!");
										parent.form.statusLabel.Text = msg;
										SystemSounds.Asterisk.Play();
										return;
									}
									CadenciiCommand run = new CadenciiCommand(
										VsqCommand.generateCommandUpdateTempo(vsq.TempoTable[index].Clock,
											vsq.TempoTable[index].Clock,
											-1));
									EditorManager.editHistory.register(vsq.executeCommand(run));
									parent.form.setEdited(true);
									#endregion
								} else {
									#region ツールがEraser以外
									TempoTableEntry tte = vsq.TempoTable[index];
									EditorManager.itemSelection.clearTempo();
									EditorManager.itemSelection.addTempo(tte.Clock);
									int bar_count = vsq.getBarCountFromClock(tte.Clock);
									int bar_top_clock = vsq.getClockFromBarCount(bar_count);
									//int local_denominator, local_numerator;
									Timesig timesig = vsq.getTimesigAt(tte.Clock);
									int clock_per_beat = 480 * 4 / timesig.denominator;
									int clocks_in_bar = tte.Clock - bar_top_clock;
									int beat_in_bar = clocks_in_bar / clock_per_beat + 1;
									int clocks_in_beat = clocks_in_bar - (beat_in_bar - 1) * clock_per_beat;
									FormTempoConfig dlg = null;
									try {
										dlg = ApplicationUIHost.Create<FormTempoConfig>(bar_count, beat_in_bar, timesig.numerator, clocks_in_beat, clock_per_beat, (float)(6e7 / tte.Tempo), MusicManager.getVsqFile().getPreMeasure());
										dlg.Location = parent.GetFormPreferedLocation(dlg);
										var dr = DialogManager.ShowModalDialog(dlg, parent.form);
										if (dr == Cadencii.Gui.DialogResult.OK) {
											int new_beat = dlg.getBeatCount();
											int new_clocks_in_beat = dlg.getClock();
											int new_clock = bar_top_clock + (new_beat - 1) * clock_per_beat + new_clocks_in_beat;
											CadenciiCommand run = new CadenciiCommand(
												VsqCommand.generateCommandUpdateTempo(new_clock, new_clock, (int)(6e7 / (double)dlg.getTempo())));
											EditorManager.editHistory.register(vsq.executeCommand(run));
											parent.form.setEdited(true);
											parent.form.refreshScreen();
										}
									} catch (Exception ex) {
										Logger.write(GetType () + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex + "\n");
										Logger.StdErr("FormMain#picturePositionIndicator_MouseDoubleClick; ex=" + ex);
									} finally {
										if (dlg != null) {
											try {
												dlg.Close();
											} catch (Exception ex2) {
												Logger.write(GetType () + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex2 + "\n");
												Logger.StdErr("FormMain#picturePositionIndicator_MouseDoubleClick; ex2=" + ex2);
											}
										}
									}
									#endregion
								}
							}
							#endregion
						} else {
							#region テンポ変更がなかった場合
							EditorManager.itemSelection.clearEvent();
							EditorManager.itemSelection.clearTempo();
							EditorManager.itemSelection.clearTimesig();
							EditTool selected = EditorManager.SelectedTool;
							if (selected == EditTool.PENCIL ||
								selected == EditTool.LINE) {
								int changing_clock = EditorManager.clockFromXCoord(e.X);
								int changing_tempo = vsq.getTempoAt(changing_clock);
								int bar_count;
								int bar_top_clock;
								int local_denominator, local_numerator;
								bar_count = vsq.getBarCountFromClock(changing_clock);
								bar_top_clock = vsq.getClockFromBarCount(bar_count);
								int index2 = -1;
								for (int i = 0; i < vsq.TimesigTable.Count; i++) {
									if (vsq.TimesigTable[i].BarCount > bar_count) {
										index2 = i;
										break;
									}
								}
								if (index2 >= 1) {
									local_denominator = vsq.TimesigTable[index2 - 1].Denominator;
									local_numerator = vsq.TimesigTable[index2 - 1].Numerator;
								} else {
									local_denominator = vsq.TimesigTable[0].Denominator;
									local_numerator = vsq.TimesigTable[0].Numerator;
								}
								int clock_per_beat = 480 * 4 / local_denominator;
								int clocks_in_bar = changing_clock - bar_top_clock;
								int beat_in_bar = clocks_in_bar / clock_per_beat + 1;
								int clocks_in_beat = clocks_in_bar - (beat_in_bar - 1) * clock_per_beat;
								FormTempoConfig dlg = null;
								try {
									dlg = ApplicationUIHost.Create<FormTempoConfig>(bar_count - vsq.getPreMeasure() + 1,
										beat_in_bar,
										local_numerator,
										clocks_in_beat,
										clock_per_beat,
										(float)(6e7 / changing_tempo),
										vsq.getPreMeasure());
									dlg.Location = parent.GetFormPreferedLocation(dlg);
									var dr = DialogManager.ShowModalDialog(dlg, parent.form);
									if (dr == Cadencii.Gui.DialogResult.OK) {
										int new_beat = dlg.getBeatCount();
										int new_clocks_in_beat = dlg.getClock();
										int new_clock = bar_top_clock + (new_beat - 1) * clock_per_beat + new_clocks_in_beat;
										#if DEBUG
										CDebug.WriteLine("    new_beat=" + new_beat);
										CDebug.WriteLine("    new_clocks_in_beat=" + new_clocks_in_beat);
										CDebug.WriteLine("    changing_clock=" + changing_clock);
										CDebug.WriteLine("    new_clock=" + new_clock);
										#endif
										CadenciiCommand run = new CadenciiCommand(
											VsqCommand.generateCommandUpdateTempo(new_clock, new_clock, (int)(6e7 / (double)dlg.getTempo())));
										EditorManager.editHistory.register(vsq.executeCommand(run));
										parent.form.setEdited(true);
										parent.form.refreshScreen();
									}
								} catch (Exception ex) {
									Logger.write(GetType () + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex + "\n");
								} finally {
									if (dlg != null) {
										try {
											dlg.Close();
										} catch (Exception ex2) {
											Logger.write(GetType () + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex2 + "\n");
										}
									}
								}
							}
							#endregion
						}
						mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.NONE;
						#endregion
					} else if (32 < e.Y && e.Y <= parent.form.picturePositionIndicator.Height - 1) {
						#region 拍子の変更
						EditorManager.itemSelection.clearEvent();
						EditorManager.itemSelection.clearTempo();
						if (EditorManager.itemSelection.getTimesigCount() > 0) {
							#region 拍子変更があった場合
							int index = 0;
							int last_barcount = EditorManager.itemSelection.getLastTimesigBarcount();
							for (int i = 0; i < vsq.TimesigTable.Count; i++) {
								if (vsq.TimesigTable[i].BarCount == last_barcount) {
									index = i;
									break;
								}
							}
							if (EditorManager.SelectedTool == EditTool.ERASER) {
								#region ツールがEraser
								if (vsq.TimesigTable[index].Clock == 0) {
									string msg = _("Cannot remove first symbol of track!");
									parent.form.statusLabel.Text = msg;
									SystemSounds.Asterisk.Play();
									return;
								}
								int barcount = vsq.TimesigTable[index].BarCount;
								CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandUpdateTimesig(barcount, barcount, -1, -1));
								EditorManager.editHistory.register(vsq.executeCommand(run));
								parent.form.setEdited(true);
								#endregion
							} else {
								#region ツールがEraser以外
								int pre_measure = vsq.getPreMeasure();
								int clock = EditorManager.clockFromXCoord(e.X);
								int bar_count = vsq.getBarCountFromClock(clock);
								int total_clock = vsq.TotalClocks;
								Timesig timesig = vsq.getTimesigAt(clock);
								bool num_enabled = !(bar_count == 0);
								FormBeatConfigController dlg = null;
								try {
									dlg = new FormBeatConfigController(c => ApplicationUIHost.Create<FormBeatConfigUi> (c), bar_count - pre_measure + 1, timesig.numerator, timesig.denominator, num_enabled, pre_measure);
									var dlgUi = dlg.getUi ();
									var p = parent.GetFormPreferedLocation (dlgUi.Width, dlgUi.Height);
									dlgUi.Location = new Point (p.X, p.Y);
									var dr = DialogManager.ShowModalDialog(dlg.getUi(), parent.form);
									if (dr == Cadencii.Gui.DialogResult.OK) {
										if (dlg.isEndSpecified()) {
											int[] new_barcounts = new int[2];
											int[] numerators = new int[2];
											int[] denominators = new int[2];
											int[] barcounts = new int[2];
											new_barcounts[0] = dlg.getStart() + pre_measure - 1;
											new_barcounts[1] = dlg.getEnd() + pre_measure - 1;
											numerators[0] = dlg.getNumerator();
											denominators[0] = dlg.getDenominator();
											numerators[1] = timesig.numerator;
											denominators[1] = timesig.denominator;
											barcounts[0] = bar_count;
											barcounts[1] = dlg.getEnd() + pre_measure - 1;
											CadenciiCommand run = new CadenciiCommand(
												VsqCommand.generateCommandUpdateTimesigRange(barcounts, new_barcounts, numerators, denominators));
											EditorManager.editHistory.register(vsq.executeCommand(run));
											parent.form.setEdited(true);
										} else {
											#if DEBUG
											Logger.StdOut("picturePositionIndicator_MouseDoubleClick");
											Logger.StdOut("    bar_count=" + bar_count);
											Logger.StdOut("    dlg.Start+pre_measure-1=" + (dlg.getStart() + pre_measure - 1));
											#endif
											CadenciiCommand run = new CadenciiCommand(
												VsqCommand.generateCommandUpdateTimesig(bar_count,
													dlg.getStart() + pre_measure - 1,
													dlg.getNumerator(),
													dlg.getDenominator()));
											EditorManager.editHistory.register(vsq.executeCommand(run));
											parent.form.setEdited(true);
										}
									}
								} catch (Exception ex) {
									Logger.write(GetType () + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex + "\n");
									Logger.StdErr("FormMain#picturePositionIndicator_MouseDoubleClick; ex=" + ex);
								} finally {
									if (dlg != null) {
										try {
											dlg.close();
										} catch (Exception ex2) {
											Logger.write(GetType () + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex2 + "\n");
											Logger.StdErr("FormMain#picturePositionIndicator_MouseDoubleClic; ex2=" + ex2);
										}
									}
								}
								#endregion
							}
							#endregion
						} else {
							#region 拍子変更がなかった場合
							EditorManager.itemSelection.clearEvent();
							EditorManager.itemSelection.clearTempo();
							EditorManager.itemSelection.clearTimesig();
							EditTool selected = EditorManager.SelectedTool;
							if (selected == EditTool.PENCIL ||
								selected == EditTool.LINE) {
								int pre_measure = MusicManager.getVsqFile().getPreMeasure();
								int clock = EditorManager.clockFromXCoord(e.X);
								int bar_count = MusicManager.getVsqFile().getBarCountFromClock(clock);
								int numerator, denominator;
								Timesig timesig = MusicManager.getVsqFile().getTimesigAt(clock);
								int total_clock = MusicManager.getVsqFile().TotalClocks;
								//int max_barcount = EditorManager.VsqFile.getBarCountFromClock( total_clock ) - pre_measure + 1;
								//int min_barcount = 1;
								#if DEBUG
								CDebug.WriteLine("FormMain.picturePositionIndicator_MouseClick; bar_count=" + (bar_count - pre_measure + 1));
								#endif
								FormBeatConfigController dlg = null;
								try {
									dlg = new FormBeatConfigController(c => ApplicationUIHost.Create<FormBeatConfigUi> (c), bar_count - pre_measure + 1, timesig.numerator, timesig.denominator, true, pre_measure);
									var dlgUi = dlg.getUi ();
									var p = parent.GetFormPreferedLocation(dlgUi.Width, dlgUi.Height);
									dlgUi.Location = new Point (p.X, p.Y);
									var dr = DialogManager.ShowModalDialog(dlg.getUi(), parent.form);
									if (dr == Cadencii.Gui.DialogResult.OK) {
										if (dlg.isEndSpecified()) {
											int[] new_barcounts = new int[2];
											int[] numerators = new int[2];
											int[] denominators = new int[2];
											int[] barcounts = new int[2];
											new_barcounts[0] = dlg.getStart() + pre_measure - 1;
											new_barcounts[1] = dlg.getEnd() + pre_measure - 1 + 1;
											numerators[0] = dlg.getNumerator();
											numerators[1] = timesig.numerator;

											denominators[0] = dlg.getDenominator();
											denominators[1] = timesig.denominator;

											barcounts[0] = dlg.getStart() + pre_measure - 1;
											barcounts[1] = dlg.getEnd() + pre_measure - 1 + 1;
											CadenciiCommand run = new CadenciiCommand(
												VsqCommand.generateCommandUpdateTimesigRange(barcounts, new_barcounts, numerators, denominators));
											EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
											parent.form.setEdited(true);
										} else {
											CadenciiCommand run = new CadenciiCommand(
												VsqCommand.generateCommandUpdateTimesig(bar_count,
													dlg.getStart() + pre_measure - 1,
													dlg.getNumerator(),
													dlg.getDenominator()));
											EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
											parent.form.setEdited(true);
										}
									}
								} catch (Exception ex) {
									Logger.write(GetType () + ".picutrePositionIndicator_MouseDoubleClick; ex=" + ex + "\n");
								} finally {
									if (dlg != null) {
										try {
											dlg.close();
										} catch (Exception ex2) {
											Logger.write(GetType () + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex2 + "\n");
										}
									}
								}
							}
							#endregion
						}
						mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.NONE;
						#endregion
					}
					parent.form.picturePositionIndicator.Refresh();
					parent.form.pictPianoRoll.Refresh();
				}
			}

			public void RunMouseDown(MouseEventArgs e)
			{
				if (e.X < EditorManager.keyWidth || parent.form.Width - 3 < e.X) {
					return;
				}

				mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.NONE;
				Keys modifiers = GuiHost.ModifierKeys;
				VsqFileEx vsq = MusicManager.getVsqFile();
				if (e.Button == MouseButtons.Left) {
					if (0 <= e.Y && e.Y <= 18) {
						#region スタート/エンドマーク
						int tolerance = EditorManager.editorConfig.PxTolerance;
						int start_marker_width = Resources.start_marker.Width;
						int end_marker_width = Resources.end_marker.Width;
						int startx = EditorManager.xCoordFromClocks(vsq.config.StartMarker);
						int endx = EditorManager.xCoordFromClocks(vsq.config.EndMarker);

						// マウスの当たり判定が重なるようなら，判定幅を最小にする
						int start0 = startx - tolerance;
						int start1 = startx + start_marker_width + tolerance;
						int end0 = endx - end_marker_width - tolerance;
						int end1 = endx + tolerance;
						if (vsq.config.StartMarkerEnabled && vsq.config.EndMarkerEnabled) {
							if (start0 < end1 && end1 < start1 ||
								start1 < end0 && end0 < start1) {
								start0 = startx;
								start1 = startx + start_marker_width;
								end0 = endx - end_marker_width;
								end1 = endx;
							}
						}

						if (vsq.config.StartMarkerEnabled) {
							if (start0 <= e.X && e.X <= start1) {
								mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.MARK_START;
							}
						}
						if (vsq.config.EndMarkerEnabled && mPositionIndicatorMouseDownMode != PositionIndicatorMouseDownMode.MARK_START) {
							if (end0 <= e.X && e.X <= end1) {
								mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.MARK_END;
							}
						}
						#endregion
					} else if (18 < e.Y && e.Y <= 32) {
						#region テンポ
						int index = -1;
						int count = MusicManager.getVsqFile().TempoTable.Count;
						for (int i = 0; i < count; i++) {
							int clock = MusicManager.getVsqFile().TempoTable[i].Clock;
							int x = EditorManager.xCoordFromClocks(clock);
							if (x < 0) {
								continue;
							} else if (parent.form.Width < x) {
								break;
							}
							string s = PortUtil.formatDecimal("#.00", 60e6 / (float)MusicManager.getVsqFile().TempoTable[i].Tempo);
							Size size = Utility.measureString(s, new Font(EditorManager.editorConfig.ScreenFontName, Cadencii.Gui.Font.PLAIN, EditorConfig.FONT_SIZE8));
							if (Utility.isInRect(new Point(e.X, e.Y), new Rectangle(x, 14, (int)size.Width, 14))) {
								index = i;
								break;
							}
						}

						if (index >= 0) {
							int clock = MusicManager.getVsqFile().TempoTable[index].Clock;
							if (EditorManager.SelectedTool != EditTool.ERASER) {
								int mouse_clock = EditorManager.clockFromXCoord(e.X);
								mTempoDraggingDeltaClock = mouse_clock - clock;
								mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.TEMPO;
							}
							if ((modifiers & Keys.Shift) == Keys.Shift) {
								if (EditorManager.itemSelection.getTempoCount() > 0) {
									int last_clock = EditorManager.itemSelection.getLastTempoClock();
									int start = Math.Min(last_clock, clock);
									int end = Math.Max(last_clock, clock);
									for (int i = 0; i < MusicManager.getVsqFile().TempoTable.Count; i++) {
										int tclock = MusicManager.getVsqFile().TempoTable[i].Clock;
										if (tclock < start) {
											continue;
										} else if (end < tclock) {
											break;
										}
										if (start <= tclock && tclock <= end) {
											EditorManager.itemSelection.addTempo(tclock);
										}
									}
								} else {
									EditorManager.itemSelection.addTempo(clock);
								}
							} else if ((modifiers & parent.form.s_modifier_key) == parent.form.s_modifier_key) {
								if (EditorManager.itemSelection.isTempoContains(clock)) {
									EditorManager.itemSelection.removeTempo(clock);
								} else {
									EditorManager.itemSelection.addTempo(clock);
								}
							} else {
								if (!EditorManager.itemSelection.isTempoContains(clock)) {
									EditorManager.itemSelection.clearTempo();
								}
								EditorManager.itemSelection.addTempo(clock);
							}
						} else {
							EditorManager.itemSelection.clearEvent();
							EditorManager.itemSelection.clearTempo();
							EditorManager.itemSelection.clearTimesig();
						}
						#endregion
					} else if (32 < e.Y && e.Y <= parent.form.picturePositionIndicator.Height - 1) {
						#region 拍子
						// クリック位置に拍子が表示されているかどうか検査
						int index = -1;
						for (int i = 0; i < MusicManager.getVsqFile().TimesigTable.Count; i++) {
							string s = MusicManager.getVsqFile().TimesigTable[i].Numerator + "/" + MusicManager.getVsqFile().TimesigTable[i].Denominator;
							int x = EditorManager.xCoordFromClocks(MusicManager.getVsqFile().TimesigTable[i].Clock);
							Size size = Utility.measureString(s, new Font(EditorManager.editorConfig.ScreenFontName, Cadencii.Gui.Font.PLAIN, EditorConfig.FONT_SIZE8));
							if (Utility.isInRect(new Point(e.X, e.Y), new Rectangle(x, 28, (int)size.Width, 14))) {
								index = i;
								break;
							}
						}

						if (index >= 0) {
							int barcount = MusicManager.getVsqFile().TimesigTable[index].BarCount;
							if (EditorManager.SelectedTool != EditTool.ERASER) {
								int barcount_clock = MusicManager.getVsqFile().getClockFromBarCount(barcount);
								int mouse_clock = EditorManager.clockFromXCoord(e.X);
								mTimesigDraggingDeltaClock = mouse_clock - barcount_clock;
								mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.TIMESIG;
							}
							if ((modifiers & Keys.Shift) == Keys.Shift) {
								if (EditorManager.itemSelection.getTimesigCount() > 0) {
									int last_barcount = EditorManager.itemSelection.getLastTimesigBarcount();
									int start = Math.Min(last_barcount, barcount);
									int end = Math.Max(last_barcount, barcount);
									for (int i = 0; i < MusicManager.getVsqFile().TimesigTable.Count; i++) {
										int tbarcount = MusicManager.getVsqFile().TimesigTable[i].BarCount;
										if (tbarcount < start) {
											continue;
										} else if (end < tbarcount) {
											break;
										}
										if (start <= tbarcount && tbarcount <= end) {
											EditorManager.itemSelection.addTimesig(MusicManager.getVsqFile().TimesigTable[i].BarCount);
										}
									}
								} else {
									EditorManager.itemSelection.addTimesig(barcount);
								}
							} else if ((modifiers & parent.form.s_modifier_key) == parent.form.s_modifier_key) {
								if (EditorManager.itemSelection.isTimesigContains(barcount)) {
									EditorManager.itemSelection.removeTimesig(barcount);
								} else {
									EditorManager.itemSelection.addTimesig(barcount);
								}
							} else {
								if (!EditorManager.itemSelection.isTimesigContains(barcount)) {
									EditorManager.itemSelection.clearTimesig();
								}
								EditorManager.itemSelection.addTimesig(barcount);
							}
						} else {
							EditorManager.itemSelection.clearEvent();
							EditorManager.itemSelection.clearTempo();
							EditorManager.itemSelection.clearTimesig();
						}
						#endregion
					}
				}
				parent.form.refreshScreen();
			}

			public void RunMouseUp(MouseEventArgs e)
			{
				Keys modifiers = GuiHost.ModifierKeys;
				#if DEBUG
				CDebug.WriteLine("picturePositionIndicator_MouseClick");
				#endif
				if (e.Button == MouseButtons.Left) {
					VsqFileEx vsq = MusicManager.getVsqFile();
					if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.NONE) {
						if (4 <= e.Y && e.Y <= 18) {
							#region マーカー位置の変更
							int clock = EditorManager.clockFromXCoord(e.X);
							if (EditorManager.editorConfig.getPositionQuantize() != QuantizeMode.off) {
								int unit = EditorManager.getPositionQuantizeClock();
								clock = FormMainModel.Quantize(clock, unit);
							}
							if (EditorManager.isPlaying()) {
								EditorManager.setPlaying(false, parent.form);
								EditorManager.setCurrentClock(clock);
								EditorManager.setPlaying(true, parent.form);
							} else {
								EditorManager.setCurrentClock(clock);
							}
							parent.form.refreshScreen();
							#endregion
						} else if (18 < e.Y && e.Y <= 32) {
							#region テンポの変更
							#if DEBUG
							CDebug.WriteLine("TempoChange");
							#endif
							EditorManager.itemSelection.clearEvent();
							EditorManager.itemSelection.clearTimesig();
							if (EditorManager.itemSelection.getTempoCount() > 0) {
								#region テンポ変更があった場合
								int index = -1;
								int clock = EditorManager.itemSelection.getLastTempoClock();
								for (int i = 0; i < vsq.TempoTable.Count; i++) {
									if (clock == vsq.TempoTable[i].Clock) {
										index = i;
										break;
									}
								}
								if (index >= 0 && EditorManager.SelectedTool == EditTool.ERASER) {
									#region ツールがEraser
									if (vsq.TempoTable[index].Clock == 0) {
										string msg = _("Cannot remove first symbol of track!");
										parent.form.statusLabel.Text = msg;
										SystemSounds.Asterisk.Play();
										return;
									}
									CadenciiCommand run = new CadenciiCommand(
										VsqCommand.generateCommandUpdateTempo(vsq.TempoTable[index].Clock,
											vsq.TempoTable[index].Clock,
											-1));
									EditorManager.editHistory.register(vsq.executeCommand(run));
									parent.form.setEdited(true);
									#endregion
								}
								#endregion
							}
							mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.NONE;
							#endregion
						} else if (32 < e.Y && e.Y <= parent.form.picturePositionIndicator.Height - 1) {
							#region 拍子の変更
							EditorManager.itemSelection.clearEvent();
							EditorManager.itemSelection.clearTempo();
							if (EditorManager.itemSelection.getTimesigCount() > 0) {
								#region 拍子変更があった場合
								int index = 0;
								int last_barcount = EditorManager.itemSelection.getLastTimesigBarcount();
								for (int i = 0; i < vsq.TimesigTable.Count; i++) {
									if (vsq.TimesigTable[i].BarCount == last_barcount) {
										index = i;
										break;
									}
								}
								if (EditorManager.SelectedTool == EditTool.ERASER) {
									#region ツールがEraser
									if (vsq.TimesigTable[index].Clock == 0) {
										string msg = _("Cannot remove first symbol of track!");
										parent.form.statusLabel.Text = msg;
										SystemSounds.Asterisk.Play();
										return;
									}
									int barcount = vsq.TimesigTable[index].BarCount;
									CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandUpdateTimesig(barcount, barcount, -1, -1));
									EditorManager.editHistory.register(vsq.executeCommand(run));
									parent.form.setEdited(true);
									#endregion
								}
								#endregion
							}
							#endregion
						}
					} else if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.TEMPO) {
						int count = EditorManager.itemSelection.getTempoCount();
						int[] clocks = new int[count];
						int[] new_clocks = new int[count];
						int[] tempos = new int[count];
						int i = -1;
						bool contains_first_tempo = false;
						foreach (var item in EditorManager.itemSelection.getTempoIterator()) {
							int clock = item.getKey();
							i++;
							clocks[i] = clock;
							if (clock == 0) {
								contains_first_tempo = true;
								break;
							}
							TempoTableEntry editing = EditorManager.itemSelection.getTempo(clock).editing;
							new_clocks[i] = editing.Clock;
							tempos[i] = editing.Tempo;
						}
						if (contains_first_tempo) {
							SystemSounds.Asterisk.Play();
						} else {
							CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandUpdateTempoRange(clocks, new_clocks, tempos));
							EditorManager.editHistory.register(vsq.executeCommand(run));
							parent.form.setEdited(true);
						}
					} else if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.TIMESIG) {
						int count = EditorManager.itemSelection.getTimesigCount();
						int[] barcounts = new int[count];
						int[] new_barcounts = new int[count];
						int[] numerators = new int[count];
						int[] denominators = new int[count];
						int i = -1;
						bool contains_first_bar = false;
						foreach (var item in EditorManager.itemSelection.getTimesigIterator()) {
							int bar = item.getKey();
							i++;
							barcounts[i] = bar;
							if (bar == 0) {
								contains_first_bar = true;
								break;
							}
							TimeSigTableEntry editing = EditorManager.itemSelection.getTimesig(bar).editing;
							new_barcounts[i] = editing.BarCount;
							numerators[i] = editing.Numerator;
							denominators[i] = editing.Denominator;
						}
						if (contains_first_bar) {
							SystemSounds.Asterisk.Play();
						} else {
							CadenciiCommand run = new CadenciiCommand(
								VsqCommand.generateCommandUpdateTimesigRange(barcounts, new_barcounts, numerators, denominators));
							EditorManager.editHistory.register(vsq.executeCommand(run));
							parent.form.setEdited(true);
						}
					}
				}
				mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.NONE;
				parent.form.pictPianoRoll.Refresh();
				parent.form.picturePositionIndicator.Refresh();
			}

			public void RunMouseMove(MouseEventArgs e)
			{
				VsqFileEx vsq = MusicManager.getVsqFile();
				if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.TEMPO) {
					int clock = EditorManager.clockFromXCoord(e.X) - mTempoDraggingDeltaClock;
					int step = EditorManager.getPositionQuantizeClock();
					clock = FormMainModel.Quantize(clock, step);
					int last_clock = EditorManager.itemSelection.getLastTempoClock();
					int dclock = clock - last_clock;
					foreach (var item in EditorManager.itemSelection.getTempoIterator()) {
						int key = item.getKey();
						EditorManager.itemSelection.getTempo(key).editing.Clock = EditorManager.itemSelection.getTempo(key).original.Clock + dclock;
					}
					parent.form.picturePositionIndicator.Refresh();
				} else if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.TIMESIG) {
					int clock = EditorManager.clockFromXCoord(e.X) - mTimesigDraggingDeltaClock;
					int barcount = vsq.getBarCountFromClock(clock);
					int last_barcount = EditorManager.itemSelection.getLastTimesigBarcount();
					int dbarcount = barcount - last_barcount;
					foreach (var item in EditorManager.itemSelection.getTimesigIterator()) {
						int bar = item.getKey();
						EditorManager.itemSelection.getTimesig(bar).editing.BarCount = EditorManager.itemSelection.getTimesig(bar).original.BarCount + dbarcount;
					}
					parent.form.picturePositionIndicator.Refresh();
				} else if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.MARK_START) {
					int clock = EditorManager.clockFromXCoord(e.X);
					int unit = EditorManager.getPositionQuantizeClock();
					clock = FormMainModel.Quantize(clock, unit);
					if (clock < 0) {
						clock = 0;
					}
					int draft_start = Math.Min(clock, vsq.config.EndMarker);
					int draft_end = Math.Max(clock, vsq.config.EndMarker);
					if (draft_start != vsq.config.StartMarker) {
						vsq.config.StartMarker = draft_start;
						parent.form.setEdited(true);
					}
					if (draft_end != vsq.config.EndMarker) {
						vsq.config.EndMarker = draft_end;
						parent.form.setEdited(true);
					}
					parent.form.refreshScreen();
				} else if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.MARK_END) {
					int clock = EditorManager.clockFromXCoord(e.X);
					int unit = EditorManager.getPositionQuantizeClock();
					clock = FormMainModel.Quantize(clock, unit);
					if (clock < 0) {
						clock = 0;
					}
					int draft_start = Math.Min(clock, vsq.config.StartMarker);
					int draft_end = Math.Max(clock, vsq.config.StartMarker);
					if (draft_start != vsq.config.StartMarker) {
						vsq.config.StartMarker = draft_start;
						parent.form.setEdited(true);
					}
					if (draft_end != vsq.config.EndMarker) {
						vsq.config.EndMarker = draft_end;
						parent.form.setEdited(true);
					}
					parent.form.refreshScreen();
				}
			}

			public void RunPaint(PaintEventArgs e)
			{
				Graphics g = e.Graphics;
				DrawTo(g);
				#if MONITOR_FPS
				g.setColor(Cadencii.Gui.Colors.Red);
				g.setFont(EditorConfig.baseFont10Bold);
				g.drawString(PortUtil.formatDecimal("#.00", parent.form.mFps) + " / " + PortUtil.formatDecimal("#.00", parent.form.mFps2), 5, 5);
				#endif
			}

			public void RunPreviewKeyDown(KeyEventArgs e)
			{
				parent.form.processSpecialShortcutKey(e, true);
			}
			#endregion

			public void DrawTo(Cadencii.Gui.Graphics g1)
			{
				Graphics g = (Graphics)g1;
				Font SMALL_FONT = EditorConfig.baseFont8;
				int small_font_offset = AppConfig.baseFont8OffsetHeight;
				try {
					int key_width = EditorManager.keyWidth;
					int width = parent.form.picturePositionIndicator.Width;
					int height = parent.form.picturePositionIndicator.Height;
					VsqFileEx vsq = MusicManager.getVsqFile();

					#region 小節ごとの線
					int dashed_line_step = EditorManager.getPositionQuantizeClock();
					for (Iterator<VsqBarLineType> itr = vsq.getBarLineIterator(EditorManager.clockFromXCoord(width)); itr.hasNext(); ) {
						VsqBarLineType blt = itr.next();
						int local_clock_step = 480 * 4 / blt.getLocalDenominator();
						int x = EditorManager.xCoordFromClocks(blt.clock());
						if (blt.isSeparator()) {
							int current = blt.getBarCount() - vsq.getPreMeasure() + 1;
							g.setColor(FormMainModel.ColorR105G105B105);
							g.drawLine(x, 0, x, 49);
							// 小節の数字
							//g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
							g.setColor(Cadencii.Gui.Colors.Black);
							g.setFont(SMALL_FONT);
							g.drawString(current + "", x + 4, 8 - small_font_offset + 1);
							//g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
						} else {
							g.setColor(FormMainModel.ColorR105G105B105);
							g.drawLine(x, 11, x, 16);
							g.drawLine(x, 26, x, 31);
							g.drawLine(x, 41, x, 46);
						}
						if (dashed_line_step > 1 && EditorManager.isGridVisible()) {
							int numDashedLine = local_clock_step / dashed_line_step;
							for (int i = 1; i < numDashedLine; i++) {
								int x2 = EditorManager.xCoordFromClocks(blt.clock() + i * dashed_line_step);
								g.setColor(FormMainModel.ColorR065G065B065);
								g.drawLine(x2, 9 + 5, x2, 14 + 3);
								g.drawLine(x2, 24 + 5, x2, 29 + 3);
								g.drawLine(x2, 39 + 5, x2, 44 + 3);
							}
						}
					}
					#endregion

					if (vsq != null) {
						#region 拍子の変更
						int c = vsq.TimesigTable.Count;
						for (int i = 0; i < c; i++) {
							TimeSigTableEntry itemi = vsq.TimesigTable[i];
							int clock = itemi.Clock;
							int barcount = itemi.BarCount;
							int x = EditorManager.xCoordFromClocks(clock);
							if (width < x) {
								break;
							}
							string s = itemi.Numerator + "/" + itemi.Denominator;
							g.setFont(SMALL_FONT);
							if (EditorManager.itemSelection.isTimesigContains(barcount)) {
								g.setColor(EditorManager.getHilightColor());
								g.drawString(s, x + 4, 40 - small_font_offset + 1);
							} else {
								g.setColor(Cadencii.Gui.Colors.Black);
								g.drawString(s, x + 4, 40 - small_font_offset + 1);
							}

							if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.TIMESIG) {
								if (EditorManager.itemSelection.isTimesigContains(barcount)) {
									int edit_clock_x = EditorManager.xCoordFromClocks(vsq.getClockFromBarCount(EditorManager.itemSelection.getTimesig(barcount).editing.BarCount));
									g.setColor(FormMainModel.ColorR187G187B255);
									g.drawLine(edit_clock_x - 1, 32,
										edit_clock_x - 1, parent.form.picturePositionIndicator.Height - 1);
									g.setColor(FormMainModel.ColorR007G007B151);
									g.drawLine(edit_clock_x, 32,
										edit_clock_x, parent.form.picturePositionIndicator.Height - 1);
								}
							}
						}
						#endregion

						#region テンポの変更
						g.setFont(SMALL_FONT);
						c = vsq.TempoTable.Count;
						for (int i = 0; i < c; i++) {
							TempoTableEntry itemi = vsq.TempoTable[i];
							int clock = itemi.Clock;
							int x = EditorManager.xCoordFromClocks(clock);
							if (width < x) {
								break;
							}
							string s = PortUtil.formatDecimal("#.00", 60e6 / (float)itemi.Tempo);
							if (EditorManager.itemSelection.isTempoContains(clock)) {
								g.setColor(EditorManager.getHilightColor());
								g.drawString(s, x + 4, 24 - small_font_offset + 1);
							} else {
								g.setColor(Cadencii.Gui.Colors.Black);
								g.drawString(s, x + 4, 24 - small_font_offset + 1);
							}

							if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.TEMPO) {
								if (EditorManager.itemSelection.isTempoContains(clock)) {
									int edit_clock_x = EditorManager.xCoordFromClocks(EditorManager.itemSelection.getTempo(clock).editing.Clock);
									g.setColor(FormMainModel.ColorR187G187B255);
									g.drawLine(edit_clock_x - 1, 18,
										edit_clock_x - 1, 32);
									g.setColor(FormMainModel.ColorR007G007B151);
									g.drawLine(edit_clock_x, 18,
										edit_clock_x, 32);
								}
							}
						}
						#endregion
					}

					#region 現在のマーカー
					// ソングポジション
					float xoffset = key_width + EditorManager.keyOffset - parent.form.Model.StartToDrawX;
					int marker_x = (int)(EditorManager.getCurrentClock() * parent.form.Model.ScaleX + xoffset);
					if (key_width <= marker_x && marker_x <= width) {
						g.setStroke(new Stroke(2.0f));
						g.setColor(Cadencii.Gui.Colors.White);
						g.drawLine(marker_x, 0, marker_x, height);
						g.setStroke(new Stroke());
					}

					// スタートマーカーとエンドマーカー
					bool right = false;
					bool left = false;
					if (vsq.config.StartMarkerEnabled) {
						int x = EditorManager.xCoordFromClocks(vsq.config.StartMarker);
						if (x < key_width) {
							left = true;
						} else if (width < x) {
							right = true;
						} else {
							g.drawImage(
								new Cadencii.Gui.Image () { NativeImage = Resources.start_marker }, x, 3, this);
						}
					}
					if (vsq.config.EndMarkerEnabled) {
						int x = EditorManager.xCoordFromClocks(vsq.config.EndMarker) - 6;
						if (x < key_width) {
							left = true;
						} else if (width < x) {
							right = true;
						} else {
							g.drawImage(
								new Cadencii.Gui.Image () { NativeImage = Resources.end_marker }, x, 3, this);
						}
					}

					// 範囲外にスタートマーカーとエンドマーカーがある場合のマーク
					if (right) {
						g.setColor(Cadencii.Gui.Colors.White);
						g.fillPolygon(
							new int[] { width - 6, width, width - 6 },
							new int[] { 3, 10, 16 },
							3);
					}
					if (left) {
						g.setColor(Cadencii.Gui.Colors.White);
						g.fillPolygon(
							new int[] { key_width + 7, key_width + 1, key_width + 7 },
							new int[] { 3, 10, 16 },
							3);
					}
					#endregion

					#region TEMPO & BEAT
					// TEMPO BEATの文字の部分。小節数が被っている可能性があるので、塗り潰す
					var col = parent.form.picturePositionIndicator.BackColor;
					g.setColor(new Color(col.R, col.G, col.B, col.A));
					g.fillRect(0, 0, EditorManager.keyWidth, 48);
					// 横ライン上
					g.setColor(new Color(104, 104, 104));
					g.drawLine(0, 17, width, 17);
					// 横ライン中央
					g.drawLine(0, 32, width, 32);
					// 横ライン下
					g.drawLine(0, 47, width, 47);
					// 縦ライン
					g.drawLine(EditorManager.keyWidth, 0, EditorManager.keyWidth, 48);
					/* TEMPO&BEATとピアノロールの境界 */
					g.drawLine(EditorManager.keyWidth, 48, width - 18, 48);
					g.setFont(SMALL_FONT);
					g.setColor(Cadencii.Gui.Colors.Black);
					g.drawString("TEMPO", 11, 24 - small_font_offset + 1);
					g.drawString("BEAT", 11, 40 - small_font_offset + 1);
					#endregion
				} catch (Exception ex) {
					Logger.write(GetType () + ".picturePositionIndicatorDrawTo; ex=" + ex + "\n");
					Logger.StdErr("FormMain#picturePositionIndicatorDrawTo; ex=" + ex);
				}
			}

			#region cPotisionIndicator
			public void RunStartMarkerCommand()
			{
				int clock = mPositionIndicatorPopupShownClock;
				VsqFileEx vsq = MusicManager.getVsqFile();
				vsq.config.StartMarkerEnabled = true;
				vsq.config.StartMarker = clock;
				if (vsq.config.EndMarker < clock) {
					vsq.config.EndMarker = clock;
				}
				parent.form.menuVisualStartMarker.Checked = true;
				parent.form.setEdited(true);
				parent.form.picturePositionIndicator.Refresh();
			}

			public void RunEndMarkerCommand()
			{
				int clock = mPositionIndicatorPopupShownClock;
				VsqFileEx vsq = MusicManager.getVsqFile();
				vsq.config.EndMarkerEnabled = true;
				vsq.config.EndMarker = clock;
				if (vsq.config.StartMarker > clock) {
					vsq.config.StartMarker = clock;
				}
				parent.form.menuVisualEndMarker.Checked = true;
				parent.form.setEdited(true);
				parent.form.picturePositionIndicator.Refresh();
			}
			#endregion
		}
	}
}
