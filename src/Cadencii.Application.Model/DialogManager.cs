/*
 * EditorManager.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using Cadencii.Gui;
using DialogResult = Cadencii.Gui.DialogResult;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application
{
	public class DialogManager
	{
		#region MessageBoxのラッパー

		public static DialogResult ShowMessageBox (string text)
		{
			return ShowMessageBox (text, "", MessageBoxButtons.OK, MessageBoxIcon.None);
		}

		public static DialogResult ShowMessageBox (string text, string caption)
		{
			return ShowMessageBox (text, caption, MessageBoxButtons.OK, MessageBoxIcon.None);
		}

		public static DialogResult ShowMessageBox (string text, string caption, MessageBoxButtons optionType)
		{
			return ShowMessageBox (text, caption, optionType, MessageBoxIcon.None);
		}

		/// <summary>
		/// ダイアログを，メインウィンドウに対してモーダルに表示し，ダイアログの結果を取得します
		/// </summary>
		/// <param name="dialog"></param>
		/// <param name="main_form"></param>
		/// <returns></returns>
		public static DialogResult ShowModalDialog (UiForm dialog, UiForm parent_form)
		{
			return ApplicationUIHost.Instance.Dialogs.ShowModalDialog (dialog, parent_form);
		}

		/// <summary>
		/// ダイアログを，メインウィンドウに対してモーダルに表示し，ダイアログの結果を取得します
		/// </summary>
		/// <param name="dialog"></param>
		/// <param name="main_form"></param>
		/// <returns></returns>
		public static DialogResult ShowModalFolderDialog (UiFolderBrowserDialog folderBrowserDialog, UiForm mainForm)
		{
			return ApplicationUIHost.Instance.Dialogs.ShowModalFolderDialog (folderBrowserDialog, mainForm);
		}

		/// <summary>
		/// ダイアログを，メインウィンドウに対してモーダルに表示し，ダイアログの結果を取得します
		/// </summary>
		/// <param name="dialog"></param>
		/// <param name="open_mode"></param>
		/// <param name="main_form"></param>
		/// <returns></returns>
		public static DialogResult ShowModalFileDialog (UiFileDialog fileDialog, bool open_mode, UiForm mainForm)
		{
			return ApplicationUIHost.Instance.Dialogs.ShowModalFileDialog (fileDialog, open_mode, mainForm);
		}

		/// <summary>
		/// beginShowDialogが呼ばれた後，endShowDialogがまだ呼ばれていないときにtrue
		/// </summary>
		/// <returns></returns>
		public static bool IsShowingDialog ()
		{
			return ApplicationUIHost.Instance.Dialogs.IsShowingDialog;
		}

		public static DialogResult ShowMessageBox (string text, string caption, MessageBoxButtons optionType, MessageBoxIcon messageType)
		{
			return ApplicationUIHost.Instance.Dialogs.ShowMessageBox (text, caption, optionType, messageType);
		}

		#endregion

		public static bool ShowDialogTo (FormWorker formWorker, UiForm mainFormWindow)
		{
			return ApplicationUIHost.Instance.Dialogs.ShowDialogTo (formWorker, mainFormWindow);
		}
	}
}
