﻿/*
 * BooleanEnum.cs
 * Copyright (C) 2009-2010 kbinani
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
namespace org.kbinani.cadencii {
#endif

    /// <summary>
    /// ブール値をOn，Offで表現するための列挙型
    /// </summary>
    public enum BooleanEnum {
        /// <summary>
        /// ブール値falseを表す
        /// </summary>
        Off,
        /// <summary>
        /// ブール値trueを表す
        /// </summary>
        On,
    }

#if !JAVA
}
#endif
