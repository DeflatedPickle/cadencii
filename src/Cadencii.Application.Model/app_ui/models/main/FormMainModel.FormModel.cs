using System;
using cadencii.apputil;
using System.IO;
using System.Linq;
using Cadencii.Gui;
using Cadencii.Media.Vsq;
using cadencii.core;
using Cadencii.Media;
using Cadencii.Utilities;
using Cadencii.Media.Windows;
using Cadencii.Gui.Toolkit;
using cadencii;
using Cadencii.Application.Media;
using Cadencii.Application.Controls;
using Cadencii.Application.Forms;

namespace Cadencii.Application.Models
{
	public partial class FormMainModel
	{
		public class FormModel
		{
			readonly FormMainModel parent;

			public FormModel (FormMainModel parent)
			{
				this.parent = parent;
			}

			/// <summary>
			/// アイコンパレットのドラッグ＆ドロップ処理中，一度でもpictPianoRoll内にマウスが入ったかどうか
			/// </summary>
			private bool mIconPaletteOnceDragEntered = false;

			private FormWindowState mWindowState = FormWindowState.Normal;

			//BOOKMARK: FormMain

			#region FormMain

			public void DragLeave ()
			{
				EditorManager.EditMode = EditMode.NONE;
				mIconPaletteOnceDragEntered = false;
			}

			/// <summary>
			/// アイテムがドラッグされている最中の処理を行います
			/// </summary>
			void handleDragOver (int screen_x, int screen_y)
			{
				if (EditorManager.EditMode != EditMode.DRAG_DROP) {
					return;
				}
				var pt = parent.form.pictPianoRoll.PointToScreen (Cadencii.Gui.Point.Empty);
				if (!mIconPaletteOnceDragEntered) {
					int keywidth = EditorManager.keyWidth;
					Rectangle rc = new Rectangle (pt.X + keywidth, pt.Y, parent.form.pictPianoRoll.Width - keywidth, parent.form.pictPianoRoll.Height);
					if (Utility.isInRect (new Point (screen_x, screen_y), rc)) {
						mIconPaletteOnceDragEntered = true;
					} else {
						return;
					}
				}
				var e0 = new MouseEventArgs (MouseButtons.Left,
					        1,
					        screen_x - pt.X,
					        screen_y - pt.Y,
					        0);
				parent.PianoRoll.RunPianoRollMouseMove (e0);
			}

			public void DragOver (DragEventArgs e)
			{
				handleDragOver (e.X, e.Y);
			}

			/// <summary>
			/// ピアノロールにドロップされたIconDynamicsHandleの受け入れ処理を行います
			/// </summary>
			void handleDragDrop (IconDynamicsHandle handle, int screen_x, int screen_y)
			{
				if (handle == null) {
					return;
				}
				var locPianoroll = parent.form.pictPianoRoll.PointToScreen (Cadencii.Gui.Point.Empty);
				// ドロップ位置を特定して，アイテムを追加する
				int x = screen_x - locPianoroll.X;
				int y = screen_y - locPianoroll.Y;
				int clock1 = EditorManager.clockFromXCoord (x);

				// クオンタイズの処理
				int unit = EditorManager.getPositionQuantizeClock ();
				int clock = FormMainModel.Quantize (clock1, unit);

				int note = EditorManager.noteFromYCoord (y);
				VsqFileEx vsq = MusicManager.getVsqFile ();
				int clockAtPremeasure = vsq.getPreMeasureClocks ();
				if (clock < clockAtPremeasure) {
					return;
				}
				if (note < 0 || 128 < note) {
					return;
				}

				int selected = EditorManager.Selected;
				VsqTrack vsq_track = vsq.Track [selected];
				VsqTrack work = (VsqTrack)vsq_track.clone ();

				if (EditorManager.mAddingEvent == null) {
					// ここは多分起こらない
					return;
				}
				VsqEvent item = (VsqEvent)EditorManager.mAddingEvent.clone ();
				item.Clock = clock;
				item.ID.Note = note;
				work.addEvent (item);
				work.reflectDynamics ();
				CadenciiCommand run = VsqFileEx.generateCommandTrackReplace (selected, work, vsq.AttachedCurves.get (selected - 1));
				EditorManager.editHistory.register (vsq.executeCommand (run));
				parent.form.setEdited (true);
				EditorManager.EditMode = EditMode.NONE;
				parent.form.refreshScreen ();
			}

