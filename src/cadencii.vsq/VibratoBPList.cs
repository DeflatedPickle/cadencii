/*
 * VibratoBPList.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package cadencii.vsq;

import java.util.*;
import java.io.*;
import cadencii.*;
#else
using System;
using System.Collections.Generic;
using cadencii;
using cadencii.java.util;

namespace cadencii.vsq
{
#endif

#if JAVA
    public class VibratoBPList implements Cloneable, Serializable
#else
    [Serializable]
    public class VibratoBPList : ICloneable
#endif
    {
        private List<VibratoBPPair> m_list;

        public VibratoBPList()
        {
            m_list = new List<VibratoBPPair>();
        }

        public VibratoBPList( String strNum, String strBPX, String strBPY )
        {
            int num = 0;
            try {
                num = int.Parse( strNum );
            } catch ( Exception ex ) {
                serr.println( "org.kbinani.vsq.VibratoBPList#.ctor; ex=" + ex );
                num = 0;
            }
            String[] bpx = PortUtil.splitString( strBPX, ',' );
            String[] bpy = PortUtil.splitString( strBPY, ',' );
            int actNum = Math.Min( num, Math.Min( bpx.Length, bpy.Length ) );
            if ( actNum > 0 ) {
                float[] x = new float[actNum];
                int[] y = new int[actNum];
                for ( int i = 0; i < actNum; i++ ) {
                    try {
                        x[i] = (float)double.Parse( bpx[i] );
                        y[i] = int.Parse( bpy[i] );
                    } catch ( Exception ex ) {
                        serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                    }
                }

                int len = Math.Min( x.Length, y.Length );
                m_list = new List<VibratoBPPair>( len );
                for ( int i = 0; i < len; i++ ) {
                    m_list.Add( new VibratoBPPair( x[i], y[i] ) );
                }
                m_list.Sort();
            } else {
                m_list = new List<VibratoBPPair>();
            }
        }

        public VibratoBPList( float[] x, int[] y )
#if JAVA
            throws NullPointerException
#endif
        {
            if ( x == null ) {
#if JAVA
                throw new NullPointerException( "x" );
#else
                throw new ArgumentNullException( "x" );
#endif
            }
            if ( y == null ) {
#if JAVA
                throw new NullPointerException( "y" );
#else
                throw new ArgumentNullException( "y" );
#endif
            }
            int len = Math.Min( x.Length, y.Length );
            m_list = new List<VibratoBPPair>( len );
            for ( int i = 0; i < len; i++ ) {
                m_list.Add( new VibratoBPPair( x[i], y[i] ) );
            }
            m_list.Sort();
        }

        /// <summary>
        /// このインスタンスと，指定したVibratoBPListのインスタンスが等しいかどうかを調べます
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool equals( VibratoBPList item )
        {
            if ( item == null ) {
                return false;
            }
            int size = this.m_list.Count;
            if ( size != item.m_list.Count ) {
                return false;
            }
            for ( int i = 0; i < size; i++ ) {
                VibratoBPPair p0 = this.m_list[ i ];
                VibratoBPPair p1 = item.m_list[ i ];
                if ( p0.X != p1.X ) {
                    return false;
                }
                if ( p0.Y != p1.Y ) {
                    return false;
                }
            }
            return true;
        }

        public int getValue( float x, int default_value )
        {
            if ( m_list.Count <= 0 ) {
                return default_value;
            }
            int index = -1;
            int size = m_list.Count;
            for ( int i = 0; i < size; i++ ) {
                if ( x < m_list[ i ].X ) {
                    break;
                }
                index = i;
            }
            if ( index == -1 ) {
                return default_value;
            } else {
                return m_list[ index ].Y;
            }
        }

        public Object clone()
        {
            VibratoBPList ret = new VibratoBPList();
            for ( int i = 0; i < m_list.Count; i++ ) {
                ret.m_list.Add( new VibratoBPPair( m_list[ i ].X, m_list[ i ].Y ) );
            }
            return ret;
        }

#if !JAVA
        public object Clone()
        {
            return clone();
        }
#endif

        public int getCount()
        {
            return m_list.Count;
        }

        public VibratoBPPair getElement( int index )
        {
            return m_list[ index ];
        }

        public void setElement( int index, VibratoBPPair value )
        {
            m_list[ index] =  value ;
        }

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public String Data
        {
            get
            {
                return getData();
            }
            set
            {
                setData( value );
            }
        }
#endif

        public String getData()
        {
            String ret = "";
            for ( int i = 0; i < m_list.Count; i++ ) {
                ret += (i == 0 ? "" : ",") + m_list[ i ].X + "=" + m_list[ i ].Y;
            }
            return ret;
        }

        public void setData( String value )
        {
            m_list.Clear();
            String[] spl = PortUtil.splitString( value, ',' );
            for ( int i = 0; i < spl.Length; i++ ) {
                String[] spl2 = PortUtil.splitString( spl[i], '=' );
                if ( spl2.Length < 2 ) {
                    continue;
                }
                m_list.Add( new VibratoBPPair( (float)double.Parse( spl2[0] ), int.Parse( spl2[1] ) ) );
            }
        }

        public void clear()
        {
            m_list.Clear();
        }
    }

#if !JAVA
}
#endif
