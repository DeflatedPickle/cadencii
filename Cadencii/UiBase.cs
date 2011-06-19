/*
 * UiBase.cs
 * Copyright © 2011 kbinani
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

import org.kbinani.windows.forms.*;

#elif __cplusplus

namespace org{ namespace kbinani{ namespace cadencii{

#else

using System;

using org.kbinani.windows.forms;

namespace org.kbinani.cadencii
{

#endif

    public interface UiBase
    {
        BDialogResult showDialog( Object parent_form );
    }

#if JAVA

#elif __cplusplus

} } }

#else

}

#endif