using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactsManager.Domain.Models;
using ContactsManager.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ContactsManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ContactsController> _logger;

        public ContactsController(IUnitOfWork unitOfWork, ILogger<ContactsController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacs()
        {
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
                if (!Exist(id))
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
            if (ExistEmail(contact.Email))
            {
                _logger.LogError("Exist the email.");
                throw new Exception("Exist the email.");
            }
            if (Younger18(contact.DateOfBirth))
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

        #region Help Methods

        private bool Exist(Guid id) => _unitOfWork.ContactRepository.GetbyIdAsync(id).Result != null;

        private bool ExistEmail(string email) => _unitOfWork.ContactRepository.GetAsync(c => c.Email == email).Result
                                                    .FirstOrDefault() != null;

        private bool Younger18(DateTime date) => (DateTime.Now - date).Days / 365 < 18;

        #endregion
    }
}
