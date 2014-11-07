using System;
using cadencii.core;
using System.Collections.Generic;
using System.IO;


using cadencii.vsq;
using cadencii.vsq.io;
using System.Text;
using System.Linq;
using cadencii.java.util;
using Cadencii.Gui;
using cadencii.apputil;

namespace cadencii
{
	public partial class FormMainModel
	{
		public class SettingsMenuModel
		{
			readonly FormMainModel parent;

			public SettingsMenuModel (FormMainModel parent)
			{
				this.parent = parent;
			}

			public void RunSettingDefaultSingerStyleCommand ()
			{
				FormSingerStyleConfig dlg = null;
				try {
					dlg = ApplicationUIHost.Create<FormSingerStyleConfig> ();
					dlg.PMBendDepth = (ApplicationGlobal.appConfig.DefaultPMBendDepth);
					dlg.PMBendLength = (ApplicationGlobal.appConfig.DefaultPMBendLength);
					dlg.PMbPortamentoUse = (ApplicationGlobal.appConfig.DefaultPMbPortamentoUse);
					dlg.DEMdecGainRate = (ApplicationGlobal.appConfig.DefaultDEMdecGainRate);
					dlg.DEMaccent = (ApplicationGlobal.appConfig.DefaultDEMaccent);

					int selected = EditorManager.Selected;
					dlg.Location = parent.GetFormPreferedLocation (dlg.Width, dlg.Height);
					var dr = DialogManager.showModalDialog (dlg, parent.form);
					if (dr == 1) {
						ApplicationGlobal.appConfig.DefaultPMBendDepth = dlg.PMBendDepth;
						ApplicationGlobal.appConfig.DefaultPMBendLength = dlg.PMBendLength;
						ApplicationGlobal.appConfig.DefaultPMbPortamentoUse = dlg.PMbPortamentoUse;
						ApplicationGlobal.appConfig.DefaultDEMdecGainRate = dlg.DEMdecGainRate;
						ApplicationGlobal.appConfig.DefaultDEMaccent = dlg.DEMaccent;
						if (dlg.ApplyCurrentTrack) {
							VsqFileEx vsq = MusicManager.getVsqFile ();
							VsqTrack vsq_track = vsq.Track [selected];
							VsqTrack copy = (VsqTrack)vsq_track.clone ();
							bool changed = false;
							for (int i = 0; i < copy.getEventCount (); i++) {
								if (copy.getEvent (i).ID.type == VsqIDType.Anote) {
									EditorManager.editorConfig.applyDefaultSingerStyle (copy.getEvent (i).ID);
									changed = true;
								}
							}
							if (changed) {
								CadenciiCommand run =
									VsqFileEx.generateCommandTrackReplace (
										selected,
										copy,
										vsq.AttachedCurves.get (selected - 1));
								EditorManager.editHistory.register (vsq.executeCommand (run));
								parent.form.updateDrawObjectList ();
								parent.form.refreshScreen ();
							}
						}
					}
				} catch (Exception ex) {
					Logger.write (GetType () + ".menuSettingDefaultSingerStyle_Click; ex=" + ex + "\n");
				} finally {
					if (dlg != null) {
						try {
							dlg.Close ();
						} catch (Exception ex2) {
							Logger.write (GetType () + ".menuSettingDefaultSingerStyle_Click; ex=" + ex2 + "\n");
						}
					}
				}
			}

			public void RunSettingGameControlerSettingCommand ()
			{
				FormGameControllerConfig dlg = null;
				try {
					dlg = ApplicationUIHost.Create<FormGameControllerConfig> ();
					dlg.Location = parent.GetFormPreferedLocation (dlg.Width, dlg.Height);
					var dr = DialogManager.showModalDialog (dlg, parent.form);
					if (dr == 1) {
						EditorManager.editorConfig.GameControlerRectangle = dlg.getRectangle ();
						EditorManager.editorConfig.GameControlerTriangle = dlg.getTriangle ();
						EditorManager.editorConfig.GameControlerCircle = dlg.getCircle ();
						EditorManager.editorConfig.GameControlerCross = dlg.getCross ();
						EditorManager.editorConfig.GameControlL1 = dlg.getL1 ();
						EditorManager.editorConfig.GameControlL2 = dlg.getL2 ();
						EditorManager.editorConfig.GameControlR1 = dlg.getR1 ();
						EditorManager.editorConfig.GameControlR2 = dlg.getR2 ();
						EditorManager.editorConfig.GameControlSelect = dlg.getSelect ();
						EditorManager.editorConfig.GameControlStart = dlg.getStart ();
						EditorManager.editorConfig.GameControlPovDown = dlg.getPovDown ();
						EditorManager.editorConfig.GameControlPovLeft = dlg.getPovLeft ();
						EditorManager.editorConfig.GameControlPovUp = dlg.getPovUp ();
						EditorManager.editorConfig.GameControlPovRight = dlg.getPovRight ();
					}
				} catch (Exception ex) {
					Logger.write (GetType () + ".menuSettingGrameControlerSetting_Click; ex=" + ex + "\n");
				} finally {
					if (dlg != null) {
						try {
							dlg.Close ();
						} catch (Exception ex2) {
							Logger.write (GetType () + ".menuSettingGameControlerSetting_Click; ex=" + ex2 + "\n");
						}
					}
				}
			}

