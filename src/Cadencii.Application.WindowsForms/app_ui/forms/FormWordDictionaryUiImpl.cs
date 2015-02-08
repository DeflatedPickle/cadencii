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
using cadencii.apputil;
using Cadencii.Media.Vsq;
using cadencii;
using Cadencii.Gui;
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
            GuiHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
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
			this.AsGui ().Location = new Point(x, y);
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
			var item = ApplicationUIHost.Create<UiListViewItem> (new string[] { value });
            if (listDictionaries.Columns.Count < 1) {
				listDictionaries.Columns.Add(ApplicationUIHost.Create<UiListViewColumn> ());
            }
            item.Selected = selected;
            listDictionaries.Items.Add (item);
        }

        public void listDictionariesSetRowChecked(int row, bool value)
        {
            listDictionaries.Items[row].Checked = value;
        }

        public void SetSelectedRow(int row)
        {
            for (int i = 0; i < listDictionaries.Items.Count; i++) {
				listDictionaries.Items[i].Selected = (i == row);
            }
        }

        public void listDictionariesSetItemAt(int row, string value)
        {
            listDictionaries.Items[row].GetSubItem(0).Text = value;
        }

        public bool listDictionariesIsRowChecked(int row)
        {
            return listDictionaries.Items[row].Checked;
        }

        public string listDictionariesGetItemAt(int row)
        {
            return listDictionaries.Items[row].GetSubItem(0).Text;
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
			this.AsGui ().Size = new Size(width, height);
        }

        public void setDialogResult(bool value)
        {
            if (value) {
				this.AsGui ().DialogResult = Cadencii.Gui.DialogResult.OK;
            } else {
				this.AsGui ().DialogResult = Cadencii.Gui.DialogResult.Cancel;
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

        void FormWordDictionaryUiImpl_FormClosing()
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

		private UiListViewColumn columnHeader1;


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
			this.FormClosing += (o,e) => this.FormWordDictionaryUiImpl_FormClosing ();
		}

        #endregion

		#pragma warning disable 0169,0649
		public UiListView listDictionaries { get; set; }
        private UiLabel lblAvailableDictionaries;
        private UiButton btnOK;
        private UiButton btnCancel;
        private UiButton btnUp;
        private UiButton btnDown;
		#pragma warning restore 0169,0649
        #endregion

    }

}
