using System;

namespace Cadencii.Gui.Toolkit
{
	public static class Extensions
	{
		public static void Mnemonic (this IHasText control, Keys value)
		{
			control.Text = GetMnemonicString (control.Text, value);
		}

		// FIXME: this is winforms-specific. Should be abstracted.
		private static string GetMnemonicString (string text, Keys keys)
		{
			int value = (int)keys;
			if (value == 0) {
				return text;
			}
			if ((value < 48 || 57 < value) && (value < 65 || 90 < value)) {
				return text;
			}

			if (text.Length >= 2) {
				char lastc = text [0];
				int index = -1; // 第index文字目が、ニーモニック
				for (int i = 1; i < text.Length; i++) {
					char c = text [i];
					if (lastc == '&' && c != '&') {
						index = i;
					}
					lastc = c;
				}

				if (index >= 0) {
					string newtext = text.Substring (0, index) + new string ((char)value, 1) + ((index + 1 < text.Length) ? text.Substring (index + 1) : "");
					return newtext;
				}
			}
			return text + "(&" + new string ((char)value, 1) + ")";
		}

		public static void SetColumnHeaders(this UiListView list_view, string[] headers)
		{
			if (list_view.Columns.Count < headers.Length) {
				for (int i = list_view.Columns.Count; i < headers.Length; i++) {
					list_view.Columns.Add (AwtHost.Current.New<UiListViewColumn> ());
				}
			}
			for (int i = 0; i < headers.Length; i++) {
				list_view.Columns [i].Text = headers[i];
			}
		}
	}
}

