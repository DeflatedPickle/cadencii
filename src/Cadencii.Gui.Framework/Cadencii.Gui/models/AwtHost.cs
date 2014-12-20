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
using Cadencii.Gui;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using cadencii;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Gui
{
	public abstract partial class AwtHost
	{
		public const int YES_OPTION = 0;
		public const int NO_OPTION = 1;
		public const int CANCEL_OPTION = 2;
		public const int OK_OPTION = 0;
		public const int CLOSED_OPTION = -1;

		public static Keys ModifierKeys { get; private set; }

		public static int MouseHoverTime { get; private set; }

		static AwtHost ()
		{
			var type = AppDomain.CurrentDomain.GetAssemblies ().SelectMany (a => a.GetTypes ()).First (t => t.IsSubclassOf (typeof(AwtHost)));
			Current = (AwtHost) Activator.CreateInstance (type);
			ModifierKeys = Current.DefaultModifierKeys ();
			MouseHoverTime = Current.PlatformMouseHoverTime;
			Current.InitializeCursors ();
		}

		public abstract void InitializeSystemColors ();

		public abstract void InitializeCursors ();

		public abstract Keys DefaultModifierKeys ();

		public abstract int PlatformMouseHoverTime { get; }

		public abstract void ApplicationDoEvents ();

		public abstract string GetComponentName (Object obj);

		public abstract void ApplyFontRecurse (UiControl control, Font font);

		public static AwtHost Current { get; set; }

		protected Dictionary<Type,Type> Types = new Dictionary<Type,Type> ();

		public abstract int HorizontalScrollBarThumbWidth { get; }
		public abstract int VerticalScrollBarThumbHeight { get; }
		public abstract int VerticalScrollBarWidth { get; }

		public abstract Font SystemMenuFont { get; }

		[Obsolete ("Use Create<T>")]
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

		public static object Create (string name, params object [] args)
		{
			var type = new Type [] { typeof (AwtHost), typeof (UiControl) }
				.SelectMany (t => t.Assembly.GetTypes ())
				.First (t => t.Name == name);
			return Create (type, args);
		}

		public static object Create (Type type, params object [] args)
		{
			var implType = AppDomain.CurrentDomain.GetAssemblies ().SelectMany (a => a.GetTypes ()).Where (t => t.IsClass).First (t => type.IsInterface ? t.GetInterfaces ().Contains (type) : t.IsSubclassOf (type));
			return Activator.CreateInstance (implType, args, null);
		}

		public static T Create<T> (params object [] args)
		{
			return (T) Create (typeof(T), args);
		}

	}
}
