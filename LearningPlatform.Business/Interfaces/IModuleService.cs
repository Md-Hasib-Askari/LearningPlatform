public interface IModuleService
{
    Task<ModuleDto> CreateModuleAsync(CreateModuleDto createModuleDto, CancellationToken cancellationToken);
    Task<ModuleDto> GetModuleByIdAsync(Guid moduleId, CancellationToken cancellationToken);
    Task<IEnumerable<ModuleDto>> GetModulesByCourseIdAsync(Guid courseId, CancellationToken cancellationToken);
    Task<ModuleDto> UpdateModuleAsync(Guid moduleId, UpdateModuleDto updateModuleDto, CancellationToken cancellationToken);
    Task DeleteModuleAsync(Guid moduleId, CancellationToken cancellationToken);
}