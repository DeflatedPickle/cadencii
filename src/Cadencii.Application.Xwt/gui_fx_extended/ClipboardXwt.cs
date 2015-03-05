using System;
using XClipboard = Xwt.Clipboard;
using cadencii;

namespace Cadencii.Application.Forms
{
	class ClipboardXwt : Clipboard
	{
		public override void SetText (string value)
		{
			XClipboard.SetText (value);
		}

		public override string GetText ()
		{
			return XClipboard.GetText ();
		}

		public override void SetDataObject (object data, bool copy)
		{
			XClipboard.SetData (data);
		}

		public override object GetDataObject (Type dataType)
		{
			return Convert.ChangeType (XClipboard.GetData<object> (), dataType);
		}
	}
}

