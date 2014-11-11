public static class Print_Lyric
{
    public static bool Edit(Cadencii.Media.Vsq.VsqFile Vsq)
    {
        System.IO.StreamWriter sw = null;
        try {
            sw = new System.IO.StreamWriter(@"c:\lyrics.txt");
            for (int i = 0; i < Vsq.Track[1].getEventCount(); i++) {
                Cadencii.Media.Vsq.VsqEvent item = Vsq.Track[1].getEvent(i);
                if (item.ID.type == Cadencii.Media.Vsq.VsqIDType.Anote) {
                    int clStart = item.Clock;
                    int clEnd = clStart + item.ID.Length;
                    double secStart = Vsq.getSecFromClock(clStart);
                    double secEnd = Vsq.getSecFromClock(clEnd);
                    sw.WriteLine(secStart + "\t" + secEnd + "\t" + item.ID.LyricHandle.L0.Phrase + "\t" + item.ID.LyricHandle.L0.getPhoneticSymbol());
                }
            }
        } catch {
            return false;
        } finally {
            if (sw != null) {
                sw.Close();
            }
        }
        return true;
    }
}