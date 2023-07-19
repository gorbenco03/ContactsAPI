using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactsController : Controller
    {
        private readonly ContactsAPIDbContext dbContext;

        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult>  GetContacts()
        {
             return  Ok(await dbContext.Contacts.ToListAsync());

        }


        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetSingleContact([FromRoute] Guid id)
        {
            var contct = await dbContext.Contacts.FindAsync(id);

            if (contct == null) { return NotFound(); }
            else
            {
                return Ok(contct);
            }
           
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteContat([FromRoute]  Guid id)
        {
            var contct = await dbContext.Contacts.FindAsync(id);

            if (contct == null) { return NotFound(); }
            else
            {
                dbContext.Remove(contct);

                dbContext.SaveChanges();
                return Ok(contct);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Adress = addContactRequest.Adress,
                Email = addContactRequest.Email,
                FullName = addContactRequest.FullName,
                PhoneNumber = addContactRequest.PhoneNumber,
            };

            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return Ok(contact);

        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContatRequest)
        {

            var contct = await  dbContext.Contacts.FindAsync(id);

            if (contct != null)
            {
                contct.FullName = updateContatRequest.FullName;
                contct.Adress = updateContatRequest.Adress;
                contct.PhoneNumber = updateContatRequest.PhoneNumber;
                contct.Email = updateContatRequest.Email;
                await dbContext.SaveChangesAsync();

                return Ok(contct); 
            }
            else return NotFound();
        }
    }
}
