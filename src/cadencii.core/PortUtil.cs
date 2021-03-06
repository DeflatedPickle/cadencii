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
#if JAVA
package cadencii;

import java.awt.AWTEvent;
import java.awt.Color;
import java.awt.Component;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.FontMetrics;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.GraphicsDevice;
import java.awt.GraphicsEnvironment;
import java.awt.MouseInfo;
import java.awt.Point;
import java.awt.Rectangle;
import java.awt.Toolkit;
import java.awt.Window;
import java.awt.datatransfer.Clipboard;
import java.awt.datatransfer.DataFlavor;
import java.awt.datatransfer.StringSelection;
import java.awt.datatransfer.Transferable;
import java.awt.event.AWTEventListener;
import java.awt.event.InputEvent;
import java.awt.event.KeyEvent;
import java.awt.geom.CubicCurve2D;
import java.awt.Robot;
import java.awt.AWTException;
import java.io.File;
import java.io.FileFilter;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.nio.ByteBuffer;
import java.nio.channels.FileChannel;
import java.nio.charset.Charset;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.text.DecimalFormat;
import java.text.MessageFormat;
import java.util.Date;
import java.util.Vector;

#else
using System;
using System.IO;
using System.Text;
using cadencii.java.awt;
using cadencii.java.awt.event_;
using cadencii.java.util;
using cadencii.javax.swing;

