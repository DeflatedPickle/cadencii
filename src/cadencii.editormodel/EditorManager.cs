using System;

namespace cadencii
{
	public static class EditorManager
	{
		static EditorManager ()
		{
			Selected = 1;
			SelectedTool = EditTool.PENCIL;
			editHistory = new EditHistoryModel();
		}

		static int mSelected;

		/// <summary>
		/// 現在選択されているトラックを取得または設定します
		/// </summary>
		public static int Selected {
			get {
				int tracks = MusicManager.getVsqFile ().Track.Count;
				if (tracks <= mSelected) {
					mSelected = tracks - 1;
				}
				return mSelected;
			}
			set {
				mSelected = value;
			}
		}

		/// <summary>
		/// 現在選択されているツールを取得または設定します
		/// </summary>
		/// <returns></returns>
		public static EditTool SelectedTool { get; set; }

		/// <summary>
		/// 編集履歴を管理するmodel
		/// </summary>
		public static EditHistoryModel editHistory = null;
	}
}

