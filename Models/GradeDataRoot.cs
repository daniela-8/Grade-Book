using System.Text.Json.Serialization;

namespace Siemens.Internship2026.GradeBook.Models;

public class GradeDataRoot
{
    [JsonPropertyName("items")]
    public List<Grade> Items { get; set; } = new();
}