			public void DragDrop (DragEventArgs e)
			{
				EditorManager.EditMode = EditMode.NONE;
				mIconPaletteOnceDragEntered = false;
				parent.form.mMouseDowned = false;
				if (!e.Data.GetDataPresent (typeof(IconDynamicsHandle))) {
					return;
				}
				var locPianoroll = parent.form.pictPianoRoll.PointToScreen (Cadencii.Gui.Point.Empty);
				int keywidth = EditorManager.keyWidth;
				Rectangle rcPianoroll = new Rectangle (locPianoroll.X + keywidth,
					                       locPianoroll.Y,
					                       parent.form.pictPianoRoll.Width - keywidth,
					                       parent.form.pictPianoRoll.Height);
				if (!Utility.isInRect (new Point (e.X, e.Y), rcPianoroll)) {
					return;
				}

				// dynaff, crescend, decrescend のどれがドロップされたのか検査
				IconDynamicsHandle this_is_it = (IconDynamicsHandle)e.Data.GetData (typeof(IconDynamicsHandle));
				if (this_is_it == null) {
					return;
				}

				handleDragDrop (this_is_it, e.X, e.Y);
			}

			public void DragEnter (DragEventArgs e)
			{
				if (e.Data.GetDataPresent (typeof(IconDynamicsHandle))) {
					// ドロップ可能
					e.Effect = DragDropEffects.All;
					EditorManager.EditMode = EditMode.DRAG_DROP;
					parent.form.mMouseDowned = true;
				} else {
					e.Effect = DragDropEffects.None;
					EditorManager.EditMode = EditMode.NONE;
				}
			}

			public void FormMain_FormClosed ()
			{
				#if DEBUG
				Logger.StdOut ("FormMain#FormMain_FormClosed");
				#endif
				parent.ClearTempWave ();
				string tempdir = Path.Combine (ApplicationGlobal.getCadenciiTempDir (), ApplicationGlobal.getID ());
				if (!Directory.Exists (tempdir)) {
					PortUtil.createDirectory (tempdir);
				}
				string log = Path.Combine (tempdir, "run.log");
				Cadencii.Media.Windows.MMDebug.close ();
				try {
					if (System.IO.File.Exists (log)) {
						PortUtil.deleteFile (log);
					}
					PortUtil.deleteDirectory (tempdir, true);
				} catch (Exception ex) {
					Logger.write (GetType () + ".FormMain_FormClosed; ex=" + ex + "\n");
					Logger.StdErr ("FormMain#FormMain_FormClosed; ex=" + ex);
				}
				EditorManager.stopGenerator ();
				VSTiDllManager.terminate ();
				#if ENABLE_MIDI
				//MidiPlayer.stop();
				if (parent.mMidiIn != null) {
					parent.mMidiIn.close ();
				}
				#endif
				#if ENABLE_MTC
			if ( m_midi_in_mtc != null ) {
			m_midi_in_mtc.Close();
			}
				#endif
				PlaySound.kill ();
				PluginLoader.cleanupUnusedAssemblyCache ();
			}

			public void RunFormClosing (FormClosingEventArgs e)
			{
				// 設定値を格納
				if (ApplicationGlobal.appConfig.ViewWaveform) {
					EditorManager.editorConfig.SplitContainer2LastDividerLocation = parent.form.splitContainer2.DividerLocation;
				}
				if (EditorManager.editorConfig.PropertyWindowStatus.State == PanelState.Docked) {
					EditorManager.editorConfig.PropertyWindowStatus.DockWidth = parent.form.splitContainerProperty.DividerLocation;
				}
				/*
			// I believe this should not be special case.
			if (e.CloseReason == CloseReason.WindowsShutDown) {
				return;
			}
			*/
				bool cancel = handleFormClosing ();
				e.Cancel = cancel;
			}

