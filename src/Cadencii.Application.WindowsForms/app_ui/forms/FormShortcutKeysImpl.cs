/*
 * FormShortcutKeys.cs
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
using System.Collections.Generic;
using cadencii.apputil;
using cadencii;
using cadencii.java.util;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Forms
{

    public class FormShortcutKeysImpl : FormImpl, FormShortcutKeys
    {
        /// <summary>
        /// カテゴリーのリスト
        /// </summary>
        private static readonly string[] mCategories = new string[]{
            "menuFile", "menuEdit", "menuVisual", "menuJob", "menuLyric", "menuTrack",
            "menuScript", "menuSetting", "menuHelp", ".other" };
        private static int mColumnWidthCommand = 272;
        private static int mColumnWidthShortcutKey = 177;
        private static int mWindowWidth = 541;
        private static int mWindowHeight = 572;

        private SortedDictionary<string, ValuePair<string, Keys[]>> mDict;
        private SortedDictionary<string, ValuePair<string, Keys[]>> mFirstDict;
        private List<string> mFieldName = new List<string>();
        private FormMainImpl mMainForm = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dict">メニューアイテムの表示文字列をキーとする，メニューアイテムのフィールド名とショートカットキーのペアを格納したマップ</param>
        public FormShortcutKeysImpl(SortedDictionary<string, ValuePair<string, Keys[]>> dict, FormMainImpl main_form)
        {
            try {
                InitializeComponent();
            } catch (Exception ex) {
#if DEBUG
                Logger.StdErr("FormShortcutKeys#.ctor; ex=" + ex);
#endif
            }

#if DEBUG
            Logger.StdOut("FormShortcutKeys#.ctor; dict.size()=" + dict.Count);
            Logger.StdOut("FormShortcutKeys#.ctor; mColumnWidthCommand=" + mColumnWidthCommand + "; mColumnWidthShortcutKey=" + mColumnWidthShortcutKey);
#endif
            mMainForm = main_form;
            list.SetColumnHeaders(new string[] { _("Command"), _("Shortcut Key") });
            list.Columns[0].Width = mColumnWidthCommand;
            list.Columns[1].Width = mColumnWidthShortcutKey;

            applyLanguage();
            setResources();

            mDict = dict;
            comboCategory.SelectedIndex = 0;
            mFirstDict = new SortedDictionary<string, ValuePair<string, Keys[]>>();
            copyDict(mDict, mFirstDict);

            comboEditKey.Items.Clear();
            comboEditKey.Items.Add(Keys.None);
            // アルファベット順になるように一度配列に入れて並べ替える
            int size = EditorManager.SHORTCUT_ACCEPTABLE.Count;
            Keys[] keys = new Keys[size];
            for (int i = 0; i < size; i++) {
                keys[i] = EditorManager.SHORTCUT_ACCEPTABLE[i];
            }
            bool changed = true;
            while (changed) {
                changed = false;
                for (int i = 0; i < size - 1; i++) {
                    for (int j = i + 1; j < size; j++) {
                        string itemi = keys[i] + "";
                        string itemj = keys[j] + "";
                        if (itemi.CompareTo(itemj) > 0) {
                            Keys t = keys[i];
                            keys[i] = keys[j];
                            keys[j] = t;
                            changed = true;
                        }
                    }
                }
            }
            foreach (Keys key in keys) {
                comboEditKey.Items.Add(key);
            }
			this.AsAwt ().Size = new Dimension (mWindowWidth, mWindowHeight);

            registerEventHandlers();
            updateList();
            AwtHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
        }

        #region public methods
        public void applyLanguage()
        {
            this.Text = _("Shortcut Config");

            btnOK.Text = _("OK");
            btnCancel.Text = _("Cancel");
            btnRevert.Text = _("Revert");
            btnLoadDefault.Text = _("Load Default");

            list.SetColumnHeaders(new string[] { _("Command"), _("Shortcut Key") });

            labelCategory.Text = _("Category");
            int selected = comboCategory.SelectedIndex;
            comboCategory.Items.Clear();
            foreach (string category in mCategories) {
                string c = category;
                if (category == "menuFile") {
                    c = _("File");
                } else if (category == "menuEdit") {
                    c = _("Edit");
                } else if (category == "menuVisual") {
                    c = _("Visual");
                } else if (category == "menuJob") {
                    c = _("Job");
                } else if (category == "menuLyric") {
                    c = _("Lyric");
                } else if (category == "menuTrack") {
                    c = _("Track");
                } else if (category == "menuScript") {
                    c = _("Script");
                } else if (category == "menuSetting") {
                    c = _("Setting");
                } else if (category == "menuHelp") {
                    c = _("Help");
                } else {
                    c = _("Other");
                }
                comboCategory.Items.Add(c);
            }
            if (comboCategory.Items.Count <= selected) {
                selected = comboCategory.Items.Count - 1;
            }
            comboCategory.SelectedIndex = selected;

            labelCommand.Text = _("Command");
            labelEdit.Text = _("Edit");
            labelEditKey.Text = _("Key:");
            labelEditModifier.Text = _("Modifier:");
        }

        public SortedDictionary<string, ValuePair<string, Keys[]>> getResult()
        {
            SortedDictionary<string, ValuePair<string, Keys[]>> ret = new SortedDictionary<string, ValuePair<string, Keys[]>>();
            copyDict(mDict, ret);
            return ret;
        }
        #endregion

        #region helper methods
        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private static void copyDict(SortedDictionary<string, ValuePair<string, Keys[]>> src, SortedDictionary<string, ValuePair<string, Keys[]>> dest)
        {
            dest.Clear();
            foreach (var name in src.Keys) {
                string key = src[name].getKey();
                Keys[] values = src[name].getValue();
                List<Keys> cp = new List<Keys>();
                foreach (Keys k in values) {
                    cp.Add(k);
                }
                dest[name] = new ValuePair<string, Keys[]>(key, cp.ToArray());
            }
        }

        /// <summary>
        /// リストを更新します
        /// </summary>
        private void updateList()
        {
            list.SelectedIndexChanged -= new EventHandler(list_SelectedIndexChanged);
            list.ClearItems();
            list.SelectedIndexChanged += new EventHandler(list_SelectedIndexChanged);
            mFieldName.Clear();

            // 現在のカテゴリーを取得
            int selected = comboCategory.SelectedIndex;
            if (selected < 0) {
                selected = 0;
            }
            string category = mCategories[selected];

            // 現在のカテゴリーに合致するものについてのみ，リストに追加
            foreach (var display in mDict.Keys) {
                ValuePair<string, Keys[]> item = mDict[display];
                string field_name = item.getKey();
                Keys[] keys = item.getValue();
                bool add_this_one = false;
                if (category == ".other") {
                    add_this_one = true;
                    for (int i = 0; i < mCategories.Length; i++) {
                        string c = mCategories[i];
                        if (c == ".other") {
                            continue;
                        }
                        if (field_name.StartsWith(c)) {
                            add_this_one = false;
                            break;
                        }
                    }
                } else {
                    if (field_name.StartsWith(category)) {
                        add_this_one = true;
                    }
                }
                if (add_this_one) {
                    list.AddRow(new string[] { display, Utility.getShortcutDisplayString(keys) }, false);
                    mFieldName.Add(field_name);
                }
            }

            updateColor();
            //applyLanguage();
        }

        /// <summary>
        /// リストアイテムの背景色を更新します．
        /// 2つ以上のメニューに対して同じショートカットが割り当てられた場合に警告色で表示する．
        /// </summary>
        private void updateColor()
        {
            int size = list.ItemCount;
            for (int i = 0; i < size; i++) {
                //BListViewItem list_item = list.getItemAt( i );
                string field_name = mFieldName[i];
				string key_display = list.GetItem (i).GetSubItem (1).Text;
                if (key_display == "") {
                    // ショートカットキーが割り当てられていないのでスルー
					list.GetItem (i).BackColor = Colors.White;
                    continue;
                }

                bool found = false;
                foreach (var display1 in mDict.Keys) {
                    ValuePair<string, Keys[]> item1 = mDict[display1];
                    string field_name1 = item1.getKey();
                    if (field_name == field_name1) {
                        // 自分自身なのでスルー
                        continue;
                    }
                    Keys[] keys1 = item1.getValue();
                    string key_display1 = Utility.getShortcutDisplayString(keys1);
                    if (key_display == key_display1) {
                        // 同じキーが割り当てられてる！！
                        found = true;
                        break;
                    }
                }

                // 背景色を変える
                if (found) {
                    list.GetItem(i).BackColor = Colors.Yellow;
                } else {
                    list.GetItem(i).BackColor = Colors.White;
                }
            }
        }

        private void registerEventHandlers()
        {
            btnLoadDefault.Click += new EventHandler(btnLoadDefault_Click);
            btnRevert.Click += new EventHandler(btnRevert_Click);
			this.FormClosing += (o,e) => FormShortcutKeys_FormClosing ();
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            comboCategory.SelectedIndexChanged += new EventHandler(comboCategory_SelectedIndexChanged);
            list.SelectedIndexChanged += new EventHandler(list_SelectedIndexChanged);
            this.SizeChanged += new EventHandler(FormShortcutKeys_SizeChanged);
            reRegisterHandlers();
        }

        private void unRegisterHandlers()
        {
            comboEditKey.SelectedIndexChanged -= new EventHandler(comboEditKey_SelectedIndexChanged);
            checkCommand.CheckedChanged -= new EventHandler(handleModifier_CheckedChanged);
            checkShift.CheckedChanged -= new EventHandler(handleModifier_CheckedChanged);
            checkControl.CheckedChanged -= new EventHandler(handleModifier_CheckedChanged);
            checkOption.CheckedChanged -= new EventHandler(handleModifier_CheckedChanged);
        }

        private void reRegisterHandlers()
        {
            comboEditKey.SelectedIndexChanged += new EventHandler(comboEditKey_SelectedIndexChanged);
            checkCommand.CheckedChanged += new EventHandler(handleModifier_CheckedChanged);
            checkShift.CheckedChanged += new EventHandler(handleModifier_CheckedChanged);
            checkControl.CheckedChanged += new EventHandler(handleModifier_CheckedChanged);
            checkOption.CheckedChanged += new EventHandler(handleModifier_CheckedChanged);
        }

        private void setResources()
        {
        }
        #endregion

        #region event handlers
        public void FormShortcutKeys_SizeChanged(Object sender, EventArgs e)
        {
            mWindowWidth = this.Width;
            mWindowHeight = this.Height;
        }

        public void handleModifier_CheckedChanged(Object sender, EventArgs e)
        {
            updateSelectionKeys();
        }

        public void comboEditKey_SelectedIndexChanged(Object sender, EventArgs e)
        {
            updateSelectionKeys();
        }

        /// <summary>
        /// 現在選択中のコマンドのショートカットキーを，comboEditKey, 
        /// checkCommand, checkShift, checkControl, checkControlの状態にあわせて変更します．
        /// </summary>
        private void updateSelectionKeys()
        {
            int indx = comboEditKey.SelectedIndex;
            if (indx < 0) {
                return;
            }
            if (list.SelectedIndices.Count == 0) {
                return;
            }
			int indx_row = (int) list.SelectedIndices[0];
            Keys key = (Keys)comboEditKey.Items[indx];
			string display = list.GetItem(indx_row).GetSubItem(0).Text;
            if (!mDict.ContainsKey(display)) {
                return;
            }
            List<Keys> capturelist = new List<Keys>();
            if (key != Keys.None) {
                capturelist.Add(key);
                if (checkCommand.Checked) {
                    capturelist.Add(Keys.Menu);
                }
                if (checkShift.Checked) {
                    capturelist.Add(Keys.Shift);
                }
                if (checkControl.Checked) {
                    capturelist.Add(Keys.Control);
                }
                if (checkOption.Checked) {
                    capturelist.Add(Keys.Alt);
                }
            }
            Keys[] keys = capturelist.ToArray();
            mDict[display].setValue(keys);
			list.GetItem(indx_row).GetSubItem(1).Text = Utility.getShortcutDisplayString(keys);
        }

        public void list_SelectedIndexChanged(Object sender, EventArgs e)
        {
            if (list.SelectedIndices.Count == 0) {
                return;
            }
			int indx = (int) list.SelectedIndices[0];
			string display = list.GetItem(indx).GetSubItem(0).Text;
            if (!mDict.ContainsKey(display)) {
                return;
            }
            unRegisterHandlers();
            ValuePair<string, Keys[]> item = mDict[display];
            Keys[] keys = item.getValue();
            List<Keys> vkeys = new List<Keys>(keys);
            checkCommand.Checked = vkeys.Contains(Keys.Menu);
            checkShift.Checked = vkeys.Contains(Keys.Shift);
            checkControl.Checked = vkeys.Contains(Keys.Control);
            checkOption.Checked = vkeys.Contains(Keys.Alt);
            int size = comboEditKey.Items.Count;
            comboEditKey.SelectedIndex = -1;
            for (int i = 0; i < size; i++) {
                Keys k = (Keys)comboEditKey.Items[i];
                if (vkeys.Contains(k)) {
                    comboEditKey.SelectedIndex = i;
                    break;
                }
            }
            reRegisterHandlers();
        }

        public void comboCategory_SelectedIndexChanged(Object sender, EventArgs e)
        {
            int selected = comboCategory.SelectedIndex;
#if DEBUG
            Logger.StdOut("FormShortcutKeys#comboCategory_selectedIndexChanged; selected=" + selected);
#endif
            if (selected < 0) {
                comboCategory.SelectedIndex = 0;
                //updateList();
                return;
            }
            updateList();
        }

        public void btnRevert_Click(Object sender, EventArgs e)
        {
            copyDict(mFirstDict, mDict);
            updateList();
        }

        public void btnLoadDefault_Click(Object sender, EventArgs e)
        {
            var defaults = mMainForm.getDefaultShortcutKeys();
            for (int i = 0; i < defaults.Count; i++) {
                string name = defaults[i].Key;
                Keys[] keys = defaults[i].Value;
                foreach (var display in mDict.Keys) {
                    if (name.Equals(mDict[display].getKey())) {
                        mDict[display].setValue(keys);
                        break;
                    }
                }
            }
            updateList();
        }

        public void FormShortcutKeys_FormClosing()
        {
            mColumnWidthCommand = list.Columns[0].Width;
            mColumnWidthShortcutKey = list.Columns[1].Width;
#if DEBUG
            Logger.StdOut("FormShortCurKeys#FormShortcutKeys_FormClosing; columnWidthCommand,columnWidthShortcutKey=" + mColumnWidthCommand + "," + mColumnWidthShortcutKey);
#endif
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
            this.AsAwt ().DialogResult = Cadencii.Gui.DialogResult.Cancel;
        }

        public void btnOK_Click(Object sender, EventArgs e)
        {
            this.AsAwt ().DialogResult = Cadencii.Gui.DialogResult.OK;
        }
        #endregion

        #region UI implementation
        #region UI Impl for C#
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "FormShortcutKeys.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		#pragma warning disable 0649
        UiButton btnCancel;
        UiButton btnOK;
        UiListView list;
        UiButton btnLoadDefault;
        UiButton btnRevert;
        UiToolTip toolTip;
        UiLabel labelCategory;
        UiComboBox comboCategory;
        UiLabel labelCommand;
        UiLabel labelEdit;
        UiLabel labelEditKey;
        UiLabel labelEditModifier;
        UiComboBox comboEditKey;
        UiCheckBox checkCommand;
        UiCheckBox checkShift;
        UiCheckBox checkControl;
        UiCheckBox checkOption;
		#pragma warning restore 0649

        #endregion
        #endregion

    }

}
