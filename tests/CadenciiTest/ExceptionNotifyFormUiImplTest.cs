using System;
using NUnit.Framework;
using cadencii;
using Cadencii.Application.Forms;

namespace cadencii
{
    [TestFixture]
    public class ExceptionNotifyFormUiImplTest : ExceptionNotifyFormUiImpl
    {
        public ExceptionNotifyFormUiImplTest()
            : base(new ExceptionNotifyFormController (c => new ExceptionNotifyFormUiImpl (c)))
        {
        }

        [Test]
        public void testSetTitle()
        {
            string expected = "たいとる";
            this.setTitle(expected);
            Assert.AreEqual(expected, base.Text);
        }
    }
}
