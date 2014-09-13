#if ENABLE_VOCALOID
/*
 * VocaloidWaveGenerator.cs
 * Copyright © 2010-2011 kbinani
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
using System.Threading;
using System.IO;
using System.Text;
using cadencii;
using cadencii.java.awt;
using cadencii.java.io;
using cadencii.java.util;
using cadencii.media;
using cadencii.vsq;

namespace cadencii
{

    /// <summary>
    /// ドライバーからの波形を受け取るためのインターフェース
    /// </summary>
    public interface IWaveIncoming
    {
        /// <summary>
        /// ドライバから波形を受け取るためのコールバック関数
        /// </summary>
        /// <param name="l">左チャンネルの波形データ</param>
        /// <param name="r">右チャンネルの波形データ</param>
        /// <param name="length">波形データの長さ。配列の長さよりも短い場合がある</param>
        void waveIncomingImpl(double[] l, double[] r, int length, WorkerState state);
    }
}

namespace cadencii
{

}
#endif
