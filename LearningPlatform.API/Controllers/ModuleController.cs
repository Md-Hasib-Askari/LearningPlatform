
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("modules")]
public class ModuleController : ControllerBase
{
    private readonly IModuleService _moduleService;

    public ModuleController(IModuleService moduleService)
    {
        _moduleService = moduleService;
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateModule(Guid id, [FromBody] UpdateModuleDto updateModuleDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedModule = await _moduleService.UpdateModuleAsync(id, updateModuleDto, cancellationToken);
        if (updatedModule == null)
        {
            return NotFound();
        }

        return Ok(updatedModule);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteModule(Guid id, CancellationToken cancellationToken)
    {
        await _moduleService.DeleteModuleAsync(id, cancellationToken);
        return NoContent();
    }
}