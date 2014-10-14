using System;

namespace cadencii.java.awt {
	public class KeyPressEventArgs : EventArgs 
	{
		public KeyPressEventArgs (char keyChar)
		{
			this.KeyChar = keyChar;
		}

		public bool Handled { get; set; }
		public char KeyChar { get; set; }
	}
}