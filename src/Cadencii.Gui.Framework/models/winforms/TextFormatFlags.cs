using System;

namespace cadencii
{
	[Flags]
	public enum TextFormatFlags
	{
		Left = 0,
		Top = 0,
		Default = 0,
		GlyphOverhangPadding = 0,
		HorizontalCenter = 1,
		Right = 2,
		VerticalCenter = 4,
		Bottom = 8,
		WordBreak = 16,
		SingleLine = 32,
		ExpandTabs = 64,
		NoClipping = 256,
		ExternalLeading = 512,
		NoPrefix = 2048,
		Internal = 4096,
		TextBoxControl = 8192,
		PathEllipsis = 16384,
		EndEllipsis = 32768,
		ModifyString = 65536,
		RightToLeft = 131072,
		WordEllipsis = 262144,
		NoFullWidthCharacterBreak = 524288,
		HidePrefix = 1048576,
		PrefixOnly = 2097152,
		PreserveGraphicsClipping = 16777216,
		PreserveGraphicsTranslateTransform = 33554432,
		NoPadding = 268435456,
		LeftAndRightPadding = 536870912
	}
}
