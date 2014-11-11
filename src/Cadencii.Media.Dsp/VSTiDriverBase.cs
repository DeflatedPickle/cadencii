#if ENABLE_VOCALOID
/*
 * VSTiDriverBase.cs
 * Copyright © 2008-2013 kbinani
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
#define TEST

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using cadencii.java.util;
using Cadencii.Media.Vsq;
using cadencii.dsp;
using VstSdk;
using Cadencii.Platform.Windows;

namespace cadencii
{
    using VstInt32 = Int32;
    using VstIntPtr = Int32;


    public struct TempoInfo
    {
        /// <summary>
        /// テンポが変更される時刻を表すクロック数
        /// </summary>
        public int Clock;
        /// <summary>
        /// テンポ
        /// </summary>
        public int Tempo;
        /// <summary>
        /// テンポが変更される時刻
        /// </summary>
        public double TotalSec;
    }

    public abstract class VSTiDriverBase
    {
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        protected delegate IntPtr PVSTMAIN([MarshalAs(UnmanagedType.FunctionPtr)]audioMasterCallback audioMaster);

        public bool loaded = false;
        public string path = "";

        protected PVSTMAIN mainDelegate;
        private IntPtr mainProcPointer;
        protected audioMasterCallback audioMaster;
        /// <summary>
        /// 読込んだdllから作成したVOCALOID2の本体。VOCALOID2への操作はs_aeffect->dispatcherで行う
        /// </summary>
        public AEffectWrapper aEffect;
        protected IntPtr aEffectPointer;
        /// <summary>
        /// 読込んだdllのハンドル
        /// </summary>
        protected IntPtr dllHandle;
        /// <summary>
        /// 波形バッファのサイズ。
        /// </summary>
        protected int blockSize;
        /// <summary>
        /// サンプリングレート。VOCALOID2 VSTiは限られたサンプリングレートしか受け付けない。たいてい44100Hzにする
        /// </summary>
        protected int sampleRate;
        /// <summary>
        /// バッファ(bufferLeft, bufferRight)の長さ
        /// </summary>
        const int BUFLEN = 44100;
        /// <summary>
        /// 左チャンネル用バッファ
        /// </summary>
        private IntPtr bufferLeft = IntPtr.Zero;
        /// <summary>
        /// 右チャンネル用バッファ
        /// </summary>
        private IntPtr bufferRight = IntPtr.Zero;
        /// <summary>
        /// 左右チャンネルバッファの配列(buffers={bufferLeft, bufferRight})
        /// </summary>
        private IntPtr buffers = IntPtr.Zero;
        /// <summary>
        /// パラメータの，ロード時のデフォルト値
        /// </summary>
        private float[] paramDefaults = null;
        /* /// <summary>
        /// win32.LoadLibraryExを使うかどうか。trueならwin32.LoadLibraryExを使い、falseならutil.dllのLoadDllをつかう。既定ではtrue
        /// </summary>
        private bool useNativeDllLoader = true;*/
        protected MemoryManager memoryManager = new MemoryManager();
        private Object mSyncRoot = new Object();

        readonly DspUIHost uihost;

        /// <summary>
        /// このドライバが担当する、合成エンジンの種類を取得する
        /// </summary>
        /// <returns>合成エンジンの種類</returns>
        public abstract RendererKind getRendererKind();

        public int getSampleRate()
        {
            return sampleRate;
        }

        public void resetAllParameters()
        {
            if (paramDefaults == null) {
                return;
            }
            for (int i = 0; i < paramDefaults.Length; i++) {
                setParameter(i, paramDefaults[i]);
            }
        }

        public virtual float getParameter(int index)
        {
            float ret = 0.0f;
            try {
                ret = aEffect.GetParameter(index);
            } catch (Exception ex) {
                Logger.StdErr("vstidrv#getParameter; ex=" + ex);
            }
            return ret;
        }

        public virtual void setParameter(int index, float value)
        {
            try {
                aEffect.SetParameter(index, value);
            } catch (Exception ex) {
                Logger.StdErr("vstidrv#setParameter; ex=" + ex);
            }
        }

        public string getStringCore(int opcode, int index, int str_capacity)
        {
            byte[] arr = new byte[str_capacity + 1];
            for (int i = 0; i < str_capacity; i++) {
                arr[i] = 0;
            }
            IntPtr ptr = IntPtr.Zero;
            try {
                unsafe {
                    fixed (byte* bptr = &arr[0]) {
                        ptr = new IntPtr(bptr);
                        aEffect.Dispatch(opcode, index, 0, ptr, 0.0f);
                    }
                }
            } catch (Exception ex) {
                Logger.StdErr("vstidrv#getStringCore; ex=" + ex);
            }
            string ret = Encoding.ASCII.GetString(arr);
            return ret;
        }

        public string getParameterDisplay(int index)
        {
            return getStringCore(AEffectOpcodes.effGetParamDisplay, index, VstStringConstants.kVstMaxParamStrLen);
        }

        public string getParameterLabel(int index)
        {
            return getStringCore(AEffectOpcodes.effGetParamLabel, index, VstStringConstants.kVstMaxParamStrLen);
        }

        public string getParameterName(int index)
        {
            return getStringCore(AEffectOpcodes.effGetParamName, index, VstStringConstants.kVstMaxParamStrLen);
        }

        private void initBuffer()
        {
            if (bufferLeft == IntPtr.Zero) {
                bufferLeft = Marshal.AllocHGlobal(sizeof(float) * BUFLEN);
            }
            if (bufferRight == IntPtr.Zero) {
                bufferRight = Marshal.AllocHGlobal(sizeof(float) * BUFLEN);
            }
            if (buffers == IntPtr.Zero) {
                unsafe {
                    buffers = Marshal.AllocHGlobal(sizeof(float*) * 2);
                }
            }
        }

        private void releaseBuffer()
        {
            if (bufferLeft != IntPtr.Zero) {
                Marshal.FreeHGlobal(bufferLeft);
                bufferLeft = IntPtr.Zero;
            }
            if (bufferRight != IntPtr.Zero) {
                Marshal.FreeHGlobal(bufferRight);
                bufferRight = IntPtr.Zero;
            }
            if (buffers != IntPtr.Zero) {
                Marshal.FreeHGlobal(buffers);
                buffers = IntPtr.Zero;
            }
        }

        public void process(double[] left, double[] right, int length)
        {
            process<double>(left, right, length, _ => _);
        }

        public void process(float[] left, float[] right, int length)
        {
            process<float>(left, right, length, _ => _);
        }

        private void process<T>(T[] left, T[] right, int length, Func<float, T> convert)
        {
            if (left == null || right == null) {
                return;
            }
            try {
                initBuffer();
                int remain = length;
                int offset = 0;
                unsafe {
                    float* left_ch = (float*)bufferLeft.ToPointer();
                    float* right_ch = (float*)bufferRight.ToPointer();
                    for (int i = 0; i < BUFLEN; ++i) {
                        left_ch[i] = 0;
                        right_ch[i] = 0;
                    }
                    float** out_buffer = (float**)buffers.ToPointer();
                    out_buffer[0] = left_ch;
                    out_buffer[1] = right_ch;
                    while (remain > 0) {
                        int proc = (remain > BUFLEN) ? BUFLEN : remain;
                        aEffect.ProcessReplacing(IntPtr.Zero, new IntPtr(out_buffer), proc);
                        for (int i = 0; i < proc; i++) {
                            left[i + offset] = convert(left_ch[i]);
                            right[i + offset] = convert(right_ch[i]);
                        }
                        remain -= proc;
                        offset += proc;
                    }
                }
            } catch (Exception ex) {
                Logger.StdErr("vstidrv#process; ex=" + ex);
            }
        }

        public virtual void send(MidiEvent[] events, int delta_clocks = 0)
        {
            if (events.Length == 0) return;
            unsafe {
                MemoryManager mman = null;
                try {
                    mman = new MemoryManager();
                    int nEvents = events.Length;
                    VstEvents* pVSTEvents = (VstEvents*)mman.malloc(sizeof(VstEvent) + nEvents * sizeof(VstEvent*)).ToPointer();
                    pVSTEvents->numEvents = 0;
                    pVSTEvents->reserved = (VstIntPtr)0;

                    for (int i = 0; i < nEvents; i++) {
                        MidiEvent pProcessEvent = events[i];
                        //byte event_code = (byte)pProcessEvent.firstByte;
                        VstEvent* pVSTEvent = (VstEvent*)0;
                        VstMidiEvent* pMidiEvent;
                        pMidiEvent = (VstMidiEvent*)mman.malloc((int)(sizeof(VstMidiEvent) + (pProcessEvent.data.Length + 1) * sizeof(byte))).ToPointer();
                        pMidiEvent->byteSize = sizeof(VstMidiEvent);
                        pMidiEvent->deltaFrames = delta_clocks;
                        pMidiEvent->detune = 0;
                        pMidiEvent->flags = 1;
                        pMidiEvent->noteLength = 0;
                        pMidiEvent->noteOffset = 0;
                        pMidiEvent->noteOffVelocity = 0;
                        pMidiEvent->reserved1 = 0;
                        pMidiEvent->reserved2 = 0;
                        pMidiEvent->type = VstEventTypes.kVstMidiType;
                        pMidiEvent->midiData[0] = (byte)(0xff & pProcessEvent.firstByte);
                        for (int j = 0; j < pProcessEvent.data.Length; j++) {
                            pMidiEvent->midiData[j + 1] = (byte)(0xff & pProcessEvent.data[j]);
                        }
                        pVSTEvents->events[pVSTEvents->numEvents++] = (int)(VstEvent*)pMidiEvent;
                    }
                    aEffect.Dispatch(AEffectXOpcodes.effProcessEvents, 0, 0, new IntPtr(pVSTEvents), 0);
                } catch (Exception ex) {
                    Logger.StdErr("vstidrv#send; ex=" + ex);
                } finally {
                    if (mman != null) {
                        try {
                            mman.dispose();
                        } catch (Exception ex2) {
                            Logger.StdErr("vstidrv#send; ex2=" + ex2);
                        }
                    }
                }
            }
        }

        protected virtual VstIntPtr AudioMaster(ref AEffect effect, VstInt32 opcode, VstInt32 index, VstIntPtr value, IntPtr ptr, float opt)
        {
            VstIntPtr result = 0;
            switch (opcode) {
                case AudioMasterOpcodes.audioMasterVersion: {
                    result = Constants.kVstVersion;
                    break;
                }
            }
            return result;
        }

        public PluginUI getUi (UiForm window)
		{
			return uihost.GetPluginUI (window);
		}

        public virtual void setSampleRate(int sample_rate)
        {
            sampleRate = sample_rate;
            int ret1 = aEffect.Dispatch(AEffectOpcodes.effSetSampleRate, 0, 0, IntPtr.Zero, (float)sampleRate);
            int ret2 = aEffect.Dispatch(AEffectOpcodes.effSetBlockSize, 0, sampleRate, IntPtr.Zero, 0);
#if DEBUG
            Logger.StdOut("vstidrv#setSampleRate; ret1=" + ret1 + "; ret2=" + ret2);
#endif
        }

        public virtual bool open(int block_size, int sample_rate)
        {
            dllHandle = win32.LoadLibraryExW(path, IntPtr.Zero, win32.LOAD_WITH_ALTERED_SEARCH_PATH);
            if (dllHandle == IntPtr.Zero) {
                Logger.StdErr("vstidrv#open; dllHandle is null");
                return false;
            }

            mainProcPointer = win32.GetProcAddress(dllHandle, "main");
            mainDelegate = (PVSTMAIN)Marshal.GetDelegateForFunctionPointer(mainProcPointer,
                                                                            typeof(PVSTMAIN));
            if (mainDelegate == null) {
                Logger.StdErr("vstidrv#open; mainDelegate is null");
                return false;
            }

            audioMaster = new audioMasterCallback(AudioMaster);
            if (audioMaster == null) {
                Logger.StdErr("vstidrv#open; audioMaster is null");
                return false;
            }

            aEffectPointer = IntPtr.Zero;
            try {
                aEffectPointer = mainDelegate(audioMaster);
            } catch (Exception ex) {
                Logger.StdErr("vstidrv#open; ex=" + ex);
                return false;
            }
            if (aEffectPointer == IntPtr.Zero) {
                Logger.StdErr("vstidrv#open; aEffectPointer is null");
                return false;
            }
            blockSize = block_size;
            sampleRate = sample_rate;
            aEffect = new AEffectWrapper();
            aEffect.aeffect = (AEffect)Marshal.PtrToStructure(aEffectPointer, typeof(AEffect));
            aEffect.Dispatch(AEffectOpcodes.effOpen, 0, 0, IntPtr.Zero, 0);
            int ret = aEffect.Dispatch(AEffectOpcodes.effSetSampleRate, 0, 0, IntPtr.Zero, (float)sampleRate);
#if DEBUG
            Logger.StdOut("vstidrv#open; dll_path=" + path + "; ret for effSetSampleRate=" + ret);
#endif

            aEffect.Dispatch(AEffectOpcodes.effSetBlockSize, 0, blockSize, IntPtr.Zero, 0);

            // デフォルトのパラメータ値を取得
            int num = aEffect.aeffect.numParams;
            paramDefaults = new float[num];
            for (int i = 0; i < num; i++) {
                paramDefaults[i] = aEffect.GetParameter(i);
            }

            return true;
        }

        public virtual void close()
        {
            lock (mSyncRoot) {
#if TEST
                Logger.StdOut("vstidrv#close");
#endif
                uihost.ClosePluginUI ();
                try {
                    Logger.StdOut("vstidrv#close; (aEffect==null)=" + (aEffect == null));
                    if (aEffect != null) {
                        aEffect.Dispatch(AEffectOpcodes.effClose, 0, 0, IntPtr.Zero, 0.0f);
                    }
                    Logger.StdOut("vstidrv#close; dllHandle=" + dllHandle);
                    if (dllHandle != IntPtr.Zero) {
                        win32.FreeLibrary(dllHandle);
                    }
                    aEffect = null;
                    dllHandle = IntPtr.Zero;
                    mainDelegate = null;
                    audioMaster = null;
                } catch (Exception ex) {
                    Logger.StdErr("vstidrv#close; ex=" + ex);
                }
                releaseBuffer();
            }
        }

        protected VSTiDriverBase ()
        {
        	uihost = DspUIHost.Create (this);
        }

        ~VSTiDriverBase()
        {
            lock (mSyncRoot) {
                close();
            }
        }
    }

}
#endif // ENABLE_VOCALOID
