using System;
using cadencii.media;
using System.Collections.Generic;
using cadencii.javax.sound.midi;
using cadencii.vsq;
using Cadencii.Media.Windows;

namespace cadencii
{
	public partial class FormMainModel
	{
		/// <summary>
		/// refreshScreenを呼び出す時に使うデリゲート
		/// </summary>
		/// <param name="value"></param>
		delegate void DelegateRefreshScreen(bool value);

		#if ENABLE_MIDI
		public MidiInDevice     mMidiIn = null;
		#endif
		#if ENABLE_MIDI
		/// <summary>
		/// MIDI入力句デバイスを再読込みします
		/// </summary>
		public void reloadMidiIn()
		{
			if (mMidiIn != null) {
				mMidiIn.MidiReceived -= new MidiReceivedEventHandler(mMidiIn_MidiReceived);
				mMidiIn.close();
				mMidiIn = null;
			}
			int portNumber = EditorManager.editorConfig.MidiInPort.PortNumber;
			int portNumberMtc = EditorManager.editorConfig.MidiInPortMtc.PortNumber;
		#if DEBUG
			Logger.StdOut("FormMain#reloadMidiIn; portNumber=" + portNumber + "; portNumberMtc=" + portNumberMtc);
		#endif
			try {
				mMidiIn = new MidiInDevice(portNumber);
				mMidiIn.MidiReceived += new MidiReceivedEventHandler(mMidiIn_MidiReceived);
		#if ENABLE_MTC
		if ( portNumber == portNumberMtc ) {
		m_midi_in.setReceiveSystemCommonMessage( true );
		m_midi_in.setReceiveSystemRealtimeMessage( true );
		m_midi_in.MidiReceived += handleMtcMidiReceived;
		m_midi_in.Start();
		} else {
		m_midi_in.setReceiveSystemCommonMessage( false );
		m_midi_in.setReceiveSystemRealtimeMessage( false );
		}
		#else
				mMidiIn.setReceiveSystemCommonMessage(false);
				mMidiIn.setReceiveSystemRealtimeMessage(false);
		#endif
			} catch (Exception ex) {
				Logger.write(GetType () + ".reloadMidiIn; ex=" + ex + "\n");
				Logger.StdErr("FormMain#reloadMidiIn; ex=" + ex);
			}

		#if ENABLE_MTC
		if ( m_midi_in_mtc != null ) {
		m_midi_in_mtc.MidiReceived -= handleMtcMidiReceived;
		m_midi_in_mtc.Dispose();
		m_midi_in_mtc = null;
		}
		if ( portNumber != portNumberMtc ) {
		try {
		m_midi_in_mtc = new MidiInDevice( EditorManager.editorConfig.MidiInPortMtc.PortNumber );
		m_midi_in_mtc.MidiReceived += handleMtcMidiReceived;
		m_midi_in_mtc.setReceiveSystemCommonMessage( true );
		m_midi_in_mtc.setReceiveSystemRealtimeMessage( true );
		m_midi_in_mtc.Start();
		} catch ( Exception ex ) {
		Logger.write( typeof( FormMain ) + ".reloadMidiIn; ex=" + ex + "\n" );
		Logger.StdErr( "FormMain#reloadMidiIn; ex=" + ex );
		}
		}
		#endif
			updateMidiInStatus();
		}
		#endif

		#if ENABLE_MTC
		/// <summary>
		/// MTC用のMIDI-INデバイスからMIDIを受信します。
		/// </summary>
		/// <param name="now"></param>
		/// <param name="dataArray"></param>
		private void handleMtcMidiReceived( double now, byte[] dataArray ) {
		byte data = (byte)(dataArray[1] & 0x0f);
		byte type = (byte)((dataArray[1] >> 4) & 0x0f);
		if ( type == 0 ) {
		mtcFrameLsb = data;
		} else if ( type == 1 ) {
		mtcFrameMsb = data;
		} else if ( type == 2 ) {
		mtcSecLsb = data;
		} else if ( type == 3 ) {
		mtcSecMsb = data;
		} else if ( type == 4 ) {
		mtcMinLsb = data;
		} else if ( type == 5 ) {
		mtcMinMsb = data;
		} else if ( type == 6 ) {
		mtcHourLsb = data;
		} else if ( type == 7 ) {
		mtcHourMsb = (byte)(data & 1);
		int fpsType = (data & 6) >> 1;
		double fps = 30.0;
		if ( fpsType == 0 ) {
		fps = 24.0;
		} else if ( fpsType == 1 ) {
		fps = 25;
		} else if ( fpsType == 2 ) {
		fps = 30000.0 / 1001.0;
		} else if ( fpsType == 3 ) {
		fps = 30.0;
		}
		int hour = (mtcHourMsb << 4 | mtcHourLsb);
		int min = (mtcMinMsb << 4 | mtcMinLsb);
		int sec = (mtcSecMsb << 4 | mtcSecLsb);
		int frame = (mtcFrameMsb << 4 | mtcFrameLsb) + 2;
		double time = (hour * 60.0 + min) * 60.0 + sec + frame / fps;
		mtcLastReceived = now;
		#if DEBUG
		int clock = (int)MusicManager.getVsqFile().getClockFromSec( time );
		EditorManager.setCurrentClock( clock );
		#endif
		/*if ( !EditorManager.isPlaying() ) {
                    EditorManager.EditMode = EditMode.REALTIME_MTC );
                    EditorManager.setPlaying( true );
                    EventHandler handler = new EventHandler( EditorManager_PreviewStarted );
                    if ( handler != null ) {
                        this.Invoke( handler );
                        while ( VSTiProxy.getPlayTime() <= 0.0 ) {
                            System.Windows.Forms.Application.DoEvents();
                        }
                        EditorManager.setPlaying( true );
                    }
                }*/
		#if DEBUG
		Logger.StdOut( "FormMain#handleMtcMidiReceived; time=" + time );
		#endif
	}
}
		#endif

