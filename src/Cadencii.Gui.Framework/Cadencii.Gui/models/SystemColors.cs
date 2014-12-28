using System;

namespace Cadencii.Gui
{
	public static class SystemColors
	{
		static SystemColors ()
		{
			GuiHost.Current.InitializeSystemColors ();
		}

		public static Color Control { get; set; }
		public static Color ControlDark { get; set; }
		public static Color ControlText { get; set; }
		public static Color ActiveBorder { get; set; }
		public static Color Window { get; set; }
		public static Color WindowText { get; set; }
	}
}

