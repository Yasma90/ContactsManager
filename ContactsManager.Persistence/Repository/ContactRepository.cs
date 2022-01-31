using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ContactsManager.Domain.Models;
using ContactsManager.Persistence.ContextDb;
using ContactsManager.Persistence.Repository.Interfaces;

namespace ContactsManager.Persistence.Repository
{
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        public ContactRepository(ContactsDbContext context) : base(context)
        {
        }

    }
}
