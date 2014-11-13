using System;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Forms
{
	public interface InputBox : UiForm
	{
		string Result { get; set; }
	}
}

