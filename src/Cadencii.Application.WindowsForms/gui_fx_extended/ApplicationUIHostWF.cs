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
using System.Collections.ObjectModel;
using Cadencii.Application.Controls;

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
					pi.SetValue (null, ((System.Drawing.Image) obj).ToAwt ());
				else
					throw new NotImplementedException (pi.PropertyType.FullName);
			}
		}

		#region XML Control Tree support

		object FindControlTreeItem (Dictionary<string,object> roots, string name)
		{
			object dummy;

			return roots.TryGetValue (name, out dummy) ? dummy : roots.Where (p => p.Value is Control)
				.Select (p => (Control) p.Value)
				.Select (c => FindControlTreeItem (c, name))
				.FirstOrDefault (c => c != null);
		}

		object FindControlTreeItem (Control c, string name)
		{
			if (c.Name == name)
				return c;
			return c.Controls.OfType<Control> ()
				.Select (cc => FindControlTreeItem (cc, name))
				.FirstOrDefault (x => x != null);
		}

		Func<Type,string, object> get_static_property = (type,value) => type.GetProperty (value.Contains ('.') ? value.Substring (value.IndexOf ('.') + 1) : value.Substring (1)).GetValue (null);
		Func<Type,string, object> get_static_field = (type,value) => type.GetField (value.Contains ('.') ? value.Substring (value.IndexOf ('.') + 1) : value.Substring (1)).GetValue (null);

		object Deserialize (Dictionary<string,object> roots, string value, Type t)
		{
			if (value.FirstOrDefault () == '$') {
				if (t == typeof (System.Drawing.Color)) {
					if (value [1] == '(')
						return typeof (System.Drawing.Color).GetMethods ().First (m => m.Name == "FromArgb" && m.GetParameters ().Length == 3).Invoke (null, value.Substring (2, value.Length - 3).Split (',').Select (tk => int.Parse (tk)).Cast<object> ().ToArray ());
					else if (value.StartsWith ("$SystemColors"))
						return get_static_property (typeof (System.Drawing.SystemColors), value);
					else
						return get_static_property (typeof (System.Drawing.Color), value);
				}
				else if (t == typeof (Cadencii.Gui.Color))
					return get_static_field (typeof (Cadencii.Gui.Colors), value);
				else if (t == typeof (System.Windows.Forms.Cursor))
					return get_static_property (typeof (System.Windows.Forms.Cursors), value);
				else if (t == typeof (Cadencii.Gui.Cursor))
					return get_static_property (typeof (Cadencii.Gui.Cursors), value);
				else
					throw new NotSupportedException ();
			}
			if (t.IsEnum)
				return Enum.ToObject (t, value.Split ('\'')
					.Select (s => s.Trim ())
					.Select (s => Enum.Parse (t, s))
					.Select (e => Convert.ChangeType (e, typeof(int)))
					.Cast<int> ()
					.Sum ());
			else if (Type.GetTypeCode (t) != TypeCode.Object)
				return Convert.ChangeType (value, t);
			else if (value.FirstOrDefault () == '#') {
				var ctrl = FindControlTreeItem (roots, value.Substring (1));
				if (ctrl == null)
					throw new Exception ("Control not found: " + value);
				return ctrl;
			} else {
				var argStrings = value.TrimStart ('(').TrimEnd (')').Split (',').Select (x => x.Trim ()).ToArray ();
				var ctor = t.GetConstructors ().First (c => c.GetParameters ().Length == argStrings.Length);
				var argObjs = argStrings.Zip (ctor.GetParameters (), (s, pi) => Deserialize (roots, s, pi.ParameterType)).ToArray ();
				return Activator.CreateInstance (t, argObjs);
			}
		}

		object CreateObject (string name, params object [] additionalConstructorArguments)
		{
			var ct = typeof (System.Windows.Forms.Form).Assembly.GetType ("System.Windows.Forms." + name);
			var obj = ct != null ? Activator.CreateInstance (ct, additionalConstructorArguments) : ApplicationUIHost.Create (name, additionalConstructorArguments);
			return obj;
		}

		void AddToCollection (object c, object obj)
		{
			c.GetType ().GetMethods ().First (m => m.Name == "Add" && m.GetParameters ().First ().ParameterType.IsAssignableFrom (obj.GetType ())).Invoke (c, new object [] { obj });
		}

		void ApplyXml (Dictionary<string,object> roots, XmlElement e, object o, bool asCollection)
		{
			var bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			var root = roots.Count > 1 ? roots.Values.First (c => c is Form) : roots.Values.First ();
			foreach (XmlElement c in e.SelectNodes ("*")) {
				if (c.LocalName.First () == '_') {
					// get property, can be collection ("MenuStrip.Items") or non-collection ("SplitContainer.Panel1")
					var pv = o.GetType ().GetProperty (c.LocalName.Substring (1), bf).GetValue (o);
					ApplyXml (roots, c, pv, pv is System.Collections.ICollection || typeof (ICollection<RebarBand>).IsAssignableFrom (pv.GetType ()));
				} else {
					object obj;
					if (c.LocalName.Contains ('.')) {
						// special cases
						switch (c.LocalName) {
						case "ListViewGroup.WithConstructorArguments":
							obj = Deserialize (roots, c.InnerText, typeof (ListViewGroup));
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

					// add to tree first, then apply xml, so that some reference to controls (#blah) can be resolved as expected.
					if (asCollection)
						AddToCollection (o, obj);
					else
						((Control) o).Controls.Add ((Control) obj);

					if (!(obj is string))
						ApplyXml (roots, c, obj, false);

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
				}
				else if (a.LocalName == "use-default-constructor")
					continue;
				else {
					var t = o.GetType ();
					var p = t.GetProperty (a.LocalName, bf | BindingFlags.DeclaredOnly) ?? t.GetProperty (a.LocalName, bf);
					if (p == null)
						throw new ArgumentException ("Property '" + a.LocalName + "' was not found");
					var v = Deserialize (roots, a.Value, p.PropertyType);
					p.SetValue (o, v);
				}
			}
		}

		public override void ApplyXml (Cadencii.Gui.Toolkit.UiControl control, string xmlResourceName)
		{
			var xml = new XmlDocument ();
			xml.Load (typeof (FormMainModel).Assembly.GetManifestResourceStream (xmlResourceName));
			var roots = new Dictionary<string,object> ();
			roots.Add (string.Empty, (Control) control.Native);
			if (xml.DocumentElement.LocalName == "Widgets") {
				foreach (XmlElement el in xml.DocumentElement.SelectNodes ("*")) {
					if (el.LocalName == "Form")
						ApplyXml (roots, el, control, false);
					else if (el.LocalName == "____UNKNOWN____")
						continue; // skip
					else {
						var obj = el.GetAttribute ("use-default-constructor") == "true" ? CreateObject (el.LocalName) : CreateObject (el.LocalName, ((UiForm) control).Components);
						ApplyXml (roots, el, obj, false);
						roots.Add (el.GetAttribute ("id"), obj);
					}
				}
			}
			else
				ApplyXml (roots, xml.DocumentElement, control, false);
		}

		#endregion
	}
}

