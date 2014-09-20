﻿using System;
using System.Linq;

namespace cadencii
{
	public abstract class ApplicationUIHost
	{
		public static T Create<T> ()
		{
			var type = AppDomain.CurrentDomain.GetAssemblies ().SelectMany (a => a.GetTypes ()).First (t => t.IsSubclassOf (typeof(T)));
			return (T) Activator.CreateInstance (type);
		}

		public static ApplicationUIHost Instance { get; private set; }

		static ApplicationUIHost ()
		{
			Instance = Create<ApplicationUIHost> ();
		}

		public abstract Dialogs Dialogs { get; }
	}
}
