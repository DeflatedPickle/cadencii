/*
 * VersionInfo.cs
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
using cadencii;
using Cadencii.Gui;

using Cadencii.Utilities;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Forms
{
    using Graphics = Cadencii.Gui.Graphics;

    public class VersionInfoImpl : FormImpl, VersionInfo
    {
        const float m_speed = 35f;
        const int m_height = 380;
        const int FONT_SIZE = 10;

		private readonly Color m_background = Cadencii.Gui.Colors.White;

        private double m_scroll_started;
        private AuthorListEntry[] m_credit;
        private string m_version;
        private bool m_credit_mode = false;
        private float m_last_t = 0f;
        private float m_last_speed = 0f;
        private float m_shift = 0f;
        private int m_button_width_about = 75;
        private int m_button_width_credit = 75;
        private Cadencii.Gui.Image m_scroll = null;
        private Cadencii.Gui.Image m_scroll_with_id = null;
        private string m_app_name = "";
        private Color m_app_name_color = Cadencii.Gui.Colors.Black;
        private Color m_version_color = new Color(105, 105, 105);
        private bool m_shadow_enablde = false;
        Timer timer;
        private bool m_show_twitter_id = false;

        public VersionInfoImpl(string app_name, string version)
        {
            InitializeComponent();
            if (this.components == null) {
                this.components = new System.ComponentModel.Container();
            }
			timer = ApplicationUIHost.Create<Timer> (this.components);
            m_version = version;
            m_app_name = app_name;

            timer.Interval = 30;
            registerEventHandlers();
            setResources();
            applyLanguage();

			DoubleBuffered = true;
			UserPaint = true;
			AllPaintingInWmPaint = true;

            m_credit = new AuthorListEntry[] { };
			lblVstLogo.ForeColor = m_version_color;
#if DEBUG
            //m_scroll = generateAuthorListB( false );
            //m_scroll_with_id = generateAuthorListB( true );
#endif
            chkTwitterID.Visible = false;
        }

        public bool isShowTwitterID()
        {
            return m_show_twitter_id;
        }

        public void setShowTwitterID(bool value)
        {
            m_show_twitter_id = value;
        }

        public void applyLanguage()
        {
            string about = PortUtil.formatMessage(_("About {0}"), m_app_name);
            string credit = _("Credit");
			var size1 = Utility.measureString(about, btnFlip.Font);
			var size2 = Utility.measureString(credit, btnFlip.Font);
            m_button_width_about = Math.Max(75, (int)(size1.Width * 1.3));
            m_button_width_credit = Math.Max(75, (int)(size2.Width * 1.3));
            if (m_credit_mode) {
				btnFlip.Size = new Size(m_button_width_about, btnFlip.Height);
                btnFlip.Text = about;
            } else {
				btnFlip.Size = new Size(m_button_width_credit, btnFlip.Height);
                btnFlip.Text = credit;
            }
            this.Text = about;
        }

        public static string _(string s)
        {
            return Messaging.getMessage(s);
        }

        /// <summary>
        /// バージョン番号表示の文字色を取得または設定します
        /// </summary>
        public Color getVersionColor()
        {
            return m_version_color;
        }

        public void setVersionColor(Color value)
        {
            m_version_color = value;
			lblVstLogo.ForeColor = value;
        }

        /// <summary>
        /// アプリケーション名表示の文字色を取得または設定します
        /// </summary>
        public Color getAppNameColor()
        {
            return m_app_name_color;
        }

        public void setAppNameColor(Color value)
        {
            m_app_name_color = value;
        }

        public void setCredit(Cadencii.Gui.Image value)
        {
            m_scroll = value;
        }

        public string getAppName()
        {
            return m_app_name;
        }

        public void setAppName(string value)
        {
            m_app_name = value;
        }

        public void setAuthorList(AuthorListEntry[] value)
        {
            m_credit = value;
            m_scroll = generateAuthorListB(false);
            m_scroll_with_id = generateAuthorListB(true);
        }

        private Image generateAuthorListB(bool show_twitter_id)
        {
            int shadow_shift = 2;
            string font_name = "Arial";
            Font font = new Font(font_name, Cadencii.Gui.Font.PLAIN, FONT_SIZE);
            var size = Utility.measureString("the quick brown fox jumped over the lazy dogs. THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS. 0123456789", font);
            int width = this.Width;
            int height = size.Height;
            //StringFormat sf = new StringFormat();
            Image ret = new Image((int)width, (int)(40f + m_credit.Length * height * 1.1f));
            Graphics g = new Graphics(ret);
			g.setColor(Cadencii.Gui.Colors.White);
            g.fillRect(0, 0, ret.Width, ret.Height);
            int align = 0;
            int valign = 0;
            //sf.Alignment = StringAlignment.Center;
            Font f = new Font(font_name, Cadencii.Gui.Font.BOLD, (int)(FONT_SIZE * 1.2f));
            if (m_shadow_enablde) {
                g.setColor(new Color(0, 0, 0, 40));
				g.drawStringEx(
                    m_app_name,
                    f,
                    new Rectangle(shadow_shift, shadow_shift, width, height),
                    align,
                    valign);
            }
            g.setColor(Cadencii.Gui.Colors.Black);
			g.drawStringEx(
                m_app_name,
                f,
                new Rectangle(0, 0, width, height),
                align,
                valign);
            for (int i = 0; i < m_credit.Length; i++) {
                AuthorListEntry itemi = m_credit[i];
                Font f2 = new Font(font_name, itemi.getStyle(), FONT_SIZE);
                string id = show_twitter_id ? itemi.getTwitterID() : "";
                if (id == null) {
                    id = "";
                }
                string str = itemi.getName() + (id.Equals("") ? "" : (" (" + id + ")"));
                if (m_shadow_enablde) {
                    g.setColor(new Color(0, 0, 0, 40));
					g.drawStringEx(
                        str,
                        font,
                        new Rectangle(0 + shadow_shift, 40 + (int)(i * height * 1.1) + shadow_shift, width, height),
                        align,
                        valign);
                }
                g.setColor(Cadencii.Gui.Colors.Black);
				g.drawStringEx(
                    str,
                    f2,
                    new Rectangle(0, 40 + (int)(i * height * 1.1), width, height),
                    align,
                    valign);
            }
            return ret;
        }

        void btnSaveAuthorList_Click(Object sender, EventArgs e)
        {
#if DEBUG
			using (var dlg = ApplicationUIHost.Create<UiSaveFileDialog>()) {
				if (dlg.ShowDialog() == Cadencii.Gui.DialogResult.OK) {
                    using (var stream = new System.IO.FileStream(dlg.FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write)) {
                        m_scroll.Save(stream);
                    }
                }
            }
#endif
        }

        public void btnOK_Click(Object sender, EventArgs e)
        {
			this.AsGui ().DialogResult = Cadencii.Gui.DialogResult.OK;
            timer.Stop();
            Close();
        }

        public void btnFlip_Click(Object sender, EventArgs e)
        {
            m_credit_mode = !m_credit_mode;
            if (m_credit_mode) {
                try {
                    btnFlip.Text = PortUtil.formatMessage(_("About {0}"), m_app_name);
                } catch (Exception ex) {
                    btnFlip.Text = "About " + m_app_name;
                }
                m_scroll_started = PortUtil.getCurrentTime();
                m_last_speed = 0f;
                m_last_t = 0f;
                m_shift = 0f;
                pictVstLogo.Visible = false;
                lblVstLogo.Visible = false;
                chkTwitterID.Visible = true;
                timer.Start();
            } else {
                timer.Stop();
                btnFlip.Text = _("Credit");
                pictVstLogo.Visible = true;
                lblVstLogo.Visible = true;
                chkTwitterID.Visible = false;
            }
            this.Refresh();
        }

        public void timer_Tick(Object sender, EventArgs e)
        {
            Invalidate();
        }

		protected override void OnDraw (Xwt.Drawing.Context ctx, Xwt.Rectangle dirtyRect)
		{
            try {
				paintCor(new Graphics() {NativeGraphics = ctx});
            } catch (Exception ex) {
#if DEBUG
                Logger.StdErr("VersionInfo_Paint; ex=" + ex);
#endif
            }
        }

        private void paintCor(Graphics g1)
        {
            Graphics g = (Graphics)g1;
            g.clipRect(0, 0, this.Width, m_height);
			g.setColor(Cadencii.Gui.Colors.White);
            g.fillRect(0, 0, this.Width, this.Height);
            //g.clearRect( 0, 0, getWidth(), getHeight() );
            if (m_credit_mode) {
                float times = (float)(PortUtil.getCurrentTime() - m_scroll_started) - 3f;
                float speed = (float)((2.0 - math.erfc(times * 0.8)) / 2.0) * m_speed;
                float dt = times - m_last_t;
                m_shift += (speed + m_last_speed) * dt / 2f;
                m_last_t = times;
                m_last_speed = speed;
                Image image = m_show_twitter_id ? m_scroll_with_id : m_scroll;
                if (image != null) {
                    float dx = (this.Width - image.Width) * 0.5f;
                    g.drawImage(image, (int)dx, (int)(90f - m_shift), null);
                    if (90f - m_shift + image.Height < 0) {
                        m_shift = -m_height * 1.5f;
                    }
                }
                int grad_height = 60;
                Rectangle top = new Rectangle(0, 0, this.Width, grad_height);
                Rectangle bottom = new Rectangle(0, m_height - grad_height, this.Width, grad_height);
                g.clipRect(0, m_height - grad_height + 1, this.Width, grad_height - 1);
                g.setClip(null);
            } else {
                g.setFont(new Font("Century Gorhic", Cadencii.Gui.Font.BOLD, FONT_SIZE * 2));
                g.setColor(m_app_name_color);
                g.drawString(m_app_name, 20, 60);
                g.setFont(new Font("Arial", 0, FONT_SIZE));
                string[] spl = PortUtil.splitString(m_version, '\n');
                int y = 100;
                int delta = (int)(FONT_SIZE * 1.1);
                if (delta == FONT_SIZE) {
                    delta++;
                }
                for (int i = 0; i < spl.Length; i++) {
                    g.drawString((i == 0 ? "version" : "") + spl[i], 25, y);
                    y += delta;
                }
            }
        }

        private void VersionInfo_KeyDown(Object sender, KeyEventArgs e)
        {
            if (((Keys) e.KeyCode & Keys.Escape) == Keys.Escape) {
				this.AsGui ().DialogResult = Cadencii.Gui.DialogResult.Cancel;
                Close();
            }
        }

        private void VersionInfo_FontChanged(Object sender, EventArgs e)
        {
			// skip
            //for (int i = 0; i < this.Controls.Count; i++) {
			//	GuiHost.Current.ApplyFontRecurse((UiControl) this.Controls[i], new Cadencii.Gui.Font(this.Font));
            //}
        }

        public void chkTwitterID_CheckedChanged(Object sender, EventArgs e)
        {
            m_show_twitter_id = chkTwitterID.Checked;
            Refresh();
        }

        private void registerEventHandlers()
        {
			this.AsGui ().KeyDown += this.VersionInfo_KeyDown;
            //this.FontChanged += this.VersionInfo_FontChanged;
            this.timer.Tick += timer_Tick;
            this.btnFlip.Click += btnFlip_Click;
            this.btnOK.Click += btnOK_Click;
            this.chkTwitterID.CheckedChanged += chkTwitterID_CheckedChanged;
        }

        private void setResources()
        {
            pictVstLogo.Image = Resources.VSTonWht;
        }

        #region ui implementation
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
			this.btnFlip = ApplicationUIHost.Create<UiButton>();
			this.btnOK = ApplicationUIHost.Create<UiButton>();
			this.lblVstLogo = ApplicationUIHost.Create<UiLabel>();
			this.pictVstLogo = ApplicationUIHost.Create<UiPictureBox>();
			this.chkTwitterID = ApplicationUIHost.Create<UiCheckBox>();
            ((System.ComponentModel.ISupportInitialize)(this.pictVstLogo)).BeginInit();
            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "VersionInfo.xml");
            this.ResumeLayout(false);

        }

        #endregion

		UiPanel mainPanel;
        UiButton btnFlip;
        UiButton btnOK;
        UiPictureBox pictVstLogo;
        UiLabel lblVstLogo;
        UiCheckBox chkTwitterID;
        #endregion
    }

}
