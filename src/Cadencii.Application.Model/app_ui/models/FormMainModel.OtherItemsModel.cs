using System;
using Cadencii.Gui;
using cadencii.vsq;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using cadencii.core;
using System.IO;
using Cadencii.Xml;
using System.Diagnostics;
using Cadencii.Utilities;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public partial class FormMainModel
	{
		public class OtherItemsModel
		{
			readonly FormMainModel parent;

			public OtherItemsModel (FormMainModel parent)
			{
				this.parent = parent;
			}

			/// <summary>
			/// EditorManager.keyWidthを調節するモードに入ったかどうか
			/// </summary>
			bool mKeyLengthSplitterMouseDown = false;
			/// <summary>
			/// EditorManager.keyWidthを調節するモードに入る直前での、マウスのスクリーン座標
			/// </summary>
			Point mKeyLengthSplitterInitialMouse = new Point();
			/// <summary>
			/// EditorManager.keyWidthを調節するモードに入る直前での、keyWidthの値
			/// </summary>
			int mKeyLengthInitValue = 68;
			/// <summary>
			/// EditorManager.keyWidthを調節するモードに入る直前での、trackSelectorのgetRowsPerColumn()の値
			/// </summary>
			int mKeyLengthTrackSelectorRowsPerColumn = 1;
			/// <summary>
			/// EditorManager.keyWidthを調節するモードに入る直前での、splitContainer1のSplitterLocationの値
			/// </summary>
			int mKeyLengthSplitterDistance = 0;
			/// <summary>
			/// ピアノロールの縦方向の拡大率を変更するパネル上でのマウスの状態。
			/// 0がデフォルト、&gt;0は+ボタンにマウスが降りた状態、&lt;0は-ボタンにマウスが降りた状態
			/// </summary>
			private int mPianoRollScaleYMouseStatus = 0;

			//BOOKMARK: pictKeyLengthSplitter
			#region pictKeyLengthSplitter
			public void pictKeyLengthSplitter_MouseDown()
			{
				mKeyLengthSplitterMouseDown = true;
				mKeyLengthSplitterInitialMouse = Screen.Instance.GetScreenMousePosition();
				mKeyLengthInitValue = EditorManager.keyWidth;
				mKeyLengthTrackSelectorRowsPerColumn = parent.form.TrackSelector.getRowsPerColumn();
				mKeyLengthSplitterDistance = parent.form.splitContainer1.DividerLocation;
			}

			public void pictKeyLengthSplitter_MouseMove()
			{
				if (!mKeyLengthSplitterMouseDown) {
					return;
				}
				int dx = Screen.Instance.GetScreenMousePosition().X - mKeyLengthSplitterInitialMouse.X;
				int draft = mKeyLengthInitValue + dx;
				if (draft < AppConfig.MIN_KEY_WIDTH) {
					draft = AppConfig.MIN_KEY_WIDTH;
				} else if (AppConfig.MAX_KEY_WIDTH < draft) {
					draft = AppConfig.MAX_KEY_WIDTH;
				}
				EditorManager.keyWidth = draft;
				int current = parent.form.TrackSelector.getRowsPerColumn();
				if (current >= mKeyLengthTrackSelectorRowsPerColumn) {
					int max_divider_location = parent.form.splitContainer1.Height - parent.form.splitContainer1.DividerSize - parent.form.splitContainer1.Panel2MinSize;
					if (max_divider_location < mKeyLengthSplitterDistance) {
						parent.form.splitContainer1.DividerLocation = (max_divider_location);
					} else {
						parent.form.splitContainer1.DividerLocation = (mKeyLengthSplitterDistance);
					}
				}
				parent.form.updateLayout();
				parent.form.refreshScreen();
			}

			public void pictKeyLengthSplitter_MouseUp()
			{
				mKeyLengthSplitterMouseDown = false;
			}
			#endregion

			public void handleVibratoPresetSubelementClick(UiToolStripMenuItem item)
			{
				string text = item.Text;

				// メニューの表示文字列から，どの設定値についてのイベントかを探す
				VibratoHandle target = null;
				int size = EditorManager.editorConfig.AutoVibratoCustom.Count;
				for (int i = 0; i < size; i++) {
					VibratoHandle handle = EditorManager.editorConfig.AutoVibratoCustom[i];
					if (text.Equals(handle.getCaption())) {
						target = handle;
						break;
					}
				}

				// ターゲットが特定できなかったらbailout
				if (target == null) {
					return;
				}

				// 選択状態のアイテムを取得
				IEnumerable<SelectedEventEntry> itr = EditorManager.itemSelection.getEventIterator();
				if (itr.Count() == 0) {
					// アイテムがないのでbailout
					return;
				}
				VsqEvent ev = itr.First().original;
				if (ev.ID.VibratoHandle == null) {
					return;
				}

				// 設定値にコピー
				VibratoHandle h = ev.ID.VibratoHandle;
				target.setStartRate(h.getStartRate());
				target.setStartDepth(h.getStartDepth());
				if (h.getRateBP() == null) {
					target.setRateBP(null);
				} else {
					target.setRateBP((VibratoBPList)h.getRateBP().clone());
				}
				if (h.getDepthBP() == null) {
					target.setDepthBP(null);
				} else {
					target.setDepthBP((VibratoBPList)h.getDepthBP().clone());
				}
			}

			public void bgWorkScreen_DoWork()
			{
				try {
					parent.form.Invoke(new Action(parent.form.refreshScreenCore));
				} catch (Exception ex) {
					Logger.StdErr("FormMain#bgWorkScreen_DoWork; ex=" + ex);
					Logger.write(GetType () + ".bgWorkScreen_DoWork; ex=" + ex + "\n");
				}
			}

			public void toolStripContainer_TopToolStripPanel_SizeChanged()
			{
				if (parent.form.WindowState == FormWindowState.Minimized) {
					return;
				}
				Dimension minsize = parent.form.getWindowMinimumSize();
				int wid = parent.form.Width;
				int hei = parent.form.Height;
				bool change_size_required = false;
				if (minsize.Width > wid) {
					wid = minsize.Width;
					change_size_required = true;
				}
				if (minsize.Height > hei) {
					hei = minsize.Height;
					change_size_required = true;
				}
				var min_size = parent.form.getWindowMinimumSize();
				parent.form.MinimumSize = new Dimension (min_size.Width, min_size.Height);
				if (change_size_required) {
					parent.form.Size = new Dimension(wid, hei);
				}
			}
			public void handleBgmOffsetSeconds_Click(BgmMenuItem menu)
			{
				int index = menu.getBgmIndex();
				InputBox ib = null;
				try {
					ib = ApplicationUIHost.Create<InputBox>(_("Input Offset Seconds"));
					ib.Location = parent.GetFormPreferedLocation(ib);
					ib.setResult(MusicManager.getBgm(index).readOffsetSeconds + "");
					var dr = DialogManager.ShowModalDialog(ib, parent.form);
					if (dr != Cadencii.Gui.DialogResult.OK) {
						return;
					}
					List<BgmFile> list = new List<BgmFile>();
					int count = MusicManager.getBgmCount();
					BgmFile item = null;
					for (int i = 0; i < count; i++) {
						if (i == index) {
							item = (BgmFile)MusicManager.getBgm(i).clone();
							list.Add(item);
						} else {
							list.Add(MusicManager.getBgm(i));
						}
					}
					double draft;
					try {
						draft = double.Parse(ib.getResult());
						item.readOffsetSeconds = draft;
						menu.ToolTipText = draft + " " + _("seconds");
					} catch (Exception ex3) {
						Logger.write(GetType () + ".handleBgmOffsetSeconds_Click; ex=" + ex3 + "\n");
					}
					CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate(list);
					EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
					parent.form.setEdited(true);
				} catch (Exception ex) {
					Logger.write(GetType () + ".handleBgmOffsetSeconds_Click; ex=" + ex + "\n");
				} finally {
					if (ib != null) {
						try {
							ib.Dispose();
						} catch (Exception ex2) {
							Logger.write(GetType () + ".handleBgmOffsetSeconds_Click; ex=" + ex2 + "\n");
						}
					}
				}
			}

			public void handleBgmStartAfterPremeasure_CheckedChanged(BgmMenuItem menu)
			{
				int index = menu.getBgmIndex();
				List<BgmFile> list = new List<BgmFile>();
				int count = MusicManager.getBgmCount();
				for (int i = 0; i < count; i++) {
					if (i == index) {
						BgmFile item = (BgmFile)MusicManager.getBgm(i).clone();
						item.startAfterPremeasure = menu.Checked;
						list.Add(item);
					} else {
						list.Add(MusicManager.getBgm(i));
					}
				}
				CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate(list);
				EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(run));
				parent.form.setEdited(true);
			}

			public void handleBgmAdd_Click()
			{
				string dir = ApplicationGlobal.appConfig.getLastUsedPathIn("wav");
				parent.form.openWaveDialog.SetSelectedFile(dir);
				var ret = DialogManager.ShowModalFileDialog(parent.form.openWaveDialog, true, parent.form);
				if (ret != Cadencii.Gui.DialogResult.OK) {
					return;
				}

				string file = parent.form.openWaveDialog.FileName;
				ApplicationGlobal.appConfig.setLastUsedPathIn(file, ".wav");

				// 既に開かれていたらキャンセル
				int count = MusicManager.getBgmCount();
				bool found = false;
				for (int i = 0; i < count; i++) {
					BgmFile item = MusicManager.getBgm(i);
					if (file == item.file) {
						found = true;
						break;
					}
				}
				if (found) {
					DialogManager.ShowMessageBox(
						PortUtil.formatMessage(_("file '{0}' is already registered as BGM."), file),
						_("Error"),
						cadencii.Dialog.MSGBOX_DEFAULT_OPTION,
						cadencii.Dialog.MSGBOX_WARNING_MESSAGE);
					return;
				}

				// 登録
				EditorManager.addBgm(file);
				parent.form.setEdited(true);
				parent.form.updateBgmMenuState();
			}

			public void handleBgmRemove_Click(BgmMenuItem item)
			{
				int index = item.getBgmIndex();
				BgmFile bgm = MusicManager.getBgm(index);
				if (DialogManager.ShowMessageBox(PortUtil.formatMessage(_("remove '{0}'?"), bgm.file),
					"Cadencii",
					cadencii.Dialog.MSGBOX_YES_NO_OPTION,
					cadencii.Dialog.MSGBOX_QUESTION_MESSAGE) != Cadencii.Gui.DialogResult.Yes) {
					return;
				}
				EditorManager.removeBgm(index);
				parent.form.setEdited(true);
				parent.form.updateBgmMenuState();
			}

			public void handleSettingPaletteTool(PaletteToolMenuItem tsmi)
			{
				#if ENABLE_SCRIPT
				string id = tsmi.getPaletteToolID();
				if (!PaletteToolServer.loadedTools.ContainsKey(id)) {
					return;
				}
				Object instance = PaletteToolServer.loadedTools[id];
				IPaletteTool ipt = (IPaletteTool)instance;
				if (ipt.openDialog() == Cadencii.Gui.DialogResult.OK) {
					XmlSerializer xsms = new XmlSerializer(instance.GetType(), true);
					string dir = Path.Combine(ApplicationGlobal.getApplicationDataPath(), "tool");
					if (!Directory.Exists(dir)) {
						PortUtil.createDirectory(dir);
					}
					string cfg = id + ".config";
					string config = Path.Combine(dir, cfg);
					FileStream fs = null;
					try {
						fs = new FileStream(config, FileMode.OpenOrCreate, FileAccess.ReadWrite);
						xsms.serialize(fs, null);
					} catch (Exception ex) {
						Logger.write(GetType () + ".handleSettingPaletteTool; ex=" + ex + "\n");
					} finally {
						if (fs != null) {
							try {
								fs.Close();
							} catch (Exception ex2) {
								Logger.write(GetType () + ".handleSettingPaletteTool; ex=" + ex2 + "\n");
							}
						}
					}
				}
				#endif
			}

			//BOOKMARK: vScroll
			#region vScroll
			public void vScroll_Enter()
			{
				parent.form.pictPianoRoll.Focus();
			}

			public void vScroll_ValueChanged()
			{
				parent.form.Model.StartToDrawY = (parent.form.calculateStartToDrawY(parent.form.vScroll.Value));
				if (EditorManager.EditMode != EditMode.MIDDLE_DRAG) {
					// MIDDLE_DRAGのときは，pictPianoRoll_MouseMoveでrefreshScreenされるので，それ以外のときだけ描画・
					parent.form.refreshScreen(true);
				}
			}
			#endregion

			//BOOKMARK: hScroll
			#region hScroll
			public void hScroll_Enter()
			{
				parent.form.pictPianoRoll.Focus();
			}

			public void hScroll_Resize()
			{
				if (parent.form.WindowState != FormWindowState.Minimized) {
					parent.form.updateScrollRangeHorizontal();
				}
			}

			public void hScroll_ValueChanged()
			{
				int stdx = parent.form.calculateStartToDrawX();
				parent.form.Model.StartToDrawX = (stdx);
				if (EditorManager.EditMode != EditMode.MIDDLE_DRAG) {
					// MIDDLE_DRAGのときは，pictPianoRoll_MouseMoveでrefreshScreenされるので，それ以外のときだけ描画・
					parent.form.refreshScreen(true);
				}
			}
			#endregion

			//BOOKMARK: trackBar
			#region trackBar
			public void trackBar_Enter()
			{
				parent.form.pictPianoRoll.Focus();
			}

			public void trackBar_ValueChanged()
			{
				parent.form.Model.ScaleX = (parent.GetScaleXFromTrackBarValue(parent.form.trackBar.Value));
				parent.form.Model.StartToDrawX = (parent.form.calculateStartToDrawX());
				parent.form.updateDrawObjectList();
				parent.form.Refresh();
			}
			#endregion
			#region buttonVZoom & buttonVMooz

			public void buttonVZoom_Click()
			{
				parent.ZoomPianoRollHeight(1);
			}

			public void buttonVMooz_Click()
			{
				parent.ZoomPianoRollHeight(-1);
			}
			#endregion

			#region pictureBox2
			public void pictureBox2_Paint(PaintEventArgs e)
			{
				if (parent.form.mGraphicsPictureBox2 == null) {
					parent.form.mGraphicsPictureBox2 = new Graphics();
				}
				var ctl = parent.form.mGraphicsPictureBox2;
					ctl.NativeGraphics = e.Graphics.NativeGraphics;
				int width = parent.form.pictureBox2.Width;
				int height = parent.form.pictureBox2.Height;
				int unit_height = height / 4;
				ctl.setColor(FormMainModel.ColorR214G214B214);
				ctl.fillRect(0, 0, width, height);
				if (mPianoRollScaleYMouseStatus > 0) {
					ctl.setColor(Cadencii.Gui.Colors.Gray);
					ctl.fillRect(0, 0, width, unit_height);
				} else if (mPianoRollScaleYMouseStatus < 0) {
					ctl.setColor(Cadencii.Gui.Colors.Gray);
					ctl.fillRect(0, unit_height * 2, width, unit_height);
				}
				ctl.setStroke(getStrokeDefault());
				ctl.setColor(Cadencii.Gui.Colors.Gray);
				//mGraphicsPictureBox2.drawRect( 0, 0, width - 1, unit_height * 2 );
				ctl.drawLine(0, unit_height, width, unit_height);
				ctl.drawLine(0, unit_height * 2, width, unit_height * 2);
				ctl.setStroke(getStroke2px());
				int cx = width / 2;
				int cy = unit_height / 2;
				ctl.setColor((mPianoRollScaleYMouseStatus > 0) ? Cadencii.Gui.Colors.LightGray : Cadencii.Gui.Colors.Gray);
				ctl.drawLine(cx - 4, cy, cx + 4, cy);
				ctl.drawLine(cx, cy - 4, cx, cy + 4);
				cy += unit_height * 2;
				ctl.setColor((mPianoRollScaleYMouseStatus < 0) ? Cadencii.Gui.Colors.LightGray : Cadencii.Gui.Colors.Gray);
				ctl.drawLine(cx - 4, cy, cx + 4, cy);
			}

			public void pictureBox2_MouseDown(MouseEventArgs e)
			{
				// 拡大・縮小ボタンが押されたかどうか判定
				int height = parent.form.pictureBox2.Height;
				int width = parent.form.pictureBox2.Width;
				int height4 = height / 4;
				if (0 <= e.X && e.X < width) {
					int scaley = EditorManager.editorConfig.PianoRollScaleY;
					if (0 <= e.Y && e.Y < height4) {
						if (scaley + 1 <= AppConfig.MAX_PIANOROLL_SCALEY) {
							parent.ZoomPianoRollHeight(1);
							mPianoRollScaleYMouseStatus = 1;
						} else {
							mPianoRollScaleYMouseStatus = 0;
						}
					} else if (height4 * 2 <= e.Y && e.Y < height4 * 3) {
						if (scaley - 1 >= AppConfig.MIN_PIANOROLL_SCALEY) {
							parent.ZoomPianoRollHeight(-1);
							mPianoRollScaleYMouseStatus = -1;
						} else {
							mPianoRollScaleYMouseStatus = 0;
						}
					} else {
						mPianoRollScaleYMouseStatus = 0;
					}
				} else {
					mPianoRollScaleYMouseStatus = 0;
				}
				parent.form.refreshScreen();
			}

			public void pictureBox2_MouseUp()
			{
				mPianoRollScaleYMouseStatus = 0;
				parent.form.pictureBox2.Invalidate();
			}
			#endregion


			public void menuToolsCreateVConnectSTANDDb_Click()
			{
				string creator = Path.Combine(PortUtil.getApplicationStartupPath (), "vConnectStandDBConvert.exe");
				if (System.IO.File.Exists(creator)) {
					Process.Start(creator);
				}
			}

			public void menuHelpCheckForUpdates_Click()
			{
				parent.form.showUpdateInformationAsync(true);
			}
			//BOOKMARK: propertyWindow
			#region PropertyWindowListenerの実装

			#if ENABLE_PROPERTY
			public void propertyWindowFormClosing()
			{
			#if DEBUG
				Logger.StdOut("FormMain#propertyWindowFormClosing");
			#endif
				parent.form.updatePropertyPanelState(PanelState.Hidden);
			}
			#endif

			#if ENABLE_PROPERTY
			public void propertyWindowStateChanged()
			{
			#if DEBUG
				Logger.StdOut("FormMain#propertyWindow_WindowStateChanged");
			#endif
				if (EditorManager.editorConfig.PropertyWindowStatus.State == PanelState.Window) {
			#if DEBUG
					Logger.StdOut("FormMain#proprtyWindow_WindowStateChanged; isWindowMinimized=" + EditorManager.propertyWindow.getUi().isWindowMinimized());
			#endif
					if (EditorManager.propertyWindow.getUi().isWindowMinimized()) {
						parent.form.updatePropertyPanelState(PanelState.Docked);
					}
				}
			}

			public void propertyWindowLocationOrSizeChanged()
			{
			#if DEBUG
				Logger.StdOut("FormMain#propertyWindow_LocationOrSizeChanged");
			#endif
				if (EditorManager.editorConfig.PropertyWindowStatus.State == PanelState.Window) {
					if (EditorManager.propertyWindow != null && false == EditorManager.propertyWindow.getUi().isWindowMinimized()) {
						var parent = this.parent.form.Location;
						int propertyX = EditorManager.propertyWindow.getUi().getX();
						int propertyY = EditorManager.propertyWindow.getUi().getY();
						EditorManager.editorConfig.PropertyWindowStatus.Bounds =
							new XmlRectangle(propertyX - parent.X,
								propertyY - parent.Y,
								EditorManager.propertyWindow.getUi().getWidth(),
								EditorManager.propertyWindow.getUi().getHeight());
					}
				}
			}
			#endif
			#endregion

			//BOOKMARK: iconPalette
			#region iconPalette
			public void iconPalette_LocationChanged()
			{
				var point = EditorManager.iconPalette.Location;
				EditorManager.editorConfig.FormIconPaletteLocation = new XmlPoint(point.X, point.Y);
			}

			public void iconPalette_FormClosing()
			{
				parent.FlipIconPaletteVisible(EditorManager.iconPalette.Visible);
			}
			#endregion


			//BOOKMARK: mixerWindow
			#region mixerWindow
			public void mixerWindow_FormClosing()
			{
				parent.FlipMixerDialogVisible(EditorManager.MixerWindow.Visible);
			}

			public void mixerWindow_SoloChanged(int track, bool solo)
			{
				#if DEBUG
				CDebug.WriteLine("FormMain#mixerWindow_SoloChanged");
				CDebug.WriteLine("    track=" + track);
				CDebug.WriteLine("    solo=" + solo);
				#endif
				VsqFileEx vsq = MusicManager.getVsqFile();
				if (vsq == null) {
					return;
				}
				vsq.setSolo(track, solo);
				if (EditorManager.MixerWindow != null) {
					EditorManager.MixerWindow.updateStatus();
				}
			}

			public void mixerWindow_MuteChanged(int track, bool mute)
			{
				#if DEBUG
				CDebug.WriteLine("FormMain#mixerWindow_MuteChanged");
				CDebug.WriteLine("    track=" + track);
				CDebug.WriteLine("    mute=" + mute);
				#endif
				VsqFileEx vsq = MusicManager.getVsqFile();
				if (vsq == null) {
					return;
				}
				if (track < 0) {
					MusicManager.getBgm(-track - 1).mute = mute ? 1 : 0;
				} else {
					vsq.setMute(track, mute);
				}
				if (EditorManager.MixerWindow != null) {
					EditorManager.MixerWindow.updateStatus();
				}
			}

			public void mixerWindow_PanpotChanged(int track, int panpot)
			{
				if (track == 0) {
					// master
					MusicManager.getVsqFile().Mixer.MasterPanpot = panpot;
				} else if (track > 0) {
					// slave
					MusicManager.getVsqFile().Mixer.Slave[track - 1].Panpot = panpot;
				} else {
					MusicManager.getBgm(-track - 1).panpot = panpot;
				}
			}

			public void mixerWindow_FederChanged(int track, int feder)
			{
				#if DEBUG
				Logger.StdOut("FormMain#mixerWindow_FederChanged; track=" + track + "; feder=" + feder);
				#endif
				if (track == 0) {
					MusicManager.getVsqFile().Mixer.MasterFeder = feder;
				} else if (track > 0) {
					MusicManager.getVsqFile().Mixer.Slave[track - 1].Feder = feder;
				} else {
					MusicManager.getBgm(-track - 1).feder = feder;
				}
			}
			#endregion

			#region mPropertyPanelContainer
			#if ENABLE_PROPERTY
			public void mPropertyPanelContainer_StateChangeRequired(PanelState arg)
			{
				parent.form.updatePropertyPanelState(arg);
			}
			#endif
			#endregion

			#region propertyPanel
			#if ENABLE_PROPERTY
			public void propertyPanel_CommandExecuteRequired(CadenciiCommand command)
			{
			#if DEBUG
				CDebug.WriteLine("m_note_property_dlg_CommandExecuteRequired");
			#endif
				EditorManager.editHistory.register(MusicManager.getVsqFile().executeCommand(command));
				parent.form.updateDrawObjectList();
				parent.form.refreshScreen();
				parent.form.setEdited(true);
			}
			#endif
			#endregion

			public void menuWindowMinimize_Click()
			{
				var state = (FormWindowState) parent.form.WindowState;
				if (state != FormWindowState.Minimized) {
					parent.form.WindowState = FormWindowState.Minimized;
				}
			}

			//BOOKMARK: panelOverview
			#region panelOverview
			public void panelOverview_Enter()
			{
				parent.form.pictPianoRoll.Focus ();
			}
			#endregion

			/// <summary>
			/// 現在のツールバーの場所を保存します
			/// </summary>
			public void SaveToolbarLocation()
			{
				if (parent.form.WindowState == FormWindowState.Minimized) return;
				// どのツールバーが一番上かつ左にあるか？
				var list = new System.Collections.Generic.List<RebarBand>();
				list.AddRange(new RebarBand[]{
					parent.form.bandFile,
					parent.form.bandMeasure,
					parent.form.bandPosition,
					parent.form.bandTool });
				// ソートする
				bool changed = true;
				while (changed) {
					changed = false;
					for (int i = 0; i < list.Count - 1; i++) {
						// y座標が大きいか，y座標が同じでもx座標が大きい場合に入れ替える
						bool swap =
							(list[i].Location.Y > list[i + 1].Location.Y) ||
							(list[i].Location.Y == list[i + 1].Location.Y && list[i].Location.X > list[i + 1].Location.X);
						if (swap) {
							var a = list[i];
							list[i] = list[i + 1];
							list[i + 1] = a;
							changed = true;
						}
					}
				}
				// 各ツールバー毎に，ツールバーの状態を検出して保存
				saveToolbarLocationCore(
					list,
					parent.form.bandFile,
					out EditorManager.editorConfig.BandSizeFile,
					out EditorManager.editorConfig.BandNewRowFile,
					out EditorManager.editorConfig.BandOrderFile);
				saveToolbarLocationCore(
					list,
					parent.form.bandMeasure,
					out EditorManager.editorConfig.BandSizeMeasure,
					out EditorManager.editorConfig.BandNewRowMeasure,
					out EditorManager.editorConfig.BandOrderMeasure);
				saveToolbarLocationCore(
					list,
					parent.form.bandPosition,
					out EditorManager.editorConfig.BandSizePosition,
					out EditorManager.editorConfig.BandNewRowPosition,
					out EditorManager.editorConfig.BandOrderPosition);
				saveToolbarLocationCore(
					list,
					parent.form.bandTool,
					out EditorManager.editorConfig.BandSizeTool,
					out EditorManager.editorConfig.BandNewRowTool,
					out EditorManager.editorConfig.BandOrderTool);
			}

			/// <summary>
			/// ツールバーの位置の順に並べ替えたリストの中の一つのツールバーに対して，その状態を検出して保存
			/// </summary>
			/// <param name="list"></param>
			/// <param name="band"></param>
			/// <param name="band_size"></param>
			/// <param name="new_row"></param>
			private void saveToolbarLocationCore(
				System.Collections.Generic.List<RebarBand> list,
				RebarBand band,
				out int band_size,
				out bool new_row,
				out int band_order)
			{
				band_size = 0;
				new_row = true;
				band_order = 0;
				var indx = list.IndexOf(band);
				if (indx < 0) return;
				new_row = (indx == 0) ? false : (list[indx - 1].Location.Y < list[indx].Location.Y);
				band_size = band.BandSize;
				band_order = indx;
			}

			/// <summary>
			/// デフォルトのストローク
			/// </summary>
			Stroke mStrokeDefault = null;
			/// <summary>
			/// デフォルトのストロークを取得します
			/// </summary>
			/// <returns></returns>
			Stroke getStrokeDefault()
			{
				if (mStrokeDefault == null) {
					mStrokeDefault = new Stroke();
				}
				return mStrokeDefault;
			}

			/// <summary>
			/// 描画幅2pxのストローク
			/// </summary>
			Stroke mStroke2px = null;
			/// <summary>
			/// 描画幅が2pxのストロークを取得します
			/// </summary>
			/// <returns></returns>
			Stroke getStroke2px()
			{
				if (mStroke2px == null) {
					mStroke2px = new Stroke(2.0f);
				}
				return mStroke2px;
			}
		}
	}
}
