﻿using System;
using cadencii.core;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using Cadencii.Media.Vsq;
using cadencii;
using Cadencii.Application.Media;
using Cadencii.Utilities;

namespace Cadencii.Application.Models
{
	public partial class FormMainModel
	{
		public class VisualMenuModel
		{
			readonly FormMainModel parent;

			public VisualMenuModel (FormMainModel parent)
			{
				this.parent = parent;
			}

			//BOOKMARK: menuVisual
			#region menuVisual*
			public void RunVisualPropertyCheckedChanged()
			{
				#if ENABLE_PROPERTY
				if (parent.form.menuVisualProperty.Checked) {
					if (EditorManager.editorConfig.PropertyWindowStatus.IsMinimized) {
						parent.form.updatePropertyPanelState(PanelState.Docked);
					} else {
						parent.form.updatePropertyPanelState(PanelState.Window);
					}
				} else {
					parent.form.updatePropertyPanelState(PanelState.Hidden);
				}
				#endif
			}

			public void RunVisualOverviewCheckedChanged()
			{
				#if DEBUG
				Cadencii.Utilities.Logger.StdOut("FormMain#menuVisualOverview_CheckedChanged; menuVisualOverview.isSelected()=" + parent.form.menuVisualOverview.Checked);
				#endif
				EditorManager.editorConfig.OverviewEnabled = parent.form.menuVisualOverview.Checked;
				parent.form.updateLayout();
			}

			public void RunVisualMixerCommand()
			{
				bool v = !EditorManager.editorConfig.MixerVisible;
				parent.FlipMixerDialogVisible(v);
				parent.form.Focus();
			}

			public void RunVisualGridlineCheckedChanged()
			{
				EditorManager.setGridVisible(parent.form.menuVisualGridline.Checked);
				parent.form.refreshScreen();
			}

			public void RunVisualIconPaletteCommand()
			{
				bool v = !EditorManager.editorConfig.IconPaletteVisible;
				parent.FlipIconPaletteVisible(v);
			}

			public void RunVisualLyricsCheckedChanged()
			{
				EditorManager.editorConfig.ShowLyric = parent.form.menuVisualLyrics.Checked;
			}

			public void RunVisualNotePropertyCheckedChanged()
			{
				EditorManager.editorConfig.ShowExpLine = parent.form.menuVisualNoteProperty.Checked;
				parent.form.refreshScreen();
			}

			public void RunVisualPitchLineCheckedChanged()
			{
				ApplicationGlobal.appConfig.ViewAtcualPitch = parent.form.menuVisualPitchLine.Checked;
			}

			public void RunVisualControlTrackCheckedChanged()
			{
				parent.FlipControlCurveVisible(parent.form.menuVisualControlTrack.Checked);
			}

			public void RunVisualWaveformCheckedChanged()
			{
				ApplicationGlobal.appConfig.ViewWaveform = parent.form.menuVisualWaveform.Checked;
				parent.updateSplitContainer2Size(true);
			}

			public void RunVisualPluginUiDropDownOpening()
			{
				#if ENABLE_VOCALOID
				// VOCALOID1, 2
				int c = VSTiDllManager.vocaloidDriver.Count;
				for (int i = 0; i < c; i++) {
					VocaloidDriver vd = VSTiDllManager.vocaloidDriver[i];
					bool chkv = true;
					PluginUI ui;
					if (vd == null) {
						chkv = false;
					} else if (!vd.loaded) {
						chkv = false;
					} else if ((ui = vd.getUi(parent.form)) == null) {
						chkv = false;
					} else if (ui.IsDisposed) {
						chkv = false;
					} else if (!ui.Visible) {
						chkv = false;
					}
					RendererKind kind = vd.getRendererKind();
					if (kind == RendererKind.VOCALOID1) {
						parent.form.menuVisualPluginUiVocaloid1.Checked = chkv;
					} else if (kind == RendererKind.VOCALOID2) {
						parent.form.menuVisualPluginUiVocaloid2.Checked = chkv;
					}
				}
				#endif

				#if ENABLE_AQUESTONE
				// AquesTone
				AquesToneDriver drv = VSTiDllManager.getAquesToneDriver();
				bool chk = true;
				if (drv == null) {
					chk = false;
				} else if (!drv.loaded) {
					chk = false;
				} else {
					var ui = drv.getUi(parent.form);
					if (ui == null) {
						chk = false;
					} else if (ui.IsDisposed) {
						chk = false;
					} else if (!ui.Visible) {
						chk = false;
					}
				}
				parent.form.menuVisualPluginUiAquesTone.Checked = chk;
				#endif
			}

