
using DotNetApiEventBus;

namespace App.UnitOfWork
{
    public interface IEventPublisherUnitOfWork : IUnitOfWork, IEventPublisher
    {
    }
}