namespace cadencii {
    using boolean = System.Boolean;
    using Float = System.Single;
    using Integer = System.Int32;
    using Long = System.Int64;
    using Character = System.Char;
#endif

#if JAVA
    public class PortUtil implements AWTEventListener
#else
    public class PortUtil
#endif
    {

#if JAVA
        private static boolean s_ctrl = false;
        private static boolean s_shift = false;
        private static boolean s_alt = false;
        private static boolean s_meta = false;
        private static PortUtil s_instance = null;
#endif

        public static Color AliceBlue = new Color( 240, 248, 255 );
        public static Color AntiqueWhite = new Color( 250, 235, 215 );
        public static Color Aqua = new Color( 0, 255, 255 );
        public static Color Aquamarine = new Color( 127, 255, 212 );
        public static Color Azure = new Color( 240, 255, 255 );
        public static Color Beige = new Color( 245, 245, 220 );
        public static Color Bisque = new Color( 255, 228, 196 );
        public static Color Black = new Color( 0, 0, 0 );
        public static Color BlanchedAlmond = new Color( 255, 235, 205 );
        public static Color Blue = new Color( 0, 0, 255 );
        public static Color BlueViolet = new Color( 138, 43, 226 );
        public static Color Brown = new Color( 165, 42, 42 );
        public static Color BurlyWood = new Color( 222, 184, 135 );
        public static Color CadetBlue = new Color( 95, 158, 160 );
        public static Color Chartreuse = new Color( 127, 255, 0 );
        public static Color Chocolate = new Color( 210, 105, 30 );
        public static Color Coral = new Color( 255, 127, 80 );
        public static Color CornflowerBlue = new Color( 100, 149, 237 );
        public static Color Cornsilk = new Color( 255, 248, 220 );
        public static Color Crimson = new Color( 220, 20, 60 );
        public static Color Cyan = new Color( 0, 255, 255 );
        public static Color DarkBlue = new Color( 0, 0, 139 );
        public static Color DarkCyan = new Color( 0, 139, 139 );
        public static Color DarkGoldenrod = new Color( 184, 134, 11 );
        public static Color DarkGray = new Color( 169, 169, 169 );
        public static Color DarkGreen = new Color( 0, 100, 0 );
        public static Color DarkKhaki = new Color( 189, 183, 107 );
        public static Color DarkMagenta = new Color( 139, 0, 139 );
        public static Color DarkOliveGreen = new Color( 85, 107, 47 );
        public static Color DarkOrange = new Color( 255, 140, 0 );
        public static Color DarkOrchid = new Color( 153, 50, 204 );
        public static Color DarkRed = new Color( 139, 0, 0 );
        public static Color DarkSalmon = new Color( 233, 150, 122 );
        public static Color DarkSeaGreen = new Color( 143, 188, 139 );
        public static Color DarkSlateBlue = new Color( 72, 61, 139 );
        public static Color DarkSlateGray = new Color( 47, 79, 79 );
        public static Color DarkTurquoise = new Color( 0, 206, 209 );
        public static Color DarkViolet = new Color( 148, 0, 211 );
        public static Color DeepPink = new Color( 255, 20, 147 );
        public static Color DeepSkyBlue = new Color( 0, 191, 255 );
        public static Color DimGray = new Color( 105, 105, 105 );
        public static Color DodgerBlue = new Color( 30, 144, 255 );
        public static Color Firebrick = new Color( 178, 34, 34 );
        public static Color FloralWhite = new Color( 255, 250, 240 );
        public static Color ForestGreen = new Color( 34, 139, 34 );
        public static Color Fuchsia = new Color( 255, 0, 255 );
        public static Color Gainsboro = new Color( 220, 220, 220 );
        public static Color GhostWhite = new Color( 248, 248, 255 );
        public static Color Gold = new Color( 255, 215, 0 );
        public static Color Goldenrod = new Color( 218, 165, 32 );
        public static Color Gray = new Color( 128, 128, 128 );
        public static Color Green = new Color( 0, 128, 0 );
        public static Color GreenYellow = new Color( 173, 255, 47 );
        public static Color Honeydew = new Color( 240, 255, 240 );
        public static Color HotPink = new Color( 255, 105, 180 );
        public static Color IndianRed = new Color( 205, 92, 92 );
        public static Color Indigo = new Color( 75, 0, 130 );
        public static Color Ivory = new Color( 255, 255, 240 );
        public static Color Khaki = new Color( 240, 230, 140 );
        public static Color Lavender = new Color( 230, 230, 250 );
        public static Color LavenderBlush = new Color( 255, 240, 245 );
        public static Color LawnGreen = new Color( 124, 252, 0 );
        public static Color LemonChiffon = new Color( 255, 250, 205 );
        public static Color LightBlue = new Color( 173, 216, 230 );
        public static Color LightCoral = new Color( 240, 128, 128 );
        public static Color LightCyan = new Color( 224, 255, 255 );
        public static Color LightGoldenrodYellow = new Color( 250, 250, 210 );
        public static Color LightGreen = new Color( 144, 238, 144 );
        public static Color LightGray = new Color( 211, 211, 211 );
        public static Color LightPink = new Color( 255, 182, 193 );
        public static Color LightSalmon = new Color( 255, 160, 122 );
        public static Color LightSeaGreen = new Color( 32, 178, 170 );
        public static Color LightSkyBlue = new Color( 135, 206, 250 );
        public static Color LightSlateGray = new Color( 119, 136, 153 );
        public static Color LightSteelBlue = new Color( 176, 196, 222 );
        public static Color LightYellow = new Color( 255, 255, 224 );
        public static Color Lime = new Color( 0, 255, 0 );
        public static Color LimeGreen = new Color( 50, 205, 50 );
        public static Color Linen = new Color( 250, 240, 230 );
        public static Color Magenta = new Color( 255, 0, 255 );
        public static Color Maroon = new Color( 128, 0, 0 );
        public static Color MediumAquamarine = new Color( 102, 205, 170 );
        public static Color MediumBlue = new Color( 0, 0, 205 );
        public static Color MediumOrchid = new Color( 186, 85, 211 );
        public static Color MediumPurple = new Color( 147, 112, 219 );
        public static Color MediumSeaGreen = new Color( 60, 179, 113 );
        public static Color MediumSlateBlue = new Color( 123, 104, 238 );
        public static Color MediumSpringGreen = new Color( 0, 250, 154 );
        public static Color MediumTurquoise = new Color( 72, 209, 204 );
        public static Color MediumVioletRed = new Color( 199, 21, 133 );
        public static Color MidnightBlue = new Color( 25, 25, 112 );
        public static Color MintCream = new Color( 245, 255, 250 );
        public static Color MistyRose = new Color( 255, 228, 225 );
        public static Color Moccasin = new Color( 255, 228, 181 );
        public static Color NavajoWhite = new Color( 255, 222, 173 );
        public static Color Navy = new Color( 0, 0, 128 );
        public static Color OldLace = new Color( 253, 245, 230 );
        public static Color Olive = new Color( 128, 128, 0 );
        public static Color OliveDrab = new Color( 107, 142, 35 );
        public static Color Orange = new Color( 255, 165, 0 );
        public static Color OrangeRed = new Color( 255, 69, 0 );
        public static Color Orchid = new Color( 218, 112, 214 );
        public static Color PaleGoldenrod = new Color( 238, 232, 170 );
        public static Color PaleGreen = new Color( 152, 251, 152 );
        public static Color PaleTurquoise = new Color( 175, 238, 238 );
        public static Color PaleVioletRed = new Color( 219, 112, 147 );
        public static Color PapayaWhip = new Color( 255, 239, 213 );
        public static Color PeachPuff = new Color( 255, 218, 185 );
        public static Color Peru = new Color( 205, 133, 63 );
        public static Color Pink = new Color( 255, 192, 203 );
        public static Color Plum = new Color( 221, 160, 221 );
        public static Color PowderBlue = new Color( 176, 224, 230 );
        public static Color Purple = new Color( 128, 0, 128 );
        public static Color Red = new Color( 255, 0, 0 );
        public static Color RosyBrown = new Color( 188, 143, 143 );
        public static Color RoyalBlue = new Color( 65, 105, 225 );
        public static Color SaddleBrown = new Color( 139, 69, 19 );
        public static Color Salmon = new Color( 250, 128, 114 );
        public static Color SandyBrown = new Color( 244, 164, 96 );
        public static Color SeaGreen = new Color( 46, 139, 87 );
        public static Color SeaShell = new Color( 255, 245, 238 );
        public static Color Sienna = new Color( 160, 82, 45 );
        public static Color Silver = new Color( 192, 192, 192 );
        public static Color SkyBlue = new Color( 135, 206, 235 );
        public static Color SlateBlue = new Color( 106, 90, 205 );
        public static Color SlateGray = new Color( 112, 128, 144 );
        public static Color Snow = new Color( 255, 250, 250 );
        public static Color SpringGreen = new Color( 0, 255, 127 );
        public static Color SteelBlue = new Color( 70, 130, 180 );
        public static Color Tan = new Color( 210, 180, 140 );
        public static Color Teal = new Color( 0, 128, 128 );
        public static Color Thistle = new Color( 216, 191, 216 );
        public static Color Tomato = new Color( 255, 99, 71 );
        public static Color Turquoise = new Color( 64, 224, 208 );
        public static Color Violet = new Color( 238, 130, 238 );
        public static Color Wheat = new Color( 245, 222, 179 );
        public static Color White = new Color( 255, 255, 255 );
        public static Color WhiteSmoke = new Color( 245, 245, 245 );
        public static Color Yellow = new Color( 255, 255, 0 );
        public static Color YellowGreen = new Color( 154, 205, 50 );

        public const int YES_OPTION = 0;
        public const int NO_OPTION = 1;
        public const int CANCEL_OPTION = 2;
        public const int OK_OPTION = 0;
        public const int CLOSED_OPTION = -1;
#if JAVA
        static{
            s_instance = new PortUtil();
            Toolkit.getDefaultToolkit().addAWTEventListener( s_instance, AWTEvent.KEY_EVENT_MASK );
        }
#else
        static PortUtil() {
        }
#endif

        private PortUtil() {
        }

        /// <summary>
        /// java:コンポーネントのnameプロパティを返します。C#:コントロールのNameプロパティを返します。
        /// objがnullだったり、型がComponent/Controlでない場合は空文字を返します。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String getComponentName( Object obj ) {
            if ( obj == null ) {
                return "";
            }
#if JAVA
            if( obj instanceof Component ){
                String s = ((Component)obj).getName();
                if( s == null ){
                    s = "";
                }
                return s;
            }else{
                return "";
            }
#else
            if ( obj is System.Windows.Forms.Control ) {
                return ((System.Windows.Forms.Control)obj).Name;
            } else {
                return "";
            }
#endif
        }

#if JAVA
        public static String formatMessage( String patern, Object... args ){
            return MessageFormat.format( patern, args );
        }
#else
        public static String formatMessage( String patern, params Object[] args ) {
            return String.Format( patern, args );
        }
#endif

