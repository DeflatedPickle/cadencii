﻿/*
 * SequenceConfig.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

#else
using System;

namespace org.kbinani.cadencii
{
    using boolean = System.Boolean;
#endif

    public class SequenceConfig
    {
        public int SamplingRate = 44100;
        /// <summary>
        /// waveファイル出力時のチャンネル数（1または2）
        /// <version>3.3+</version>
        /// </summary>
        public int WaveFileOutputChannel = 2;
        /// <summary>
        /// waveファイル出力時に、全トラックをmixして出力するかどうか
        /// <version>3.3+</version>
        /// </summary>
        public boolean WaveFileOutputFromMasterTrack = false;

        public boolean equals( SequenceConfig item )
        {
            if ( item == null ) {
                return false;
            }
            if ( this.SamplingRate != item.SamplingRate ) {
                return false;
            }
            if ( this.WaveFileOutputFromMasterTrack != item.WaveFileOutputFromMasterTrack ) {
                return false;
            }
            if ( this.WaveFileOutputChannel != item.WaveFileOutputChannel ) {
                return false;
            }
            return true;
        }

        public Object clone()
        {
            SequenceConfig config = new SequenceConfig();
            config.SamplingRate = this.SamplingRate;
            config.WaveFileOutputChannel = this.WaveFileOutputChannel;
            config.WaveFileOutputFromMasterTrack = this.WaveFileOutputFromMasterTrack;
            return config;
        }
    }

#if !JAVA
}
#endif
