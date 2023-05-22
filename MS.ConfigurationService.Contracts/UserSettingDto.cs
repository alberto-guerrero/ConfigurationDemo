using System.Dynamic;

namespace MS.ConfigurationService.Contracts
{
    public class UserSettingDto
    {
        public string UserId { get; set; } = string.Empty;
        public string SettingKey { get; set; } = string.Empty;
        public ExpandoObject SettingValue { get; set; } = new ExpandoObject();
    }
}