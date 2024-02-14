using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using usersapi.Models;

namespace usersapi.Repository;

public interface IPersonRepository : IWriteRepository<Person>, IReadRepository<Person>
{
}

public class PersonRepository : IPersonRepository
{
    private protected readonly PersonManagerContext dbContext;

    public PersonRepository(PersonManagerContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Person? GetById(int id)
    {
        return dbContext.People.FirstOrDefault(e => e.Id == id);
    }

    public IEnumerable<Person> GetAll()
    {
        return dbContext.People;
    }

    public IEnumerable<Person> Find(Expression<Func<Person, bool>> expression)
    {
        return dbContext.People.Where(expression);
    }

    public void Add(Person entity)
    {
        dbContext.People.Add(entity);
    }

    public void Remove(int id)
    {
        var entity = dbContext.People.Find(id);
        dbContext.People.Remove(entity);
    }

    public void Update(Person entity)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
    }

    public void Save()
    {
        dbContext.SaveChanges();
    }
}