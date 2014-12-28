using System;
using Cadencii.Gui;
using cadencii.core;
using System.Collections.Generic;
using Cadencii.Media.Vsq;
using System.IO;
using System.Threading;
using cadencii.apputil;
using cadencii.utau;
using Cadencii.Utilities;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Forms;
using cadencii;
using Cadencii.Application.Media;
using Cadencii.Application.Controls;
using Cadencii.Application.Drawing;
using Cadencii.Media;
using Cadencii.Application.Scripts;

namespace Cadencii.Application.Models
{
	public partial class FormMainModel
	{
		public static class Consts
		{
			public const string ApplicationName = "Cadencii";
			/// <summary>
			/// splitContainer*で使用するSplitterWidthプロパティの値
			/// </summary>
			public const int _SPL_SPLITTER_WIDTH = 4;
			/// <summary>
			/// エントリの端を移動する時の、ハンドル許容範囲の幅
			/// </summary>
			public const int _EDIT_HANDLE_WIDTH = 7;
			/// <summary>
			/// 表情線の先頭部分のピクセル幅
			/// </summary>
			public const int _PX_ACCENT_HEADER = 21;

			#region constants and internal enums
			/// <summary>
			/// スクロールバーの最小サイズ(ピクセル)
			/// </summary>
			public const int MIN_BAR_ACTUAL_LENGTH = 17;
			public const int _TOOL_BAR_HEIGHT = 46;
			/// <summary>
			/// 単音プレビュー時に、wave生成完了を待つ最大の秒数
			/// </summary>
			public const double _WAIT_LIMIT = 5.0;
			public const string RECENT_UPDATE_INFO_URL = "http://www.cadencii.info/recent.php";
			/// <summary>
			/// splitContainer2.Panel2の最小サイズ
			/// </summary>
			public const int _SPL2_PANEL2_MIN_HEIGHT = 25;
			public const int _PICT_POSITION_INDICATOR_HEIGHT = 48;
			public const int _SCROLL_WIDTH = 16;
			/// <summary>
			/// Overviewペインの高さ
			/// </summary>
			public const int _OVERVIEW_HEIGHT = 50;
			/// <summary>
			/// splitContainerPropertyの最小幅
			/// </summary>
			public const int _PROPERTY_DOCK_MIN_WIDTH = 50;
			/// <summary>
			/// WAVE再生時のバッファーサイズの最大値
			/// </summary>
			public const int MAX_WAVE_MSEC_RESOLUTION = 1000;
			/// <summary>
			/// WAVE再生時のバッファーサイズの最小値
			/// </summary>
			public const int MIN_WAVE_MSEC_RESOLUTION = 100;
			#endregion

		}

		#region static readonly field
		/// <summary>
		/// ピアノロールでの，音符の塗りつぶし色
		/// </summary>
		public static readonly Color ColorNoteFill = new Color(181, 220, 86);
		public static readonly Color ColorR105G105B105 = new Color(105, 105, 105);
		public static readonly Color ColorR187G187B255 = new Color(187, 187, 255);
		public static readonly Color ColorR007G007B151 = new Color(7, 7, 151);
		public static readonly Color ColorR065G065B065 = new Color(65, 65, 65);
		public static readonly Color ColorTextboxBackcolor = new Color(128, 128, 128);
		public static readonly Color ColorR214G214B214 = new Color(214, 214, 214);
		#endregion

		private static readonly AuthorListEntry[] _CREDIT = new AuthorListEntry[]{
			new AuthorListEntry( "is developped by:", 2 ),
			new AuthorListEntry( "kbinani", "@kbinani" ),
			new AuthorListEntry( "修羅場P", "@shurabaP" ),
			new AuthorListEntry( "もみじぱん", "@momijipan" ),
			new AuthorListEntry( "結晶", "@gondam" ),
			new AuthorListEntry( "" ),
			new AuthorListEntry(),
			new AuthorListEntry(),
			new AuthorListEntry( "Special Thanks to", 3 ),
			new AuthorListEntry(),
			new AuthorListEntry( "tool icons designer:", 2 ),
			new AuthorListEntry( "Yusuke KAMIYAMANE", "@ykamiyamane" ),
			new AuthorListEntry(),
			new AuthorListEntry( "developper of WORLD:", 2 ),
			new AuthorListEntry( "Masanori MORISE", "@m_morise" ),
			new AuthorListEntry(),
			new AuthorListEntry( "developper of v.Connect-STAND:", 2 ),
			new AuthorListEntry( "修羅場P", "@shurabaP" ),
			new AuthorListEntry(),
			new AuthorListEntry( "developper of UTAU:", 2 ),
			new AuthorListEntry( "飴屋/菖蒲", "@ameyaP_" ),
			new AuthorListEntry(),
			new AuthorListEntry( "developper of RebarDotNet:", 2 ),
			new AuthorListEntry( "Anthony Baraff" ),
			new AuthorListEntry(),
			new AuthorListEntry( "promoter:", 2 ),
			new AuthorListEntry( "zhuo", "@zhuop" ),
			new AuthorListEntry(),
			new AuthorListEntry( "library tester:", 2 ),
			new AuthorListEntry( "evm" ),
			new AuthorListEntry( "そろそろP" ),
			new AuthorListEntry( "めがね１１０" ),
			new AuthorListEntry( "上総" ),
			new AuthorListEntry( "NOIKE", "@knoike" ),
			new AuthorListEntry( "逃亡者" ),
			new AuthorListEntry(),
			new AuthorListEntry( "translator:", 2 ),
			new AuthorListEntry( "Eji (zh-TW translation)", "@ejiwarp" ),
			new AuthorListEntry( "kankan (zh-TW translation)" ),
			new AuthorListEntry( "yxmline (zh-CN translation)" ),
			new AuthorListEntry( "BubblyYoru (en translation)", "@BubblyYoru" ),
			new AuthorListEntry( "BeForU (kr translation)", "@BeForU" ),
			new AuthorListEntry(),
			new AuthorListEntry(),
			new AuthorListEntry( "Thanks to", 3 ),
			new AuthorListEntry(),
			new AuthorListEntry( "ないしょの人" ),
			new AuthorListEntry( "naquadah" ),
			new AuthorListEntry( "1zo" ),
			new AuthorListEntry( "Amby" ),
			new AuthorListEntry( "ケロッグ" ),
			new AuthorListEntry( "beginner" ),
			new AuthorListEntry( "b2ox", "@b2ox" ),
			new AuthorListEntry( "麻太郎" ),
			new AuthorListEntry( "PEX", "@pex_zeo" ),
			new AuthorListEntry( "やなぎがうら" ),
			new AuthorListEntry( "cocoonP", "@cocoonP" ),
			new AuthorListEntry( "かつ" ),
			new AuthorListEntry( "ちゃそ", "@marimarikerori" ),
			new AuthorListEntry( "ちょむ" ),
			new AuthorListEntry( "whimsoft" ),
			new AuthorListEntry( "kitiketao", "@okoktaokokta" ),
			new AuthorListEntry( "カプチ２" ),
			new AuthorListEntry( "あにぃ" ),
			new AuthorListEntry( "tomo" ),
			new AuthorListEntry( "ナウ□マP", "@now_romaP" ),
			new AuthorListEntry( "内藤　魅亜", "@mianaito" ),
			new AuthorListEntry( "空茶", "@maizeziam" ),
			new AuthorListEntry( "いぬくま" ),
			new AuthorListEntry( "shu-t", "@shu_sonicwave" ),
			new AuthorListEntry( "さささ", "@sasasa3396" ),
			new AuthorListEntry( "あろも～ら", "@aromora" ),
			new AuthorListEntry( "空耳P", "@soramiku" ),
			new AuthorListEntry( "kotoi" ),
			new AuthorListEntry( "げっぺータロー", "@geppeitaro" ),
			new AuthorListEntry( "みけCAT", "@mikecat_mixc" ),
			new AuthorListEntry( "ぎんじ" ),
			new AuthorListEntry( "BeForU", "@BeForU" ),
			new AuthorListEntry( "all members of Cadencii bbs", 2 ),
			new AuthorListEntry(),
			new AuthorListEntry( "     ... and you !", 3 ),
		};

		static string _(string id)
		{
			return Messaging.getMessage(id);
		}

		readonly FormMain form;

		public FormMainModel (FormMain form)
		{
			this.form = form;
			FormMain = new FormModel (this);
			FileMenu = new FileMenuModel (this);
			EditMenu = new EditMenuModel (this);
			VisualMenu = new VisualMenuModel (this);
			TrackMenu = new TrackMenuModel (this);
			JobMenu = new JobMenuModel (this);
			ScriptMenu = new ScriptMenuModel (this);
			LyricMenu = new LyricMenuModel (this);
			SettingsMenu = new SettingsMenuModel (this);
			HelpMenu = new HelpMenuModel (this);
			HiddenMenu = new HiddenMenuModel (this);
			PianoMenu = new PianoMenuModel (this);
			TrackSelectorMenu = new TrackSelectorMenuModel (this);
			PianoRoll = new PictPianoRollModel (this);
			TrackSelector = new TrackSelectorModel (this);
			PositionIndicator = new PositionIndicatorModel (this);
			WaveView = new WaveViewModel (this);
			ToolBars = new ToolBarsModel (this);
			OtherItems = new OtherItemsModel (this);
			EditorManagerCommands = new EditorManagerCommandsModel (this);
			InputTextBox = new InputTextBoxModel (this);

			form.initializeRendererMenuHandler(this);

			ScaleX = 0.1f;
		}

		public void InitializeControls ()
		{
			EditorManagerCommands.InitializeControls ();
		}

