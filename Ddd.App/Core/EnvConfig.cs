namespace Ddd.App.Core
{
    public class EnvConfig
    {
        public const string EnvConfigSectionName = nameof(EnvConfig);
        public string Path { get; set; }  = string.Empty;
    }
}
