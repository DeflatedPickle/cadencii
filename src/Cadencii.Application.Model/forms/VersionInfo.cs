using System;
using cadencii.java.awt;
using cadencii.apputil;

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

		void setCredit (java.awt.Image value);

		string getAppName ();

		void setAppName (string value);

		void setAuthorList (AuthorListEntry[] value);
	}
}

