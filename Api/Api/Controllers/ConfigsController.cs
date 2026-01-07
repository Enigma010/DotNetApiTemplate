using App.Commands;
using App.Db;
using App.Entities;
using App.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json", "application/problem+json")]
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
        public async Task<IActionResult> PostAsync([FromBody] ConfigCreateCmd cmd)
        {
            Config config = await _service.CreateAsync(cmd);
            return Ok(config);
        }

        /// <summary>
        /// HTTP GET to getsa specific configuration by ID
        /// </summary>
        /// <param name="id">The ID of the configuration</param>
        /// <returns>The configuration</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Config), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            return await this.GetToActionResultsAsync<Guid, Config>(() => _service.GetAsync(id));
        }

        /// <summary>
        /// HTTP GET to return all the configurations
        /// </summary>
        /// <returns>The configurations</returns>
        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<Config>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromQuery] Paging paging)
        {
            return await this.GetToActionResultsAsync<Guid, IEnumerable<Config>>(() => _service.GetAsync(paging));
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
        public async Task<IActionResult> NameAsync([FromRoute] Guid id,
            [FromBody] ConfigRenameCmd cmd)
        {
            return await this.PostToActionResultsAsync<Guid, Config>(() => _service.RenameAsync(id, cmd));
        }

        /// <summary>
        /// HTTP PUT to update a specific configuration
        /// </summary>
        /// <param name="id">The configuration ID</param>
        /// <param name="cmd">The change command</param>
        /// <returns></returns>
        [HttpPost("{id}/enablement")]
        [ProducesResponseType(typeof(Config), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EnablementAsync([FromRoute] Guid id,
            [FromBody] ConfigEnablementCmd cmd)
        {
            return await this.PostToActionResultsAsync<Guid, Config>(() => _service.EnablementAsync(id, cmd));
        }

        /// <summary>
        /// HTTP DELETE to delete a specific configuration by ID
        /// </summary>
        /// <param name="id">The ID of the configuration</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            return await this.DeleteToActionResultsAsync<Guid>(() => _service.DeleteAsync(id));
        }
    }
}
