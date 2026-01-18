public interface ILessonService
{
    Task<LessonDto> CreateLessonAsync(CreateLessonDto createLessonDto, CancellationToken cancellationToken);
    Task<LessonDto> GetLessonByIdAsync(Guid lessonId, CancellationToken cancellationToken);
    Task<IEnumerable<LessonDto>> GetLessonsByModuleIdAsync(Guid moduleId, CancellationToken cancellationToken);
    Task<LessonDto> UpdateLessonAsync(Guid lessonId, UpdateLessonDto updateLessonDto, CancellationToken cancellationToken);
    Task DeleteLessonAsync(Guid lessonId, CancellationToken cancellationToken);
    Task MarkLessonAsCompleteAsync(Guid lessonId, CancellationToken cancellationToken);
}