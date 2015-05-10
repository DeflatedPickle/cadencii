using System;
using Xwt;

namespace Cadencii.Gui.Toolkit
{
	// FIXME: everything needs to be implemented, but Xwt has no Font dialog.
	public class FontDialogImpl : UiFontDialog
	{
		public object Native { get; set; }
		public void Dispose ()
		{
		}

		public Font Font { get; set; }
		public bool AllowVectorFonts { get; set; }
		public bool AllowVerticalFonts { get; set; }
		public bool FontMustExist { get; set; }
		public bool ShowEffects { get; set; }
		public DialogResult ShowDialog ()
		{
			return DialogResult.Cancel;
		}
	}
}

