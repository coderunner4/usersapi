namespace usersapi.Models;

public class Person : IEntity
{
    public int Id { get; set; }
    public string Name{ get; set; }
    public string Email { get; set; }
    public DateTime BirthDate { get; set; }
}
