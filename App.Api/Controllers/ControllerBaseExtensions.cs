using App.Db;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public static class ControllerBaseExtensions
    {
        /// <summary>
        /// Executes the specified asynchronous operation and returns its result as an HTTP response.
        /// </summary>
        /// <typeparam name="IdType">The type of the identifier associated with the resource being accessed.</typeparam>
        /// <typeparam name="ReturnType">The type of the result returned by the asynchronous operation.</typeparam>
        /// <param name="controller">The controller instance on which this extension method is called. Cannot be null.</param>
        /// <param name="func">A function that represents the asynchronous operation to execute. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IActionResult"/>
        /// representing the HTTP response generated from the operation's result.</returns>
        public static async Task<IActionResult> GetToActionResultsAsync<IdType, ReturnType>(this ControllerBase controller,
            Func<Task<ReturnType>> func)
        {
            return await controller.ToActionResultsAsync<IdType, ReturnType>(func);
        }
        /// <summary>
        /// Executes the specified asynchronous operation and returns an HTTP 200 OK response with the result, or an
        /// appropriate error response if the operation fails.
        /// </summary>
        /// <remarks>This method is intended for use in ASP.NET Core controllers to simplify the
        /// implementation of HTTP PUT endpoints. It standardizes response handling, including error propagation and
        /// result formatting.</remarks>
        /// <typeparam name="IdType">The type of the resource identifier associated with the operation.</typeparam>
        /// <typeparam name="ReturnType">The type of the value returned by the asynchronous operation.</typeparam>
        /// <param name="controller">The controller instance on which this extension method is called. Cannot be null.</param>
        /// <param name="func">A function that performs the asynchronous operation and returns a result to include in the response. Cannot
        /// be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IActionResult"/>
        /// representing the HTTP response.</returns>
        public static async Task<IActionResult> PostToActionResultsAsync<IdType, ReturnType>(this ControllerBase controller,
            Func<Task<ReturnType>> func)
        {
            return await controller.ToActionResultsAsync<IdType, ReturnType>(func);
        }

        /// <summary>
        /// Executes an asynchronous delete operation and returns an appropriate HTTP response.
        /// </summary>
        /// <typeparam name="IdType">The type of the identifier used for the resource to be deleted.</typeparam>
        /// <param name="controller">The controller instance on which this extension method is called. Cannot be null.</param>
        /// <param name="func">A delegate that performs the asynchronous delete operation. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IActionResult"/>
        /// indicating the outcome of the delete operation.</returns>
        public static async Task<IActionResult> DeleteToActionResultsAsync<IdType>(this ControllerBase controller,
            Func<Task> func)
        {
            return await controller.ToActionResultsAsync<IdType>(func);
        }
        /// <summary>
        /// Executes the specified asynchronous operation and returns an HTTP 200 OK response with the result, or an
        /// HTTP 404 Not Found response if the entity is not found.
        /// </summary>
        /// <remarks>If the operation throws a <see cref="DbEntityNotFoundException{IdType}"/>, the method
        /// returns a NotFound response. Otherwise, the result of the operation is returned in an Ok response. This
        /// method simplifies common controller patterns for entity retrieval.</remarks>
        /// <typeparam name="IdType">The type of the entity identifier used to detect not found exceptions.</typeparam>
        /// <param name="controller">The controller instance used to generate HTTP responses.</param>
        /// <param name="func">An asynchronous function that performs the operation and returns the result object.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IActionResult"/>
        /// representing either an HTTP 200 OK response with the operation result, or an HTTP 404 Not Found response if
        /// the entity is not found.</returns>
        public static async Task<IActionResult> ToActionResultsAsync<IdType, ReturnType>(this ControllerBase controller,
            Func<Task<ReturnType>> func)
        {
            try
            {
                return controller.Ok(await func());
            }
            catch (DbEntityNotFoundException<IdType>)
            {
                return controller.NotFound();
            }
        }

        /// <summary>
        /// Executes the specified asynchronous operation and returns a standardized HTTP response based on the outcome.
        /// </summary>
        /// <remarks>If the asynchronous operation throws a <see
        /// cref="DbEntityNotFoundException{IdType}"/>, the method returns a 404 Not Found response. Otherwise, a 204 No
        /// Content response is returned upon successful completion. This method simplifies error handling for
        /// controller actions that may fail due to missing entities.</remarks>
        /// <typeparam name="IdType">The type of the entity identifier used when handling not found exceptions.</typeparam>
        /// <param name="controller">The controller instance used to generate HTTP responses.</param>
        /// <param name="func">A delegate representing the asynchronous operation to execute. Must not be null.</param>
        /// <returns>A <see cref="Task{IActionResult}"/> that resolves to a <see cref="NoContentResult"/> if the operation
        /// completes successfully, or a <see cref="NotFoundResult"/> if the entity is not found.</returns>
        public static async Task<IActionResult> ToActionResultsAsync<IdType>(this ControllerBase controller,
            Func<Task> func)
        {
            try
            {
                await func();
                return controller.NoContent();
            }
            catch (DbEntityNotFoundException<IdType>)
            {
                return controller.NotFound();
            }
        }
    }
}
