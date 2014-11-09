using System;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface InputBox : UiForm
	{
		string Result { get; set; }
	}
}

