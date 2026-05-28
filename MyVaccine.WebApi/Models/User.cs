namespace MyVaccine.WebApi.Models;

public class User : BaseTable
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ICollection<FamilyGroup> FamilyGroups { get; set; } = new List<FamilyGroup>();
    public ICollection<UsersAllergy> UsersAllergies { get; set; } = new List<UsersAllergy>();
}
