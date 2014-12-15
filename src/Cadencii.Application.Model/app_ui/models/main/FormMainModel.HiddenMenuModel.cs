using System;
using System.Collections.Generic;
using cadencii.apputil;
using System.IO;
using System.Text;
using Cadencii.Media.Vsq;
using Cadencii.Gui;
using Cadencii.Utilities;
using cadencii;
using Cadencii.Application.Media;

namespace Cadencii.Application.Models
{
	public partial class FormMainModel
	{
		public class HiddenMenuModel
		{
			readonly FormMainModel parent;

			public HiddenMenuModel (FormMainModel parent)
			{
				this.parent = parent;
			}
			//BOOKMARK: menuHidden
			#region menuHidden*
			public void RunHiddenVisualForwardParameterCommand()
			{
				parent.form.TrackSelector.Model.SelectNextCurve();
			}

			public void RunHiddenVisualBackwardParameterCommand()
			{
				parent.form.TrackSelector.Model.SelectPreviousCurve();
			}

			public void RunHiddenTrackNextCommand()
			{
				if (EditorManager.Selected == MusicManager.getVsqFile().Track.Count - 1) {
					EditorManager.Selected = (1);
				} else {
					EditorManager.Selected = (EditorManager.Selected + 1);
				}
				parent.form.refreshScreen();
			}

			public void RunHiddenShortenCommand()
			{
				QuantizeMode qmode = EditorManager.editorConfig.getLengthQuantize();
				bool triplet = EditorManager.editorConfig.isLengthQuantizeTriplet();
				int delta = -QuantizeModeUtil.getQuantizeClock(qmode, triplet);
				parent.LengthenSelectedEvent(delta);
			}

			public void RunHiddenTrackBackCommand()
			{
				if (EditorManager.Selected == 1) {
					EditorManager.Selected = (MusicManager.getVsqFile().Track.Count - 1);
				} else {
					EditorManager.Selected = (EditorManager.Selected - 1);
				}
				parent.form.refreshScreen();
			}

			public void RunHiddenEditPasteCommand()
			{
				parent.Paste();
			}

			public void RunHiddenFlipCurveOnPianorollModeCommand()
			{
				EditorManager.mCurveOnPianoroll = !EditorManager.mCurveOnPianoroll;
				parent.form.refreshScreen();
			}

			public void RunHiddenGoToEndMarkerCommand()
			{
				if (EditorManager.isPlaying()) {
					return;
				}

				VsqFileEx vsq = MusicManager.getVsqFile();
				if (vsq.config.EndMarkerEnabled) {
					EditorManager.setCurrentClock(vsq.config.EndMarker);
					parent.EnsurePlayerCursorVisible();
					parent.form.refreshScreen();
				}
			}

			public void RunHiddenGoToStartMarkerCommand()
			{
				if (EditorManager.isPlaying()) {
					return;
				}

				VsqFileEx vsq = MusicManager.getVsqFile();
				if (vsq.config.StartMarkerEnabled) {
					EditorManager.setCurrentClock(vsq.config.StartMarker);
					parent.EnsurePlayerCursorVisible();
					parent.form.refreshScreen();
				}
			}

			public void RunHiddenLengthenCommand()
			{
				QuantizeMode qmode = EditorManager.editorConfig.getLengthQuantize();
				bool triplet = EditorManager.editorConfig.isLengthQuantizeTriplet();
				int delta = QuantizeModeUtil.getQuantizeClock(qmode, triplet);
				parent.LengthenSelectedEvent(delta);
			}

			public void RunHiddenMoveDownCommand()
			{
				parent.form.moveUpDownLeftRight(-1, 0);
			}

			public void RunHiddenMoveUpCommand()
			{
				parent.form.moveUpDownLeftRight(1, 0);
			}

			public void RunHiddenPlayFromStartMarkerCommand()
			{
				if (EditorManager.isPlaying()) {
					return;
				}
				VsqFileEx vsq = MusicManager.getVsqFile();
				if (!vsq.config.StartMarkerEnabled) {
					return;
				}

				EditorManager.setCurrentClock(vsq.config.StartMarker);
				EditorManager.setPlaying(true, parent.form);
			}

