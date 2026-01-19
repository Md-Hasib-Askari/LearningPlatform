public interface IEnrollmentService
{
    Task<Enrollment?> GetByUserIdAndCourseIdAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Enrollment>> GetEnrollmentsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseIdAsync(Guid courseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<EnrolledStudentDto>> GetStudentsEnrolledInCourseAsync(Guid courseId, CancellationToken cancellationToken = default);
    Task<Enrollment> EnrollUserAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default);
    Task<bool> UnenrollUserAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default);
}