		public FormModel FormMain { get; private set; }
		public FileMenuModel FileMenu { get; private set; }
		public EditMenuModel EditMenu { get; private set; }
		public VisualMenuModel VisualMenu { get; private set; }
		public TrackMenuModel TrackMenu { get; private set; }
		public JobMenuModel JobMenu { get; private set; }
		public ScriptMenuModel ScriptMenu { get; private set; }
		public LyricMenuModel LyricMenu { get; private set; }
		public SettingsMenuModel SettingsMenu { get; private set; }
		public HelpMenuModel HelpMenu { get; private set; }
		public HiddenMenuModel HiddenMenu { get; private set; }
		public PianoMenuModel PianoMenu { get; private set; }
		public TrackSelectorMenuModel TrackSelectorMenu { get; private set; }
		public PictPianoRollModel PianoRoll { get; private set; }
		public TrackSelectorModel TrackSelector { get; private set; }
		public PositionIndicatorModel PositionIndicator { get; private set; }
		public WaveViewModel WaveView { get; private set; }
		public ToolBarsModel ToolBars { get; private set; }
		public OtherItemsModel OtherItems { get; private set; }
		public EditorManagerCommandsModel EditorManagerCommands { get; private set; }
		public InputTextBoxModel InputTextBox { get; private set; }

		#region FormMainController
		/// <summary>
		/// MIDIステップ入力モードがONかどうかを取得します
		/// </summary>
		/// <returns></returns>
		public bool IsStepSequencerEnabled { get; set; }

		/// <summary>
		/// ピアノロールの，Y方向のスケールを取得します(pixel/cent)
		/// </summary>
		/// <returns></returns>
		public float ScaleY {
			get {
				if (EditorManager.editorConfig.PianoRollScaleY < AppConfig.MIN_PIANOROLL_SCALEY) {
					EditorManager.editorConfig.PianoRollScaleY = AppConfig.MIN_PIANOROLL_SCALEY;
				} else if (AppConfig.MAX_PIANOROLL_SCALEY < EditorManager.editorConfig.PianoRollScaleY) {
					EditorManager.editorConfig.PianoRollScaleY = AppConfig.MAX_PIANOROLL_SCALEY;
				}
				if (EditorManager.editorConfig.PianoRollScaleY == 0) {
					return EditorManager.editorConfig.PxTrackHeight / 100.0f;
				} else if (EditorManager.editorConfig.PianoRollScaleY > 0) {
					return (2 * EditorManager.editorConfig.PianoRollScaleY + 5) * EditorManager.editorConfig.PxTrackHeight / 5 / 100.0f;
				} else {
					return (EditorManager.editorConfig.PianoRollScaleY + 8) * EditorManager.editorConfig.PxTrackHeight / 8 / 100.0f;
				}
			}
		}

		/// <summary>
		/// ピアノロールの，X方向のスケールを取得します(pixel/clock)
		/// </summary>
		/// <returns></returns>
		public float ScaleX { get; set; }

		/// <summary>
		/// ピアノロールの，X方向のスケールの逆数を取得します(clock/pixel)
		/// </summary>
		/// <returns></returns>
		public float ScaleXInv {
			get { return 1.0f / ScaleX; }
		}

		/// <summary>
		/// ピアノロール画面の，ビューポートと仮想スクリーンとの横方向のオフセットを取得します
		/// </summary>
		/// <returns></returns>
		public int StartToDrawX { get; set; }

		/// <summary>
		/// ピアノロール画面の，ビューポートと仮想スクリーンとの縦方向のオフセットを取得します
		/// </summary>
		/// <returns></returns>
		public int StartToDrawY { get; set; }
		#endregion

		/// <summary>
		/// 合成器の種類のメニュー項目を管理するハンドラをまとめたリスト
		/// </summary>
		public List<RendererMenuHandler> RendererMenuHandlers;

		public static void TrackSelector_MouseClick (FormMain window, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) {
				if (EditorManager.keyWidth < e.X && e.X < window.TrackSelector.Width) {
					if (window.TrackSelector.Height - TrackSelectorConsts.OFFSET_TRACK_TAB <= e.Y && e.Y <= window.TrackSelector.Height) {
						window.MenuTrackTab.Show (window.TrackSelector, e.X, e.Y);
					} else {
						window.MenuTrackSelector.Show (window.TrackSelector, e.X, e.Y);
					}
				}
			}
		}

		public static int Quantize(int clock, int unit)
		{
			int odd = clock % unit;
			int new_clock = clock - odd;
			if (odd > unit / 2) {
				new_clock += unit;
			}
			return new_clock;
		}

		public Point GetFormPreferedLocation(UiForm dlg)
		{
			return GetFormPreferedLocation(dlg.Width, dlg.Height);
		}

		/// <summary>
		/// 現在の編集データを全て破棄する。DirtyCheckは行われない。
		/// </summary>
		public void ClearExistingData()
		{
			EditorManager.editHistory.clear();
			EditorManager.itemSelection.clearBezier();
			EditorManager.itemSelection.clearEvent();
			EditorManager.itemSelection.clearTempo();
			EditorManager.itemSelection.clearTimesig();
			if (EditorManager.isPlaying()) {
				EditorManager.setPlaying(false, form);
			}
			form.waveView.Model.unloadAll();
		}

		public void ClearTempWave()
		{
			string tmppath = Path.Combine(ApplicationGlobal.getCadenciiTempDir(), ApplicationGlobal.getID());
			if (!Directory.Exists(tmppath)) {
				return;
			}

			// 今回このPCが起動されるよりも以前に，Cadenciiが残したデータを削除する
			//TODO: システムカウンタは約49日でリセットされてしまい，厳密には実装できないようなので，保留．

			// このFormMainのインスタンスが使用したデータを消去する
			for (int i = 1; i <= ApplicationGlobal.MAX_NUM_TRACK; i++) {
				string file = Path.Combine(tmppath, i + ".wav");
				if (System.IO.File.Exists(file)) {
					for (int error = 0; error < 100; error++) {
						try {
							PortUtil.deleteFile(file);
							break;
						} catch (Exception ex) {
							Logger.write(GetType () + ".clearTempWave; ex=" + ex + "\n");
							#if DEBUG
							Cadencii.Media.Windows.MMDebug.push_log("FormMain+ClearTempWave()");
							Cadencii.Media.Windows.MMDebug.push_log("    ex=" + ex.ToString());
							Cadencii.Media.Windows.MMDebug.push_log("    error_count=" + error);
							#endif

							Thread.Sleep(100);
						}
					}
				}
			}
			string whd = Path.Combine(tmppath, UtauWaveGenerator.FILEBASE + ".whd");
			if (System.IO.File.Exists(whd)) {
				try {
					PortUtil.deleteFile(whd);
				} catch (Exception ex) {
					Logger.write(GetType () + ".clearTempWave; ex=" + ex + "\n");
				}
			}
			string dat = Path.Combine(tmppath, UtauWaveGenerator.FILEBASE + ".dat");
			if (System.IO.File.Exists(dat)) {
				try {
					PortUtil.deleteFile(dat);
				} catch (Exception ex) {
					Logger.write(GetType () + ".clearTempWave; ex=" + ex + "\n");
				}
			}
		}

		/// <summary>
		/// 保存されていない編集内容があるかどうかチェックし、必要なら確認ダイアログを出す。
		/// </summary>
		/// <returns>保存されていない保存内容などない場合、または、保存する必要がある場合で（保存しなくてよいと指定された場合または保存が行われた場合）にtrueを返す</returns>
		public bool DirtyCheck()
		{
			if (form.mEdited) {
				string file = MusicManager.getFileName();
				if (file == "") {
					file = "Untitled";
				} else {
					file = PortUtil.getFileName(file);
				}
				var dr = DialogManager.ShowMessageBox(_("Save this sequence?"),
					_("Affirmation"),
					MessageBoxButtons.YesNoCancel,
					MessageBoxIcon.Question);
				if (dr == Cadencii.Gui.DialogResult.Yes) {
					if (MusicManager.getFileName() == "") {
						var dr2 = DialogManager.ShowModalFileDialog(form.saveXmlVsqDialog, false, form);
						if (dr2 == Cadencii.Gui.DialogResult.OK) {
							string sf = form.saveXmlVsqDialog.FileName;
							EditorManager.saveTo(sf);
							return true;
						} else {
							return false;
						}
					} else {
						EditorManager.saveTo(MusicManager.getFileName());
						return true;
					}
				} else if (dr == Cadencii.Gui.DialogResult.No) {
					return true;
				} else {
					return false;
				}
			} else {
				return true;
			}
		}

		/// <summary>
		/// editorConfigのRecentFilesを元に，menuFileRecentのドロップダウンアイテムを更新します
		/// </summary>
		public void UpdateRecentFileMenu()
		{
			int added = 0;
			form.menuFileRecent.DropDownItems.Clear();
			if (EditorManager.editorConfig.RecentFiles != null) {
				for (int i = 0; i < EditorManager.editorConfig.RecentFiles.Count; i++) {
					string item = EditorManager.editorConfig.RecentFiles[i];
					if (item == null) {
						continue;
					}
					if (item != "") {
						string short_name = PortUtil.getFileName(item);
						bool available = System.IO.File.Exists(item);
						var itm = ApplicationUIHost.Create<RecentFileMenuItem> ();
						itm.FilePath = item;
						itm.Text = short_name;
						string tooltip = "";
						if (!available) {
							tooltip = _("[file not found]") + " ";
						}
						tooltip += item;
						itm.ToolTipText = tooltip;
						itm.Enabled = available;
						itm.Click += (o, e) => FileMenu.ShowRecentFileInMenuItem (itm);
						itm.MouseEnter += (o, e) => FileMenu.UpdateStatusBarLabelByRecentFile (itm);
						form.menuFileRecent.DropDownItems.Add(itm);
						added++;
					}
				}
			} else {
				EditorManager.editorConfig.pushRecentFiles("");
			}
			form.menuFileRecent.DropDownItems.Add(ApplicationUIHost.Create<UiToolStripSeparator> ());
			form.menuFileRecent.DropDownItems.Add(form.menuFileRecentClear);
			form.menuFileRecent.Enabled = true;
		}

