﻿using MassTransit;
using MassTransit.Transactions;
using Microsoft.Extensions.Logging;
using System.Transactions;
using UnitOfWork;

namespace DotNetEventBus
{
    public interface IEventPublisher : IUnitOfWork
    {
        Task Publish(IEnumerable<object> events);
    }
    public class EventPublisher : IEventPublisher
    {
        /// <summary>
        /// The event busx
        /// </summary>
        private readonly IBusControl _bus;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<IEventPublisher> _logger;
        /// <summary>
        /// The transaction bus
        /// </summary>
        private TransactionalEnlistmentBus? _transactionBus;
        /// <summary>
        /// The transaction
        /// </summary>
        private TransactionScope? _transaction;
        /// <summary>
        /// Creates a new event publisher
        /// </summary>
        /// <param name="bus">The event bus</param>
        /// <param name="logger">The logger</param>
        public EventPublisher(IBusControl bus, ILogger<IEventPublisher> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        /// <summary>
        /// Begins a transaction
        /// </summary>
        /// <returns></returns>
        public async Task Begin()
        {
            if (_transactionBus == null)
            {
                await _bus.StartAsync();
                _transactionBus = new TransactionalEnlistmentBus(_bus);
            }
            if(_transaction == null)
            {
                _transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            }
        }

        /// <summary>
        /// Commits a transaction
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown if commit is invoked and begin wasn't invoked</exception>
        public async Task Commit()
        {
            if (_transaction == null || _transactionBus == null)
            {
                throw new InvalidOperationException("Cannot call Commit without first calling Begin");
            }
            _transaction.Complete();
            await Task.CompletedTask;
            return;
        }

        /// <summary>
        /// Rollbacks a unit of work
        /// </summary>
        /// <returns></returns>
        public async Task Rollback()
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Publishes events to the event bus
        /// </summary>
        /// <param name="events">The events</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if the events are null</exception>
        public async Task Publish(IEnumerable<object> events)
        {
            if (events == null)
            {
                throw new ArgumentNullException("The event cannot be null");
            }
            await _bus.PublishBatch(events);
        }
    }
}
