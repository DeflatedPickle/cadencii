using System;
using System.Linq;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application
{
	public abstract class ApplicationUIHost
	{
		public static object Create (string name, params object [] args)
		{
			var type = new Type [] { typeof (ApplicationUIHost), typeof (UiControl) }
				.SelectMany (t => t.Assembly.GetTypes ())
				.FirstOrDefault (t => t.Name == name);
			if (type == null)
				throw new Exception (string.Format ("Specified type {0} not found.", name));
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

		public static ApplicationUIHost Instance { get; private set; }

		static ApplicationUIHost ()
		{
			Instance = Create<ApplicationUIHost> ();
			Instance.InitializeResources ();
		}

		public abstract void InitializeResources ();

		public abstract void ApplyXml (Cadencii.Gui.Toolkit.UiControl control, string xmlResourceName);

		public abstract Dialogs Dialogs { get; }

		public abstract Clipboard Clipboard { get; }
	}
}

