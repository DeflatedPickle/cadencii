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
using NMouseEventArgs = Cadencii.Gui.MouseEventArgs;
using NMouseEventHandler = Cadencii.Gui.MouseEventHandler;

namespace cadencii
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
            this.BackgroundImage = Properties.Resources.splash;
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
			Point screen = Cadencii.Gui.Screen.Instance.GetScreenMousePosition();
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

			Point screen = Cadencii.Gui.Screen.Instance.GetScreenMousePosition();
            var p = new System.Drawing.Point(screen.X - mouseDownedLocation.X, screen.Y - mouseDownedLocation.Y);
            this.Location = p;
        }
        #endregion

        #region ui implementation
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelIcon = new System.Windows.Forms.FlowLayoutPanel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // panelIcon
            // 
            this.panelIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelIcon.BackColor = System.Drawing.Color.Transparent;
            this.panelIcon.Location = new System.Drawing.Point(12, 200);
            this.panelIcon.Name = "panelIcon";
            this.panelIcon.Size = new System.Drawing.Size(476, 123);
            this.panelIcon.TabIndex = 1;
            // 
            // FormSplash
            // 
            this.ClientSize = new System.Drawing.Size(500, 335);
            this.Controls.Add(this.panelIcon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSplash";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.ResumeLayout(false);

        }
        #endregion

    }

}
