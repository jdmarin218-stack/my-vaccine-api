using System.ComponentModel.DataAnnotations.Schema;

namespace MyVaccine.WebApi.Models;

public class VaccineRecord : BaseTable
{
    public DateTime AdministeredDate { get; set; }
    public string AdministeredLocation { get; set; } = string.Empty;
    public string AdministeredBy { get; set; } = string.Empty;
    public int DependentId { get; set; }
    [ForeignKey("DependentId")]
    public Dependent? Dependent { get; set; }
    public int VaccineId { get; set; }
    [ForeignKey("VaccineId")]
    public Vaccine? Vaccine { get; set; }
}