		/// <summary>
		/// xvsqファイルを開きます
		/// </summary>
		/// <returns>ファイルを開くのに成功した場合trueを，それ以外はfalseを返します</returns>
		public bool OpenVsqCor(string file)
		{
			if (EditorManager.readVsq(file)) {
				return true;
			}
			if (MusicManager.getVsqFile().Track.Count >= 2) {
				form.updateScrollRangeHorizontal();
			}
			EditorManager.editorConfig.pushRecentFiles(file);
			UpdateRecentFileMenu();
			form.setEdited(false);
			EditorManager.editHistory.clear();
			EditorManager.MixerWindow.updateStatus();

			// キャッシュwaveなどの処理
			if (ApplicationGlobal.appConfig.UseProjectCache) {
				#region キャッシュディレクトリの処理
				VsqFileEx vsq = MusicManager.getVsqFile();
				string cacheDir = vsq.cacheDir; // xvsqに保存されていたキャッシュのディレクトリ
				string dir = PortUtil.getDirectoryName(file);
				string name = PortUtil.getFileNameWithoutExtension(file);
				string estimatedCacheDir = Path.Combine(dir, name + ".cadencii"); // ファイル名から推測されるキャッシュディレクトリ
				if (cacheDir == null) {
					cacheDir = "";
				}
				if (cacheDir != "" &&
					Directory.Exists(cacheDir) &&
					estimatedCacheDir != "" &&
					cacheDir != estimatedCacheDir) {
					// ファイル名から推測されるキャッシュディレクトリ名と
					// xvsqに指定されているキャッシュディレクトリと異なる場合
					// cacheDirの必要な部分をestimatedCacheDirに移す

					// estimatedCacheDirが存在しない場合、新しく作る
					#if DEBUG
					Logger.StdOut("FormMain#openVsqCor;fsys.isDirectoryExists( estimatedCacheDir )=" + Directory.Exists(estimatedCacheDir));
					#endif
					if (!Directory.Exists(estimatedCacheDir)) {
						try {
							PortUtil.createDirectory(estimatedCacheDir);
						} catch (Exception ex) {
							Logger.write(GetType () + ".openVsqCor; ex=" + ex + "\n");
							Logger.StdErr("FormMain#openVsqCor; ex=" + ex);
							DialogManager.ShowMessageBox(PortUtil.formatMessage(_("cannot create cache directory: '{0}'"), estimatedCacheDir),
								_("Info."),
								Cadencii.Gui.GuiHost.OK_OPTION,
								MessageBoxIcon.Information);
							return true;
						}
					}

					// ファイルを移す
					for (int i = 1; i < vsq.Track.Count; i++) {
						string wavFrom = Path.Combine(cacheDir, i + ".wav");
						string xmlFrom = Path.Combine(cacheDir, i + ".xml");

						string wavTo = Path.Combine(estimatedCacheDir, i + ".wav");
						string xmlTo = Path.Combine(estimatedCacheDir, i + ".xml");
						if (System.IO.File.Exists(wavFrom)) {
							try {
								PortUtil.moveFile(wavFrom, wavTo);
							} catch (Exception ex) {
								Logger.write(GetType () + ".openVsqCor; ex=" + ex + "\n");
								Logger.StdErr("FormMain#openVsqCor; ex=" + ex);
							}
						}
						if (System.IO.File.Exists(xmlFrom)) {
							try {
								PortUtil.moveFile(xmlFrom, xmlTo);
							} catch (Exception ex) {
								Logger.write(GetType () + ".openVsqCor; ex=" + ex + "\n");
								Logger.StdErr("FormMain#openVsqCor; ex=" + ex);
							}
						}
					}
				}
				cacheDir = estimatedCacheDir;

				// キャッシュが無かったら作成
				if (!Directory.Exists(cacheDir)) {
					try {
						PortUtil.createDirectory(cacheDir);
					} catch (Exception ex) {
						Logger.write(GetType () + ".openVsqCor; ex=" + ex + "\n");
						Logger.StdErr("FormMain#openVsqCor; ex=" + ex);
						DialogManager.ShowMessageBox(PortUtil.formatMessage(_("cannot create cache directory: '{0}'"), estimatedCacheDir),
							_("Info."),
							Cadencii.Gui.GuiHost.OK_OPTION,
							MessageBoxIcon.Information);
						return true;
					}
				}

				// RenderedStatusを読み込む
				for (int i = 1; i < vsq.Track.Count; i++) {
					EditorManager.deserializeRenderingStatus(cacheDir, i);
				}

				// キャッシュ内のwavを、waveViewに読み込む
				form.waveView.Model.unloadAll();
				for (int i = 1; i < vsq.Track.Count; i++) {
					string wav = Path.Combine(cacheDir, i + ".wav");
					#if DEBUG
					Logger.StdOut("FormMain#openVsqCor; wav=" + wav + "; isExists=" + System.IO.File.Exists(wav));
					#endif
					if (!System.IO.File.Exists(wav)) {
						continue;
					}
					form.waveView.Model.load(i - 1, wav);
				}

				// 一時ディレクトリを、cachedirに変更
				ApplicationGlobal.setTempWaveDir(cacheDir);
				#endregion
			}
			return false;
		}

		/// <summary>
		/// 指定した歌手とリサンプラーについて，設定値に登録されていないものだったら登録する．
		/// </summary>
		/// <param name="resampler_path"></param>
		/// <param name="singer_path"></param>
		private void CheckUnknownResamplerAndSinger(ByRef<string> resampler_path, ByRef<string> singer_path)
		{
			string utau = Utility.getExecutingUtau();
			string utau_dir = "";
			if (utau != "") {
				utau_dir = PortUtil.getDirectoryName(utau);
			}

			// 可能なら，VOICEの文字列を置換する
			string search = "%VOICE%";
			if (singer_path.value.StartsWith(search) && singer_path.value.Length > search.Length) {
				singer_path.value = singer_path.value.Substring(search.Length);
				singer_path.value = Path.Combine(Path.Combine(utau_dir, "voice"), singer_path.value);
			}

			// 歌手はknownかunknownか？
			// 歌手指定が知らない歌手だった場合に，ダイアログを出すかどうか
			bool check_unknown_singer = false;
			if (System.IO.File.Exists(Path.Combine(singer_path.value, "oto.ini"))) {
				// oto.iniが存在する場合
				// editorConfigに入っていない場合に，ダイアログを出す
				bool found = false;
				for (int i = 0; i < ApplicationGlobal.appConfig.UtauSingers.Count; i++) {
					SingerConfig sc = ApplicationGlobal.appConfig.UtauSingers[i];
					if (sc == null) {
						continue;
					}
					if (sc.VOICEIDSTR == singer_path.value) {
						found = true;
						break;
					}
				}
				check_unknown_singer = !found;
			}

			// リサンプラーが知っているやつかどうか
			bool check_unknwon_resampler = false;
			#if DEBUG
			Logger.StdOut("FormMain#checkUnknownResamplerAndSinger; resampler_path.value=" + resampler_path.value);
			#endif
			string resampler_dir = PortUtil.getDirectoryName(resampler_path.value);
			if (resampler_dir == "") {
				// ディレクトリが空欄なので，UTAUのデフォルトのリサンプラー指定である
				resampler_path.value = Path.Combine(utau_dir, resampler_path.value);
				resampler_dir = PortUtil.getDirectoryName(resampler_path.value);
			}
			if (resampler_dir != "" && System.IO.File.Exists(resampler_path.value)) {
				bool found = false;
				for (int i = 0; i < ApplicationGlobal.appConfig.getResamplerCount(); i++) {
					string resampler = ApplicationGlobal.appConfig.getResamplerAt(i);
					if (resampler == resampler_path.value) {
						found = true;
						break;
					}
				}
				check_unknwon_resampler = !found;
			}

			// unknownな歌手やリサンプラーが発見された場合.
			// 登録するかどうか問い合わせるダイアログを出す
			FormCheckUnknownSingerAndResampler dialog = null;
			try {
				if (check_unknown_singer || check_unknwon_resampler) {
					dialog = ApplicationUIHost.Create<FormCheckUnknownSingerAndResampler>(singer_path.value, check_unknown_singer, resampler_path.value, check_unknwon_resampler);
					dialog.Location = GetFormPreferedLocation(dialog.Width, dialog.Height);
					var dr = DialogManager.ShowModalDialog(dialog, form);
					if (dr != DialogResult.OK) {
						return;
					}

					// 登録する
					// リサンプラー
					if (dialog.isResamplerChecked()) {
						string path = dialog.getResamplerPath();
						if (System.IO.File.Exists(path)) {
							ApplicationGlobal.appConfig.addResampler(path);
						}
					}
					// 歌手
					if (dialog.isSingerChecked()) {
						string path = dialog.getSingerPath();
						if (Directory.Exists(path)) {
							SingerConfig sc = new SingerConfig();
							Utau.readUtauSingerConfig(path, sc);
							ApplicationGlobal.appConfig.UtauSingers.Add(sc);
						}
						EditorManager.reloadUtauVoiceDB();
					}
				}
			} catch (Exception ex) {
				Logger.write(GetType () + ".checkUnknownResamplerAndSinger; ex=" + ex + "\n");
			} finally {
				if (dialog != null) {
					try {
						dialog.Close();
					} catch (Exception ex2) {
					}
				}
			}
		}

		/// <summary>
		/// フォームをマウス位置に出す場合に推奨されるフォーム位置を計算します
		/// </summary>
		/// <param name="dlg"></param>
		/// <returns></returns>
		public Point GetFormPreferedLocation(int dialogWidth, int dialogHeight)
		{
			Point mouse = Screen.Instance.GetScreenMousePosition();
			Rectangle rcScreen = Screen.Instance.GetWorkingArea(form);
			int top = mouse.Y - dialogHeight / 2;
			if (top + dialogHeight > rcScreen.Y + rcScreen.Height) {
				// ダイアログの下端が隠れる場合、位置をずらす
				top = rcScreen.Y + rcScreen.Height - dialogHeight;
			}
			if (top < rcScreen.Y) {
				// ダイアログの上端が隠れる場合、位置をずらす
				top = rcScreen.Y;
			}
			int left = mouse.X - dialogWidth / 2;
			if (left + dialogWidth > rcScreen.X + rcScreen.Width) {
				// ダイアログの右端が隠れる場合，位置をずらす
				left = rcScreen.X + rcScreen.Width - dialogWidth;
			}
			if (left < rcScreen.X) {
				// ダイアログの左端が隠れる場合，位置をずらす
				left = rcScreen.X;
			}
			return new Point(left, top);
		}

