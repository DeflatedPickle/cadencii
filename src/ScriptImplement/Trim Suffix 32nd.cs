public static class Trim_Suffix_32nd
{
    public static bool Edit(Cadencii.Media.Vsq.VsqFile Vsq)
    {
        for (int i = 1; i < Vsq.Track.Count; i++) {
            for (int j = 0; j < Vsq.Track[i].getEventCount(); j++) {
                Cadencii.Media.Vsq.VsqEvent item = Vsq.Track[i].getEvent(j);
                if (item.ID.type == Cadencii.Media.Vsq.VsqIDType.Anote) {
                    // 32分音符の長さは，クロック数に直すと60クロック
                    if (item.ID.Length > 60) {
                        item.ID.Length -= 60;
                    }
                }
            }
        }
        return true;
    }
}