			/// <summary>
			/// ウィンドウが閉じようとしているときの処理を行う
			/// 戻り値がtrueの場合，ウィンドウが閉じるのをキャンセルする処理が必要
			/// </summary>
			/// <returns></returns>
			bool handleFormClosing ()
			{
				if (parent.form.isEdited ()) {
					string file = MusicManager.getFileName ();
					if (file.Equals ("")) {
						file = "Untitled";
					} else {
						file = PortUtil.getFileName (file);
					}
					var ret = DialogManager.ShowMessageBox (_ ("Save this sequence?"),
						         _ ("Affirmation"),
						MessageBoxButtons.YesNoCancel,
						MessageBoxIcon.Question);
					if (ret == Cadencii.Gui.DialogResult.Yes) {
						if (MusicManager.getFileName ().Equals ("")) {
							var dr = DialogManager.ShowModalFileDialog (parent.form.saveXmlVsqDialog, false, parent.form);
							if (dr == Cadencii.Gui.DialogResult.OK) {
								EditorManager.saveTo (parent.form.saveXmlVsqDialog.FileName);
							} else {
								return true;
							}
						} else {
							EditorManager.saveTo (MusicManager.getFileName ());
						}

					} else if (ret == Cadencii.Gui.DialogResult.Cancel) {
						return true;
					}
				}
				EditorManager.editorConfig.WindowMaximized = (parent.form.WindowState == FormWindowState.Maximized);
				EditorManager.saveConfig ();
				UtauWaveGenerator.clearCache ();
				VConnectWaveGenerator.clearCache ();

				#if ENABLE_MIDI
				if (parent.mMidiIn != null) {
					parent.mMidiIn.close ();
				}
				#endif
				parent.form.bgWorkScreen.Dispose ();
				return false;
			}

			public void RunLocationChanged ()
			{
				if (parent.form.WindowState == FormWindowState.Normal) {
					var bounds = parent.form.Bounds;
					EditorManager.editorConfig.WindowRect = new Rectangle (bounds.X, bounds.Y, bounds.Width, bounds.Height);
				}
			}

