using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Models;

namespace MyVaccine.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VaccineController : ControllerBase
{
    private readonly MyVaccineAppDbContext _context;

    public VaccineController(MyVaccineAppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var vaccines = await _context.Vaccines
            .Include(v => v.Categories)
            .ToListAsync();
        return Ok(vaccines);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var vaccine = await _context.Vaccines
            .Include(v => v.Categories)
            .FirstOrDefaultAsync(v => v.Id == id);
        if (vaccine == null) return NotFound();
        return Ok(vaccine);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Vaccine vaccine)
    {
        _context.Vaccines.Add(vaccine);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = vaccine.Id }, vaccine);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Vaccine vaccine)
    {
        if (id != vaccine.Id) return BadRequest();
        vaccine.UpdatedAt = DateTime.UtcNow;
        _context.Entry(vaccine).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var vaccine = await _context.Vaccines.FindAsync(id);
        if (vaccine == null) return NotFound();
        _context.Vaccines.Remove(vaccine);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}