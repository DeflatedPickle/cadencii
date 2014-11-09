using System;

namespace Cadencii.Gui.Toolkit
{
	[Flags]
	public enum DragDropEffects {
		None	= 0x00000000,
		Copy	= 0x00000001,
		Move	= 0x00000002,
		Link	= 0x00000004,
		Scroll	= unchecked((int)0x80000000),
		All	= unchecked((int)0x80000003)
	}
}