		/// <summary>
		/// VsqEvent, VsqBPList, BezierCurvesの全てのクロックを、tempoに格納されているテンポテーブルに
		/// 合致するようにシフトします．ただし，このメソッド内ではtargetのテンポテーブルは変更せず，クロック値だけが変更される．
		/// </summary>
		/// <param name="work"></param>
		/// <param name="tempo"></param>
		public static void ShiftClockToMatchWith(VsqFileEx target, VsqFile tempo, double shift_seconds)
		{
			// テンポをリプレースする場合。
			// まずクロック値を、リプレース後のモノに置き換え
			for (int track = 1; track < target.Track.Count; track++) {
				// ノート・歌手イベントをシフト
				for (Iterator<VsqEvent> itr = target.Track[track].getEventIterator(); itr.hasNext(); ) {
					VsqEvent item = itr.next();
					if (item.ID.type == VsqIDType.Singer && item.Clock == 0) {
						continue;
					}
					int clock = item.Clock;
					double sec_start = target.getSecFromClock(clock) + shift_seconds;
					double sec_end = target.getSecFromClock(clock + item.ID.getLength()) + shift_seconds;
					int clock_start = (int)tempo.getClockFromSec(sec_start);
					int clock_end = (int)tempo.getClockFromSec(sec_end);
					item.Clock = clock_start;
					item.ID.setLength(clock_end - clock_start);
					if (item.ID.VibratoHandle != null) {
						double sec_vib_start = target.getSecFromClock(clock + item.ID.VibratoDelay) + shift_seconds;
						int clock_vib_start = (int)tempo.getClockFromSec(sec_vib_start);
						item.ID.VibratoDelay = clock_vib_start - clock_start;
						item.ID.VibratoHandle.setLength(clock_end - clock_vib_start);
					}
				}

				// コントロールカーブをシフト
				for (int j = 0; j < BezierCurves.CURVE_USAGE.Length; j++) {
					CurveType ct = BezierCurves.CURVE_USAGE[j];
					VsqBPList item = target.Track[track].getCurve(ct.getName());
					if (item == null) {
						continue;
					}
					VsqBPList repl = new VsqBPList(item.getName(), item.getDefault(), item.getMinimum(), item.getMaximum());
					for (int i = 0; i < item.size(); i++) {
						int clock = item.getKeyClock(i);
						int value = item.getElement(i);
						double sec = target.getSecFromClock(clock) + shift_seconds;
						if (sec >= 0) {
							int clock_new = (int)tempo.getClockFromSec(sec);
							repl.add(clock_new, value);
						}
					}
					target.Track[track].setCurve(ct.getName(), repl);
				}

				// ベジエカーブをシフト
				for (int j = 0; j < BezierCurves.CURVE_USAGE.Length; j++) {
					CurveType ct = BezierCurves.CURVE_USAGE[j];
					List<BezierChain> list = target.AttachedCurves.get(track - 1).get(ct);
					if (list == null) {
						continue;
					}
					foreach (var chain in list) {
						foreach (var point in chain.points) {
							PointD bse = new PointD(tempo.getClockFromSec(target.getSecFromClock(point.getBase().getX()) + shift_seconds),
								point.getBase().getY());
							double rx = point.getBase().getX() + point.controlRight.getX();
							double new_rx = tempo.getClockFromSec(target.getSecFromClock(rx) + shift_seconds);
							PointD ctrl_r = new PointD(new_rx - bse.getX(), point.controlRight.getY());

							double lx = point.getBase().getX() + point.controlLeft.getX();
							double new_lx = tempo.getClockFromSec(target.getSecFromClock(lx) + shift_seconds);
							PointD ctrl_l = new PointD(new_lx - bse.getX(), point.controlLeft.getY());
							point.setBase(bse);
							point.controlLeft = ctrl_l;
							point.controlRight = ctrl_r;
						}
					}
				}
			}
		}

		/// <summary>
		/// 選択されている音符の表情を編集するためのダイアログを起動し、編集を行います。
		/// </summary>
		public void EditNoteExpressionProperty()
		{
			SelectedEventEntry item = EditorManager.itemSelection.getLastEvent();
			if (item == null) {
				return;
			}

			VsqEvent ev = item.original;
			SynthesizerType type = SynthesizerType.VOCALOID2;
			int selected = EditorManager.Selected;
			VsqFileEx vsq = MusicManager.getVsqFile();
			RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
			if (kind == RendererKind.VOCALOID1) {
				type = SynthesizerType.VOCALOID1;
			}
			FormNoteExpressionConfig dlg = null;
			try {
				dlg = ApplicationUIHost.Create<FormNoteExpressionConfig>(type, ev.ID.NoteHeadHandle);
				dlg.PMBendDepth = (ev.ID.PMBendDepth);
				dlg.PMBendLength = (ev.ID.PMBendLength);
				dlg.PMbPortamentoUse = (ev.ID.PMbPortamentoUse);
				dlg.DEMdecGainRate = (ev.ID.DEMdecGainRate);
				dlg.DEMaccent = (ev.ID.DEMaccent);

				dlg.Location = GetFormPreferedLocation(dlg.Width, dlg.Height);
				var dr = DialogManager.ShowModalDialog(dlg, form);
				if (dr == DialogResult.OK) {
					VsqEvent edited = (VsqEvent)ev.clone();
					edited.ID.PMBendDepth = dlg.PMBendDepth;
					edited.ID.PMBendLength = dlg.PMBendLength;
					edited.ID.PMbPortamentoUse = dlg.PMbPortamentoUse;
					edited.ID.DEMdecGainRate = dlg.DEMdecGainRate;
					edited.ID.DEMaccent = dlg.DEMaccent;
					edited.ID.NoteHeadHandle = dlg.EditedNoteHeadHandle;
					CadenciiCommand run = new CadenciiCommand(
						VsqCommand.generateCommandEventChangeIDContaints(selected, ev.InternalID, edited.ID));
					EditorManager.editHistory.register(vsq.executeCommand(run));
					form.setEdited(true);
					form.refreshScreen();
				}
			} catch (Exception ex) {
				Logger.write(GetType () + ".editNoteExpressionProperty; ex=" + ex + "\n");
			} finally {
				if (dlg != null) {
					try {
						dlg.Close();
					} catch (Exception ex2) {
						Logger.write(GetType () + ".editNoteExpressionProperty; ex=" + ex2 + "\n");
					}
				}
			}
		}

		/// <summary>
		/// 選択されている音符のビブラートを編集するためのダイアログを起動し、編集を行います。
		/// </summary>
		public void EditNoteVibratoProperty()
		{
			SelectedEventEntry item = EditorManager.itemSelection.getLastEvent();
			if (item == null) {
				return;
			}

			VsqEvent ev = item.original;
			int selected = EditorManager.Selected;
			VsqFileEx vsq = MusicManager.getVsqFile();
			RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
			SynthesizerType type = SynthesizerType.VOCALOID2;
			if (kind == RendererKind.VOCALOID1) {
				type = SynthesizerType.VOCALOID1;
			}
			FormVibratoConfig dlg = null;
			try {
				dlg = ApplicationUIHost.Create<FormVibratoConfig>(
					ev.ID.VibratoHandle,
					ev.ID.getLength(),
					ApplicationGlobal.appConfig.DefaultVibratoLength,
					type,
					ApplicationGlobal.appConfig.UseUserDefinedAutoVibratoType);
				dlg.Location = GetFormPreferedLocation(dlg);
				var dr = DialogManager.ShowModalDialog(dlg, form);
				if (dr == DialogResult.OK) {
					VsqEvent edited = (VsqEvent)ev.clone();
					if (dlg.getVibratoHandle() != null) {
						edited.ID.VibratoHandle = (VibratoHandle)dlg.getVibratoHandle().clone();
						//edited.ID.VibratoHandle.setStartDepth( ApplicationGlobal.appConfig.DefaultVibratoDepth );
						//edited.ID.VibratoHandle.setStartRate( ApplicationGlobal.appConfig.DefaultVibratoRate );
						edited.ID.VibratoDelay = ev.ID.getLength() - dlg.getVibratoHandle().getLength();
					} else {
						edited.ID.VibratoHandle = null;
					}
					CadenciiCommand run = new CadenciiCommand(
						VsqCommand.generateCommandEventChangeIDContaints(selected, ev.InternalID, edited.ID));
					EditorManager.editHistory.register(vsq.executeCommand(run));
					form.setEdited(true);
					form.refreshScreen();
				}
			} catch (Exception ex) {
				Logger.write(GetType () + ".editNoteVibratoProperty; ex=" + ex + "\n");
			} finally {
				if (dlg != null) {
					try {
						dlg.Close();
					} catch (Exception ex2) {
						Logger.write(GetType () + ".editNoteVibratoProperty; ex=" + ex2 + "\n");
					}
				}
			}
		}

		/// <summary>
		/// ミキサーダイアログの表示・非表示状態を更新します
		/// </summary>
		/// <param name="visible">表示状態にする場合true，そうでなければfalse</param>
		public void FlipMixerDialogVisible(bool visible)
		{
			EditorManager.MixerWindow.Visible = visible;
			EditorManager.editorConfig.MixerVisible = visible;
			if (visible != form.menuVisualMixer.Checked) {
				form.menuVisualMixer.Checked = visible;
			}
		}

		/// <summary>
		/// アイコンパレットの表示・非表示状態を更新します
		/// </summary>
		public void FlipIconPaletteVisible(bool visible)
		{
			EditorManager.iconPalette.Visible = visible;
			EditorManager.editorConfig.IconPaletteVisible = visible;
			if (visible != form.menuVisualIconPalette.Checked) {
				form.menuVisualIconPalette.Checked = visible;
			}
		}

