using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using InfHelper;
using InfHelper.Models;
using InfHelperTests.ModelsForTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InfHelperTests;

[TestClass()]
public class InfHelperTests
{
    private const string testFolder = "infs";
    [TestMethod()]
    public void ParseTest()
    {
        var content = File.ReadAllText(Path.Combine(testFolder, "oem100.inf"));
        var data = InfUtil.Parse(content);

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
        var data = InfUtil.Parse(content);

        // random key and key value
        Assert.AreEqual("\"$WINDOWS NT$\"", data["vErSiOn"]["SIGnatURE"].PrimitiveValue);
        // anonymous key
        Assert.AreEqual("RazerCoinstaller.dll", data["RAzEr_CoInstaller_COpyFiles"].Keys.First().PrimitiveValue);

        //anonymous key with multiple values
        var values = new HashSet<string> { "HKR", null, "CoInstallers32", "0x00010000", "RazerCoinstaller.dll,RazerCoinstaller" };
        Assert.IsTrue(data["Razer_CoInstaller_AddReg"].Keys.First().KeyValues.All(x => values.Contains(x.Value)));
    }

	[TestMethod]
	public void DuplicateCategoryParseTest()
	{
		var content = File.ReadAllText(Path.Combine(testFolder, "mergesections.inf"));
		var data = InfUtil.Parse(content);

        var versionSection = data.Categories.Where(x => x.IsNamed("Version"));

        Assert.AreEqual(1, versionSection.Count());
        Assert.AreEqual(7, versionSection.First().Keys.Count);
	}

	[TestMethod()]
    public void FileParserEndpointTest()
    {
        var sw = new Stopwatch();
        var files = Directory.GetFiles(testFolder);
        foreach (var file in files)
        {
            sw.Reset();
            Trace.WriteLine("Parsing file: " + file);
            sw.Start();
			InfUtil.ParseFile(file);
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
            "Razer_Installer_CopyFilesWithBrackets = 16428,\"Razer\\RzWizardPkg ; [Brackets=X]\"";
        var data = InfUtil.Parse(formula);
        Assert.AreEqual("11", data["DestinationDirs"]["Razer_CoInstaller_CopyFiles"].PrimitiveValue);
        Assert.AreEqual("16422, \"Razer\\RzWizardPkg\"", data["DestinationDirs"]["Razer_Installer_CopyFiles"].PrimitiveValue);
        Assert.AreEqual("16426, \"Razer\\RzWizardPkg\"", data["DestinationDirs"]["Razer_Installer_CopyFilesWOW64"].PrimitiveValue);
        Assert.AreEqual("16428, \"Razer\\RzWizardPkg ; [Brackets=X]\"", data["DestinationDirs"]["Razer_Installer_CopyFilesWithBrackets"].PrimitiveValue);
    }

    [TestMethod()]
    public void QuotedKeyParsingTest()
    {
        string formula =
            "[AzaliaManufacturerID.NTamd64.10.0...15063]\r\n\"Realtek High Definition Audio\" = IntcAzAudModel, HDAUDIO\\FUNC_01&VEN_10EC&DEV_0257&SUBSYS_17AA39F5 ; ThinkBook 16p NX ARH\r\n";
        var data = InfUtil.Parse(formula);
        Assert.AreEqual("IntcAzAudModel, HDAUDIO\\FUNC_01&VEN_10EC&DEV_0257&SUBSYS_17AA39F5", data["AzaliaManufacturerID.NTamd64.10.0...15063"]["Realtek High Definition Audio"].PrimitiveValue);
    }

    [TestMethod()]
    public void SearchMethdTest()
    {
        string formula =
            "[DestinationDirs]\r\nRazer_CoInstaller_CopyFiles = 11 ; Comment\r\nRazer_Installer_CopyFiles = 16422,\"Razer\\RzWizardPkg\"\r\nRazer_Installer_CopyFilesWOW64 = 16426,\"Razer\\RzWizardPkg\"";
        var data = InfUtil.Parse(formula);
        Assert.AreEqual("11", data.FindKeyById("Razer_CoInstaller_CopyFiles").First().PrimitiveValue);
    }

    [TestMethod()]
    public void CustomSerializationTest()
    {
        var serilized = InfUtil.SerializeFileInto<DriverInfo>(Path.Combine(testFolder, "oem100.inf"), out InfData _);
        Assert.AreEqual("HIDClass",serilized.Class);
        Assert.AreEqual("%Razer%",serilized.Provider);
        Assert.AreEqual("\"Razer Installer\"", serilized.DiskId1);
    }

    [TestMethod()]
    public void CustomSerializationTest2()
    {
        var serilized = InfUtil.SerializeFileInto<DriverInfo>(Path.Combine(testFolder, "oem147.inf"), out InfData _);
        Assert.AreEqual("net", serilized.Class);
        Assert.AreEqual("%PROVIDER_NAME%", serilized.Provider);
    }

    [TestMethod()]
    public void CustomSerializationDereferenceTest()
    {
        var serilized = InfUtil.SerializeFileInto<DriverInfoDereferenced>(Path.Combine(testFolder, "oem100.inf"), out InfData _);
        Assert.AreEqual("HIDClass", serilized.Class);
        Assert.AreEqual("Razer Inc", serilized.Provider);
    }

    [TestMethod()]
    public void CustomSerializationHugeDereferenceTest()
    {
        foreach (var file in Directory.GetFiles(testFolder))
        {
            var serilized = InfUtil.SerializeFileInto<DriverInfoDereferenced>(file, out InfData _);
            Assert.IsNotNull(serilized.Provider);
        }
    }
    
    [TestMethod()]
    public void CanParseSpacesInCategoryName()
    {
        var info = InfUtil.ParseFile(Path.Combine(testFolder, "spaces.inf"));
        
        // info.Categories should contain [OEM URLS]
        Assert.IsTrue(info.Categories.Count(x => x.Name == "OEM URLS") == 1);
    }

    [TestMethod()]
    public void AssumeNewLinesEndQuote()
    {
        var data = InfUtil.ParseFile(Path.Combine(testFolder, "oem137.inf"));

        var strings = data.Categories.FirstOrDefault(c => c.Name == "Strings");

        Assert.AreEqual("\"{DFF21BE1-F70F-11D0-B917-00A0C9223196}\"", strings.Keys.Where(k => k.Id == "KSNODETYPE_MICROPHONE").First().PrimitiveValue);

        Assert.AreEqual("\"{17CCA71B-ECD7-11D0-B908-00A0C9223196}\"", strings.Keys.Where(k => k.Id == "Proxy.CLSID").First().PrimitiveValue);
    }

    [TestMethod()]
    public void EmptySeparatorsAreNull()
    {
        var info = InfUtil.ParseFile(Path.Combine(testFolder, "oem136.inf"));

        var sourceDisksSection = info.Categories.FirstOrDefault(c => c.Name == "SourceDisksNames");

        Assert.IsTrue(sourceDisksSection.Keys.First().KeyValues.Count == 4);
        Assert.IsTrue(sourceDisksSection.Keys.First().KeyValues.Last().Value == "\\");
    }
}