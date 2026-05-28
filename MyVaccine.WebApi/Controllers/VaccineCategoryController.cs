using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Models;

namespace MyVaccine.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VaccineCategoryController : ControllerBase
{
    private readonly MyVaccineAppDbContext _context;

    public VaccineCategoryController(MyVaccineAppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _context.VaccineCategories.ToListAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _context.VaccineCategories.FindAsync(id);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VaccineCategory category)
    {
        _context.VaccineCategories.Add(category);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] VaccineCategory category)
    {
        if (id != category.Id) return BadRequest();
        category.UpdatedAt = DateTime.UtcNow;
        _context.Entry(category).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.VaccineCategories.FindAsync(id);
        if (category == null) return NotFound();
        _context.VaccineCategories.Remove(category);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}