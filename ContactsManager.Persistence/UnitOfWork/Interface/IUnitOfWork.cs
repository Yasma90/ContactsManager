using ContactsManager.Persistence.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IContactRepository ContactRepository { get; set; }
        Task<int> SaveChangesAsync();
    }
}
