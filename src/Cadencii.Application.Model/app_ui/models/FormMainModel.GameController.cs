using System;
using Cadencii.Gui;
using Cadencii.Media.Vsq;
using Cadencii.Utilities;

namespace cadencii
{
	public partial class FormMainModel
	{
		enum GameControlMode
		{
			DISABLED,
			NORMAL,
			KEYBOARD,
			CURSOR,
		}

		GameControlMode mGameMode = GameControlMode.DISABLED;
		GameControllerManager game_controller_manager = ApplicationUIHost.Create<GameControllerManager> ();
		Cadencii.Gui.Timer mTimer;
		/// <summary>
		/// 前回ゲームコントローラのイベントを処理した時刻
		/// </summary>
		double mLastEventProcessed;
		bool mLastPovR = false;
		bool mLastPovL = false;
		bool mLastPovU = false;
		bool mLastPovD = false;
		bool mLastBtnX = false;
		bool mLastBtnO = false;
		bool mLastBtnRe = false;
		bool mLastBtnTr = false;
		bool mLastBtnSelect = false;

		/// <summary>
		/// 現在のゲームコントローラのモードに応じてstripLblGameCtrlModeの表示状態を更新します。
		/// </summary>
		public void UpdateGameControlerStatus()
		{
			if (mGameMode == GameControlMode.DISABLED) {
				form.stripLblGameCtrlMode.Text = _("Disabled");
				form.stripLblGameCtrlMode.Image = Resources.slash;
			} else if (mGameMode == GameControlMode.CURSOR) {
				form.stripLblGameCtrlMode.Text = _("Cursor");
				form.stripLblGameCtrlMode.Image = null;
			} else if (mGameMode == GameControlMode.KEYBOARD) {
				form.stripLblGameCtrlMode.Text = _("Keyboard");
				form.stripLblGameCtrlMode.Image = Resources.piano;
			} else if (mGameMode == GameControlMode.NORMAL) {
				form.stripLblGameCtrlMode.Text = _("Normal");
				form.stripLblGameCtrlMode.Image = null;
			}
		}

		/// <summary>
		/// 識別済みのゲームコントローラを取り外します
		/// </summary>
		public void RemoveGameControler()
		{
			if (mTimer != null) {
				mTimer.Stop();
				mTimer.Dispose();
				mTimer = null;
			}
			mGameMode = GameControlMode.DISABLED;
			UpdateGameControlerStatus();
		}

		/// <summary>
		/// PCに接続されているゲームコントローラを識別・接続します
		/// </summary>
		public void LoadGameController()
		{
			try {
				bool init_success = false;
				int num_joydev = game_controller_manager.InitializeJoyPad ();
				if (num_joydev <= 0) {
					init_success = false;
				} else {
					init_success = true;
				}
				if (init_success) {
					mGameMode = GameControlMode.NORMAL;
					form.stripLblGameCtrlMode.Image = null;
					form.stripLblGameCtrlMode.Text = mGameMode.ToString();
					mTimer = ApplicationUIHost.Create<Timer>();
					mTimer.Interval = 10;
					mTimer.Tick += (o, e) => TimerTick ();
					mTimer.Start();
				} else {
					mGameMode = GameControlMode.DISABLED;
				}
			} catch (Exception ex) {
				Logger.write(GetType () + ".loadGameControler; ex=" + ex + "\n");
				mGameMode = GameControlMode.DISABLED;
				#if DEBUG
				CDebug.WriteLine("FormMain+ReloadGameControler");
				CDebug.WriteLine("    ex=" + ex);
				#endif
			}
			UpdateGameControlerStatus();
		}

