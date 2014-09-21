/*
 * sound.midi.cs
 * Copyright © 2011 kbinani
 *
 * This file is part of cadencii.core.
 *
 * cadencii.core is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.core is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;

namespace cadencii.javax.sound.midi
{

    // 本来ならinterface

    public class MidiMessage
    {
        protected byte[] data;
        protected int length;

        /// <summary>
        /// 本来はprotected
        /// </summary>
        /// <param name="data"></param>
        public MidiMessage(byte[] data)
        {
            if (data == null) {
                this.data = new byte[0];
                this.length = 0;
            } else {
                this.data = new byte[data.Length];
                for (int i = 0; i < data.Length; i++) {
                    this.data[i] = (byte)(0xff & data[i]);
                }
                this.length = data.Length;
            }
        }

        public int getLength()
        {
            return length;
        }

        public byte[] getMessage()
        {
            return data;
        }

        public int getStatus()
        {
            if (data != null && data.Length > 0) {
                return data[0];
            }
            return 0;
        }
    }

}
