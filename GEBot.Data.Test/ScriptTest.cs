using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GEBot.Data;

namespace GEBot.Data.Test
{
    [TestFixture]
    public class ScriptTest
    {
        /// <summary>
        /// проверяем метод ScriptDataBotDB и поле Login
        /// </summary>
        [Test]
        public void DbBotsNewLoginTest()
        {
            IScriptDataBot script = new ScriptDataBotDB(2);
            Assert.AreEqual(script.GetDataBot().Login, "dag002");

        }

        /// <summary>
        /// проверяем метод ScriptDataBotDB и поле nameOfFamily
        /// </summary>
        [Test]
        public void DbBotsNewFamilyTest()
        {
            IScriptDataBot script = new ScriptDataBotDB(3);
            Assert.AreEqual(script.GetDataBot().nameOfFamily, "Red");

        }


    }
}
