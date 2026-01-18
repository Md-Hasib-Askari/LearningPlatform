using AutoMapper;

public class ModuleService : IModuleService
{
    private readonly IModuleRepository _moduleRepo;
    private readonly IMapper _mapper;
    public ModuleService(IModuleRepository moduleRepo, IMapper mapper)
    {
        _moduleRepo = moduleRepo;
        _mapper = mapper;
    }

    public async Task<ModuleDto> CreateModuleAsync(Guid courseId, CreateModuleDto createModuleDto, CancellationToken cancellationToken)
    {
        var module = new Module();
        module.Create(
            createModuleDto.Title,
            createModuleDto.Description,
            createModuleDto.OrderIndex,
            courseId
        );
        await _moduleRepo.AddAsync(module, cancellationToken);
        return _mapper.Map<ModuleDto>(module);
    }

    public async Task DeleteModuleAsync(Guid moduleId, CancellationToken cancellationToken)
    {
        var module = await _moduleRepo.GetByIdAsync(moduleId, cancellationToken);
        if (module != null)
        {
            await _moduleRepo.DeleteAsync(module, cancellationToken);
        }
    }

    public async Task<ModuleDto> GetModuleByIdAsync(Guid moduleId, CancellationToken cancellationToken)
    {
        var module = await _moduleRepo.GetByIdAsync(moduleId, cancellationToken);
        return _mapper.Map<ModuleDto>(module);
    }

    public async Task<IEnumerable<ModuleDto>> GetModulesByCourseIdAsync(Guid courseId, CancellationToken cancellationToken)
    {
        var modules = await _moduleRepo.GetModulesByCourseIdAsync(courseId, cancellationToken);
        return _mapper.Map<IEnumerable<ModuleDto>>(modules);
    }

    public async Task<ModuleDto> UpdateModuleAsync(Guid moduleId, UpdateModuleDto updateModuleDto, CancellationToken cancellationToken)
    {
        var module = await _moduleRepo.GetByIdAsync(moduleId, cancellationToken);
        if (module == null)
        {
            throw new KeyNotFoundException("Module not found");
        }

        _mapper.Map(updateModuleDto, module);
        await _moduleRepo.UpdateAsync(module, cancellationToken);
        return _mapper.Map<ModuleDto>(module);
    }
}