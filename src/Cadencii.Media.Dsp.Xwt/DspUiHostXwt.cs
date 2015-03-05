using System;
using Cadencii.Gui.Toolkit;
using Cadencii.Utilities;
using VstSdk;

namespace cadencii.dsp.xwt
{
	public class DspUIHostXwt : DspUIHost
	{
		public DspUIHostXwt (VSTiDriverBase vsti)
		{
			this.vsti = vsti;
		}

		VSTiDriverBase vsti;

		/// <summary>
		/// プラグインのUI
		/// </summary>
		PluginUIXwt ui = null;

		public override void ClosePluginUI ()
		{
			if (ui != null && !ui.IsDisposed) {
				ui.UI.Close ();
			}
		}

		public override PluginUI GetPluginUI (UiForm window)
		{
			return getUi ((Xwt.Window) window.Native);
		}

		public PluginUI getUi (Xwt.Window main_window)
		{
			if (ui == null) {
				if (main_window != null) {
					// mainWindowのスレッドで、uiが作成されるようにする
					main_window.Invoke (new Action (createPluginUi));
				}
			}
			return ui;
		}

		private void createPluginUi ()
		{
			var aEffect = vsti.aEffect;
			bool hasUi = (aEffect.aeffect.flags & VstAEffectFlags.effFlagsHasEditor) == VstAEffectFlags.effFlagsHasEditor;
			if (!hasUi) {
				return;
			}
			if (ui == null) {
				ui = new PluginUIXwt ();
			}
			if (!ui.IsOpened) {
				// Editorを持っているかどうかを確認
				if ((aEffect.aeffect.flags & VstAEffectFlags.effFlagsHasEditor) == VstAEffectFlags.effFlagsHasEditor) {
					try {
						// プラグインの名前を取得
						string product = vsti.getStringCore (AEffectXOpcodes.effGetProductString, 0, VstStringConstants.kVstMaxProductStrLen);
						ui.UI.Title = product;
						ui.UI.Location = new Xwt.Point (0, 0);
						aEffect.Dispatch (AEffectOpcodes.effEditOpen, 0, 0, ui.UI.Handle, 0.0f);
						//Thread.Sleep( 250 );
						ui.UI.UpdatePluginUiRect ();
						var rect = ui.UI.WindowRect;
						ui.UI.Size = new Xwt.Size (rect.Width, rect.Height);
						//ui.UI.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
						ui.IsOpened = true;
					} catch (Exception ex) {
						Logger.StdErr ("vstidrv#open; ex=" + ex);
						ui.IsOpened = false;
					}
				}
			}
			return;
		}
	}
}

