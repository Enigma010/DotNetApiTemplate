using App.Commands;
using App.Entities;

namespace AppTests.Entities
{
    public class ConfigTests
    {
        [Theory]
        [InlineData("12334")]
        [InlineData("")]
        public void SetName(string newName)
        {
            Config config = new Config();
            bool enabled = config.Enabled;
            config.Change(new ChangeConfigCmd()
            {
                Name = newName,
                Enabled = config.Enabled
            });
            Assert.Equal(newName, config.Name);
            Assert.Equal(enabled, config.Enabled);
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetEnabled(bool enabled)
        {
            Config config = new Config();
            string name = config.Name;
            config.Change(new ChangeConfigCmd()
            {
                Name = config.Name,
                Enabled = enabled
            });
            Assert.Equal(name, config.Name);
            Assert.Equal(enabled, config.Enabled);
        }
    }
}
