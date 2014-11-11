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
//using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using cadencii;
using Cadencii.Gui;
using cadencii.java.util;
//
//using Cadencii.Xml;
using Cadencii.Media.Vsq;
using cadencii.apputil;
//using cadencii.uicore;
using Cadencii.Utilities;



namespace cadencii.core
{

    /// <summary>
    /// Cadenciiの環境設定
    /// </summary>
    public class EditorConfig
    {
        /// <summary>
        /// デフォルトで使用する歌手の名前
        /// </summary>
        public string DefaultSingerName = "Miku";
        public int DefaultPMBendDepth = 8;
        public int DefaultPMBendLength = 0;
        public int DefaultPMbPortamentoUse = 3;
        public int DefaultDEMdecGainRate = 50;
        public int DefaultDEMaccent = 50;
        public DefaultVibratoLengthEnum DefaultVibratoLength = DefaultVibratoLengthEnum.L66;
        /// <summary>
        /// デフォルトビブラートのRate
        /// バージョン3.3で廃止
        /// </summary>
        [Obsolete]
        private int __revoked__DefaultVibratoRate = 64;
        /// <summary>
        /// デフォルトビブラートのDepth
        /// バージョン3.3で廃止
        /// </summary>
        [Obsolete]
        private int __revoked__DefaultVibratoDepth = 64;
        public ClockResolution ControlCurveResolution = ClockResolution.L30;
        /// <summary>
        /// 言語設定
        /// </summary>
        public string Language = "";
        /// <summary>
        /// 実行環境
        /// </summary>
        private PlatformEnum __revoked__Platform = PlatformEnum.Windows;
		
        public List<SingerConfig> UtauSingers = new List<SingerConfig>();

        public bool CurveVisibleVelocity = true;
        public bool CurveVisibleAccent = true;
        public bool CurveVisibleDecay = true;
        public bool CurveVisibleVibratoRate = true;
        public bool CurveVisibleVibratoDepth = true;
        public bool CurveVisibleDynamics = true;
        public bool CurveVisibleBreathiness = true;
        public bool CurveVisibleBrightness = true;
        public bool CurveVisibleClearness = true;
        public bool CurveVisibleOpening = true;
        public bool CurveVisibleGendorfactor = true;
        public bool CurveVisiblePortamento = true;
        public bool CurveVisiblePit = true;
        public bool CurveVisiblePbs = true;
        public bool CurveVisibleHarmonics = false;
        public bool CurveVisibleFx2Depth = false;
        public bool CurveVisibleReso1 = false;
        public bool CurveVisibleReso2 = false;
        public bool CurveVisibleReso3 = false;
        public bool CurveVisibleReso4 = false;
        public bool CurveVisibleEnvelope = false;
        /// <summary>
        /// wave波形を表示するかどうか
        /// </summary>
        public bool ViewWaveform = false;

        public bool ViewAtcualPitch = false;
        /// <summary>
        /// UTAU互換の合成器のパス(1個目)
        /// </summary>
        public string PathResampler = "";
        /// <summary>
        /// UTAU互換の合成器のパス(2個目以降)
        /// </summary>
        public List<string> PathResamplers = new List<string>();
        /// <summary>
        /// UTAU用のwave切り貼りツール
        /// </summary>
        public string PathWavtool = "";
        /// <summary>
        /// AquesToneのVSTi dllへのパス
        /// </summary>
        public string PathAquesTone = "";
        /// <summary>
        /// AquesTone2 の VSTi dll へのパス
        /// </summary>
        public string PathAquesTone2 = "";
        /// <summary>
        /// waveファイル出力時のチャンネル数（1または2）
        /// 3.3で廃止
        /// </summary>
        private int __revoked__WaveFileOutputChannel = 2;
        /// <summary>
        /// waveファイル出力時に、全トラックをmixして出力するかどうか
        /// 3.3で廃止
        /// </summary>
        private bool __revoked__WaveFileOutputFromMasterTrack = false;
        /// <summary>
        /// プロジェクトごとのキャッシュディレクトリを使うかどうか
        /// </summary>
        public bool UseProjectCache = true;
        /// <summary>
        /// 鍵盤用のキャッシュが無いとき、FormGenerateKeySoundを表示しないかどうか。
        /// trueなら表示しない、falseなら表示する（デフォルト）
        /// </summary>
        public bool DoNotAskKeySoundGeneration = false;
        /// <summary>
        /// VOCALOID1 (1.0)のDLLを読み込まない場合true。既定ではfalse
        /// 3.3で廃止
        /// </summary>
        private bool __revoked__DoNotUseVocaloid100 = false;
        /// <summary>
        /// VOCALOID1 (1.1)のDLLを読み込まない場合true。既定ではfalse
        /// 3.3で廃止
        /// </summary>
        private bool __revoked__DoNotUseVocaloid101 = false;

