using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize(Roles = "Admin,Staff,Instructor,Student")]
public class EnrollmentController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;
    private readonly ILogger<EnrollmentController> _logger;

    public EnrollmentController(IEnrollmentService enrollmentService, ILogger<EnrollmentController> logger)
    {
        _enrollmentService = enrollmentService;
        _logger = logger;
    }

    [HttpPost("courses/{courseId:guid}/enroll")]
    public async Task<IActionResult> EnrollInCourse(Guid courseId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Unauthorized();
        }

        _logger.LogInformation("User {UserId} enrolling in course {CourseId}", userIdGuid, courseId);
        var enrollment = await _enrollmentService.EnrollUserAsync(userIdGuid, courseId, cancellationToken);
        if (enrollment == null)
        {
            return BadRequest("Enrollment failed.");
        }

        return Ok(enrollment);
    }

    [HttpDelete("courses/{courseId:guid}/unenroll")]
    public async Task<IActionResult> UnenrollFromCourse(Guid courseId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Unauthorized();
        }

        _logger.LogInformation("User {UserId} unenrolling from course {CourseId}", userIdGuid, courseId);
        var unenrollment = await _enrollmentService.UnenrollUserAsync(userIdGuid, courseId, cancellationToken);
        if (!unenrollment)
        {
            return BadRequest("Unenrollment failed.");
        }

        return NoContent();
    }

    [HttpGet("enrollments/my-enrollments")]
    public async Task<IActionResult> GetUserEnrollments(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var userIdGuid))
        {
            return Unauthorized();
        }

        var enrollments = await _enrollmentService.GetEnrollmentsByUserIdAsync(userIdGuid, cancellationToken);
        return Ok(enrollments);
    }

    [HttpGet("courses/{courseId:guid}/students")]
    public async Task<IActionResult> GetEnrolledStudents(Guid courseId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching students enrolled in course {CourseId}", courseId);
        var students = await _enrollmentService.GetStudentsEnrolledInCourseAsync(courseId, cancellationToken);
        return Ok(students);
    }
}