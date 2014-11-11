public static class Transpose_Plus_1_Degree
{
    public static bool Edit(Cadencii.Media.Vsq.VsqFile Vsq)
    {
        for (int i = 1; i < Vsq.Track.Count; i++) {
            for (int j = 0; j < Vsq.Track[i].getEventCount(); j++) {
                Cadencii.Media.Vsq.VsqEvent item = Vsq.Track[i].getEvent(j);
                if (item.ID.type == Cadencii.Media.Vsq.VsqIDType.Anote) {
                    if (item.ID.Note < 127) {
                        item.ID.Note++;
                    }
                }
            }
        }
        return true;
    }
}