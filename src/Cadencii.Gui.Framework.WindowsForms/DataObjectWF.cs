using System;

namespace Cadencii.Gui
{
	public class DataObjectWF : IDataObject
	{
		System.Windows.Forms.IDataObject obj;

		public DataObjectWF (System.Windows.Forms.IDataObject obj)
		{
			this.obj = obj;
		}

		#region IDataObject implementation

		public object GetData (string format)
		{
			return obj.GetData (format);
		}

		public object GetData (string format, bool autoConvert)
		{
			return obj.GetData (format, autoConvert);
		}

		public object GetData (Type format)
		{
			return obj.GetData (format);
		}

		public bool GetDataPresent (string format)
		{
			return obj.GetDataPresent (format);
		}

		public bool GetDataPresent (string format, bool autoConvert)
		{
			return obj.GetDataPresent (format, autoConvert);
		}

		public bool GetDataPresent (Type format)
		{
			return obj.GetDataPresent (format);
		}

		public string[] GetFormats ()
		{
			return obj.GetFormats ();
		}

		public string[] GetFormats (bool autoConvert)
		{
			return obj.GetFormats (autoConvert);
		}

		public void SetData (object data)
		{
			obj.SetData (data);
		}

		public void SetData (string format, bool autoConvert, object data)
		{
			obj.SetData (format, autoConvert, data);
		}

		public void SetData (string format, object data)
		{
			obj.SetData (format, data);
		}

		public void SetData (Type format, object data)
		{
			obj.SetData (format, data);
		}

		#endregion
	}
}
