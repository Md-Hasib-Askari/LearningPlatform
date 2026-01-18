using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("courses")]
[Authorize(Roles = "Admin,Staff,Instructor")]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly IModuleService _moduleService;

    public CourseController(ICourseService courseService, IModuleService moduleService)
    {
        _courseService = courseService;
        _moduleService = moduleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        var courses = await _courseService.GetAllAsync();
        return Ok(courses);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCourseById(Guid id)
    {
        var course = await _courseService.GetByIdAsync(id);
        if (course == null)
        {
            return NotFound();
        }
        return Ok(course);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto createCourseDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdCourse = await _courseService.CreateAsync(createCourseDto);
        return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.Id }, createdCourse);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] UpdateCourseDto updateCourseDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedCourse = await _courseService.UpdateAsync(id, updateCourseDto);
        if (updatedCourse == null)
        {
            return NotFound();
        }

        return Ok(updatedCourse);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCourse(Guid id)
    {
        var deleted = await _courseService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("{courseId:guid}/modules")]
    public async Task<IActionResult> GetModulesByCourseId(Guid courseId, CancellationToken cancellationToken)
    {
        var modules = await _moduleService.GetModulesByCourseIdAsync(courseId, cancellationToken);
        return Ok(modules);
    }

    [HttpPost("{courseId:guid}/modules")]
    public async Task<IActionResult> CreateModule(Guid courseId, [FromBody] CreateModuleDto createModuleDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdModule = await _moduleService.CreateModuleAsync(createModuleDto, cancellationToken);
        return CreatedAtAction(nameof(GetModulesByCourseId), new { courseId }, createdModule);
    }
}