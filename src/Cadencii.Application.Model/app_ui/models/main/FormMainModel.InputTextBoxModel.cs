using System;
using cadencii.apputil;
using System.IO;
using System.Linq;
using Cadencii.Gui;
using Cadencii.Media.Vsq;
using cadencii.core;
using Cadencii.Media;
using Cadencii.Gui.Toolkit;
using cadencii;
using Cadencii.Application.Media;

namespace Cadencii.Application.Models
{
	public partial class FormMainModel
	{
		public class InputTextBoxModel
		{
			readonly FormMainModel parent;

			public InputTextBoxModel (FormMainModel parent)
			{
				this.parent = parent;
			}
			//BOOKMARK: inputTextBox
			#region EditorManager.InputTextBox
			public void mInputTextBox_KeyDown(Object sender, KeyEventArgs e)
			{
				#if DEBUG
				Logger.StdOut("FormMain#mInputTextBox_KeyDown");
				#endif
				bool shift = ((Keys) e.Modifiers & Keys.Shift) == Keys.Shift;
				bool tab = (Keys) e.KeyCode == Keys.Tab;
				bool enter = (Keys) e.KeyCode == Keys.Return;
				if (tab || enter) {
					parent.executeLyricChangeCommand();
					int selected = EditorManager.Selected;
					int index = -1;
					int width = parent.form.pictPianoRoll.Width;
					int height = parent.form.pictPianoRoll.Height;
					int key_width = EditorManager.keyWidth;
					VsqTrack track = MusicManager.getVsqFile().Track[selected];
					track.sortEvent();
					if (tab) {
						int clock = 0;
						int search_index = EditorManager.itemSelection.getLastEvent().original.InternalID;
						int c = track.getEventCount();
						for (int i = 0; i < c; i++) {
							VsqEvent item = track.getEvent(i);
							if (item.InternalID == search_index) {
								index = i;
								clock = item.Clock;
								break;
							}
						}
						if (shift) {
							// 1個前の音符イベントを検索
							int tindex = -1;
							for (int i = track.getEventCount() - 1; i >= 0; i--) {
								VsqEvent ve = track.getEvent(i);
								if (ve.ID.type == VsqIDType.Anote && i != index && ve.Clock <= clock) {
									tindex = i;
									break;
								}
							}
							index = tindex;
						} else {
							// 1個後の音符イベントを検索
							int tindex = -1;
							int c2 = track.getEventCount();
							for (int i = 0; i < c2; i++) {
								VsqEvent ve = track.getEvent(i);
								if (ve.ID.type == VsqIDType.Anote && i != index && ve.Clock >= clock) {
									tindex = i;
									break;
								}
							}
							index = tindex;
						}
					}
					if (0 <= index && index < track.getEventCount()) {
						EditorManager.itemSelection.clearEvent();
						VsqEvent item = track.getEvent(index);
						EditorManager.itemSelection.addEvent(item.InternalID);
						int x = EditorManager.xCoordFromClocks(item.Clock);
						int y = EditorManager.yCoordFromNote(item.ID.Note);
						bool phonetic_symbol_edit_mode = EditorManager.InputTextBox.isPhoneticSymbolEditMode();
						parent.showInputTextBox(
							item.ID.LyricHandle.L0.Phrase,
							item.ID.LyricHandle.L0.getPhoneticSymbol(),
							new Point(x, y),
							phonetic_symbol_edit_mode);
						int clWidth = (int)(EditorManager.InputTextBox.Width * parent.ScaleXInv);

						// 画面上にEditorManager.InputTextBoxが見えるように，移動
						int SPACE = 20;
						// vScrollやhScrollをいじった場合はfalseにする．
						bool refresh_screen = true;
						// X軸方向について，見えるように移動
						if (x < key_width || width < x + EditorManager.InputTextBox.Width) {
							int clock, clock_x;
							if (x < key_width) {
								// 左に隠れてしまう場合
								clock = item.Clock;
							} else {
								// 右に隠れてしまう場合
								clock = item.Clock + clWidth;
							}
							if (shift) {
								// 左方向に移動していた場合
								// 右から３分の１の位置に移動させる
								clock_x = width - (width - key_width) / 3;
							} else {
								// 右方向に移動していた場合
								clock_x = key_width + (width - key_width) / 3;
							}
							double draft_d = (key_width + EditorManager.keyOffset - clock_x) * parent.ScaleXInv + clock;
							if (draft_d < 0.0) {
								draft_d = 0.0;
							}
							int draft = (int)draft_d;
							if (draft < parent.form.hScroll.Minimum) {
								draft = parent.form.hScroll.Minimum;
							} else if (parent.form.hScroll.Maximum < draft) {
								draft = parent.form.hScroll.Maximum;
							}
							refresh_screen = false;
							parent.form.hScroll.Value = draft;
						}
						// y軸方向について，見えるように移動
						int track_height = (int)(100 * parent.ScaleY);
						if (y <= 0 || height - track_height <= y) {
							int note = item.ID.Note;
							if (y <= 0) {
								// 上にはみ出してしまう場合
								note = item.ID.Note + 1;
							} else {
								// 下にはみ出してしまう場合
								note = item.ID.Note - 2;
							}
							if (127 < note) {
								note = 127;
							}
							if (note < 0) {
								note = 0;
							}
							parent.EnsureNoteVisibleOnPianoRoll(note);
						}
						if (refresh_screen) {
							parent.form.refreshScreen();
						}
					} else {
						int id = EditorManager.itemSelection.getLastEvent().original.InternalID;
						EditorManager.itemSelection.clearEvent();
						EditorManager.itemSelection.addEvent(id);
						parent.hideInputTextBox();
					}
				}
			}

			public void mInputTextBox_KeyUp(Object sender, KeyEventArgs e)
			{
				#if DEBUG
				Logger.StdOut("FormMain#mInputTextBox_KeyUp");
				#endif
				bool flip = ((Keys) e.KeyCode == Keys.Up || (Keys) e.KeyCode == Keys.Down) && ((Keys) e.Modifiers == Keys.Alt);
				bool hide = (Keys) e.KeyCode == Keys.Escape;

				if (flip) {
					if (EditorManager.InputTextBox.Visible) {
						parent.FlipInputTextBoxMode();
					}
				} else if (hide) {
					parent.hideInputTextBox();
				}
			}

			public void mInputTextBox_ImeModeChanged(Object sender, EventArgs e)
			{
				parent.mLastIsImeModeOn = EditorManager.InputTextBox.ImeMode == ImeMode.Hiragana;
			}

			public void mInputTextBox_KeyPress(Object sender, KeyPressEventArgs e)
			{
				#if DEBUG
				Logger.StdOut("FormMain#mInputTextBox_KeyPress");
				#endif
				//           Enter                                  Tab
				e.Handled = (e.KeyChar == Convert.ToChar(13)) || (e.KeyChar == Convert.ToChar(09));
			}
			#endregion
		}
	}
}