        /// <summary>
        /// 単位は秒
        /// </summary>
        /// <returns></returns>
        public static double getCurrentTime() {
#if JAVA
            return new Date().getTime() / 1000.0;
#else
            return DateTime.Now.Ticks * 100.0 / 1e9;
#endif
        }

#if JAVA
        public void eventDispatched( AWTEvent e ){
            if( e instanceof KeyEvent ){
                KeyEvent ke = (KeyEvent)e;
                int code = ke.getKeyCode();
                int id = ke.getID();
                if( id == KeyEvent.KEY_PRESSED ){
                    if( code == KeyEvent.VK_ALT ){
                        s_alt = true;
                    }else if( code == KeyEvent.VK_CONTROL ){
                        s_ctrl = true;
                    }else if( code == KeyEvent.VK_SHIFT ){
                        s_shift = true;
                    }else if( code == KeyEvent.VK_META ){
                        s_meta = true;
                    }
                }else if( id == KeyEvent.KEY_RELEASED ){
                    if( code == KeyEvent.VK_ALT ){
                        s_alt = false;
                    }else if( code == KeyEvent.VK_CONTROL ){
                        s_ctrl = false;
                    }else if( code == KeyEvent.VK_SHIFT ){
                        s_shift = false;
                    }else if( code == KeyEvent.VK_META ){
                        s_meta = false;
                    }
                }
            }
        }

        public static int getCurrentModifierKey() {
            int ret = 0;
            if( s_ctrl ){
                ret += InputEvent.CTRL_MASK;
            }
            if( s_alt ){
                ret += InputEvent.ALT_MASK;
            }
            if( s_shift ){
                ret += InputEvent.SHIFT_MASK;
            }
            if( s_meta ){
                ret += InputEvent.META_MASK;
            }
            return ret;
        }
#else
        public static int getCurrentModifierKey() {
            int ret = 0;
            System.Windows.Forms.Keys k = System.Windows.Forms.Control.ModifierKeys;
            if ( (k & System.Windows.Forms.Keys.Control) == System.Windows.Forms.Keys.Control ) {
                ret += InputEvent.CTRL_MASK;
            }
            if ( (k & System.Windows.Forms.Keys.Alt) == System.Windows.Forms.Keys.Alt ) {
                ret += InputEvent.ALT_MASK;
            }
            if ( (k & System.Windows.Forms.Keys.Shift) == System.Windows.Forms.Keys.Shift ) {
                ret += InputEvent.SHIFT_MASK;
            }
            return ret;
        }
#endif

#if JAVA
        public static Rectangle getScreenBounds( Component w ){
            return w.getGraphicsConfiguration().getBounds();
        }
#else
        public static Rectangle getScreenBounds( System.Windows.Forms.Control w ) {
            System.Drawing.Rectangle rc = System.Windows.Forms.Screen.GetWorkingArea( w );
            return new Rectangle( rc.X, rc.Y, rc.Width, rc.Height );
        }
#endif

        #region Clipboard
        public static void setClipboardText( String value ) {
#if JAVA
            Clipboard clip = Toolkit.getDefaultToolkit().getSystemClipboard();
		    clip.setContents( new StringSelection( value ), null );
#else
            System.Windows.Forms.Clipboard.SetText( value );
#endif
        }

        public static String getClipboardText() {
#if JAVA
            Clipboard clip = Toolkit.getDefaultToolkit().getSystemClipboard();
            Transferable data = clip.getContents( null );

            String str = null;
            if( data == null ){
                str = null;
            }else if( !data.isDataFlavorSupported( DataFlavor.stringFlavor ) ){
                str = null;
            }else{
                try {
                    str = (String)data.getTransferData( DataFlavor.stringFlavor );
                }catch( Exception e ){
                    str = null;
                }
            }
            return str;
#else
            return System.Windows.Forms.Clipboard.GetText();
#endif
        }
        #endregion

        #region BitConverter

        public static byte[] getbytes_int64_le( long data ) {
#if JAVA
            byte[] dat = new byte[8];
            dat[0] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[1] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[2] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[3] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[4] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[5] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[6] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[7] = (byte)(data & (byte)0xff);
            return dat;
#else
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
#endif
        }

        public static byte[] getbytes_uint32_le( long data ) {
#if JAVA
            byte[] dat = new byte[4];
            data = 0xffffffff & data;
            dat[0] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[1] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[2] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[3] = (byte)(data & (byte)0xff);
            return dat;
#else
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
#endif
        }

        public static byte[] getbytes_int32_le( int data ) {
            long v = data;
            if ( v < 0 ) {
                v += 4294967296L;
            }
            return getbytes_uint32_le( v );
        }

        public static byte[] getbytes_int32_be( int data ) {
            long v = data;
            if ( v < 0 ) {
                v += 4294967296L;
            }
            return getbytes_uint32_be( v );
        }

        public static byte[] getbytes_int64_be( long data ) {
#if JAVA
            byte[] dat = new byte[8];
            dat[7] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[6] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[5] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[4] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[3] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[2] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[1] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[0] = (byte)(data & (byte)0xff);
            return dat;
#else
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
#endif
        }

        public static byte[] getbytes_uint32_be( long data ) {
#if JAVA
            byte[] dat = new byte[4];
            data = 0xffffffff & data;
            dat[3] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[2] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[1] = (byte)(data & (byte)0xff);
            data = (data >>> 8);
            dat[0] = (byte)(data & (byte)0xff);
            return dat;
#else
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
#endif
        }

        public static byte[] getbytes_int16_le( short data ) {
            int i = data;
            if ( i < 0 ) {
                i += 65536;
            }
            return getbytes_uint16_le( i );
        }