		#region mTimer
		void TimerTick ()
		{
			if (!form.mFormActivated) {
				return;
			}
			try {
				double now = PortUtil.getCurrentTime();

				var ret = game_controller_manager.GetJoyPadStatus ();
				var buttons = ret.Buttons;
				var pov0 = ret.Pov0;
				bool event_processed = false;
				double dt_ms = (now - mLastEventProcessed) * 1000.0;

				AppConfig m = EditorManager.editorConfig;
				bool btn_x = (0 <= m.GameControlerCross && m.GameControlerCross < buttons.Length && buttons[m.GameControlerCross] > 0x00);
				bool btn_o = (0 <= m.GameControlerCircle && m.GameControlerCircle < buttons.Length && buttons[m.GameControlerCircle] > 0x00);
				bool btn_tr = (0 <= m.GameControlerTriangle && m.GameControlerTriangle < buttons.Length && buttons[m.GameControlerTriangle] > 0x00);
				bool btn_re = (0 <= m.GameControlerRectangle && m.GameControlerRectangle < buttons.Length && buttons[m.GameControlerRectangle] > 0x00);
				bool pov_r = pov0 == m.GameControlPovRight;
				bool pov_l = pov0 == m.GameControlPovLeft;
				bool pov_u = pov0 == m.GameControlPovUp;
				bool pov_d = pov0 == m.GameControlPovDown;
				bool L1 = (0 <= m.GameControlL1 && m.GameControlL1 < buttons.Length && buttons[m.GameControlL1] > 0x00);
				bool R1 = (0 <= m.GameControlL2 && m.GameControlL2 < buttons.Length && buttons[m.GameControlR1] > 0x00);
				bool L2 = (0 <= m.GameControlR1 && m.GameControlR1 < buttons.Length && buttons[m.GameControlL2] > 0x00);
				bool R2 = (0 <= m.GameControlR2 && m.GameControlR2 < buttons.Length && buttons[m.GameControlR2] > 0x00);
				bool SELECT = (0 <= m.GameControlSelect && m.GameControlSelect <= buttons.Length && buttons[m.GameControlSelect] > 0x00);
				if (mGameMode == GameControlMode.NORMAL) {
					mLastBtnX = btn_x;

					if (!event_processed && !btn_o && mLastBtnO) {
						if (EditorManager.isPlaying()) {
							timer.Stop();
						}
						EditorManager.setPlaying(!EditorManager.isPlaying(), form);
						mLastEventProcessed = now;
						event_processed = true;
					}
					mLastBtnO = btn_o;

					if (!event_processed && pov_r && dt_ms > EditorManager.editorConfig.GameControlerMinimumEventInterval) {
						Forward();
						mLastEventProcessed = now;
						event_processed = true;
					}
					mLastPovR = pov_r;

					if (!event_processed && pov_l && dt_ms > EditorManager.editorConfig.GameControlerMinimumEventInterval) {
						Rewind();
						mLastEventProcessed = now;
						event_processed = true;
					}
					mLastPovL = pov_l;

					if (!event_processed && pov_u && dt_ms > EditorManager.editorConfig.GameControlerMinimumEventInterval) {
						int draft_vscroll = form.vScroll.Value - (int)(100 * form.Model.ScaleY) * 3;
						if (draft_vscroll < form.vScroll.Minimum) {
							draft_vscroll = form.vScroll.Minimum;
						}
						form.vScroll.Value = draft_vscroll;
						form.refreshScreen();
						mLastEventProcessed = now;
						event_processed = true;
					}

					if (!event_processed && pov_d && dt_ms > EditorManager.editorConfig.GameControlerMinimumEventInterval) {
						int draft_vscroll = form.vScroll.Value + (int)(100 * form.Model.ScaleY) * 3;
						if (draft_vscroll > form.vScroll.Maximum) {
							draft_vscroll = form.vScroll.Maximum;
						}
						form.vScroll.Value = draft_vscroll;
						form.refreshScreen();
						mLastEventProcessed = now;
						event_processed = true;
					}

					if (!event_processed && !SELECT && mLastBtnSelect) {
						event_processed = true;
						mGameMode = GameControlMode.KEYBOARD;
						form.stripLblGameCtrlMode.Text = mGameMode.ToString();
						form.stripLblGameCtrlMode.Image = Resources.piano;
					}
					mLastBtnSelect = SELECT;
				} else if (mGameMode == GameControlMode.KEYBOARD) {
					if (!event_processed && !SELECT && mLastBtnSelect) {
						event_processed = true;
						mGameMode = GameControlMode.NORMAL;
						UpdateGameControlerStatus();
						mLastBtnSelect = SELECT;
						return;
					}
					mLastBtnSelect = SELECT;

					int note = -1;
					if (pov_r && !mLastPovR) {
						note = 60;
					} else if (btn_re && !mLastBtnRe) {
						note = 62;
					} else if (btn_tr && !mLastBtnTr) {
						note = 64;
					} else if (btn_o && !mLastBtnO) {
						note = 65;
					} else if (btn_x && !mLastBtnX) {
						note = 67;
					} else if (pov_u && !mLastPovU) {
						note = 59;
					} else if (pov_l && !mLastPovL) {
						note = 57;
					} else if (pov_d && !mLastPovD) {
						note = 55;
					}
					if (note >= 0) {
						if (L1) {
							note += 12;
						} else if (L2) {
							note -= 12;
						}
						if (R1) {
							note += 1;
						} else if (R2) {
							note -= 1;
						}
					}
					mLastBtnO = btn_o;
					mLastBtnX = btn_x;
					mLastBtnRe = btn_re;
					mLastBtnTr = btn_tr;
					mLastPovL = pov_l;
					mLastPovD = pov_d;
					mLastPovR = pov_r;
					mLastPovU = pov_u;
					if (note >= 0) {
						#if DEBUG
						CDebug.WriteLine(GetType () + ".TimerTick");
						CDebug.WriteLine("    note=" + note);
						#endif
						if (EditorManager.isPlaying()) {
							int clock = EditorManager.getCurrentClock();
							int selected = EditorManager.Selected;
							if (EditorManager.mAddingEvent != null) {
								EditorManager.mAddingEvent.ID.setLength(clock - EditorManager.mAddingEvent.Clock);
								CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventAdd(selected,
									EditorManager.mAddingEvent));
								EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
								if (!form.isEdited()) {
									form.setEdited(true);
								}
								form.updateDrawObjectList();
							}
							EditorManager.mAddingEvent = new VsqEvent(clock, new VsqID(0));
							EditorManager.mAddingEvent.ID.type = VsqIDType.Anote;
							EditorManager.mAddingEvent.ID.Dynamics = 64;
							EditorManager.mAddingEvent.ID.VibratoHandle = null;
							EditorManager.mAddingEvent.ID.LyricHandle = new LyricHandle("a", "a");
							EditorManager.mAddingEvent.ID.Note = note;
						}
						KeySoundPlayer.play(note);
					} else {
						if (EditorManager.isPlaying() && EditorManager.mAddingEvent != null) {
							EditorManager.mAddingEvent.ID.setLength(EditorManager.getCurrentClock() - EditorManager.mAddingEvent.Clock);
						}
					}
				}
			} catch (Exception ex) {
				Logger.write(GetType () + ".TimerTickk; ex=" + ex + "\n");
				#if DEBUG
				CDebug.WriteLine("    ex=" + ex);
				#endif
				mGameMode = GameControlMode.DISABLED;
				UpdateGameControlerStatus();
				mTimer.Stop();
			}
		}
		#endregion

	}
}

