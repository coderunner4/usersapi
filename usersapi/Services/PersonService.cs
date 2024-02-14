using usersapi.DTOs;
using usersapi.Models;
using usersapi.Repository;

namespace usersapi.Services;

public interface IPersonService
{
    IEnumerable<PersonDTO> GetAllPersons();
    PersonDTO? GetPersonById(int id);
    PersonDTO? GetPersonByEmail(string email);
    int? AddPerson(PersonDTO person);
    bool? UpdatePerson(PersonDTO person);
    bool? RemovePerson(int id);
}

public class PersonService : IPersonService
{
    private IPersonRepository _personRepository;
    public PersonService(IPersonRepository personsRepository)
    {
        _personRepository = personsRepository;
    }

    public IEnumerable<PersonDTO> GetAllPersons()
    {
        return _personRepository.GetAll().Select(y => PersonDTO(y));
    }

    public PersonDTO? GetPersonById(int id)
    {
        var entity = _personRepository.GetById(id);
        return entity != null ? PersonDTO(entity) : null;
    }

    public PersonDTO? GetPersonByEmail(string email)
    {
        var entity = _personRepository.Find(y => y.Email == email).FirstOrDefault();
        return entity != null ? PersonDTO(entity) : null;
    }

    public int? AddPerson(PersonDTO person)
    {
        var newPerson = PersonEntity(person);
        _personRepository.Add(newPerson);
        _personRepository.Save();

        var personEntity = _personRepository.GetById(newPerson.Id);
        return personEntity != null ? personEntity.Id : null;
    }

    public bool? UpdatePerson(PersonDTO person)
    {
        var upPersonEntity = _personRepository.Find(y => y.Id == person.Id).FirstOrDefault();
        if (upPersonEntity == null)
        {
            return null;
        }

        upPersonEntity.Name = person.Name;
        upPersonEntity.Email = person.Email;
        upPersonEntity.BirthDate = person.BirthDate;

        _personRepository.Update(upPersonEntity);
        _personRepository.Save();

        // in case of exception cases, an exception will be thrown and return will not be true
        // that is alternative path not explicitly not capture here, so it can hanlded outside as side concerns
        return true;
    }

    public bool? RemovePerson(int id)
    {
        _personRepository.Remove(id);
        _personRepository.Save();

        return true;
    }

    private static PersonDTO PersonDTO(Person perEntity) =>
       new PersonDTO
       {
           Id = perEntity.Id,
           BirthDate = perEntity.BirthDate,
           Name = perEntity.Name,
           Email = perEntity.Email
       };

    private static Person PersonEntity(PersonDTO personItem) =>
       new Person
       {
           Id = personItem.Id,
           BirthDate = personItem.BirthDate,
           Name = personItem.Name,
           Email = personItem.Email
       };
}