		#if ENABLE_MIDI
		public void updateMidiInStatus()
		{
			int midiport = EditorManager.editorConfig.MidiInPort.PortNumber;
			List<MidiDevice.Info> devices = new List<MidiDevice.Info>();
			foreach (MidiDevice.Info info in MidiSystem.getMidiDeviceInfo()) {
				MidiDevice device = null;
				try {
					device = MidiSystem.getMidiDevice(info);
				} catch (Exception ex) {
					device = null;
				}
				if (device == null) continue;
				int max = device.getMaxTransmitters();
				if (max > 0 || max == -1) {
					devices.Add(info);
				}
			}
			if (midiport < 0 || devices.Count <= 0) {
		form.stripLblMidiIn.Text = _("Disabled");
		form.stripLblMidiIn.Image = Resources.slash;
			} else {
				if (midiport >= devices.Count) {
					midiport = 0;
					EditorManager.editorConfig.MidiInPort.PortNumber = midiport;
				}
		form.stripLblMidiIn.Text = devices[midiport].getName();
		form.stripLblMidiIn.Image = Resources.piano;
			}
		}
		#endif
		#if ENABLE_MIDI
		public void mMidiIn_MidiReceived(Object sender, cadencii.javax.sound.midi.MidiMessage message)
		{
			byte[] data = message.getMessage();
		#if DEBUG
			Logger.StdOut("FormMain#mMidiIn_MidiReceived; data.Length=" + data.Length);
		#endif
			if (data.Length <= 2) {
				return;
			}
		#if DEBUG
			Logger.StdOut("FormMain#mMidiIn_MidiReceived; EditorManager.isPlaying()=" + EditorManager.isPlaying());
		#endif
			if (EditorManager.isPlaying()) {
				return;
			}
		#if DEBUG
			Logger.StdOut("FormMain#mMidiIn_MidiReceived; isStepSequencerEnabeld()=" + form.Model.IsStepSequencerEnabled);
		#endif
	if (false == form.Model.IsStepSequencerEnabled) {
				return;
			}
			int code = data[0] & 0xf0;
			if (code != 0x80 && code != 0x90) {
				return;
			}
			if (code == 0x90 && data[2] == 0x00) {
				code = 0x80;//ベロシティ0のNoteOnはNoteOff
			}

			int note = (0xff & data[1]);

			int clock = EditorManager.getCurrentClock();
			int unit = EditorManager.getPositionQuantizeClock();
			if (unit > 1) {
				clock = FormMainModel.Quantize(clock, unit);
			}

		#if DEBUG
			Logger.StdOut("FormMain#mMidiIn_Received; clock=" + clock + "; note=" + note);
		#endif
			if (code == 0x80) {
				/*if ( EditorManager.mAddingEvent != null ) {
                    int len = clock - EditorManager.mAddingEvent.Clock;
                    if ( len <= 0 ) {
                        len = unit;
                    }
                    EditorManager.mAddingEvent.ID.Length = len;
                    int selected = EditorManager.Selected;
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventAdd( selected,
                                                                                                   EditorManager.mAddingEvent ) );
                    EditorManager.register( MusicManager.getVsqFile().executeCommand( run ) );
                    if ( !isEdited() ) {
                        setEdited( true );
                    }
                    updateDrawObjectList();
                }*/
			} else if (code == 0x90) {
				if (EditorManager.mAddingEvent != null) {
					// mAddingEventがnullでない場合は打ち込みの試行中(未確定の音符がある)
					// であるので，ノートだけが変わるようにする
					clock = EditorManager.mAddingEvent.Clock;
				} else {
					EditorManager.mAddingEvent = new VsqEvent();
				}
				EditorManager.mAddingEvent.Clock = clock;
				if (EditorManager.mAddingEvent.ID == null) {
					EditorManager.mAddingEvent.ID = new VsqID();
				}
				EditorManager.mAddingEvent.ID.type = VsqIDType.Anote;
				EditorManager.mAddingEvent.ID.Dynamics = 64;
				EditorManager.mAddingEvent.ID.VibratoHandle = null;
				if (EditorManager.mAddingEvent.ID.LyricHandle == null) {
					EditorManager.mAddingEvent.ID.LyricHandle = new LyricHandle("a", "a");
				}
				EditorManager.mAddingEvent.ID.LyricHandle.L0.Phrase = "a";
				EditorManager.mAddingEvent.ID.LyricHandle.L0.setPhoneticSymbol("a");
				EditorManager.mAddingEvent.ID.Note = note;

				// 音符の長さを計算
				int length = QuantizeModeUtil.getQuantizeClock(
					EditorManager.editorConfig.getLengthQuantize(),
					EditorManager.editorConfig.isLengthQuantizeTriplet());

				// 音符の長さを設定
				EditorManager.editLengthOfVsqEvent(
					EditorManager.mAddingEvent,
					length,
					EditorManager.vibratoLengthEditingRule);

				// 現在位置は，音符の末尾になる
				EditorManager.setCurrentClock(clock + length);

				// 画面を再描画
				if (form.InvokeRequired) {
					DelegateRefreshScreen deleg = null;
					try {
						deleg = new DelegateRefreshScreen(form.refreshScreen);
					} catch (Exception ex4) {
						deleg = null;
					}
					if (deleg != null) {
						form.Invoke(deleg, true);
					}
				} else {
					form.refreshScreen(true);
				}
				// 鍵盤音を鳴らす
				KeySoundPlayer.play(note);
			}
		}
		#endif

			}
}

