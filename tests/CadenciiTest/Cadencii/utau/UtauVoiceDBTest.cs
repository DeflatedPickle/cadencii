using NUnit.Framework;
using cadencii;
using Cadencii.Media.Vsq;
using cadencii.utau;

namespace cadencii.test.utau
{
    [TestFixture]
    class UtauVoiceDBTest
    {
        [Test]
        public void test()
        {
            var config = new SingerConfig();
            config.VOICEIDSTR = "./fixture/utau_voice_db";
            var db = new UtauVoiceDB(config);
            {
                var actual = db.attachFileNameFromLyric("��", 60);
                Assert.AreEqual("��", actual.Alias);
                Assert.AreEqual("��.wav", actual.fileName);
                Assert.AreEqual(6f, actual.msOffset);
                Assert.AreEqual(52f, actual.msConsonant);
                Assert.AreEqual(69f, actual.msBlank);
                Assert.AreEqual(1f, actual.msPreUtterance);
                Assert.AreEqual(2f, actual.msOverlap);
            }
            {
                var actual = db.attachFileNameFromLyric("��", 61);
                Assert.AreEqual("�큪", actual.Alias);
                Assert.AreEqual(@"A\�큪.wav", actual.fileName);
            }
        }
    }
}