		/// <summary>
		/// コントロールトラックの表示・非表示状態を更新します
		/// </summary>
		public void FlipControlCurveVisible(bool visible)
		{
			form.TrackSelector.Model.CurveVisible = visible;
			if (visible) {
				form.splitContainer1.SplitterFixed = (false);
				form.splitContainer1.DividerSize = (FormMainModel.Consts._SPL_SPLITTER_WIDTH);
				form.splitContainer1.DividerLocation = (form.splitContainer1.Height - EditorManager.mLastTrackSelectorHeight - form.splitContainer1.DividerSize);
				form.splitContainer1.Panel2MinSize = (form.TrackSelector.Model.PreferredMinSize);
			} else {
				EditorManager.mLastTrackSelectorHeight = form.splitContainer1.Height - form.splitContainer1.DividerLocation - form.splitContainer1.DividerSize;
				form.splitContainer1.SplitterFixed = (true);
				form.splitContainer1.DividerSize = (0);
				int panel2height = TrackSelectorConsts.OFFSET_TRACK_TAB * 2;
				form.splitContainer1.DividerLocation = (form.splitContainer1.Height - panel2height - form.splitContainer1.DividerSize);
				form.splitContainer1.Panel2MinSize = (panel2height);
			}
			form.refreshScreen();
		}

		/// <summary>
		/// 歌詞の流し込みダイアログを開き，選択された音符を起点に歌詞を流し込みます
		/// </summary>
		public void ImportLyric()
		{
			int start = 0;
			int selected = EditorManager.Selected;
			VsqFileEx vsq = MusicManager.getVsqFile();
			VsqTrack vsq_track = vsq.Track[selected];
			int selectedid = EditorManager.itemSelection.getLastEvent().original.InternalID;
			int numEvents = vsq_track.getEventCount();
			for (int i = 0; i < numEvents; i++) {
				if (selectedid == vsq_track.getEvent(i).InternalID) {
					start = i;
					break;
				}
			}
			int count = vsq_track.getEventCount() - 1 - start + 1;
			try {
				if (form.mDialogImportLyric == null) {
					form.mDialogImportLyric = ApplicationUIHost.Create<FormImportLyric>(count);
				} else {
					form.mDialogImportLyric.setMaxNotes (count);
				}
				var dlg = form.mDialogImportLyric;
				dlg.Location = GetFormPreferedLocation(dlg);
				var dr = DialogManager.ShowModalDialog(dlg, form);
				if (dr == DialogResult.OK) {
					string[] phrases = dlg.Letters;
					#if DEBUG
					foreach (string s in phrases) {
						Logger.StdOut("FormMain#importLyric; phrases; s=" + s);
					}
					#endif
					int min = Math.Min(count, phrases.Length);
					List<string> new_phrases = new List<string>();
					List<string> new_symbols = new List<string>();
					for (int i = 0; i < phrases.Length; i++) {
						SymbolTableEntry entry = SymbolTable.attatch(phrases[i]);
						if (new_phrases.Count + 1 > count) {
							break;
						}
						if (entry == null) {
							new_phrases.Add(phrases[i]);
							new_symbols.Add("a");
						} else {
							if (entry.Word.IndexOf('-') >= 0) {
								// 分節に分割する必要がある
								string[] spl = PortUtil.splitString(entry.Word, '\t');
								if (new_phrases.Count + spl.Length > count) {
									// 分節の全部を分割すると制限個数を超えてしまう
									// 分割せずにハイフンを付けたまま登録
									new_phrases.Add(entry.Word.Replace("\t", ""));
									new_symbols.Add(entry.getSymbol());
								} else {
									string[] spl_symbol = PortUtil.splitString(entry.getRawSymbol(), '\t');
									for (int j = 0; j < spl.Length; j++) {
										new_phrases.Add(spl[j]);
										new_symbols.Add(spl_symbol[j]);
									}
								}
							} else {
								// 分節に分割しない
								new_phrases.Add(phrases[i]);
								new_symbols.Add(entry.getSymbol());
							}
						}
					}
					VsqEvent[] new_events = new VsqEvent[new_phrases.Count];
					int indx = -1;
					for (Iterator<int> itr = vsq_track.indexIterator(IndexIteratorKind.NOTE); itr.hasNext(); ) {
						int index = itr.next();
						if (index < start) {
							continue;
						}
						indx++;
						VsqEvent item = vsq_track.getEvent(index);
						new_events[indx] = (VsqEvent)item.clone();
						new_events[indx].ID.LyricHandle.L0.Phrase = new_phrases[indx];
						new_events[indx].ID.LyricHandle.L0.setPhoneticSymbol(new_symbols[indx]);
						MusicManager.applyUtauParameter(vsq_track, new_events[indx]);
						if (indx + 1 >= new_phrases.Count) {
							break;
						}
					}
					CadenciiCommand run = new CadenciiCommand(
						VsqCommand.generateCommandEventReplaceRange(selected, new_events));
					EditorManager.editHistory.register(vsq.executeCommand(run));
					form.setEdited(true);
					form.Refresh();
				}
			} catch (Exception ex) {
				Logger.write(GetType () + ".importLyric; ex=" + ex + "\n");
			} finally {
				form.mDialogImportLyric.Hide();
			}
		}


		public void HandlePositionQuantize(QuantizeMode? quantizeMode)
		{
			QuantizeMode qm = quantizeMode ?? EditorManager.editorConfig.getPositionQuantize();
			EditorManager.editorConfig.setPositionQuantize(qm);
			EditorManager.editorConfig.setLengthQuantize(qm);
			form.refreshScreen();
		}

		public void HandlePositionQuantizeTriplet ()
		{
			bool triplet = !EditorManager.editorConfig.isPositionQuantizeTriplet();
			EditorManager.editorConfig.setPositionQuantizeTriplet(triplet);
			EditorManager.editorConfig.setLengthQuantizeTriplet(triplet);
			form.refreshScreen();
		}

		#region トラックの編集関連
		/// <summary>
		/// トラック全体のコピーを行います。
		/// </summary>
		public void CopyTrack()
		{
			VsqFileEx vsq = MusicManager.getVsqFile();
			int selected = EditorManager.Selected;
			VsqTrack track = (VsqTrack)vsq.Track[selected].clone();
			track.setName(track.getName() + " (1)");
			CadenciiCommand run = VsqFileEx.generateCommandAddTrack(track,
				vsq.Mixer.Slave[selected - 1],
				vsq.Track.Count,
				vsq.AttachedCurves.get(selected - 1)); ;
			EditorManager.editHistory.register(vsq.executeCommand(run));
			form.setEdited(true);
			EditorManager.MixerWindow.updateStatus();
			form.refreshScreen();
		}

		/// <summary>
		/// トラックの名前変更を行います。
		/// </summary>
		public void ChangeTrackName()
		{
			InputBox ib = null;
			try {
				int selected = EditorManager.Selected;
				VsqFileEx vsq = MusicManager.getVsqFile();
				ib = ApplicationUIHost.Create<InputBox>(_("Input new name of track"));
				ib.Result = (vsq.Track[selected].getName());
				ib.Location = GetFormPreferedLocation(ib);
				var dr = DialogManager.ShowModalDialog(ib, form);
				if (dr == DialogResult.OK) {
					string ret = ib.Result;
					CadenciiCommand run = new CadenciiCommand(
						VsqCommand.generateCommandTrackChangeName(selected, ret));
					EditorManager.editHistory.register(vsq.executeCommand(run));
					form.setEdited(true);
					form.refreshScreen();
				}
			} catch (Exception ex) {
			} finally {
				if (ib != null) {
					ib.Close();
				}
			}
		}

		/// <summary>
		/// トラックの削除を行います。
		/// </summary>
		public void DeleteTrack()
		{
			int selected = EditorManager.Selected;
			VsqFileEx vsq = MusicManager.getVsqFile();
			if (DialogManager.ShowMessageBox(
				PortUtil.formatMessage(_("Do you wish to remove track? {0} : '{1}'"), selected, vsq.Track[selected].getName()),
				FormMainModel.Consts.ApplicationName,
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question) == Cadencii.Gui.DialogResult.Yes) {
				CadenciiCommand run = VsqFileEx.generateCommandDeleteTrack(selected);
				if (selected >= 2) {
					EditorManager.Selected = selected - 1;
				}
				EditorManager.editHistory.register(vsq.executeCommand(run));
				form.updateDrawObjectList();
				form.setEdited(true);
				EditorManager.MixerWindow.updateStatus();
				form.refreshScreen();
			}
		}

		/// <summary>
		/// トラックの追加を行います。
		/// </summary>
		public void AddTrack()
		{
			VsqFileEx vsq = MusicManager.getVsqFile();
			int i = vsq.Track.Count;
			string name = "Voice" + i;
			string singer = ApplicationGlobal.appConfig.DefaultSingerName;
			VsqTrack vsq_track = new VsqTrack(name, singer);

			RendererKind kind = ApplicationGlobal.appConfig.DefaultSynthesizer;
			string renderer = kind.getVersionString();
			List<VsqID> singers = MusicManager.getSingerListFromRendererKind(kind);

			vsq_track.changeRenderer(renderer, singers);
			CadenciiCommand run = VsqFileEx.generateCommandAddTrack(vsq_track,
				new VsqMixerEntry(0, 0, 0, 0),
				i,
				new BezierCurves());
			EditorManager.editHistory.register(vsq.executeCommand(run));
			form.updateDrawObjectList();
			form.setEdited(true);
			EditorManager.Selected = (i);
			EditorManager.MixerWindow.updateStatus();
			form.refreshScreen();
		}
		#endregion


