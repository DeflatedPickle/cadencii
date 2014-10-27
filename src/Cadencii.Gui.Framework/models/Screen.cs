using System;

namespace cadencii.java.awt
{
	public class Screen
	{
		public static Screen Instance = new Screen ();

		ScreenAdapter a;

		public Screen ()
		{
			a = AwtHost.Current.New<ScreenAdapter> ();
		}

		public abstract class ScreenAdapter
		{
			public abstract Rectangle getScreenBounds (object nativeContrl);
			public abstract void setMousePosition (Point p);
			public abstract Point getMousePosition ();
			public abstract bool isPointInScreens (Point p);
			public abstract Rectangle getWorkingArea (object nativeWindow);
		}

		public Rectangle getScreenBounds (object nativeContrl)
		{
			return a.getScreenBounds (nativeContrl);
		}
		public void setMousePosition (Point p)
		{
			a.setMousePosition (p);
		}
		public Point getMousePosition ()
		{
			return a.getMousePosition ();
		}
		public bool isPointInScreens (Point p)
		{
			return a.isPointInScreens (p);
		}
		public Rectangle getWorkingArea (object nativeWindow)
		{
			return a.getWorkingArea (nativeWindow);
		}
	}
}

