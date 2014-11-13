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
using System.Windows.Forms;
using Cadencii.Gui;

using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;
using NMouseEventArgs = Cadencii.Gui.Toolkit.MouseEventArgs;
using NMouseEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.MouseEventArgs>;
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
        private FlowLayoutPanel panelIcon;
        private ToolTip toolTip;
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
            p.MouseDown += (sender, e) => handleMouseDown (sender, e.ToWF ());
            p.MouseUp += (sender, e) => handleMouseUp (sender, e.ToWF ());
            p.MouseMove += (sender, e) => handleMouseMove (sender, e.ToWF ());
            panelIcon.BringToFront();
            panelIcon.Controls.Add((Control) p.Native);
        }

        #endregion

        #region helper methods
        private void setResources()
        {
            this.BackgroundImage = cadencii.Properties.Resources.splash;
        }

        private void registerEventHandlers()
        {
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(handleMouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(handleMouseUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(handleMouseMove);
            panelIcon.MouseDown += new MouseEventHandler(handleMouseDown);
            panelIcon.MouseUp += new MouseEventHandler(handleMouseUp);
            panelIcon.MouseMove += new MouseEventHandler(handleMouseMove);
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
            var point = this.PointToClient(new System.Drawing.Point(screen.X, screen.Y));
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
            var p = new System.Drawing.Point(screen.X - mouseDownedLocation.X, screen.Y - mouseDownedLocation.Y);
            this.Location = p;
        }
        #endregion

        #region ui implementation
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
		ApplicationUIHost.Instance.ApplyXml (this, "FormSplashUi.xml");
            this.ResumeLayout(false);

        }
        #endregion

    }

}
