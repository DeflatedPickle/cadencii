using System;
using System.Dynamic;
using System.Linq;
using System.IO;
using System.Reflection;

namespace cadencii
{
	public static class Properties
	{
		internal static dynamic Resources = new DynamicResource ();
	}

	public class DynamicResource : DynamicObject
	{
		static readonly string[] resource_names = new DirectoryInfo (".").GetFiles ("resources/*").Select (d => d.ToString ()).ToArray ();

		public override bool TryGetMember (GetMemberBinder binder, out object result)
		{
			result = GetResource (binder.Name);
			return result != null;
		}

		public object GetResource (string name)
		{
			var res = resource_names.FirstOrDefault (n => Path.GetFileNameWithoutExtension (n) == name);
			if (res != null) {
				switch (Path.GetExtension (res)) {
				case ".png":
					return Xwt.Drawing.BitmapImage.FromFile (res);
				case ".ico":
					return null;
				case ".txt":
					return File.ReadAllText (res);
				}
			}
			return null;
		}
	}
}

