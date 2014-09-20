
namespace cadencii.java.awt
{
	public struct Size
	{
		public static Size Empty {
			get { return new Size (0, 0); }
		}

		int w, h;

		public Size (int width, int height)
		{
			w = width;
			h = height;
		}

		public int Width { get { return w; } }
		public int Height { get { return h; } }
	}
}
