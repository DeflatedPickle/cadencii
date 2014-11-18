using System;

using Keys = Cadencii.Gui.Toolkit.Keys;
using NMouseButtons = Cadencii.Gui.Toolkit.MouseButtons;
using NMouseEventArgs = Cadencii.Gui.Toolkit.MouseEventArgs;
using NMouseEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.MouseEventArgs>;
using NKeyEventArgs = Cadencii.Gui.Toolkit.KeyEventArgs;
using NKeyEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.KeyEventArgs>;
using PaintEventArgs = Cadencii.Gui.Toolkit.PaintEventArgs;
using System.Threading;
using Cadencii.Gui;
using Cadencii.Media.Vsq;
using System.Collections.Generic;
using cadencii.java.util;
using cadencii.core;
using System.Media;
using Cadencii.Utilities;
using Cadencii.Gui.Toolkit;
using cadencii;
using Cadencii.Application.Media;
using Cadencii.Application.Forms;
using Cadencii.Application.Drawing;

namespace Cadencii.Application.Models
{
	public partial class FormMainModel
	{
		public class PictPianoRollModel
		{
			readonly FormMainModel parent;

			public PictPianoRollModel (FormMainModel parent)
			{
				this.parent = parent;
			}

			/// <summary>
			/// ピアノロールの右クリックが表示される直前のマウスの位置
			/// </summary>
			Point mContextMenuOpenedPosition = new Point();
			bool mMouseMoved = false;
			/// <summary>
			/// 画面外への自動スクロールモード
			/// </summary>
			ExtDragXMode mExtDragX = ExtDragXMode.NONE;
			ExtDragYMode mExtDragY = ExtDragYMode.NONE;
			/// <summary>
			/// EditMode=MoveEntryで，移動を開始する直前のマウスの仮想スクリーン上の位置
			/// </summary>
			Point mMouseMoveInit = new Point();
			/// <summary>
			/// EditMode=MoveEntryで，移動を開始する直前のマウスの位置と，音符の先頭との距離(ピクセル)
			/// </summary>
			int mMouseMoveOffset;
			/// <summary>
			/// ビブラート範囲を編集中の音符のInternalID
			/// </summary>
			int mVibratoEditingId = -1;

			#if ENABLE_MOUSEHOVER
			/// <summary>
			/// マウスホバーを発生させるスレッド
			/// </summary>
			public Thread mMouseHoverThread = null;
			#endif

			//BOOKMARK: pictPianoRoll
			#region pictPianoRoll
			public void RunPianoRollKeyUp(KeyEventArgs e)
			{
				parent.form.processSpecialShortcutKey(e, false);
			}

			public void RunPianoRollMouseClick(MouseEventArgs e)
			{
				#if DEBUG
				CDebug.WriteLine("pictPianoRoll_MouseClick");
				#endif
				Keys modefiers = AwtHost.ModifierKeys;
				EditMode edit_mode = EditorManager.EditMode;

				bool is_button_left = e.Button == NMouseButtons.Left;
				int selected = EditorManager.Selected;

				if (e.Button == NMouseButtons.Left) {
					#if ENABLE_MOUSEHOVER
					if ( mMouseHoverThread != null ) {
						mMouseHoverThread.Abort();
					}
					#endif

					// クリック位置にIDが無いかどうかを検査
					ByRef<Rectangle> out_id_rect = new ByRef<Rectangle>(new Rectangle());
					VsqEvent item = parent.form.getItemAtClickedPosition(new Point(e.X, e.Y), out_id_rect);
					Rectangle id_rect = out_id_rect.value;
					#if DEBUG
					CDebug.WriteLine("    (item==null)=" + (item == null));
					#endif
					if (item != null &&
						edit_mode != EditMode.MOVE_ENTRY_WAIT_MOVE &&
						edit_mode != EditMode.MOVE_ENTRY &&
						edit_mode != EditMode.MOVE_ENTRY_WHOLE_WAIT_MOVE &&
						edit_mode != EditMode.MOVE_ENTRY_WHOLE &&
						edit_mode != EditMode.EDIT_LEFT_EDGE &&
						edit_mode != EditMode.EDIT_RIGHT_EDGE &&
						edit_mode != EditMode.MIDDLE_DRAG &&
						edit_mode != EditMode.CURVE_ON_PIANOROLL) {
						if ((modefiers & Keys.Shift) != Keys.Shift && (modefiers & AwtHost.ModifierKeys) != AwtHost.ModifierKeys) {
							EditorManager.itemSelection.clearEvent();
						}
						EditorManager.itemSelection.addEvent(item.InternalID);
						int internal_id = item.InternalID;
						parent.hideInputTextBox();
						if (EditorManager.SelectedTool == EditTool.ERASER) {
							CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventDelete(selected, internal_id));
							EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
							parent.form.setEdited(true);
							EditorManager.itemSelection.clearEvent();
							return;
							#if ENABLE_SCRIPT
						} else if (EditorManager.SelectedTool == EditTool.PALETTE_TOOL) {
							List<int> internal_ids = new List<int>();
							foreach (var see in EditorManager.itemSelection.getEventIterator()) {
								internal_ids.Add(see.original.InternalID);
							}
							var btn = e.Button;
							if (parent.IsMouseMiddleButtonDown(btn)) {
								btn = NMouseButtons.Middle;
							}
							bool result = PaletteToolServer.invokePaletteTool(EditorManager.mSelectedPaletteTool,
								selected,
								internal_ids.ToArray(),
								btn);
							if (result) {
								parent.form.setEdited(true);
								EditorManager.itemSelection.clearEvent();
								return;
							}
							#endif
						}
					} else {
						if (edit_mode != EditMode.MOVE_ENTRY_WAIT_MOVE &&
							edit_mode != EditMode.MOVE_ENTRY &&
							edit_mode != EditMode.MOVE_ENTRY_WHOLE_WAIT_MOVE &&
							edit_mode != EditMode.MOVE_ENTRY_WHOLE &&
							edit_mode != EditMode.EDIT_LEFT_EDGE &&
							edit_mode != EditMode.EDIT_RIGHT_EDGE &&
							edit_mode != EditMode.EDIT_VIBRATO_DELAY) {
							if (!EditorManager.mIsPointerDowned) {
								EditorManager.itemSelection.clearEvent();
							}
							parent.hideInputTextBox();
						}
						if (EditorManager.SelectedTool == EditTool.ERASER) {
							// マウス位置にビブラートの波波があったら削除する
							int stdx = parent.form.Model.StartToDrawX;
							int stdy = parent.form.Model.StartToDrawY;
							for (int i = 0; i < EditorManager.mDrawObjects[selected - 1].Count; i++) {
								DrawObject dobj = EditorManager.mDrawObjects[selected - 1][i];
								if (dobj.mRectangleInPixel.X + parent.form.Model.StartToDrawX + dobj.mRectangleInPixel.Width - stdx < 0) {
									continue;
								} else if (parent.form.pictPianoRoll.Width < dobj.mRectangleInPixel.X + EditorManager.keyWidth - stdx) {
									break;
								}
								Rectangle rc = new Rectangle(dobj.mRectangleInPixel.X + EditorManager.keyWidth + dobj.mVibratoDelayInPixel - stdx,
									dobj.mRectangleInPixel.Y + (int)(100 * parent.form.Model.ScaleY) - stdy,
									dobj.mRectangleInPixel.Width - dobj.mVibratoDelayInPixel,
									(int)(100 * parent.form.Model.ScaleY));
								if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
									//ビブラートの範囲なのでビブラートを消す
									VsqEvent item3 = null;
									VsqID item2 = null;
									int internal_id = -1;
									internal_id = dobj.mInternalID;
									foreach (var ve in MusicManager.getVsqFile().Track[selected].getNoteEventIterator()) {
										if (ve.InternalID == dobj.mInternalID) {
											item2 = (VsqID)ve.ID.clone();
											item3 = ve;
											break;
										}
									}
									if (item2 != null) {
										item2.VibratoHandle = null;
										CadenciiCommand run = new CadenciiCommand(
											VsqCommand.generateCommandEventChangeIDContaints(selected,
												internal_id,
												item2));
										EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
										parent.form.setEdited(true);
									}
									break;
								}
							}
						}
					}
				} else if (e.Button == NMouseButtons.Right) {
					bool show_context_menu = (e.X > EditorManager.keyWidth);
					#if ENABLE_MOUSEHOVER
					if ( mMouseHoverThread != null ) {
						if ( !mMouseHoverThread.IsAlive && EditorManager.editorConfig.PlayPreviewWhenRightClick ) {
							show_context_menu = false;
						}
					} else {
						if ( EditorManager.editorConfig.PlayPreviewWhenRightClick ) {
							show_context_menu = false;
						}
					}
					#endif
					show_context_menu = EditorManager.showContextMenuWhenRightClickedOnPianoroll ? (show_context_menu && !mMouseMoved) : false;
					if (show_context_menu) {
						#if ENABLE_MOUSEHOVER
						if ( mMouseHoverThread != null ) {
							mMouseHoverThread.Abort();
						}
						#endif

						parent.form.updateContextMenuPiano (new Point(e.X, e.Y));

						mContextMenuOpenedPosition = new Point(e.X, e.Y);
						parent.form.cMenuPiano.Show(parent.form.pictPianoRoll, e.X, e.Y);
					} else {
						ByRef<Rectangle> out_id_rect = new ByRef<Rectangle>();
						VsqEvent item = parent.form.getItemAtClickedPosition(parent.form.mButtonInitial, out_id_rect);
						Rectangle id_rect = out_id_rect.value;
						#if DEBUG
						CDebug.WriteLine("pitcPianoRoll_MouseClick; button is right; (item==null)=" + (item == null));
						#endif
						if (item != null) {
							int itemx = EditorManager.xCoordFromClocks(item.Clock);
							int itemy = EditorManager.yCoordFromNote(item.ID.Note);
						}
					}
				} else if (e.Button == NMouseButtons.Middle) {
					#if ENABLE_SCRIPT
					if (EditorManager.SelectedTool == EditTool.PALETTE_TOOL) {
						ByRef<Rectangle> out_id_rect = new ByRef<Rectangle>();
						VsqEvent item = parent.form.getItemAtClickedPosition(new Point(e.X, e.Y), out_id_rect);
						Rectangle id_rect = out_id_rect.value;
						if (item != null) {
							EditorManager.itemSelection.clearEvent();
							EditorManager.itemSelection.addEvent(item.InternalID);
							List<int> internal_ids = new List<int>();
							foreach (var see in EditorManager.itemSelection.getEventIterator()) {
								internal_ids.Add(see.original.InternalID);
							}
							bool result = PaletteToolServer.invokePaletteTool(EditorManager.mSelectedPaletteTool,
								EditorManager.Selected,
								internal_ids.ToArray(),
								e.Button);
							if (result) {
								parent.form.setEdited(true);
								EditorManager.itemSelection.clearEvent();
								return;
							}
						}
					}
					#endif
				}
			}

