using MassTransit;
using MassTransit.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus
{

    /// <summary>
    /// Event consumer definition, by default all applications will have all events emitted to be listened to.
    /// </summary>
    /// <typeparam name="EventType">The event type</typeparam>
    public class EventConsumerDefinition<EventType> : ConsumerDefinition<EventType> 
        where EventType : class, IConsumer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public EventConsumerDefinition(IConfiguration configuration) 
        {
            AppName = configuration.GetSection("App")["Name"] ?? throw new InvalidOperationException("Missing configuation App.Name");
            Endpoint(x =>
            {
                x.InstanceId = AppName;
            });
            //EndpointName = $"{AppName}-{typeof(EventType).GetGenericArguments()[0].Name}";
        }
        /// <summary>
        /// The application name
        /// </summary>
        public string AppName
        {
            get;
            private set;
        }
    }
}
