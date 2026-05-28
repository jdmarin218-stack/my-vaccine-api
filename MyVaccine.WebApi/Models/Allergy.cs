namespace MyVaccine.WebApi.Models;

public class Allergy : BaseTable
{
    public string Name { get; set; } = string.Empty;
    public ICollection<UsersAllergy> UsersAllergies { get; set; } = new List<UsersAllergy>();
}