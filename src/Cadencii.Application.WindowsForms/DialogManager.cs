/*
 * AppManager.cs
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
using cadencii.java.awt;
using DialogResult = cadencii.java.awt.DialogResult;

namespace cadencii
{
	public class DialogManager
	{
		/// <summary>
		/// ダイアログを表示中かどうか
		/// </summary>
//		private static bool mShowingDialog = false;
		#if ENABLE_PROPERTY
		/// <summary>
		/// プロパティウィンドウが分離した場合のプロパティウィンドウのインスタンス。
		/// メインウィンドウとプロパティウィンドウが分離している時、propertyPanelがpropertyWindowの子になる
		/// </summary>
		public static FormNotePropertyController propertyWindow;
		#endif
		/// <summary>
		/// ミキサーダイアログ
		/// </summary>
		public static FormMixer mMixerWindow;
		/// <summary>
		/// アイコンパレット・ウィンドウのインスタンス
		/// </summary>
		public static FormIconPalette iconPalette = null;

		/// <summary>
		/// メインウィンドウにフォーカスを当てる要求があった時発生するイベント
		/// </summary>
		public static EventHandler MainWindowFocusRequired;

		#region MessageBoxのラッパー

		public static DialogResult showMessageBox (string text)
		{
			return showMessageBox (text, "", cadencii.Dialog.MSGBOX_DEFAULT_OPTION, cadencii.Dialog.MSGBOX_PLAIN_MESSAGE);
		}

		public static DialogResult showMessageBox (string text, string caption)
		{
			return showMessageBox (text, caption, cadencii.Dialog.MSGBOX_DEFAULT_OPTION, cadencii.Dialog.MSGBOX_PLAIN_MESSAGE);
		}

		public static DialogResult showMessageBox (string text, string caption, int optionType)
		{
			return showMessageBox (text, caption, optionType, cadencii.Dialog.MSGBOX_PLAIN_MESSAGE);
		}

		/// <summary>
		/// ダイアログを，メインウィンドウに対してモーダルに表示し，ダイアログの結果を取得します
		/// </summary>
		/// <param name="dialog"></param>
		/// <param name="main_form"></param>
		/// <returns></returns>
		public static DialogResult showModalDialog (object dialogForm, object parentForm)
		{
			return ApplicationUIHost.Instance.Dialogs.ShowModalDialog (dialogForm, parentForm);
		}

		/// <summary>
		/// ダイアログを，メインウィンドウに対してモーダルに表示し，ダイアログの結果を取得します
		/// </summary>
		/// <param name="dialog"></param>
		/// <param name="main_form"></param>
		/// <returns></returns>
		public static int showModalDialog (UiBase dialog, object parent_form)
		{
			return ApplicationUIHost.Instance.Dialogs.ShowModalDialog (dialog, parent_form);
		}

		/// <summary>
		/// ダイアログを，メインウィンドウに対してモーダルに表示し，ダイアログの結果を取得します
		/// </summary>
		/// <param name="dialog"></param>
		/// <param name="main_form"></param>
		/// <returns></returns>
		public static DialogResult showModalFolderDialog (object folderBrowserDialog, object mainForm)
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
		public static DialogResult showModalFileDialog (object fileDialog, bool open_mode, object mainForm)
		{
			return ApplicationUIHost.Instance.Dialogs.ShowModalFileDialog (fileDialog, open_mode, mainForm);
		}

		/// <summary>
		/// beginShowDialogが呼ばれた後，endShowDialogがまだ呼ばれていないときにtrue
		/// </summary>
		/// <returns></returns>
		public static bool isShowingDialog ()
		{
			return ApplicationUIHost.Instance.Dialogs.IsShowingDialog;
		}

		public static DialogResult showMessageBox (string text, string caption, int optionType, int messageType)
		{
			ApplicationUIHost.Instance.Dialogs.BeforeShowDialog ();
			var ret = (DialogResult)cadencii.windows.forms.Utility.showMessageBox (text, caption, optionType, messageType);
			ApplicationUIHost.Instance.Dialogs.AfterShowDialog ();
			return ret;
		}

		#endregion

		public static bool showDialogTo (FormWorker fw, object mainFormWindow)
		{
			return ApplicationUIHost.Instance.Dialogs.ShowDialogTo (fw, mainFormWindow);
		}
	}
}
