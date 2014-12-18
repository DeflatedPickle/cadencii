
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

		#pragma warning disable 0649
        UiTabControl tabPreference;
        UiTabPage tabSequence;
        UiButton btnCancel;
        UiButton btnOK;
        UiTabPage tabAnother;
        UiTabPage tabAppearance;
        UiButton btnChangeMenuFont;
        UiLabel labelMenu;
        UiLabel labelMenuFontName;
        UiLabel labelScreen;
        UiButton btnChangeScreenFont;
        UiLabel labelScreenFontName;
        UiComboBox comboVibratoLength;
        UiLabel lblVibratoLength;
        UiGroupBox groupVocaloidEditorCompatible;
        UiLabel label3;
        UiLabel lblAutoVibratoType1;
        UiLabel lblAutoVibratoThresholdLength;
        UiCheckBox chkEnableAutoVibrato;
        UiComboBox comboAutoVibratoType1;
        UiLabel label7;
        UiCheckBox chkChasePastEvent;
        UiLabel lblWait;
        UiLabel lblPreSendTime;
        UiLabel lblDefaultSinger;
        UiComboBox comboDefualtSinger;
        UiLabel label12;
        NumericUpDownEx numWait;
        NumericUpDownEx numPreSendTime;
        UiLabel label13;
        UiLabel lblResolControlCurve;
        UiLabel label1;
        UiLabel lblResolution;
        UiComboBox comboResolControlCurve;
        UiLabel label2;
        UiComboBox comboLanguage;
        UiLabel lblLanguage;
        UiTabPage tabPlatform;
        UiTabPage tabOperation;
        NumericUpDownEx numMaximumFrameRate;
        UiLabel lblMaximumFrameRate;
        UiLabel labelWheelOrder;
        NumericUpDownEx numericUpDownEx1;
        UiCheckBox chkScrollHorizontal;
        UiCheckBox chkCursorFix;
        UiCheckBox chkKeepLyricInputMode;
        UiLabel lblTrackHeight;
        NumericUpDownEx numTrackHeight;
        UiLabel lblMouseHoverTime;
        NumericUpDownEx numMouseHoverTime;
        UiLabel lblMilliSecond;
        UiCheckBox chkPlayPreviewWhenRightClick;
        UiCheckBox chkCurveSelectingQuantized;
        UiGroupBox groupVisibleCurve;
        UiGroupBox groupFont;
        UiCheckBox chkBri;
        UiCheckBox chkBre;
        UiCheckBox chkDyn;
        UiCheckBox chkVel;
        UiCheckBox chkVibratoDepth;
        UiCheckBox chkVibratoRate;
        UiCheckBox chkDecay;
        UiCheckBox chkAccent;
        UiCheckBox chkPit;
        UiCheckBox chkPor;
        UiCheckBox chkGen;
        UiCheckBox chkOpe;
        UiCheckBox chkCle;
        UiLabel lblMidiInPort;
        UiComboBox comboMidiInPortNumber;
        UiCheckBox chkFx2Depth;
        UiCheckBox chkHarmonics;
        UiCheckBox chkReso2;
        UiCheckBox chkReso1;
        UiCheckBox chkReso4;
        UiCheckBox chkReso3;
        UiTabPage tabUtausingers;
        UiListView listSingers;
        UiGroupBox groupUtauCores;
        UiLabel lblResampler;
        UiButton btnWavtool;
        UiLabel lblWavtool;
        UiTextBox txtWavtool;
        UiButton btnUp;
        UiButton btnDown;
        UiButton btnAdd;
        UiGroupBox groupPianoroll;
        UiGroupBox groupMisc;
        UiButton btnRemove;
        UiCheckBox chkTranslateRoman;
        UiCheckBox chkPbs;
        UiCheckBox chkEnvelope;
        UiTabPage tabFile;
        UiCheckBox chkAutoBackup;
        UiLabel lblAutoBackupInterval;
        NumericUpDownEx numAutoBackupInterval;
        UiLabel lblAutoBackupMinutes;
        UiComboBox comboAutoVibratoType2;
        UiLabel lblAutoVibratoType2;
        UiCheckBox chkUseSpaceKeyAsMiddleButtonModifier;
        UiComboBox comboMtcMidiInPortNumber;
        UiLabel labelMtcMidiInPort;
        UiCheckBox chkKeepProjectCache;
        UiTabPage tabSingingSynth;
        UiGroupBox groupSynthesizerDll;
        UiCheckBox chkLoadVocaloid1;
        UiGroupBox groupVsti;
        UiButton btnAquesTone;
        UiTextBox txtAquesTone;
        UiLabel lblAquesTone;
        UiTextBox txtVOCALOID2;
        UiTextBox txtVOCALOID1;
        UiLabel lblVOCALOID2;
        UiLabel lblVOCALOID1;
        UiCheckBox chkLoadAquesTone;
        UiCheckBox chkLoadVocaloid2;
        UiLabel lblBufferSize;
        NumericUpDownEx numBuffer;
        UiLabel lblBuffer;
        UiGroupBox groupDefaultSynthesizer;
        UiComboBox comboDefaultSynthesizer;
        UiLabel label6;
        UiGroupBox groupUserDefined;
        UiLabel lblAutoVibratoType;
        UiRadioButton radioVocaloidEditorCompatible;
        UiRadioButton radioUserDefined;
        UiLabel bLabel1;
        NumberTextBox txtAutoVibratoThresholdLength;
        UiComboBox comboAutoVibratoTypeCustom;
        UiLabel lblAutoVibratoTypeCustom;
        UiListView listResampler;
        UiButton buttonResamplerRemove;
        UiButton buttonResamplerAdd;
        UiButton buttonResamplerUp;
        UiButton buttonResamplerDown;
        UiLabel labelWavtoolPath;
        UiListViewColumn columnHeaderPath;
        UiCheckBox checkEnableWideCharacterWorkaround;
        UiButton btnAquesTone2;
        UiLabel lblAquesTone2;
        UiTextBox txtAquesTone2;
        UiCheckBox chkLoadAquesTone2;
		#pragma warning restore 0649
    }
}
