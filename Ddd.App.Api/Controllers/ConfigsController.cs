using Ddd.App.Commands;
using Ddd.App.Db;
using Ddd.App.Entities;
using Ddd.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController()]
    [Route("config")]
    [Produces("application/json", "application/problem+json")]
    [Authorize()]
    public class ConfigsController : ControllerBase
    {
        /// <summary>
        /// The service for working with configurations
        /// </summary>
        private readonly IConfigService _service;

        /// <summary>
        /// Creates a new configuraiton controller
        /// </summary>
        /// <param name="service">The configuraiton service</param>
        public ConfigsController(IConfigService service)
        {
            _service = service;
        }

        /// <summary>
        /// HTTP POST request to create a configuration
        /// </summary>
        /// <param name="cmd">The create command</param>
        /// <returns>The configuration</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Config), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PostAsync([FromBody] ConfigCreateCmd cmd)
        {
            try
            {
                Config config = await _service.CreateAsync(cmd);
                return Ok(config);
            }
            catch (DbEntityMultipleSingletonsException<Config>)
            {
                return Conflict();
            }
        }

        /// <summary>
        /// HTTP GET to getsa specific configuration by ID
        /// </summary>
        /// <param name="id">The ID of the configuration</param>
        /// <returns>The configuration</returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(Config), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAsync()
        {
            return await this.GetToActionResultsAsync<Guid, Config>(() => _service.GetAsync());
        }

        /// <summary>
        /// HTTP PUT to update a specific configuration
        /// </summary>
        /// <param name="id">The configuration ID</param>
        /// <param name="cmd">The change command</param>
        /// <returns></returns>
        [HttpPost("{id}/name")]
        [ProducesResponseType(typeof(Config), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> NameAsync([FromRoute] Guid id,
            [FromBody] ConfigRenameCmd cmd)
        {
            return await this.PostToActionResultsAsync<Guid, Config>(() => _service.RenameAsync(id, cmd));
        }

        /// <summary>
        /// HTTP DELETE to delete a specific configuration by ID
        /// </summary>
        /// <param name="id">The ID of the configuration</param>
        /// <returns></returns>
        [HttpDelete("")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteAsync()
        {
            return await this.DeleteToActionResultsAsync<Guid>(() => _service.DeleteAsync());
        }
    }
}