			public void RunSettingPreferenceCommand ()
			{
				try {
					if (parent.form.mDialogPreference == null) {
						parent.form.mDialogPreference = ApplicationUIHost.Create<Preference> ();
					}
					var dlg = parent.form.mDialogPreference;
					dlg.setBaseFont (new Font (EditorManager.editorConfig.BaseFontName, Cadencii.Gui.Font.PLAIN, cadencii.core.EditorConfig.FONT_SIZE9));
					dlg.setScreenFont (new Font (EditorManager.editorConfig.ScreenFontName, Cadencii.Gui.Font.PLAIN, cadencii.core.EditorConfig.FONT_SIZE9));
					dlg.setWheelOrder (EditorManager.editorConfig.WheelOrder);
					dlg.setCursorFixed (EditorManager.editorConfig.CursorFixed);
					dlg.setDefaultVibratoLength (ApplicationGlobal.appConfig.DefaultVibratoLength);
					dlg.setAutoVibratoThresholdLength (EditorManager.editorConfig.AutoVibratoThresholdLength);
					dlg.setAutoVibratoType1 (EditorManager.editorConfig.AutoVibratoType1);
					dlg.setAutoVibratoType2 (EditorManager.editorConfig.AutoVibratoType2);
					dlg.setAutoVibratoTypeCustom (EditorManager.editorConfig.AutoVibratoTypeCustom);
					dlg.setEnableAutoVibrato (EditorManager.editorConfig.EnableAutoVibrato);
					dlg.setPreSendTime (ApplicationGlobal.appConfig.PreSendTime);
					dlg.setControlCurveResolution (ApplicationGlobal.appConfig.ControlCurveResolution);
					dlg.setDefaultSingerName (ApplicationGlobal.appConfig.DefaultSingerName);
					dlg.setScrollHorizontalOnWheel (EditorManager.editorConfig.ScrollHorizontalOnWheel);
					dlg.setMaximumFrameRate (EditorManager.editorConfig.MaximumFrameRate);
					dlg.setKeepLyricInputMode (EditorManager.editorConfig.KeepLyricInputMode);
					dlg.setPxTrackHeight (EditorManager.editorConfig.PxTrackHeight);
					dlg.setMouseHoverTime (EditorManager.editorConfig.getMouseHoverTime ());
					dlg.setPlayPreviewWhenRightClick (EditorManager.editorConfig.PlayPreviewWhenRightClick);
					dlg.setCurveSelectingQuantized (EditorManager.editorConfig.CurveSelectingQuantized);
					dlg.setCurveVisibleAccent (ApplicationGlobal.appConfig.CurveVisibleAccent);
					dlg.setCurveVisibleBre (ApplicationGlobal.appConfig.CurveVisibleBreathiness);
					dlg.setCurveVisibleBri (ApplicationGlobal.appConfig.CurveVisibleBrightness);
					dlg.setCurveVisibleCle (ApplicationGlobal.appConfig.CurveVisibleClearness);
					dlg.setCurveVisibleDecay (ApplicationGlobal.appConfig.CurveVisibleDecay);
					dlg.setCurveVisibleDyn (ApplicationGlobal.appConfig.CurveVisibleDynamics);
					dlg.setCurveVisibleGen (ApplicationGlobal.appConfig.CurveVisibleGendorfactor);
					dlg.setCurveVisibleOpe (ApplicationGlobal.appConfig.CurveVisibleOpening);
					dlg.setCurveVisiblePit (ApplicationGlobal.appConfig.CurveVisiblePit);
					dlg.setCurveVisiblePbs (ApplicationGlobal.appConfig.CurveVisiblePbs);
					dlg.setCurveVisiblePor (ApplicationGlobal.appConfig.CurveVisiblePortamento);
					dlg.setCurveVisibleVel (ApplicationGlobal.appConfig.CurveVisibleVelocity);
					dlg.setCurveVisibleVibratoDepth (ApplicationGlobal.appConfig.CurveVisibleVibratoDepth);
					dlg.setCurveVisibleVibratoRate (ApplicationGlobal.appConfig.CurveVisibleVibratoRate);
					dlg.setCurveVisibleFx2Depth (ApplicationGlobal.appConfig.CurveVisibleFx2Depth);
					dlg.setCurveVisibleHarmonics (ApplicationGlobal.appConfig.CurveVisibleHarmonics);
					dlg.setCurveVisibleReso1 (ApplicationGlobal.appConfig.CurveVisibleReso1);
					dlg.setCurveVisibleReso2 (ApplicationGlobal.appConfig.CurveVisibleReso2);
					dlg.setCurveVisibleReso3 (ApplicationGlobal.appConfig.CurveVisibleReso3);
					dlg.setCurveVisibleReso4 (ApplicationGlobal.appConfig.CurveVisibleReso4);
					dlg.setCurveVisibleEnvelope (ApplicationGlobal.appConfig.CurveVisibleEnvelope);
					#if ENABLE_MIDI
					dlg.setMidiInPort (EditorManager.editorConfig.MidiInPort.PortNumber);
					#endif
					#if ENABLE_MTC
					m_preference_dlg.setMtcMidiInPort( EditorManager.editorConfig.MidiInPortMtc.PortNumber );

					#endif
					List<string> resamplers = new List<string> ();
					int size = ApplicationGlobal.appConfig.getResamplerCount ();
					for (int i = 0; i < size; i++) {
						resamplers.Add (ApplicationGlobal.appConfig.getResamplerAt (i));
					}
					dlg.setResamplersConfig (resamplers);
					dlg.setPathWavtool (ApplicationGlobal.appConfig.PathWavtool);
					dlg.setUtausingers (ApplicationGlobal.appConfig.UtauSingers);
					dlg.setSelfDeRomantization (EditorManager.editorConfig.SelfDeRomanization);
					dlg.setAutoBackupIntervalMinutes (EditorManager.editorConfig.AutoBackupIntervalMinutes);
					dlg.setUseSpaceKeyAsMiddleButtonModifier (EditorManager.editorConfig.UseSpaceKeyAsMiddleButtonModifier);
					dlg.setPathAquesTone (ApplicationGlobal.appConfig.PathAquesTone);
					dlg.setPathAquesTone2 (ApplicationGlobal.appConfig.PathAquesTone2);
					dlg.setUseProjectCache (ApplicationGlobal.appConfig.UseProjectCache);
					dlg.setAquesToneRequired (!ApplicationGlobal.appConfig.DoNotUseAquesTone);
					dlg.setAquesTone2Requried (!ApplicationGlobal.appConfig.DoNotUseAquesTone2);
					dlg.setVocaloid1Required (!ApplicationGlobal.appConfig.DoNotUseVocaloid1);
					dlg.setVocaloid2Required (!ApplicationGlobal.appConfig.DoNotUseVocaloid2);
					dlg.setBufferSize (ApplicationGlobal.appConfig.BufferSizeMilliSeconds);
					dlg.setDefaultSynthesizer (ApplicationGlobal.appConfig.DefaultSynthesizer);
					dlg.setUseUserDefinedAutoVibratoType (ApplicationGlobal.appConfig.UseUserDefinedAutoVibratoType);
					dlg.setEnableWideCharacterWorkaround (ApplicationGlobal.appConfig.UseWideCharacterWorkaround);

					dlg.Location = parent.GetFormPreferedLocation (dlg.Width, dlg.Height);

					var dr = DialogManager.showModalDialog (dlg, parent.form);
					if (dr == 1) {
						string old_base_font_name = EditorManager.editorConfig.BaseFontName;
						float old_base_font_size = EditorManager.editorConfig.BaseFontSize;
						Font new_base_font = dlg.getBaseFont ();
						if (!old_base_font_name.Equals (new_base_font.getName ()) ||
						    old_base_font_size != new_base_font.getSize2D ()) {
							EditorManager.editorConfig.BaseFontName = dlg.getBaseFont ().getName ();
							EditorManager.editorConfig.BaseFontSize = dlg.getBaseFont ().getSize2D ();
							parent.form.updateMenuFonts ();
						}

						EditorManager.editorConfig.ScreenFontName = dlg.getScreenFont ().getName ();
						EditorManager.editorConfig.WheelOrder = dlg.getWheelOrder ();
						EditorManager.editorConfig.CursorFixed = dlg.isCursorFixed ();

						ApplicationGlobal.appConfig.DefaultVibratoLength = dlg.getDefaultVibratoLength ();
						EditorManager.editorConfig.AutoVibratoThresholdLength = dlg.getAutoVibratoThresholdLength ();
						EditorManager.editorConfig.AutoVibratoType1 = dlg.getAutoVibratoType1 ();
						EditorManager.editorConfig.AutoVibratoType2 = dlg.getAutoVibratoType2 ();
						EditorManager.editorConfig.AutoVibratoTypeCustom = dlg.getAutoVibratoTypeCustom ();

						EditorManager.editorConfig.EnableAutoVibrato = dlg.isEnableAutoVibrato ();
						ApplicationGlobal.appConfig.PreSendTime = dlg.getPreSendTime ();
						ApplicationGlobal.appConfig.Language = dlg.getLanguage ();
						if (!Messaging.getLanguage ().Equals (ApplicationGlobal.appConfig.Language)) {
							Messaging.setLanguage (ApplicationGlobal.appConfig.Language);
							parent.form.applyLanguage ();
							dlg.applyLanguage ();
							EditorManager.MixerWindow.applyLanguage ();
							if (parent.form.mVersionInfo != null && !parent.form.mVersionInfo.IsDisposed) {
								parent.form.mVersionInfo.applyLanguage ();
							}
							#if ENABLE_PROPERTY
							EditorManager.propertyWindow.applyLanguage ();
							EditorManager.propertyPanel.updateValue (EditorManager.Selected);
							#endif
							if (parent.form.mDialogMidiImportAndExport != null) {
								parent.form.mDialogMidiImportAndExport.applyLanguage ();
							}
						}

						ApplicationGlobal.appConfig.ControlCurveResolution = dlg.getControlCurveResolution ();
						ApplicationGlobal.appConfig.DefaultSingerName = dlg.getDefaultSingerName ();
						EditorManager.editorConfig.ScrollHorizontalOnWheel = dlg.isScrollHorizontalOnWheel ();
						EditorManager.editorConfig.MaximumFrameRate = dlg.getMaximumFrameRate ();
						int fps = 1000 / EditorManager.editorConfig.MaximumFrameRate;
						parent.form.timer.Interval = (fps <= 0) ? 1 : fps;
						parent.form.applyShortcut ();
						EditorManager.editorConfig.KeepLyricInputMode = dlg.isKeepLyricInputMode ();
						if (EditorManager.editorConfig.PxTrackHeight != dlg.getPxTrackHeight ()) {
							EditorManager.editorConfig.PxTrackHeight = dlg.getPxTrackHeight ();
							parent.form.updateDrawObjectList ();
						}
						EditorManager.editorConfig.setMouseHoverTime (dlg.getMouseHoverTime ());
						EditorManager.editorConfig.PlayPreviewWhenRightClick = dlg.isPlayPreviewWhenRightClick ();
						EditorManager.editorConfig.CurveSelectingQuantized = dlg.isCurveSelectingQuantized ();

						ApplicationGlobal.appConfig.CurveVisibleAccent = dlg.isCurveVisibleAccent ();
						ApplicationGlobal.appConfig.CurveVisibleBreathiness = dlg.isCurveVisibleBre ();
						ApplicationGlobal.appConfig.CurveVisibleBrightness = dlg.isCurveVisibleBri ();
						ApplicationGlobal.appConfig.CurveVisibleClearness = dlg.isCurveVisibleCle ();
						ApplicationGlobal.appConfig.CurveVisibleDecay = dlg.isCurveVisibleDecay ();
						ApplicationGlobal.appConfig.CurveVisibleDynamics = dlg.isCurveVisibleDyn ();
						ApplicationGlobal.appConfig.CurveVisibleGendorfactor = dlg.isCurveVisibleGen ();
						ApplicationGlobal.appConfig.CurveVisibleOpening = dlg.isCurveVisibleOpe ();
						ApplicationGlobal.appConfig.CurveVisiblePit = dlg.isCurveVisiblePit ();
						ApplicationGlobal.appConfig.CurveVisiblePbs = dlg.isCurveVisiblePbs ();
						ApplicationGlobal.appConfig.CurveVisiblePortamento = dlg.isCurveVisiblePor ();
						ApplicationGlobal.appConfig.CurveVisibleVelocity = dlg.isCurveVisibleVel ();
						ApplicationGlobal.appConfig.CurveVisibleVibratoDepth = dlg.isCurveVisibleVibratoDepth ();
						ApplicationGlobal.appConfig.CurveVisibleVibratoRate = dlg.isCurveVisibleVibratoRate ();
						ApplicationGlobal.appConfig.CurveVisibleFx2Depth = dlg.isCurveVisibleFx2Depth ();
						ApplicationGlobal.appConfig.CurveVisibleHarmonics = dlg.isCurveVisibleHarmonics ();
						ApplicationGlobal.appConfig.CurveVisibleReso1 = dlg.isCurveVisibleReso1 ();
						ApplicationGlobal.appConfig.CurveVisibleReso2 = dlg.isCurveVisibleReso2 ();
						ApplicationGlobal.appConfig.CurveVisibleReso3 = dlg.isCurveVisibleReso3 ();
						ApplicationGlobal.appConfig.CurveVisibleReso4 = dlg.isCurveVisibleReso4 ();
						ApplicationGlobal.appConfig.CurveVisibleEnvelope = dlg.isCurveVisibleEnvelope ();

						#if ENABLE_MIDI
						EditorManager.editorConfig.MidiInPort.PortNumber = dlg.getMidiInPort ();
						#endif
						#if ENABLE_MTC
						EditorManager.editorConfig.MidiInPortMtc.PortNumber = m_preference_dlg.getMtcMidiInPort();
						#endif
						#if ENABLE_MIDI || ENABLE_MTC
						parent.form.updateMidiInStatus ();
						parent.form.reloadMidiIn ();
						#endif

						List<string> new_resamplers = new List<string> ();
						dlg.copyResamplersConfig (new_resamplers);
						ApplicationGlobal.appConfig.clearResampler ();
						for (int i = 0; i < new_resamplers.Count; i++) {
							ApplicationGlobal.appConfig.addResampler (new_resamplers [i]);
						}
						ApplicationGlobal.appConfig.PathWavtool = dlg.getPathWavtool ();

						ApplicationGlobal.appConfig.UtauSingers.Clear ();
						foreach (var sc in dlg.getUtausingers()) {
							ApplicationGlobal.appConfig.UtauSingers.Add ((SingerConfig)sc.clone ());
						}
						EditorManager.reloadUtauVoiceDB ();

						EditorManager.editorConfig.SelfDeRomanization = dlg.isSelfDeRomantization ();
						EditorManager.editorConfig.AutoBackupIntervalMinutes = dlg.getAutoBackupIntervalMinutes ();
						EditorManager.editorConfig.UseSpaceKeyAsMiddleButtonModifier = dlg.isUseSpaceKeyAsMiddleButtonModifier ();

						#if ENABLE_AQUESTONE
						var old_aquestone_config = Tuple.Create (ApplicationGlobal.appConfig.PathAquesTone, ApplicationGlobal.appConfig.DoNotUseAquesTone);
						ApplicationGlobal.appConfig.PathAquesTone = dlg.getPathAquesTone ();
						ApplicationGlobal.appConfig.DoNotUseAquesTone = !dlg.isAquesToneRequired ();
						if (old_aquestone_config.Item1 != ApplicationGlobal.appConfig.PathAquesTone
						    || old_aquestone_config.Item2 != ApplicationGlobal.appConfig.DoNotUseAquesTone) {
							VSTiDllManager.reloadAquesTone ();
						}

						var old_aquestone2_config = Tuple.Create (ApplicationGlobal.appConfig.PathAquesTone2, ApplicationGlobal.appConfig.DoNotUseAquesTone2);
						ApplicationGlobal.appConfig.PathAquesTone2 = dlg.getPathAquesTone2 ();
						ApplicationGlobal.appConfig.DoNotUseAquesTone2 = !dlg.isAquesTone2Required ();
						if (old_aquestone2_config.Item1 != ApplicationGlobal.appConfig.PathAquesTone2
						    || old_aquestone2_config.Item2 != ApplicationGlobal.appConfig.DoNotUseAquesTone2) {
							VSTiDllManager.reloadAquesTone2 ();
						}
						#endif
						parent.form.updateRendererMenu ();

						//EditorManager.editorConfig.__revoked__WaveFileOutputFromMasterTrack = dlg.isWaveFileOutputFromMasterTrack();
						//EditorManager.editorConfig.__revoked__WaveFileOutputChannel = dlg.getWaveFileOutputChannel();
						if (ApplicationGlobal.appConfig.UseProjectCache && !dlg.isUseProjectCache ()) {
							// プロジェクト用キャッシュを使用していたが，使用しないように変更された場合.
							// プロジェクト用キャッシュが存在するなら，共用のキャッシュに移動する．
							string file = MusicManager.getFileName ();
							if (file != null && !file.Equals ("")) {
								string dir = PortUtil.getDirectoryName (file);
								string name = PortUtil.getFileNameWithoutExtension (file);
								string projectCacheDir = Path.Combine (dir, name + ".cadencii");
								string commonCacheDir = Path.Combine (ApplicationGlobal.getCadenciiTempDir (), ApplicationGlobal.getID ());
								if (Directory.Exists (projectCacheDir)) {
									VsqFileEx vsq = MusicManager.getVsqFile ();
									for (int i = 1; i < vsq.Track.Count; i++) {
										// wavを移動
										string wavFrom = Path.Combine (projectCacheDir, i + ".wav");
										string wavTo = Path.Combine (commonCacheDir, i + ".wav");
										if (!System.IO.File.Exists (wavFrom)) {
											continue;
										}
										if (System.IO.File.Exists (wavTo)) {
											try {
												PortUtil.deleteFile (wavTo);
											} catch (Exception ex) {
												Logger.write (GetType () + ".menuSettingPreference_Click; ex=" + ex + "\n");
												serr.println ("FormMain#menuSettingPreference_Click; ex=" + ex);
												continue;
											}
										}
										try {
											PortUtil.moveFile (wavFrom, wavTo);
										} catch (Exception ex) {
											Logger.write (GetType () + ".menuSettingPreference_Click; ex=" + ex + "\n");
											serr.println ("FormMain#menuSettingPreference_Click; ex=" + ex);
										}

										// xmlを移動
										string xmlFrom = Path.Combine (projectCacheDir, i + ".xml");
										string xmlTo = Path.Combine (commonCacheDir, i + ".xml");
										if (!System.IO.File.Exists (xmlFrom)) {
											continue;
										}
										if (System.IO.File.Exists (xmlTo)) {
											try {
												PortUtil.deleteFile (xmlTo);
											} catch (Exception ex) {
												Logger.write (GetType () + ".menuSettingPreference_Click; ex=" + ex + "\n");
												serr.println ("FormMain#menuSettingPreference_Click; ex=" + ex);
												continue;
											}
										}
										try {
											PortUtil.moveFile (xmlFrom, xmlTo);
										} catch (Exception ex) {
											Logger.write (GetType () + ".menuSettingPreference_Click; ex=" + ex + "\n");
											serr.println ("FormMain#menuSettingPreference_Click; ex=" + ex);
										}
									}

									// projectCacheDirが空なら，ディレクトリごと削除する
									string[] files = PortUtil.listFiles (projectCacheDir, "*.*");
									if (files.Length <= 0) {
										try {
											PortUtil.deleteDirectory (projectCacheDir);
										} catch (Exception ex) {
											Logger.write (GetType () + ".menuSettingPreference_Click; ex=" + ex + "\n");
											serr.println ("FormMain#menuSettingPreference_Click; ex=" + ex);
										}
									}

									// キャッシュのディレクトリを再指定
									ApplicationGlobal.setTempWaveDir (commonCacheDir);
								}
							}
						}
						ApplicationGlobal.appConfig.UseProjectCache = dlg.isUseProjectCache ();
						ApplicationGlobal.appConfig.DoNotUseVocaloid1 = !dlg.isVocaloid1Required ();
						ApplicationGlobal.appConfig.DoNotUseVocaloid2 = !dlg.isVocaloid2Required ();
						ApplicationGlobal.appConfig.BufferSizeMilliSeconds = dlg.getBufferSize ();
						ApplicationGlobal.appConfig.DefaultSynthesizer = dlg.getDefaultSynthesizer ();
						ApplicationGlobal.appConfig.UseUserDefinedAutoVibratoType = dlg.isUseUserDefinedAutoVibratoType ();
						ApplicationGlobal.appConfig.UseWideCharacterWorkaround = dlg.isEnableWideCharacterWorkaround ();

						parent.form.TrackSelector.prepareSingerMenu (VsqFileEx.getTrackRendererKind (MusicManager.getVsqFile ().Track [EditorManager.Selected]));
						parent.form.TrackSelector.updateVisibleCurves ();

						parent.form.updateRendererMenu ();
						EditorManager.updateAutoBackupTimerStatus ();

						// editorConfig.PxTrackHeightが変更されている可能性があるので，更新が必要
						parent.form.controller.setStartToDrawY (parent.form.calculateStartToDrawY (parent.form.vScroll.Value));

						if (parent.form.menuVisualControlTrack.Checked) {
							parent.form.splitContainer1.Panel2MinSize = (parent.form.TrackSelector.getPreferredMinSize ());
						}

						EditorManager.saveConfig ();
						parent.form.applyLanguage ();
						#if ENABLE_SCRIPT
						parent.UpdateScriptShortcut ();
						#endif

						parent.form.updateDrawObjectList ();
						parent.form.refreshScreen ();
					}
				} catch (Exception ex) {
					Logger.write (GetType () + ".menuSettingPreference_Click; ex=" + ex + "\n");
					CDebug.WriteLine ("FormMain#menuSettingPreference_Click; ex=" + ex);
				}
			}

