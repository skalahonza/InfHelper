using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using InfHelper;
using InfHelper.Models;
using InfHelperTests.ModelsForTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var helper = new InfUtil();
            var data = helper.Parse(content);

            // random key and key value
            Assert.AreEqual("\"$WINDOWS NT$\"", data["Version"]["Signature"].PrimitiveValue);
            // anonymous key
            Assert.AreEqual("RazerCoinstaller.dll", data["Razer_CoInstaller_CopyFiles"].Keys.First().PrimitiveValue);

            //anonymous key with multiple values
            var values = new HashSet<string> { "HKR", null, "CoInstallers32", "0x00010000", "RazerCoinstaller.dll,RazerCoinstaller" };
            Assert.IsTrue(data["Razer_CoInstaller_AddReg"].Keys.First().KeyValues.All(x => values.Contains(x.Value)));
        }

        [TestMethod()]
        public void ParseCaseInsensitiveTest()
        {
            var content = File.ReadAllText(Path.Combine(testFolder, "oem100.inf"));
            var helper = new InfUtil();
            var data = helper.Parse(content);

            // random key and key value
            Assert.AreEqual("\"$WINDOWS NT$\"", data["vErSiOn"]["SIGnatURE"].PrimitiveValue);
            // anonymous key
            Assert.AreEqual("RazerCoinstaller.dll", data["RAzEr_CoInstaller_COpyFiles"].Keys.First().PrimitiveValue);

            //anonymous key with multiple values
            var values = new HashSet<string> { "HKR", null, "CoInstallers32", "0x00010000", "RazerCoinstaller.dll,RazerCoinstaller" };
            Assert.IsTrue(data["Razer_CoInstaller_AddReg"].Keys.First().KeyValues.All(x => values.Contains(x.Value)));
        }

        [TestMethod()]
        public void FileParserEndpointTest()
        {
            var sw = new Stopwatch();
            var helper = new InfUtil();
            var files = Directory.GetFiles(testFolder);
            foreach (var file in files)
            {
                sw.Reset();
                Trace.WriteLine("Parsing file: " + file);
                sw.Start();
                helper.ParseFile(file);
                sw.Stop();
                Trace.WriteLine($"Completed. Elapsed time: {sw.Elapsed}");
            }
        }

        [TestMethod()]
        public void PureValueParsingTest()
        {
            string formula =
                "[DestinationDirs]\r\nRazer_CoInstaller_CopyFiles = 11\r\nRazer_Installer_CopyFiles = 16422,\"Razer\\RzWizardPkg\"\r\nRazer_Installer_CopyFilesWOW64 = 16426,\"Razer\\RzWizardPkg\"";
            var helper = new InfUtil();
            var data = helper.Parse(formula);
            Assert.AreEqual("11", data["DestinationDirs"]["Razer_CoInstaller_CopyFiles"].PrimitiveValue);
            Assert.AreEqual("16422, \"Razer\\RzWizardPkg\"", data["DestinationDirs"]["Razer_Installer_CopyFiles"].PrimitiveValue);
            Assert.AreEqual("16426, \"Razer\\RzWizardPkg\"", data["DestinationDirs"]["Razer_Installer_CopyFilesWOW64"].PrimitiveValue);
        }

        [TestMethod()]
        public void SearchMethdTest()
        {
            string formula =
                "[DestinationDirs]\r\nRazer_CoInstaller_CopyFiles = 11\r\nRazer_Installer_CopyFiles = 16422,\"Razer\\RzWizardPkg\"\r\nRazer_Installer_CopyFilesWOW64 = 16426,\"Razer\\RzWizardPkg\"";
            var helper = new InfUtil();
            var data = helper.Parse(formula);
            Assert.AreEqual("11", data.FindKeyById("Razer_CoInstaller_CopyFiles").First().PrimitiveValue);
        }

        [TestMethod()]
        public void CustomSerializationTest()
        {
            var helper = new InfUtil();
            InfData data;
            var serilized = helper.SerializeFileInto<DriverInfo>(Path.Combine(testFolder, "oem100.inf"), out data);
            Assert.AreEqual("HIDClass",serilized.Class);
            Assert.AreEqual("%Razer%",serilized.Provider);
        }
    }
}