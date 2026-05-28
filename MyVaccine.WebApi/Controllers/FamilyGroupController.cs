using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Models;

namespace MyVaccine.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FamilyGroupController : ControllerBase
{
    private readonly MyVaccineAppDbContext _context;

    public FamilyGroupController(MyVaccineAppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var familyGroups = await _context.FamilyGroups
            .Include(fg => fg.Dependents)
            .ToListAsync();
        return Ok(familyGroups);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var familyGroup = await _context.FamilyGroups
            .Include(fg => fg.Dependents)
            .FirstOrDefaultAsync(fg => fg.Id == id);
        if (familyGroup == null) return NotFound();
        return Ok(familyGroup);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FamilyGroup familyGroup)
    {
        _context.FamilyGroups.Add(familyGroup);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = familyGroup.Id }, familyGroup);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] FamilyGroup familyGroup)
    {
        if (id != familyGroup.Id) return BadRequest();
        familyGroup.UpdatedAt = DateTime.UtcNow;
        _context.Entry(familyGroup).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var familyGroup = await _context.FamilyGroups.FindAsync(id);
        if (familyGroup == null) return NotFound();
        _context.FamilyGroups.Remove(familyGroup);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}