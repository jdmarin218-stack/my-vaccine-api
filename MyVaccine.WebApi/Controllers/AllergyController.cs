using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Models;

namespace MyVaccine.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AllergyController : ControllerBase
{
    private readonly MyVaccineAppDbContext _context;

    public AllergyController(MyVaccineAppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var allergies = await _context.Allergies.ToListAsync();
        return Ok(allergies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var allergy = await _context.Allergies.FindAsync(id);
        if (allergy == null) return NotFound();
        return Ok(allergy);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Allergy allergy)
    {
        _context.Allergies.Add(allergy);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = allergy.Id }, allergy);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Allergy allergy)
    {
        if (id != allergy.Id) return BadRequest();
        allergy.UpdatedAt = DateTime.UtcNow;
        _context.Entry(allergy).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var allergy = await _context.Allergies.FindAsync(id);
        if (allergy == null) return NotFound();
        _context.Allergies.Remove(allergy);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}