        /// <summary>
        /// VOCALOID2のDLLを読み込まない場合true。既定ではfalse
        /// </summary>
        public bool DoNotUseVocaloid2 = false;
        /// <summary>
        /// AquesToneのDLLを読み込まない場合true。既定ではfalse
        /// </summary>
        public bool DoNotUseAquesTone = false;
        /// <summary>
        /// AquesTone2のDLLを読み込まない場合true。既定ではfalse
        /// </summary>
        public bool DoNotUseAquesTone2 = false;
        /// <summary>
        /// 2個目のVOCALOID1 DLLを読み込むかどうか。既定ではfalse
        /// 3.3で廃止
        /// </summary>
        private bool __revoked__LoadSecondaryVocaloid1Dll = false;
        /// <summary>
        /// VOALOID1のDLLを読み込まない場合はtrue．既定ではfalse
        /// </summary>
        public bool DoNotUseVocaloid1 = false;
        /// <summary>
        /// WAVE再生時のバッファーサイズ。既定では1000ms。
        /// </summary>
        public int BufferSizeMilliSeconds = 1000;
        /// <summary>
        /// トラックを新規作成するときのデフォルトの音声合成システム
        /// </summary>
        public RendererKind DefaultSynthesizer
#if ENABLE_VOCALOID
 = RendererKind.VOCALOID2;
#else
            = RendererKind.VCNT;
#endif
		/// <summary>
        /// 最後に入力したファイルパスのリスト
        /// リストに入る文字列は，拡張子+タブ文字+パスの形式にする
        /// 拡張子はピリオドを含めない
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public List<string> LastUsedPathIn = new List<string>();
        /// <summary>
        /// 最後に出力したファイルパスのリスト
        /// リストに入る文字列は，拡張子+タブ文字+パスの形式にする
        /// 拡張子はピリオドを含めない
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public List<string> LastUsedPathOut = new List<string>();
        /// <summary>
        /// UTAUのresampler用に，ジャンクション機能を使うかどうか
        /// version 3.3+
        /// </summary>
        public bool UseWideCharacterWorkaround = false;
        public bool DoNotAutomaticallyCheckForUpdates = false;

        /// <summary>
        /// バッファーサイズに設定できる最大値
        /// </summary>
        public const int MAX_BUFFER_MILLISEC = 1000;
        /// <summary>
        /// バッファーサイズに設定できる最小値
        /// </summary>
        public const int MIN_BUFFER_MILLIXEC = 100;
		
		/// <summary>
        /// 自動ビブラートを作成するとき，ユーザー定義タイプのビブラートを利用するかどうか．デフォルトではfalse
        /// </summary>
        public bool UseUserDefinedAutoVibratoType = false;

		#region used by cadencii.dsp
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

	public int PreSendTime = 500;
#endregion

        /// <summary>
        /// コンストラクタ．起動したOSによって動作を変える場合がある
        /// </summary>
        public EditorConfig()
        {
            // 言語設定を，システムのデフォルトの言語を用いる
            this.Language = Messaging.getRuntimeLanguageName();
        }

        #region private static method
        private static string getLastUsedPathCore(List<string> list, string extension)
        {
            if (extension == null) return "";
            if (PortUtil.getStringLength(extension) <= 0) return "";
            if (extension.Equals(".")) return "";

            if (extension.StartsWith(".")) {
                extension = extension.Substring(1);
            }

            int c = list.Count;
            for (int i = 0; i < c; i++) {
                string s = list[i];
                if (s.StartsWith(extension)) {
                    string[] spl = PortUtil.splitString(s, '\t');
                    if (spl.Length >= 2) {
                        return spl[1];
                    }
                    break;
                }
            }
            return "";
        }

        private static void setLastUsedPathCore(List<string> list, string path, string ext_with_dot)
        {
            string extension = ext_with_dot;
            if (extension == null) return;
            if (extension.Equals(".")) return;
            if (extension.StartsWith(".")) {
                extension = extension.Substring(1);
            }

            int c = list.Count;
            string entry = extension + "\t" + path;
            for (int i = 0; i < c; i++) {
                string s = list[i];
                if (s.StartsWith(extension)) {
                    list[i] = entry;
                    return;
                }
            }
            list.Add(entry);
        }
        #endregion

		#region public method
        /// <summary>
        /// 登録されているUTAU互換合成器の個数を調べます
        /// </summary>
        /// <returns></returns>
        public int getResamplerCount()
        {
            int ret = PathResamplers.Count;
            if (!PathResampler.Equals("")) {
                ret++;
            }
            return ret;
        }