        /// <summary>
        /// compatible to BitConverter
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static byte[] getbytes_uint16_le( int data ) {
            byte[] dat = new byte[2];
#if JAVA
            dat[0] = (byte)(data & (byte)0xff);
            data = (byte)(data >>> 8);
            dat[1] = (byte)(data & (byte)0xff);
#else
            dat[0] = (byte)(data & (byte)0xff);
            data = (byte)(data >> 8);
            dat[1] = (byte)(data & (byte)0xff);
#endif
            return dat;
        }

        public static byte[] getbytes_uint16_be( int data ) {
            byte[] dat = new byte[2];
#if JAVA
            dat[1] = (byte)(data & (byte)0xff);
            data = (byte)(data >>> 8);
            dat[0] = (byte)(data & (byte)0xff);
#else
            dat[1] = (byte)(data & (byte)0xff);
            data = (byte)(data >> 8);
            dat[0] = (byte)(data & (byte)0xff);
#endif
            return dat;
        }

        public static long make_int64_le( byte[] buf ) {
            return (long)((long)((long)((long)((long)((long)((long)((long)((long)((((0xff & buf[7]) << 8) | (0xff & buf[6])) << 8) | (0xff & buf[5])) << 8) | (0xff & buf[4])) << 8) | (0xff & buf[3])) << 8 | (0xff & buf[2])) << 8) | (0xff & buf[1])) << 8 | (0xff & buf[0]);
        }

        public static long make_int64_be( byte[] buf ) {
            return (long)((long)((long)((long)((long)((long)((long)((long)((long)((((0xff & buf[0]) << 8) | (0xff & buf[1])) << 8) | (0xff & buf[2])) << 8) | (0xff & buf[3])) << 8) | (0xff & buf[4])) << 8 | (0xff & buf[5])) << 8) | (0xff & buf[6])) << 8 | (0xff & buf[7]);
        }

        public static long make_uint32_le( byte[] buf, int index ) {
            return (long)((long)((long)((long)(((0xff & buf[index + 3]) << 8) | (0xff & buf[index + 2])) << 8) | (0xff & buf[index + 1])) << 8) | (0xff & buf[index]);
        }

        public static long make_uint32_le( byte[] buf ) {
            return make_uint32_le( buf, 0 );
        }

        public static long make_uint32_be( byte[] buf, int index ) {
            return (long)((long)((long)((long)(((0xff & buf[index]) << 8) | (0xff & buf[index + 1])) << 8) | (0xff & buf[index + 2])) << 8) | (0xff & buf[index + 3]);
        }

        public static long make_uint32_be( byte[] buf ) {
            return make_uint32_be( buf, 0 );
        }

        public static int make_int32_le( byte[] buf ) {
            long v = make_uint32_le( buf );
            if ( v >= 2147483647L ) {
                v -= 4294967296L;
            }
            return (int)v;
        }

        /// <summary>
        /// compatible to BitConverter
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static int make_uint16_le( byte[] buf, int index ) {
            return (int)((int)((0xff & buf[index + 1]) << 8) | (0xff & buf[index]));
        }

        public static int make_uint16_le( byte[] buf ) {
            return make_uint16_le( buf, 0 );
        }

        public static int make_uint16_be( byte[] buf, int index ) {
            return (int)((int)((0xff & buf[index]) << 8) | (0xff & buf[index + 1]));
        }

        public static int make_uint16_be( byte[] buf ) {
            return make_uint16_be( buf, 0 );
        }

        /// <summary>
        /// compatible to BitConverter
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static short make_int16_le( byte[] buf, int index ) {
            int i = make_uint16_le( buf, index );
            if ( i >= 32768 ) {
                i = i - 65536;
            }
            return (short)i;
        }

        public static short make_int16_le( byte[] buf ) {
            return make_int16_le( buf, 0 );
        }

        public static double make_double_le( byte[] buf ) {
#if JAVA
            long n = 0L;
            for ( int i = 7; i >= 0; i-- ) {
                n = (n << 8) | (buf[i] & 0xffL);
            }
            return Double.longBitsToDouble( n );
#else
            if ( !BitConverter.IsLittleEndian ) {
                for ( int i = 0; i < 4; i++ ) {
                    byte d = buf[i];
                    buf[i] = buf[7 - i];
                    buf[7 - i] = d;
                }
            }
            return BitConverter.ToDouble( buf, 0 );
#endif
        }

        public static double make_double_be( byte[] buf ) {
#if JAVA
            long n = 0L;
            for ( int i = 0; i <= 7; i++ ) {
                n = (n << 8) | (buf[i] & 0xffL);
            }
            return Double.longBitsToDouble( n );
#else
            if ( BitConverter.IsLittleEndian ) {
                for ( int i = 0; i < 4; i++ ) {
                    byte d = buf[i];
                    buf[i] = buf[7 - i];
                    buf[7 - i] = d;
                }
            }
            return BitConverter.ToDouble( buf, 0 );
#endif
        }

        public static float make_float_le( byte[] buf ) {
#if JAVA
            int n = 0;
            for ( int i = 3; i >= 0; i-- ) {
                n = (n << 8) | (buf[i] & 0xff);
            }
            return Float.intBitsToFloat( n );
#else
            if ( !BitConverter.IsLittleEndian ) {
                for ( int i = 0; i < 2; i++ ) {
                    byte d = buf[i];
                    buf[i] = buf[3 - i];
                    buf[3 - i] = d;
                }
            }
            return BitConverter.ToSingle( buf, 0 );
#endif
        }

        public static float make_float_be( byte[] buf ) {
#if JAVA
            int n = 0;
            for ( int i = 0; i <= 3; i++ ) {
                n = (n << 8) | (buf[i] & 0xff);
            }
            return Float.intBitsToFloat( n );
#else
            if ( BitConverter.IsLittleEndian ) {
                for ( int i = 0; i < 2; i++ ) {
                    byte d = buf[i];
                    buf[i] = buf[3 - i];
                    buf[3 - i] = d;
                }
            }
            return BitConverter.ToSingle( buf, 0 );
#endif
        }

