/*
 * FormSplash.cs
 * Copyright © 2010-2011 kbinani
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
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;
using cadencii;

namespace Cadencii.Application.Forms
{

    /// <summary>
    /// 起動時に表示されるスプラッシュウィンドウ
    /// </summary>
    public class FormSplashUiImpl : FormImpl, FormSplashUi
    {
        /// <summary>
        /// addIconメソッドを呼び出すときに使うデリゲート
        /// </summary>
        /// <param name="path_image"></param>
        /// <param name="singer_name"></param>
        private delegate void AddIconThreadSafeDelegate(string path_image, string singer_name);

        bool mouseDowned = false;
        private System.ComponentModel.IContainer components;
        Point mouseDownedLocation = new Point(0, 0);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormSplashUiImpl()
        {
            InitializeComponent();
            registerEventHandlers();
            setResources();
        }

        #region public methods
        /// <summary>
        /// アイコンパレードの末尾にアイコンを追加します。デリゲートを使用し、スレッド・セーフな処理を行います。
        /// </summary>
        /// <param name="path_image"></param>
        /// <param name="singer_name"></param>
        public void addIconThreadSafe(string path_image, string singer_name)
        {
            Delegate deleg = (Delegate)new AddIconThreadSafeDelegate(addIcon);
            if (deleg != null) {
                this.Invoke(deleg, new string[] { path_image, singer_name });
            }
        }

        /// <summary>
        /// アイコンパレードの末尾にアイコンを追加します
        /// </summary>
        /// <param name="path_image">イメージファイルへのパス</param>
        /// <param name="singer_name">歌手の名前</param>
        public void addIcon(string path_image, string singer_name)
        {
            IconParader p = ApplicationUIHost.Create<IconParader> ();
            var img = IconParaderController.createIconImage(path_image, singer_name);
            p.setImage(img);
			p.MouseDown += handleMouseDown;
			p.MouseUp += handleMouseUp;
			p.MouseMove += handleMouseMove;
            panelIcon.BringToFront();
            panelIcon.AddControl(p);
        }

        #endregion

        #region helper methods
        private void setResources()
        {
			// skip it, we don't show splash screen anymore.
            //this.BackgroundImage = cadencii.Properties.Resources.splash;
        }

        private void registerEventHandlers()
        {
			var form = AsGui ();
            panelIcon.MouseDown += handleMouseDown;
            panelIcon.MouseUp += handleMouseUp;
            panelIcon.MouseMove += handleMouseMove;
        }
        #endregion

        #region event handlers
        /// <summary>
        /// このスプラッシュウィンドウに，MouseDownイベントを通知します
        /// </summary>
        /// <param name="screen_x"></param>
        /// <param name="screen_y"></param>
        public void handleMouseDown(Object sender, MouseEventArgs arg)
        {
            mouseDowned = true;
			Point screen = Cadencii.Gui.Toolkit.Screen.Instance.GetScreenMousePosition();
            var point = this.PointToClient(new Point(screen.X, screen.Y));
            Point p = new Point(point.X, point.Y);
            mouseDownedLocation = p;
        }

        /// <summary>
        /// このスプラッシュウィンドウに，MouseUpイベントを通知します
        /// </summary>
        public void handleMouseUp(Object sender, MouseEventArgs arg)
        {
            mouseDowned = false;
        }

        /// <summary>
        /// このスプラッシュウィンドウに，MouseMoveイベントを通知します
        /// </summary>
        public void handleMouseMove(Object sender, MouseEventArgs arg)
        {
            if (!mouseDowned) {
                return;
            }

			Point screen = Cadencii.Gui.Toolkit.Screen.Instance.GetScreenMousePosition();
			var p = new Point(screen.X - mouseDownedLocation.X, screen.Y - mouseDownedLocation.Y);
			this.AsGui ().Location = p;
        }
        #endregion

        #region ui implementation
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
			this.toolTip = ApplicationUIHost.Create<UiToolTip>(this.components);
            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "FormSplashUi.xml");
            this.ResumeLayout(false);

        }
        #endregion

		#pragma warning disable 0169,0649
		private UiFlowLayoutPanel panelIcon;
		private UiToolTip toolTip;
		#pragma warning restore 0169,0649
    }

}
