using System;
using cadencii.java.awt;

namespace cadencii
{
	public interface BitmapEx : IDisposable
	{
		Color GetPixel (int x, int y);
	}
}