        public static byte[] getbytes_double_le( double value ) {
#if JAVA
            long n = Double.doubleToLongBits( value );
            return getbytes_int64_le( n );
#else
            byte[] ret = BitConverter.GetBytes( value );
            if ( !BitConverter.IsLittleEndian ) {
                for ( int i = 0; i < 4; i++ ) {
                    byte d = ret[i];
                    ret[i] = ret[7 - i];
                    ret[7 - i] = d;
                }
            }
            return ret;
#endif
        }

        public static byte[] getbytes_double_be( double value ) {
#if JAVA
            long n = Double.doubleToLongBits( value );
            return getbytes_int64_be( n );
#else
            byte[] ret = BitConverter.GetBytes( value );
            if ( BitConverter.IsLittleEndian ) {
                for ( int i = 0; i < 4; i++ ) {
                    byte d = ret[i];
                    ret[i] = ret[7 - i];
                    ret[7 - i] = d;
                }
            }
            return ret;
#endif
        }

        public static byte[] getbytes_float_le( float value ) {
#if JAVA
            int n = Float.floatToIntBits( value );
            return getbytes_int32_le( n );
#else
            byte[] ret = BitConverter.GetBytes( value );
            if ( !BitConverter.IsLittleEndian ) {
                for ( int i = 0; i < 2; i++ ) {
                    byte d = ret[i];
                    ret[i] = ret[3 - i];
                    ret[3 - i] = d;
                }
            }
            return ret;
#endif
        }

        public static byte[] getbytes_float_be( float value ) {
#if JAVA
            int n = Float.floatToIntBits( value );
            return getbytes_int32_be( n );
#else
            byte[] ret = BitConverter.GetBytes( value );
            if ( BitConverter.IsLittleEndian ) {
                for ( int i = 0; i < 2; i++ ) {
                    byte d = ret[i];
                    ret[i] = ret[3 - i];
                    ret[3 - i] = d;
                }
            }
            return ret;
#endif
        }
        #endregion

        #region Graphics extension
        public static void drawBezier( Graphics2D g, float x1, float y1,
                           float ctrlx1, float ctrly1,
                           float ctrlx2, float ctrly2,
                           float x2, float y2 ) {
#if JAVA
            g.draw( new CubicCurve2D.Float( x1, y1, ctrlx1, ctrly1, ctrlx2, ctrly2, x2, y2 ) );
#else
            Stroke stroke = g.getStroke();
            System.Drawing.Pen pen = null;
            if ( stroke is BasicStroke ) {
                pen = ((BasicStroke)stroke).pen;
            } else {
                pen = new System.Drawing.Pen( System.Drawing.Color.Black );
            }
            g.nativeGraphics.DrawBezier( pen, new System.Drawing.PointF( x1, y1 ),
                                              new System.Drawing.PointF( ctrlx1, ctrly1 ),
                                              new System.Drawing.PointF( ctrlx2, ctrly2 ),
                                              new System.Drawing.PointF( x2, y2 ) );
#endif
        }

        public const int STRING_ALIGN_FAR = 1;
        public const int STRING_ALIGN_NEAR = -1;
        public const int STRING_ALIGN_CENTER = 0;
#if JAVA
#else
        private static System.Drawing.StringFormat mStringFormat = new System.Drawing.StringFormat();
#endif
        public static void drawStringEx( Graphics g1, String s, Font font, Rectangle rect, int align, int valign ) {
#if JAVA
            Graphics2D g = (Graphics2D)g1;
            g.setFont( font );
            FontMetrics fm = g.getFontMetrics();
            Dimension ret = new Dimension( fm.stringWidth( s ), fm.getHeight() );
            float x = 0;
            float y = 0;
            if( align > 0 ){
                x = rect.x + rect.width - ret.width;
            }else if( align < 0 ){
                x = rect.x;
            }else{
                x = rect.x + rect.width / 2.0f - ret.width / 2.0f;
            }
            if( valign > 0 ){
                y = rect.y + rect.height;// - ret.height;
            }else if( valign < 0 ){
                y = rect.y + ret.height;
            }else{
                y = rect.y + rect.height / 2.0f + ret.height / 2.0f;
            }
            g.drawString( s, x, y );
#else
            if ( align > 0 ) {
                mStringFormat.Alignment = System.Drawing.StringAlignment.Far;
            } else if ( align < 0 ) {
                mStringFormat.Alignment = System.Drawing.StringAlignment.Near;
            } else {
                mStringFormat.Alignment = System.Drawing.StringAlignment.Center;
            }
            if ( valign > 0 ) {
                mStringFormat.LineAlignment = System.Drawing.StringAlignment.Far;
            } else if ( valign < 0 ) {
                mStringFormat.LineAlignment = System.Drawing.StringAlignment.Near;
            } else {
                mStringFormat.LineAlignment = System.Drawing.StringAlignment.Center;
            }
            g1.nativeGraphics.DrawString( s, font.font, g1.brush, new System.Drawing.RectangleF( rect.x, rect.y, rect.width, rect.height ), mStringFormat );
#endif
        }
        #endregion

        #region System.IO
        public static double getFileLastModified( String path ) {
#if JAVA
            File f = new File( path );
            if( f.exists() ){
                return f.lastModified();
            }else{
                return 0.0;
            }
#else
            if ( File.Exists( path ) ) {
                return new FileInfo( path ).LastWriteTimeUtc.Ticks * 100.0 / 1e9;
            }
            return 0;
#endif
        }

        public static long getFileLength( String fpath ) {
#if JAVA
            File f = new File( fpath );
            return f.length();
#else
            return new FileInfo( fpath ).Length;
#endif
        }

        public static String getExtension( String fpath ) {
#if JAVA
            String name = (new File( fpath )).getName();
            int index = name.lastIndexOf( '.' );
            if( index <= 0 ){
                return "";
            }else{
                return name.substring( index );
            }
#else
            return Path.GetExtension( fpath );
#endif
        }

        public static String getFileName( String path ) {
#if JAVA
            File f = new File( path );
            return f.getName();
#else
            return Path.GetFileName( path );
#endif
        }

