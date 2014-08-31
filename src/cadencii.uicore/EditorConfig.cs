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
using cadencii.java.io;
using cadencii.java.util;
using cadencii.windows.forms;
using cadencii.xml;
using cadencii.vsq;
using cadencii.apputil;
using cadencii.uicore;
using cadencii.vsq;



namespace cadencii
{

    /// <summary>
    /// Cadenciiの環境設定
    /// </summary>
    public class EditorConfig
    {
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

        public List<SingerConfig> UtauSingers = new List<SingerConfig>();

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
            // デフォルトのフォントを，システムのメニューフォントと同じにする
            System.Drawing.Font f = System.Windows.Forms.SystemInformation.MenuFont;
            if (f != null) {
                this.BaseFontName = f.Name;
                this.ScreenFontName = f.Name;
            }

            // 言語設定を，システムのデフォルトの言語を用いる
            this.Language = Messaging.getRuntimeLanguageName();
        }
		
        /// <summary>
        /// 音符イベントに，デフォルトの歌唱スタイルを適用します
        /// </summary>
        /// <param name="item"></param>
        public void applyDefaultSingerStyle(VsqID item)
        {
            if (item == null) return;
            item.PMBendDepth = this.DefaultPMBendDepth;
            item.PMBendLength = this.DefaultPMBendLength;
            item.PMbPortamentoUse = this.DefaultPMbPortamentoUse;
            item.DEMdecGainRate = this.DefaultDEMdecGainRate;
            item.DEMaccent = this.DefaultDEMaccent;
        }
		
        /// <summary>
        /// 自動ビブラートを作成します
        /// </summary>
        /// <param name="type"></param>
        /// <param name="vibrato_clocks"></param>
        /// <returns></returns>
        public VibratoHandle createAutoVibrato(SynthesizerType type, int vibrato_clocks)
        {
            if (UseUserDefinedAutoVibratoType) {
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
            if (draft_key_width < ApplicationGlobal.MIN_KEY_WIDTH) {
				draft_key_width = ApplicationGlobal.MIN_KEY_WIDTH;
			} else if (ApplicationGlobal.MAX_KEY_WIDTH < draft_key_width) {
				draft_key_width = ApplicationGlobal.MAX_KEY_WIDTH;
            }
        }
   }
}