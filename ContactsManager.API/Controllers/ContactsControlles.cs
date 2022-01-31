using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactsManager.Domain.Models;
using ContactsManager.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;

namespace ContactsManager.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ContactsController> _logger;
        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        //static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        public ContactsController(IUnitOfWork unitOfWork, ILogger<ContactsController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacs()
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            return await _unitOfWork.ContactRepository.GetAllAsync();
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(Guid id)
        {
            var contact = await _unitOfWork.ContactRepository.GetbyIdAsync(id);

            if (contact == null)
            {
                _logger.LogDebug("Contact don't found.");
                return NotFound();
            }

            return contact;
        }

        // PUT: api/Contacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(Guid id, Contact contact)
        {
            if (!ModelState.IsValid || id != contact.Id)
            {
                return BadRequest();
            }

            _unitOfWork.ContactRepository.Update(contact);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_unitOfWork.ContactRepository.Exist(id))
                {
                    _logger.LogError($"Update function error: Contact don't exist.");
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"Update function error: {ex.Message}");
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Contacts
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
            if (_unitOfWork.ContactRepository.ExistEmail(contact.Email))
            {
                _logger.LogError("Exist the email.");
                throw new Exception("Exist the email.");
            }
            if (_unitOfWork.ContactRepository.Younger18(contact.DateOfBirth))
            {
                _logger.LogError("Contact have less of 18 years old.");
                throw new Exception("Contact have less of 18 years old.");
            }
            if (!ModelState.IsValid || contact.DateOfBirth >= DateTime.Now)
            {
                return BadRequest();
            }
            await _unitOfWork.ContactRepository.AddAsync(contact);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("GetContact", new { id = contact.Id }, contact);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            var contact = await _unitOfWork.ContactRepository.DeleteAsync(id);
            if (contact == null)
            {
                _logger.LogDebug("Contact don't found.");
                return NotFound();
            }

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

    }
}