			public void RunSettingShortcutCommand ()
			{
				SortedDictionary<string, ValuePair<string, Keys[]>> dict = new SortedDictionary<string, ValuePair<string, Keys[]>> ();
				SortedDictionary<string, Keys[]> configured = EditorManager.editorConfig.getShortcutKeysDictionary (parent.form.getDefaultShortcutKeys ());
				#if DEBUG
				sout.println ("FormMain#menuSettingShortcut_Click; configured=");
				foreach (var name in configured.Keys) {
					Keys[] keys = configured [name];
					string disp = Utility.getShortcutDisplayString (keys);
					sout.println ("    " + name + " -> " + disp);
				}
				#endif

				// スクリプトのToolStripMenuITemを蒐集
				List<string> script_shortcut = new List<string> ();
				foreach (var tsi in parent.form.menuScript.DropDownItems) {
					if (tsi is UiToolStripMenuItem) {
						var tsmi = (UiToolStripMenuItem)tsi;
						string name = tsmi.Name;
						script_shortcut.Add (name);
						if (!configured.ContainsKey (name)) {
							configured [name] = new Keys[] { };
						}
					}
				}

				foreach (var name in configured.Keys) {
					ByRef<Object> owner = new ByRef<Object> (null);
					Object menu = parent.form.searchMenuItemFromName (name, owner);
					#if DEBUG
					if (menu == null || owner.value == null) {
						serr.println ("FormMain#enuSettingShrtcut_Click; name=" + name + "; menu is null");
						continue;
					}
					#endif
					UiToolStripMenuItem casted_owner_item = null;
					if (owner.value is UiToolStripMenuItem) {
						casted_owner_item = (UiToolStripMenuItem)owner.value;
					}
					if (casted_owner_item == null) {
						continue;
					}
					string parentS = "";
					if (!casted_owner_item.Name.Equals (parent.form.menuHidden.Name)) {
						string s = casted_owner_item.Text;
						int i = s.IndexOf ("(&");
						if (i > 0) {
							s = s.Substring (0, i);
						}
						parentS = s + " -> ";
					}
					UiToolStripMenuItem casted_menu = null;
					if (menu is UiToolStripMenuItem) {
						casted_menu = (UiToolStripMenuItem)menu;
					}
					if (casted_menu == null) {
						continue;
					}
					string s1 = casted_menu.Text;
					int i1 = s1.IndexOf ("(&");
					if (i1 > 0) {
						s1 = s1.Substring (0, i1);
					}
					dict [parentS + s1] = new ValuePair<string, Keys[]> (name, configured [name]);
				}

				// 最初に戻る、のショートカットキー
				Keys[] keysGoToFirst = EditorManager.editorConfig.SpecialShortcutGoToFirst;
				if (keysGoToFirst == null) {
					keysGoToFirst = new Keys[] { };
				}
				dict [_ ("Go to the first")] = new ValuePair<string, Keys[]> ("SpecialShortcutGoToFirst", keysGoToFirst);

				FormShortcutKeys form = null;
				try {
					form = ApplicationUIHost.Create<FormShortcutKeys> (dict, parent.form);
					form.Location = parent.GetFormPreferedLocation (form.Width, form.Height);
					var dr = DialogManager.showModalDialog (form, parent.form);
					if (dr == 1) {
						SortedDictionary<string, ValuePair<string, Keys[]>> res = form.getResult ();
						foreach (var display in res.Keys) {
							string name = res [display].getKey ();
							Keys[] keys = res [display].getValue ();
							bool found = false;
							if (name.Equals ("SpecialShortcutGoToFirst")) {
								EditorManager.editorConfig.SpecialShortcutGoToFirst = keys;
							} else {
								for (int i = 0; i < EditorManager.editorConfig.ShortcutKeys.Count; i++) {
									if (EditorManager.editorConfig.ShortcutKeys [i].Key.Equals (name)) {
										EditorManager.editorConfig.ShortcutKeys [i].Value = keys;
										found = true;
										break;
									}
								}
								if (!found) {
									EditorManager.editorConfig.ShortcutKeys.Add (new ValuePairOfStringArrayOfKeys (name, keys));
								}
							}
						}
						parent.form.applyShortcut ();
					}
				} catch (Exception ex) {
					Logger.write (GetType () + ".menuSettingShortcut_Click; ex=" + ex + "\n");
				} finally {
					if (form != null) {
						try {
							form.Close ();
						} catch (Exception ex2) {
							Logger.write (GetType () + ".menuSettingSHortcut_Click; ex=" + ex2 + "\n");
						}
					}
				}
			}

