using System;
using System.Dynamic;
using System.Linq;
using System.IO;

namespace cadencii
{
	public static class Properties
	{
		internal static dynamic Resources = new DynamicResource ();
	}

	public class DynamicResource : DynamicObject
	{
		static readonly string [] resource_names = 
			typeof (Properties).Assembly.GetManifestResourceNames ();
		
		public override bool TryGetMember (GetMemberBinder binder, out object result)
		{
			if (resource_names.Contains (binder.Name)) {
				switch (Path.GetExtension (binder.Name)) {
				case ".png":
					result = Xwt.Drawing.BitmapImage.FromResource (binder.Name);
					return true;
				case ".ico":
					result = null;
					return false;
				case ".txt":
					result = new StreamReader (GetType ().Assembly.GetManifestResourceStream (binder.Name)).ReadToEnd ();
					return true;
				}
			}
			result = null;
			return false;
		}
	}
}