        public static String getDirectoryName( String path ) {
#if JAVA
            File f = new File( path );
            return f.getParent();
#else
            if ( path == null ) {
                return "";
            }
            if ( path.Length == 0 ) {
                return "";
            }
            return System.IO.Path.GetDirectoryName( path );
#endif
        }

        public static String getFileNameWithoutExtension( String path ) {
#if JAVA
            String file = getFileName( path );
            int index = file.lastIndexOf( "." );
            if( index > 0 ){
                file = file.substring( 0, index );
            }
            return file;
#else
            return System.IO.Path.GetFileNameWithoutExtension( path );
#endif
        }

        public static String createTempFile() {
#if JAVA
            String ret = "";
            try{
                ret = File.createTempFile( "tmp", "" ).getAbsolutePath();
            }catch( Exception ex ){
                System.out.println( "PortUtil#createTempFile; ex=" + ex );
            }
            return ret;
#else
            return System.IO.Path.GetTempFileName();
#endif
        }

        public static String[] listDirectories( String directory ) {
#if JAVA
            if( directory == null ){
                return new String[]{};
            }
            File f = new File( directory );
            File[] list = f.listFiles();
            if( list == null ){
                return new String[]{};
            }
            Vector<String> ret = new Vector<String>();
            for( int i = 0; i < list.length; i++ ){
                File t = list[i];
                if( t.isDirectory() ){
                    ret.add( t.getName() );
                }
            }
            return ret.toArray( new String[]{} );
#else
            return System.IO.Directory.GetDirectories( directory );
#endif
        }

        public static String[] listFiles( String directory, String extension ) {
#if JAVA
            File f = new File( directory );
            File[] list = f.listFiles();
            if( list == null ){
                return new String[]{};
            }
            Vector<String> ret = new Vector<String>();
            for( int i = 0; i < list.length; i++ ){
                File t = list[i];
                if( !t.isDirectory() ){
                    String name = t.getName();
                    if( name.endsWith( extension ) ){
                        ret.add( name );
                    }
                }
            }
            return ret.toArray( new String[]{} );
#else
            return System.IO.Directory.GetFiles( directory, "*" + extension );
#endif
        }

        public static void deleteFile( String path ) {
#if JAVA
            new File( path ).delete();
#else
            System.IO.File.Delete( path );
#endif
        }

        public static void moveFile( String pathBefore, String pathAfter )
#if JAVA
            throws java.io.FileNotFoundException, java.io.IOException
#endif
        {
#if JAVA
            copyFile( pathBefore, pathAfter );
            deleteFile( pathBefore );
#else
            System.IO.File.Move( pathBefore, pathAfter );
#endif
        }

#if !JAVA
        [Obsolete]
        public static bool isDirectoryExists( String path )
        {
            return System.IO.Directory.Exists( path );
        }
#endif

#if !JAVA
        [Obsolete]
        public static boolean isFileExists( String path )
        {
            return System.IO.File.Exists( path );
        }
#endif

        /*public static String combinePath( String path1, String path2 ) {
#if JAVA
            if( path1 != null && path1.endsWith( File.separator ) ){
                path1 = path1.substring( 0, path1.length() - 1 );
            }
            if( path2 != null && path2.startsWith( File.separator ) ){
                path2 = path2.substring( 1 );
            }
            return path1 + File.separator + path2;
#else
            return System.IO.Path.Combine( path1, path2 );
#endif
        }*/

        public static String getTempPath() {
#if JAVA
            String ret = System.getProperty( "java.io.tmpdir" );
            if( ret == null ){
                return "";
            }else{
                return ret;
            }
#else
            return Path.GetTempPath();
#endif
        }

        public static void createDirectory( String path ) {
#if JAVA
            File f = new File( path );
            f.mkdir();
#else
            Directory.CreateDirectory( path );
#endif
        }

        public static void deleteDirectory( String path, boolean recurse ) {
#if JAVA
            File f = new File( path );
            File[] list = f.listFiles();
            for( int i = 0; i < list.length; i++ ){
                File f0 = new File( fsys.combine( path, list[i].getName() ) );
                if( f0.isDirectory() ){
                    if( recurse ){
                        deleteDirectory( f0.getPath(), true );
                    }
                }else{
                    f0.delete();
                }
            }
#else
            Directory.Delete( path, recurse );
#endif
        }

        public static void deleteDirectory( String path ) {
#if JAVA
            (new File( path )).delete();
#else
            Directory.Delete( path );
#endif
        }

        public static void copyFile( String file1, String file2 )
#if JAVA
            throws FileNotFoundException, IOException
#endif
 {
#if JAVA
            FileChannel sourceChannel = new FileInputStream( new File( file1 ) ).getChannel();
            FileChannel destinationChannel = new FileOutputStream( new File( file2 ) ).getChannel();
            sourceChannel.transferTo( 0, sourceChannel.size(), destinationChannel );
            sourceChannel.close();
            destinationChannel.close();
#else
            File.Copy( file1, file2 );
#endif
        }
        #endregion

        #region Number Formatting
        public static boolean tryParseInt( String s, ByRef<Integer> value ) {
            try {
                value.value = int.Parse( s );
            } catch ( Exception ex ) {
                return false;
            }
            return true;
        }

        public static boolean tryParseFloat( String s, ByRef<Float> value ) {
            try {
                value.value = (float)double.Parse( s );
            } catch ( Exception ex ) {
                return false;
            }
            return true;
        }

        /*public static float parseFloat( String value ) {
#if JAVA
            return Float.parseFloat( value );
#else
            return float.Parse( value );
#endif
        }*/

        public static String formatDecimal( String format, double value ) {
#if JAVA
            DecimalFormat df = new DecimalFormat( format );
            return df.format( value );
#else
            return value.ToString( format );
#endif
        }

        public static String formatDecimal( String format, long value ) {
#if JAVA
            DecimalFormat df = new DecimalFormat( format );
            return df.format( value );
#else
            return value.ToString( format );
#endif
        }

        public static String toHexString( long value, int digits ) {
            String ret = toHexString( value );
            int add = digits - getStringLength( ret );
            for ( int i = 0; i < add; i++ ) {
                ret = "0" + ret;
            }
            return ret;
        }