			public void RunPianoRollMouseDoubleClick(MouseEventArgs e)
			{
				#if DEBUG
				CDebug.WriteLine("FormMain#pictPianoRoll_MouseDoubleClick");
				#endif
				ByRef<Rectangle> out_rect = new ByRef<Rectangle>();
				VsqEvent item = parent.form.getItemAtClickedPosition(new Point(e.X, e.Y), out_rect);
				Rectangle rect = out_rect.value;
				int selected = EditorManager.Selected;
				VsqFileEx vsq = MusicManager.getVsqFile();
				if (item != null && item.ID.type == VsqIDType.Anote) {
					#if ENABLE_SCRIPT
					if (EditorManager.SelectedTool != EditTool.PALETTE_TOOL)
					#endif

					{
						EditorManager.itemSelection.clearEvent();
						EditorManager.itemSelection.addEvent(item.InternalID);
						#if ENABLE_MOUSEHOVER
						mMouseHoverThread.Abort();
						#endif
						if (!EditorManager.editorConfig.KeepLyricInputMode) {
							parent.mLastSymbolEditMode = false;
						}
						parent.showInputTextBox(
							item.ID.LyricHandle.L0.Phrase,
							item.ID.LyricHandle.L0.getPhoneticSymbol(),
							new Point(rect.X, rect.Y),
							parent.mLastSymbolEditMode);
						parent.form.refreshScreen();
						return;
					}
				} else {
					EditorManager.itemSelection.clearEvent();
					parent.hideInputTextBox();
					if (EditorManager.editorConfig.ShowExpLine && EditorManager.keyWidth <= e.X) {
						int stdx = parent.form.Model.StartToDrawX;
						int stdy = parent.form.Model.StartToDrawY;
						foreach (var dobj in EditorManager.mDrawObjects[selected - 1]) {
							// 表情コントロールプロパティを表示するかどうかを決める
							rect = new Rectangle(
								dobj.mRectangleInPixel.X + EditorManager.keyWidth - stdx,
								dobj.mRectangleInPixel.Y - stdy + (int)(100 * parent.form.Model.ScaleY),
								21,
								(int)(100 * parent.form.Model.ScaleY));
							if (Utility.isInRect(new Point(e.X, e.Y), rect)) {
								VsqEvent selectedEvent = null;
								for (Iterator<VsqEvent> itr2 = vsq.Track[selected].getEventIterator(); itr2.hasNext(); ) {
									VsqEvent ev = itr2.next();
									if (ev.InternalID == dobj.mInternalID) {
										selectedEvent = ev;
										break;
									}
								}
								if (selectedEvent != null) {
									#if ENABLE_MOUSEHOVER
									if ( mMouseHoverThread != null ) {
										mMouseHoverThread.Abort();
									}
									#endif
									SynthesizerType type = SynthesizerType.VOCALOID2;
									RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
									if (kind == RendererKind.VOCALOID1) {
										type = SynthesizerType.VOCALOID1;
									}
									FormNoteExpressionConfig dlg = null;
									try {
										dlg = ApplicationUIHost.Create<FormNoteExpressionConfig>(type, selectedEvent.ID.NoteHeadHandle);
										dlg.PMBendDepth = (selectedEvent.ID.PMBendDepth);
										dlg.PMBendLength = (selectedEvent.ID.PMBendLength);
										dlg.PMbPortamentoUse = (selectedEvent.ID.PMbPortamentoUse);
										dlg.DEMdecGainRate = (selectedEvent.ID.DEMdecGainRate);
										dlg.DEMaccent = (selectedEvent.ID.DEMaccent);
										dlg.Location = parent.GetFormPreferedLocation(dlg);
										var dr = DialogManager.ShowModalDialog(dlg, parent.form);
										if (dr == Cadencii.Gui.DialogResult.OK) {
											VsqID id = (VsqID)selectedEvent.ID.clone();
											id.PMBendDepth = dlg.PMBendDepth;
											id.PMBendLength = dlg.PMBendLength;
											id.PMbPortamentoUse = dlg.PMbPortamentoUse;
											id.DEMdecGainRate = dlg.DEMdecGainRate;
											id.DEMaccent = dlg.DEMaccent;
											id.NoteHeadHandle = dlg.EditedNoteHeadHandle;
											CadenciiCommand run = new CadenciiCommand(
												VsqCommand.generateCommandEventChangeIDContaints(selected, selectedEvent.InternalID, id));
											EditorManager.editHistory.register(vsq.executeCommand(run));
											parent.form.setEdited(true);
											parent.form.refreshScreen();
										}
									} catch (Exception ex) {
										Logger.write(GetType () + ".pictPianoRoll_MouseDoubleClick; ex=" + ex + "\n");
										Logger.StdErr(GetType () + ".pictPianoRoll_MouseDoubleClick" + ex);
									} finally {
										if (dlg != null) {
											try {
												dlg.Close();
											} catch (Exception ex2) {
												Logger.write(GetType () + ".pictPianoRoll_MouseDoubleClick; ex=" + ex2 + "\n");
												Logger.StdErr(GetType () + ".pictPianoRoll_MouseDoubleClick");
											}
										}
									}
									return;
								}
								break;
							}

							// ビブラートプロパティダイアログを表示するかどうかを決める
							rect = new Rectangle(
								dobj.mRectangleInPixel.X + EditorManager.keyWidth - stdx + 21,
								dobj.mRectangleInPixel.Y - stdy + (int)(100 * parent.form.Model.ScaleY),
								dobj.mRectangleInPixel.Width - 21,
								(int)(100 * parent.form.Model.ScaleY));
							if (Utility.isInRect(new Point(e.X, e.Y), rect)) {
								VsqEvent selectedEvent = null;
								for (Iterator<VsqEvent> itr2 = vsq.Track[selected].getEventIterator(); itr2.hasNext(); ) {
									VsqEvent ev = itr2.next();
									if (ev.InternalID == dobj.mInternalID) {
										selectedEvent = ev;
										break;
									}
								}
								if (selectedEvent != null) {
									#if ENABLE_MOUSEHOVER
									if ( mMouseHoverThread != null ) {
										mMouseHoverThread.Abort();
									}
									#endif
									SynthesizerType type = SynthesizerType.VOCALOID2;
									RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
									#if DEBUG
									Logger.StdOut("FormMain#pictPianoRoll_MouseDoubleClick; kind=" + kind);
									#endif
									if (kind == RendererKind.VOCALOID1) {
										type = SynthesizerType.VOCALOID1;
									}
									FormVibratoConfig dlg = null;
									try {
										dlg = ApplicationUIHost.Create<FormVibratoConfig>(
											selectedEvent.ID.VibratoHandle,
											selectedEvent.ID.getLength(),
											ApplicationGlobal.appConfig.DefaultVibratoLength,
											type,
											ApplicationGlobal.appConfig.UseUserDefinedAutoVibratoType);
										dlg.Location = parent.GetFormPreferedLocation(dlg);
										var dr = DialogManager.ShowModalDialog(dlg, parent.form);
										if (dr == Cadencii.Gui.DialogResult.OK) {
											VsqID t = (VsqID)selectedEvent.ID.clone();
											VibratoHandle handle = dlg.getVibratoHandle();
											#if DEBUG
											Logger.StdOut("FormMain#pictPianoRoll_MouseDoubleClick; (handle==null)=" + (handle == null));
											#endif
											if (handle != null) {
												string iconid = handle.IconID;
												int vibrato_length = handle.getLength();
												int note_length = selectedEvent.ID.getLength();
												t.VibratoDelay = note_length - vibrato_length;
												t.VibratoHandle = handle;
											} else {
												t.VibratoHandle = null;
											}
											CadenciiCommand run = new CadenciiCommand(
												VsqCommand.generateCommandEventChangeIDContaints(
													selected,
													selectedEvent.InternalID,
													t));
											EditorManager.editHistory.register(vsq.executeCommand(run));
											parent.form.setEdited(true);
											parent.form.refreshScreen();
										}
									} catch (Exception ex) {
										Logger.write(GetType () + ".pictPianoRoll_MouseDoubleClick; ex=" + ex + "\n");
									} finally {
										if (dlg != null) {
											try {
												dlg.Close();
											} catch (Exception ex2) {
												Logger.write(GetType () + ".pictPianoRoll_MouseDoubleClick; ex=" + ex2 + "\n");
											}
										}
									}
									return;
								}
								break;
							}

						}
					}
				}

				if (e.Button == NMouseButtons.Left) {
					// 必要な操作が何も無ければ，クリック位置にソングポジションを移動
					if (EditorManager.keyWidth < e.X) {
						int clock = FormMainModel.Quantize(EditorManager.clockFromXCoord(e.X), EditorManager.getPositionQuantizeClock());
						EditorManager.setCurrentClock(clock);
					}
				} else if (e.Button == NMouseButtons.Middle) {
					// ツールをポインター <--> 鉛筆に切り替える
					if (EditorManager.keyWidth < e.X) {
						if (EditorManager.SelectedTool == EditTool.ARROW) {
							EditorManager.SelectedTool = EditTool.PENCIL;
						} else {
							EditorManager.SelectedTool = EditTool.ARROW;
						}
					}
				}
			}