			public void RunHiddenPrintPoToCSVCommand()
			{
				#if DEBUG
				Logger.StdOut("FormMain#menuHiddenPrintPoToCSV_Click");
				#endif

				List<string> keys = new List<string>();
				string[] langs = Messaging.getRegisteredLanguage();
				foreach (string lang in langs) {
					foreach (string key in Messaging.getKeys(lang)) {
						if (!keys.Contains(key)) {
							keys.Add(key);
						}
					}
				}

				keys.Sort();
				string dir = PortUtil.getApplicationStartupPath();
				string fname = Path.Combine(dir, "cadencii_trans.csv");
				#if DEBUG
				Logger.StdOut("FormMain#menuHiddenPrintPoToCSV_Click; fname=" + fname);
				#endif
				string old_lang = Messaging.getLanguage();
				StreamWriter br = null;
				try {
					br = new StreamWriter(fname, false, new UTF8Encoding(false));
					string line = "\"en\"";
					foreach (string lang in langs) {
						line += ",\"" + lang + "\"";
					}
					br.WriteLine(line);
					foreach (string key in keys) {
						line = "\"" + key + "\"";
						foreach (string lang in langs) {
							Messaging.setLanguage(lang);
							line += ",\"" + Messaging.getMessage(key) + "\"";
						}
						br.WriteLine(line);
					}
				} catch (Exception ex) {
					Logger.StdErr("FormMain#menuHiddenPrintPoToCSV_Click; ex=" + ex);
				} finally {
					if (br != null) {
						try {
							br.Close();
						} catch (Exception ex2) {
						}
					}
				}
				Messaging.setLanguage(old_lang);
			}

			public void RunHiddenMoveLeftCommand()
			{
				QuantizeMode mode = EditorManager.editorConfig.getPositionQuantize();
				bool triplet = EditorManager.editorConfig.isPositionQuantizeTriplet();
				int delta = -QuantizeModeUtil.getQuantizeClock(mode, triplet);
				#if DEBUG
				Logger.StdOut("FormMain#menuHiddenMoveLeft_Click; delta=" + delta);
				#endif
				parent.form.moveUpDownLeftRight(0, delta);
			}

			public void RunHiddenMoveRightCommand()
			{
				QuantizeMode mode = EditorManager.editorConfig.getPositionQuantize();
				bool triplet = EditorManager.editorConfig.isPositionQuantizeTriplet();
				int delta = QuantizeModeUtil.getQuantizeClock(mode, triplet);
				parent.form.moveUpDownLeftRight(0, delta);
			}

			public void RunHiddenSelectBackwardCommand()
			{
				parent.SelectBackward();
			}

			public void RunHiddenSelectForwardCommand()
			{
				parent.SelectForward();
			}

			public void RunHiddenEditFlipToolPointerPencilCommand()
			{
				if (EditorManager.SelectedTool == EditTool.ARROW) {
					EditorManager.SelectedTool = (EditTool.PENCIL);
				} else {
					EditorManager.SelectedTool = (EditTool.ARROW);
				}
				parent.form.refreshScreen();
			}

			public void RunHiddenEditFlipToolPointerEraserCommand()
			{
				if (EditorManager.SelectedTool == EditTool.ARROW) {
					EditorManager.SelectedTool = (EditTool.ERASER);
				} else {
					EditorManager.SelectedTool = (EditTool.ARROW);
				}
				parent.form.refreshScreen();
			}

			public void RunHiddenEditLyricCommand()
			{
				bool input_enabled = EditorManager.InputTextBox.Enabled;
				if (!input_enabled && EditorManager.itemSelection.getEventCount() > 0) {
					VsqEvent original = EditorManager.itemSelection.getLastEvent().original;
					int clock = original.Clock;
					int note = original.ID.Note;
					Point pos = new Point(EditorManager.xCoordFromClocks(clock), EditorManager.yCoordFromNote(note));
					if (!EditorManager.editorConfig.KeepLyricInputMode) {
						parent.mLastSymbolEditMode = false;
					}
					parent.showInputTextBox(original.ID.LyricHandle.L0.Phrase,
						original.ID.LyricHandle.L0.getPhoneticSymbol(),
						pos, parent.mLastSymbolEditMode);
					parent.form.refreshScreen();
				} else if (input_enabled) {
					if (EditorManager.InputTextBox.IsPhoneticSymbolEditMode) {
						parent.FlipInputTextBoxMode();
					}
				}
			}
			#endregion

		}
	}
}
