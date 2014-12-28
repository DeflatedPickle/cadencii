/*
 * CurveType.cs
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
using System;

namespace Cadencii.Utilities
{

    enum CurveTypeImpl
    {
        VEL,
        DYN,
        BRE,
        BRI,
        CLE,
        OPE,
        GEN,
        POR,
        PIT,
        PBS,
        VibratoRate,
        VibratoDepth,
        Accent,
        Decay,
        Harmonics,
        Fx2Depth,
        Reso1Freq,
        Reso1Bw,
        Reso1Amp,
        Reso2Freq,
        Reso2Bw,
        Reso2Amp,
        Reso3Freq,
        Reso3Bw,
        Reso3Amp,
        Reso4Freq,
        Reso4Bw,
        Reso4Amp,
        Env,
        Empty,
    }

    /// <summary>
    /// vsqファイルで編集可能なカーブ・プロパティの種類
    /// </summary>
    [Serializable]
    public struct CurveType : IEquatable<CurveType>, IComparable<CurveType>
    {
        private string mName;
        private bool mIsScalar;
        private int mMinimum;
        private int mMaximum;
        private int mDefault;
        private bool mIsAttachNote;
        private int mIndex;
        private CurveTypeImpl mType;

        /// <summary>
        /// ベロシティ(index=-4)
        /// </summary>
        public static readonly CurveType VEL = new CurveType(CurveTypeImpl.VEL, true, true, 0, 127, 64, -4);
        /// <summary>
        /// ダイナミクス　64(index=0)
        /// </summary>
        public static readonly CurveType DYN = new CurveType(CurveTypeImpl.DYN, false, false, 0, 127, 64, 0);
        /// <summary>
        /// ブレシネス　0(index=1)
        /// </summary>
        public static readonly CurveType BRE = new CurveType(CurveTypeImpl.BRE, false, false, 0, 127, 0, 1);
        /// <summary>
        /// ブライトネス　64(index=2)
        /// </summary>
        public static readonly CurveType BRI = new CurveType(CurveTypeImpl.BRI, false, false, 0, 127, 64, 2);
        /// <summary>
        /// クリアネス　0(index=3)
        /// </summary>
        public static readonly CurveType CLE = new CurveType(CurveTypeImpl.CLE, false, false, 0, 127, 0, 3);
        /// <summary>
        /// オープニング　127(index=4)
        /// </summary>
        public static readonly CurveType OPE = new CurveType(CurveTypeImpl.OPE, false, false, 0, 127, 127, 4);
        /// <summary>
        /// ジェンダーファクター　64(index=5)
        /// </summary>
        public static readonly CurveType GEN = new CurveType(CurveTypeImpl.GEN, false, false, 0, 127, 64, 5);
        /// <summary>
        /// ポルタメントタイミング　64(index=6)
        /// </summary>
        public static readonly CurveType POR = new CurveType(CurveTypeImpl.POR, false, false, 0, 127, 64, 6);
        public static readonly CurveType PIT = new CurveType(CurveTypeImpl.PIT, false, false, -8192, 8191, 0, 7);
        public static readonly CurveType PBS = new CurveType(CurveTypeImpl.PBS, false, false, 0, 24, 2, 8);
        /// <summary>
        /// ビブラートの振動の速さ(index=9)
        /// </summary>
        public static readonly CurveType VibratoRate = new CurveType(CurveTypeImpl.VibratoRate, false, true, 0, 127, 64, 9);
        /// <summary>
        /// ビブラートの振幅の大きさ(index=10)
        /// </summary>
        public static readonly CurveType VibratoDepth = new CurveType(CurveTypeImpl.VibratoDepth, false, true, 0, 127, 50, 10);
        /// <summary>
        /// Accent(index=-3)
        /// </summary>
        public static readonly CurveType Accent = new CurveType(CurveTypeImpl.Accent, true, true, 0, 100, 50, -3);
        /// <summary>
        /// Decay(index=-2)
        /// </summary>
        public static readonly CurveType Decay = new CurveType(CurveTypeImpl.Decay, true, true, 0, 100, 50, -2);
        /// <summary>
        /// Harmonics(index=11)
        /// </summary>
        public static readonly CurveType Harmonics = new CurveType(CurveTypeImpl.Harmonics, false, false, 0, 127, 0, 11);
        /// <summary>
        /// FX2Depth(index=12)
        /// </summary>
        public static readonly CurveType Fx2Depth = new CurveType(CurveTypeImpl.Fx2Depth, false, false, 0, 127, 0, 12);
        /// <summary>
        /// reso1freq(index=13)
        /// </summary>
        public static readonly CurveType Reso1Freq = new CurveType(CurveTypeImpl.Reso1Freq, false, false, 0, 127, 0, 13);
        /// <summary>
        /// reso1bw(index=14)
        /// </summary>
        public static readonly CurveType Reso1Bw = new CurveType(CurveTypeImpl.Reso1Bw, false, false, 0, 127, 0, 14);
        /// <summary>
        /// reso1amp(index=15)
        /// </summary>
        public static readonly CurveType Reso1Amp = new CurveType(CurveTypeImpl.Reso1Amp, false, false, 0, 127, 0, 15);
        /// <summary>
        /// reso2freq(index=16)
        /// </summary>
        public static readonly CurveType Reso2Freq = new CurveType(CurveTypeImpl.Reso2Freq, false, false, 0, 127, 0, 16);
        /// <summary>
        /// reso2bw(index=17)
        /// </summary>
        public static readonly CurveType Reso2Bw = new CurveType(CurveTypeImpl.Reso2Bw, false, false, 0, 127, 0, 17);
        /// <summary>
        /// reso2amp(index=18)
        /// </summary>
        public static readonly CurveType Reso2Amp = new CurveType(CurveTypeImpl.Reso2Amp, false, false, 0, 127, 0, 18);
        /// <summary>
        /// reso3freq(index=19)
        /// </summary>
        public static readonly CurveType Reso3Freq = new CurveType(CurveTypeImpl.Reso3Freq, false, false, 0, 127, 0, 19);
        /// <summary>
        /// reso3bw(index=20)
        /// </summary>
        public static readonly CurveType Reso3Bw = new CurveType(CurveTypeImpl.Reso3Bw, false, false, 0, 127, 0, 20);
        /// <summary>
        /// reso3amp(index=21)
        /// </summary>
        public static readonly CurveType Reso3Amp = new CurveType(CurveTypeImpl.Reso3Amp, false, false, 0, 127, 0, 21);
        /// <summary>
        /// reso4freq(index=22)
        /// </summary>
        public static readonly CurveType Reso4Freq = new CurveType(CurveTypeImpl.Reso4Freq, false, false, 0, 127, 0, 22);
        /// <summary>
        /// reso4bw(index=23)
        /// </summary>
        public static readonly CurveType Reso4Bw = new CurveType(CurveTypeImpl.Reso4Bw, false, false, 0, 127, 0, 23);
        /// <summary>
        /// reso4amp(index=24)
        /// </summary>
        public static readonly CurveType Reso4Amp = new CurveType(CurveTypeImpl.Reso4Amp, false, false, 0, 127, 0, 24);
        public static readonly CurveType Env = new CurveType(CurveTypeImpl.Env, true, true, 0, 200, 100, -1);

        public static readonly CurveType Empty = new CurveType(CurveTypeImpl.Empty, false, false, 0, 0, 0, -1);

        private CurveType(CurveTypeImpl type_impl, bool is_scalar, bool is_attach_note, int min, int max, int defalt_value, int index)
        {
            mType = type_impl;
            mIsScalar = is_scalar;
            mMinimum = min;
            mMaximum = max;
            mDefault = defalt_value;
            mIsAttachNote = is_attach_note;
            mIndex = index;
            if (mType == CurveTypeImpl.VEL) {
                mName = "VEL";
            } else if (mType == CurveTypeImpl.DYN) {
                mName = "DYN";
            } else if (mType == CurveTypeImpl.BRE) {
                mName = "BRE";
            } else if (mType == CurveTypeImpl.BRI) {
                mName = "BRI";
            } else if (mType == CurveTypeImpl.CLE) {
                mName = "CLE";
            } else if (mType == CurveTypeImpl.OPE) {
                mName = "OPE";
            } else if (mType == CurveTypeImpl.GEN) {
                mName = "GEN";
            } else if (mType == CurveTypeImpl.POR) {
                mName = "POR";
            } else if (mType == CurveTypeImpl.PIT) {
                mName = "PIT";
            } else if (mType == CurveTypeImpl.PBS) {
                mName = "PBS";
            } else if (mType == CurveTypeImpl.VibratoRate) {
                mName = "V-Rate";
            } else if (mType == CurveTypeImpl.VibratoDepth) {
                mName = "V-Depth";
            } else if (mType == CurveTypeImpl.Accent) {
                mName = "Accent";
            } else if (mType == CurveTypeImpl.Decay) {
                mName = "Decay";
            } else if (mType == CurveTypeImpl.Harmonics) {
                mName = "Harm";
            } else if (mType == CurveTypeImpl.Fx2Depth) {
                mName = "fx2dep";
            } else if (mType == CurveTypeImpl.Reso1Freq) {
                mName = "res1freq";
            } else if (mType == CurveTypeImpl.Reso1Bw) {
                mName = "res1bw";
            } else if (mType == CurveTypeImpl.Reso1Amp) {
                mName = "res1amp";
            } else if (mType == CurveTypeImpl.Reso2Freq) {
                mName = "res2freq";
            } else if (mType == CurveTypeImpl.Reso2Bw) {
                mName = "res2bw";
            } else if (mType == CurveTypeImpl.Reso2Amp) {
                mName = "res2amp";
            } else if (mType == CurveTypeImpl.Reso3Freq) {
                mName = "res3freq";
            } else if (mType == CurveTypeImpl.Reso3Bw) {
                mName = "res3bw";
            } else if (mType == CurveTypeImpl.Reso3Amp) {
                mName = "res3amp";
            } else if (mType == CurveTypeImpl.Reso4Freq) {
                mName = "res4freq";
            } else if (mType == CurveTypeImpl.Reso4Bw) {
                mName = "res4bw";
            } else if (mType == CurveTypeImpl.Reso4Amp) {
                mName = "res4amp";
            } else if (mType == CurveTypeImpl.Env) {
                mName = "Env";
            } else {
#if DEBUG
                Logger.StdOut("CurveType#.ctor; mType=" + mType);
#endif
                mName = "Empty";
            }
        }

        public Object clone()
        {
            return new CurveType(this.mType, this.mIsScalar, this.mIsAttachNote, this.mMinimum, this.mMaximum, this.mDefault, this.mIndex);
        }

        public int compareTo(CurveType item)
        {
            if (mIndex < 0) {
                if (item.mIndex < 0) {
                    return mType.CompareTo(item.mType);
                } else {
                    return 1;
                }
            } else {
                if (item.mIndex < 0) {
                    return -1;
                } else {
                    return mIndex - item.mIndex;
                }
            }
        }

        public int CompareTo(CurveType item)
        {
            return compareTo(item);
        }

        public override string ToString()
        {
            return toString();
        }

        public string toString()
        {
            return getName();
        }

        public int getIndex()
        {
            return mIndex;
        }

        public string getName()
        {
            return mName;
        }

        public bool isAttachNote()
        {
            return mIsAttachNote;
        }

        public bool isScalar()
        {
            return mIsScalar;
        }

        public int getMaximum()
        {
            return mMaximum;
        }

        public int getMinimum()
        {
            return mMinimum;
        }

        public int getDefault()
        {
            return mDefault;
        }

        public bool equals(CurveType other)
        {
            return (mType == other.mType) && (mIsScalar == other.mIsScalar);
        }

        public bool Equals(CurveType obj)
        {
            return this.equals(obj);
        }
    }

}
