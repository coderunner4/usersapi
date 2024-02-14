using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using usersapi.DTOs;
using usersapi.Services;

namespace usersapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpGet("{id:int}")]
    public ActionResult<PersonDTO> GetPerson(int id)
    {
        var personItem = _personService.GetPersonById(id);
        if (personItem == null)
        {
            return NotFound();
        }
        
        // TODO: Return ServerResponse class Object, in case of error use ErrorMessage
        return personItem;
    }

    [HttpGet("check-email")]
    public IActionResult CheckEmailExists(string email)
    {
        // TODO: Check email format
        // TODO: SOLID Voilation Fix - Use Model Validator instead of hard checks here
        var person = _personService.GetPersonByEmail(email);
        return Ok(new { Exists = person != null });
    }

    [HttpGet]
    public ActionResult<IEnumerable<PersonDTO>> GetPersons()
    {
        return _personService.GetAllPersons().ToList();
    }

    [HttpPut("{id:int}")]
    public IActionResult PutPerson(int id, PersonDTO personDTO)
    {
        // TODO: Check email format
        // TODO: SOLID Voilation Fix - Use Model Validator instead of hard checks here
        // Commenting server-side checks for now
        /**
        if (personDTO == null
            || id != personDTO.Id        
            || string.IsNullOrWhiteSpace(personDTO.FirstName)
            || string.IsNullOrWhiteSpace(personDTO.LastName)
            || string.IsNullOrWhiteSpace(personDTO.Email)
            || personDTO.DepartmentId <= 0)
        {
            return BadRequest("InValid Information Provided");
        }
        **/

        if (id <=0 )        
        {
            return BadRequest("InValid Information Provided");
        }

        personDTO.Id = id;
        var personItem = _personService.GetPersonById(personDTO.Id);
        if (personItem == null)
        {
            return NotFound();
        }

        personItem.Name = personDTO.Name;
        personItem.Email = personDTO.Email;
        personItem.BirthDate = personDTO.BirthDate;

        try
        {
            _personService.UpdatePerson(personItem);
        }
        catch (DbUpdateConcurrencyException) when (!PersonItemExists(personDTO.Id))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost]
    public ActionResult<PersonDTO> PostPerson(PersonDTO personDTO)
    {
        // TODO: Check email format
        // TODO: SOLID Voilation Fix - Use Model Validator instead of hard checks here
        // Commenting server-side checks for now
        /*
        if (personDTO == null
            || string.IsNullOrWhiteSpace(personDTO.FirstName)
            || string.IsNullOrWhiteSpace(personDTO.LastName)
            || string.IsNullOrWhiteSpace(personDTO.Email)
            || personDTO.DepartmentId <= 0)
        {
            return BadRequest("InValid Information Provided");
        }*/

        var resId = _personService.AddPerson(personDTO);
        if(resId == null){
            return NotFound();
        }
        
        // return newly added user id back
        personDTO.Id = (int) resId;

        return CreatedAtAction(
            nameof(GetPerson),
            new { id = personDTO.Id },
            personDTO);
    }

    private bool PersonItemExists(int id)
    {
        return _personService.GetPersonById(id) != null;
    }
}