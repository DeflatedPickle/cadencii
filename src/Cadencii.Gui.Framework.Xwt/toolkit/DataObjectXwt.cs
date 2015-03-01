using System;
using Cadencii.Gui.Toolkit;
using System.Linq;

namespace Cadencii.Gui
{
	public class DataObjectXwt : IDataObject
	{
		Xwt.ITransferData obj;

		public DataObjectXwt (Xwt.ITransferData obj)
		{
			this.obj = obj;
		}

		#region IDataObject implementation

		public object GetData (string format)
		{
			return obj.GetValue (Xwt.TransferDataType.FromId (format));
		}

		// FIXME: autoConvert not supported.
		public object GetData (string format, bool autoConvert)
		{
			return obj.GetValue (Xwt.TransferDataType.FromId (format));
		}

		public object GetData (Type format)
		{
			return obj.GetValue (Xwt.TransferDataType.FromType (format));
		}

		public bool GetDataPresent (string format)
		{
			return obj.HasType (Xwt.TransferDataType.FromId (format));
		}

		// FIXME: autoConvert not supported.
		public bool GetDataPresent (string format, bool autoConvert)
		{
			return obj.HasType (Xwt.TransferDataType.FromId (format));
		}

		public bool GetDataPresent (Type format)
		{
			return obj.HasType (Xwt.TransferDataType.FromType (format));
		}

		public string[] GetFormats ()
		{
			return obj.Uris.Select (u => u.ToString ()).ToArray ();
		}

		// FIXME: autoConvert not supported.
		public string[] GetFormats (bool autoConvert)
		{
			return obj.Uris.Select (u => u.ToString ()).ToArray ();
		}

		public void SetData (object data)
		{
			throw new NotImplementedException ();
		}

		public void SetData (string format, bool autoConvert, object data)
		{
			throw new NotImplementedException ();
		}

		public void SetData (string format, object data)
		{
			throw new NotImplementedException ();
		}

		public void SetData (Type format, object data)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}
