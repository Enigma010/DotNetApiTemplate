using App.ActionModels;
using App.Entities;
using App.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigsController : ControllerBase
    {
        private readonly IConfigService _service;
        public ConfigsController(IConfigService service) 
        { 
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]string name)
        {
            Config config = await _service.CreateAsync(name);
            return Ok(config);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            return Ok(await _service.GetAsync(id));
        }
        [HttpGet()]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _service.GetAsync());
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] Guid id, 
            [FromBody] ConfigChangeActionModel change)
        {
            Config config = await _service.ChangeAsync(id, change);
            return Ok(config);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
