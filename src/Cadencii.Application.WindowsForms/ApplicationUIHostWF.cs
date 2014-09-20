using System;

namespace cadencii
{
	public class ApplicationUIHostWF : ApplicationUIHost
	{
		Dialogs dialogs = new DialogsWF ();
		public override Dialogs Dialogs { get { return dialogs; } }
	}
}

