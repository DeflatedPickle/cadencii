/*
 * Utility.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of cadencii.windows.forms.
 *
 * cadencii.windows.forms is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.windows.forms is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using cadencii;
using Cadencii.Gui;

namespace cadencii.windows.forms
{
    public static class Utility
    {
        public static System.Windows.Forms.DialogResult showMessageBox(string text, string caption, int optionType, int messageType)
        {
            System.Windows.Forms.DialogResult ret = System.Windows.Forms.DialogResult.Cancel;
            System.Windows.Forms.MessageBoxButtons btn = System.Windows.Forms.MessageBoxButtons.OK;
            if (optionType == Dialog.MSGBOX_YES_NO_CANCEL_OPTION) {
                btn = System.Windows.Forms.MessageBoxButtons.YesNoCancel;
			} else if (optionType == Dialog.MSGBOX_YES_NO_OPTION) {
                btn = System.Windows.Forms.MessageBoxButtons.YesNo;
			} else if (optionType == Dialog.MSGBOX_OK_CANCEL_OPTION) {
                btn = System.Windows.Forms.MessageBoxButtons.OKCancel;
            } else {
                btn = System.Windows.Forms.MessageBoxButtons.OK;
            }

            System.Windows.Forms.MessageBoxIcon icon = System.Windows.Forms.MessageBoxIcon.None;
			if (messageType == Dialog.MSGBOX_ERROR_MESSAGE) {
                icon = System.Windows.Forms.MessageBoxIcon.Error;
			} else if (messageType == Dialog.MSGBOX_INFORMATION_MESSAGE) {
                icon = System.Windows.Forms.MessageBoxIcon.Information;
			} else if (messageType == Dialog.MSGBOX_PLAIN_MESSAGE) {
                icon = System.Windows.Forms.MessageBoxIcon.None;
			} else if (messageType == Dialog.MSGBOX_QUESTION_MESSAGE) {
                icon = System.Windows.Forms.MessageBoxIcon.Question;
			} else if (messageType == Dialog.MSGBOX_WARNING_MESSAGE) {
                icon = System.Windows.Forms.MessageBoxIcon.Warning;
            }

            System.Windows.Forms.DialogResult dr = System.Windows.Forms.MessageBox.Show(text, caption, btn, icon);
            if (dr == System.Windows.Forms.DialogResult.OK) {
                ret = System.Windows.Forms.DialogResult.OK;
            } else if (dr == System.Windows.Forms.DialogResult.Cancel) {
                ret = System.Windows.Forms.DialogResult.Cancel;
            } else if (dr == System.Windows.Forms.DialogResult.Yes) {
                ret = System.Windows.Forms.DialogResult.Yes;
            } else if (dr == System.Windows.Forms.DialogResult.No) {
                ret = System.Windows.Forms.DialogResult.No;
            }
            return ret;
        }

		
        /// <summary>
        /// 文字列itemをfontを用いて描画したとき、幅widthピクセルに収まるようにitemを調節したものを返します。
        /// 例えば"1 Voice"→"1 Voi..."ナド。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="font"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string trimString(string item, Font font, int width)
        {
            string edited = item;
            int delete_count = PortUtil.getStringLength(item);
            bool д = true;
            for (; д; ) {
                Dimension measured = cadencii.apputil.Util.measureString(edited, font);
                if (measured.Width <= width) {
                    return edited;
                }
                delete_count -= 1;
                if (delete_count > 0) {
                    edited = item.Substring(0, delete_count) + "...";
                } else {
                    return edited;
                }
            }
            return item;
        }
    }

}
