namespace MyVaccine.WebApi.Models;

public class Dependent : BaseTable
{
    public string Name { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
    public int FamilyGroupId { get; set; }
    public FamilyGroup FamilyGroup { get; set; } = null!;
    public ICollection<VaccineRecord> VaccineRecords { get; set; } = new List<VaccineRecord>();
}