			public void RunVisualPluginUiVocaloidCommonCommand(RendererKind search)
			{
				#if DEBUG
				Logger.StdOut("FormMain#menuVisualPluginVocaloidCommon_Click; search=" + search);
				#endif

				#if ENABLE_VOCALOID
				int c = VSTiDllManager.vocaloidDriver.Count;
				for (int i = 0; i < c; i++) {
					VocaloidDriver vd = VSTiDllManager.vocaloidDriver[i];
					bool chk = true;
					if (vd == null) {
						chk = false;
					} else if (!vd.loaded) {
						chk = false;
					} else {
						var ui = vd.getUi(parent.form);
						if (ui == null) {
							chk = false;
						} else if (ui.IsDisposed) {
							chk = false;
						}
					}
					if (!chk) {
						continue;
					}
					RendererKind kind = vd.getRendererKind();
					bool v = true;
					if (kind == search) {
						if (search == RendererKind.VOCALOID1) {
							v = !parent.form.menuVisualPluginUiVocaloid1.Checked;
							parent.form.menuVisualPluginUiVocaloid1.Checked = v;
							var ui = vd.getUi(parent.form);
							ui.Visible = v;
						} else if (search == RendererKind.VOCALOID2) {
							v = !parent.form.menuVisualPluginUiVocaloid2.Checked;
							parent.form.menuVisualPluginUiVocaloid2.Checked = v;
							var ui = vd.getUi(parent.form);
							ui.Visible = v;
						}
						break;
					}
				}
				#endif
			}

			private void onClickVisualPluginUiAquesTone(UiToolStripMenuItem menu, AquesToneDriverBase drv)
			{
				bool visible = !menu.Checked;
				menu.Checked = visible;
				#if ENABLE_AQUESTONE
				bool chk = true;
				PluginUI ui = null;
				if (drv == null) {
					chk = false;
				} else if (!drv.loaded) {
					chk = false;
				} else {
					ui = drv.getUi(parent.form);
					if (ui == null) {
						chk = false;
					} else if (ui.IsDisposed) {
						chk = false;
					}
				}
				if (!chk) {
					menu.Checked = false;
					return;
				}
				if (ui != null && !ui.IsDisposed) {
					ui.Visible = visible;
				}
				#endif
			}

			public void RunVisualPluginUiAquesToneCommand()
			{
				onClickVisualPluginUiAquesTone(parent.form.menuVisualPluginUiAquesTone, VSTiDllManager.getAquesToneDriver());
			}

			public void RunVisualPluginUiAquesTone2Command()
			{
				onClickVisualPluginUiAquesTone(parent.form.menuVisualPluginUiAquesTone2, VSTiDllManager.getAquesTone2Driver());
			}

			public void RunStartMarkerCommand()
			{
				VsqFileEx vsq = MusicManager.getVsqFile();
				vsq.config.StartMarkerEnabled = !vsq.config.StartMarkerEnabled;
				parent.form.menuVisualStartMarker.Checked = vsq.config.StartMarkerEnabled;
				parent.form.setEdited(true);
				parent.form.pictPianoRoll.Focus();
				parent.form.refreshScreen();
			}

			public void RunEndMarkerCommand()
			{
				VsqFileEx vsq = MusicManager.getVsqFile();
				vsq.config.EndMarkerEnabled = !vsq.config.EndMarkerEnabled;
				parent.form.stripBtnEndMarker.Pushed = vsq.config.EndMarkerEnabled;
				parent.form.menuVisualEndMarker.Checked = vsq.config.EndMarkerEnabled;
				parent.form.setEdited(true);
				parent.form.pictPianoRoll.Focus();
				parent.form.refreshScreen();
			}

			#endregion
		}
	}
}