			void CustomizeRebar ()
			{
				// ツールバーの位置を復帰させる
				// toolStipの位置を，前回終了時の位置に戻す
				int chevron_width = EditorManager.editorConfig.ChevronWidth;

				/*
				parent.form.bandFile = ApplicationUIHost.Create<RebarBand> ();
				parent.form.bandPosition = ApplicationUIHost.Create<RebarBand> ();
				parent.form.bandMeasure = ApplicationUIHost.Create<RebarBand> ();
				parent.form.bandTool = ApplicationUIHost.Create<RebarBand> ();
				*/

				var bandFile = parent.form.bandFile;
				var bandPosition = parent.form.bandPosition;
				var bandMeasure = parent.form.bandMeasure;
				var bandTool = parent.form.bandTool;

				bool variant_height = false;
				bandFile.VariantHeight = variant_height;
				bandPosition.VariantHeight = variant_height;
				bandMeasure.VariantHeight = variant_height;
				bandTool.VariantHeight = variant_height;

				int MAX_BAND_HEIGHT = 26;// toolBarTool.Height;

				var toolBarFile = (UiToolBar) bandFile.Child;
				var toolBarPosition = (UiToolBar) bandPosition.Child;
				var toolBarMeasure = (UiToolBar) bandMeasure.Child;
				var toolBarTool = (UiToolBar) bandTool.Child;
				/*
				var toolBarFile = parent.form.toolBarFile;
				var toolBarPosition = parent.form.toolBarPosition;
				var toolBarMeasure = parent.form.toolBarMeasure;
				var toolBarTool = parent.form.toolBarTool;
				parent.form.rebar.AddControl (toolBarFile);
				parent.form.rebar.AddControl (toolBarTool);
				parent.form.rebar.AddControl (toolBarPosition);
				parent.form.rebar.AddControl (toolBarMeasure);
				*/
				// bandFile
				if (toolBarFile.Buttons.Count > 0) {
					bandFile.IdealWidth =
					toolBarFile.Buttons [toolBarFile.Buttons.Count - 1].Rectangle.Right + chevron_width;
				}
				bandFile.BandSize = EditorManager.editorConfig.BandSizeFile;
				bandFile.NewRow = EditorManager.editorConfig.BandNewRowFile;
				// bandPosition
				if (toolBarPosition.Buttons.Count > 0) {
					bandPosition.IdealWidth =
					toolBarPosition.Buttons [toolBarPosition.Buttons.Count - 1].Rectangle.Right + chevron_width;
				}
				bandPosition.BandSize = EditorManager.editorConfig.BandSizePosition;
				bandPosition.NewRow = EditorManager.editorConfig.BandNewRowPosition;
				// bandMeasure
				if (toolBarMeasure.Buttons.Count > 0) {
					bandMeasure.IdealWidth =
					toolBarMeasure.Buttons [toolBarMeasure.Buttons.Count - 1].Rectangle.Right + chevron_width;
				}
				bandMeasure.BandSize = EditorManager.editorConfig.BandSizeMeasure;
				bandMeasure.NewRow = EditorManager.editorConfig.BandNewRowMeasure;
				// bandTool
				if (toolBarTool.Buttons.Count > 0) {
					bandTool.IdealWidth =
					toolBarTool.Buttons [toolBarTool.Buttons.Count - 1].Rectangle.Right + chevron_width;
				}
				bandTool.BandSize = EditorManager.editorConfig.BandSizeTool;
				bandTool.NewRow = EditorManager.editorConfig.BandNewRowTool;

				// it's possible to support this later, but now I want cleaner XMLization.
				#if false
				// 一度リストに入れてから追加する
				var bands = new RebarBand[] { null, null, null, null };
				// 番号がおかしくないかチェック
				if (EditorManager.editorConfig.BandOrderFile < 0 || bands.Length <= EditorManager.editorConfig.BandOrderFile)
					EditorManager.editorConfig.BandOrderFile = 0;
				if (EditorManager.editorConfig.BandOrderMeasure < 0 || bands.Length <= EditorManager.editorConfig.BandOrderMeasure)
					EditorManager.editorConfig.BandOrderMeasure = 0;
				if (EditorManager.editorConfig.BandOrderPosition < 0 || bands.Length <= EditorManager.editorConfig.BandOrderPosition)
					EditorManager.editorConfig.BandOrderPosition = 0;
				if (EditorManager.editorConfig.BandOrderTool < 0 || bands.Length <= EditorManager.editorConfig.BandOrderTool)
					EditorManager.editorConfig.BandOrderTool = 0;
				bands [EditorManager.editorConfig.BandOrderFile] = bandFile;
				bands [EditorManager.editorConfig.BandOrderMeasure] = bandMeasure;
				bands [EditorManager.editorConfig.BandOrderPosition] = bandPosition;
				bands [EditorManager.editorConfig.BandOrderTool] = bandTool;
				// nullチェック
				bool null_exists = false;
				for (var i = 0; i < bands.Length; i++) {
					if (bands [i] == null) {
						null_exists = true;
						break;
					}
				}
				if (null_exists) {
					// 番号に矛盾があれば，デフォルトの並び方で
					bands [0] = bandFile;
					bands [1] = bandMeasure;
					bands [2] = bandPosition;
					bands [3] = bandTool;
					bandFile.NewRow = true;
					bandMeasure.NewRow = true;
					bandPosition.NewRow = true;
					bandTool.NewRow = true;
				}

				// 追加
				for (var i = 0; i < bands.Length; i++) {
					if (i == 0)
						bands [i].NewRow = true;
					bands [i].MinHeight = 24;
					parent.form.rebar.Bands.Add (bands [i]);
				}
				#endif

				#if DEBUG
				Logger.StdOut (GetType () + ".ctor; this.Width=" + parent.form.Width);
				#endif
			}

