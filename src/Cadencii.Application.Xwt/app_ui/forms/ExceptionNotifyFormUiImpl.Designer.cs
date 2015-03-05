using Cadencii.Gui.Toolkit;


namespace Cadencii.Application.Forms
{
    partial class ExceptionNotifyFormUiImpl
    {
        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "ExceptionNotifyFormUi.xml");
			this.buttonCancel.Click += buttonCancel_Click;
			this.buttonSend.Click += buttonSend_Click;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

		#pragma warning disable 0169,0649
        UiTextBox textMessage;
        UiButton buttonCancel;
        UiButton buttonSend;
        UiLabel labelDescription;
		#pragma warning restore 0169,0649
    }
}
