#if !JAVA
/*
 * AttackVariationConverter.cs
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
using System.Windows.Forms;
using org.kbinani.vsq;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.java.io;

namespace org.kbinani.cadencii {

    using boolean = System.Boolean;

    public class AttackVariationConverter : TypeConverter {
        public override bool GetStandardValuesSupported( ITypeDescriptorContext context ) {
            return true;
        }

        public override StandardValuesCollection GetStandardValues( ITypeDescriptorContext context ) {
            SynthesizerType type = SynthesizerType.VOCALOID2;
            VsqFileEx vsq = AppManager.getVsqFile();
            if ( vsq != null ) {
                RendererKind kind = VsqFileEx.getTrackRendererKind( vsq.Track.get( AppManager.getSelected() ) );
                if ( kind == RendererKind.VOCALOID1_100 || kind == RendererKind.VOCALOID1_101 ){
                    type = SynthesizerType.VOCALOID1;
                }
            }
            Vector<AttackVariation> list = new Vector<AttackVariation>();
            list.add( new AttackVariation() );
            for ( Iterator<NoteHeadHandle> itr = VocaloSysUtil.attackConfigIterator( type ); itr.hasNext(); ) {
                NoteHeadHandle aconfig = itr.next();
                list.add( new AttackVariation( aconfig.getDisplayString() ) );
            }
            return new StandardValuesCollection( list.toArray( new AttackVariation[] { } ) );
        }

        public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType ) {
            if ( destinationType == typeof( String ) ) {
                return true;
            } else {
                return base.CanConvertTo( context, destinationType );
            }
        }

        public override Object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, Type destinationType ) {
            if ( destinationType == typeof( String ) && value is AttackVariation ) {
                return ((AttackVariation)value).mDescription;
            } else {
                return base.ConvertTo( context, culture, value, destinationType );
            }
        }

        public override Object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value ) {
            if ( value is String ) {
                if ( value.Equals( new AttackVariation().mDescription ) ) {
                    return new AttackVariation();
                } else {
                    SynthesizerType type = SynthesizerType.VOCALOID2;
                    VsqFileEx vsq = AppManager.getVsqFile();
                    if ( vsq != null ) {
                        RendererKind kind = VsqFileEx.getTrackRendererKind( vsq.Track.get( AppManager.getSelected() ) );
                        if ( kind == RendererKind.VOCALOID1_100 || kind == RendererKind.VOCALOID1_101 ) {
                            type = SynthesizerType.VOCALOID1;
                        }
                        String svalue = (String)value;
                        for ( Iterator<NoteHeadHandle> itr = VocaloSysUtil.attackConfigIterator( type ); itr.hasNext(); ) {
                            NoteHeadHandle aconfig = itr.next();
                            String display_string = aconfig.getDisplayString();
                            if ( svalue.Equals( display_string ) ) {
                                return new AttackVariation( display_string );
                            }
                        }
                    }
                    return new AttackVariation();
                }
            } else {
                return base.ConvertFrom( context, culture, value );
            }
        }

        public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType ) {
            if ( sourceType == typeof( String ) ) {
                return true;
            } else {
                return base.CanConvertFrom( context, sourceType );
            }
        }
    }

}
#endif
