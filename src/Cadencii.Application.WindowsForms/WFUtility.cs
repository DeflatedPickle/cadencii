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
using cadencii.java.awt;

namespace cadencii.windows.forms
{
    public static class Utility
    {
        public static string SelectedFilter(this System.Windows.Forms.FileDialog dialog)
        {
            string[] filters = dialog.Filter.Split('|');
            int index = dialog.FilterIndex;
            if (0 <= index && index < filters.Length) {
                return filters[index];
            } else {
                return string.Empty;
            }
        }

        public static System.Windows.Forms.FileDialog SetSelectedFile(this System.Windows.Forms.FileDialog dialog, string file_path)
        {
            string file_name = string.Empty;
            string initial_directory = System.IO.Directory.Exists(file_path) ? file_path : (System.IO.File.Exists(file_path) ? System.IO.Path.GetDirectoryName(file_path) : file_path);

            dialog.FileName = file_name;
            dialog.InitialDirectory = initial_directory;
            return dialog;
        }

        public static void AddRow(this System.Windows.Forms.ListView list_view, string[] items, bool selected = false)
        {
            var item = new System.Windows.Forms.ListViewItem(items);
            item.Checked = selected;
            if (list_view.Columns.Count < items.Length) {
                for (int i = list_view.Columns.Count; i < items.Length; i++) {
                    list_view.Columns.Add("");
                }
            }
            list_view.Items.Add(item);
        }

        public static void SetColumnHeaders(this System.Windows.Forms.ListView list_view, string[] headers)
        {
            if (list_view.Columns.Count < headers.Length) {
                for (int i = list_view.Columns.Count; i < headers.Length; i++) {
                    list_view.Columns.Add("");
                }
            }
            for (int i = 0; i < headers.Length; i++) {
                list_view.Columns[i].Text = headers[i];
            }
        }

        public static System.Windows.Forms.Control Mnemonic(this System.Windows.Forms.Control control, Keys value)
        {
            control.Text = GetMnemonicString(control.Text, value);
            return control;
        }

        public static System.Windows.Forms.ToolStripItem Mnemonic(this System.Windows.Forms.ToolStripItem item, Keys value)
        {
            item.Text = GetMnemonicString(item.Text, value);
            return item;
        }

        private static string GetMnemonicString(string text, Keys keys)
        {
            int value = (int)keys;
            if (value == 0) {
                return text;
            }
            if ((value < 48 || 57 < value) && (value < 65 || 90 < value)) {
                return text;
            }

            if (text.Length >= 2) {
                char lastc = text[0];
                int index = -1; // 第index文字目が、ニーモニック
                for (int i = 1; i < text.Length; i++) {
                    char c = text[i];
                    if (lastc == '&' && c != '&') {
                        index = i;
                    }
                    lastc = c;
                }

                if (index >= 0) {
                    string newtext = text.Substring(0, index) + new string((char)value, 1) + ((index + 1 < text.Length) ? text.Substring(index + 1) : "");
                    return newtext;
                }
            }
            return text + "(&" + new string((char)value, 1) + ")";
        }

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