			public void RunLoad ()
			{
				parent.form.applyLanguage ();

				CustomizeRebar ();

				parent.updateSplitContainer2Size (false);

				parent.EnsureNoteVisibleOnPianoRoll (60);

				// 鍵盤用の音源の準備．Javaはこの機能は削除で．
				// 鍵盤用のキャッシュが古い位置に保存されている場合。
				string cache_new = ApplicationGlobal.getKeySoundPath ();
				string cache_old = Path.Combine (PortUtil.getApplicationStartupPath (), "cache");
				if (Directory.Exists (cache_old)) {
					bool exists = false;
					for (int i = 0; i < 127; i++) {
						string s = Path.Combine (cache_new, i + ".wav");
						if (System.IO.File.Exists (s)) {
							exists = true;
							break;
						}
					}

					// 新しいキャッシュが1つも無い場合に、古いディレクトリからコピーする
					if (!exists) {
						for (int i = 0; i < 127; i++) {
							string wav_from = Path.Combine (cache_old, i + ".wav");
							string wav_to = Path.Combine (cache_new, i + ".wav");
							if (System.IO.File.Exists (wav_from)) {
								try {
									PortUtil.copyFile (wav_from, wav_to);
									PortUtil.deleteFile (wav_from);
								} catch (Exception ex) {
									Logger.write (GetType () + ".FormMain_Load; ex=" + ex + "\n");
									Logger.StdErr ("FormMain#FormMain_Load; ex=" + ex);
								}
							}
						}
					}
				}

				// 足りてないキャッシュがひとつでもあればFormGenerateKeySound発動する
				bool cache_is_incomplete = false;
				for (int i = 0; i < 127; i++) {
					string wav = Path.Combine (cache_new, i + ".wav");
					if (!System.IO.File.Exists (wav)) {
						cache_is_incomplete = true;
						break;
					}
				}

				bool init_key_sound_player_immediately = true; //FormGenerateKeySoundの終了を待たずにKeySoundPlayer.initするかどうか。
				if (!ApplicationGlobal.appConfig.DoNotAskKeySoundGeneration && cache_is_incomplete) {
					FormAskKeySoundGenerationController dialog = null;
					bool always_check_this = !ApplicationGlobal.appConfig.DoNotAskKeySoundGeneration;
					Cadencii.Gui.DialogResult dialog_result = Cadencii.Gui.DialogResult.None;
					try {
						dialog = new FormAskKeySoundGenerationController ();
						dialog.setupUi (ApplicationUIHost.Create<FormAskKeySoundGenerationUi> (dialog));
						dialog.getUi ().setAlwaysPerformThisCheck (always_check_this);
						dialog_result = DialogManager.ShowModalDialog (dialog.getUi (), parent.form);
						always_check_this = dialog.getUi ().isAlwaysPerformThisCheck ();
					} catch (Exception ex) {
						Logger.write (GetType () + ".FormMain_Load; ex=" + ex + "\n");
						Logger.StdErr ("FormMain#FormMain_Load; ex=" + ex);
					} finally {
						if (dialog != null) {
							try {
								dialog.getUi ().close (true);
							} catch (Exception ex2) {
								Logger.write (GetType () + ".FormMain_Load; ex=" + ex2 + "\n");
								Logger.StdErr ("FormMain#FormMain_Load; ex2=" + ex2);
							}
						}
					}
					ApplicationGlobal.appConfig.DoNotAskKeySoundGeneration = !always_check_this;

					if (dialog_result == Cadencii.Gui.DialogResult.OK) {
						FormGenerateKeySound form = null;
						try {
							form = ApplicationUIHost.Create<FormGenerateKeySound> (true);
							form.FormClosed += (o, e) => FormGenerateKeySound_FormClosed ();
							form.ShowDialog ();
						} catch (Exception ex) {
							Logger.write (GetType () + ".FormMain_Load; ex=" + ex + "\n");
							Logger.StdErr ("FormMain#FormMain_Load; ex=" + ex);
						}
						init_key_sound_player_immediately = false;
					}
				}

				if (init_key_sound_player_immediately) {
					try {
						KeySoundPlayer.init ();
					} catch (Exception ex) {
						Logger.write (GetType () + ".FormMain_Load; ex=" + ex + "\n");
						Logger.StdErr ("FormMain#FormMain_Load; ex=" + ex);
					}
				}

				if (!ApplicationGlobal.appConfig.DoNotAutomaticallyCheckForUpdates) {
					parent.form.showUpdateInformationAsync (false);
				}
			}

