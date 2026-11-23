using DotNetApiEventBus;
using System.Diagnostics.CodeAnalysis;

namespace Ddd.App.UnitOfWork
{
    [ExcludeFromCodeCoverage(Justification = "This class is a simple implementation of the IEventPublisherUnitOfWork interface and does not contain any complex logic that requires unit testing.")]
    public class EventPublisherUnitOfWork : IEventPublisherUnitOfWork
    {
        private readonly IEventPublisher _eventPublisher;
        public EventPublisherUnitOfWork(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }
        public bool UseScopedTransactions => false;

        public async Task Begin()
        {
            EventPublisher eventPublish = _eventPublisher as EventPublisher
                ?? throw new InvalidOperationException("Event publisher is not of correct type");
            await eventPublish.Begin();
        }

        public async Task Commit()
        {
            EventPublisher eventPublish = _eventPublisher as EventPublisher
                ?? throw new InvalidOperationException("Event publisher is not of correct type");
            await eventPublish.Commit();
        }

        public async Task Publish(IEnumerable<object> events)
        {
            await _eventPublisher.Publish(events);
        }

        public async Task Rollback()
        {
            EventPublisher eventPublish = _eventPublisher as EventPublisher
                ?? throw new InvalidOperationException("Event publisher is not of correct type");
            await eventPublish.Rollback();
        }
    }
}
