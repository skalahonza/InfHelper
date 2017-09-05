using InfHelper.Models.Attributes;

namespace InfHelperTests.ModelsForTesting
{
    public class DriverInfoDereferenced
    {
        [InfKeyValue("Version", "Class", true)]
        public string Class { get; set; }
        [InfKeyValue("Version", "Provider", true)]
        public string Provider { get; set; }
    }
}