using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UnitOfWork;

namespace UnitOfWork
{
    /// <summary>
    /// Registers services that need to define a unit of work block that needs to either
    /// all succeed or be rolled back.  The standard usage is the following
    /// <code>
    ///     using (var unitOfWorks = new UnitOfWorks(new IUnitOfWork[] { _repository, _eventPublisher}))
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
    public class UnitOfWorks : IDisposable
    {
        /// <summary>
        /// The registered services that define unit of work blocks
        /// </summary>
        private List<IUnitOfWork> _unitOfWorks = new List<IUnitOfWork>();
        public UnitOfWorks(IEnumerable<object> possibleUnitOfWorks)
        {
            possibleUnitOfWorks.ToList().ForEach(possibleUnitOfWorks =>
            {
                if (possibleUnitOfWorks is IUnitOfWork unitOfWork)
                {
                    _unitOfWorks.Add(unitOfWork);
                    unitOfWork.Begin();
                }
            });
        }
        /// <summary>
        /// Runs an action, representing the application logic, if successful commits
        /// the changes, otherwise it rolls them back
        /// </summary>
        /// <param name="action">The application logic</param>
        /// <returns></returns>
        public async Task Run(Action action)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    action();
                    await Commit();
                    scope.Complete();
                }
                catch (Exception)
                {
                    await Rollback();
                    throw;
                }
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
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var returnValue = await func();
                    await Commit();
                    scope.Complete();
                    return returnValue;
                }
                catch (Exception)
                {
                    await Rollback();
                    throw;
                }
            }
        }
        /// <summary>
        /// Runs an async function.  If successful commits the changes, otherwise rolls it back.
        /// </summary>
        /// <param name="func">The application function</param>
        /// <returns></returns>
        public async Task RunAsync(Func<Task> func)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await func();
                    await Commit();
                }
                catch (Exception)
                {
                    await Rollback();
                    throw;
                }
            }
        }
        /// <summary>
        /// Commits all unit of works
        /// </summary>
        /// <returns></returns>
        public async Task Commit()
        {
            foreach (var unitOfWork in _unitOfWorks)
            {
                await unitOfWork.Commit();
            }
        }
        /// <summary>
        /// Rolls back all unit of works
        /// </summary>
        /// <returns></returns>
        private async Task Rollback()
        {
            foreach (var unitOfWork in _unitOfWorks)
            {
                await unitOfWork.Rollback();
            }
        }
        /// <summary>
        /// Called during dispose
        /// </summary>
        public async void Dispose()
        {
            await Rollback();
        }
    }
}
