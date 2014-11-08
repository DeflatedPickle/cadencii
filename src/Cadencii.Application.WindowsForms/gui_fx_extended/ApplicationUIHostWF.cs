using System;
using System.Reflection;
using Cadencii.Gui;

namespace cadencii
{
	public class ApplicationUIHostWF : ApplicationUIHost
	{
		Dialogs dialogs = new DialogsWF ();
		public override Dialogs Dialogs { get { return dialogs; } }
		Clipboard clipboard = new ClipboardWF ();
		public override Clipboard Clipboard { get { return clipboard; } }

		public override void InitializeResources ()
		{
			var resourceType = typeof (cadencii.Properties.Resources);
			var bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
			foreach (var pi in typeof (Resources).GetProperties ()) {
				var obj = resourceType.GetProperty (pi.Name, bf).GetValue (null);
				if (pi.PropertyType.IsEnum)
					pi.SetValue (null, Convert.ChangeType (obj, pi.PropertyType));
				else if (pi.PropertyType == typeof (Image))
					pi.SetValue (null, cadencii.ExtensionsWF.ToAwt ((System.Drawing.Image) obj));
				else
					throw new NotImplementedException (pi.PropertyType.FullName);
			}
		}
	}
}

