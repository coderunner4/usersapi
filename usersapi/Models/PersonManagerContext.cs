using Microsoft.EntityFrameworkCore;

namespace usersapi.Models;

public class PersonManagerContext : DbContext
{
    public PersonManagerContext(DbContextOptions<PersonManagerContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>().HasData(
            new Person {Id = 1, Name = "John Deo" , Email= "john.deo@abc.com", BirthDate = new DateTime(1970,1,1) }
        );
    }    

    public DbSet<Person> People { get; set; }
}