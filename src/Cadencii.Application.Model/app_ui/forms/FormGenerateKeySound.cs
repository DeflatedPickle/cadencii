using System;
using Cadencii.Media.Vsq;
using cadencii.core;
using System.IO;
using Cadencii.Media;
using Cadencii.Utilities;
using Cadencii.Gui.Toolkit;
using cadencii;

namespace Cadencii.Application.Forms
{
	public class PrepareStartArgument
        {
            public string singer = "Miku";
            public double amplitude = 1.0;
            public string directory = "";
            public bool replace = true;
        }

	public interface FormGenerateKeySound : UiForm
	{
	}

	public static class FormGenerateKeySoundStatic
	{
		
        #region public static methods
        public static void GenerateSinglePhone(int note, string singer, string file, double amp)
        {
            string renderer = "";
            SingerConfig[] singers1 = VocaloSysUtil.getSingerConfigs(SynthesizerType.VOCALOID1);
            int c = singers1.Length;
            string first_found_singer = "";
            string first_found_renderer = "";
            for (int i = 0; i < c; i++) {
                if (first_found_singer.Equals("")) {
                    first_found_singer = singers1[i].VOICENAME;
                    first_found_renderer = VsqFileEx.RENDERER_DSB2;
                }
                if (singers1[i].VOICENAME.Equals(singer)) {
                    renderer = VsqFileEx.RENDERER_DSB2;
                    break;
                }
            }

            SingerConfig[] singers2 = VocaloSysUtil.getSingerConfigs(SynthesizerType.VOCALOID2);
            c = singers2.Length;
            for (int i = 0; i < c; i++) {
                if (first_found_singer.Equals("")) {
                    first_found_singer = singers2[i].VOICENAME;
                    first_found_renderer = VsqFileEx.RENDERER_DSB3;
                }
                if (singers2[i].VOICENAME.Equals(singer)) {
                    renderer = VsqFileEx.RENDERER_DSB3;
                    break;
                }
            }

            foreach (var sc in ApplicationGlobal.appConfig.UtauSingers) {
                if (first_found_singer.Equals("")) {
                    first_found_singer = sc.VOICENAME;
                    first_found_renderer = VsqFileEx.RENDERER_UTU0;
                }
                if (sc.VOICENAME.Equals(singer)) {
                    renderer = VsqFileEx.RENDERER_UTU0;
                    break;
                }
            }

            VsqFileEx vsq = new VsqFileEx(singer, 1, 4, 4, 500000);
            if (renderer.Equals("")) {
                singer = first_found_singer;
                renderer = first_found_renderer;
            }
            vsq.Track[1].getCommon().Version = renderer;
            VsqEvent item = new VsqEvent(1920, new VsqID(0));
            item.ID.LyricHandle = new LyricHandle("あ", "a");
            item.ID.setLength(480);
            item.ID.Note = note;
            item.ID.VibratoHandle = null;
            item.ID.type = VsqIDType.Anote;
            vsq.Track[1].addEvent(item);
            vsq.updateTotalClocks();
            int ms_presend = 500;
            string tempdir = Path.Combine(ApplicationGlobal.getCadenciiTempDir(), ApplicationGlobal.getID());
            if (!Directory.Exists(tempdir)) {
                try {
                    PortUtil.createDirectory(tempdir);
                } catch (Exception ex) {
                    Logger.write(typeof(FormGenerateKeySound) + ".GenerateSinglePhone; ex=" + ex + "\n");
                    Logger.StdErr("Program#GenerateSinglePhone; ex=" + ex);
                    return;
                }
            }
            WaveWriter ww = null;
            try {
                ww = new WaveWriter(file);
                RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[1]);
                WaveGenerator generator = VSTiDllManager.getWaveGenerator(kind);
                int sample_rate = vsq.config.SamplingRate;
                FileWaveReceiver receiver = new FileWaveReceiver(file, 1, 16, sample_rate);
                generator.setReceiver(receiver);
				generator.setGlobalConfig(ApplicationGlobal.appConfig);
#if DEBUG
                Logger.StdOut("FormGenerateKeySound#GenerateSinglePhone; sample_rate=" + sample_rate);
#endif
                generator.init(vsq, 1, 0, vsq.TotalClocks, sample_rate);
                double total_sec = vsq.getSecFromClock(vsq.TotalClocks) + 1.0;
                WorkerStateImp state = new WorkerStateImp();
                generator.begin((long)(total_sec * sample_rate), state);
            } catch (Exception ex) {
                Logger.StdErr("FormGenerateKeySound#GenerateSinglePhone; ex=" + ex);
                Logger.write(typeof(FormGenerateKeySound) + ".GenerateSinglePhone; ex=" + ex + "\n");
            } finally {
                if (ww != null) {
                    try {
                        ww.close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormGenerateKeySound) + ".GenerateSinglePhone; ex=" + ex2 + "\n");
                        Logger.StdErr("FormGenerateKeySound#GenerateSinglePhone; ex2=" + ex2);
                    }
                }
            }
        }
        #endregion
	}
}

