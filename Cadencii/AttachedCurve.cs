﻿/*
 * AttachedCurve.cs
 * Copyright (C) 2008-2010 kbinani
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

import java.util.*;
import org.kbinani.*;
#else
using System;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.java.io;

namespace org.kbinani.cadencii {
#endif

#if JAVA
    public class AttachedCurve implements Cloneable{
#else
    public class AttachedCurve : ICloneable {
#endif
        private Vector<BezierCurves> m_curves = new Vector<BezierCurves>();

#if !JAVA
        /// <summary>
        /// XML保存用
        /// </summary>
        public Vector<BezierCurves> Curves {
            get {
                return getCurves();
            }
            set {
                setCurves( value );
            }
        }
#endif

        public Vector<BezierCurves> getCurves() {
            return m_curves;
        }

        public void setCurves( Vector<BezierCurves> value ) {
            m_curves = value;
        }

        /// <summary>
        /// このクラスの指定した名前のプロパティが総称型引数を用いる型である場合に，
        /// その型の限定名を返します．それ以外の場合は空文字を返します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String getGenericTypeName( String name ) {
            if ( name != null ) {
                if ( name.Equals( "Curves" ) ) {
                    return "org.kbinani.cadencii.BezierCurves";
                }
            }
            return "";
        }

        public BezierCurves get( int index ) {
            return m_curves.get( index );
        }

        public void set( int index, BezierCurves value ) {
            m_curves.set( index, value );
        }

        public void add( BezierCurves item ) {
            m_curves.add( item );
        }

        public void removeElementAt( int index ) {
            m_curves.removeElementAt( index );
        }

        public void insertElementAt( int position, BezierCurves attached_curve ) {
            m_curves.insertElementAt( attached_curve, position );
        }

        public Object clone() {
            AttachedCurve ret = new AttachedCurve();
            ret.m_curves.clear();
            int c = m_curves.size();
            for ( int i = 0; i < c; i++ ) {
                ret.m_curves.add( (BezierCurves)m_curves.get( i ).clone() );
            }
            return ret;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif
    }

#if !JAVA
}
#endif
