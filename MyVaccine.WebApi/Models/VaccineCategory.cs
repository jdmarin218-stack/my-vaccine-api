namespace MyVaccine.WebApi.Models;

public class VaccineCategory : BaseTable
{
    public string Name { get; set; } = string.Empty;
    public ICollection<Vaccine> Vaccines { get; set; } = new List<Vaccine>();
}