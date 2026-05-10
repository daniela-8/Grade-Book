using Microsoft.AspNetCore.Mvc;
using Siemens.Internship2026.GradeBook.Interfaces;

namespace Siemens.Internship2026.GradeBook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradeController : ControllerBase
{
    private readonly IGradeService _gradeService;

    public GradeController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var grades = await _gradeService.GetAllAsync();
        return Ok(grades);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("Id must be a positive integer.");

        var grade = await _gradeService.GetByIdAsync(id);
        if (grade == null)
            return NotFound($"Grade with Id {id} was not found.");

        return Ok(grade);
    }

    [HttpGet("top-passing")]
    public async Task<IActionResult> GetTopPassing([FromQuery] int count)
    {
        if (count <= 0)
            return BadRequest("Count must be a positive integer.");

        var grades = await _gradeService.GetTopPassingGradesAsync(count);
        return Ok(grades);
    }
}