			public void RunPianoRollMouseDown(MouseEventArgs e0)
			{
				#if DEBUG
				CDebug.WriteLine("pictPianoRoll_MouseDown");
				#endif
				var btn0 = e0.Button;
				if (parent.IsMouseMiddleButtonDown(btn0)) {
					btn0 = NMouseButtons.Middle;
				}
				var e = new NMouseEventArgs(btn0, e0.Clicks, e0.X, e0.Y, e0.Delta);

				mMouseMoved = false;
				if (!EditorManager.isPlaying() && 0 <= e.X && e.X <= EditorManager.keyWidth) {
					int note = EditorManager.noteFromYCoord(e.Y);
					if (0 <= note && note <= 126) {
						if (e.Button == NMouseButtons.Left) {
							KeySoundPlayer.play(note);
						}
						return;
					}
				}

				EditorManager.itemSelection.clearTempo();
				EditorManager.itemSelection.clearTimesig();
				EditorManager.itemSelection.clearPoint();
				/*if ( e.Button == BMouseButtons.Left ) {
                EditorManager.selectedRegionEnabled = false;
            }*/

				parent.form.mMouseDowned = true;
				parent.form.mButtonInitial = new Point(e.X, e.Y);
				Keys modefier = AwtHost.ModifierKeys;

				EditTool selected_tool = EditorManager.SelectedTool;
				#if ENABLE_SCRIPT
				if (selected_tool != EditTool.PALETTE_TOOL && e.Button == NMouseButtons.Middle) {
				#else
				if ( e.Button == BMouseButtons.Middle ) {
				#endif
					EditorManager.EditMode =EditMode.MIDDLE_DRAG;
					parent.form.mMiddleButtonVScroll = parent.form.vScroll.Value;
					parent.form.mMiddleButtonHScroll = parent.form.hScroll.Value;
					return;
				}

				int stdx = parent.form.Model.StartToDrawX;
				int stdy = parent.form.Model.StartToDrawY;
				if (e.Button == NMouseButtons.Left && EditorManager.mCurveOnPianoroll && (selected_tool == EditTool.PENCIL || selected_tool == EditTool.LINE)) {
					parent.form.pictPianoRoll.mMouseTracer.clear();
					parent.form.pictPianoRoll.mMouseTracer.appendFirst(e.X + stdx, e.Y + stdy);
					parent.form.Cursor = Cursors.Default;
					EditorManager.EditMode =EditMode.CURVE_ON_PIANOROLL;
					return;
				}

				ByRef<Rectangle> out_rect = new ByRef<Rectangle>();
				VsqEvent item = parent.form.getItemAtClickedPosition(new Point(e.X, e.Y), out_rect);
				Rectangle rect = out_rect.value;

				#if ENABLE_SCRIPT
				if (selected_tool == EditTool.PALETTE_TOOL && item == null && e.Button == NMouseButtons.Middle) {
					EditorManager.EditMode =EditMode.MIDDLE_DRAG;
					parent.form.mMiddleButtonVScroll = parent.form.vScroll.Value;
					parent.form.mMiddleButtonHScroll = parent.form.hScroll.Value;
					return;
				}
				#endif

				int selected = EditorManager.Selected;
				VsqFileEx vsq = MusicManager.getVsqFile();
				VsqTrack vsq_track = vsq.Track[selected];
				int key_width = EditorManager.keyWidth;

				// マウス位置にある音符を検索
				if (item == null) {
					if (e.Button == NMouseButtons.Left) {
						EditorManager.IsWholeSelectedIntervalEnabled = false;
					}
					#region 音符がなかった時
					#if DEBUG
					CDebug.WriteLine("    No Event");
					#endif
					if (EditorManager.itemSelection.getLastEvent() != null) {
						parent.executeLyricChangeCommand();
					}
					bool start_mouse_hover_generator = true;

					// CTRLキーを押しながら範囲選択
					if ((modefier & AwtHost.ModifierKeys) == AwtHost.ModifierKeys) {
						EditorManager.IsWholeSelectedIntervalEnabled = true;
						EditorManager.IsCurveSelectedIntervalEnabled = false;
						EditorManager.itemSelection.clearPoint();
						int startClock = EditorManager.clockFromXCoord(e.X);
						if (EditorManager.editorConfig.CurveSelectingQuantized) {
							int unit = EditorManager.getPositionQuantizeClock();
							startClock = FormMainModel.Quantize(startClock, unit);
						}
						EditorManager.mWholeSelectedInterval = new SelectedRegion(startClock);
						EditorManager.mWholeSelectedInterval.setEnd(startClock);
						EditorManager.mIsPointerDowned = true;
					} else {
						DrawObject vibrato_dobj = null;
						if (selected_tool == EditTool.LINE || selected_tool == EditTool.PENCIL) {
							// ビブラート範囲の編集
							int px_vibrato_length = 0;
							mVibratoEditingId = -1;
							Rectangle pxFound = new Rectangle();
							List<DrawObject> target_list = EditorManager.mDrawObjects[selected - 1];
							int count = target_list.Count;
							for (int i = 0; i < count; i++) {
								DrawObject dobj = target_list[i];
								if (dobj.mRectangleInPixel.Width <= dobj.mVibratoDelayInPixel) {
									continue;
								}
								if (dobj.mRectangleInPixel.X + key_width + dobj.mRectangleInPixel.Width - stdx < 0) {
									continue;
								} else if (parent.form.pictPianoRoll.Width < dobj.mRectangleInPixel.X + key_width - stdx) {
									break;
								}
								Rectangle rc = new Rectangle(dobj.mRectangleInPixel.X + key_width + dobj.mVibratoDelayInPixel - stdx - Consts._EDIT_HANDLE_WIDTH / 2,
									dobj.mRectangleInPixel.Y + (int)(100 * parent.form.Model.ScaleY) - stdy,
									Consts._EDIT_HANDLE_WIDTH,
									(int)(100 * parent.form.Model.ScaleY));
								if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
									vibrato_dobj = dobj;
									//vibrato_found = true;
									mVibratoEditingId = dobj.mInternalID;
									pxFound.X = dobj.mRectangleInPixel.X;
									pxFound.Y = dobj.mRectangleInPixel.Y;
									pxFound.Width = dobj.mRectangleInPixel.Width;
									pxFound.Height = dobj.mRectangleInPixel.Height;// = new Rectangle dobj.mRectangleInPixel;
									pxFound.X += key_width;
									px_vibrato_length = dobj.mRectangleInPixel.Width - dobj.mVibratoDelayInPixel;
									break;
								}
							}
							if (vibrato_dobj != null) {
								int clock = EditorManager.clockFromXCoord(pxFound.X + pxFound.Width - px_vibrato_length - stdx);
								int note = vibrato_dobj.mNote - 1;// EditorManager.noteFromYCoord( pxFound.y + (int)(100 * EditorManager.getScaleY()) - stdy );
								int length = vibrato_dobj.mClock + vibrato_dobj.mLength - clock;// (int)(pxFound.Width * EditorManager.ScaleXInv);
								EditorManager.mAddingEvent = new VsqEvent(clock, new VsqID(0));
								EditorManager.mAddingEvent.ID.type = VsqIDType.Anote;
								EditorManager.mAddingEvent.ID.Note = note;
								EditorManager.mAddingEvent.ID.setLength(length);
								EditorManager.mAddingEventLength = vibrato_dobj.mLength;
								EditorManager.mAddingEvent.ID.VibratoDelay = length - (int)(px_vibrato_length * parent.form.Model.ScaleXInv);
								EditorManager.EditMode = EditMode.EDIT_VIBRATO_DELAY;
								start_mouse_hover_generator = false;
							}
						}
						if (vibrato_dobj == null) {
							if ((selected_tool == EditTool.PENCIL || selected_tool == EditTool.LINE) &&
								e.Button == NMouseButtons.Left &&
								e.X >= key_width) {
								int clock = EditorManager.clockFromXCoord(e.X);
								if (MusicManager.getVsqFile().getPreMeasureClocks() - EditorManager.editorConfig.PxTolerance * parent.form.Model.ScaleXInv <= clock) {
									//10ピクセルまでは許容範囲
									if (MusicManager.getVsqFile().getPreMeasureClocks() > clock) { //だけど矯正するよ。
										clock = MusicManager.getVsqFile().getPreMeasureClocks();
									}
									int note = EditorManager.noteFromYCoord(e.Y);
									EditorManager.itemSelection.clearEvent();
									int unit = EditorManager.getPositionQuantizeClock();
									int new_clock = FormMainModel.Quantize(clock, unit);
									EditorManager.mAddingEvent = new VsqEvent(new_clock, new VsqID(0));
									// デフォルトの歌唱スタイルを適用する
									EditorManager.editorConfig.applyDefaultSingerStyle(EditorManager.mAddingEvent.ID);
									if (parent.form.mPencilMode.getMode() == PencilModeEnum.Off) {
										EditorManager.EditMode = EditMode.ADD_ENTRY;
										parent.form.mButtonInitial = new Point(e.X, e.Y);
										EditorManager.mAddingEvent.ID.setLength(0);
										EditorManager.mAddingEvent.ID.Note = note;
										parent.form.Cursor = Cursors.Default;
										#if DEBUG
										CDebug.WriteLine("    EditMode=" + EditorManager.EditMode);
										#endif
									} else {
										EditorManager.EditMode = EditMode.ADD_FIXED_LENGTH_ENTRY;
										EditorManager.mAddingEvent.ID.setLength(parent.form.mPencilMode.getUnitLength());
										EditorManager.mAddingEvent.ID.Note = note;
										parent.form.Cursor = Cursors.Default;
									}
								} else {
									SystemSounds.Asterisk.Play();
								}
								#if ENABLE_SCRIPT
							} else if ((selected_tool == EditTool.ARROW || selected_tool == EditTool.PALETTE_TOOL) && e.Button == NMouseButtons.Left) {
								#else
								} else if ( (selected_tool == EditTool.ARROW) && e.Button == BMouseButtons.Left ) {
								#endif
								EditorManager.IsWholeSelectedIntervalEnabled = false;
								EditorManager.itemSelection.clearEvent();
								EditorManager.mMouseDownLocation = new Point(e.X + stdx, e.Y + stdy);
								EditorManager.mIsPointerDowned = true;
								#if DEBUG
								CDebug.WriteLine("    EditMode=" + EditorManager.EditMode);
								#endif
							}
						}
					}
					if (e.Button == NMouseButtons.Right && !EditorManager.editorConfig.PlayPreviewWhenRightClick) {
						start_mouse_hover_generator = false;
					}
					#if ENABLE_MOUSEHOVER
					if ( start_mouse_hover_generator ) {
						mMouseHoverThread = new Thread( new ParameterizedThreadStart( MouseHoverEventGenerator ) );
						mMouseHoverThread.Start( EditorManager.noteFromYCoord( e.Y ) );
					}
					#endif
					#endregion
				} else {
					#region 音符があった時
					#if DEBUG
					CDebug.WriteLine("    Event Found");
					#endif
					if (EditorManager.itemSelection.isEventContains(selected, item.InternalID)) {
						parent.executeLyricChangeCommand();
					}
					parent.hideInputTextBox();
					if (selected_tool != EditTool.ERASER) {
						#if ENABLE_MOUSEHOVER
						mMouseHoverThread = new Thread( new ParameterizedThreadStart( MouseHoverEventGenerator ) );
						mMouseHoverThread.Start( item.ID.Note );
						#endif
					}

					// まず、両端の編集モードに移行可能かどうか調べる
					if (item.ID.type != VsqIDType.Aicon ||
						(item.ID.type == VsqIDType.Aicon && !item.ID.IconDynamicsHandle.isDynaffType())) {
						#if ENABLE_SCRIPT
						if (selected_tool != EditTool.ERASER && selected_tool != EditTool.PALETTE_TOOL && e.Button == NMouseButtons.Left) {
						#else
						if ( selected_tool != EditTool.ERASER && e.Button == BMouseButtons.Left ) {
						#endif
							int min_width = 4 * Consts._EDIT_HANDLE_WIDTH;
							foreach (var dobj in EditorManager.mDrawObjects[selected - 1]) {
								int edit_handle_width = Consts._EDIT_HANDLE_WIDTH;
								if (dobj.mRectangleInPixel.Width < min_width) {
									edit_handle_width = dobj.mRectangleInPixel.Width / 4;
								}

								// 左端の"のり代"にマウスがあるかどうか
								Rectangle rc = new Rectangle(dobj.mRectangleInPixel.X - stdx + key_width,
									dobj.mRectangleInPixel.Y - stdy,
									edit_handle_width,
									dobj.mRectangleInPixel.Height);
								if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
									EditorManager.IsWholeSelectedIntervalEnabled = false;
									EditorManager.EditMode = EditMode.EDIT_LEFT_EDGE;
									if (!EditorManager.itemSelection.isEventContains(selected, item.InternalID)) {
										EditorManager.itemSelection.clearEvent();
									}
									EditorManager.itemSelection.addEvent(item.InternalID);
									parent.form.Cursor = Cursors.VSplit;
									parent.form.refreshScreen();
									#if DEBUG
									CDebug.WriteLine("    EditMode=" + EditorManager.EditMode);
									#endif
									return;
								}

								// 右端の糊代にマウスがあるかどうか
								rc = new Rectangle(dobj.mRectangleInPixel.X + key_width + dobj.mRectangleInPixel.Width - stdx - edit_handle_width,
									dobj.mRectangleInPixel.Y - stdy,
									edit_handle_width,
									dobj.mRectangleInPixel.Height);
								if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
									EditorManager.IsWholeSelectedIntervalEnabled = false;
									EditorManager.EditMode = EditMode.EDIT_RIGHT_EDGE;
									if (!EditorManager.itemSelection.isEventContains(selected, item.InternalID)) {
										EditorManager.itemSelection.clearEvent();
									}
									EditorManager.itemSelection.addEvent(item.InternalID);
									parent.form.Cursor = Cursors.VSplit;
									parent.form.refreshScreen();
									return;
								}
							}
						}
					}

					if (e.Button == NMouseButtons.Left || e.Button == NMouseButtons.Middle) {
						#if ENABLE_SCRIPT
						if (selected_tool == EditTool.PALETTE_TOOL) {
							EditorManager.IsWholeSelectedIntervalEnabled = false;
							EditorManager.EditMode = EditMode.NONE;
							EditorManager.itemSelection.clearEvent();
							EditorManager.itemSelection.addEvent(item.InternalID);
						} else
						#endif
							if (selected_tool != EditTool.ERASER) {
								mMouseMoveInit = new Point(e.X + stdx, e.Y + stdy);
								int head_x = EditorManager.xCoordFromClocks(item.Clock);
								mMouseMoveOffset = e.X - head_x;
								if ((modefier & Keys.Shift) == Keys.Shift) {
									// シフトキー同時押しによる範囲選択
									List<int> add_required = new List<int>();
									add_required.Add(item.InternalID);

									// 現在の選択アイテムがある場合，
									// 直前に選択したアイテムと，現在選択しようとしているアイテムとの間にあるアイテムを
									// 全部選択する
									SelectedEventEntry sel = EditorManager.itemSelection.getLastEvent();
									if (sel != null) {
										int last_id = sel.original.InternalID;
										int last_clock = 0;
										int this_clock = 0;
										bool this_found = false, last_found = false;
										for (Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
											VsqEvent ev = itr.next();
											if (ev.InternalID == last_id) {
												last_clock = ev.Clock;
												last_found = true;
											} else if (ev.InternalID == item.InternalID) {
												this_clock = ev.Clock;
												this_found = true;
											}
											if (last_found && this_found) {
												break;
											}
										}
										int start = Math.Min(last_clock, this_clock);
										int end = Math.Max(last_clock, this_clock);
										for (Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
											VsqEvent ev = itr.next();
											if (start <= ev.Clock && ev.Clock <= end) {
												if (!add_required.Contains(ev.InternalID)) {
													add_required.Add(ev.InternalID);
												}
											}
										}
									}
									EditorManager.itemSelection.addEventAll(add_required);
								} else if ((modefier & AwtHost.ModifierKeys) == AwtHost.ModifierKeys) {
									// CTRLキーを押しながら選択／選択解除
									if (EditorManager.itemSelection.isEventContains(selected, item.InternalID)) {
										EditorManager.itemSelection.removeEvent(item.InternalID);
									} else {
										EditorManager.itemSelection.addEvent(item.InternalID);
									}
								} else {
									if (!EditorManager.itemSelection.isEventContains(selected, item.InternalID)) {
										// MouseDownしたアイテムが、まだ選択されていなかった場合。当該アイテム単独に選択しなおす
										EditorManager.itemSelection.clearEvent();
									}
									EditorManager.itemSelection.addEvent(item.InternalID);
								}

								// 範囲選択モードで、かつマウス位置の音符がその範囲に入っていた場合にのみ、MOVE_ENTRY_WHOLE_WAIT_MOVEに移行
								if (EditorManager.IsWholeSelectedIntervalEnabled &&
									EditorManager.mWholeSelectedInterval.getStart() <= item.Clock &&
									item.Clock <= EditorManager.mWholeSelectedInterval.getEnd()) {
									EditorManager.EditMode = EditMode.MOVE_ENTRY_WHOLE_WAIT_MOVE;
									EditorManager.mWholeSelectedIntervalStartForMoving = EditorManager.mWholeSelectedInterval.getStart();
								} else {
									EditorManager.IsWholeSelectedIntervalEnabled = false;
									EditorManager.EditMode = EditMode.MOVE_ENTRY_WAIT_MOVE;
								}

								parent.form.Cursor = Cursors.Hand;
								#if DEBUG
								CDebug.WriteLine("    EditMode=" + EditorManager.EditMode);
								CDebug.WriteLine("    m_config.SelectedEvent.Count=" + EditorManager.itemSelection.getEventCount());
								#endif
							}
					}
					#endregion
				}
				parent.form.refreshScreen();
			}

			public void RunPianoRollMouseMove(MouseEventArgs e)
			{
				lock (EditorManager.mDrawObjects) {
					if (parent.form.mFormActivated) {
						#if ENABLE_PROPERTY
						if (EditorManager.InputTextBox != null && !EditorManager.InputTextBox.IsDisposed && !EditorManager.InputTextBox.Visible && !EditorManager.propertyPanel.isEditing()) {
						#else
						if (EditorManager.InputTextBox != null && !EditorManager.InputTextBox.IsDisposed && !EditorManager.InputTextBox.Visible) {
						#endif
							parent.form.pictPianoRoll.Focus();
						}
					}

					EditMode edit_mode = EditorManager.EditMode;
					int stdx = parent.form.Model.StartToDrawX;
					int stdy = parent.form.Model.StartToDrawY;
					int selected = EditorManager.Selected;
					EditTool selected_tool = EditorManager.SelectedTool;

					if (edit_mode == EditMode.CURVE_ON_PIANOROLL && EditorManager.mCurveOnPianoroll) {
						parent.form.pictPianoRoll.mMouseTracer.append(e.X + stdx, e.Y + stdy);
						if (!parent.timer.Enabled) {
							parent.form.refreshScreen();
						}
						return;
					}

					if (!mMouseMoved && edit_mode == EditMode.MIDDLE_DRAG) {
						parent.form.Cursor = Cursors.Hand;
					}

					if (e.X != parent.form.mButtonInitial.X || e.Y != parent.form.mButtonInitial.Y) {
						mMouseMoved = true;
					}
					if (!(edit_mode == EditMode.MIDDLE_DRAG) && EditorManager.isPlaying()) {
						return;
					}

					if (edit_mode == EditMode.MOVE_ENTRY_WAIT_MOVE ||
						edit_mode == EditMode.MOVE_ENTRY_WHOLE_WAIT_MOVE) {
						int x = e.X + stdx;
						int y = e.Y + stdy;
						if (mMouseMoveInit.X != x || mMouseMoveInit.Y != y) {
							if (edit_mode == EditMode.MOVE_ENTRY_WAIT_MOVE) {
								EditorManager.EditMode = EditMode.MOVE_ENTRY;
								edit_mode = EditMode.MOVE_ENTRY;
							} else {
								EditorManager.EditMode = EditMode.MOVE_ENTRY_WHOLE;
								edit_mode = EditMode.MOVE_ENTRY_WHOLE;
							}
						}
					}

					#if ENABLE_MOUSEHOVER
					if (mMouseMoved && mMouseHoverThread != null) {
						mMouseHoverThread.Abort();
					}
					#endif

					int clock = EditorManager.clockFromXCoord(e.X);
					if (parent.form.mMouseDowned) {
						if (mExtDragX == ExtDragXMode.NONE) {
							if (EditorManager.keyWidth > e.X) {
								mExtDragX = ExtDragXMode.LEFT;
							} else if (parent.form.pictPianoRoll.Width < e.X) {
								mExtDragX = ExtDragXMode.RIGHT;
							}
						} else {
							if (EditorManager.keyWidth <= e.X && e.X <= parent.form.pictPianoRoll.Width) {
								mExtDragX = ExtDragXMode.NONE;
							}
						}

						if (mExtDragY == ExtDragYMode.NONE) {
							if (0 > e.Y) {
								mExtDragY = ExtDragYMode.UP;
							} else if (parent.form.pictPianoRoll.Height < e.Y) {
								mExtDragY = ExtDragYMode.DOWN;
							}
						} else {
							if (0 <= e.Y && e.Y <= parent.form.pictPianoRoll.Height) {
								mExtDragY = ExtDragYMode.NONE;
							}
						}
					} else {
						mExtDragX = ExtDragXMode.NONE;
						mExtDragY = ExtDragYMode.NONE;
					}

					if (edit_mode == EditMode.MIDDLE_DRAG) {
						mExtDragX = ExtDragXMode.NONE;
						mExtDragY = ExtDragYMode.NONE;
					}

					double now = 0, dt = 0;
					if (mExtDragX != ExtDragXMode.NONE || mExtDragY != ExtDragYMode.NONE) {
						now = PortUtil.getCurrentTime();
						dt = now - parent.form.mTimerDragLastIgnitted;
					}
					if (mExtDragX == ExtDragXMode.RIGHT || mExtDragX == ExtDragXMode.LEFT) {
						int px_move = EditorManager.editorConfig.MouseDragIncrement;
						if (px_move / dt > EditorManager.editorConfig.MouseDragMaximumRate) {
							px_move = (int)(dt * EditorManager.editorConfig.MouseDragMaximumRate);
						}
						double d_draft;
						if (mExtDragX == ExtDragXMode.LEFT) {
							px_move *= -1;
						}
						int left_clock = EditorManager.clockFromXCoord(EditorManager.keyWidth);
						float inv_scale_x = parent.form.Model.ScaleXInv;
						int dclock = (int)(px_move * inv_scale_x);
						d_draft = 5 * inv_scale_x + left_clock + dclock;
						if (d_draft < 0.0) {
							d_draft = 0.0;
						}
						int draft = (int)d_draft;
						if (parent.form.hScroll.Maximum < draft) {
							if (edit_mode == EditMode.ADD_ENTRY ||
								edit_mode == EditMode.MOVE_ENTRY ||
								edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY ||
								edit_mode == EditMode.DRAG_DROP) {
								parent.form.hScroll.Maximum = draft;
							} else {
								draft = parent.form.hScroll.Maximum;
							}
						}
						if (draft < parent.form.hScroll.Minimum) {
							draft = parent.form.hScroll.Minimum;
						}
						parent.form.hScroll.Value = draft;
					}
					if (mExtDragY == ExtDragYMode.UP || mExtDragY == ExtDragYMode.DOWN) {
						int min = parent.form.vScroll.Minimum;
						int max = parent.form.vScroll.Maximum - parent.form.vScroll.LargeChange;
						int px_move = EditorManager.editorConfig.MouseDragIncrement;
						if (px_move / dt > EditorManager.editorConfig.MouseDragMaximumRate) {
							px_move = (int)(dt * EditorManager.editorConfig.MouseDragMaximumRate);
						}
						px_move += 50;
						if (mExtDragY == ExtDragYMode.UP) {
							px_move *= -1;
						}
						int draft = parent.form.vScroll.Value + px_move;
						if (draft < 0) {
							draft = 0;
						}
						int df = (int)draft;
						if (df < min) {
							df = min;
						} else if (max < df) {
							df = max;
						}
						parent.form.vScroll.Value = df;
					}
					if (mExtDragX != ExtDragXMode.NONE || mExtDragY != ExtDragYMode.NONE) {
						parent.form.mTimerDragLastIgnitted = now;
					}

					// 選択範囲にあるイベントを選択．
					if (EditorManager.mIsPointerDowned) {
						if (EditorManager.IsWholeSelectedIntervalEnabled) {
							int endClock = EditorManager.clockFromXCoord(e.X);
							if (EditorManager.editorConfig.CurveSelectingQuantized) {
								int unit = EditorManager.getPositionQuantizeClock();
								endClock = FormMainModel.Quantize(endClock, unit);
							}
							EditorManager.mWholeSelectedInterval.setEnd(endClock);
						} else {
							Point mouse = new Point(e.X + stdx, e.Y + stdy);
							int tx, ty, twidth, theight;
							int lx = EditorManager.mMouseDownLocation.X;
							if (lx < mouse.X) {
								tx = lx;
								twidth = mouse.X - lx;
							} else {
								tx = mouse.X;
								twidth = lx - mouse.X;
							}
							int ly = EditorManager.mMouseDownLocation.Y;
							if (ly < mouse.Y) {
								ty = ly;
								theight = mouse.Y - ly;
							} else {
								ty = mouse.Y;
								theight = ly - mouse.Y;
							}

							Rectangle rect = new Rectangle(tx, ty, twidth, theight);
							List<int> add_required = new List<int>();
							int internal_id = -1;
							foreach (var dobj in EditorManager.mDrawObjects[selected - 1]) {
								int x0 = dobj.mRectangleInPixel.X + EditorManager.keyWidth;
								int x1 = dobj.mRectangleInPixel.X + EditorManager.keyWidth + dobj.mRectangleInPixel.Width;
								int y0 = dobj.mRectangleInPixel.Y;
								int y1 = dobj.mRectangleInPixel.Y + dobj.mRectangleInPixel.Height;
								internal_id = dobj.mInternalID;
								if (x1 < tx) {
									continue;
								}
								if (tx + twidth < x0) {
									break;
								}
								bool found = Utility.isInRect(new Point(x0, y0), rect) |
									Utility.isInRect(new Point(x0, y1), rect) |
									Utility.isInRect(new Point(x1, y0), rect) |
									Utility.isInRect(new Point(x1, y1), rect);
								if (found) {
									add_required.Add(internal_id);
								} else {
									if (x0 <= tx && tx + twidth <= x1) {
										if (ty < y0) {
											if (y0 <= ty + theight) {
												add_required.Add(internal_id);
											}
										} else if (y0 <= ty && ty < y1) {
											add_required.Add(internal_id);
										}
									} else if (y0 <= ty && ty + theight <= y1) {
										if (tx < x0) {
											if (x0 <= tx + twidth) {
												add_required.Add(internal_id);
											}
										} else if (x0 <= tx && tx < x1) {
											add_required.Add(internal_id);
										}
									}
								}
							}
							List<int> remove_required = new List<int>();
							foreach (var selectedEvent in EditorManager.itemSelection.getEventIterator()) {
								if (!add_required.Contains(selectedEvent.original.InternalID)) {
									remove_required.Add(selectedEvent.original.InternalID);
								}
							}
							if (remove_required.Count > 0) {
								EditorManager.itemSelection.removeEventRange(PortUtil.convertIntArray(remove_required.ToArray()));
							}
							add_required.RemoveAll((id) => EditorManager.itemSelection.isEventContains(selected, id));
							EditorManager.itemSelection.addEventAll(add_required);
						}
					}

					if (edit_mode == EditMode.MIDDLE_DRAG) {
						#region MiddleDrag
						int drafth = parent.form.computeHScrollValueForMiddleDrag(e.X);
						int draftv = parent.form.computeVScrollValueForMiddleDrag(e.Y);
						bool moved = false;
						if (drafth != parent.form.hScroll.Value) {
							//moved = true;
							//hScroll.beQuiet();
							parent.form.hScroll.Value = drafth;
						}
						if (draftv != parent.form.vScroll.Value) {
							//moved = true;
							//vScroll.beQuiet();
							parent.form.vScroll.Value = draftv;
						}
						//if ( moved ) {
						//    vScroll.setQuiet( false );
						//    hScroll.setQuiet( false );
						//    refreshScreen( true );
						//}
						parent.form.refreshScreen(true);
						if (EditorManager.isPlaying()) {
							return;
						}
						#endregion
						return;
					} else if (edit_mode == EditMode.ADD_ENTRY) {
						#region ADD_ENTRY
						int unit = EditorManager.getLengthQuantizeClock();
						int length = clock - EditorManager.mAddingEvent.Clock;
						int odd = length % unit;
						int new_length = length - odd;

						if (unit * parent.form.Model.ScaleX > 10) { //これをしないと、グリッド2個分増えることがある
							int next_clock = EditorManager.clockFromXCoord(e.X + 10);
							int next_length = next_clock - EditorManager.mAddingEvent.Clock;
							int next_new_length = next_length - (next_length % unit);
							if (next_new_length == new_length + unit) {
								new_length = next_new_length;
							}
						}

						if (new_length <= 0) {
							new_length = 0;
						}
						EditorManager.mAddingEvent.ID.setLength(new_length);
						#endregion
					} else if (edit_mode == EditMode.MOVE_ENTRY || edit_mode == EditMode.MOVE_ENTRY_WHOLE) {
						#region MOVE_ENTRY, MOVE_ENTRY_WHOLE
						if (EditorManager.itemSelection.getEventCount() > 0) {
							VsqEvent original = EditorManager.itemSelection.getLastEvent().original;
							int note = EditorManager.noteFromYCoord(e.Y);                           // 現在のマウス位置でのnote
							int note_init = original.ID.Note;
							int dnote = (edit_mode == EditMode.MOVE_ENTRY) ? note - note_init : 0;

							int tclock = EditorManager.clockFromXCoord(e.X - mMouseMoveOffset);
							int clock_init = original.Clock;

							int dclock = tclock - clock_init;

							if (EditorManager.editorConfig.getPositionQuantize() != QuantizeMode.off) {
								int unit = EditorManager.getPositionQuantizeClock();
								int new_clock = FormMainModel.Quantize(original.Clock + dclock, unit);
								dclock = new_clock - clock_init;
							}

							EditorManager.mWholeSelectedIntervalStartForMoving = EditorManager.mWholeSelectedInterval.getStart() + dclock;

							foreach (var item in EditorManager.itemSelection.getEventIterator()) {
								int new_clock = item.original.Clock + dclock;
								int new_note = item.original.ID.Note + dnote;
								item.editing.Clock = new_clock;
								item.editing.ID.Note = new_note;
							}
						}
						#endregion
					} else if (edit_mode == EditMode.EDIT_LEFT_EDGE) {
						#region EditLeftEdge
						int unit = EditorManager.getLengthQuantizeClock();
						VsqEvent original = EditorManager.itemSelection.getLastEvent().original;
						int clock_init = original.Clock;
						int dclock = clock - clock_init;
						foreach (var item in EditorManager.itemSelection.getEventIterator()) {
							int end_clock = item.original.Clock + item.original.ID.getLength();
							int new_clock = item.original.Clock + dclock;
							int new_length = FormMainModel.Quantize(end_clock - new_clock, unit);
							if (new_length <= 0) {
								new_length = unit;
							}
							item.editing.Clock = end_clock - new_length;
							if (EditorManager.vibratoLengthEditingRule == VibratoLengthEditingRule.PERCENTAGE) {
								double percentage = item.original.ID.VibratoDelay / (double)item.original.ID.getLength() * 100.0;
								int newdelay = (int)(new_length * percentage / 100.0);
								item.editing.ID.VibratoDelay = newdelay;
							}
							item.editing.ID.setLength(new_length);
						}
						#endregion
					} else if (edit_mode == EditMode.EDIT_RIGHT_EDGE) {
						#region EditRightEdge
						int unit = EditorManager.getLengthQuantizeClock();

						VsqEvent original = EditorManager.itemSelection.getLastEvent().original;
						int dlength = clock - (original.Clock + original.ID.getLength());
						foreach (var item in EditorManager.itemSelection.getEventIterator()) {
							int new_length = FormMainModel.Quantize(item.original.ID.getLength() + dlength, unit);
							if (new_length <= 0) {
								new_length = unit;
							}
							if (EditorManager.vibratoLengthEditingRule == VibratoLengthEditingRule.PERCENTAGE) {
								double percentage = item.original.ID.VibratoDelay / (double)item.original.ID.getLength() * 100.0;
								int newdelay = (int)(new_length * percentage / 100.0);
								item.editing.ID.VibratoDelay = newdelay;
							}
							item.editing.ID.setLength(new_length);
							#if DEBUG
							Logger.StdOut("FormMain#pictPianoRoll_MouseMove; length(before,after)=(" + item.original.ID.getLength() + "," + item.editing.ID.getLength() + ")");
							#endif
						}
						#endregion
					} else if (edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY) {
						#region AddFixedLengthEntry
						int note = EditorManager.noteFromYCoord(e.Y);
						int unit = EditorManager.getPositionQuantizeClock();
						int new_clock = FormMainModel.Quantize(EditorManager.clockFromXCoord(e.X), unit);
						EditorManager.mAddingEvent.ID.Note = note;
						EditorManager.mAddingEvent.Clock = new_clock;
						#endregion
					} else if (edit_mode == EditMode.EDIT_VIBRATO_DELAY) {
						#region EditVibratoDelay
						int new_vibrato_start = clock;
						int old_vibrato_end = EditorManager.mAddingEvent.Clock + EditorManager.mAddingEvent.ID.getLength();
						int new_vibrato_length = old_vibrato_end - new_vibrato_start;
						int max_length = (int)(EditorManager.mAddingEventLength - Consts._PX_ACCENT_HEADER * parent.form.Model.ScaleXInv);
						if (max_length < 0) {
							max_length = 0;
						}
						if (new_vibrato_length > max_length) {
							new_vibrato_start = old_vibrato_end - max_length;
							new_vibrato_length = max_length;
						}
						if (new_vibrato_length < 0) {
							new_vibrato_start = old_vibrato_end;
							new_vibrato_length = 0;
						}
						EditorManager.mAddingEvent.Clock = new_vibrato_start;
						EditorManager.mAddingEvent.ID.setLength(new_vibrato_length);
						if (!parent.timer.Enabled) {
							parent.form.refreshScreen();
						}
						#endregion
						return;
					} else if (edit_mode == EditMode.DRAG_DROP) {
						#region DRAG_DROP
						// クオンタイズの処理
						int unit = EditorManager.getPositionQuantizeClock();
						int clock1 = FormMainModel.Quantize(clock, unit);
						int note = EditorManager.noteFromYCoord(e.Y);
						EditorManager.mAddingEvent.Clock = clock1;
						EditorManager.mAddingEvent.ID.Note = note;
						#endregion
					}

					// カーソルの形を決める
					if (!parent.form.mMouseDowned &&
						edit_mode != EditMode.CURVE_ON_PIANOROLL &&
						!(EditorManager.mCurveOnPianoroll && (selected_tool == EditTool.PENCIL || selected_tool == EditTool.LINE))) {
						bool split_cursor = false;
						bool hand_cursor = false;
						int min_width = 4 * Consts._EDIT_HANDLE_WIDTH;
						foreach (var dobj in EditorManager.mDrawObjects[selected - 1]) {
							Rectangle rc;
							if (dobj.mType != DrawObjectType.Dynaff) {
								int edit_handle_width = Consts._EDIT_HANDLE_WIDTH;
								if (dobj.mRectangleInPixel.Width < min_width) {
									edit_handle_width = dobj.mRectangleInPixel.Width / 4;
								}

								// 音符左側の編集領域
								rc = new Rectangle(
									dobj.mRectangleInPixel.X + EditorManager.keyWidth - stdx,
									dobj.mRectangleInPixel.Y - stdy,
									edit_handle_width,
									(int)(100 * parent.form.Model.ScaleY));
								if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
									split_cursor = true;
									break;
								}

								// 音符右側の編集領域
								rc = new Rectangle(dobj.mRectangleInPixel.X + EditorManager.keyWidth + dobj.mRectangleInPixel.Width - stdx - edit_handle_width,
									dobj.mRectangleInPixel.Y - stdy,
									edit_handle_width,
									(int)(100 * parent.form.Model.ScaleY));
								if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
									split_cursor = true;
									break;
								}
							}

							// 音符本体
							rc = new Rectangle(dobj.mRectangleInPixel.X + EditorManager.keyWidth - stdx,
								dobj.mRectangleInPixel.Y - stdy,
								dobj.mRectangleInPixel.Width,
								dobj.mRectangleInPixel.Height);
							if (dobj.mType == DrawObjectType.Note) {
								if (EditorManager.editorConfig.ShowExpLine && !dobj.mIsOverlapped) {
									rc.Height *= 2;
									if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
										// ビブラートの開始位置
										rc = new Rectangle(dobj.mRectangleInPixel.X + EditorManager.keyWidth + dobj.mVibratoDelayInPixel - stdx - Consts._EDIT_HANDLE_WIDTH / 2,
											dobj.mRectangleInPixel.Y + (int)(100 * parent.form.Model.ScaleY) - stdy,
											Consts._EDIT_HANDLE_WIDTH,
											(int)(100 * parent.form.Model.ScaleY));
										if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
											split_cursor = true;
											break;
										} else {
											hand_cursor = true;
											break;
										}
									}
								} else {
									if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
										hand_cursor = true;
										break;
									}
								}
							} else {
								if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
									hand_cursor = true;
									break;
								}
							}
						}

						if (split_cursor) {
							parent.form.Cursor = Cursors.VSplit;
						} else if (hand_cursor) {
							parent.form.Cursor = Cursors.Hand;
						} else {
							parent.form.Cursor = Cursors.Default;
						}
					}
					if (!parent.timer.Enabled) {
						parent.form.refreshScreen(true);
					}
				}
			}

			/// <summary>
			/// ピアノロールからマウスボタンが離れたときの処理
			/// </summary>
			/// <param name="sender"></param>
			/// <param name="e"></param>
			public void RunPianoRollMouseUp(MouseEventArgs e)
			{
				#if DEBUG
				CDebug.WriteLine("pictureBox1_MouseUp");
				CDebug.WriteLine("    m_config.EditMode=" + EditorManager.EditMode);
				#endif
				EditorManager.mIsPointerDowned = false;
				parent.form.mMouseDowned = false;

				Keys modefiers = AwtHost.ModifierKeys;

				EditMode edit_mode = EditorManager.EditMode;
				VsqFileEx vsq = MusicManager.getVsqFile();
				int selected = EditorManager.Selected;
				VsqTrack vsq_track = vsq.Track[selected];
				CurveType selected_curve = parent.form.TrackSelector.SelectedCurve;
				int stdx = parent.form.Model.StartToDrawX;
				int stdy = parent.form.Model.StartToDrawY;
				double d2_13 = 8192; // = 2^13
				int track_height = (int)(100 * parent.form.Model.ScaleY);
				int half_track_height = track_height / 2;

				if (edit_mode == EditMode.CURVE_ON_PIANOROLL) {
					if (parent.form.pictPianoRoll.mMouseTracer.size() > 1) {
						// マウスの軌跡の左右端(px)
						int px_start = parent.form.pictPianoRoll.mMouseTracer.firstKey();
						int px_end = parent.form.pictPianoRoll.mMouseTracer.lastKey();

						// マウスの軌跡の左右端(クロック)
						int cl_start = EditorManager.clockFromXCoord(px_start - stdx);
						int cl_end = EditorManager.clockFromXCoord(px_end - stdx);

						// 編集が行われたかどうか
						bool edited = false;
						// 作業用のPITカーブのコピー
						VsqBPList pit = (VsqBPList)vsq_track.getCurve("pit").clone();
						VsqBPList pbs = (VsqBPList)vsq_track.getCurve("pbs"); // こっちはクローンしないよ

						// トラック内の全音符に対して、マウス軌跡と被っている部分のPITを編集する
						foreach (var item in vsq_track.getNoteEventIterator()) {
							int cl_item_start = item.Clock;
							if (cl_end < cl_item_start) {
								break;
							}
							int cl_item_end = cl_item_start + item.ID.getLength();
							if (cl_item_end < cl_start) {
								continue;
							}

							// ここに到達するってことは、pitに編集が加えられるってこと。
							edited = true;

							// マウス軌跡と被っている部分のPITを削除
							int cl_remove_start = Math.Max(cl_item_start, cl_start);
							int cl_remove_end = Math.Min(cl_item_end, cl_end);
							int value_at_remove_end = pit.getValue(cl_remove_end);
							int value_at_remove_start = pit.getValue(cl_remove_start);
							List<int> remove = new List<int>();
							foreach (var clock in pit.keyClockIterator()) {
								if (cl_remove_start <= clock && clock <= cl_remove_end) {
									remove.Add(clock);
								}
							}
							foreach (var clock in remove) {
								pit.remove(clock);
							}
							remove = null;

							int px_item_start = EditorManager.xCoordFromClocks(cl_item_start) + stdx;
							int px_item_end = EditorManager.xCoordFromClocks(cl_item_end) + stdx;

							int lastv = value_at_remove_start;
							bool cl_item_end_added = false;
							bool cl_item_start_added = false;
							int last_px = 0, last_py = 0;
							foreach (var p in parent.form.pictPianoRoll.mMouseTracer.iterator()) {
								if (p.X < px_item_start) {
									last_px = p.X;
									last_py = p.Y;
									continue;
								}
								if (px_item_end < p.X) {
									break;
								}

								int clock = EditorManager.clockFromXCoord(p.X - stdx);
								if (clock < cl_item_start) {
									last_px = p.X;
									last_py = p.Y;
									continue;
								} else if (cl_item_end < clock) {
									break;
								}
								double note = EditorManager.noteFromYCoordDoublePrecision(p.Y - stdy - half_track_height);
								int v_pit = (int)(d2_13 / (double)pbs.getValue(clock) * (note - item.ID.Note));

								// 正規化
								if (v_pit < pit.getMinimum()) {
									v_pit = pit.getMinimum();
								} else if (pit.getMaximum() < v_pit) {
									v_pit = pit.getMaximum();
								}

								if (cl_item_start < clock && !cl_item_start_added &&
									cl_start <= cl_item_start && cl_item_start < cl_end) {
									// これから追加しようとしているデータ点の時刻が、音符の開始時刻よりも後なんだけれど、
									// 音符の開始時刻におけるデータをまだ書き込んでない場合
									double a = (p.Y - last_py) / (double)(p.X - last_px);
									double x_at_clock = EditorManager.xCoordFromClocks(cl_item_start) + stdx;
									double ext_y = last_py + a * (x_at_clock - last_px);
									double tnote = EditorManager.noteFromYCoordDoublePrecision((int)(ext_y - stdy - half_track_height));
									int t_vpit = (int)(d2_13 / (double)pbs.getValue(cl_item_start) * (tnote - item.ID.Note));
									pit.add(cl_item_start, t_vpit);
									lastv = t_vpit;
									cl_item_start_added = true;
								}

								// 直前の値と違っている場合にのみ追加
								if (v_pit != lastv) {
									pit.add(clock, v_pit);
									lastv = v_pit;
									if (clock == cl_item_end) {
										cl_item_end_added = true;
									} else if (clock == cl_item_start) {
										cl_item_start_added = true;
									}
								}
							}

							if (!cl_item_end_added &&
								cl_start <= cl_item_end && cl_item_end <= cl_end) {
								pit.add(cl_item_end, lastv);
							}

							pit.add(cl_remove_end, value_at_remove_end);
						}

						// 編集操作が行われた場合のみ、コマンドを発行
						if (edited) {
							CadenciiCommand run = new CadenciiCommand(
								VsqCommand.generateCommandTrackCurveReplace(selected, "PIT", pit));
							EditorManager.editHistory.register(vsq.executeCommand(run));
							parent.form.setEdited(true);
						}
					}
					parent.form.pictPianoRoll.mMouseTracer.clear();
					EditorManager.EditMode = EditMode.NONE;
					return;
				}

				if (edit_mode == EditMode.MIDDLE_DRAG) {
					parent.form.Cursor = Cursors.Default;
				} else if (edit_mode == EditMode.ADD_ENTRY || edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY) {
					#region AddEntry || AddFixedLengthEntry
					if (EditorManager.Selected >= 0) {
						if ((edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY) ||
							(edit_mode == EditMode.ADD_ENTRY && (parent.form.mButtonInitial.X != e.X || parent.form.mButtonInitial.Y != e.Y) && EditorManager.mAddingEvent.ID.getLength() > 0)) {
							if (EditorManager.mAddingEvent.Clock < vsq.getPreMeasureClocks()) {
								SystemSounds.Asterisk.Play();
							} else {
								parent.form.fixAddingEvent();
							}
						}
					}
					#endregion
				} else if (edit_mode == EditMode.MOVE_ENTRY) {
					#region MoveEntry
					#if DEBUG
					Logger.StdOut("FormMain#pictPianoRoll_MouseUp; edit_mode is MOVE_ENTRY");
					#endif
					if (EditorManager.itemSelection.getEventCount() > 0) {
						SelectedEventEntry last_selected_event = EditorManager.itemSelection.getLastEvent();
						#if DEBUG
						Logger.StdOut("FormMain#pictPianoRoll_MouseUp; last_selected_event.original.InternalID=" + last_selected_event.original.InternalID);
						#endif
						VsqEvent original = last_selected_event.original;
						if (original.Clock != last_selected_event.editing.Clock ||
							original.ID.Note != last_selected_event.editing.ID.Note) {
							bool out_of_range = false; // プリメジャーにめり込んでないかどうか
							bool contains_dynamics = false; // Dynaff, Crescend, Desrecendが含まれているかどうか
							VsqTrack copied = (VsqTrack)vsq_track.clone();
							int clockAtPremeasure = vsq.getPreMeasureClocks();
							foreach (var ev in EditorManager.itemSelection.getEventIterator()) {
								int internal_id = ev.original.InternalID;
								if (ev.editing.Clock < clockAtPremeasure) {
									out_of_range = true;
									break;
								}
								if (ev.editing.ID.Note < 0 || 128 < ev.editing.ID.Note) {
									out_of_range = true;
									break;
								}
								for (Iterator<VsqEvent> itr2 = copied.getEventIterator(); itr2.hasNext(); ) {
									VsqEvent item = itr2.next();
									if (item.InternalID == internal_id) {
										item.Clock = ev.editing.Clock;
										item.ID = (VsqID)ev.editing.ID.clone();
										break;
									}
								}
								if (ev.original.ID.type == VsqIDType.Aicon) {
									contains_dynamics = true;
								}
							}
							if (out_of_range) {
								SystemSounds.Asterisk.Play();
							} else {
								if (contains_dynamics) {
									copied.reflectDynamics();
								}
								CadenciiCommand run = VsqFileEx.generateCommandTrackReplace(selected,
									copied,
									vsq.AttachedCurves.get(selected - 1));
								EditorManager.editHistory.register(vsq.executeCommand(run));
								EditorManager.itemSelection.updateSelectedEventInstance();
								parent.form.setEdited(true);
							}
						} else {
							/*if ( (modefier & Keys.Shift) == Keys.Shift || (modefier & Keys.Control) == Keys.Control ) {
                            Rectangle rc;
                            VsqEvent select = IdOfClickedPosition( e.Location, out rc );
                            if ( select != null ) {
                                m_config.addSelectedEvent( item.InternalID );
                            }
                        }*/
						}
						lock (EditorManager.mDrawObjects) {
							EditorManager.mDrawObjects[selected - 1].Sort();
						}
					}
					#endregion
				} else if (edit_mode == EditMode.EDIT_LEFT_EDGE || edit_mode == EditMode.EDIT_RIGHT_EDGE) {
					#region EDIT_LEFT_EDGE | EDIT_RIGHT_EDGE
					if (mMouseMoved) {
						VsqEvent original = EditorManager.itemSelection.getLastEvent().original;
						int count = EditorManager.itemSelection.getEventCount();
						int[] ids = new int[count];
						int[] clocks = new int[count];
						VsqID[] values = new VsqID[count];
						int i = -1;
						bool contains_aicon = false; // dynaff, crescend, decrescendが含まれていればtrue
						foreach (var ev in EditorManager.itemSelection.getEventIterator()) {
							if (ev.original.ID.type == VsqIDType.Aicon) {
								contains_aicon = true;
							}
							i++;

							EditorManager.editLengthOfVsqEvent(ev.editing, ev.editing.ID.getLength(), EditorManager.vibratoLengthEditingRule);
							ids[i] = ev.original.InternalID;
							clocks[i] = ev.editing.Clock;
							values[i] = ev.editing.ID;
						}

						CadenciiCommand run = null;
						if (contains_aicon) {
							VsqFileEx copied_vsq = (VsqFileEx)vsq.clone();
							VsqCommand vsq_command =
								VsqCommand.generateCommandEventChangeClockAndIDContaintsRange(selected,
									ids,
									clocks,
									values);
							copied_vsq.executeCommand(vsq_command);
							VsqTrack copied = (VsqTrack)copied_vsq.Track[selected].clone();
							copied.reflectDynamics();
							run = VsqFileEx.generateCommandTrackReplace(selected,
								copied,
								vsq.AttachedCurves.get(selected - 1));
						} else {
							run = new CadenciiCommand(
								VsqCommand.generateCommandEventChangeClockAndIDContaintsRange(selected,
									ids,
									clocks,
									values));
						}
						EditorManager.editHistory.register(vsq.executeCommand(run));
						parent.form.setEdited(true);
					}
					#endregion
				} else if (edit_mode == EditMode.EDIT_VIBRATO_DELAY) {
					#region EditVibratoDelay
					if (mMouseMoved) {
						double max_length = EditorManager.mAddingEventLength - Consts._PX_ACCENT_HEADER * parent.form.Model.ScaleXInv;
						double rate = EditorManager.mAddingEvent.ID.getLength() / max_length;
						if (rate > 0.99) {
							rate = 1.0;
						}
						int vibrato_length = (int)(EditorManager.mAddingEventLength * rate);
						VsqEvent item = null;
						foreach (var ve in vsq_track.getNoteEventIterator()) {
							if (ve.InternalID == mVibratoEditingId) {
								item = (VsqEvent)ve.clone();
								break;
							}
						}
						if (item != null) {
							if (vibrato_length <= 0) {
								item.ID.VibratoHandle = null;
								item.ID.VibratoDelay = item.ID.getLength();
							} else {
								item.ID.VibratoHandle.setLength(vibrato_length);
								item.ID.VibratoDelay = item.ID.getLength() - vibrato_length;
							}
							CadenciiCommand run = new CadenciiCommand(
								VsqCommand.generateCommandEventChangeIDContaints(selected, mVibratoEditingId, item.ID));
							EditorManager.editHistory.register(vsq.executeCommand(run));
							parent.form.setEdited(true);
						}
					}
					#endregion
				} else if (edit_mode == EditMode.MOVE_ENTRY_WHOLE) {
					#if DEBUG
					Logger.StdOut("FormMain#pictPianoRoll_MouseUp; EditMode.MOVE_ENTRY_WHOLE");
					#endif
					#region MOVE_ENTRY_WHOLE
					int src_clock_start = EditorManager.mWholeSelectedInterval.getStart();
					int src_clock_end = EditorManager.mWholeSelectedInterval.getEnd();
					int dst_clock_start = EditorManager.mWholeSelectedIntervalStartForMoving;
					int dst_clock_end = dst_clock_start + (src_clock_end - src_clock_start);
					int dclock = dst_clock_start - src_clock_start;

					int num = EditorManager.itemSelection.getEventCount();
					int[] selected_ids = new int[num]; // 後段での再選択用のInternalIDのリスト
					int last_selected_id = EditorManager.itemSelection.getLastEvent().original.InternalID;

					// 音符イベントを移動
					VsqTrack work = (VsqTrack)vsq_track.clone();
					int k = 0;
					foreach (var item in EditorManager.itemSelection.getEventIterator()) {
						int internal_id = item.original.InternalID;
						selected_ids[k] = internal_id;
						k++;
						#if DEBUG
						Logger.StdOut("FormMain#pictPianoRoll_MouseUp; internal_id=" + internal_id);
						#endif
						foreach (var vsq_event in work.getNoteEventIterator()) {
							if (internal_id == vsq_event.InternalID) {
								#if DEBUG
								Logger.StdOut("FormMain#pictPianoRoll_MouseUp; before: clock=" + vsq_event.Clock + "; after: clock=" + item.editing.Clock);
								#endif
								vsq_event.Clock = item.editing.Clock;
								break;
							}
						}
					}

					// 全てのコントロールカーブのデータ点を移動
					for (int i = 0; i < BezierCurves.CURVE_USAGE.Length; i++) {
						CurveType curve_type = BezierCurves.CURVE_USAGE[i];
						VsqBPList bplist = work.getCurve(curve_type.getName());
						if (bplist == null) {
							continue;
						}

						// src_clock_startからsrc_clock_endの範囲にあるデータ点をコピー＆削除
						VsqBPList copied = new VsqBPList(bplist.getName(), bplist.getDefault(), bplist.getMinimum(), bplist.getMaximum());
						int size = bplist.size();
						for (int j = size - 1; j >= 0; j--) {
							int clock = bplist.getKeyClock(j);
							if (src_clock_start <= clock && clock <= src_clock_end) {
								VsqBPPair bppair = bplist.getElementB(j);
								copied.add(clock, bppair.value);
								bplist.removeElementAt(j);
							}
						}

						// dst_clock_startからdst_clock_endの範囲にあるコントロールカーブのデータ点をすべて削除
						size = bplist.size();
						for (int j = size - 1; j >= 0; j--) {
							int clock = bplist.getKeyClock(j);
							if (dst_clock_start <= clock && clock <= dst_clock_end) {
								bplist.removeElementAt(j);
							}
						}

						// コピーしたデータを、クロックをずらしながら追加
						size = copied.size();
						for (int j = 0; j < size; j++) {
							int clock = copied.getKeyClock(j);
							VsqBPPair bppair = copied.getElementB(j);
							bplist.add(clock + dclock, bppair.value);
						}
					}

					// コマンドを作成＆実行
					CadenciiCommand run = VsqFileEx.generateCommandTrackReplace(selected,
						work,
						vsq.AttachedCurves.get(selected - 1));
					EditorManager.editHistory.register(vsq.executeCommand(run));

					// 選択範囲を更新
					EditorManager.mWholeSelectedInterval = new SelectedRegion(dst_clock_start);
					EditorManager.mWholeSelectedInterval.setEnd(dst_clock_end);
					EditorManager.mWholeSelectedIntervalStartForMoving = dst_clock_start;

					// 音符の再選択
					EditorManager.itemSelection.clearEvent();
					List<int> list_selected_ids = new List<int>();
					for (int i = 0; i < num; i++) {
						list_selected_ids.Add(selected_ids[i]);
					}
					EditorManager.itemSelection.addEventAll(list_selected_ids);
					EditorManager.itemSelection.addEvent(last_selected_id);

					parent.form.setEdited(true);
					#endregion
				} else if (EditorManager.IsWholeSelectedIntervalEnabled) {
					int start = EditorManager.mWholeSelectedInterval.getStart();
					int end = EditorManager.mWholeSelectedInterval.getEnd();
					#if DEBUG
					Logger.StdOut("FormMain#pictPianoRoll_MouseUp; WholeSelectedInterval; (start,end)=" + start + ", " + end);
					#endif
					EditorManager.itemSelection.clearEvent();

					// 音符の選択状態を更新
					List<int> add_required_event = new List<int>();
					for (Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
						VsqEvent ve = itr.next();
						if (start <= ve.Clock && ve.Clock + ve.ID.getLength() <= end) {
							add_required_event.Add(ve.InternalID);
						}
					}
					EditorManager.itemSelection.addEventAll(add_required_event);

					// コントロールカーブ点の選択状態を更新
					List<long> add_required_point = new List<long>();
					VsqBPList list = vsq_track.getCurve(selected_curve.getName());
					if (list != null) {
						int count = list.size();
						for (int i = 0; i < count; i++) {
							int clock = list.getKeyClock(i);
							if (clock < start) {
								continue;
							} else if (end < clock) {
								break;
							} else {
								VsqBPPair v = list.getElementB(i);
								add_required_point.Add(v.id);
							}
						}
					}
					if (add_required_point.Count > 0) {
						EditorManager.itemSelection.addPointAll(selected_curve,
							PortUtil.convertLongArray(add_required_point.ToArray()));
					}
				}
				heaven:
				EditorManager.EditMode = EditMode.NONE;
				parent.form.refreshScreen(true);
			}

			public void RunPianoRollMouseWheelCommand(MouseEventArgs e)
			{
				Keys modifier = AwtHost.ModifierKeys;
				bool horizontal = (modifier & Keys.Shift) == Keys.Shift;
				if (EditorManager.editorConfig.ScrollHorizontalOnWheel) {
					horizontal = !horizontal;
				}
				if ((modifier & Keys.Control) == Keys.Control) {
					// ピアノロール拡大率を変更
					if (horizontal) {
						int max = parent.form.trackBar.Maximum;
						int min = parent.form.trackBar.Minimum;
						int width = max - min;
						int delta = (width / 10) * (e.Delta > 0 ? 1 : -1);
						int old_tbv = parent.form.trackBar.Value;
						int draft = old_tbv + delta;
						if (draft < min) {
							draft = min;
						}
						if (max < draft) {
							draft = max;
						}
						if (old_tbv != draft) {

							// マウス位置を中心に拡大されるようにしたいので．
							// マウスのスクリーン座標
							Point screen_p_at_mouse = Screen.Instance.GetScreenMousePosition();
							// ピアノロール上でのマウスのx座標
							int x_at_mouse = parent.form.pictPianoRoll.PointToClient(new Cadencii.Gui.Point(screen_p_at_mouse.X, screen_p_at_mouse.Y)).X;
							// マウス位置でのクロック -> こいつが保存される
							int clock_at_mouse = EditorManager.clockFromXCoord(x_at_mouse);
							// 古い拡大率
							float scale0 = parent.form.Model.ScaleX;
							// 新しい拡大率
							float scale1 = parent.GetScaleXFromTrackBarValue(draft);
							// 古いstdx
							int stdx0 = parent.form.Model.StartToDrawX;
							int stdx1 = (int)(clock_at_mouse * (scale1 - scale0) + stdx0);
							// 新しいhScroll.Value
							int hscroll_value = (int)(stdx1 / scale1);
							if (hscroll_value < parent.form.hScroll.Minimum) {
								hscroll_value = parent.form.hScroll.Minimum;
							}
							if (parent.form.hScroll.Maximum < hscroll_value) {
								hscroll_value = parent.form.hScroll.Maximum;
							}

							parent.form.Model.ScaleX = (scale1);
							parent.form.Model.StartToDrawX = (stdx1);
							parent.form.hScroll.Value = hscroll_value;
							parent.form.trackBar.Value = draft;
						}
					} else {
						parent.ZoomPianoRollHeight(e.Delta > 0 ? 1 : -1);
					}
				} else {
					// スクロール操作
					if (e.X <= EditorManager.keyWidth || parent.form.pictPianoRoll.Width < e.X) {
						horizontal = false;
					}
					if (horizontal) {
						parent.form.hScroll.Value = parent.form.computeScrollValueFromWheelDelta(e.Delta);
					} else {
						double new_val = (double)parent.form.vScroll.Value - e.Delta * 10;
						int min = parent.form.vScroll.Minimum;
						int max = parent.form.vScroll.Maximum - parent.form.vScroll.LargeChange;
						if (new_val > max) {
							parent.form.vScroll.Value = max;
						} else if (new_val < min) {
							parent.form.vScroll.Value = min;
						} else {
							parent.form.vScroll.Value = (int)new_val;
						}
					}
				}
				parent.form.refreshScreen();
			}

			public void RunPianoRollResize ()
			{
				if (parent.form.WindowState != FormWindowState.Minimized) {
					parent.form.updateScrollRangeVertical();
					parent.form.Model.StartToDrawY = (parent.form.calculateStartToDrawY(parent.form.vScroll.Value));
				}
			}

			public void RunPianoRollPreviewKeyDown(KeyEventArgs e)
			{
				parent.form.processSpecialShortcutKey(e, true);
			}

			public void RunPianoRollPreviewKeyDown2(KeyEventArgs e)
			{
				parent.form.processSpecialShortcutKey(e, true);
			}
			#endregion

			#if ENABLE_MOUSEHOVER
			void MouseHoverEventGenerator( Object arg ) {
				int note = (int)arg;
				if ( EditorManager.editorConfig.MouseHoverTime > 0 ) {
					Thread.Sleep( EditorManager.editorConfig.MouseHoverTime );
				}
				KeySoundPlayer.play( note );
			}
			#endif

		}
	}
}
