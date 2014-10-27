/*
 * awt.cs
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
using cadencii.java.awt;
using cadencii.java.awt.geom;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace cadencii.java.awt
{
	public abstract partial class AwtHost
	{
		public const int YES_OPTION = 0;
		public const int NO_OPTION = 1;
		public const int CANCEL_OPTION = 2;
		public const int OK_OPTION = 0;
		public const int CLOSED_OPTION = -1;

		static AwtHost ()
		{
			var type = AppDomain.CurrentDomain.GetAssemblies ().SelectMany (a => a.GetTypes ()).First (t => t.IsSubclassOf (typeof(AwtHost)));
			Current = (AwtHost)Activator.CreateInstance (type);
		}

		public abstract string getComponentName (Object obj);

		public static AwtHost Current { get; set; }

		protected Dictionary<Type,Type> Types = new Dictionary<Type,Type> ();

		public T New<T> (params object[] args)
		{
			Type t;
			if (!Types.TryGetValue (typeof(T), out t)) {
				t = GetType ().Assembly.GetTypes ().FirstOrDefault (x => x.IsSubclassOf (typeof(T)));
				if (t != null)
					Types [typeof(T)] = t;
				else
					throw new InvalidOperationException (string.Format ("Cannot find {0} from registered types", typeof(T)));
			}
			return (T)Activator.CreateInstance (t, args);
		}
	}
}
