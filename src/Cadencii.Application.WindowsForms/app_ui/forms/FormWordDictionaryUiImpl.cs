/*
 * FormWordDictionaryUiImpl.cs
 * Copyright © 2008-2011 kbinani
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
using cadencii.apputil;
using Cadencii.Media.Vsq;
using cadencii;
using cadencii.java.util;

using Cadencii.Gui;
using DialogResult = System.Windows.Forms.DialogResult;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Forms
{
    class FormWordDictionaryUiImpl : FormImpl, FormWordDictionaryUi
    {
        private FormWordDictionaryUiListener listener;

        public FormWordDictionaryUiImpl(FormWordDictionaryUiListener listener)
        {
            this.listener = listener;
            InitializeComponent();
			registerEventHandlers ();
            AwtHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
        }

        public void listDictionariesSetColumnWidth(int columnWidth)
        {
            listDictionaries.Columns[0].Width = columnWidth;
        }

        public int listDictionariesGetColumnWidth()
        {
            return listDictionaries.Columns[0].Width;
        }


        #region FormWordDictionaryUiの実装

        public void setTitle(string value)
        {
            this.Text = value;
        }

        public int getWidth()
        {
            return this.Width;
        }

        public int getHeight()
        {
            return this.Height;
        }

        public void setLocation(int x, int y)
        {
            this.Location = new System.Drawing.Point(x, y);
        }

        public void close()
        {
            this.Close();
        }

        /*public void listDictionariesSetColumnHeader( string header )
        {
            if ( listDictionaries.Columns.Count < 1 )
            {
                listDictionaries.Columns.Add( "" );
            }
            listDictionaries.Columns[0].Text = header;
        }*/

        public void listDictionariesClearSelection()
        {
            listDictionaries.SelectedIndices.Clear();
        }

        public void listDictionariesAddRow(string value, bool selected)
        {
            ListViewItem item = new ListViewItem(new string[] { value });
            if (listDictionaries.Columns.Count < 1) {
                listDictionaries.Columns.Add("");
            }
            item.Selected = selected;
            listDictionaries.Items.Add(item);
        }

        public void listDictionariesSetRowChecked(int row, bool value)
        {
            listDictionaries.Items[row].Checked = value;
        }

        public void listDictionariesSetSelectedRow(int row)
        {
            for (int i = 0; i < listDictionaries.Items.Count; i++) {
                listDictionaries.Items[i].Selected = (i == row);
            }
        }

        public void listDictionariesSetItemAt(int row, string value)
        {
            listDictionaries.Items[row].SubItems[0].Text = value;
        }

        public bool listDictionariesIsRowChecked(int row)
        {
            return listDictionaries.Items[row].Checked;
        }

        public string listDictionariesGetItemAt(int row)
        {
            return listDictionaries.Items[row].SubItems[0].Text;
        }

        public int listDictionariesGetSelectedRow()
        {
            if (listDictionaries.SelectedIndices.Count == 0) {
                return -1;
            } else {
                return listDictionaries.SelectedIndices[0];
            }
        }

        public int listDictionariesGetItemCountRow()
        {
            return listDictionaries.Items.Count;
        }

        public void listDictionariesClear()
        {
            listDictionaries.Items.Clear();
        }

        public void buttonDownSetText(string value)
        {
            btnDown.Text = value;
        }

        public void buttonUpSetText(string value)
        {
            btnUp.Text = value;
        }

        public void buttonOkSetText(string value)
        {
            btnOK.Text = value;
        }

        public void buttonCancelSetText(string value)
        {
            btnCancel.Text = value;
        }

        public void labelAvailableDictionariesSetText(string value)
        {
            lblAvailableDictionaries.Text = value;
        }

        public void setSize(int width, int height)
        {
            this.Size = new System.Drawing.Size(width, height);
        }

        public void setDialogResult(bool value)
        {
            if (value) {
                this.DialogResult = DialogResult.OK;
            } else {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        #endregion


        #region UiBaseの実装

        public int showDialog(object parent_form)
        {
            DialogResult ret;
            if (parent_form == null || (parent_form != null && !(parent_form is Form))) {
                ret = base.ShowDialog();
            } else {
                Form form = (Form)parent_form;
                ret = base.ShowDialog(form);
            }
            if (ret == DialogResult.OK || ret == DialogResult.Yes) {
                return 1;
            } else {
                return 0;
            }
        }

        #endregion


        #region イベントハンドラ

        void btnCancel_Click(object sender, EventArgs e)
        {
            if (listener != null) {
                listener.buttonCancelClick();
            }
        }

        void btnDown_Click(object sender, EventArgs e)
        {
            if (listener != null) {
                listener.buttonDownClick();
            }
        }

        void btnUp_Click(object sender, EventArgs e)
        {
            if (listener != null) {
                listener.buttonUpClick();
            }
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            if (listener != null) {
                listener.buttonOkClick();
            }
        }

        void FormWordDictionaryUiImpl_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (listener != null) {
                listener.formClosing();
            }
        }

        void FormWordDictionaryUiImpl_Load(object sender, EventArgs e)
        {
            if (listener != null) {
                listener.formLoad();
            }
        }

        #endregion

        private ColumnHeader columnHeader1;


        #region UI implementation

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
            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "FormWordDictionaryUi.xml");
            this.ResumeLayout(false);
            this.PerformLayout();
        }

		void registerEventHandlers ()
		{
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
			this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
			this.Load += new System.EventHandler(this.FormWordDictionaryUiImpl_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormWordDictionaryUiImpl_FormClosing);
		}

        #endregion

        private ListView listDictionaries;
        private Label lblAvailableDictionaries;
        private Button btnOK;
        private Button btnCancel;
        private Button btnUp;
        private Button btnDown;
        #endregion

    }

}
