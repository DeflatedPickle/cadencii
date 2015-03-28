using System;

namespace Cadencii.Gui.Toolkit
{
	public class Screen
	{
		public static Screen Instance = new Screen ();

		ScreenAdapter a;

		public Screen ()
		{
			a = GuiHost.Current.New<ScreenAdapter> ();
		}

		public abstract class ScreenAdapter
		{
			public abstract Rectangle getScreenBounds (object nativeControl);
			public abstract void setMousePosition (Point p);
			public abstract Point getMousePosition ();
			public abstract bool isPointInScreens (Point p);
			public abstract Rectangle getWorkingArea (object nativeWindow);
		}

		public Rectangle getScreenBounds (object nativeControl)
		{
			return a.getScreenBounds (nativeControl);
		}
		public void SetScreenMousePosition (Point p)
		{
			a.setMousePosition (p);
		}
		public Point GetScreenMousePosition ()
		{
			return a.getMousePosition ();
		}
		public bool IsPointInScreens (Point p)
		{
			return a.isPointInScreens (p);
		}
		public Rectangle GetWorkingArea (UiForm nativeWindow)
		{
			return a.getWorkingArea (nativeWindow.Native);
		}
	}
}

