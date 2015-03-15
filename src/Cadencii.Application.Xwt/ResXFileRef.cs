using System;
using System.ComponentModel;

namespace Cadencii.Gui
{
	[TypeConverter ("Cadencii.Gui.XwtResXFileRefConverter")]
	class ResXFileRef
	{
		public ResXFileRef ()
		{
		}
	}

	class XwtResXFileRefConverter : TypeConverter
	{
		public override bool CanConvertTo (ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof (Xwt.Drawing.BitmapImage) || base.CanConvertTo (context, destinationType);
		}

		public override object ConvertTo (ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof (Xwt.Drawing.BitmapImage))
				return Xwt.Drawing.BitmapImage.FromFile (value.ToString ());
			return base.ConvertTo (context, culture, value, destinationType);
		}
	}
}

