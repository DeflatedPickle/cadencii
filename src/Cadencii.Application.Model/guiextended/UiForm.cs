using System;

namespace cadencii
{
	public interface UiForm : UiBase
	{
		bool TopMost { get; set; }

		object Invoke (Delegate d);

		void Close ();
	}
}

