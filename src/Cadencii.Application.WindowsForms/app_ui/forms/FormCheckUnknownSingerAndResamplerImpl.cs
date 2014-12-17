/*
 * FormCheckUnknownSingerAndResampler.cs
 * Copyright © 2010 kbinani
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
using cadencii.java.util;

using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;



namespace Cadencii.Application.Forms
{

	public class FormCheckUnknownSingerAndResamplerImpl : FormImpl, FormCheckUnknownSingerAndResampler
    {
        /// <summary>
        /// コンストラクタ．
        /// </summary>
        /// <param name="singer"></param>
        /// <param name="apply_singer"></param>
        /// <param name="resampler"></param>
        /// <param name="apply_resampler"></param>
        public FormCheckUnknownSingerAndResamplerImpl(string singer, bool apply_singer, string resampler, bool apply_resampler)
        {
            InitializeComponent();
            applyLanguage();
            AwtHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());

            // singers
            checkSingerImport.Checked = apply_singer;
            checkSingerImport.Enabled = apply_singer;
            textSingerPath.ReadOnly = true;
            textSingerPath.Enabled = apply_singer;
            if (apply_singer) {
                textSingerPath.Text = singer;
                SingerConfig sc = new SingerConfig();
                string path_image = cadencii.utau.Utau.readUtauSingerConfig(singer, sc);
#if DEBUG
                Logger.StdOut("FormCheckUnknownSingerAndResampler#.ctor;  path_image=" + path_image);
#endif
		pictureSinger.Image = IconParaderController.createIconImage(path_image, sc.VOICENAME);
                labelSingerName.Text = sc.VOICENAME;
            }

            // resampler
            checkResamplerImport.Checked = apply_resampler;
            checkResamplerImport.Enabled = apply_resampler;
            textResamplerPath.ReadOnly = true;
            textResamplerPath.Enabled = apply_resampler;
            if (apply_resampler) {
                textResamplerPath.Text = resampler;
            }

            registerEventHandlers();
        }

        #region public methods
        /// <summary>
        /// 原音の項目にチェックが入れられたか
        /// </summary>
        /// <returns></returns>
        public bool isSingerChecked()
        {
            return checkSingerImport.Checked;
        }

        /// <summary>
        /// 原音のパスを取得します
        /// </summary>
        /// <returns></returns>
        public string getSingerPath()
        {
            return textSingerPath.Text;
        }

        /// <summary>
        /// リサンプラーの項目にチェックが入れられたかどうか
        /// </summary>
        /// <returns></returns>
        public bool isResamplerChecked()
        {
            return checkResamplerImport.Checked;
        }

        /// <summary>
        /// リサンプラーのパスを取得します
        /// </summary>
        /// <returns></returns>
        public string getResamplerPath()
        {
            return textResamplerPath.Text;
        }
        #endregion

        #region helper methods
        /// <summary>
        /// イベントハンドラを登録します
        /// </summary>
        private void registerEventHandlers()
        {
        }

        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void applyLanguage()
        {
            this.Text = _("Unknown singers and resamplers");
            labelMessage.Text = _("These singers and resamplers are not registered to Cadencii.\nCheck the box if you want to register them.");
            checkSingerImport.Text = _("Import singer");
            checkResamplerImport.Text = _("Import resampler");
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
			ApplicationUIHost.Instance.ApplyXml (this, "FormCheckUnknownSingerAndResampler.xml");
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

		#pragma warning disable 0649
        UiButton buttonCancel;
        UiButton buttonOk;
        UiLabel labelMessage;
        UiCheckBox checkSingerImport;
        UiLabel labelSingerName;
        UiTextBox textSingerPath;
        UiCheckBox checkResamplerImport;
        UiTextBox textResamplerPath;
        IconParader pictureSinger;
		#pragma warning restore 0649

        #endregion
    }

}
