namespace Cadencii.Application.Forms
{
    partial class ExceptionNotifyFormUiImpl
    {
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
			ApplicationUIHost.Instance.ApplyXml (this, "ExceptionNotifyFormUi.xml");
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        protected System.Windows.Forms.TextBox textMessage;
        protected System.Windows.Forms.Button buttonCancel;
        protected System.Windows.Forms.Button buttonSend;
        protected System.Windows.Forms.Label labelDescription;

    }
}
