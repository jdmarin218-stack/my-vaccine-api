namespace MyVaccine.WebApi.Models;

public class Vaccine : BaseTable
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<VaccineCategory> Categories { get; set; } = new List<VaccineCategory>();
    public ICollection<VaccineRecord> VaccineRecords { get; set; } = new List<VaccineRecord>();
}