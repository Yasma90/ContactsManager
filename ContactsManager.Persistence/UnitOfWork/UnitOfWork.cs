using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ContactsManager.Persistence.ContextDb;
using ContactsManager.Persistence.Repository;
using ContactsManager.Persistence.Repository.Interfaces;

namespace ContactsManager.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ContactsDbContext _context;
        public IContactRepository ContactRepository { get; set; }
        private bool disposedValue;

        public UnitOfWork(ContactsDbContext context, IContactRepository contactRepository)
        {
            _context = context;
            ContactRepository = contactRepository;
        }
        
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        #region Disponsable

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~UnitOfWork()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
