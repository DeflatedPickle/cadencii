/*
 * FormVibratoConfig.cs
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
using cadencii;
using cadencii.apputil;
using Cadencii.Media.Vsq;

using cadencii.java.util;
using Keys = Cadencii.Gui.Toolkit.Keys;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;
using Cadencii.Media;


namespace Cadencii.Application.Forms
{

	public class FormVibratoConfigImpl : FormImpl, FormVibratoConfig
    {
        private VibratoHandle m_vibrato;
        private int m_note_length;

        /// <summary>
        /// コンストラクタ．引数vibrato_handleには，Cloneしたものを渡さなくてよい．
        /// </summary>
        /// <param name="vibrato_handle"></param>
        /// <param name="note_length"></param>
        /// <param name="default_vibrato_length"></param>
        /// <param name="type"></param>
        /// <param name="use_original"></param>
        public FormVibratoConfigImpl(
            VibratoHandle vibrato_handle,
            int note_length,
            DefaultVibratoLengthEnum default_vibrato_length,
            SynthesizerType type,
            bool use_original)
        {
            InitializeComponent();

#if DEBUG
            CDebug.WriteLine("FormVibratoConfig.ctor(Vsqhandle,int,DefaultVibratoLength)");
            CDebug.WriteLine("    (vibrato_handle==null)=" + (vibrato_handle == null));
            Logger.StdOut("    type=" + type);
#endif
            if (use_original) {
                radioUserDefined.Checked = true;
            } else {
                if (type == SynthesizerType.VOCALOID1) {
                    radioVocaloid1.Checked = true;
                } else {
                    radioVocaloid2.Checked = true;
                }
            }
            if (vibrato_handle != null) {
                m_vibrato = (VibratoHandle)vibrato_handle.clone();
            }

            // 選択肢の状態を更新
            updateComboBoxStatus();
            // どれを選ぶか？
            if (vibrato_handle != null) {
#if DEBUG
                Logger.StdOut("FormVibratoConfig#.ctor; vibrato_handle.IconID=" + vibrato_handle.IconID);
#endif
                for (int i = 0; i < comboVibratoType.Items.Count; i++) {
                    VibratoHandle handle = (VibratoHandle)comboVibratoType.Items[i];
#if DEBUG
                    Logger.StdOut("FormVibratoConfig#.ctor; handle.IconID=" + handle.IconID);
#endif
                    if (vibrato_handle.IconID.Equals(handle.IconID)) {
                        comboVibratoType.SelectedIndex = i;
                        break;
                    }
                }
            }

            txtVibratoLength.Enabled = (vibrato_handle != null);
            if (vibrato_handle != null) {
                txtVibratoLength.Text = (int)((float)vibrato_handle.getLength() / (float)note_length * 100.0f) + "";
            } else {
                string s = "";
                if (default_vibrato_length == DefaultVibratoLengthEnum.L100) {
                    s = "100";
                } else if (default_vibrato_length == DefaultVibratoLengthEnum.L50) {
                    s = "50";
                } else if (default_vibrato_length == DefaultVibratoLengthEnum.L66) {
                    s = "66";
                } else if (default_vibrato_length == DefaultVibratoLengthEnum.L75) {
                    s = "75";
                }
                txtVibratoLength.Text = s;
            }

            m_note_length = note_length;

            registerEventHandlers();
            setResources();
            applyLanguage();

            AwtHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
        }

        #region public methods
        public void applyLanguage()
        {
            this.Text = _("Vibrato property");
            lblVibratoLength.Text = _("Vibrato length");
            lblVibratoLength.Mnemonic(Keys.L);
            lblVibratoType.Text = _("Vibrato Type");
            lblVibratoType.Mnemonic(Keys.T);
            btnOK.Text = _("OK");
            btnCancel.Text = _("Cancel");
            groupSelect.Text = _("Select from");
        }

        /// <summary>
        /// 編集済みのビブラート設定．既にCloneされているので，改めてCloneしなくて良い
        /// </summary>
        public VibratoHandle getVibratoHandle()
        {
            return m_vibrato;
        }
        #endregion

        #region helper methods
        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        /// <summary>
        /// ビブラートの選択肢の状態を更新します
        /// </summary>
        private void updateComboBoxStatus()
        {
            // 選択位置
            int old = comboVibratoType.SelectedIndex;

            // 全部削除
            comboVibratoType.Items.Clear();

            // 「ビブラート無し」を表すアイテムを追加
            VibratoHandle empty = new VibratoHandle();
            empty.setCaption("[Non Vibrato]");
            empty.IconID = "$04040000";
            comboVibratoType.Items.Add(empty);

            // 選択元を元に，選択肢を追加する
            if (radioUserDefined.Checked) {
                // ユーザー定義のを使う場合
                int size = EditorManager.editorConfig.AutoVibratoCustom.Count;
                for (int i = 0; i < size; i++) {
                    VibratoHandle handle = EditorManager.editorConfig.AutoVibratoCustom[i];
                    comboVibratoType.Items.Add(handle);
                }
            } else {
                // VOCALOID1/VOCALOID2のシステム定義のを使う場合
                SynthesizerType type = radioVocaloid1.Checked ? SynthesizerType.VOCALOID1 : SynthesizerType.VOCALOID2;
                foreach (var vconfig in VocaloSysUtil.vibratoConfigIterator(type)) {
                    comboVibratoType.Items.Add(vconfig);
                }
            }

            // 選択位置を戻せるなら戻す
            int index = old;
            if (index >= comboVibratoType.Items.Count) {
                index = comboVibratoType.Items.Count - 1;
            }
            if (0 <= index) {
                comboVibratoType.SelectedIndex = index;
            }
        }

        private void registerEventHandlers()
        {
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            radioUserDefined.CheckedChanged += new EventHandler(handleRadioCheckedChanged);
            comboVibratoType.SelectedIndexChanged += new EventHandler(comboVibratoType_SelectedIndexChanged);
            txtVibratoLength.TextChanged += new EventHandler(txtVibratoLength_TextChanged);
        }

        public void handleRadioCheckedChanged(Object sender, EventArgs e)
        {
            comboVibratoType.SelectedIndexChanged -= new EventHandler(comboVibratoType_SelectedIndexChanged);
            updateComboBoxStatus();
            comboVibratoType.SelectedIndexChanged += new EventHandler(comboVibratoType_SelectedIndexChanged);
        }

        private void setResources()
        {
        }
        #endregion

        #region event handlers
        public void btnOK_Click(Object sender, EventArgs e)
        {
			this.AsAwt ().DialogResult = Cadencii.Gui.DialogResult.OK;
        }

        public void comboVibratoType_SelectedIndexChanged(Object sender, EventArgs e)
        {
            int index = comboVibratoType.SelectedIndex;
#if DEBUG
            Logger.StdOut("FormVibratoConfig#comboVibratoType_SelectedIndexChanged; index=" + index);
#endif
            if (index >= 0) {
                string s = ((VibratoHandle)comboVibratoType.Items[index]).IconID;
#if DEBUG
                Logger.StdOut("FormVibratoConfig#comboVibratoType_SelectedIndexChanged; index=" + index + "; iconid=" + s);
#endif
                if (s.Equals("$04040000")) {
#if DEBUG
                    Logger.StdOut("FormVibratoConfig#comboVibratoType_SelectedIndexChanged; B; m_vibrato -> null");
#endif
                    m_vibrato = null;
                    txtVibratoLength.Enabled = false;
                    return;
                } else {
                    txtVibratoLength.Enabled = true;
                    VibratoHandle src = null;
                    if (radioUserDefined.Checked) {
                        int size = EditorManager.editorConfig.AutoVibratoCustom.Count;
                        for (int i = 0; i < size; i++) {
                            VibratoHandle handle = EditorManager.editorConfig.AutoVibratoCustom[i];
                            if (s == handle.IconID) {
                                src = handle;
                                break;
                            }
                        }
                    } else {
                        SynthesizerType type = radioVocaloid1.Checked ? SynthesizerType.VOCALOID1 : SynthesizerType.VOCALOID2;
                        foreach (var vconfig in VocaloSysUtil.vibratoConfigIterator(type)) {
                            if (s == vconfig.IconID) {
                                src = vconfig;
                                break;
                            }
                        }
                    }
#if DEBUG
                    Logger.StdOut("FormVibratoConfig#comboVibratoType_SelectedIndexChanged; (src==null)=" + (src == null));
#endif
                    if (src != null) {
                        int percent;
                        try {
                            percent = int.Parse(txtVibratoLength.Text);
                        } catch (Exception ex) {
                            return;
                        }
                        m_vibrato = (VibratoHandle)src.clone();
                        m_vibrato.setLength((int)(m_note_length * percent / 100.0f));
                        return;
                    }
                }
            }
        }

        public void txtVibratoLength_TextChanged(Object sender, EventArgs e)
        {
#if DEBUG
            CDebug.WriteLine("txtVibratoLength_TextChanged");
            CDebug.WriteLine("    (m_vibrato==null)=" + (m_vibrato == null));
#endif
            int percent = 0;
            try {
                percent = int.Parse(txtVibratoLength.Text);
                if (percent < 0) {
                    percent = 0;
                } else if (100 < percent) {
                    percent = 100;
                }
            } catch (Exception ex) {
                return;
            }
            if (percent == 0) {
                m_vibrato = null;
#if DEBUG
                Logger.StdOut("FormVibratoConfig#txtVibratoLength_TextChanged; A; m_vibrato -> null");
#endif
                txtVibratoLength.Enabled = false;
            } else {
                if (m_vibrato != null) {
                    int new_length = (int)(m_note_length * percent / 100.0f);
                    m_vibrato.setLength(new_length);
                }
            }
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
			this.AsAwt ().DialogResult = Cadencii.Gui.DialogResult.Cancel;
        }
        #endregion

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
            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "FormVibratoConfig.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

		#pragma warning disable 0169,0649
        UiLabel lblVibratoLength;
        UiLabel lblVibratoType;
        NumberTextBox txtVibratoLength;
        UiLabel label3;
        UiComboBox comboVibratoType;
        UiButton btnCancel;
        UiButton btnOK;
        UiGroupBox groupSelect;
        UiRadioButton radioVocaloid2;
        UiRadioButton radioVocaloid1;
        UiRadioButton radioUserDefined;
		#pragma warning restore 0169,0649
        #endregion
    }

}
