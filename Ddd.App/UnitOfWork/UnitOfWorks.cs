using DotNetApiLogging;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;

namespace Ddd.App.UnitOfWork
{
    /// <summary>
    /// Registers services that need to define a unit of work block that needs to either
    /// all succeed or be rolled back.  The standard usage is the following
    /// <code>
    ///     await using (var unitOfWorks = new UnitOfWorks(new IUnitOfWork[] { _repository, _eventPublisher}, logger))
    ///     {
    ///         return await unitOfWorks.RunAsync(async () =>
    ///         {
    ///             // Do application logic here
    ///         });
    ///     }
    /// </code>
    /// Note that the example above uses RunAsync but you could also be using Run.
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public class UnitOfWorks : IAsyncDisposable, IDisposable
    {
        private readonly ILogger _logger;
        /// <summary>
        /// The registered services that define unit of work blocks
        /// </summary>
        private readonly List<IUnitOfWork> _unitOfWorks = new List<IUnitOfWork>();
        private bool _disposed;
        private bool _completed; // true when commit succeeded for this instance

        public UnitOfWorks(IEnumerable<IUnitOfWork> unitOfWorks, ILogger logger)
        {
            _logger = logger;
            _logger.LogInformationCaller($"Creating {nameof(UnitOfWorks)}");

            if (unitOfWorks is null)
            {
                return;
            }

            foreach (var unitOfWork in unitOfWorks)
            {
                var unitOfWorkType = unitOfWork.GetType().Name;
                _logger.LogInformation("Adding unit of work {UnitOfWorkType}", unitOfWorkType);
                _unitOfWorks.Add(unitOfWork);
            }
        }

        /// <summary>
        /// Runs an action, representing the application logic, if successful commits
        /// the changes, otherwise it rolls them back
        /// </summary>
        /// <param name="action">The application logic</param>
        /// <returns></returns>
        public async Task Run(Action action)
        {
            _logger.LogInformationCaller($"Run {nameof(UnitOfWorks)} with action {action.ToString()}");
            try
            {
                BeginAll(false);
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        BeginAll(true); // Begin inside the ambient TransactionScope so resource enlistment can occur
                        _logger.LogInformation("Running action");
                        action();
                        _logger.LogInformation("Ran action");

                        await CommitAll(true);
                        _completed = true;
                        _logger.LogInformation("Committed");

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Exception encountered");
                        _logger.LogInformation("Rolling back");
                        try
                        {
                            await RollbackAll(true);
                        }
                        catch (Exception rbEx)
                        {
                            _logger.LogError(rbEx, "Exception encountered during rollback");
                            // Do not swallow the original exception; aggregate both for visibility
                            throw new AggregateException(ex, rbEx);
                        }

                        _logger.LogInformation("Rolled back");
                        throw;
                    }
                }
                await CommitAll(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception encountered");
                try
                {
                    await RollbackAll(false);
                }
                catch (Exception rbEx)
                {
                    _logger.LogError(rbEx, "Exception encountered during rollback");
                    throw new AggregateException(ex, rbEx);
                }

                throw;
            }
        }

        /// <summary>
        /// Runs an async function.  If successful commits the changes, otherwise rolls it back,
        /// and returns the value
        /// </summary>
        /// <typeparam name="RunReturnType">The object type to return</typeparam>
        /// <param name="func">The application function</param>
        /// <returns>The application return object</returns>
        public async Task<RunReturnType> RunAsync<RunReturnType>(Func<Task<RunReturnType>> func)
        {
            _logger.LogInformationCaller($"RunAsync {nameof(UnitOfWorks)} with func {func.ToString()}");
            try
            {
                BeginAll(false);
                RunReturnType returnValue;
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        BeginAll(true);
                        _logger.LogInformation("Running function");
                        returnValue = await func();
                        _logger.LogInformation("Ran function");

                        await CommitAll(true);
                        _completed = true;
                        _logger.LogInformation("Committed");

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Exception encountered");
                        try
                        {
                            await RollbackAll(true);
                        }
                        catch (Exception rbEx)
                        {
                            _logger.LogError(rbEx, "Exception encountered during rollback");
                            throw new AggregateException(ex, rbEx);
                        }

                        throw;
                    }
                }
                await CommitAll(false);
                return returnValue;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception encountered");
                try
                {
                    await RollbackAll(false);
                }
                catch (Exception rbEx)
                {
                    _logger.LogError(rbEx, "Exception encountered during rollback");
                    throw new AggregateException(ex, rbEx);
                }

