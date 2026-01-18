
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class ModuleController : ControllerBase
{
    private readonly IModuleService _moduleService;

    public ModuleController(IModuleService moduleService)
    {
        _moduleService = moduleService;
    }

    [HttpGet("courses/{courseId:guid}/modules")]
    public async Task<IActionResult> GetModulesByCourseId(Guid courseId, CancellationToken cancellationToken)
    {
        var modules = await _moduleService.GetModulesByCourseIdAsync(courseId, cancellationToken);
        return Ok(modules);
    }

    [HttpPost("courses/{courseId:guid}/modules")]
    public async Task<IActionResult> CreateModule(Guid courseId, [FromBody] CreateModuleDto createModuleDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var createdModule = await _moduleService.CreateModuleAsync(courseId, createModuleDto, cancellationToken);
        return CreatedAtAction(nameof(GetModulesByCourseId), new { courseId }, createdModule);
    }

    [HttpPut("modules/{id:guid}")]
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

    [HttpDelete("modules/{id:guid}")]
    public async Task<IActionResult> DeleteModule(Guid id, CancellationToken cancellationToken)
    {
        await _moduleService.DeleteModuleAsync(id, cancellationToken);
        return NoContent();
    }
}