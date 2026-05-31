using Ddd.App.Commands;
using Ddd.App.Entities;
using Ddd.App.Events;

namespace AppTests.Entities
{
    public class ConfigTests
    {
        [Theory]
        [InlineData("12334")]
        [InlineData("abc")]
        public void Rename(string newName)
        {
            Config config = new Config();
            string oldName = config.Name;
            AssertConfigCreated(config);
            config.Rename(new ConfigRenameCmd()
            {
                NewName = newName
            });
            AssertConfigCreatedRenamedEvents(config);
            Assert.Equal(newName, config.Name);
            IReadOnlyCollection<object> stateChanges = config.GetEvents();
            AssertConfigCreatedEventFromConstructor(stateChanges.ElementAt(0) as ConfigCreatedEvent);
            ConfigRenamedEvent? configRenamedEvent = stateChanges.ElementAt(1) as ConfigRenamedEvent;
            Assert.NotNull(configRenamedEvent);
            Assert.Equal(newName, configRenamedEvent.NewName);
            Assert.Equal(oldName, configRenamedEvent.OldName);
        }
        public static Action<object> AssertType<AssertType>()
        {
            Action<object> assert = (sc) =>
            {
                Assert.IsType<AssertType>(sc);
            };
            return assert;
        }
        private void AssertConfigCreated(Config config)
        {
            Assert.Collection(config.GetEvents(),
                AssertType<ConfigCreatedEvent>());
        }
        private void AssertConfigCreatedRenamedEvents(Config config)
        {
            Assert.Collection(
                config.GetEvents(),
                AssertType<ConfigCreatedEvent>(),
                AssertType<ConfigRenamedEvent>());
        }

        private void AssertConfigCreatedEventFromConstructor(ConfigCreatedEvent? configCreatedEvent)
        {
            Assert.NotNull(configCreatedEvent);
            Assert.Equal(string.Empty, configCreatedEvent.Name);
            Assert.False(configCreatedEvent.Enabled);
        }
    }
}
