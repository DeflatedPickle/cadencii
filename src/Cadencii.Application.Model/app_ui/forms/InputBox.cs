using System;

namespace cadencii
{
	public interface InputBox : UiForm
	{
		string getResult();
		void setResult(string value);
	}
}