			void FormGenerateKeySound_FormClosed ()
			{
				try {
					KeySoundPlayer.init ();
				} catch (Exception ex) {
					Logger.write (GetType () + ".FormGenerateKeySound_FormClosed; ex=" + ex + "\n");
					Logger.StdErr ("FormMain#FormGenerateKeySound_FormClosed; ex=" + ex);
				}
			}

			public void RunSizeChanged ()
			{
				if (mWindowState == (FormWindowState)parent.form.WindowState) {
					return;
				}
				var state = (FormWindowState)parent.form.WindowState;
				if (state == FormWindowState.Normal || state == FormWindowState.Maximized) {
					if (state == FormWindowState.Normal) {
						var bounds = parent.form.Bounds;
						EditorManager.editorConfig.WindowRect = new Rectangle (bounds.X, bounds.Y, bounds.Width, bounds.Height);
					}
					#if ENABLE_PROPERTY
					// プロパティウィンドウの状態を更新
					if (EditorManager.editorConfig.PropertyWindowStatus.State == PanelState.Window) {
						if (EditorManager.propertyWindow.getUi ().isWindowMinimized ()) {
							EditorManager.propertyWindow.getUi ().deiconfyWindow ();
						}
						if (!EditorManager.propertyWindow.getUi ().isVisible ()) {
							EditorManager.propertyWindow.getUi ().setVisible (true);
						}
					}
					#endif
					// ミキサーウィンドウの状態を更新
					bool vm = EditorManager.editorConfig.MixerVisible;
					if (vm != EditorManager.MixerWindow.Visible) {
						EditorManager.MixerWindow.Visible = vm;
					}

					// アイコンパレットの状態を更新
					if (EditorManager.iconPalette != null && parent.form.menuVisualIconPalette.Checked) {
						if (!EditorManager.iconPalette.Visible) {
							EditorManager.iconPalette.Visible = true;
						}
					}
					parent.form.updateLayout ();
					parent.form.Focus ();
				} else if (state == FormWindowState.Minimized) {
					#if ENABLE_PROPERTY
					EditorManager.propertyWindow.getUi ().setVisible (false);
					#endif
					EditorManager.MixerWindow.Visible = false;
					if (EditorManager.iconPalette != null) {
						EditorManager.iconPalette.Visible = false;
					}
				}/* else if ( state == BForm.MAXIMIZED_BOTH ) {
#if ENABLE_PROPERTY
                EditorManager.propertyWindow.setExtendedState( BForm.NORMAL );
                EditorManager.propertyWindow.setVisible( EditorManager.editorConfig.PropertyWindowStatus.State == PanelState.Window );
#endif
                EditorManager.MixerWindow.setVisible( EditorManager.editorConfig.MixerVisible );
                if ( EditorManager.iconPalette != null && menuVisualIconPalette.isSelected() ) {
                    EditorManager.iconPalette.setVisible( true );
                }
                this.requestFocus();
            }*/
			}

			public void RunMouseWheel (MouseEventArgs e)
			{
				#if DEBUG
				Logger.StdOut ("FormMain#FormMain_MouseWheel");
				#endif
				if ((AwtHost.ModifierKeys & Keys.Shift) == Keys.Shift) {
					parent.form.hScroll.Value = parent.form.computeScrollValueFromWheelDelta (e.Delta);
				} else {
					var vScroll = parent.form.vScroll;
					int max = vScroll.Maximum - vScroll.LargeChange;
					int min = vScroll.Minimum;
					double new_val = (double)vScroll.Value - e.Delta;
					if (new_val > max) {
						vScroll.Value = max;
					} else if (new_val < min) {
						vScroll.Value = min;
					} else {
						vScroll.Value = (int)new_val;
					}
				}
				parent.form.refreshScreen ();
			}

			public void RunPreviewKeyDown (KeyEventArgs e)
			{
				#if DEBUG
				Logger.StdOut ("FormMain#FormMain_PreviewKeyDown");
				#endif
				parent.form.processSpecialShortcutKey (e, true);
			}

			public void RunDeactivate ()
			{
				parent.form.mFormActivated = false;
			}

			public void RunActivated ()
			{
				parent.form.mFormActivated = true;
			}

			#endregion
		}
	}
}