        /// <summary>
        /// 登録されているUTAU互換合成器の登録を全て解除します
        /// </summary>
        public void clearResampler()
        {
            PathResamplers.Clear();
            PathResampler = "";
        }

        /// <summary>
        /// 第index番目に登録されているUTAU互換合成器のパスを取得します
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string getResamplerAt(int index)
        {
            if (index == 0) {
                return PathResampler;
            } else {
                index--;
                if (0 <= index && index < PathResamplers.Count) {
                    return PathResamplers[index];
                }
            }
            return "";
        }

        public IEnumerable<string> resamplers()
        {
            yield return PathResampler;
            foreach (var path in PathResamplers) {
                yield return path;
            }
            yield break;
        }

        /// <summary>
        /// 第index番目のUTAU互換合成器のパスを設定します
        /// </summary>
        /// <param name="index"></param>
        /// <param name="path"></param>
        public void setResamplerAt(int index, string path)
        {
            if (path == null) {
                return;
            }
            if (path.Equals("")) {
                return;
            }
            if (index == 0) {
                PathResampler = path;
            } else {
                index--;
                if (0 <= index && index < PathResamplers.Count) {
                    PathResamplers[index] = path;
                }
            }
        }

        /// <summary>
        /// 第index番目のUTAU互換合成器を登録解除します
        /// </summary>
        /// <param name="index"></param>
        public void removeResamplerAt(int index)
        {
            int size = PathResamplers.Count;
            if (index == 0) {
                if (size > 0) {
                    PathResampler = PathResamplers[0];
                    for (int i = 0; i < size - 1; i++) {
                        PathResamplers[i] = PathResamplers[i + 1];
                    }
                    PathResamplers.RemoveAt(size - 1);
                } else {
                    PathResampler = "";
                }
            } else {
                index--;
                if (0 <= index && index < size) {
                    for (int i = 0; i < size - 1; i++) {
                        PathResamplers[i] = PathResamplers[i + 1];
                    }
                    PathResamplers.RemoveAt(size - 1);
                }
            }
        }

        /// <summary>
        /// 新しいUTAU互換合成器のパスを登録します
        /// </summary>
        /// <param name="path"></param>
        public void addResampler(string path)
        {
            int count = getResamplerCount();
            if (count == 0) {
                PathResampler = path;
            } else {
                PathResamplers.Add(path);
            }
        }

        /// <summary>
        /// 最後に出力したファイルのパスのうち，拡張子が指定したものと同じであるものを取得します
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public string getLastUsedPathIn(string extension)
        {
            string ret = getLastUsedPathCore(LastUsedPathIn, extension);
            if (ret.Equals("")) {
                return getLastUsedPathCore(LastUsedPathOut, extension);
            }
            /*if ( !ret.Equals( "" ) ) {
                ret = PortUtil.getDirectoryName( ret );
            }*/
            return ret;
        }

        /// <summary>
        /// 最後に出力したファイルのパスを設定します
        /// </summary>
        /// <param name="path"></param>
        public void setLastUsedPathIn(string path, string ext_with_dot)
        {
            setLastUsedPathCore(LastUsedPathIn, path, ext_with_dot);
        }

        /// <summary>
        /// 最後に入力したファイルのパスのうち，拡張子が指定したものと同じであるものを取得します．
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public string getLastUsedPathOut(string extension)
        {
            string ret = getLastUsedPathCore(LastUsedPathOut, extension);
            if (ret.Equals("")) {
                ret = getLastUsedPathCore(LastUsedPathIn, extension);
            }
            /*if ( !ret.Equals( "" ) ) {
                ret = PortUtil.getDirectoryName( ret );
            }*/
            return ret;
        }

        /// <summary>
        /// 最後に入力したファイルのパスを設定します
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ext_with_dot">ピリオド付きの拡張子（ex. ".txt"）</param>
        public void setLastUsedPathOut(string path, string ext_with_dot)
        {
            setLastUsedPathCore(LastUsedPathOut, path, ext_with_dot);
        }

        public int getControlCurveResolutionValue()
        {
            return ClockResolutionUtility.getValue(ControlCurveResolution);
        }
        #endregion
		/// <summary>
        /// このインスタンスの整合性をチェックします．
        /// </summary>
        public void check()
        {
			if (PathResamplers == null) {
                PathResamplers = new List<string>();
            }

            // SynthEngineの違いを識別しないように変更．VOALOID1に縮約する
            if (DefaultSynthesizer == RendererKind.VOCALOID1_100 ||
                DefaultSynthesizer == RendererKind.VOCALOID1_101) {
                DefaultSynthesizer = RendererKind.VOCALOID1;
            }
        }
    }

}
