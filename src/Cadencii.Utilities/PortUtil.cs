/*
 * PortUtil.cs
 * Copyright © 2009-2011 kbinani
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
using System.IO;
using System.Text;

namespace Cadencii.Utilities
{

    public class PortUtil
    {
        private PortUtil()
        {
        }

        public static string formatMessage(string patern, params Object[] args)
        {
            return string.Format(patern, args);
        }

        /// <summary>
        /// 単位は秒
        /// </summary>
        /// <returns></returns>
        public static double getCurrentTime()
        {
            return DateTime.Now.Ticks * 100.0 / 1e9;
        }
        #region BitConverter

        public static byte[] getbytes_int64_le(long data)
        {
            byte[] dat = new byte[8];
            dat[0] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[1] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[2] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[3] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[4] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[5] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[6] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[7] = (byte)(data & (byte)0xff);
            return dat;
        }

        public static byte[] getbytes_uint32_le(long data)
        {
            byte[] dat = new byte[4];
            data = 0xffffffff & data;
            dat[0] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[1] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[2] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[3] = (byte)(data & (byte)0xff);
            return dat;
        }

        public static byte[] getbytes_int32_le(int data)
        {
            long v = data;
            if (v < 0) {
                v += 4294967296L;
            }
            return getbytes_uint32_le(v);
        }

        public static byte[] getbytes_int32_be(int data)
        {
            long v = data;
            if (v < 0) {
                v += 4294967296L;
            }
            return getbytes_uint32_be(v);
        }

        public static byte[] getbytes_int64_be(long data)
        {
            byte[] dat = new byte[8];
            dat[7] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[6] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[5] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[4] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[3] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[2] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[1] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[0] = (byte)(data & (byte)0xff);
            return dat;
        }

        public static byte[] getbytes_uint32_be(long data)
        {
            byte[] dat = new byte[4];
            data = 0xffffffff & data;
            dat[3] = (byte)(data & (byte)0xff);
            data = data >> 8;
            dat[2] = (byte)(data & (byte)0xff);
            data = data >> 8;
            dat[1] = (byte)(data & (byte)0xff);
            data = data >> 8;
            dat[0] = (byte)(data & (byte)0xff);
            return dat;
        }

        public static byte[] getbytes_int16_le(short data)
        {
            int i = data;
            if (i < 0) {
                i += 65536;
            }
            return getbytes_uint16_le(i);
        }

        /// <summary>
        /// compatible to BitConverter
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static byte[] getbytes_uint16_le(int data)
        {
            byte[] dat = new byte[2];
            dat[0] = (byte)(data & (byte)0xff);
            data = (byte)(data >> 8);
            dat[1] = (byte)(data & (byte)0xff);
            return dat;
        }

        public static byte[] getbytes_uint16_be(int data)
        {
            byte[] dat = new byte[2];
            dat[1] = (byte)(data & (byte)0xff);
            data = (byte)(data >> 8);
            dat[0] = (byte)(data & (byte)0xff);
            return dat;
        }

        public static long make_int64_le(byte[] buf)
        {
            return (long)((long)((long)((long)((long)((long)((long)((long)((long)((((0xff & buf[7]) << 8) | (0xff & buf[6])) << 8) | (0xff & buf[5])) << 8) | (0xff & buf[4])) << 8) | (0xff & buf[3])) << 8 | (0xff & buf[2])) << 8) | (0xff & buf[1])) << 8 | (0xff & buf[0]);
        }

        public static long make_int64_be(byte[] buf)
        {
            return (long)((long)((long)((long)((long)((long)((long)((long)((long)((((0xff & buf[0]) << 8) | (0xff & buf[1])) << 8) | (0xff & buf[2])) << 8) | (0xff & buf[3])) << 8) | (0xff & buf[4])) << 8 | (0xff & buf[5])) << 8) | (0xff & buf[6])) << 8 | (0xff & buf[7]);
        }

        public static long make_uint32_le(byte[] buf, int index)
        {
            return (long)((long)((long)((long)(((0xff & buf[index + 3]) << 8) | (0xff & buf[index + 2])) << 8) | (0xff & buf[index + 1])) << 8) | (0xff & buf[index]);
        }

        public static long make_uint32_le(byte[] buf)
        {
            return make_uint32_le(buf, 0);
        }

        public static long make_uint32_be(byte[] buf, int index)
        {
            return (long)((long)((long)((long)(((0xff & buf[index]) << 8) | (0xff & buf[index + 1])) << 8) | (0xff & buf[index + 2])) << 8) | (0xff & buf[index + 3]);
        }

        public static long make_uint32_be(byte[] buf)
        {
            return make_uint32_be(buf, 0);
        }

        public static int make_int32_le(byte[] buf)
        {
            long v = make_uint32_le(buf);
            if (v >= 2147483647L) {
                v -= 4294967296L;
            }
            return (int)v;
        }

        /// <summary>
        /// compatible to BitConverter
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static int make_uint16_le(byte[] buf, int index)
        {
            return (int)((int)((0xff & buf[index + 1]) << 8) | (0xff & buf[index]));
        }

        public static int make_uint16_le(byte[] buf)
        {
            return make_uint16_le(buf, 0);
        }

        public static int make_uint16_be(byte[] buf, int index)
        {
            return (int)((int)((0xff & buf[index]) << 8) | (0xff & buf[index + 1]));
        }

        public static int make_uint16_be(byte[] buf)
        {
            return make_uint16_be(buf, 0);
        }

        /// <summary>
        /// compatible to BitConverter
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static short make_int16_le(byte[] buf, int index)
        {
            int i = make_uint16_le(buf, index);
            if (i >= 32768) {
                i = i - 65536;
            }
            return (short)i;
        }

        public static short make_int16_le(byte[] buf)
        {
            return make_int16_le(buf, 0);
        }

        public static double make_double_le(byte[] buf)
        {
            if (!BitConverter.IsLittleEndian) {
                for (int i = 0; i < 4; i++) {
                    byte d = buf[i];
                    buf[i] = buf[7 - i];
                    buf[7 - i] = d;
                }
            }
            return BitConverter.ToDouble(buf, 0);
        }

        public static double make_double_be(byte[] buf)
        {
            if (BitConverter.IsLittleEndian) {
                for (int i = 0; i < 4; i++) {
                    byte d = buf[i];
                    buf[i] = buf[7 - i];
                    buf[7 - i] = d;
                }
            }
            return BitConverter.ToDouble(buf, 0);
        }

        public static float make_float_le(byte[] buf)
        {
            if (!BitConverter.IsLittleEndian) {
                for (int i = 0; i < 2; i++) {
                    byte d = buf[i];
                    buf[i] = buf[3 - i];
                    buf[3 - i] = d;
                }
            }
            return BitConverter.ToSingle(buf, 0);
        }

        public static float make_float_be(byte[] buf)
        {
            if (BitConverter.IsLittleEndian) {
                for (int i = 0; i < 2; i++) {
                    byte d = buf[i];
                    buf[i] = buf[3 - i];
                    buf[3 - i] = d;
                }
            }
            return BitConverter.ToSingle(buf, 0);
        }

        public static byte[] getbytes_double_le(double value)
        {
            byte[] ret = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian) {
                for (int i = 0; i < 4; i++) {
                    byte d = ret[i];
                    ret[i] = ret[7 - i];
                    ret[7 - i] = d;
                }
            }
            return ret;
        }

        public static byte[] getbytes_double_be(double value)
        {
            byte[] ret = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) {
                for (int i = 0; i < 4; i++) {
                    byte d = ret[i];
                    ret[i] = ret[7 - i];
                    ret[7 - i] = d;
                }
            }
            return ret;
        }

        public static byte[] getbytes_float_le(float value)
        {
            byte[] ret = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian) {
                for (int i = 0; i < 2; i++) {
                    byte d = ret[i];
                    ret[i] = ret[3 - i];
                    ret[3 - i] = d;
                }
            }
            return ret;
        }

        public static byte[] getbytes_float_be(float value)
        {
            byte[] ret = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) {
                for (int i = 0; i < 2; i++) {
                    byte d = ret[i];
                    ret[i] = ret[3 - i];
                    ret[3 - i] = d;
                }
            }
            return ret;
        }
        #endregion

        #region System.IO
        public static double getFileLastModified(string path)
        {
            if (File.Exists(path)) {
                return new FileInfo(path).LastWriteTimeUtc.Ticks * 100.0 / 1e9;
            }
            return 0;
        }

        public static long getFileLength(string fpath)
        {
            return new FileInfo(fpath).Length;
        }

        public static string getExtension(string fpath)
        {
            return Path.GetExtension(fpath);
        }

        public static string getFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public static string getDirectoryName(string path)
        {
            if (path == null) {
                return "";
            }
            if (path.Length == 0) {
                return "";
            }
            return System.IO.Path.GetDirectoryName(path);
        }

        public static string getFileNameWithoutExtension(string path)
        {
            return System.IO.Path.GetFileNameWithoutExtension(path);
        }

        public static string createTempFile()
        {
            return System.IO.Path.GetTempFileName();
        }

        public static string[] listDirectories(string directory)
        {
            return System.IO.Directory.GetDirectories(directory);
        }

        public static string[] listFiles(string directory, string extension)
        {
            return System.IO.Directory.GetFiles(directory, "*" + extension);
        }

        public static void deleteFile(string path)
        {
            System.IO.File.Delete(path);
        }

        public static void moveFile(string pathBefore, string pathAfter)
        {
            System.IO.File.Move(pathBefore, pathAfter);
        }

        [Obsolete]
        public static bool isDirectoryExists(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        [Obsolete]
        public static bool isFileExists(string path)
        {
            return System.IO.File.Exists(path);
        }

        public static string getTempPath()
        {
            return Path.GetTempPath();
        }

        public static void createDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public static void deleteDirectory(string path, bool recurse)
        {
            Directory.Delete(path, recurse);
        }

        public static void deleteDirectory(string path)
        {
            Directory.Delete(path);
        }

        public static void copyFile(string file1, string file2)
        {
            File.Copy(file1, file2);
        }
        #endregion

        #region Number Formatting
        public static bool tryParseInt(string s, ByRef<int> value)
        {
            try {
                value.value = int.Parse(s);
            } catch (Exception ex) {
                return false;
            }
            return true;
        }

        public static bool tryParseFloat(string s, ByRef<float> value)
        {
            try {
                value.value = (float)double.Parse(s);
            } catch (Exception ex) {
                return false;
            }
            return true;
        }

        public static string formatDecimal(string format, double value)
        {
            return value.ToString(format);
        }

        public static string formatDecimal(string format, long value)
        {
            return value.ToString(format);
        }

        public static string toHexString(long value, int digits)
        {
            string ret = toHexString(value);
            int add = digits - getStringLength(ret);
            for (int i = 0; i < add; i++) {
                ret = "0" + ret;
            }
            return ret;
        }

        public static string toHexString(long value)
        {
            return Convert.ToString(value, 16);
        }

        public static long fromHexString(string s)
        {
            return Convert.ToInt64(s, 16);
        }
        #endregion

        #region String Utility
        /// <summary>
        /// 文字列の指定した位置の文字を取得します
        /// </summary>
        /// <param name="s">文字列</param>
        /// <param name="index">位置．先頭が0</param>
        /// <returns></returns>
        public static char charAt(string s, int index)
        {
            return s[index];
        }

        public static string[] splitString(string s, params char[] separator)
        {
            return splitStringCorB(s, separator, int.MaxValue, false);
        }

        public static string[] splitString(string s, char[] separator, int count)
        {
            return splitStringCorB(s, separator, count, false);
        }

        public static string[] splitString(string s, char[] separator, bool ignore_empty_entries)
        {
            return splitStringCorB(s, separator, int.MaxValue, ignore_empty_entries);
        }

        public static string[] splitString(string s, string[] separator, bool ignore_empty_entries)
        {
            return splitStringCorA(s, separator, int.MaxValue, ignore_empty_entries);
        }

        public static string[] splitString(string s, char[] separator, int count, bool ignore_empty_entries)
        {
            return splitStringCorB(s, separator, count, ignore_empty_entries);
        }

        public static string[] splitString(string s, string[] separator, int count, bool ignore_empty_entries)
        {
            return splitStringCorA(s, separator, count, ignore_empty_entries);
        }

        private static string[] splitStringCorB(string s, char[] separator, int count, bool ignore_empty_entries)
        {
            int length = separator.Length;
            string[] spl = new string[length];
            for (int i = 0; i < length; i++) {
                spl[i] = separator[i] + "";
            }
            return splitStringCorA(s, spl, count, false);
        }

        private static string[] splitStringCorA(string s, string[] separator, int count, bool ignore_empty_entries)
        {
            return s.Split(separator, count, (ignore_empty_entries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None));
        }

        public static int getStringLength(string s)
        {
            if (s == null) {
                return 0;
            } else {
                return s.Length;
            }
        }

        public static int getEncodedByteCount(string encoding, string str)
        {
            byte[] buf = getEncodedByte(encoding, str);
            return buf.Length;
        }

        public static byte[] getEncodedByte(string encoding, string str)
        {
            Encoding enc = Encoding.GetEncoding(encoding);
            return enc.GetBytes(str);
        }

        public static string getDecodedString(string encoding, int[] data, int offset, int length)
        {
            Encoding enc = Encoding.GetEncoding(encoding);
            byte[] d = new byte[data.Length];
            for (int i = 0; i < data.Length; i++) {
                d[i] = (byte)data[i];
            }
            return enc.GetString(d, offset, length);
        }

        public static string getDecodedString(string encoding, int[] data)
        {
            return getDecodedString(encoding, data, 0, data.Length);
        }

        #endregion

        public static string getMD5FromString(string str)
        {
            return Misc.getmd5(str);
        }

        public static string getMD5(string file)
        {
            string ret = "";
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read)) {
                ret = Misc.getmd5(fs);
            }
            return ret;
        }

        #region Array conversion
        public static int[] convertIntArray(int[] arr)
        {
            return arr;
        }

        public static long[] convertLongArray(long[] arr)
        {
            return arr;
        }

        public static byte[] convertByteArray(byte[] arr)
        {
            return arr;
        }

        public static float[] convertFloatArray(float[] arr)
        {
            return arr;
        }

        public static char[] convertCharArray(char[] arr)
        {
            return arr;
        }

        #endregion
		
	static string app_startup_path;
	public static void SetApplicationStartupPath (string value)
	{
		app_startup_path = value;
	}
        public static string getApplicationStartupPath()
        {
        	if (app_startup_path == null)
        		throw new InvalidOperationException ("Application startup path must be set before attempt to get.");
            return app_startup_path;
        }
    }

}
