using Siemens.Internship2026.GradeBook.Interfaces;
using Siemens.Internship2026.GradeBook.Models;

namespace Siemens.Internship2026.GradeBook.Repositories;

public class GradeRepository : IGradeRepository
{
    private readonly List<Grade> _grades = new()
    {
        new Grade { Id = 1, Value = 4.25m, IsActive = true },
        new Grade { Id = 2, Value = 7.50m, IsActive = true },
        new Grade { Id = 3, Value = 1.99m, IsActive = false },
        new Grade { Id = 4, Value = 9.75m, IsActive = true },
        new Grade { Id = 5, Value = 3.00m, IsActive = false },
        new Grade { Id = 6, Value = 6.49m, IsActive = true },
        new Grade { Id = 7, Value = 10.00m, IsActive = true },
        new Grade { Id = 8, Value = 2.30m, IsActive = false },
        new Grade { Id = 9, Value = 8.10m, IsActive = true },
        new Grade { Id = 10, Value = 5.00m, IsActive = true }
    };

    public Task<Grade?> GetByIdAsync(int id)
    {
        var grade = _grades.FirstOrDefault(g => g.Id == id);
        return Task.FromResult(grade);
    }

    public Task<IEnumerable<Grade>> GetAllAsync()
    {
        return Task.FromResult(_grades.AsEnumerable());
    }
}