			public void RunSettingVibratoPresetCommand ()
			{
				FormVibratoPreset dialog = null;
				try {
					dialog = ApplicationUIHost.Create<FormVibratoPreset> (EditorManager.editorConfig.AutoVibratoCustom);
					dialog.Location = parent.GetFormPreferedLocation (dialog.Width, dialog.Height);
					var ret = DialogManager.showModalDialog (dialog, parent.form);
					if (ret != 1) {
						return;
					}

					// ダイアログの結果を取得
					List<VibratoHandle> result = dialog.getResult ();

					// ダイアログ結果を，設定値にコピー
					// ダイアログのコンストラクタであらかじめcloneされているので，
					// ここではcloneする必要はない．
					EditorManager.editorConfig.AutoVibratoCustom.Clear ();
					for (int i = 0; i < result.Count; i++) {
						EditorManager.editorConfig.AutoVibratoCustom.Add (result [i]);
					}

					// メニューの表示状態を更新
					parent.form.updateVibratoPresetMenu ();
				} catch (Exception ex) {
					Logger.write (GetType () + ".menuSettingVibratoPreset_Click; ex=" + ex + "\n");
				} finally {
					if (dialog != null) {
						try {
							dialog.Dispose ();
						} catch (Exception ex2) {
						}
					}
				}
			}

