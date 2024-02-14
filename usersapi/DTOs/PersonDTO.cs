using System.ComponentModel.DataAnnotations;

namespace usersapi.DTOs;

public class PersonDTO
{
    public int Id { get; set; }
    public string Name{ get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public DateTime BirthDate { get; set; }
}
