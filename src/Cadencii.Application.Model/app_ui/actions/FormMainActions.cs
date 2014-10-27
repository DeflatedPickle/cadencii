using System;
using cadencii.java.awt;

namespace cadencii
{
	public class FormMainActions
	{
		public FormMainActions ()
		{
		}

		public static void TrackSelector_MouseClick (FormMainUi window, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) {
				if (EditorManager.keyWidth < e.X && e.X < window.trackSelector.Width) {
					if (window.trackSelector.Height - TrackSelectorConsts.OFFSET_TRACK_TAB <= e.Y && e.Y <= window.trackSelector.Height) {
						window.cMenuTrackTab.Show (window.trackSelector, e.X, e.Y);
					} else {
						window.cMenuTrackSelector.Show (window.trackSelector, e.X, e.Y);
					}
				}
			}
		}
	}
}

