using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Models;

namespace MyVaccine.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VaccineRecordController : ControllerBase
{
    private readonly MyVaccineAppDbContext _context;

    public VaccineRecordController(MyVaccineAppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var records = await _context.VaccineRecords
            .Include(vr => vr.Dependent)
            .Include(vr => vr.Vaccine)
            .ToListAsync();
        return Ok(records);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var record = await _context.VaccineRecords
            .Include(vr => vr.Dependent)
            .Include(vr => vr.Vaccine)
            .FirstOrDefaultAsync(vr => vr.Id == id);
        if (record == null) return NotFound();
        return Ok(record);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VaccineRecord record)
    {
        _context.VaccineRecords.Add(record);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] VaccineRecord record)
    {
        if (id != record.Id) return BadRequest();
        record.UpdatedAt = DateTime.UtcNow;
        _context.Entry(record).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var record = await _context.VaccineRecords.FindAsync(id);
        if (record == null) return NotFound();
        _context.VaccineRecords.Remove(record);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}