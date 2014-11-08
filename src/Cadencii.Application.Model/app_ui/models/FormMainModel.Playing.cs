using System;
using System.ComponentModel;
using cadencii.media;
using cadencii.vsq;

namespace cadencii
{
	public partial class FormMainModel
	{
		/// <summary>
		/// 再生中にソングポジションが前進だけしてほしいので，逆行を防ぐために前回のソングポジションを覚えておく
		/// </summary>
		private int mLastClock = 0;
		public Cadencii.Gui.Timer timer { get; set; }

		public void InitializeTimer (IContainer components)
		{
			timer = ApplicationUIHost.Create<Cadencii.Gui.Timer> (components);
			timer.Tick += (o,e) => OnTimerTick ();
			int fps = 1000 / EditorManager.editorConfig.MaximumFrameRate;
			timer.Interval = (fps <= 0) ? 1 : fps;
		}

		public void SetTimerInterval (int i)
		{
			timer.Interval = i;
		}

		public void StartPreview()
		{
			#if DEBUG
			sout.println("FormMain#EditorManager_PreviewStarted");
			#endif
			EditorManager.mAddingEvent = null;
			int selected = EditorManager.Selected;
			VsqFileEx vsq = MusicManager.getVsqFile();
			RendererKind renderer = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
			int clock = EditorManager.getCurrentClock();
			mLastClock = clock;
			double now = PortUtil.getCurrentTime();
			EditorManager.mPreviewStartedTime = now;
			timer.Start();
			form.stripBtnPlay.ImageKey = "control_pause.png";
			form.stripBtnPlay.Text = _("Stop");
		}

		public void AbortPreview()
		{
			#if DEBUG
			sout.println("FormMain#EditorManager_PreviewAborted");
			#endif
			form.stripBtnPlay.ImageKey = "control.png";
			form.stripBtnPlay.Text = _("Play");
			timer.Stop();

			for (int i = 0; i < EditorManager.mDrawStartIndex.Length; i++) {
				EditorManager.mDrawStartIndex[i] = 0;
			}
			#if ENABLE_MIDI
			//MidiPlayer.stop();
			#endif // ENABLE_MIDI
		}

		public void FlipPlaying ()
		{
			if (EditorManager.isPlaying()) {
				double elapsed = PlaySound.getPosition();
				double threshold = EditorManager.mForbidFlipPlayingThresholdSeconds;
				if (threshold < 0) {
					threshold = 0.0;
				}
				if (elapsed > threshold) {
					timer.Stop();
					EditorManager.setPlaying(false, form);
				}
			} else {
				EditorManager.setPlaying(true, form);
			}
		}

		public void Stop ()
		{
			EditorManager.setPlaying(false, form);
			timer.Stop();
			form.focusPianoRoll();
		}

		void OnTimerTick()
		{
			if (EditorManager.isGeneratorRunning()) {
				MonitorWaveReceiver monitor = MonitorWaveReceiver.getInstance();
				double play_time = 0.0;
				if (monitor != null) {
					play_time = monitor.getPlayTime();
				}
				double now = play_time + EditorManager.mDirectPlayShift;
				int clock = (int)MusicManager.getVsqFile().getClockFromSec(now);
				if (mLastClock <= clock) {
					mLastClock = clock;
					EditorManager.setCurrentClock(clock);
					if (EditorManager.mAutoScroll) {
						form.ensureCursorVisible();
					}
				}
			} else {
				EditorManager.setPlaying(false, form);
				int ending_clock = EditorManager.getPreviewEndingClock();
				EditorManager.setCurrentClock(ending_clock);
				if (EditorManager.mAutoScroll) {
					form.ensureCursorVisible();
				}
				form.refreshScreen(true);
				if (EditorManager.IsPreviewRepeatMode) {
					int dest_clock = 0;
					VsqFileEx vsq = MusicManager.getVsqFile();
					if (vsq.config.StartMarkerEnabled) {
						dest_clock = vsq.config.StartMarker;
					}
					EditorManager.setCurrentClock(dest_clock);
					EditorManager.setPlaying(true, form);
				}
			}
			form.refreshScreen();
		}

		/// <summary>
		/// ソングポジションを1小節進めます
		/// </summary>
		public void Forward()
		{
			bool playing = EditorManager.isPlaying();
			if (playing) {
				return;
			}
			VsqFileEx vsq = MusicManager.getVsqFile();
			if (vsq == null) {
				return;
			}
			int cl_clock = EditorManager.getCurrentClock();
			int unit = QuantizeModeUtil.getQuantizeClock(
				EditorManager.editorConfig.getPositionQuantize(),
				EditorManager.editorConfig.isPositionQuantizeTriplet());
			int cl_new = FormMainModel.Quantize(cl_clock + unit, unit);

			if (cl_new <= form.hScroll.Maximum + (form.pictPianoRoll.Width - EditorManager.keyWidth) * form.controller.getScaleXInv()) {
				// 表示の更新など
				EditorManager.setCurrentClock(cl_new);

				// ステップ入力時の処理
				UpdateNoteLengthStepSequencer();

				form.ensureCursorVisible();
				EditorManager.setPlaying(playing, form);
				form.refreshScreen();
			}
		}

		/// <summary>
		/// ソングポジションを1小節戻します
		/// </summary>
		public void Rewind()
		{
			bool playing = EditorManager.isPlaying();
			if (playing) {
				return;
			}
			VsqFileEx vsq = MusicManager.getVsqFile();
			if (vsq == null) {
				return;
			}
			int cl_clock = EditorManager.getCurrentClock();
			int unit = QuantizeModeUtil.getQuantizeClock(
				EditorManager.editorConfig.getPositionQuantize(),
				EditorManager.editorConfig.isPositionQuantizeTriplet());
			int cl_new = FormMainModel.Quantize(cl_clock - unit, unit);
			if (cl_new < 0) {
				cl_new = 0;
			}

			EditorManager.setCurrentClock(cl_new);

			// ステップ入力時の処理
			UpdateNoteLengthStepSequencer();

			form.ensureCursorVisible();
			EditorManager.setPlaying(playing, form);
			form.refreshScreen();
		}

		/// <summary>
		/// MIDIステップ入力中に，ソングポジションが動いたときの処理を行います
		/// EditorManager.mAddingEventが非nullの時，音符の先頭は決まっているので，
		/// ソングポジションと，音符の先頭との距離から音符の長さを算出し，更新する
		/// EditorManager.mAddingEventがnullの時は何もしない
		/// </summary>
		void UpdateNoteLengthStepSequencer()
		{
			if (!form.controller.isStepSequencerEnabled()) {
				return;
			}

			VsqEvent item = EditorManager.mAddingEvent;
			if (item == null) {
				return;
			}

			int song_position = EditorManager.getCurrentClock();
			int start = item.Clock;
			int length = song_position - start;
			if (length < 0) length = 0;
			EditorManager.editLengthOfVsqEvent(
				item,
				length,
				EditorManager.vibratoLengthEditingRule);
		}

	}
}

