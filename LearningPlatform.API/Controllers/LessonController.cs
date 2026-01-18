using Microsoft.AspNetCore.Mvc;

[ApiController]
public class LessonController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpGet("modules/{moduleId:guid}/lessons")]
    public async Task<IActionResult> GetLessonsByModuleId(Guid moduleId, CancellationToken cancellationToken)
    {
        var lessons = await _lessonService.GetLessonsByModuleIdAsync(moduleId, cancellationToken);
        return Ok(lessons);
    }

    [HttpPost("modules/{moduleId:guid}/lessons")]
    public async Task<IActionResult> CreateLesson(Guid moduleId, [FromBody] CreateLessonDto createLessonDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        createLessonDto.ModuleId = moduleId;
        var createdLesson = await _lessonService.CreateLessonAsync(createLessonDto, cancellationToken);
        return CreatedAtAction(nameof(GetLessonsByModuleId), new { moduleId }, createdLesson);
    }

    [HttpPut("lessons/{id:guid}")]
    public async Task<IActionResult> UpdateLesson(Guid id, [FromBody] UpdateLessonDto updateLessonDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedLesson = await _lessonService.UpdateLessonAsync(id, updateLessonDto, cancellationToken);
        if (updatedLesson == null)
        {
            return NotFound();
        }

        return Ok(updatedLesson);
    }

    [HttpDelete("lessons/{id:guid}")]
    public async Task<IActionResult> DeleteLesson(Guid id, CancellationToken cancellationToken)
    {
        await _lessonService.DeleteLessonAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("lessons/{id:guid}/complete")]
    public async Task<IActionResult> MarkLessonAsComplete(Guid id, CancellationToken cancellationToken)
    {
        await _lessonService.MarkLessonAsCompleteAsync(id, cancellationToken);
        return NoContent();
    }
}