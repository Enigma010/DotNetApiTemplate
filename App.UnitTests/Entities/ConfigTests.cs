using App.Commands;
using App.Entities;
using App.Events;

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
            bool enabled = config.Enabled;
            config.Rename(new ConfigRenameCmd()
            {
                NewName = newName
            });
            AssertConfigCreatedRenamedEvents(config);
            Assert.Equal(newName, config.Name);
            Assert.Equal(enabled, config.Enabled);
            IReadOnlyCollection<object> stateChanges = config.GetEvents();
            AssertConfigCreatedEventFromConstructor(stateChanges.ElementAt(0) as ConfigCreatedEvent);
            ConfigRenamedEvent? configRenamedEvent = stateChanges.ElementAt(1) as ConfigRenamedEvent;
            Assert.NotNull(configRenamedEvent);
            Assert.Equal(newName, configRenamedEvent.NewName);
            Assert.Equal(oldName, configRenamedEvent.OldName);
        }
        [Theory]
        [InlineData(true)]
        public void Enablement(bool enabled)
        {
            Config config = new Config();
            AssertConfigCreated(config);
            config.Enablement(new ConfigEnablementCmd()
            {
                Enabled = enabled
            });
            AssertConfigCreatedEnablementEvents(config);
            IReadOnlyCollection<object> stateChanges = config.GetEvents();
            AssertConfigCreatedEventFromConstructor(stateChanges.ElementAt(0) as ConfigCreatedEvent);
            ConfigEnablementEvent? configEnablementEvent = stateChanges.ElementAt(1) as ConfigEnablementEvent;
            Assert.NotNull(configEnablementEvent);
            Assert.Equal(enabled, configEnablementEvent.NewEnabled);

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
        private void AssertConfigCreatedEnablementEvents(Config config)
        {
            Assert.Collection(
                config.GetEvents(),
                AssertType<ConfigCreatedEvent>(),
                AssertType<ConfigEnablementEvent>());
        }

        private void AssertConfigCreatedEventFromConstructor(ConfigCreatedEvent? configCreatedEvent)
        {
            Assert.NotNull(configCreatedEvent);
            Assert.Equal(string.Empty, configCreatedEvent.Name);
            Assert.False(configCreatedEvent.Enabled);
        }
    }
}