		#if ENABLE_SCRIPT
		/// <summary>
		/// スクリプトフォルダ中のスクリプトへのショートカットを作成する
		/// </summary>
		public void UpdateScriptShortcut()
		{
			// 既存のアイテムを削除
			form.menuScript.DropDownItems.Clear();
			// スクリプトをリロード
			ScriptServer.reload();

			// スクリプトごとのメニューを追加
			int count = 0;
			foreach (var id in ScriptServer.getScriptIdIterator()) {
				if (PortUtil.getFileNameWithoutExtension(id).ToLower() == "runonce") {
					continue;
				}
				string display = ScriptServer.getDisplayName(id);
				// スクリプトのメニューに共通のヘッダー(menuScript)を付ける．
				// こうしておくと，menuSettingShortcut_Clickで，スクリプトのメニューが
				// menuScriptの子だと自動で認識される
				string name = "menuScript" + id.Replace('.', '_');
				PaletteToolMenuItem item = ApplicationUIHost.Create<PaletteToolMenuItem> ();
				item.PaletteToolId = id;
				item.Text = display;
				item.Name = name;
				item.Click += (o, e) => ScriptMenu.RunScriptMenuItemCommand (item.PaletteToolId);
				form.menuScript.DropDownItems.Add(item);
				count++;
			}

			// 「スクリプトのリストを更新」を追加
			if (count > 0) {
				form.menuScript.DropDownItems.Add(ApplicationUIHost.Create<UiToolStripSeparator> ());
			}
			form.menuScript.DropDownItems.Add(form.menuScriptUpdate);
			Utility.applyToolStripFontRecurse(form.menuScript, EditorManager.editorConfig.getBaseFont());
			form.applyShortcut();
		}
		#endif

		/// <summary>
		/// 現在選択されている音符よりも1個前方の音符を選択しなおします。
		/// </summary>
		public void SelectBackward()
		{
			int count = EditorManager.itemSelection.getEventCount();
			if (count <= 0) {
				return;
			}
			VsqFileEx vsq = MusicManager.getVsqFile();
			if (vsq == null) {
				return;
			}
			int selected = EditorManager.Selected;
			VsqTrack vsq_track = vsq.Track[selected];

			// 選択されている音符のうち、最も前方にあるものがどれかを調べる
			int min_clock = int.MaxValue;
			int internal_id = -1;
			VsqIDType type = VsqIDType.Unknown;
			foreach (var item in EditorManager.itemSelection.getEventIterator()) {
				if (item.editing.Clock <= min_clock) {
					min_clock = item.editing.Clock;
					internal_id = item.original.InternalID;
					type = item.original.ID.type;
				}
			}
			if (internal_id == -1 || type == VsqIDType.Unknown) {
				return;
			}

			// 1個前のアイテムのIDを検索
			int last_id = -1;
			int clock = EditorManager.getCurrentClock();
			for (Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
				VsqEvent item = itr.next();
				if (item.ID.type != type) {
					continue;
				}
				if (item.InternalID == internal_id) {
					break;
				}
				last_id = item.InternalID;
				clock = item.Clock;
			}
			if (last_id == -1) {
				return;
			}

			// 選択しなおす
			EditorManager.itemSelection.clearEvent();
			EditorManager.itemSelection.addEvent(last_id);
			EnsureClockVisibleOnPianoRoll(clock);
		}

		/// <summary>
		/// 現在選択されている音符よりも1個後方の音符を選択しなおします。
		/// </summary>
		public void SelectForward()
		{
			int count = EditorManager.itemSelection.getEventCount();
			if (count <= 0) {
				return;
			}
			VsqFileEx vsq = MusicManager.getVsqFile();
			if (vsq == null) {
				return;
			}
			int selected = EditorManager.Selected;
			VsqTrack vsq_track = vsq.Track[selected];

			// 選択されている音符のうち、最も後方にあるものがどれかを調べる
			int max_clock = int.MinValue;
			int internal_id = -1;
			VsqIDType type = VsqIDType.Unknown;
			foreach (var item in EditorManager.itemSelection.getEventIterator()) {
				if (max_clock <= item.editing.Clock) {
					max_clock = item.editing.Clock;
					internal_id = item.original.InternalID;
					type = item.original.ID.type;
				}
			}
			if (internal_id == -1 || type == VsqIDType.Unknown) {
				return;
			}

			// 1個後ろのアイテムのIDを検索
			int last_id = -1;
			int clock = EditorManager.getCurrentClock();
			bool break_next = false;
			for (Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
				VsqEvent item = itr.next();
				if (item.ID.type != type) {
					continue;
				}
				if (item.InternalID == internal_id) {
					break_next = true;
					last_id = item.InternalID;
					clock = item.Clock;
					continue;
				}
				last_id = item.InternalID;
				clock = item.Clock;
				if (break_next) {
					break;
				}
			}
			if (last_id == -1) {
				return;
			}

			// 選択しなおす
			EditorManager.itemSelection.clearEvent();
			EditorManager.itemSelection.addEvent(last_id);
			EnsureClockVisibleOnPianoRoll(clock);
		}

		/// <summary>
		/// 歌詞入力用テキストボックスのモード（歌詞/発音記号）を切り替えます
		/// </summary>
		public void FlipInputTextBoxMode()
		{
			string new_value = EditorManager.InputTextBox.Text;
			if (!EditorManager.InputTextBox.IsPhoneticSymbolEditMode) {
				EditorManager.InputTextBox.BackColor = ColorTextboxBackcolor;
			} else {
				EditorManager.InputTextBox.BackColor = Colors.White;
			}
			EditorManager.InputTextBox.Text = EditorManager.InputTextBox.BufferText;
			EditorManager.InputTextBox.BufferText = new_value;
			EditorManager.InputTextBox.IsPhoneticSymbolEditMode = (!EditorManager.InputTextBox.IsPhoneticSymbolEditMode);
		}

		/// <summary>
		/// 選択された音符の長さを、指定したゲートタイム分長くします。
		/// </summary>
		/// <param name="delta_length"></param>
		public void LengthenSelectedEvent(int delta_length)
		{
			if (delta_length == 0) {
				return;
			}

			VsqFileEx vsq = MusicManager.getVsqFile();
			if (vsq == null) {
				return;
			}

			int selected = EditorManager.Selected;

			List<VsqEvent> items = new List<VsqEvent>();
			foreach (var item in EditorManager.itemSelection.getEventIterator()) {
				if (item.editing.ID.type != VsqIDType.Anote &&
					item.editing.ID.type != VsqIDType.Aicon) {
					continue;
				}

				// クレッシェンド、デクレッシェンドでないものを省く
				if (item.editing.ID.type == VsqIDType.Aicon) {
					if (item.editing.ID.IconDynamicsHandle == null) {
						continue;
					}
					if (!item.editing.ID.IconDynamicsHandle.isCrescendType() &&
						!item.editing.ID.IconDynamicsHandle.isDecrescendType()) {
						continue;
					}
				}

				// 長さを変える。0未満になると0に直す
				int length = item.editing.ID.getLength();
				int draft = length + delta_length;
				if (draft < 0) {
					draft = 0;
				}
				if (length == draft) {
					continue;
				}

				// ビブラートの長さを変更
				VsqEvent add = (VsqEvent)item.editing.clone();
				EditorManager.editLengthOfVsqEvent(add, draft, EditorManager.vibratoLengthEditingRule);
				items.Add(add);
			}

			if (items.Count <= 0) {
				return;
			}

			// コマンドを発行
			CadenciiCommand run = new CadenciiCommand(
				VsqCommand.generateCommandEventReplaceRange(
					selected, items.ToArray()));
			EditorManager.editHistory.register(vsq.executeCommand(run));

			// 編集されたものを再選択する
			foreach (var item in items) {
				EditorManager.itemSelection.addEvent(item.InternalID);
			}

			// 編集が施された。
			form.setEdited(true);
			form.updateDrawObjectList();

			form.refreshScreen();
		}

		bool is_space_key_down = false;

		/// <summary>
		/// マウスの真ん中ボタンが押されたかどうかを調べます。
		/// スペースキー+左ボタンで真ん中ボタンとみなすかどうか、というオプションも考慮される。
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool IsMouseMiddleButtonDown(MouseButtons button)
		{
			bool ret = false;
			if (EditorManager.editorConfig.UseSpaceKeyAsMiddleButtonModifier) {
				if (is_space_key_down && button == MouseButtons.Left) {
					ret = true;
				}
			} else {
				if (button == MouseButtons.Middle) {
					ret = true;
				}
			}
			return ret;
		}

		public void HandleSpaceKeyDown(KeyEventArgs e)
		{
			if (((Keys) e.KeyCode & Keys.Space) == Keys.Space) {
				is_space_key_down = true;
			}
		}

		public void HandleSpaceKeyUp(KeyEventArgs e)
		{
			if (((Keys) e.KeyCode & Keys.Space) == Keys.Space) {
				is_space_key_down = false;
			}
		}

		#region ensure visibility on piano roll

		/// <summary>
		/// 指定したノートナンバーが可視状態となるよう、縦スクロールバーを移動させます。
		/// </summary>
		/// <param name="note"></param>
		public void EnsureNoteVisibleOnPianoRoll(int note)
		{
			var vScroll = form.vScroll;

			Action<int> setVScrollValue = (value) => {
				int draft = Math.Min(Math.Max(value, vScroll.Minimum), vScroll.Maximum);
				vScroll.Value = draft;
			};
			if (note <= 0) {
				setVScrollValue(vScroll.Maximum - vScroll.LargeChange);
				return;
			} else if (note >= 127) {
				vScroll.Value = vScroll.Minimum;
				return;
			}
			int height = form.pictPianoRoll.Height;
			int noteTop = EditorManager.noteFromYCoord(0); //画面上端でのノートナンバー
			int noteBottom = EditorManager.noteFromYCoord(height); // 画面下端でのノートナンバー

			int maximum = vScroll.Maximum;
			int track_height = (int)(100 * ScaleY);
			// ノートナンバーnoteの現在のy座標がいくらか？
			int note_y = EditorManager.yCoordFromNote(note);
			if (note < noteBottom) {
				// ノートナンバーnoteBottomの現在のy座標が新しいnoteのy座標と同一になるよう，startToDrawYを変える
				// startToDrawYを次の値にする必要がある
				int new_start_to_draw_y = StartToDrawY + (note_y - height);
				int value = CalculateVScrollValueFromStartToDrawY(new_start_to_draw_y);
				setVScrollValue(value);
			} else if (noteTop < note) {
				// ノートナンバーnoteTopの現在のy座標が，ノートナンバーnoteの新しいy座標と同一になるよう，startToDrawYを変える
				int new_start_to_draw_y = StartToDrawY + (note_y - 0);
				int value = CalculateVScrollValueFromStartToDrawY(new_start_to_draw_y);
				setVScrollValue(value);
			}
		}

