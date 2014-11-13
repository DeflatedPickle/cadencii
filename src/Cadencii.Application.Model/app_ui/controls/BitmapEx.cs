using System;
using Cadencii.Gui;

namespace Cadencii.Application.Controls
{
	public interface BitmapEx : IDisposable
	{
		Color GetPixel (int x, int y);
	}
}

