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

using Control = Xwt.Widget;

namespace Cadencii.Application.Forms
{
	public class ApplicationUIHostXwt : ApplicationUIHost
	{
		Dialogs dialogs = new DialogsXwt ();
		public override Dialogs Dialogs { get { return dialogs; } }
		Clipboard clipboard = new ClipboardXwt ();
		public override Clipboard Clipboard { get { return clipboard; } }

		public override void InitializeResources ()
		{
			var bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
			foreach (var pi in typeof (Resources).GetProperties ()) {
				var obj = typeof (Resources).GetProperty (pi.Name).GetValue (null);
				if (pi.PropertyType.IsEnum)
					pi.SetValue (null, Convert.ChangeType (obj, pi.PropertyType));
				else if (pi.PropertyType == typeof (Image))
					pi.SetValue (null, ((Xwt.Drawing.Image) obj).ToGui ());
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
			var b = c as Xwt.Box;
			if (b != null)
				return b.Children.Select (bc => FindControlTreeItem (bc, name))
					.FirstOrDefault (x => x != null);
			var s = c as UiSplitContainer;
			if (s != null)
				new object [] {s.Panel1, s.Panel2}
				.Concat (s.Panel1.GetControls ())
				.Concat (s.Panel2.GetControls ())
				.OfType<Control> ()
				.Select (cc => FindControlTreeItem (cc, name))
				.FirstOrDefault (x => x != null);
			var t = c as UiToolStripContainer;
			if (t != null)
				new object [] {t.ContentPanel, t.BottomToolStripPanel}
				.Concat (t.ContentPanel.GetControls ())
				.Concat (t.BottomToolStripPanel.GetControls ())
				.OfType<Control> ()
				.Select (cc => FindControlTreeItem (cc, name))
				.FirstOrDefault (x => x != null);
			var p = c as IControlContainer;
			if (p == null)
				return null;
			return p.GetControls ().OfType<Control> ()
				.Select (cc => FindControlTreeItem (cc, name))
				.FirstOrDefault (x => x != null);
		}

		Func<Type,string, object> get_static_property = (type,value) => type.GetProperty (value.Contains ('.') ? value.Substring (value.IndexOf ('.') + 1) : value.Substring (1)).GetValue (null);
		Func<Type,string, object> get_static_field = (type,value) => type.GetField (value.Contains ('.') ? value.Substring (value.IndexOf ('.') + 1) : value.Substring (1)).GetValue (null);

		object Deserialize (Dictionary<string,object> roots, string value, Type t)
		{
			if (value.FirstOrDefault () == '$') {
				if (t == typeof(Xwt.Drawing.Color)) {
					if (value [1] == '(')
						return typeof(Xwt.Drawing.Color).GetMethods ().First (m => m.Name == "FromArgb" && m.GetParameters ().Length == 3).Invoke (null, value.Substring (2, value.Length - 3).Split (',').Select (tk => int.Parse (tk)).Cast<object> ().ToArray ());
					else
						return get_static_property (typeof(Xwt.Drawing.Color), value);
				} else if (t == typeof(Cadencii.Gui.Color)) {
					if (value [1] == '(')
						return Activator.CreateInstance (typeof (Cadencii.Gui.Color), value.Substring (2, value.Length - 3).Split (',').Select (tk => int.Parse (tk)).Cast<object> ().ToArray ());
					if (value.StartsWith ("$SystemColors"))
						return get_static_property (typeof(Cadencii.Gui.SystemColors), value);
					else
						return get_static_field (typeof(Cadencii.Gui.Colors), value);
				} else if (t == typeof (Xwt.CursorType))
					return get_static_property (typeof (Xwt.CursorType), value);
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
				var ctor = t.GetConstructors ().FirstOrDefault (c => c.GetParameters ().Length == argStrings.Length);
				if (ctor == null)
					throw new ArgumentException (string.Format ("Constructor for {0} with {1} parameters was not found", t, argStrings.Length));
				var argObjs = argStrings.Zip (ctor.GetParameters (), (s, pi) => Deserialize (roots, s, pi.ParameterType)).ToArray ();
				return Activator.CreateInstance (t, argObjs);
			}
		}

		object CreateObject (string name, params object [] additionalConstructorArguments)
		{
			var obj = ApplicationUIHost.Create (name, additionalConstructorArguments);
			return obj;
		}

		void AddToCollection (object c, object obj)
		{
			var method = c.GetType ().GetMethods ().FirstOrDefault (m => m.Name == "Add" && m.GetParameters ().Length == 1 && m.GetParameters ().First ().ParameterType.IsAssignableFrom (obj.GetType ()));
			if (method == null)
				throw new Exception (string.Format ("Add method for '{0}' not found in the object of type {1}", obj.GetType (), c.GetType ()));
			method.Invoke (c, new object [] { obj });
		}

		PropertyInfo GetPropertyFrom (Type type, string name)
		{
			var bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
			PropertyInfo ret;
			for (var t = type; t != null; t = t.BaseType) {
				ret = type.GetProperty (name, bf) ?? type.GetProperties (bf).FirstOrDefault (p => p.Name.EndsWith ('.' + name));
				if (ret != null)
					return ret;
			}
			foreach (var t in GetAllInterfaces (type)) {
				ret = t.GetProperty (name, bf);
				if (ret != null)
					return ret;
			}
			return null;
		}

		IEnumerable<Type> GetAllInterfaces (Type type)
		{
			foreach (var t in type.GetInterfaces ()) {
				yield return t;
				foreach (var tt in GetAllInterfaces (t))
					yield return tt;
			}
		}

		void ApplyXml (Dictionary<string,object> roots, XmlElement e, object o, bool asCollection)
		{
			var bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			var root = roots.Count > 1 ? roots.Values.First (c => c is FormImpl) : roots.Values.First ();
			foreach (XmlElement c in e.SelectNodes ("*")) {
				if (c.LocalName.First () == '_') {
					var pi = GetPropertyFrom (o.GetType (), c.LocalName.Substring (1));
					if (pi == null)
						throw new ArgumentException (string.Format ("Property '{0}' was not found in type {1}", c.LocalName.Substring (1), o.GetType ()));
					var pv = pi.GetValue (o);
					if (pv == null) {
						// set property. "_MainMenuStrip"
						foreach (XmlElement d in c.SelectNodes ("*")) {
							var dec = CreateObject (d.LocalName);
							ApplyXml (roots, d, dec, true);
							pi.SetValue (o, dec); // should be only once though.
						}
					} else {
						// get property, can be collection ("MenuStrip.Items") or non-collection ("SplitContainer.Panel1")
						ApplyXml (roots, c, pv, pv is System.Collections.ICollection || pv.GetType ().Name == typeof (CastingList<,>).Name);
					}
				} else {
					object obj;
					if (c.LocalName == "String")
						obj = c.InnerText;
					else
						obj = CreateObject (c.LocalName);

					// optionally call ISupportInitialize. It is not strictly the same as designer generated code, but still hopefully works...
					var isi = obj as ISupportInitialize;
					if (isi != null)
						((ISupportInitialize) obj).BeginInit ();
					// optionally call SuspendLayout(). Dunno when winforms generates this, so far AFAIK it is invoked for GroupBox.
					//if (obj is GroupBox)
					//	((Control) obj).SuspendLayout ();

					// add to tree first, then apply xml, so that some reference to controls (#blah) can be resolved as expected.
					if (asCollection)
						AddToCollection (o, obj);
					else
						((IControlContainer) o).AddControl ((UiControl) obj);

					if (!(obj is string))
						ApplyXml (roots, c, obj, false);

					if (isi != null)
						((ISupportInitialize) obj).EndInit ();
					//if (obj is GroupBox) {
					//	((Control) obj).ResumeLayout (false);
					//	((Control) obj).PerformLayout ();
					//}
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
					var p = GetPropertyFrom (t, a.LocalName);
					if (p == null)
						throw new ArgumentException (string.Format ("Property '{0}' was not found on {1}", a.LocalName, t));
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

