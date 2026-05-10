using Siemens.Internship2026.GradeBook.Interfaces;
using Siemens.Internship2026.GradeBook.Models;

namespace Siemens.Internship2026.GradeBook.Services;

public class GradeService : IGradeService
{
    private const decimal PassingThreshold = 5.0m;

    private readonly IGradeRepository _repository;

    public GradeService(IGradeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Grade?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Grade>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IEnumerable<Grade>> GetTopPassingGradesAsync(int count)
    {
        var all = await _repository.GetAllAsync();

        return all
            .Where(g => g.IsActive && g.Value >= PassingThreshold)
            .Take(count)
            .ToList();
    }
}
