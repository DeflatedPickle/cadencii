using System;
using Cadencii.Gui;

namespace cadencii
{
	public interface BitmapEx : IDisposable
	{
		Color GetPixel (int x, int y);
	}
}

