using cadencii;
using Cadencii.Media.Vsq;
using cadencii.java.util;
using cadencii.apputil;
using cadencii.utau;
using Cadencii.Application;
using Cadencii.Application.Media;
using Cadencii.Application.Scripts;

public class ImportOtoIniConfig
{

    /// <summary>
    /// スクリプトの本体
    /// </summary>
    /// <param name="vsq"></param>
    /// <returns></returns>
    public static ScriptReturnStatus Edit(VsqFileEx vsq)
    {
        int selected = EditorManager.Selected;
        VsqTrack vsq_track = vsq.Track[selected];
        RendererKind kind = VsqFileEx.getTrackRendererKind(vsq_track);
        if (kind != RendererKind.UTAU) {
            return ScriptReturnStatus.NOT_EDITED;
        }
        bool edited = false;
        foreach (var item in EditorManager.itemSelection.getEventIterator()) {
            VsqEvent original = item.original;
            if (original.ID.type != VsqIDType.Anote) {
                continue;
            }
            VsqEvent singer = vsq_track.getSingerEventAt(original.Clock);
            SingerConfig sc = MusicManager.getSingerInfoUtau(singer.ID.IconHandle.Language, singer.ID.IconHandle.Program);
            if (sc != null && UtauWaveGenerator.mUtauVoiceDB.ContainsKey(sc.VOICEIDSTR)) {
                string phrase = original.ID.LyricHandle.L0.Phrase;
                UtauVoiceDB db = UtauWaveGenerator.mUtauVoiceDB[sc.VOICEIDSTR];
                OtoArgs oa = db.attachFileNameFromLyric(phrase, original.ID.Note);
                VsqEvent editing = vsq_track.findEventFromID(original.InternalID);
                if (editing.UstEvent == null) {
                    editing.UstEvent = new UstEvent();
                }
                editing.UstEvent.setVoiceOverlap(oa.msOverlap);
                editing.UstEvent.setPreUtterance(oa.msPreUtterance);
                edited = true;
            }
        }

        return edited ? ScriptReturnStatus.EDITED : ScriptReturnStatus.NOT_EDITED;
    }

    /// <summary>
    /// メニューに表示されるプラグイン名を取得します
    /// </summary>
    /// <returns></returns>
    public static string GetDisplayName()
    {
        string lang = Messaging.getLanguage();
        if (lang == "ja") {
            return "先行発音とオーバーラップをoto.iniからコピー";
        } else {
            return "Copy pre-utterance & overlap from oto.ini";
        }
    }

}
