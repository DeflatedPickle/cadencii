using cadencii;
using Cadencii.Media.Vsq;
using Cadencii.Application;
using Cadencii.Application.Media;
using Cadencii.Application.Scripts;

public class RunOnce
{
    private static int runCount = 0;

    public static ScriptReturnStatus Edit(VsqFileEx vsq)
    {
        if (runCount != 0) {
            return ScriptReturnStatus.NOT_EDITED;
        }
        runCount++;

        // 以下に，起動時に変更するパラメータを記述する

        // ピアノロールに合成システムの名称をオーバーレイ表示するかどうか
        EditorManager.drawOverSynthNameOnPianoroll = true;
        // 再生中に，WAVE波形の描画をスキップするかどうか
        EditorManager.skipDrawingWaveformWhenPlaying = true;
        // 起動時のツール．デフォルトはEditTool.PENCIL
        EditorManager.SelectedTool = (EditTool.PENCIL);
        // 音符の長さを変えたとき，ビブラート長さがどう影響を受けるかを決める．
		EditorManager.vibratoLengthEditingRule = VibratoLengthEditingRule.PERCENTAGE;

        return ScriptReturnStatus.NOT_EDITED;
    }
}
