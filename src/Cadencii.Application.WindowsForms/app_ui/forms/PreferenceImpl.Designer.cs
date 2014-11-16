
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;

namespace Cadencii.Application.Forms
{

    partial class PreferenceImpl
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected void Dispose(bool disposing)
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
			ApplicationUIHost.Instance.ApplyXml (this, "Preference.xml");
            this.ResumeLayout(false);
			btnAquesTone2.Click += new System.EventHandler (btnAquesTone2_Click);

        }

        #endregion

        private System.Windows.Forms.TabControl tabPreference;
        private System.Windows.Forms.TabPage tabSequence;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TabPage tabAnother;
        private System.Windows.Forms.TabPage tabAppearance;
        private System.Windows.Forms.Button btnChangeMenuFont;
        private System.Windows.Forms.Label labelMenu;
        private System.Windows.Forms.Label labelMenuFontName;
        private System.Windows.Forms.Label labelScreen;
        private System.Windows.Forms.Button btnChangeScreenFont;
        private System.Windows.Forms.Label labelScreenFontName;
        private System.Windows.Forms.ComboBox comboVibratoLength;
        private System.Windows.Forms.Label lblVibratoLength;
        private System.Windows.Forms.GroupBox groupVocaloidEditorCompatible;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblAutoVibratoType1;
        private System.Windows.Forms.Label lblAutoVibratoThresholdLength;
        private System.Windows.Forms.CheckBox chkEnableAutoVibrato;
        private System.Windows.Forms.ComboBox comboAutoVibratoType1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkChasePastEvent;
        private System.Windows.Forms.Label lblWait;
        private System.Windows.Forms.Label lblPreSendTime;
        private System.Windows.Forms.Label lblDefaultSinger;
        private System.Windows.Forms.ComboBox comboDefualtSinger;
        private System.Windows.Forms.Label label12;
        private NumericUpDownEx numWait;
        private NumericUpDownEx numPreSendTime;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblResolControlCurve;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblResolution;
        private System.Windows.Forms.ComboBox comboResolControlCurve;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboLanguage;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.TabPage tabPlatform;
        private System.Windows.Forms.TabPage tabOperation;
        private NumericUpDownEx numMaximumFrameRate;
        private System.Windows.Forms.Label lblMaximumFrameRate;
        private System.Windows.Forms.Label labelWheelOrder;
        private NumericUpDownEx numericUpDownEx1;
        private System.Windows.Forms.CheckBox chkScrollHorizontal;
        private System.Windows.Forms.CheckBox chkCursorFix;
        private System.Windows.Forms.CheckBox chkKeepLyricInputMode;
        private System.Windows.Forms.Label lblTrackHeight;
        private NumericUpDownEx numTrackHeight;
        private System.Windows.Forms.Label lblMouseHoverTime;
        private NumericUpDownEx numMouseHoverTime;
        private System.Windows.Forms.Label lblMilliSecond;
        private System.Windows.Forms.CheckBox chkPlayPreviewWhenRightClick;
        private System.Windows.Forms.CheckBox chkCurveSelectingQuantized;
        private System.Windows.Forms.GroupBox groupVisibleCurve;
        private System.Windows.Forms.GroupBox groupFont;
        private System.Windows.Forms.CheckBox chkBri;
        private System.Windows.Forms.CheckBox chkBre;
        private System.Windows.Forms.CheckBox chkDyn;
        private System.Windows.Forms.CheckBox chkVel;
        private System.Windows.Forms.CheckBox chkVibratoDepth;
        private System.Windows.Forms.CheckBox chkVibratoRate;
        private System.Windows.Forms.CheckBox chkDecay;
        private System.Windows.Forms.CheckBox chkAccent;
        private System.Windows.Forms.CheckBox chkPit;
        private System.Windows.Forms.CheckBox chkPor;
        private System.Windows.Forms.CheckBox chkGen;
        private System.Windows.Forms.CheckBox chkOpe;
        private System.Windows.Forms.CheckBox chkCle;
        private System.Windows.Forms.Label lblMidiInPort;
        private System.Windows.Forms.ComboBox comboMidiInPortNumber;
        private System.Windows.Forms.CheckBox chkFx2Depth;
        private System.Windows.Forms.CheckBox chkHarmonics;
        private System.Windows.Forms.CheckBox chkReso2;
        private System.Windows.Forms.CheckBox chkReso1;
        private System.Windows.Forms.CheckBox chkReso4;
        private System.Windows.Forms.CheckBox chkReso3;
        private System.Windows.Forms.TabPage tabUtausingers;
        private System.Windows.Forms.ListView listSingers;
        private System.Windows.Forms.GroupBox groupUtauCores;
        private System.Windows.Forms.Label lblResampler;
        private System.Windows.Forms.Button btnWavtool;
        private System.Windows.Forms.Label lblWavtool;
        private System.Windows.Forms.TextBox txtWavtool;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox groupPianoroll;
        private System.Windows.Forms.GroupBox groupMisc;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.CheckBox chkTranslateRoman;
        private System.Windows.Forms.CheckBox chkPbs;
        private System.Windows.Forms.CheckBox chkEnvelope;
        private System.Windows.Forms.TabPage tabFile;
        private System.Windows.Forms.CheckBox chkAutoBackup;
        private System.Windows.Forms.Label lblAutoBackupInterval;
        private NumericUpDownEx numAutoBackupInterval;
        private System.Windows.Forms.Label lblAutoBackupMinutes;
        private System.Windows.Forms.ComboBox comboAutoVibratoType2;
        private System.Windows.Forms.Label lblAutoVibratoType2;
        private System.Windows.Forms.CheckBox chkUseSpaceKeyAsMiddleButtonModifier;
        private System.Windows.Forms.ComboBox comboMtcMidiInPortNumber;
        private System.Windows.Forms.Label labelMtcMidiInPort;
        private System.Windows.Forms.CheckBox chkKeepProjectCache;
        private System.Windows.Forms.TabPage tabSingingSynth;
        private System.Windows.Forms.GroupBox groupSynthesizerDll;
        private System.Windows.Forms.CheckBox chkLoadVocaloid1;
        private System.Windows.Forms.GroupBox groupVsti;
        private System.Windows.Forms.Button btnAquesTone;
        private System.Windows.Forms.TextBox txtAquesTone;
        private System.Windows.Forms.Label lblAquesTone;
        private System.Windows.Forms.TextBox txtVOCALOID2;
        private System.Windows.Forms.TextBox txtVOCALOID1;
        private System.Windows.Forms.Label lblVOCALOID2;
        private System.Windows.Forms.Label lblVOCALOID1;
        private System.Windows.Forms.CheckBox chkLoadAquesTone;
        private System.Windows.Forms.CheckBox chkLoadVocaloid2;
        private System.Windows.Forms.Label lblBufferSize;
        private NumericUpDownEx numBuffer;
        private System.Windows.Forms.Label lblBuffer;
        private System.Windows.Forms.GroupBox groupDefaultSynthesizer;
        private System.Windows.Forms.ComboBox comboDefaultSynthesizer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupUserDefined;
        private System.Windows.Forms.Label lblAutoVibratoType;
        private System.Windows.Forms.RadioButton radioVocaloidEditorCompatible;
        private System.Windows.Forms.RadioButton radioUserDefined;
        private System.Windows.Forms.Label bLabel1;
        private NumberTextBox txtAutoVibratoThresholdLength;
        private System.Windows.Forms.ComboBox comboAutoVibratoTypeCustom;
        private System.Windows.Forms.Label lblAutoVibratoTypeCustom;
        private System.Windows.Forms.ListView listResampler;
        private System.Windows.Forms.Button buttonResamplerRemove;
        private System.Windows.Forms.Button buttonResamplerAdd;
        private System.Windows.Forms.Button buttonResamplerUp;
        private System.Windows.Forms.Button buttonResamplerDown;
        private System.Windows.Forms.Label labelWavtoolPath;
        private System.Windows.Forms.ColumnHeader columnHeaderPath;
        private System.Windows.Forms.CheckBox checkEnableWideCharacterWorkaround;
        private System.Windows.Forms.Button btnAquesTone2;
        private System.Windows.Forms.Label lblAquesTone2;
        private System.Windows.Forms.TextBox txtAquesTone2;
        private System.Windows.Forms.CheckBox chkLoadAquesTone2;

    }
}
