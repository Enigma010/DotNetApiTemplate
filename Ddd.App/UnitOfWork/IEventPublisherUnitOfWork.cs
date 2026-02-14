
using DotNetApiEventBus;

namespace Ddd.App.UnitOfWork
{
    public interface IEventPublisherUnitOfWork : IUnitOfWork, IEventPublisher
    {
    }
}