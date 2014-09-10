/*
 * EditorConfig.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using cadencii;
using cadencii.java.awt;
using cadencii.xml;
using cadencii.vsq;
using cadencii.core;



namespace cadencii
{

    /// <summary>
    /// Cadenciiの環境設定
    /// </summary>
    public class EditorConfig
    {
		public const int MIN_KEY_WIDTH = 68;
        public const int MAX_KEY_WIDTH = MIN_KEY_WIDTH * 5;
		/// <summary>
        /// スプリットコンテナのディバイダの位置
        /// <version>3.3+</version>
        /// </summary>
        public int SplitContainer2LastDividerLocation = -1;

		/// <summary>
        /// マウスの操作などの許容範囲。プリメジャーにPxToleranceピクセルめり込んだ入力を行っても、エラーにならない。(補正はされる)
        /// </summary>
        public int PxTolerance = 10;
        /// <summary>
        /// マウスホイールでピアノロールを水平方向にスクロールするかどうか。
        /// </summary>
        public bool ScrollHorizontalOnWheel = true;
        /// <summary>
        /// 画面描画の最大フレームレート
        /// </summary>
        public int MaximumFrameRate = 15;
		/// <summary>
        /// 再生中に画面を描画するかどうか。デフォルトはfalse
        /// <version>3.3+</version>
        /// </summary>
        public bool SkipDrawWhilePlaying = false;
        /// <summary>
        /// ピアノロール画面の縦方向のスケール.
        /// <verssion>3.3+</verssion>
        /// </summary>
        public int PianoRollScaleY = 0;
        /// <summary>
        /// ファイル・ツールバーのサイズ
        /// <version>3.3+</version>
        /// </summary>
        public int BandSizeFile = 236;
        /// <summary>
        /// ツール・ツールバーのサイズ
        /// <version>3.3+</version>
        /// </summary>
        public int BandSizeTool = 712;
        /// <summary>
        /// メジャー・ツールバーのサイズ
        /// <version>3.3+</version>
        /// </summary>
        public int BandSizeMeasure = 714;
        /// <summary>
        /// ポジション・ツールバーのサイズ
        /// <version>3.3+</version>
        /// </summary>
        public int BandSizePosition = 234;
        /// <summary>
        /// ファイル・ツールバーを新しい行に追加するかどうか
        /// <version>3.3+</version>
        /// </summary>
        public bool BandNewRowFile = false;
        /// <summary>
        /// ツール・ツールバーを新しい行に追加するかどうか
        /// <version>3.3+</version>
        /// </summary>
        public bool BandNewRowTool = false;
        /// <summary>
        /// メジャー・ツールバーを新しい行に追加するかどうか
        /// <version>3.3+</version>
        /// </summary>
        public bool BandNewRowMeasure = false;
        /// <summary>
        /// ポジション・ツールバーを新しい行に追加するかどうか
        /// <version>3.3+</version>
        /// </summary>
        public bool BandNewRowPosition = true;
        /// <summary>
        /// ファイル・ツールバーの順番
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public int BandOrderFile = 0;
        /// <summary>
        /// ツール・ツールバーの順番
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public int BandOrderTool = 1;
        /// <summary>
        /// メジャー・ツールバーの順番
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public int BandOrderMeasure = 3;
        /// <summary>
        /// ポジション・ツールバーの順番
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public int BandOrderPosition = 2;
        /// <summary>
        /// ツールバーのChevronの幅．
        /// Winodws 7(Aero): 17px
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public int ChevronWidth = 17;
		/// <summary>
        /// ピアノロールの縦軸の拡大率を表す整数値の最大値
        /// </summary>
        public const int MAX_PIANOROLL_SCALEY = 10;
        /// <summary>
        /// ピアノロールの縦軸の拡大率を表す整数値の最小値
        /// </summary>
        public const int MIN_PIANOROLL_SCALEY = -4;
		/// <summary>
        /// 強弱記号の，ピアノロール画面上の表示幅（ピクセル）
        /// </summary>
        public const int DYNAFF_ITEM_WIDTH = 40;
        public const int FONT_SIZE8 = 8;
        public const int FONT_SIZE9 = FONT_SIZE8 + 1;
        public const int FONT_SIZE10 = FONT_SIZE8 + 2;
        public const int FONT_SIZE50 = FONT_SIZE8 + 42;

        /// <summary>
        /// 画面描画に使用する共用のフォントオブジェクト
        /// </summary>
        public static Font baseFont8 = new Font("Dialog", Font.PLAIN, FONT_SIZE8);
        /// <summary>
        /// 画面描画に使用する共用のフォントオブジェクト
        /// </summary>
        public static Font baseFont9 = new Font("Dialog", Font.PLAIN, FONT_SIZE9);
        /// <summary>
        /// 画面描画に使用する共用のフォントオブジェクト
        /// </summary>
        public static Font baseFont10 = new Font("Dialog", Font.PLAIN, FONT_SIZE10);
        /// <summary>
        /// 画面描画に使用する共用のフォントオブジェクト
        /// </summary>
        public static Font baseFont10Bold = new Font("Dialog", Font.BOLD, FONT_SIZE10);
        /// <summary>
        /// 画面描画に使用する共用のフォントオブジェクト
        /// </summary>
        public static Font baseFont50Bold = new Font("Dialog", Font.BOLD, FONT_SIZE50);
        /// <summary>
        /// 歌詞を音符の（高さ方向の）真ん中に描画するためのオフセット。
        /// たとえば，文字列の中心軸がy_centerを通るように描画したい場合は，
        /// <code>g.drawString( ..., x, y_center - baseFont10OffsetHeight + 1 )</code>
        /// とすればよい
        /// </summary>
        public static int baseFont10OffsetHeight = 0;
        /// <summary>
        /// 歌詞を音符の（高さ方向の）真ん中に描画するためのオフセット。
        /// たとえば，文字列の中心軸がy_centerを通るように描画したい場合は，
        /// <code>g.drawString( ..., x, y_center - baseFont8OffsetHeight + 1 )</code>
        /// とすればよい
        /// </summary>
        public static int baseFont8OffsetHeight = 0;
        /// <summary>
        /// 歌詞を音符の（高さ方向の）真ん中に描画するためのオフセット。
        /// たとえば，文字列の中心軸がy_centerを通るように描画したい場合は，
        /// <code>g.drawString( ..., x, y_center - baseFont9OffsetHeight + 1 )</code>
        /// とすればよい
        /// </summary>
        public static int baseFont9OffsetHeight = 0;
        /// <summary>
        /// 歌詞を音符の（高さ方向の）真ん中に描画するためのオフセット。
        /// たとえば，文字列の中心軸がy_centerを通るように描画したい場合は，
        /// <code>g.drawString( ..., x, y_center - baseFont50OffsetHeight + 1 )</code>
        /// とすればよい
        /// </summary>
        public static int baseFont50OffsetHeight = 0;
        /// <summary>
        /// フォントオブジェクトbaseFont8の描画時の高さ
        /// </summary>
        public static int baseFont8Height = FONT_SIZE8;
        /// <summary>
        /// フォントオブジェクトbaseFont9の描画時の高さ
        /// </summary>
        public static int baseFont9Height = FONT_SIZE9;
        /// <summary>
        /// フォントオブジェクトbaseFont1-の描画時の高さ
        /// </summary>
        public static int baseFont10Height = FONT_SIZE10;
        /// <summary>
        /// フォントオブジェクトbaseFont50の描画時の高さ
        /// </summary>
        public static int baseFont50Height = FONT_SIZE50;

	/// <summary>
        /// ユーザー定義のビブラート設定．
        /// <version>3.3+</version>
        /// </summary>
        public List<VibratoHandle> AutoVibratoCustom = new List<VibratoHandle>();
        /// <summary>
        /// キーボードからの入力に使用するデバイス
        /// </summary>
        public MidiPortConfig MidiInPort = new MidiPortConfig();
        /// <summary>
        /// MTCスレーブ動作を行う際使用するMIDI INポートの設定
        /// </summary>
        public MidiPortConfig MidiInPortMtc = new MidiPortConfig();

        public PropertyPanelState PropertyWindowStatus = new PropertyPanelState();

        public FormMidiImExportConfig MidiImExportConfigExport = new FormMidiImExportConfig();
        public FormMidiImExportConfig MidiImExportConfigImport = new FormMidiImExportConfig();
        public FormMidiImExportConfig MidiImExportConfigImportVsq = new FormMidiImExportConfig();
		#region static fields
        private static cadencii.xml.XmlSerializer s_serializer = new cadencii.xml.XmlSerializer(typeof(EditorConfig));
        #endregion
		/// <summary>
        /// デフォルトの横軸方向のスケール
        /// </summary>
        public int DefaultXScale = 65;
        public string BaseFontName = "MS UI Gothic";
        public float BaseFontSize = 9.0f;
        public string ScreenFontName = "MS UI Gothic";
        public int WheelOrder = 20;
        public bool CursorFixed = false;
        /// <summary>
        /// RecentFilesに登録することの出来る最大のファイル数
        /// </summary>
        public int NumRecentFiles = 5;

		/// <summary>
        /// アイコンパレット・ウィンドウの位置
        /// </summary>
        public XmlPoint FormIconPaletteLocation = new XmlPoint(0, 0);
        /// <summary>
        /// アイコンパレット・ウィンドウを常に手前に表示するかどうか
        /// 3.3で廃止
        /// </summary>
        private bool __revoked__FormIconTopMost = true;
        /// <summary>
        /// 最初に戻る、のショートカットキー
        /// </summary>
        [XmlArrayItem("BKeys")]
        public Keys[] SpecialShortcutGoToFirst = new Keys[] { Keys.Home };
		/// <summary>
        /// 最近使用したファイルのリスト
        /// </summary>
        public List<string> RecentFiles = new List<string>();

		public int PxTrackHeight = 14;
        public int MouseDragIncrement = 50;
        public int MouseDragMaximumRate = 600;
        /// <summary>
        /// ミキサーウィンドウが表示された状態かどうか
        /// </summary>
        public bool MixerVisible = false;
        /// <summary>
        /// アイコンパレットが表示された状態かどうか
        /// <version>3.3+</version>
        /// </summary>
        public bool IconPaletteVisible = false;
        public int PreSendTime = 500;
	
		/// <summary>
        /// ウィンドウが最大化された状態かどうか
        /// </summary>
        public bool WindowMaximized = false;
        /// <summary>
        /// ウィンドウの位置とサイズ．
        /// 最小化された状態での値は，この変数に代入されない(ことになっている)
        /// </summary>
        public Rectangle WindowRect = new Rectangle(0, 0, 970, 718);
        /// <summary>
        /// hScrollのスクロールボックスの最小幅(px)
        /// </summary>
        public int MinimumScrollHandleWidth = 20;
        /// <summary>
        /// 発音記号入力モードを，維持するかどうか
        /// </summary>
        public bool KeepLyricInputMode = false;
        /// <summary>
        /// ピアノロールの何もないところをクリックした場合、右クリックでもプレビュー音を再生するかどうか
        /// </summary>
        public bool PlayPreviewWhenRightClick = false;
        /// <summary>
        /// ゲームコントローラで、異なるイベントと識別する最小の時間間隔(millisec)
        /// </summary>
        public int GameControlerMinimumEventInterval = 100;
        /// <summary>
        /// カーブの選択範囲もクオンタイズするかどうか
        /// </summary>
        public bool CurveSelectingQuantized = true;
		/// <summary>
        /// ピアノロール上に歌詞を表示するかどうか
        /// </summary>
        public bool ShowLyric = true;
        /// <summary>
        /// ピアノロール上に，ビブラートとアタックの概略を表す波線を表示するかどうか
        /// </summary>
        public bool ShowExpLine = true;

		/// <summary>
        /// Button index of "△"
        /// </summary>
        public int GameControlerTriangle = 0;
        /// <summary>
        /// Button index of "○"
        /// </summary>
        public int GameControlerCircle = 1;
        /// <summary>
        /// Button index of "×"
        /// </summary>
        public int GameControlerCross = 2;
        /// <summary>
        /// Button index of "□"
        /// </summary>
        public int GameControlerRectangle = 3;
        /// <summary>
        /// Button index of "L1"
        /// </summary>
        public int GameControlL1 = 4;
        /// <summary>
        /// Button index of "R1"
        /// </summary>
        public int GameControlR1 = 5;
        /// <summary>
        /// Button index of "L2"
        /// </summary>
        public int GameControlL2 = 6;
        /// <summary>
        /// Button index of "R2"
        /// </summary>
        public int GameControlR2 = 7;
        /// <summary>
        /// Button index of "SELECT"
        /// </summary>
        public int GameControlSelect = 8;
        /// <summary>
        /// Button index of "START"
        /// </summary>
        public int GameControlStart = 9;
        /// <summary>
        /// Button index of Left Stick
        /// </summary>
        public int GameControlStirckL = 10;
        /// <summary>
        /// Button index of Right Stick
        /// </summary>
        public int GameControlStirckR = 11;
	public int GameControlPovRight = 9000;
        public int GameControlPovLeft = 27000;
        public int GameControlPovUp = 0;
        public int GameControlPovDown = 18000;

	#region public static method
        /// <summary>
        /// EditorConfigのインスタンスをxmlシリアライズするためのシリアライザを取得します
        /// </summary>
        /// <returns></returns>
        public static cadencii.xml.XmlSerializer getSerializer()
        {
            return s_serializer;
        }
        #endregion

		
        /// <summary>
        /// コンストラクタ．起動したOSによって動作を変える場合がある
        /// </summary>
        public EditorConfig()
        {
	        // FIXME: enable this.
		/*
            // デフォルトのフォントを，システムのメニューフォントと同じにする
            System.Drawing.Font f = System.Windows.Forms.SystemInformation.MenuFont;
            if (f != null) {
                this.BaseFontName = f.Name;
                this.ScreenFontName = f.Name;
            }
			*/
        }
		
        /// <summary>
        /// 音符イベントに，デフォルトの歌唱スタイルを適用します
        /// </summary>
        /// <param name="item"></param>
        public void applyDefaultSingerStyle(VsqID item)
        {
            if (item == null) return;
            item.PMBendDepth = ApplicationGlobal.appConfig.DefaultPMBendDepth;
			item.PMBendLength = ApplicationGlobal.appConfig.DefaultPMBendLength;
			item.PMbPortamentoUse = ApplicationGlobal.appConfig.DefaultPMbPortamentoUse;
			item.DEMdecGainRate = ApplicationGlobal.appConfig.DefaultDEMdecGainRate;
			item.DEMaccent = ApplicationGlobal.appConfig.DefaultDEMaccent;
        }
		/// <summary>
        /// ビブラートの自動追加を行うかどうかを決める音符長さの閾値．単位はclock
        /// <version>3.3+</version>
        /// </summary>
        public int AutoVibratoThresholdLength = 480;
        /// <summary>
        /// VOCALOID1用のデフォルトビブラート設定
        /// </summary>
        public string AutoVibratoType1 = "$04040001";
        /// <summary>
        /// VOCALOID2用のデフォルトビブラート設定
        /// </summary>
        public string AutoVibratoType2 = "$04040001";
        /// <summary>
        /// カスタムのデフォルトビブラート設定
        /// <version>3.3+</version>
        /// </summary>
        public string AutoVibratoTypeCustom = "$04040001";
        /// <summary>
        /// ビブラートの自動追加を行うかどうか
        /// </summary>
        public bool EnableAutoVibrato = true;
        /// <summary>
        /// ピアノロール上での，音符の表示高さ(ピクセル)
        /// </summary>
		
        /// <summary>
        /// 自動ビブラートを作成します
        /// </summary>
        /// <param name="type"></param>
        /// <param name="vibrato_clocks"></param>
        /// <returns></returns>
        public VibratoHandle createAutoVibrato(SynthesizerType type, int vibrato_clocks)
        {
            if (ApplicationGlobal.appConfig.UseUserDefinedAutoVibratoType) {
                if (AutoVibratoCustom == null) {
                    AutoVibratoCustom = new List<VibratoHandle>();
                }

                // 下4桁からインデックスを取得
                int index = 0;
                if (this.AutoVibratoTypeCustom == null) {
                    index = 0;
                } else {
                    int trimlen = 4;
                    int len = PortUtil.getStringLength(this.AutoVibratoTypeCustom);
                    if (len < 4) {
                        trimlen = len;
                    }
                    if (trimlen > 0) {
                        string s = this.AutoVibratoTypeCustom.Substring(len - trimlen, trimlen);
                        try {
                            index = (int)PortUtil.fromHexString(s);
                            index--;
                        } catch (Exception ex) {
                            serr.println(typeof(EditorConfig) + ".createAutoVibrato; ex=" + ex + "; AutoVibratoTypeCustom=" + AutoVibratoTypeCustom + "; s=" + s);
                            index = 0;
                        }
                    }
                }

#if DEBUG
                sout.println("EditorConfig.createAutoVibrato; AutoVibratoTypeCustom=" + AutoVibratoTypeCustom + "; index=" + index);
#endif
                VibratoHandle ret = null;
                if (0 <= index && index < this.AutoVibratoCustom.Count) {
                    ret = this.AutoVibratoCustom[index];
                    if (ret != null) {
                        ret = (VibratoHandle)ret.clone();
                    }
                }
                if (ret == null) {
                    ret = new VibratoHandle();
                }
                ret.IconID = "$0404" + PortUtil.toHexString(index + 1, 4);
                ret.setLength(vibrato_clocks);
                return ret;
            } else {
                string iconid = type == SynthesizerType.VOCALOID1 ? AutoVibratoType1 : AutoVibratoType2;
                VibratoHandle ret = VocaloSysUtil.getDefaultVibratoHandle(iconid,
                                                                           vibrato_clocks,
                                                                           type);
                if (ret == null) {
                    ret = new VibratoHandle();
                    ret.IconID = "$04040001";
                    ret.setLength(vibrato_clocks);
                }
                return ret;
            }
        }
		
        public Font getBaseFont()
        {
            return new Font(BaseFontName, Font.PLAIN, (int)BaseFontSize);
        }
		private QuantizeMode m_position_quantize = QuantizeMode.p32;
        private bool m_position_quantize_triplet = false;
        private QuantizeMode m_length_quantize = QuantizeMode.p32;
        private bool m_length_quantize_triplet = false;
        private int m_mouse_hover_time = 500;

        public int getMouseHoverTime()
        {
            return m_mouse_hover_time;
        }

        public void setMouseHoverTime(int value)
        {
            if (value < 0) {
                m_mouse_hover_time = 0;
            } else if (2000 < m_mouse_hover_time) {
                m_mouse_hover_time = 2000;
            } else {
                m_mouse_hover_time = value;
            }
        }

        // XMLシリアライズ用
        /// <summary>
        /// ピアノロール上でマウスホバーイベントが発生するまでの時間(millisec)
        /// </summary>
        public int MouseHoverTime
        {
            get
            {
                return getMouseHoverTime();
            }
            set
            {
                setMouseHoverTime(value);
            }
        }
		
        /// <summary>
        /// PositionQuantize, PositionQuantizeTriplet, LengthQuantize, LengthQuantizeTripletの描くプロパティのいずれかが
        /// 変更された時発生します
        /// </summary>
        public static event EventHandler QuantizeModeChanged;
		
        public QuantizeMode getPositionQuantize()
        {
            return m_position_quantize;
        }
		
        public void setPositionQuantize(QuantizeMode value)
        {
            if (m_position_quantize != value) {
                m_position_quantize = value;
                try {
                    invokeQuantizeModeChangedEvent();
                } catch (Exception ex) {
                    Logger.write(typeof(EditorConfig) + ".getPositionQuantize; ex=" + ex + "\n");
                    serr.println("EditorConfig#setPositionQuantize; ex=" + ex);
                }
            }
        }

        // XMLシリアライズ用
        public QuantizeMode PositionQuantize
        {
            get
            {
                return getPositionQuantize();
            }
            set
            {
                setPositionQuantize(value);
            }
        }

        public bool isPositionQuantizeTriplet()
        {
            return m_position_quantize_triplet;
        }

        public void setPositionQuantizeTriplet(bool value)
        {
            if (m_position_quantize_triplet != value) {
                m_position_quantize_triplet = value;
                try {
                    invokeQuantizeModeChangedEvent();
                } catch (Exception ex) {
                    serr.println("EditorConfig#setPositionQuantizeTriplet; ex=" + ex);
                    Logger.write(typeof(EditorConfig) + ".setPositionQuantizeTriplet; ex=" + ex + "\n");
                }
            }
        }

        // XMLシリアライズ用
        public bool PositionQuantizeTriplet
        {
            get
            {
                return isPositionQuantizeTriplet();
            }
            set
            {
                setPositionQuantizeTriplet(value);
            }
        }

        public QuantizeMode getLengthQuantize()
        {
            return m_length_quantize;
        }

        public void setLengthQuantize(QuantizeMode value)
        {
            if (m_length_quantize != value) {
                m_length_quantize = value;
                try {
                    invokeQuantizeModeChangedEvent();
                } catch (Exception ex) {
                    serr.println("EditorConfig#setLengthQuantize; ex=" + ex);
                    Logger.write(typeof(EditorConfig) + ".setLengthQuantize; ex=" + ex + "\n");
                }
            }
        }

        public QuantizeMode LengthQuantize
        {
            get
            {
                return getLengthQuantize();
            }
            set
            {
                setLengthQuantize(value);
            }
        }

        public bool isLengthQuantizeTriplet()
        {
            return m_length_quantize_triplet;
        }

        public void setLengthQuantizeTriplet(bool value)
        {
            if (m_length_quantize_triplet != value) {
                m_length_quantize_triplet = value;
                try {
                    invokeQuantizeModeChangedEvent();
                } catch (Exception ex) {
                    serr.println("EditorConfig#setLengthQuantizeTriplet; ex=" + ex);
                    Logger.write(typeof(EditorConfig) + ".setLengthQuantizeTriplet; ex=" + ex + "\n");
                }
            }
        }

        /// <summary>
        /// QuantizeModeChangedイベントを発行します
        /// </summary>
        private void invokeQuantizeModeChangedEvent()
        {
            if (QuantizeModeChanged != null) {
                QuantizeModeChanged.Invoke(typeof(EditorConfig), new EventArgs());
            }
        }

        // XMLシリアライズ用
        public bool LengthQuantizeTriplet
        {
            get
            {
                return isLengthQuantizeTriplet();
            }
            set
            {
                setLengthQuantizeTriplet(value);
            }
        }
		
        /// <summary>
        /// 「最近使用したファイル」のリストに、アイテムを追加します
        /// </summary>
        /// <param name="new_file"></param>
        public void pushRecentFiles(string new_file)
        {
            // NumRecentFilesは0以下かも知れない
            if (NumRecentFiles <= 0) {
                NumRecentFiles = 5;
            }

            // RecentFilesはnullかもしれない．
            if (RecentFiles == null) {
                RecentFiles = new List<string>();
            }

            // 重複があれば消す
            List<string> dict = new List<string>();
            foreach (var s in RecentFiles) {
                bool found = false;
                for (int i = 0; i < dict.Count; i++) {
                    if (s.Equals(dict[i])) {
                        found = true;
                    }
                }
                if (!found) {
                    dict.Add(s);
                }
            }
            RecentFiles.Clear();
            foreach (var s in dict) {
                RecentFiles.Add(s);
            }

            // 現在登録されているRecentFilesのサイズが規定より大きければ，下の方から消す
            if (RecentFiles.Count > NumRecentFiles) {
                for (int i = RecentFiles.Count - 1; i > NumRecentFiles; i--) {
                    RecentFiles.RemoveAt(i);
                }
            }

            // 登録しようとしているファイルは，RecentFilesの中に既に登録されているかs？
            int index = -1;
            for (int i = 0; i < RecentFiles.Count; i++) {
                if (RecentFiles[i].Equals(new_file)) {
                    index = i;
                    break;
                }
            }

            if (index >= 0) {  // 登録されてる場合
                RecentFiles.RemoveAt(index);
            }
            RecentFiles.Insert(0, new_file);
        }
		
        public SortedDictionary<string, Keys[]> getShortcutKeysDictionary(List<ValuePairOfStringArrayOfKeys> defs)
        {
            SortedDictionary<string, Keys[]> ret = new SortedDictionary<string, Keys[]>();
            for (int i = 0; i < ShortcutKeys.Count; i++) {
                ret[ShortcutKeys[i].Key] = ShortcutKeys[i].Value;
            }
            foreach (var item in defs) {
                if (!ret.ContainsKey(item.Key)) {
                    ret[item.Key] = item.Value;
                }
            }
            return ret;
        }
	/// <summary>
        /// ユーザー辞書のOn/Offと順序
        /// </summary>
        public List<string> UserDictionaries = new List<string>();
		/// <summary>
        /// ベジエ制御点を掴む時の，掴んだと判定する際の誤差．制御点の外輪からPxToleranceBezierピクセルずれていても，掴んだと判定する．
        /// </summary>
        public int PxToleranceBezier = 10;
        /// <summary>
        /// 歌詞入力においてローマ字が入力されたとき，Cadencii側でひらがなに変換するかどうか
        /// </summary>
        public bool SelfDeRomanization = false;
        /// <summary>
        /// openMidiDialogで最後に選択された拡張子
        /// </summary>
        public string LastUsedExtension = ".vsq";
        /// <summary>
        /// ミキサーダイアログを常に手前に表示するかどうか
        /// 3.3で廃止
        /// </summary>
        private bool __revoked__MixerTopMost = true;
        public List<ValuePairOfStringArrayOfKeys> ShortcutKeys = new List<ValuePairOfStringArrayOfKeys>();
        /// <summary>
        /// 概観ペインが表示されているかどうか
        /// </summary>
        public bool OverviewEnabled = false;
        public int OverviewScaleCount = 5;
        /// <summary>
        /// 自動バックアップする間隔．単位は分
        /// </summary>
        public int AutoBackupIntervalMinutes = 10;
        /// <summary>
        /// 鍵盤の表示幅、ピクセル,AppManager.keyWidthに代入。
        /// </summary>
        public int KeyWidth = 136;
        /// <summary>
        /// スペースキーを押しながら左クリックで、中ボタンクリックとみなす動作をさせるかどうか。
        /// </summary>
        public bool UseSpaceKeyAsMiddleButtonModifier = false;
		
        /// <summary>
        /// このインスタンスの整合性をチェックします．
        /// </summary>
        public void check()
        {
            int count = SymbolTable.getCount();
            for (int i = 0; i < count; i++) {
                SymbolTable st = SymbolTable.getSymbolTable(i);
                bool found = false;
                foreach (var s in UserDictionaries) {
                    string[] spl = PortUtil.splitString(s, new char[] { '\t' }, 2);
                    if (st.getName().Equals(spl[0])) {
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    UserDictionaries.Add(st.getName() + "\tT");
                }
            }

            // key_widthを最大，最小の間に収める
            int draft_key_width = this.KeyWidth;
            if (draft_key_width < MIN_KEY_WIDTH) {
				draft_key_width = MIN_KEY_WIDTH;
			} else if (MAX_KEY_WIDTH < draft_key_width) {
				draft_key_width = MAX_KEY_WIDTH;
            }
        }
   }
}