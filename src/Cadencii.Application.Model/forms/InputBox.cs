using System;

namespace cadencii.windows.forms
{
	public interface InputBox : UiForm
	{
		string getResult();
		void setResult(string value);
	}
}

