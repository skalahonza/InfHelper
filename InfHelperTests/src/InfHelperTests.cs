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
        private const string TestFolder = "infs";
        
        [TestMethod()]
        public void ParseTest()
        {
            var content = File.ReadAllText(Path.Combine(TestFolder, "oem100.inf"));
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
            var content = File.ReadAllText(Path.Combine(TestFolder, "oem100.inf"));
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
            var files = Directory.GetFiles(TestFolder);
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
                "[DestinationDirs]\r\n" + 
                "Razer_CoInstaller_CopyFiles = 11\r\n" +
                "Razer_Installer_CopyFiles = 16422,\"Razer\\RzWizardPkg\"\r\n" +
                "Razer_Installer_CopyFilesWOW64 = 16426,\"Razer\\RzWizardPkg\"\r\n" +
                "commentAfterPureValue = 16427,\"value\" ; 27\"\r\n" + 
                "commentAfterValue = 16428,value ; comment\"\r\n" + 
                "valueWithBrackets = 16429,\"value ; [Brackets=X]\"";
            var helper = new InfUtil();
            var data = helper.Parse(formula);
            Assert.AreEqual("11", data["DestinationDirs"]["Razer_CoInstaller_CopyFiles"].PrimitiveValue);
            Assert.AreEqual("16422, \"Razer\\RzWizardPkg\"", data["DestinationDirs"]["Razer_Installer_CopyFiles"].PrimitiveValue);
            Assert.AreEqual("16426, \"Razer\\RzWizardPkg\"", data["DestinationDirs"]["Razer_Installer_CopyFilesWOW64"].PrimitiveValue);
            Assert.AreEqual("16427, \"value\"", data["DestinationDirs"]["commentAfterPureValue"].PrimitiveValue);
            Assert.AreEqual("16428, value", data["DestinationDirs"]["commentAfterValue"].PrimitiveValue);
            Assert.AreEqual("16429, \"value ; [Brackets=X]\"", data["DestinationDirs"]["valueWithBrackets"].PrimitiveValue);
        }

        [TestMethod()]
        public void SearchMethodTest()
        {
            string formula = 
                "[DestinationDirs]\r\n" +
                "Razer_CoInstaller_CopyFiles = 11 ; Comment\r\n" +
                "Razer_Installer_CopyFiles = 16422,\"Razer\\RzWizardPkg\"\r\n" +
                "Razer_Installer_CopyFilesWOW64 = 16426,\"Razer\\RzWizardPkg\"";
            var helper = new InfUtil();
            var data = helper.Parse(formula);
            Assert.AreEqual("11", data.FindKeyById("Razer_CoInstaller_CopyFiles").First().PrimitiveValue);
        }

        [TestMethod()]
        public void CustomSerializationTest()
        {
            var helper = new InfUtil();
            var serialized = helper.SerializeFileInto<DriverInfo>(Path.Combine(TestFolder, "oem100.inf"), out InfData data);
            Assert.AreEqual("HIDClass",serialized.Class);
            Assert.AreEqual("%Razer%",serialized.Provider);
            Assert.AreEqual("\"Razer Installer\"", serialized.DiskId1);
        }

        [TestMethod()]
        public void CustomSerializationTest2()
        {
            var helper = new InfUtil();
            var serialized = helper.SerializeFileInto<DriverInfo>(Path.Combine(TestFolder, "oem147.inf"), out InfData data);
            Assert.AreEqual("net", serialized.Class);
            Assert.AreEqual("%PROVIDER_NAME%", serialized.Provider);
        }

        [TestMethod()]
        public void CustomSerializationDereferenceTest()
        {
            var helper = new InfUtil();
            var serialized = helper.SerializeFileInto<DriverInfoDereferenced>(Path.Combine(TestFolder, "oem100.inf"), out InfData data);
            Assert.AreEqual("HIDClass", serialized.Class);
            Assert.AreEqual("Razer Inc", serialized.Provider);
        }

        [TestMethod()]
        public void CustomSerializationHugeDereferenceTest()
        {
            var helper = new InfUtil();
            foreach (var file in Directory.GetFiles(TestFolder))
            {
                var serilized = helper.SerializeFileInto<DriverInfoDereferenced>(file, out InfData data);
                Assert.IsNotNull(serilized.Provider);
            }
        }
        
        [TestMethod()]
        public void CanParseSpacesInCategoryName()
        {
            var helper = new InfUtil();
            var info = helper.ParseFile(Path.Combine(TestFolder, "spaces.inf"));
            
            // info.Categories should contain [OEM URLS]
            Assert.IsTrue(info.Categories.Count(x => x.Name == "OEM URLS") == 1);
        }
    }
}