		/// <summary>
		/// 仮想スクリーン上でみた時の，現在のピアノロール画面の上端のy座標が指定した値とするための，vScrollの値を計算します
		/// calculateStartToDrawYの逆関数です
		/// </summary>
		int CalculateVScrollValueFromStartToDrawY(int start_to_draw_y)
		{
			return (int)(start_to_draw_y / ScaleY);
		}

		/// <summary>
		/// 指定したゲートタイムがピアノロール上で可視状態となるよう、横スクロールバーを移動させます。
		/// </summary>
		/// <param name="clock"></param>
		public void EnsureClockVisibleOnPianoRoll(int clock)
		{
			var hScroll = form.hScroll;

			// カーソルが画面内にあるかどうか検査
			int clock_left = EditorManager.clockFromXCoord(EditorManager.keyWidth);
			int clock_right = EditorManager.clockFromXCoord(form.pictPianoRoll.Width);
			int uwidth = clock_right - clock_left;
			if (clock < clock_left || clock_right < clock) {
				int cl_new_center = (clock / uwidth) * uwidth + uwidth / 2;
				float f_draft = cl_new_center - (form.pictPianoRoll.Width / 2 + 34 - 70) * ScaleXInv;
				if (f_draft < 0f) {
					f_draft = 0;
				}
				int draft = (int)(f_draft);
				if (draft < hScroll.Minimum) {
					draft = hScroll.Minimum;
				} else if (hScroll.Maximum < draft) {
					draft = hScroll.Maximum;
				}
				if (hScroll.Value != draft) {
					EditorManager.mDrawStartIndex[EditorManager.Selected - 1] = 0;
					hScroll.Value = draft;
				}
			}
		}

		/// <summary>
		/// プレイカーソルが見えるようスクロールする
		/// </summary>
		public void EnsurePlayerCursorVisible()
		{
			EnsureClockVisibleOnPianoRoll(EditorManager.getCurrentClock());
		}

		#endregion

		public void RunStripPaletteToolSelected(string id, Action selectTarget)
		{
			#if ENABLE_SCRIPT
			EditorManager.mSelectedPaletteTool = id;
			EditorManager.SelectedTool = (EditTool.PALETTE_TOOL);
			selectTarget ();
			#endif

			int count = form.toolBarTool.Buttons.Count;
			for (int i = 0; i < count; i++) {
				Object item = form.toolBarTool.Buttons[i];
				if (item is UiToolBarButton) {
					UiToolBarButton button = (UiToolBarButton)item;
					if (button.Style == ToolBarButtonStyle.ToggleButton && button.Tag != null && button.Tag is string) {
						if (((string)button.Tag).Equals(id)) {
							button.Pushed = true;
						} else {
							button.Pushed = false;
						}
					}
				}
			}

			foreach (var item in form.cMenuPianoPaletteTool.DropDownItems) {
				if (item is PaletteToolMenuItem) {
					PaletteToolMenuItem menu = (PaletteToolMenuItem)item;
					string tagged_id = menu.PaletteToolId;
					menu.Checked = (id == tagged_id);
				}
			}

			//MenuElement[] sub_cmenu_track_selectro_palette_tool = cMenuTrackSelectorPaletteTool.getSubElements();
			//for ( int i = 0; i < sub_cmenu_track_selectro_palette_tool.Length; i++ ) {
			//MenuElement item = sub_cmenu_track_selectro_palette_tool[i];
			foreach (var item in form.cMenuTrackSelectorPaletteTool.DropDownItems) {
				if (item is PaletteToolMenuItem) {
					PaletteToolMenuItem menu = (PaletteToolMenuItem)item;
					string tagged_id = menu.PaletteToolId;
					menu.Checked = (id == tagged_id);
				}
			}
		}

		/// <summary>
		/// ピアノロールの縦軸の拡大率をdelta段階上げます
		/// </summary>
		/// <param name="delta"></param>
		public void ZoomPianoRollHeight(int delta)
		{
			int scaley = EditorManager.editorConfig.PianoRollScaleY;
			int draft = scaley + delta;
			if (draft < AppConfig.MIN_PIANOROLL_SCALEY) {
				draft = AppConfig.MIN_PIANOROLL_SCALEY;
			}
			if (AppConfig.MAX_PIANOROLL_SCALEY < draft) {
				draft = AppConfig.MAX_PIANOROLL_SCALEY;
			}
			if (scaley != draft) {
				EditorManager.editorConfig.PianoRollScaleY = draft;
				form.updateScrollRangeVertical();
				StartToDrawY = (form.calculateStartToDrawY(form.vScroll.Value));
				form.updateDrawObjectList();
			}
		}
		/// <summary>
		/// EditorManager.editorConfig.ViewWaveformの値をもとに、splitterContainer2の表示状態を更新します
		/// </summary>
		public void updateSplitContainer2Size(bool save_to_config)
		{
			var splitContainer2 = form.splitContainer2;
			if (ApplicationGlobal.appConfig.ViewWaveform) {
				splitContainer2.Panel2MinSize = (Consts._SPL2_PANEL2_MIN_HEIGHT);
				splitContainer2.SplitterFixed = (false);
				splitContainer2.Panel2Hidden = (false);
				splitContainer2.DividerSize = (Consts._SPL_SPLITTER_WIDTH);
				int lastloc = EditorManager.editorConfig.SplitContainer2LastDividerLocation;
				if (lastloc <= 0 || lastloc > splitContainer2.Height) {
					int draft = splitContainer2.Height- 100;
					if (draft <= 0) {
						draft = splitContainer2.Height/ 2;
					}
					splitContainer2.DividerLocation = (draft);
				} else {
					splitContainer2.DividerLocation = (lastloc);
				}
			} else {
				if (save_to_config) {
					EditorManager.editorConfig.SplitContainer2LastDividerLocation = splitContainer2.DividerLocation;
				}
				splitContainer2.Panel2MinSize = (0);
				splitContainer2.Panel2Hidden = (true);
				splitContainer2.DividerSize = (0);
				splitContainer2.DividerLocation = (splitContainer2.Height);
				splitContainer2.SplitterFixed = (true);
			}
		}

		/// <summary>
		/// スクリーンに対して、ウィンドウの最適な位置を取得する
		/// </summary>
		public static Point GetAppropriateDialogLocation(int x, int y, int width, int height, int workingAreaX, int workingAreaY, int workingAreaWidth, int workingAreaHeight)
		{
			int top = y;
			if (top + height > workingAreaY + workingAreaHeight) {
				// ダイアログの下端が隠れる場合、位置をずらす
				top = workingAreaY + workingAreaHeight - height;
			}
			if (top < workingAreaY) {
				// ダイアログの上端が隠れる場合、位置をずらす
				top = workingAreaY;
			}
			int left = x;
			if (left + width > workingAreaX + workingAreaWidth) {
				left = workingAreaX + workingAreaWidth - width;
			}
			if (left < workingAreaX) {
				left = workingAreaX;
			}
			return new Point(left, top);
		}

		/// <summary>
		/// ズームスライダの現在の値から，横方向の拡大率を計算します
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public float GetScaleXFromTrackBarValue(int value)
		{
			return value / 480.0f;
		}
		/// <summary>
		/// メインメニュー項目の中から，Nameプロパティがnameであるものを検索します．見つからなければnullを返す．
		/// </summary>
		/// <param name="name"></param>
		/// <param name="parent"></param>
		/// <returns></returns>
		public UiToolStripItem SearchMenuItemFromName(UiMenuStrip menuStrip, string name, ByRef<Object> parent)
		{
			int count = menuStrip.Items.Count;
			for (int i = 0; i < count; i++) {
				var tsi = menuStrip.Items[i];
				var ret = SearchMenuItemRecurse(name, tsi, parent);
				if (ret != null) {
					if (parent.value == null) {
						parent.value = tsi;
					}
					return ret;
				}
			}
			return null;
		}

		/// <summary>
		/// 指定されたメニューアイテムから，Nameプロパティがnameであるものを再帰的に検索します．見つからなければnullを返す
		/// </summary>
		/// <param name="name"></param>
		/// <param name="tree"></param>
		/// <returns></returns>
		UiToolStripItem SearchMenuItemRecurse(string name, UiToolStripItem tree, ByRef<Object> parent)
		{
			string tree_name = "";
			UiToolStripMenuItem menu = null;
			if (tree is UiToolStripMenuItem) {
				menu = (UiToolStripMenuItem)tree;
			}
			tree_name = tree.Name;

			if (tree_name == name) {
				parent.value = null;
				return tree;
			} else {
				if (menu == null) {
					return null;
				}
				int count = menu.DropDownItems.Count;
				for (int i = 0; i < count; i++) {
					UiToolStripItem tsi = menu.DropDownItems[i];
					string tsi_name = "";
					if (tsi is UiToolStripItem) {
						tsi_name = ((UiToolStripItem)tsi).Name;
					} else {
						continue;
					}

					if (tsi_name == name) {
						return tsi;
					}
					var ret = SearchMenuItemRecurse(name, tsi, parent);
					if (ret != null) {
						parent.value = tsi;
						return ret;
					}
				}
				return null;
			}
		}

		#region InputTextBox

		bool mLastSymbolEditMode { get; set; }
		bool mLastIsImeModeOn = true;

