using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InfHelper = InfHelper.InfHelper;

namespace InfHelperTests
{
    [TestClass()]
    public class InfHelperTests
    {
        private const string testFolder = "..\\..\\infs";
        [TestMethod()]
        public void ParseTest()
        {
            var content = File.ReadAllText(Path.Combine(testFolder, "oem100.inf"));
            var helper = new global::InfHelper.InfHelper();
            var data = helper.Parse(content);

            // random key and key value
            Assert.AreEqual(data["Version"]["Signature"].PureTextValue, "$WINDOWS NT$");
            // anonymous key
            Assert.AreEqual(data["Razer_CoInstaller_CopyFiles"].Keys.First().PureTextValue, "RazerCoinstaller.dll");

            //anonymous key with multiple values
            var values = new HashSet<string>{"HKR", null, "CoInstallers32", "0x00010000", "RazerCoinstaller.dll,RazerCoinstaller"};
            Assert.IsTrue(data["Razer_CoInstaller_AddReg"].Keys.First().KeyValues.All(x => values.Contains(x.Value)));
        }
    }
}