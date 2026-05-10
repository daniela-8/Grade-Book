using System.Net.Http.Json;
using System.Text.Json;
using Siemens.Internship2026.GradeBook.Interfaces;
using Siemens.Internship2026.GradeBook.Models;

namespace Siemens.Internship2026.GradeBook.Repositories;

public class GradeRepository : IGradeRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;

    private const string DataUrl =
        "https://gist.githubusercontent.com/ArdeleanTudor/8ea407832cd9794960e0e6bbd1319f6e/raw/145b121103dd1cee3737a681c487f7295ac82e6b/gistfile1.txt";

    public GradeRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Grade?> GetByIdAsync(int id)
    {
        var grades = await FetchGradesAsync();
        return grades.FirstOrDefault(g => g.Id == id);
    }

    public async Task<IEnumerable<Grade>> GetAllAsync()
    {
        return await FetchGradesAsync();
    }

    private async Task<List<Grade>> FetchGradesAsync()
    {
        var response = await _httpClient.GetStringAsync(DataUrl);
        var root = JsonSerializer.Deserialize<GradeDataRoot>(response, JsonOptions);
        return root?.Items ?? new List<Grade>();
    }
}
