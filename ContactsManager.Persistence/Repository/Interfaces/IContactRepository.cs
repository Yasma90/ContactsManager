using ContactsManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Persistence.Repository.Interfaces
{
    public interface IContactRepository: IGenericRepository<Contact>
    {
    }
}
