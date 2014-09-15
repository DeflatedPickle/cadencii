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
using System.Windows.Forms;
using cadencii.java.awt;
using DialogResult = cadencii.java.awt.DialogResult;

namespace cadencii
{
	public class DialogManager
	{
		/// <summary>
		/// ダイアログを表示中かどうか
		/// </summary>
		private static bool mShowingDialog = false;
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
		public static event EventHandler MainWindowFocusRequired;

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
		public static DialogResult showModalDialog (Form dialog, Form parent_form)
		{
			beginShowDialog ();
			var ret = dialog.ShowDialog (parent_form);
			endShowDialog ();
			return (DialogResult)ret;
		}

		/// <summary>
		/// ダイアログを，メインウィンドウに対してモーダルに表示し，ダイアログの結果を取得します
		/// </summary>
		/// <param name="dialog"></param>
		/// <param name="main_form"></param>
		/// <returns></returns>
		public static int showModalDialog (UiBase dialog, System.Windows.Forms.Form parent_form)
		{
			beginShowDialog ();
			int ret = dialog.showDialog (parent_form);
			endShowDialog ();
			return ret;
		}

		/// <summary>
		/// ダイアログを，メインウィンドウに対してモーダルに表示し，ダイアログの結果を取得します
		/// </summary>
		/// <param name="dialog"></param>
		/// <param name="main_form"></param>
		/// <returns></returns>
		public static DialogResult showModalDialog (FolderBrowserDialog dialog, Form main_form)
		{
			beginShowDialog ();
			var ret = (DialogResult)dialog.ShowDialog (main_form);
			endShowDialog ();
			return ret;
		}

		/// <summary>
		/// ダイアログを，メインウィンドウに対してモーダルに表示し，ダイアログの結果を取得します
		/// </summary>
		/// <param name="dialog"></param>
		/// <param name="open_mode"></param>
		/// <param name="main_form"></param>
		/// <returns></returns>
		public static DialogResult showModalDialog (FileDialog dialog, bool open_mode, Form main_form)
		{
			beginShowDialog ();
			DialogResult ret = (DialogResult)dialog.ShowDialog (main_form);
			endShowDialog ();
			return ret;
		}

		/// <summary>
		/// beginShowDialogが呼ばれた後，endShowDialogがまだ呼ばれていないときにtrue
		/// </summary>
		/// <returns></returns>
		public static bool isShowingDialog ()
		{
			return mShowingDialog;
		}

		/// <summary>
		/// モーダルなダイアログを出すために，プロパティウィンドウとミキサーウィンドウの「最前面に表示」設定を一時的にOFFにします
		/// </summary>
		private static void beginShowDialog ()
		{
			mShowingDialog = true;
#if ENABLE_PROPERTY
			if (propertyWindow != null) {
				bool previous = propertyWindow.getUi ().isAlwaysOnTop ();
				propertyWindow.setPreviousAlwaysOnTop (previous);
				if (previous) {
					propertyWindow.getUi ().setAlwaysOnTop (false);
				}
			}
#endif
			if (mMixerWindow != null) {
				bool previous = mMixerWindow.TopMost;
				mMixerWindow.setPreviousAlwaysOnTop (previous);
				if (previous) {
					mMixerWindow.TopMost = false;
				}
			}

			if (iconPalette != null) {
				bool previous = iconPalette.TopMost;
				iconPalette.setPreviousAlwaysOnTop (previous);
				if (previous) {
					iconPalette.TopMost = false;
				}
			}
		}

		/// <summary>
		/// beginShowDialogで一時的にOFFにした「最前面に表示」設定を元に戻します
		/// </summary>
		private static void endShowDialog ()
		{
#if ENABLE_PROPERTY
			if (propertyWindow != null) {
				propertyWindow.getUi ().setAlwaysOnTop (propertyWindow.getPreviousAlwaysOnTop ());
			}
#endif
			if (mMixerWindow != null) {
				mMixerWindow.TopMost = mMixerWindow.getPreviousAlwaysOnTop ();
			}

			if (iconPalette != null) {
				iconPalette.TopMost = iconPalette.getPreviousAlwaysOnTop ();
			}

			try {
				if (MainWindowFocusRequired != null) {
					MainWindowFocusRequired.Invoke (typeof(AppManager), new EventArgs ());
				}
			} catch (Exception ex) {
				Logger.write (typeof(AppManager) + ".endShowDialog; ex=" + ex + "\n");
				sout.println (typeof(AppManager) + ".endShowDialog; ex=" + ex);
			}
			mShowingDialog = false;
		}

		public static DialogResult showMessageBox (string text, string caption, int optionType, int messageType)
		{
			beginShowDialog ();
			var ret = (DialogResult)cadencii.windows.forms.Utility.showMessageBox (text, caption, optionType, messageType);
			endShowDialog ();
			return ret;
		}

		#endregion

		public static bool showDialogTo (FormWorker fw, FormMain main_window)
		{
			
			beginShowDialog ();
			bool ret = fw.getUi ().showDialogTo (main_window);
#if DEBUG
			sout.println ("AppManager#patchWorkToFreeze; showDialog returns " + ret);
#endif
			endShowDialog ();

			return ret;
		}
	}
}
