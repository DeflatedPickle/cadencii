/*
 * AppManager.cs
 * Copyright © 2009-2011 kbinani
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
//#define ENABLE_OBSOLUTE_COMMAND
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Threading;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.CSharp;
using cadencii.apputil;
using cadencii.java.awt;
using cadencii.java.io;
using cadencii.java.util;
using cadencii.media;
using cadencii.vsq;
using cadencii.xml;
using cadencii.utau;
using ApplicationGlobal = cadencii.core.ApplicationGlobal;
using Keys = cadencii.java.awt.Keys;
using DialogResult = cadencii.java.awt.DialogResult;

namespace cadencii
{


    public partial class AppManager
    {
        /// <summary>
        /// AttachedCurve用のシリアライザ
        /// </summary>
        public static XmlSerializer xmlSerializerListBezierCurves = new XmlSerializer(typeof(AttachedCurve));
        /// <summary>
        /// クリップボード管理クラスのインスタンス
        /// </summary>
        public static ClipboardModel clipboard = null;

        #region Static Readonly Fields
        /// <summary>
        /// トラックの背景部分の塗りつぶし色。16トラックそれぞれで異なる
        /// </summary>
        public static readonly Color[] HILIGHT = new Color[] {
            new Color( 181, 220, 16 ),
            new Color( 231, 244, 49 ),
            new Color( 252, 230, 29 ),
            new Color( 247, 171, 20 ),
            new Color( 249, 94, 17 ),
            new Color( 234, 27, 47 ),
            new Color( 175, 20, 80 ),
            new Color( 183, 24, 149 ),
            new Color( 105, 22, 158 ),
            new Color( 22, 36, 163 ),
            new Color( 37, 121, 204 ),
            new Color( 29, 179, 219 ),
            new Color( 24, 239, 239 ),
            new Color( 25, 206, 175 ),
            new Color( 23, 160, 134 ),
            new Color( 79, 181, 21 ) };
        /// <summary>
        /// トラックをレンダリングするためのボタンの背景色。16トラックそれぞれで異なる
        /// </summary>
        public static readonly Color[] RENDER = new Color[]{
            new Color( 19, 143, 52 ),
            new Color( 158, 154, 18 ),
            new Color( 160, 143, 23 ),
            new Color( 145, 98, 15 ),
            new Color( 142, 52, 12 ),
            new Color( 142, 19, 37 ),
            new Color( 96, 13, 47 ),
            new Color( 117, 17, 98 ),
            new Color( 62, 15, 99 ),
            new Color( 13, 23, 84 ),
            new Color( 25, 84, 132 ),
            new Color( 20, 119, 142 ),
            new Color( 19, 142, 139 ),
            new Color( 17, 122, 102 ),
            new Color( 13, 86, 72 ),
            new Color( 43, 91, 12 ) };
        /// <summary>
        /// スクリプトに前置されるusingのリスト
        /// </summary>
        public static readonly string[] usingS = new string[] { "using System;",
                                             "using System.IO;",
                                             "using cadencii.vsq;",
                                             "using cadencii;",
                                             "using cadencii.java.io;",
                                             "using cadencii.java.util;",
                                             "using cadencii.java.awt;",
                                             "using cadencii.media;",
                                             "using cadencii.apputil;",
                                             "using System.Windows.Forms;",
                                             "using System.Collections.Generic;",
                                             "using System.Drawing;",
                                             "using System.Text;",
                                             "using System.Xml.Serialization;" };
        /// <summary>
        /// ショートカットキーとして受付可能なキーのリスト
        /// </summary>
        public static readonly List<Keys> SHORTCUT_ACCEPTABLE = new List<Keys>(new Keys[] {
            Keys.A,
            Keys.B,
            Keys.Back,
            Keys.C,
            Keys.D,
            Keys.D0,
            Keys.D1,
            Keys.D2,
            Keys.D3,
            Keys.D4,
            Keys.D5,
            Keys.D6,
            Keys.D7,
            Keys.D8,
            Keys.D9,
            Keys.Down,
            Keys.E,
            Keys.F,
            Keys.F1,
            Keys.F2,
            Keys.F3,
            Keys.F4,
            Keys.F5,
            Keys.F6,
            Keys.F7,
            Keys.F8,
            Keys.F9,
            Keys.F10,
            Keys.F11,
            Keys.F12,
            Keys.F13,
            Keys.F14,
            Keys.F15,
            Keys.F16,
            Keys.F17,
            Keys.F18,
            Keys.F19,
            Keys.F20,
            Keys.F21,
            Keys.F22,
            Keys.F23,
            Keys.F24,
            Keys.G,
            Keys.H,
            Keys.I,
            Keys.J,
            Keys.K,
            Keys.L,
            Keys.Left,
            Keys.M,
            Keys.N,
            Keys.NumPad0,
            Keys.NumPad1,
            Keys.NumPad2,
            Keys.NumPad3,
            Keys.NumPad4,
            Keys.NumPad5,
            Keys.NumPad6,
            Keys.NumPad7,
            Keys.NumPad8,
            Keys.NumPad9,
            Keys.O,
            Keys.P,
            Keys.PageDown,
            Keys.PageUp,
            Keys.Q,
            Keys.R,
            Keys.Right,
            Keys.S,
            Keys.Space,
            Keys.T,
            Keys.U,
            Keys.Up,
            Keys.V,
            Keys.W,
            Keys.X,
            Keys.Y,
            Keys.Z,
            Keys.Delete,
            Keys.Home,
            Keys.End,
        });
        /// <summary>
        /// よく使うボーダー線の色
        /// </summary>
        public static readonly Color COLOR_BORDER = new Color(118, 123, 138);
        #endregion

        #region Private Static Fields

        #endregion
        /// <summary>
        /// 歌詞入力に使用するテキストボックス
        /// </summary>
        public static LyricTextBox mInputTextBox = null;

		#if ENABLE_PROPERTY
		/// <summary>
		/// プロパティウィンドウが分離した場合のプロパティウィンドウのインスタンス。
		/// メインウィンドウとプロパティウィンドウが分離している時、propertyPanelがpropertyWindowの子になる
		/// </summary>
		public static FormNotePropertyController propertyWindow;
		#endif
		/// <summary>
		/// アイコンパレット・ウィンドウのインスタンス
		/// </summary>
		public static FormIconPalette iconPalette = null;

        static string _(string id)
        {
            return Messaging.getMessage(id);
        }

		static VsqFileEx mVsq { get { return MusicManager.getVsqFile (); } }
        public static void init()
        {
            EditorManager.loadConfig();
            clipboard = new ClipboardModel();
            // UTAU歌手のアイコンを読み込み、起動画面に表示を要求する
			int c = ApplicationGlobal.appConfig.UtauSingers.Count;
            for (int i = 0; i < c; i++) {
				SingerConfig sc = ApplicationGlobal.appConfig.UtauSingers[i];
                if (sc == null) {
                    continue;
                }
                string dir = sc.VOICEIDSTR;
                SingerConfig sc_temp = new SingerConfig();
                string path_image = Utau.readUtauSingerConfig(dir, sc_temp);

#if DEBUG
                sout.println("AppManager#init; path_image=" + path_image);
#endif
                if (Cadencii.splash != null) {
                    try {
                        Cadencii.splash.addIconThreadSafe(path_image, sc.VOICENAME);
                    } catch (Exception ex) {
                        serr.println("AppManager#init; ex=" + ex);
                        Logger.write(typeof(AppManager) + ".init; ex=" + ex + "\n");
                    }
                }
            }

            VocaloSysUtil.init();

			EditorManager.editorConfig.check();
			EditorManager.keyWidth = EditorManager.editorConfig.KeyWidth;
            VSTiDllManager.init();
            // アイコンパレード, VOCALOID1
            SingerConfigSys singer_config_sys1 = VocaloSysUtil.getSingerConfigSys(SynthesizerType.VOCALOID1);
            if (singer_config_sys1 != null) {
                foreach (SingerConfig sc in singer_config_sys1.getInstalledSingers()) {
                    if (sc == null) {
                        continue;
                    }
                    string name = sc.VOICENAME.ToLower();
                    string path_image = Path.Combine(
                                            Path.Combine(
                                                PortUtil.getApplicationStartupPath(), "resources"),
                                            name + ".png");
#if DEBUG
                    sout.println("AppManager#init; path_image=" + path_image);
#endif
                    if (Cadencii.splash != null) {
                        try {
                            Cadencii.splash.addIconThreadSafe(path_image, sc.VOICENAME);
                        } catch (Exception ex) {
                            serr.println("AppManager#init; ex=" + ex);
                            Logger.write(typeof(AppManager) + ".init; ex=" + ex + "\n");
                        }
                    }
                }
            }

            // アイコンパレード、VOCALOID2
            SingerConfigSys singer_config_sys2 = VocaloSysUtil.getSingerConfigSys(SynthesizerType.VOCALOID2);
            if (singer_config_sys2 != null) {
                foreach (SingerConfig sc in singer_config_sys2.getInstalledSingers()) {
                    if (sc == null) {
                        continue;
                    }
                    string name = sc.VOICENAME.ToLower();
                    string path_image = Path.Combine(
                                            Path.Combine(
                                                PortUtil.getApplicationStartupPath(), "resources"),
                                            name + ".png");
#if DEBUG
                    sout.println("AppManager#init; path_image=" + path_image);
#endif
                    if (Cadencii.splash != null) {
                        try {
                            Cadencii.splash.addIconThreadSafe(path_image, sc.VOICENAME);
                        } catch (Exception ex) {
                            serr.println("AppManager#init; ex=" + ex);
                            Logger.write(typeof(AppManager) + ".init; ex=" + ex + "\n");
                        }
                    }
                }
            }

            PlaySound.init();
            // VOCALOID2の辞書を読み込み
            SymbolTable.loadSystemDictionaries();
            // 日本語辞書
            SymbolTable.loadDictionary(
                Path.Combine(Path.Combine(PortUtil.getApplicationStartupPath(), "resources"), "dict_ja.txt"),
                "DEFAULT_JP");
            // 英語辞書
            SymbolTable.loadDictionary(
                Path.Combine(Path.Combine(PortUtil.getApplicationStartupPath(), "resources"), "dict_en.txt"),
                "DEFAULT_EN");
            // 拡張辞書
            SymbolTable.loadAllDictionaries(Path.Combine(PortUtil.getApplicationStartupPath(), "udic"));
            //VSTiProxy.CurrentUser = "";

            // 辞書の設定を適用
            try {
                // 現在辞書リストに読込済みの辞書を列挙
                List<ValuePair<string, Boolean>> current = new List<ValuePair<string, Boolean>>();
                for (int i = 0; i < SymbolTable.getCount(); i++) {
                    current.Add(new ValuePair<string, Boolean>(SymbolTable.getSymbolTable(i).getName(), false));
                }
                // editorConfig.UserDictionariesの設定値をコピー
                List<ValuePair<string, Boolean>> config_data = new List<ValuePair<string, Boolean>>();
				int count = EditorManager.editorConfig.UserDictionaries.Count;
                for (int i = 0; i < count; i++) {
					string[] spl = PortUtil.splitString(EditorManager.editorConfig.UserDictionaries[i], new char[] { '\t' }, 2);
                    config_data.Add(new ValuePair<string, Boolean>(spl[0], (spl[1].Equals("T") ? true : false)));
#if DEBUG
                    CDebug.WriteLine("    " + spl[0] + "," + spl[1]);
#endif
                }
                // 辞書リストとeditorConfigの設定を比較する
                // currentの方には、editorConfigと共通するものについてのみsetValue(true)とする
                List<ValuePair<string, Boolean>> common = new List<ValuePair<string, Boolean>>();
                for (int i = 0; i < config_data.Count; i++) {
                    for (int j = 0; j < current.Count; j++) {
                        if (config_data[i].getKey().Equals(current[j].getKey())) {
                            // editorConfig.UserDictionariesにもKeyが含まれていたらtrue
                            current[j].setValue(true);
                            common.Add(new ValuePair<string, Boolean>(config_data[i].getKey(), config_data[i].getValue()));
                            break;
                        }
                    }
                }
                // editorConfig.UserDictionariesに登録されていないが、辞書リストには読み込まれている場合。
                // この場合は、デフォルトでENABLEとし、優先順位を最後尾とする。
                for (int i = 0; i < current.Count; i++) {
                    if (!current[i].getValue()) {
                        common.Add(new ValuePair<string, Boolean>(current[i].getKey(), true));
                    }
                }
                SymbolTable.changeOrder(common);
            } catch (Exception ex) {
                serr.println("AppManager#init; ex=" + ex);
            }

            EditorManager.reloadUtauVoiceDB();

        }
    }

}
