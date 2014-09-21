using System;
using WClipboard = System.Windows.Forms.Clipboard;

namespace cadencii
{
	class ClipboardWF : Clipboard
	{
		public override void SetText (string value)
		{
			WClipboard.SetText (value);
		}

		public override string GetText ()
		{
			return WClipboard.GetText ();
		}

		public override void SetDataObject (ClipboardEntry data, bool copy)
		{
			WClipboard.SetDataObject (data, copy);
		}

		public override ClipboardEntry GetDataObject ()
		{
			var dobj = WClipboard.GetDataObject ();
			if (dobj != null)
				return (ClipboardEntry) dobj.GetData (typeof(ClipboardEntry));
			return null;
		}
	}
}