                throw;
            }
        }

        /// <summary>
        /// Runs an async function.  If successful commits the changes, otherwise rolls it back.
        /// </summary>
        /// <param name="func">The application function</param>
        /// <returns></returns>
        public async Task RunAsync(Func<Task> func)
        {
            _logger.LogInformationCaller($"RunAsync {nameof(UnitOfWorks)} with {func.ToString()}");
            try
            {
                BeginAll(false);
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        BeginAll(true);
                        _logger.LogInformation("Running function");
                        await func();
                        _logger.LogInformation("Ran function");

                        await CommitAll(true);
                        _completed = true;
                        _logger.LogInformation("Committed");

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Exception encountered");
                        try
                        {
                            await RollbackAll(true);
                        }
                        catch (Exception rbEx)
                        {
                            _logger.LogError(rbEx, "Exception encountered during rollback");
                            throw new AggregateException(ex, rbEx);
                        }

                        throw;
                    }
                }
                await CommitAll(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception encountered");
                try
                {
                    await RollbackAll(false);
                }
                catch (Exception rbEx)
                {
                    _logger.LogError(rbEx, "Exception encountered during rollback");
                    throw new AggregateException(ex, rbEx);
                }

                throw;
            }
        }

        /// <summary>
        /// Calls Begin() on all registered unit of work implementations.
        /// Begin is intentionally executed while a TransactionScope is active so
        /// implementations can enlist in the ambient transaction.
        /// </summary>
        private void BeginAll(bool useScopedTransactions)
        {
            foreach (var unitOfWork in _unitOfWorks.Where(unitOfWork => unitOfWork.UseScopedTransactions == useScopedTransactions))
            {
                _logger.LogInformation("Beginning unit of work {UnitOfWorkType}", unitOfWork.GetType().Name);
                unitOfWork.Begin();
                _logger.LogInformation("Began unit of work {UnitOfWorkType}", unitOfWork.GetType().Name);
            }
        }

        /// <summary>
        /// Commits all unit of works
        /// </summary>
        /// <returns></returns>
        private async Task CommitAll(bool useScopedTransactions)
        {
            _logger.LogInformationCaller($"Commit {nameof(UnitOfWorks)}");
            foreach (var unitOfWork in _unitOfWorks.Where(unitOfWork => unitOfWork.UseScopedTransactions == useScopedTransactions))
            {
                _logger.LogInformation("Committing {UnitOfWorkType}", unitOfWork.GetType().Name);
                await unitOfWork.Commit();
                _logger.LogInformation("Committed {UnitOfWorkType}", unitOfWork.GetType().Name);
            }
        }

        /// <summary>
        /// Rolls back all unit of works
        /// </summary>
        /// <returns></returns>
        private async Task RollbackAll(bool useScopedTransactions)
        {
            _logger.LogInformationCaller($"Rollback {nameof(UnitOfWorks)}");
            // Attempt rollback on every unit; log exceptions but continue to try others
            List<Exception> exceptions = new List<Exception>();
            foreach (var unitOfWork in _unitOfWorks.Where(unitOfWork => unitOfWork.UseScopedTransactions == useScopedTransactions))
            {
                try
                {
                    _logger.LogInformation("Rolling back {UnitOfWorkType}", unitOfWork.GetType().Name);
                    await unitOfWork.Rollback();
                    _logger.LogInformation("Rolled back {UnitOfWorkType}", unitOfWork.GetType().Name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Rollback failed for {UnitOfWorkType}", unitOfWork.GetType().Name);
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException("One or more rollbacks failed", exceptions);
            }
        }

        /// <summary>
        /// IAsyncDisposable implementation. Use this when consumers can await disposal.
        /// We keep disposal non-throwing: if the instance was not completed (committed) we do a best-effort rollback,
        /// but swallow/log rollback errors to avoid masking application exceptions during cleanup.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            if (_completed)
            {
                return;
            }

            try
            {
                await RollbackAll(false);
            }
            catch (Exception ex)
            {
                // During disposal we must not throw. Log for diagnostics and continue.
                _logger.LogError(ex, "Exception encountered during DisposeAsync rollback (suppressed)");
            }
        }

        /// <summary>
        /// IDisposable implementation for consumers that can't await disposal.
        /// It synchronously waits for rollback to complete but does not rethrow disposal errors.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            // Prefer the async disposal path; block here to support synchronous callers.
            try
            {
                DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                // Shouldn't happen because DisposeAsync suppresses errors, but be defensive.
                _logger.LogError(ex, "Unexpected exception during Dispose");
            }
        }
    }
}