        public static String toHexString( long value ) {
#if JAVA
            return Long.toHexString( value );
#else
            return Convert.ToString( value, 16 );
#endif
        }

        public static long fromHexString( String s ) {
#if JAVA
            return Long.parseLong( s, 16 );
#else
            return Convert.ToInt64( s, 16 );
#endif
        }
        #endregion

        #region String Utility
        /// <summary>
        /// 文字列の指定した位置の文字を取得します
        /// </summary>
        /// <param name="s">文字列</param>
        /// <param name="index">位置．先頭が0</param>
        /// <returns></returns>
        public static char charAt( String s, int index ) {
#if JAVA
            return s.charAt( index );
#else
            return s[index];
#endif
        }

#if JAVA
        public static String[] splitString( String s, char... separator ) {
#else
        public static String[] splitString( String s, params char[] separator ) {
#endif
            return splitStringCorB( s, separator, int.MaxValue, false );
        }

        public static String[] splitString( String s, char[] separator, int count ) {
            return splitStringCorB( s, separator, count, false );
        }

        public static String[] splitString( String s, char[] separator, boolean ignore_empty_entries ) {
            return splitStringCorB( s, separator, int.MaxValue, ignore_empty_entries );
        }

        public static String[] splitString( String s, String[] separator, boolean ignore_empty_entries ) {
            return splitStringCorA( s, separator, int.MaxValue, ignore_empty_entries );
        }

        public static String[] splitString( String s, char[] separator, int count, boolean ignore_empty_entries ) {
            return splitStringCorB( s, separator, count, ignore_empty_entries );
        }

        public static String[] splitString( String s, String[] separator, int count, boolean ignore_empty_entries ) {
            return splitStringCorA( s, separator, count, ignore_empty_entries );
        }

        private static String[] splitStringCorB( String s, char[] separator, int count, boolean ignore_empty_entries ) {
#if JAVA
            int length = separator.length;
#else
            int length = separator.Length;
#endif
            String[] spl = new String[length];
            for ( int i = 0; i < length; i++ ) {
                spl[i] = separator[i] + "";
            }
            return splitStringCorA( s, spl, count, false );
        }

        private static String[] splitStringCorA( String s, String[] separator, int count, boolean ignore_empty_entries ) {
#if JAVA
            if( separator.length == 0 ){
                return new String[]{ s };
            }
            Vector<String> ret = new Vector<String>();
            String remain = s;
            int len = separator[0].length();
            int index = remain.indexOf( separator[0] );
            int i = 1;
            while( index < 0 && i < separator.length ){
                index = remain.indexOf( separator[i] );
                len = separator[i].length();
                i++;
            }
            int added_count = 0;
            while( index >= 0 ){
                if( !ignore_empty_entries || (ignore_empty_entries && index > 0) ){
                    if( added_count + 1 == count ){
                        break;
                    }else{
                        ret.add( remain.substring( 0, index ) );
                    }
                    added_count++;
                }
                remain = remain.substring( index + len );
                index = remain.indexOf( separator[0] );
                i = 1;
                while( index < 0 && i < separator.length ){
                    index = remain.indexOf( separator[i] );
                    len = separator[i].length();
                    i++;
                }
            }
            if( !ignore_empty_entries || (ignore_empty_entries && remain.length() > 0) ){
                ret.add( remain );
            }
            return ret.toArray( new String[]{} );
#else
            return s.Split( separator, count, (ignore_empty_entries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None) );
#endif
        }

        public static int getStringLength( String s ) {
            if ( s == null ) {
                return 0;
            } else {
#if JAVA
                return s.length();
#else
                return s.Length;
#endif
            }
        }

        public static int getEncodedByteCount( String encoding, String str ) {
            byte[] buf = getEncodedByte( encoding, str );
#if JAVA
            return buf.length;
#else
            return buf.Length;
#endif
        }

        public static byte[] getEncodedByte( String encoding, String str ) {
#if JAVA
            Charset enc = Charset.forName( encoding );
            ByteBuffer bb = enc.encode( str );
            byte[] dat = new byte[bb.limit()];
            bb.get( dat );
            return dat;
#else
            Encoding enc = Encoding.GetEncoding( encoding );
            return enc.GetBytes( str );
#endif
        }

        public static String getDecodedString( String encoding, int[] data, int offset, int length ) {
#if JAVA
            Charset enc = Charset.forName( encoding );
            ByteBuffer bb = ByteBuffer.allocate( length );
            for( int i = offset; i < offset + length; i++ ){
                bb.put( (Integer.valueOf( data[i] )).byteValue() );
            }
            bb.rewind();
            return enc.decode( bb ).toString();
#else
            Encoding enc = Encoding.GetEncoding( encoding );
            byte[] d = new byte[data.Length];
            for ( int i = 0; i < data.Length; i++ ) {
                d[i] = (byte)data[i];
            }
            return enc.GetString( d, offset, length );
#endif
        }

        public static String getDecodedString( String encoding, int[] data ) {
#if JAVA
            return getDecodedString( encoding, data, 0, data.length );
#else
            return getDecodedString( encoding, data, 0, data.Length );
#endif
        }

        #endregion

        public static void setMousePosition( Point p )
        {
#if JAVA
            try{
                Robot r = new Robot();
                r.mouseMove( p.x, p.y );
            }catch( AWTException e ){
                e.printStackTrace();
            }
#else
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point( p.x, p.y );
#endif
        }

        public static Point getMousePosition() {
#if JAVA
            return MouseInfo.getPointerInfo().getLocation();
#else
            System.Drawing.Point p = System.Windows.Forms.Control.MousePosition;
            return new Point( p.X, p.Y );
#endif
        }

        /// <summary>
        /// 指定した点が，コンピュータの画面のいずれかに含まれているかどうかを調べます
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static boolean isPointInScreens( Point p ) {
#if JAVA
            GraphicsEnvironment ge = GraphicsEnvironment.getLocalGraphicsEnvironment();
            GraphicsDevice[] gs = ge.getScreenDevices();
            for (int j = 0; j < gs.length; j++) {
                GraphicsDevice gd = gs[j];
                Rectangle rc = gd.getDefaultConfiguration().getBounds();
                if( rc.x <= p.x && p.x <= rc.x + rc.width ){
                    if( rc.y <= p.y && p.y <= rc.y + rc.height ){
                        return true;
                    }
                }
            }
            return false;
#else
            foreach ( System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens ) {
                System.Drawing.Rectangle rc = screen.WorkingArea;
                if ( rc.X <= p.x && p.x <= rc.X + rc.Width ) {
                    if ( rc.Y <= p.y && p.y <= rc.Y + rc.Height ) {
                        return true;
                    }
                }
            }
            return false;
#endif
        }

#if JAVA
        public static Rectangle getWorkingArea( Window w ){
            return w.getGraphicsConfiguration().getBounds();
#else
        public static Rectangle getWorkingArea( System.Windows.Forms.Form w ) {
            System.Drawing.Rectangle r = System.Windows.Forms.Screen.GetWorkingArea( w );
            return new Rectangle( r.X, r.Y, r.Width, r.Height );
#endif
        }

        public static String getMD5FromString( String str ) {
#if JAVA
            MessageDigest digest = null;
            try {
                digest = MessageDigest.getInstance("MD5");
                byte[] buff = getEncodedByte( "UTF-8", str );
                digest.update( buff, 0, buff.length );
            } catch( NoSuchAlgorithmException ex2 ){
                System.err.println( "PortUtil#getMD5FromString; ex2=" + ex2 );
            }
            byte[] dat = digest.digest();
            String ret = "";
            for( int i = 0; i < dat.length; i++ ){
                ret += String.format( "%02x", dat[i] );
            }
            return ret;
#else
            return Misc.getmd5( str );
#endif
        }

        public static String getMD5( String file )
#if JAVA
            throws FileNotFoundException, IOException
#endif
        {
#if JAVA
            InputStream in = new FileInputStream( file );
            MessageDigest digest = null;
            try {
                digest = MessageDigest.getInstance("MD5");
                byte[] buff = new byte[4096];
                int len = 0;
                while ((len = in.read(buff, 0, buff.length)) >= 0) {
                    digest.update(buff, 0, len);
                }
            } catch (IOException e) {
                throw e;
            } catch( NoSuchAlgorithmException ex2 ){
                System.out.println( "PortUtil#getMD5; ex2=" + ex2 );
            } finally {
                if (in != null) {
                    try {
                        in.close();
                    } catch (IOException e) {
                    }
                }
            }
            byte[] dat = digest.digest();
            String ret = "";
            for( int i = 0; i < dat.length; i++ ){
                ret += String.format( "%02x", dat[i] );
            }
            return ret;
#else
            String ret = "";
            using ( FileStream fs = new FileStream( file, FileMode.Open, FileAccess.Read ) ) {
                ret = Misc.getmd5( fs );
            }
            return ret;
#endif
        }


#if JAVA
        class FileFilterImp implements FileFilter{
            private String m_extension;

            public FileFilterImp( String extension ){
                m_extension = extension;
            }

            public boolean accept( File f ){
                String file = f.getName();
                if( file.endsWith( m_extension ) ){
                    return true;
                }else{
                    return false;
                }
            }
        }
#endif

        #region Array conversion
        public static Integer[] convertIntArray( int[] arr ) {
#if JAVA
            Integer[] ret = new Integer[arr.length];
            for( int i = 0; i < arr.length; i++ ){
                ret[i] = arr[i];
            }
            return ret;
#else
            return arr;
#endif
        }

        public static Long[] convertLongArray( long[] arr ) {
#if JAVA
            Long[] ret = new Long[arr.length];
            for( int i = 0; i < arr.length; i++ ){
                ret[i] = arr[i];
            }
            return ret;
#else
            return arr;
#endif
        }

        public static Byte[] convertByteArray( byte[] arr ) {
#if JAVA
            Byte[] ret = new Byte[arr.length];
            for( int i = 0; i < arr.length; i++ ){
                ret[i] = arr[i];
            }
            return ret;
#else
            return arr;
#endif
        }

        public static Float[] convertFloatArray( float[] arr ) {
#if JAVA
            Float[] ret = new Float[arr.length];
            for( int i = 0; i < arr.length; i++ ){
                ret[i] = arr[i];
            }
            return ret;
#else
            return arr;
#endif
        }

        public static Character[] convertCharArray( char[] arr ) {
#if JAVA
            Character[] ret = new Character[arr.length];
            for( int i = 0; i < arr.length; i++ ){
                ret[i] = arr[i];
            }
            return ret;
#else
            return arr;
#endif
        }

#if JAVA
        public static int[] convertIntArray( Integer[] arr ){
            int[] ret = new int[arr.length];
            for( int i = 0; i < arr.length; i++ ){
                ret[i] = arr[i];
            }
            return ret;
        }

        public static long[] convertLongArray( Long[] arr ){
            long[] ret = new long[arr.length];
            for( int i = 0; i < arr.length; i++ ){
                ret[i] = arr[i];
            }
            return ret;
        }

        public static byte[] convertByteArray( Byte[] arr ){
            byte[] ret = new byte[arr.length];
            for( int i = 0; i < arr.length; i++ ){
                ret[i] = arr[i];
            }
            return ret;
        }

        public static float[] convertFloatArray( Float[] arr ){
            float[] ret = new float[arr.length];
            for( int i = 0; i < arr.length; i++ ){
                ret[i] = arr[i];
            }
            return ret;
        }

        public static char[] convertFloatArray( Character[] arr ){
            char[] ret = new char[arr.length];
            for( int i = 0; i < arr.length; i++ ){
                ret[i] = arr[i];
            }
            return ret;
        }
#endif
        #endregion

        public static String getApplicationStartupPath() {
#if JAVA
            return System.getProperty( "user.dir" );
#else
            return System.Windows.Forms.Application.StartupPath;
#endif
        }
    }

#if !JAVA
}
#endif
