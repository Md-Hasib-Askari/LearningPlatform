using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("courses")]
[Authorize(Roles = "Admin,Staff,Instructor")]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly ILogger<CourseController> _logger;

    public CourseController(ICourseService courseService, ILogger<CourseController> logger)
    {
        _courseService = courseService;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCourses(CancellationToken cancellationToken)
    {
        var courses = await _courseService.GetAllAsync(cancellationToken);
        return Ok(courses);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCourseById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching course with ID: {CourseId}", id);
        var course = await _courseService.GetByIdAsync(id, cancellationToken);
        if (course == null)
        {
            return NotFound();
        }
        return Ok(course);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto createCourseDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdCourse = await _courseService.CreateAsync(createCourseDto, cancellationToken);
        return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.Id }, createdCourse);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] UpdateCourseDto updateCourseDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedCourse = await _courseService.UpdateAsync(id, updateCourseDto, cancellationToken);
        if (updatedCourse == null)
        {
            return NotFound();
        }

        return Ok(updatedCourse);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCourse(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _courseService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}