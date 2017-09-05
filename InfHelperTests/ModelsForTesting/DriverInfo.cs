using InfHelper.Models.Attributes;

namespace InfHelperTests.ModelsForTesting
{
    public class DriverInfo
    {
        [InfKeyValue("Version","Class")]
        public string Class { get; set; }
        [InfKeyValue("Version", "Provider")]
        public string Provider { get; set; }
    }
}