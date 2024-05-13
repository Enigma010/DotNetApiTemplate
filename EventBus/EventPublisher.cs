using MassTransit;
using MassTransit.Middleware;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetEventBus
{
    public interface IEventPublisher
    {
        Task Publish(IEnumerable<object> events, CancellationToken stoppingToken);
    }
    public class EventPublisher : IEventPublisher
    {
        /// <summary>
        /// The event busx
        /// </summary>
        private readonly IBus _bus;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<IEventPublisher> _logger;
        /// <summary>
        /// Creates a new event publisher
        /// </summary>
        /// <param name="bus">The event bus</param>
        /// <param name="logger">The logger</param>
        public EventPublisher(IBus bus, ILogger<IEventPublisher> logger)
        {
            _bus = bus;
            _logger = logger;
        }
        /// <summary>
        /// Publishes events to the event bus
        /// </summary>
        /// <param name="events">The events</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if the events are null</exception>
        public async Task Publish(IEnumerable<object> events, CancellationToken cancellationToken)
        {
            if (events == null)
            {
                throw new ArgumentNullException("The event cannot be null");
            }
            await _bus.PublishBatch(events, cancellationToken);
        }
    }
}
