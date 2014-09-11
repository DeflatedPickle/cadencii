using System;
using VstSdk;

namespace cadencii.dsp.winforms
{
	public class UIHostWF : UIHost
	{
		public UIHostWF (VSTiDriverBase vsti)
		{
			this.vsti = vsti;
		}

		VSTiDriverBase vsti;

		/// <summary>
		/// プラグインのUI
		/// </summary>
		FormPluginUi ui = null;

		public override void ClosePluginUI ()
		{
			if (ui != null && !ui.IsDisposed) {
				ui.Close ();
			}
		}

		public override object GetPluginUI (object nativeWindow)
		{
			return getUi ((System.Windows.Forms.Form)nativeWindow);
		}

		public FormPluginUi getUi (System.Windows.Forms.Form main_window)
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
				ui = new FormPluginUi ();
			}
			if (!ui.IsOpened) {
				// Editorを持っているかどうかを確認
				if ((aEffect.aeffect.flags & VstAEffectFlags.effFlagsHasEditor) == VstAEffectFlags.effFlagsHasEditor) {
					try {
						// プラグインの名前を取得
						string product = vsti.getStringCore (AEffectXOpcodes.effGetProductString, 0, VstStringConstants.kVstMaxProductStrLen);
						ui.Text = product;
						ui.Location = new System.Drawing.Point (0, 0);
						aEffect.Dispatch (AEffectOpcodes.effEditOpen, 0, 0, ui.Handle, 0.0f);
						//Thread.Sleep( 250 );
						ui.UpdatePluginUiRect ();
						var rect = ui.WindowRect;
						ui.ClientSize = new System.Drawing.Size (rect.width, rect.height);
						ui.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
						ui.IsOpened = true;
					} catch (Exception ex) {
						serr.println ("vstidrv#open; ex=" + ex);
						ui.IsOpened = false;
					}
				}
			}
			return;
		}
	}
}

