using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using cadencii.apputil;
using Cadencii.Media.Vsq;

using cadencii;
using Cadencii.Application.Forms;
using Cadencii.Application;

public class GenerateVLF
{
    public static bool Edit(VsqFile vsq)
    {
        int track = 1;
        InputBox ib = ApplicationUIHost.Create<InputBox>("Input target track index");
        ib.Result = (track.ToString());
        if (ib.ShowDialog() != Cadencii.Gui.DialogResult.OK) {
            return false;
        }
        if (!int.TryParse(ib.Result, out track)) {
            MessageBox.Show("integer parse error");
            return false;
        }
        if (track <= 0 || vsq.Track.Count <= track) {
            MessageBox.Show("invalid target track");
            return false;
        }
        using (SaveFileDialog sfd = new SaveFileDialog()) {
            if (sfd.ShowDialog() != DialogResult.OK) {
                return false;
            }
            using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.GetEncoding(932))) {
                sw.WriteLine("vlf\t2.0");
                sw.WriteLine("vlfpart\tPhrase1\t0\t0");
                for (int i = 0; i < vsq.Track[track].getEventCount(); i++) {
                    VsqEvent ve = vsq.Track[track].getEvent(i);
                    if (ve.ID.type == VsqIDType.Anote) {
                        string symbol = "";
                        for (int j = 0; j < ve.ID.LyricHandle.L0.getPhoneticSymbolList().Count; j++) {
                            symbol += (" " + ve.ID.LyricHandle.L0.getPhoneticSymbolList()[j]);
                        }
                        symbol = symbol.Trim();
                        sw.WriteLine(ve.ID.LyricHandle.L0.Phrase + "\t" + symbol + "\t0");
                    }
                }
            }
        }
        return true;
    }
}