		/// <summary>
		/// 入力用のテキストボックスを初期化します
		/// </summary>
		public void showInputTextBox(string phrase, string phonetic_symbol, Point position, bool phonetic_symbol_edit_mode)
		{
			#if DEBUG
			CDebug.WriteLine("InitializeInputTextBox");
			#endif
			hideInputTextBox();

			EditorManager.InputTextBox.KeyUp += InputTextBox.mInputTextBox_KeyUp;
			EditorManager.InputTextBox.KeyDown += InputTextBox.mInputTextBox_KeyDown;
			EditorManager.InputTextBox.ImeModeChanged += InputTextBox.mInputTextBox_ImeModeChanged;
			EditorManager.InputTextBox.ImeMode = mLastIsImeModeOn ? ImeMode.Hiragana : ImeMode.Off;
			if (phonetic_symbol_edit_mode) {
				EditorManager.InputTextBox.BufferText = phrase;
				EditorManager.InputTextBox.IsPhoneticSymbolEditMode = true;
				EditorManager.InputTextBox.Text = phonetic_symbol;
				EditorManager.InputTextBox.BackColor = FormMainModel.ColorTextboxBackcolor;
			} else {
				EditorManager.InputTextBox.BufferText = phonetic_symbol;
				EditorManager.InputTextBox.IsPhoneticSymbolEditMode = false;
				EditorManager.InputTextBox.Text = phrase;
				EditorManager.InputTextBox.BackColor = Colors.White;
			}
			EditorManager.InputTextBox.Font = new Cadencii.Gui.Font (EditorManager.editorConfig.BaseFontName, Font.PLAIN, EditorConfig.FONT_SIZE9);
			var p = new Cadencii.Gui.Point(position.X + 4, position.Y + 2);
			EditorManager.InputTextBox.Location = p;

			EditorManager.InputTextBox.Enabled = true;
			EditorManager.InputTextBox.Visible = true;
			EditorManager.InputTextBox.Focus();
			EditorManager.InputTextBox.SelectAll();
		}

		public void hideInputTextBox()
		{
			EditorManager.InputTextBox.KeyUp -= InputTextBox.mInputTextBox_KeyUp;
			EditorManager.InputTextBox.KeyDown -= InputTextBox.mInputTextBox_KeyDown;
			EditorManager.InputTextBox.ImeModeChanged -= InputTextBox.mInputTextBox_ImeModeChanged;
			mLastSymbolEditMode = EditorManager.InputTextBox.IsPhoneticSymbolEditMode;
			EditorManager.InputTextBox.Visible = false;
			EditorManager.InputTextBox.Enabled = false;
			form.pictPianoRoll.Focus();
		}

		/// <summary>
		/// 現在のEditorManager.InputTextBoxの状態を元に、歌詞の変更を反映させるコマンドを実行します
		/// </summary>
		public void executeLyricChangeCommand()
		{
			if (!EditorManager.InputTextBox.Enabled) {
				return;
			}
			if (EditorManager.InputTextBox.IsDisposed) {
				return;
			}
			SelectedEventEntry last_selected_event = EditorManager.itemSelection.getLastEvent();
			bool phonetic_symbol_edit_mode = EditorManager.InputTextBox.IsPhoneticSymbolEditMode;

			int selected = EditorManager.Selected;
			VsqFileEx vsq = MusicManager.getVsqFile();
			VsqTrack vsq_track = vsq.Track[selected];

			// 後続に、連続している音符が何個あるか検査
			int maxcount = SymbolTable.getMaxDivisions(); // 音節の分割によって，高々maxcount個までにしか分割されない
			bool check_started = false;
			int endclock = 0;  // 直前の音符の終了クロック
			int count = 0;     // 後続音符の連続個数
			int start_index = -1;
			int indx = -1;
			for (Iterator<int> itr = vsq_track.indexIterator(IndexIteratorKind.NOTE); itr.hasNext(); ) {
				indx = itr.next();
				VsqEvent itemi = vsq_track.getEvent(indx);
				if (itemi.InternalID == last_selected_event.original.InternalID) {
					check_started = true;
					endclock = itemi.Clock + itemi.ID.getLength();
					count = 1;
					start_index = indx;
					continue;
				}
				if (check_started) {
					if (count + 1 > maxcount) {
						break;
					}
					if (itemi.Clock <= endclock) {
						count++;
						endclock = itemi.Clock + itemi.ID.getLength();
					} else {
						break;
					}
				}
			}

			// 後続の音符をリストアップ
			VsqEvent[] items = new VsqEvent[count];
			string[] original_symbol = new string[count];
			string[] original_phrase = new string[count];
			bool[] symbol_protected = new bool[count];
			indx = -1;
			for (Iterator<int> itr = vsq_track.indexIterator(IndexIteratorKind.NOTE); itr.hasNext(); ) {
				int index = itr.next();
				if (index < start_index) {
					continue;
				}
				indx++;
				if (count <= indx) {
					break;
				}
				VsqEvent ve = vsq_track.getEvent(index);
				items[indx] = (VsqEvent)ve.clone();
				original_symbol[indx] = ve.ID.LyricHandle.L0.getPhoneticSymbol();
				original_phrase[indx] = ve.ID.LyricHandle.L0.Phrase;
				symbol_protected[indx] = ve.ID.LyricHandle.L0.PhoneticSymbolProtected;
			}

			#if DEBUG
			CDebug.WriteLine("    original_phase,symbol=" + original_phrase + "," + original_symbol[0]);
			CDebug.WriteLine("    phonetic_symbol_edit_mode=" + phonetic_symbol_edit_mode);
			CDebug.WriteLine("    EditorManager.InputTextBox.setText(=" + EditorManager.InputTextBox.Text);
			#endif
			string[] phrase = new string[count];
			string[] phonetic_symbol = new string[count];
			for (int i = 0; i < count; i++) {
				phrase[i] = original_phrase[i];
				phonetic_symbol[i] = original_symbol[i];
			}
			string txt = EditorManager.InputTextBox.Text;
			int txtlen = PortUtil.getStringLength(txt);
			if (txtlen > 0) {
				// 1文字目は，UTAUの連続音入力のハイフンの可能性があるので，無駄に置換されるのを回避
				phrase[0] = txt.Substring(0, 1) + ((txtlen > 1) ? txt.Substring(1).Replace("-", "") : "");
			} else {
				phrase[0] = "";
			}
			if (!phonetic_symbol_edit_mode) {
				// 歌詞を編集するモードで、
				if (EditorManager.editorConfig.SelfDeRomanization) {
					// かつローマ字の入力を自動でひらがなに展開する設定だった場合。
					// ローマ字をひらがなに展開
					phrase[0] = KanaDeRomanization.Attach(phrase[0]);
				}
			}

			// 発音記号または歌詞が変更された場合の処理
			if ((phonetic_symbol_edit_mode && EditorManager.InputTextBox.Text != original_symbol[0]) ||
				(!phonetic_symbol_edit_mode && phrase[0] != original_phrase[0])) {
				if (phonetic_symbol_edit_mode) {
					// 発音記号を編集するモード
					phrase[0] = EditorManager.InputTextBox.BufferText;
					phonetic_symbol[0] = EditorManager.InputTextBox.Text;

					// 入力された発音記号のうち、有効なものだけをピックアップ
					string[] spl = PortUtil.splitString(phonetic_symbol[0], new char[] { ' ' }, true);
					List<string> list = new List<string>();
					for (int i = 0; i < spl.Length; i++) {
						string s = spl[i];
						if (VsqPhoneticSymbol.isValidSymbol(s)) {
							list.Add(s);
						}
					}

					// ピックアップした発音記号をスペース区切りで結合
					phonetic_symbol[0] = "";
					bool first = true;
					foreach (var s in list) {
						if (first) {
							phonetic_symbol[0] += s;
							first = false;
						} else {
							phonetic_symbol[0] += " " + s;
						}
					}

					// 発音記号を編集すると、自動で「発音記号をプロテクトする」モードになるよ
					symbol_protected[0] = true;
				} else {
					// 歌詞を編集するモード
					if (!symbol_protected[0]) {
						// 発音記号をプロテクトしない場合、歌詞から発音記号を引当てる
						SymbolTableEntry entry = SymbolTable.attatch(phrase[0]);
						if (entry == null) {
							phonetic_symbol[0] = "a";
						} else {
							phonetic_symbol[0] = entry.getSymbol();
							// 分節の分割記号'-'が入っている場合
							#if DEBUG
							Logger.StdOut("FormMain#executeLyricChangeCommand; word=" + entry.Word + "; symbol=" + entry.getSymbol() + "; rawSymbol=" + entry.getRawSymbol());
							#endif
							if (entry.Word.IndexOf('-') >= 0) {
								string[] spl_phrase = PortUtil.splitString(entry.Word, '\t');
								if (spl_phrase.Length <= count) {
									// 分節の分割数が，後続の音符数と同じか少ない場合
									string[] spl_symbol = PortUtil.splitString(entry.getRawSymbol(), '\t');
									for (int i = 0; i < spl_phrase.Length; i++) {
										phrase[i] = spl_phrase[i];
										phonetic_symbol[i] = spl_symbol[i];
									}
								} else {
									// 後続の音符の個数が足りない
									phrase[0] = entry.Word.Replace("\t", "");
								}
							}
						}
					} else {
						// 発音記号をプロテクトする場合、発音記号は最初のやつを使う
						phonetic_symbol[0] = original_symbol[0];
					}
				}
				#if DEBUG
				CDebug.WriteLine("    phrase,phonetic_symbol=" + phrase + "," + phonetic_symbol);
				#endif

				for (int j = 0; j < count; j++) {
					if (phonetic_symbol_edit_mode) {
						items[j].ID.LyricHandle.L0.setPhoneticSymbol(phonetic_symbol[j]);
					} else {
						items[j].ID.LyricHandle.L0.Phrase = phrase[j];
						items[j].ID.LyricHandle.L0.setPhoneticSymbol(phonetic_symbol[j]);
						MusicManager.applyUtauParameter(vsq_track, items[j]);
					}
					if (original_symbol[j] != phonetic_symbol[j]) {
						List<string> spl = items[j].ID.LyricHandle.L0.getPhoneticSymbolList();
						List<int> adjustment = new List<int>();
						for (int i = 0; i < spl.Count; i++) {
							string s = spl[i];
							adjustment.Add(VsqPhoneticSymbol.isConsonant(s) ? 64 : 0);
						}
						items[j].ID.LyricHandle.L0.setConsonantAdjustmentList(adjustment);
					}
				}

				CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventReplaceRange(selected, items));
				EditorManager.editHistory.register(vsq.executeCommand(run));
				form.setEdited(true);
			}
		}

		#endregion
	}
}

