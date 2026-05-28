using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Models;

namespace MyVaccine.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DependentController : ControllerBase
{
    private readonly MyVaccineAppDbContext _context;

    public DependentController(MyVaccineAppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var dependents = await _context.Dependents
            .Include(d => d.FamilyGroup)
            .ToListAsync();
        return Ok(dependents);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dependent = await _context.Dependents
            .Include(d => d.FamilyGroup)
            .FirstOrDefaultAsync(d => d.Id == id);
        if (dependent == null) return NotFound();
        return Ok(dependent);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Dependent dependent)
    {
        _context.Dependents.Add(dependent);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = dependent.Id }, dependent);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Dependent dependent)
    {
        if (id != dependent.Id) return BadRequest();
        dependent.UpdatedAt = DateTime.UtcNow;
        _context.Entry(dependent).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var dependent = await _context.Dependents.FindAsync(id);
        if (dependent == null) return NotFound();
        _context.Dependents.Remove(dependent);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}