using System;
using System.Linq;

namespace Cadencii.Application
{
	public abstract class ApplicationUIHost
	{
		public static object Create (string name, params object [] args)
		{
			var type = typeof(ApplicationUIHost).Assembly.GetTypes ().First (t => t.Name == name);
			return Create (type, args);
		}

		public static object Create (Type type, params object [] args)
		{
			var implType = AppDomain.CurrentDomain.GetAssemblies ().SelectMany (a => a.GetTypes ()).First (t => type.IsInterface ? t.GetInterfaces ().Contains (type) : t.IsSubclassOf (type));
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

