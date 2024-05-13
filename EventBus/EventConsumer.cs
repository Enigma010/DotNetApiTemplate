using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetEventBus
{
    public interface IEventConsumer<EventType> where EventType : class
    {
        Task Consume(EventType @event);
    }

    /// <summary>
    /// An abstract class for event consumers
    /// </summary>
    /// <typeparam name="EventType">The event type or contract for the event</typeparam>
    public abstract class EventConsumer<EventType> : 
        IConsumer<EventType>, 
        IEventConsumer<EventType> where EventType : class
    {
        /// <summary>
        /// The logger
        /// </summary>
        protected readonly ILogger _logger;
        /// <summary>
        /// Createa new event consumer
        /// </summary>
        /// <param name="logger"></param>
        public EventConsumer(ILogger<EventConsumer<EventType>> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// The mass transit entry point for consuming events
        /// </summary>
        /// <param name="context">The consumer context</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<EventType> context)
        {
            await Consume(context.Message);
        }
        /// <summary>
        /// The entry point for consuming events, implement this and
        /// add application logic to handle the event
        /// </summary>
        /// <param name="event">The event</param>
        /// <returns></returns>
        public abstract Task Consume(EventType @event);
    }
}
