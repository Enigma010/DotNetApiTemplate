using AppEvents;
using DotNetEventBus;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace App.Events.Consumers
{
    /// <summary>
    /// The create config consumer example, this functionally doesn't do anything and is just used to show how to 
    /// consume messages from the message bus.  Note that I'm not sure if this should really run in the App that's
    /// running in the API probject or if it should be in another project
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CreateConfigConsumer : EventConsumer<ConfigCreated>
    {
        /// <summary>
        /// Creates a new consumer
        /// </summary>
        /// <param name="logger"></param>
        public CreateConfigConsumer(ILogger<EventConsumer<ConfigCreated>> logger) : base(logger)
        {
        }
        /// <summary>
        /// Consumes the create configuration event and responds to it, in this case just logging
        /// it to the screen
        /// </summary>
        /// <param name="event">The create configuration event</param>
        /// <returns></returns>
        public override async Task Consume(ConfigCreated @event)
        {
            _logger.LogInformation($"Received {nameof(ConfigCreated)} Id: {@event.Id} Name: {@event.Name}");
            await Task.CompletedTask;
        }
    }
}
