using NUnit.Framework;
using cadencii;
using Cadencii.Media.Vsq;
using cadencii.utau;

namespace cadencii.test.utau
{
    [TestFixture]
    class PrefixMapTest
    {
        [Test]
        public void test()
        {
            var prefixmap = new PrefixMap("./fixture/utau_voice_db/prefix.map");
            var actual = prefixmap.getMappedLyric("は", 61);
            Assert.AreEqual(@"A\は↑", actual);
            Assert.AreEqual("あ", prefixmap.getMappedLyric("あ", 60));
        }
    }
}
