using System;
using Cadencii.Gui;
using cadencii.apputil;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface VersionInfo : UiForm
	{
		bool isShowTwitterID ();

		void setShowTwitterID (bool value);

		void applyLanguage ();

		Color getVersionColor ();

		void setVersionColor (Color value);

		Color getAppNameColor ();

		void setAppNameColor (Color value);

		void setCredit (Cadencii.Gui.Image value);

		string getAppName ();

		void setAppName (string value);

		void setAuthorList (AuthorListEntry[] value);
	}
}