			public void RunSettingSequenceCommand()
			{
				VsqFileEx vsq = MusicManager.getVsqFile();

				FormSequenceConfig dialog = ApplicationUIHost.Create<FormSequenceConfig>();
				int old_channels = vsq.config.WaveFileOutputChannel;
				bool old_output_master = vsq.config.WaveFileOutputFromMasterTrack;
				int old_sample_rate = vsq.config.SamplingRate;
				int old_pre_measure = vsq.getPreMeasure();

				dialog.setWaveFileOutputChannel(old_channels);
				dialog.setWaveFileOutputFromMasterTrack(old_output_master);
				dialog.setSampleRate(old_sample_rate);
				dialog.setPreMeasure(old_pre_measure);

				dialog.Location = parent.GetFormPreferedLocation(dialog);
				if (DialogManager.showModalDialog(dialog, parent.form) != 1) {
					return;
				}

				int new_channels = dialog.getWaveFileOutputChannel();
				bool new_output_master = dialog.isWaveFileOutputFromMasterTrack();
				int new_sample_rate = dialog.getSampleRate();
				int new_pre_measure = dialog.getPreMeasure();

				CadenciiCommand run =
					VsqFileEx.generateCommandChangeSequenceConfig(
						new_sample_rate,
						new_channels,
						new_output_master,
						new_pre_measure);
				EditorManager.editHistory.register(vsq.executeCommand(run));
				parent.form.setEdited(true);
			}

		}
	}
}
