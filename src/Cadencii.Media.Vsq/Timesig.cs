/*
 * Timesig.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of Cadencii.Media.Vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
namespace Cadencii.Media.Vsq
{

    public struct Timesig
    {
        public int numerator;
        public int denominator;


        public Timesig(int numerator, int denominator)
        {
            this.numerator = numerator;
            this.denominator = denominator;
        }

        public bool Equals(Timesig rhs)
        {
            return numerator == rhs.numerator
                && denominator == rhs.denominator;
        }
    }

}
