#if !JAVA
/*
 * NoteNumberProperty.cs
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
using System;
using System.ComponentModel;

namespace org.kbinani.cadencii {

    using boolean = Boolean;

    [TypeConverter( typeof( NoteNumberPropertyConverter ) )]
    public class NoteNumberProperty {
        public int noteNumber = 60;

        public override int GetHashCode() {
            return noteNumber.GetHashCode();
        }

        public override boolean Equals( Object obj ) {
            if ( obj is NoteNumberProperty ) {
                if ( noteNumber == ((NoteNumberProperty)obj).noteNumber ) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return base.Equals( obj );
            }
        }
    }

}
#endif
