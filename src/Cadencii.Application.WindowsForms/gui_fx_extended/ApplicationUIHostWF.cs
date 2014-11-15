using System;
using System.Reflection;
using Cadencii.Gui;
using System.Xml;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using cadencii;
using Cadencii.Application.Models;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Forms
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

		#region XML Control Tree support

		object FindControlTreeItem (Control c, string name)
		{
			if (c.Name == name)
				return c;
			return c.Controls.OfType<Control> ()
				.Select (cc => FindControlTreeItem (cc, name))
				.FirstOrDefault (x => x != null);
		}

		object Deserialize (Control root, string value, Type t)
		{
			if (value.FirstOrDefault () == '$') {
				if (t == typeof (System.Drawing.Color)) {
					if (value [1] == '(')
						return typeof (System.Drawing.Color).GetMethods ().First (m => m.Name == "FromArgb").Invoke (null, value.Substring (2, value.Length - 3).Split (',').Select (tk => int.Parse (tk)).Cast<object> ().ToArray ());
					else if (value.StartsWith ("$SystemColors"))
						return typeof (System.Drawing.SystemColors).GetProperty (value.Substring (value.IndexOf ('.') + 1)).GetValue (null);
					else
						return typeof (System.Drawing.Color).GetProperty (value.Substring (1)).GetValue (null);
				}
				else
					throw new NotSupportedException ();
			}
			if (t.IsEnum)
				return Enum.ToObject (t, value.Split ('\'')
					.Select (s => s.Trim ())
					.Select (s => Enum.Parse (t, s))
					.Select (e => Convert.ChangeType (e, typeof (int)))
					.Cast<int> ()
					.Sum ());
			else if (Type.GetTypeCode (t) != TypeCode.Object)
				return Convert.ChangeType (value, t);
			else if (value.FirstOrDefault () == '#')
				return FindControlTreeItem (root, value.Substring (1));
			else {
				var argStrings = value.TrimStart ('(').TrimEnd (')').Split (',').Select (x => x.Trim ()).ToArray ();
				var ctor = t.GetConstructors ().First (c => c.GetParameters ().Length == argStrings.Length);
				var argObjs = argStrings.Zip (ctor.GetParameters (), (s, pi) => Deserialize (root, s, pi.ParameterType)).ToArray ();
				return Activator.CreateInstance (t, argObjs);
			}
		}

		object CreateObject (string name)
		{
			var ct = typeof (System.Windows.Forms.Form).Assembly.GetType ("System.Windows.Forms." + name);
			var obj = ct != null ? Activator.CreateInstance (ct) : ApplicationUIHost.Create (name);
			return obj;
		}

		void ApplyXml (Control root, XmlElement e, object o, bool asCollection)
		{
			var bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			foreach (XmlElement c in e.SelectNodes ("*")) {
				if (c.LocalName.First () == '_') {
					// collection property
					var pv = o.GetType ().GetProperty (c.LocalName.Substring (1), bf).GetValue (o);
					ApplyXml (root, c, pv, true);
				} else {
					object obj;
					if (c.LocalName.Contains ('.')) {
						// special cases
						switch (c.LocalName) {
						case "ListViewGroup.WithConstructorArguments":
							obj = Deserialize (root, c.InnerText, typeof (ListViewGroup));
							break;
						default:
							throw new NotImplementedException ();
						}
					} else if (c.LocalName == "String")
						obj = c.InnerText;
					else
						obj = CreateObject (c.LocalName);

					// optionally call ISupportInitialize. It is not strictly the same as designer generated code, but still hopefully works...
					var isi = obj as ISupportInitialize;
					if (isi != null)
						((ISupportInitialize) obj).BeginInit ();
					// optionally call SuspendLayout(). Dunno when winforms generates this, so far AFAIK it is invoked for GroupBox.
					if (obj is GroupBox)
						((Control) obj).SuspendLayout ();

					if (!(obj is string))
						ApplyXml (root, c, obj, false);

					if (asCollection)
						o.GetType ().GetMethods ().First (m => m.Name == "Add" && m.GetParameters ().First ().ParameterType.IsAssignableFrom (obj.GetType ())).Invoke (o, new object [] {obj});
					else
						((Control) o).Controls.Add ((Control) obj);

					if (isi != null)
						((ISupportInitialize) obj).EndInit ();
					if (obj is GroupBox) {
						((Control) obj).ResumeLayout (false);
						((Control) obj).PerformLayout ();
					}
				}
			}
			foreach (XmlAttribute a in e.Attributes) {
				if (a.LocalName == "id") {
					var pi = root.GetType ().GetProperty (a.Value, bf);
					if (pi != null)
						pi.SetValue (root, o);
					else {
						var fi = root.GetType ().GetField (a.Value, bf);
						if (fi != null)
							fi.SetValue (root, o);
					}
				} else {
					var t = o.GetType ();
					var p = t.GetProperty (a.LocalName);
					var v = Deserialize (root, a.Value, p.PropertyType);
					p.SetValue (o, v);
				}
			}
		}

		public override void ApplyXml (Cadencii.Gui.Toolkit.UiControl control, string xmlResourceName)
		{
			var xml = new XmlDocument ();
			xml.Load (typeof (FormMainModel).Assembly.GetManifestResourceStream (xmlResourceName));
			ApplyXml ((Control) control.Native, xml.DocumentElement, control, false);
		}

		#endregion
	}
}

